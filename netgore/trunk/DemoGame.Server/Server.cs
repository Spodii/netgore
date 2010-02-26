using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using DemoGame.Server.Queries;
using log4net;
using NetGore;
using NetGore.Db;
using NetGore.Db.MySql;
using NetGore.Features.Groups;
using NetGore.Features.Shops;
using NetGore.Features.StatusEffects;
using NetGore.IO;
using NetGore.Network;
using NetGore.Scripting;

namespace DemoGame.Server
{
    public delegate void ServerConsoleCommandCallback(Server server, string command, string returnString);

    /// <summary>
    /// The core component of the game server.
    /// </summary>
    public class Server : IDisposable, IGetTime
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The minimum number of milliseconds a connection should be inactive to be considered dead.
        /// </summary>
        const int _inactiveConnectionTimeOut = 20000;

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

        readonly Queue<string> _consoleCommandQueue = new Queue<string>();
        readonly ConsoleCommands _consoleCommands;
        readonly object _consoleCommandSync = new object();
        readonly IDbController _dbController;
        readonly Stopwatch _gameTimer = new Stopwatch();
        readonly GroupManager _groupManager;
        readonly List<string> _motd = new List<string>();
        readonly ServerSockets _sockets;
        readonly int _startupTime = Environment.TickCount;
        readonly World _world;

        bool _disposed;
        bool _isRunning = true;
        IServerSettingTable _serverSettings;
        int _tick;

        /// <summary>
        /// Initializes a new instance of the <see cref="Server"/> class.
        /// </summary>
        public Server()
        {
            DbConnectionSettings settings = new DbConnectionSettings();
            _dbController = new ServerDbController(settings.GetMySqlConnectionString());
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
            _consoleCommands = new ConsoleCommands(this);
            _groupManager = new GroupManager((gm, x) => new Group(x));
            _world = new World(this);
            _sockets = new ServerSockets(this);

            // Clean-up
            new ServerRuntimeCleaner(this);

            if (log.IsInfoEnabled)
                log.Info("Server loaded.");
        }

        /// <summary>
        /// Notifies listeners when a console command has been executed.
        /// </summary>
        public event ServerConsoleCommandCallback ConsoleCommandExecuted;

        /// <summary>
        /// Gets the DbController used to communicate with the database by this server.
        /// </summary>
        public IDbController DbController
        {
            get { return _dbController; }
        }

        public IGroupManager GroupManager
        {
            get { return _groupManager; }
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
        /// Gets the current tick the server is currently on. Each tick represents one iteration of the main loop.
        /// </summary>
        public int Tick
        {
            get { return _tick; }
        }

        /// <summary>
        /// Gets the World that this Server controls.
        /// </summary>
        public World World
        {
            get { return _world; }
        }

        /// <summary>
        /// Handles the request to create a new account.
        /// </summary>
        /// <param name="conn">Connection that the request was made on.</param>
        /// <param name="name">Name of the account.</param>
        /// <param name="password">Entered password for this account.</param>
        /// <param name="email">The email address.</param>
        public void CreateAccount(IIPSocket conn, string name, string password, string email)
        {
            ThreadAsserts.IsMainThread();

            if (conn == null)
            {
                if (log.IsErrorEnabled)
                    log.Error("conn is null.");
                return;
            }

            // Create the account
            AccountID id;
            string errorMessage;
            bool success = UserAccount.TryCreateAccount(DbController, conn, name, password, email, out id, out errorMessage);

            // Send the appropriate success message
            using (var pw = ServerPacket.CreateAccount(success, errorMessage))
            {
                conn.Send(pw);
            }
        }

        /// <summary>
        /// Handles the request to create a new account.
        /// </summary>
        /// <param name="conn">Connection that the request was made on.</param>
        /// <param name="name">The name of the character to create.</param>
        public void CreateAccountCharacter(IIPSocket conn, string name)
        {
            ThreadAsserts.IsMainThread();

            var account = conn.Tag as UserAccount;
            if (account == null)
                return;

            string errorMessage;
            bool success = UserAccount.TryAddCharacter(DbController, account.Name, name, out errorMessage);

            using (var pw = ServerPacket.CreateAccountCharacter(success, errorMessage))
            {
                conn.Send(pw);
            }

            if (success)
            {
                account.LoadCharacterIDs();
                account.SendAccountCharacterInfos();
            }
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
        /// Enqueues a console command string to be executed. When the command is executed, the results will be returned
        /// through the <see cref="Server.ConsoleCommandExecuted"/> event.
        /// </summary>
        /// <param name="commandString">The command to be executed.</param>
        public void EnqueueConsoleCommand(string commandString)
        {
            lock (_consoleCommandSync)
            {
                _consoleCommandQueue.Enqueue(commandString);
            }
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

                // Update the world
                _world.Update();

                // Update the time
                serverTimeUpdater.Update(GetTime());

                // Execute the queued commands
                if (_consoleCommandQueue.Count > 0)
                {
                    lock (_consoleCommandSync)
                    {
                        while (_consoleCommandQueue.Count > 0)
                        {
                            var command = _consoleCommandQueue.Dequeue();
                            var ret = _consoleCommands.ExecuteCommand(command);

                            if (ConsoleCommandExecuted != null)
                                ConsoleCommandExecuted.Invoke(this, command, ret);
                        }
                    }
                }

                // Check if we can afford sleeping the thread
                long sleepTime = _serverUpdateRate - (_gameTimer.ElapsedMilliseconds - loopStartTime);
                if (sleepTime > 0)
                    Thread.Sleep((int)sleepTime);

                ++_tick;
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
                MaxInventorySize = GameData.MaxInventorySize,
                MaxShopItems = ShopSettings.Instance.MaxShopItems,
                MaxStatusEffectPower = StatusEffectsSettings.Instance.MaxStatusEffectPower,
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
            ThreadAsserts.IsMainThread();

            if (conn == null)
            {
                if (log.IsErrorEnabled)
                    log.Error("conn is null.");
                return;
            }

            // Try to log in the account
            UserAccount userAccount;
            AccountLoginResult loginResult = UserAccount.Login(DbController, conn, name, password, out userAccount);

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

            // Start the main game loop
            GameLoop();
        }

        /// <summary>
        /// Ensures we have added the <see cref="DbControllerQueryAttribute"/> where needed.
        /// </summary>
        static void ValidateDbControllerQueryAttributes()
        {
            const string errmsg =
                "Type `{0}` fails to implement attribute `{1}`. Ensure this is okay. If you are unsure, add the attribute anyways.";
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