using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using log4net;
using NetGore;
using NetGore.Db;
using NetGore.IO;
using NetGore.Network;
using NetGore.Scripting;

// TODO: When an item stops moving, send the position again to ensure it is valid

namespace DemoGame.Server
{
    /// <summary>
    /// The core component of the game server.
    /// </summary>
    public class Server : IDisposable, IGetTime
    {
        /// <summary>
        /// The minimum number of milliseconds a connection should be inactive to be considered dead.
        /// </summary>
        const int _inactiveConnectionTimeOut = 5000;

        /// <summary>
        /// The number of milliseconds to wait before checking for inactive connections to remove.
        /// </summary>
        const int _removeInactiveConnectionsRate = 60000;

        /// <summary>
        /// Millisecond rate at which the server updates. The server update rate does not affect the rate
        /// at which physics is update, so modifying the update rate will not affect the game
        /// speed. Server update rate is used to determine how frequently the server checks
        /// for performing updates and how long it is able to "sleep". It is recommended
        /// a high update rate is used to allow for more precise updating.
        /// </summary>
        const long _serverUpdateRate = 5; // 200 FPS

        readonly ConsoleCommands _consoleCommands;
        ConsoleInputBuffer _consoleInputBuffer;

        readonly IDbController _dbController;
        bool _disposed;

        /// <summary>
        /// Stopwatch to track the total elapsed time the game has been running
        /// </summary>
        readonly Stopwatch _gameTimer = new Stopwatch();

        /// <summary>
        /// If the server is running
        /// </summary>
        bool _isRunning = true;

        /// <summary>
        /// Lock used to ensure that only one account is logging in at a time. The main intention of this is to prevent
        /// a race condition allowing an account to log in twice from two places at once.
        /// </summary>
        readonly object _loginLock = new object();

        readonly List<string> _motd = new List<string>();
        IServerSettingTable _serverSettings;

        readonly ServerSockets _sockets;

        readonly int _startupTime = Environment.TickCount;

        /// <summary>
        /// World managed by the server
        /// </summary>
        readonly World _world;

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="Server"/> class.
        /// </summary>
        public Server()
        {
            DbConnectionSettings settings = new DbConnectionSettings();
            _dbController = new DbController(settings.SqlConnectionString());
            DbTableValidator.ValidateTables(_dbController);

            ValidateDbControllerQueryAttributes();

            // Load the game data and such
            InitializeScripts();

            // Update the GameData table
            IGameConstantTable gameDataValues = GetGameConstantTableValues();
            DbController.GetQuery<UpdateGameConstantTableQuery>().Execute(gameDataValues);
            if (log.IsInfoEnabled)
                log.Info("Updated the GameData table with the current values.");

            // Load the server settings
            LoadSettings();

            // Create some objects
            _world = new World(this);
            _sockets = new ServerSockets(this);
            _consoleCommands = new ConsoleCommands(this);

            // Clean-up
            new ServerRuntimeCleaner(this);

            if (log.IsInfoEnabled)
                log.Info("Server loaded.");
        }

        /// <summary>
        /// Gets the DbController used to communicate with the database by this server.
        /// </summary>
        public IDbController DbController
        {
            get { return _dbController; }
        }

        /// <summary>
        /// Gets if this <see cref="Server"/> has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _disposed; }
        }

        public string MOTD
        {
            get { return _serverSettings.Motd; }
        }

        /// <summary>
        /// Gets the <see cref="ServerSockets"/> used to manage connections to the server.
        /// </summary>
        public ServerSockets ServerSockets
        {
            get { return _sockets; }
        }

        /// <summary>
        /// Gets the time that the server started.
        /// </summary>
        public int StartupTime
        {
            get { return _startupTime; }
        }

        /// <summary>
        /// Gets the World that this Server controls.
        /// </summary>
        public World World
        {
            get { return _world; }
        }

        /// <summary>
        /// Creates a <see cref="ScriptTypeCollection"/> with the specified name.
        /// </summary>
        /// <param name="name">Name of the <see cref="ScriptTypeCollection"/>.</param>
        static void CreateScriptTypeCollection(string name)
        {
            if (log.IsInfoEnabled)
                log.InfoFormat("Loading scripts `{0}`.", name);

            string path = ContentPaths.Build.Data.Join("ServerScripts").Join(name);
            ScriptTypeCollection scriptTypes = new ScriptTypeCollection(name, path);

            // Display warnings
            if (log.IsWarnEnabled)
            {
                foreach (CompilerError warning in scriptTypes.CompilerErrors.Where(x => x.IsWarning))
                {
                    log.Warn(warning);
                }
            }

            // Display errors
            if (log.IsErrorEnabled)
            {
                foreach (CompilerError error in scriptTypes.CompilerErrors.Where(x => !x.IsWarning))
                {
                    log.Error(error);
                }
            }

            // Check if the compilation failed
            if (scriptTypes.CompilationFailed && log.IsFatalEnabled)
                log.FatalFormat("Failed to compile scripts for `{0}`!", name);
        }

        /// <summary>
        /// Main game loop for the server.
        /// </summary>
        void GameLoop()
        {
            ThreadAsserts.IsMainThread();

            UpdateServerTimeQuery updateServerTimeQuery = DbController.GetQuery<UpdateServerTimeQuery>();
            ServerTimeUpdater serverTimeUpdater = new ServerTimeUpdater(updateServerTimeQuery);

            long lastRemoveConnsTime = 0;

            _gameTimer.Reset();
            _gameTimer.Start();

            if (log.IsInfoEnabled)
                log.Info("Server started. Type 'help' for a list of server console commands.");

            while (_isRunning)
            {
                // Store the loop start time so we can calculate how long the loop took
                long loopStartTime = _gameTimer.ElapsedMilliseconds;

                // Check to remove inactive connections
                if (_gameTimer.ElapsedMilliseconds - lastRemoveConnsTime > _removeInactiveConnectionsRate)
                {
                    lastRemoveConnsTime = _gameTimer.ElapsedMilliseconds;
                    ServerSockets.RemoveInactiveConnections(_inactiveConnectionTimeOut);
                }

                // Update the networking
                ServerSockets.Heartbeat();

                // Handle input from the console
                foreach (var inputStr in _consoleInputBuffer.GetBuffer())
                {
                    var resultStr = _consoleCommands.ExecuteCommand(inputStr);
                    if (!string.IsNullOrEmpty(resultStr))
                        Console.WriteLine(resultStr);
                }

                // Update the world
                _world.Update();

                // Update the time
                serverTimeUpdater.Update(GetTime());

                // Check if we can afford sleeping the thread
                long sleepTime = _serverUpdateRate - (_gameTimer.ElapsedMilliseconds - loopStartTime);
                if (sleepTime > 0)
                    Thread.Sleep((int)sleepTime);
            }

            _gameTimer.Stop();
        }

        /// <summary>
        /// Creates an <see cref="IGameConstantTable"/> with the current <see cref="GameData"/> values.
        /// </summary>
        /// <returns>An <see cref="IGameConstantTable"/> with the current <see cref="GameData"/> values.</returns>
        static IGameConstantTable GetGameConstantTableValues()
        {
            GameConstantTable gdt = new GameConstantTable
            {
                MaxAccountNameLength = (byte)GameData.AccountName.MaxLength,
                MaxAccountPasswordLength = (byte)GameData.AccountPassword.MaxLength,
                MaxCharacterNameLength = (byte)GameData.CharacterName.MaxLength,
                MaxCharactersPerAccount = GameData.MaxCharactersPerAccount,
                MaxStatusEffectPower = GameData.MaxStatusEffectPower,
                MinAccountNameLength = (byte)GameData.AccountName.MinLength,
                MinAccountPasswordLength = (byte)GameData.AccountPassword.MinLength,
                MinCharacterNameLength = (byte)GameData.CharacterName.MinLength,
                ScreenHeight = (ushort)GameData.ScreenSize.Y,
                ScreenWidth = (ushort)GameData.ScreenSize.X,
                ServerIp = GameData.ServerIP,
                ServerPingPort = (ushort)GameData.ServerPingPort,
                ServerTcpPort = (ushort)GameData.ServerTCPPort,
                WorldPhysicsUpdateRate = (ushort)GameData.WorldPhysicsUpdateRate
            };

            return gdt;
        }

        static void HandleFailedLogin(IIPSocket conn, AccountLoginResult loginResult, string name)
        {
            // Get the error message
            GameMessage loginFailureGameMessage;
            switch (loginResult)
            {
                case AccountLoginResult.AccountInUse:
                    if (log.IsInfoEnabled)
                        log.InfoFormat("Login for account `{0}` failed: Account in use.", name);
                    loginFailureGameMessage = GameMessage.LoginAccountInUse;
                    break;

                case AccountLoginResult.InvalidName:
                    if (log.IsInfoEnabled)
                        log.InfoFormat("Login for account `{0}` failed: Invalid name.", name);
                    loginFailureGameMessage = GameMessage.LoginInvalidName;
                    break;

                case AccountLoginResult.InvalidPassword:
                    if (log.IsInfoEnabled)
                        log.InfoFormat("Login for account `{0}` failed: Incorrect password.", name);
                    loginFailureGameMessage = GameMessage.LoginInvalidPassword;
                    break;

                default:
                    // If the value is undefined, just say its an invalid name
                    const string errmsg = "Invalid AccountLoginResult value `{0}`.";
                    Debug.Fail(string.Format(errmsg, loginResult));
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, loginResult);
                    loginFailureGameMessage = GameMessage.LoginInvalidName;
                    break;
            }

            using (PacketWriter pw = ServerPacket.LoginUnsuccessful(loginFailureGameMessage))
            {
                conn.Send(pw);
            }
        }

        /// <summary>
        /// Initializes the scripts.
        /// </summary>
        static void InitializeScripts()
        {
            CreateScriptTypeCollection("AI");
        }

        /// <summary>
        /// Loads the server settings from the database and sets up any settings that need it.
        /// </summary>
        void LoadSettings()
        {
            if (log.IsInfoEnabled)
                log.Info("Loading server settings.");

            _serverSettings = DbController.GetQuery<SelectServerSettingsQuery>().Execute();

            // Create the MOTD list
            var motdLines = _serverSettings.Motd.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            if (motdLines.Length > 0)
                _motd.AddRange(motdLines);
            _motd.TrimExcess();
        }

        /// <summary>
        /// Handles the login attempt of an account.
        /// </summary>
        /// <param name="conn">Connection that the login request was made on.</param>
        /// <param name="name">Name of the account.</param>
        /// <param name="password">Entered password for this account.</param>
        public void LoginAccount(IIPSocket conn, string name, string password)
        {
            if (conn == null)
            {
                if (log.IsErrorEnabled)
                    log.Error("conn is null.");
                return;
            }

            // Try to log in the account
            UserAccount userAccount;
            AccountLoginResult loginResult;

            lock (_loginLock)
            {
                loginResult = UserAccount.Login(DbController, conn, name, password, out userAccount);
            }

            // Check that the login was successful
            if (loginResult != AccountLoginResult.Successful)
            {
                HandleFailedLogin(conn, loginResult, name);
                return;
            }

            // Set the connection's tag to the account
            conn.Tag = userAccount;

            // Send the "Login Successful" message
            using (PacketWriter pw = ServerPacket.LoginSuccessful())
            {
                conn.Send(pw);
            }

            if (log.IsInfoEnabled)
                log.InfoFormat("Login for account `{0}` successful.", name);

            // Send the account characters
            userAccount.SendAccountCharacterInfos();
        }

        /// <summary>
        /// Shuts down the <see cref="Server"/>.
        /// </summary>
        public void Shutdown()
        {
            _isRunning = false;
        }

        /// <summary>
        /// Starts the <see cref="Server"/>.
        /// </summary>
        public void Start()
        {
            if (log.IsInfoEnabled)
                log.Info("Starting server...");

            // Create the console input buffer
            if (_consoleInputBuffer != null)
                _consoleInputBuffer.Dispose();
            _consoleInputBuffer = new ConsoleInputBuffer();

            // Start the main game loop
            GameLoop();
        }

        /// <summary>
        /// Ensures we have added the <see cref="DbControllerQueryAttribute"/> where needed.
        /// </summary>
        static void ValidateDbControllerQueryAttributes()
        {
            const string errmsg = "Type `{0}` fails to implement attribute `{1}`. Ensure this is okay.";
            var attribType = typeof(DbControllerQueryAttribute);

            new DbControllerQueryAttributeChecker(delegate(DbControllerQueryAttributeChecker sender, Type type)
                                                  {
                                                      Debug.Fail(string.Format(errmsg, type, attribType));
                                                      if (log.IsErrorEnabled)
                                                          log.ErrorFormat(errmsg, type, attribType);
                                                  });
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes of the <see cref="Server"/>.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
                return;

            _disposed = true;

            if (_consoleInputBuffer != null)
                _consoleInputBuffer.Dispose();

            _world.Dispose();
            _dbController.Dispose();
        }

        #endregion

        #region IGetTime Members

        /// <summary>
        /// Gets the current time in milliseconds.
        /// </summary>
        /// <returns>The current time in milliseconds.</returns>
        public int GetTime()
        {
            return (int)_gameTimer.ElapsedMilliseconds;
        }

        #endregion
    }
}