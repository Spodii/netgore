using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
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
    public class Server : IDisposable, IGetTime, IServerSaveable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly Queue<string> _consoleCommandQueue = new Queue<string>();
        readonly object _consoleCommandSync = new object();
        readonly ConsoleCommands _consoleCommands;
        readonly IDbController _dbController;
        readonly GroupManager _groupManager;
        readonly List<string> _motd = new List<string>();
        readonly ServerSockets _sockets;
        readonly TickCount _startupTime = TickCount.Now;
        readonly World _world;

        bool _disposed;
        bool _isRunning = true;
        TickCount _nextServerSaveTime;
        IServerSettingTable _serverSettings;
        int _tick;

        /// <summary>
        /// Initializes a new instance of the <see cref="Server"/> class.
        /// </summary>
        public Server()
        {
            // Check for some system settings
            if (!BitConverter.IsLittleEndian)
            {
                const string errmsg = "NetGore does not support systems that are not in Little Endian mode!";
                log.Fatal(errmsg);
                throw new SystemException(errmsg);
            }

            // Initialize the engine settings
            EngineSettingsInitializer.Initialize();

            // Create the DbController
            var settings = new DbConnectionSettings();
            _dbController =
                settings.CreateDbControllerPromptEditWhenInvalid(x => new ServerDbController(x.GetMySqlConnectionString()),
                                                                 x => PromptEditDbSettingsFile(settings, x));

            if (_dbController == null)
                return;

            // Load the server settings
            LoadSettings();

            // Load the game data and such
            InitializeScripts();

            // Validate the database
            DbTableValidator.ValidateTables(_dbController);
            ValidateDbControllerQueryAttributes();

            // Update the GameData table
            var gameDataValues = GetGameConstantTableValues();
            DbController.GetQuery<UpdateGameConstantTableQuery>().Execute(gameDataValues);
            if (log.IsInfoEnabled)
                log.Info("Updated the GameData table with the current values.");

            // Create some objects
            _consoleCommands = new ConsoleCommands(this);
            _groupManager = new GroupManager((gm, x) => new Group(x));
            _world = new World(this);
            _sockets = new ServerSockets(this);

            // Clean-up
            new ServerRuntimeCleaner(this);

            if (log.IsInfoEnabled)
                log.Info("Server loaded.");

            if (log.IsWarnEnabled && !ServerSettings.AllowRemoteConnections)
            {
                log.Warn("NOTICE: ServerSettings.AllowRemoteConnections is set to false." +
                         " As a result, the server will only accept local connections." +
                         " If you do not care or want remote machines to connect to the server, please ignore this message.");
            }
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
        public TickCount StartupTime
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
            var success = UserAccount.TryCreateAccount(DbController, conn, name, password, email, out id, out errorMessage);

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
            var success = UserAccount.TryAddCharacter(DbController, account.Name, name, out errorMessage);

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
            var scriptTypes = new ScriptTypeCollection(name, path);

            // Display warnings
            if (log.IsWarnEnabled)
            {
                foreach (var warning in scriptTypes.CompilerErrors.Where(x => x.IsWarning))
                {
                    log.Warn(warning);
                }
            }

            // Display errors
            if (log.IsErrorEnabled)
            {
                foreach (var error in scriptTypes.CompilerErrors.Where(x => !x.IsWarning))
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

            var updateServerTimeQuery = DbController.GetQuery<UpdateServerTimeQuery>();
            var serverTimeUpdater = new ServerTimeUpdater(updateServerTimeQuery);

            _nextServerSaveTime = GetTime() + ServerSettings.RoutineServerSaveRate;

            var worldStatsTracker = WorldStatsTracker.Instance;

            while (_isRunning)
            {
                // Store the loop start time so we can calculate how long the loop took
                var loopStartTime = GetTime();

                // Update the networking
                ServerSockets.Heartbeat();

                // Update the world
                _world.Update();

                // Update the time
                serverTimeUpdater.Update(GetTime());

                // Handle the queued console commands
                ProcessConsoleCommands();

                // Check if it is time to save the world
                if (_nextServerSaveTime < loopStartTime)
                    ServerSave();

                // Update the world stats
                worldStatsTracker.Update();

                // Check if we can afford sleeping the thread
                var sleepTime = (long)ServerSettings.ServerUpdateRate - (GetTime() - loopStartTime);
                if (sleepTime > 0)
                    Thread.Sleep((int)sleepTime);

                ++_tick;
            }

            // Once the thread reaches this point, it means it is closing since the main loop has stopped

            // Update the world stats one last time before the server closes
            worldStatsTracker.Update();
        }

        /// <summary>
        /// Creates an <see cref="IGameConstantTable"/> with the current <see cref="GameData"/> values.
        /// </summary>
        /// <returns>An <see cref="IGameConstantTable"/> with the current <see cref="GameData"/> values.</returns>
        static IGameConstantTable GetGameConstantTableValues()
        {
            var gdt = new GameConstantTable
            {
                MaxAccountNameLength = (byte)GameData.AccountName.MaxLength,
                MaxAccountPasswordLength = (byte)GameData.AccountPassword.MaxLength,
                MaxCharacterNameLength = (byte)GameData.UserName.MaxLength,
                MaxCharactersPerAccount = GameData.MaxCharactersPerAccount,
                MaxInventorySize = GameData.MaxInventorySize,
                MaxShopItems = ShopSettings.Instance.MaxShopItems,
                MaxStatusEffectPower = StatusEffectsSettings.Instance.MaxStatusEffectPower,
                MinAccountNameLength = (byte)GameData.AccountName.MinLength,
                MinAccountPasswordLength = (byte)GameData.AccountPassword.MinLength,
                MinCharacterNameLength = (byte)GameData.UserName.MinLength,
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
            // TODO: !! Send the error message via Disconnect()
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
                    
                case AccountLoginResult.OldClient:
                    if (log.IsInfoEnabled)
                        log.InfoFormat("Login for account `{0}` failed: Old client version.", name);
                    loginFailureGameMessage = GameMessage.OldClientVersion;
                    break;

                default:
                    // If the value is undefined, just say its an invalid name
                    const string errmsg = "Invalid AccountLoginResult value `{0}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, loginResult);
                    loginFailureGameMessage = GameMessage.LoginInvalidName;
                    Debug.Fail(string.Format(errmsg, loginResult));
                    break;
            }

            using (var pw = ServerPacket.LoginUnsuccessful(loginFailureGameMessage))
            {
                conn.Send(pw);
            }

            conn.Disconnect();
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
        /// <param name="clientVersion">The version of the client.</param>
        public void LoginAccount(IIPSocket conn, string name, string password, string clientVersion)
        {
            ThreadAsserts.IsMainThread();

            if (conn == null)
            {
                if (log.IsErrorEnabled)
                    log.Error("conn is null.");
                return;
            }

            if (clientVersion != ServerSettings.MinimumClientVersion)
            {
                HandleFailedLogin(conn, AccountLoginResult.OldClient, name);
                return;
            }

            // Try to log in the account
            UserAccount userAccount;
            var loginResult = UserAccount.Login(DbController, conn, name, password, out userAccount);
    
            // Check that the login was successful
            if (loginResult != AccountLoginResult.Successful)
            {
                HandleFailedLogin(conn, loginResult, name);
                return;
            }

            // Check if banned
            int banMins;
            string banReason;
            if (BanningManager.Instance.IsBanned(userAccount.ID, out banReason, out banMins))
            {
                conn.Send(ServerPacket.LoginUnsuccessful(GameMessage.AccountBanned, banMins, banReason));
                userAccount.Dispose();
                if (log.IsInfoEnabled)
                    log.InfoFormat("Disconnected account `{0}` after successful login since they have been banned.", name);
                return;
            }

            // Set the connection's tag to the account
            conn.Tag = userAccount;

            // Send the "Login Successful" message
            using (var pw = ServerPacket.LoginSuccessful())
            {
                conn.Send(pw);
            }

            if (log.IsInfoEnabled)
                log.InfoFormat("Login for account `{0}` successful.", name);

            // Send the account characters
            userAccount.SendAccountCharacterInfos();
        }

        /// <summary>
        /// Processes any queued console commands in the <see cref="_consoleCommandQueue"/>.
        /// </summary>
        void ProcessConsoleCommands()
        {
            string[] commandsToExecute = null;

            // Grab the commands to be executed into a local array so we can release the lock quickly
            lock (_consoleCommandSync)
            {
                if (_consoleCommandQueue.Count > 0)
                {
                    commandsToExecute = _consoleCommandQueue.ToArray();
                    _consoleCommandQueue.Clear();
                }
            }

            // Execute the commands
            if (commandsToExecute != null)
            {
                foreach (var cmd in commandsToExecute)
                {
                    var ret = _consoleCommands.ExecuteCommand(cmd);

                    if (ConsoleCommandExecuted != null)
                        ConsoleCommandExecuted.Invoke(this, cmd, ret);
                }
            }
        }

        /// <summary>
        /// Creates the prompt for editing the <see cref="DbConnectionSettings"/> file.
        /// </summary>
        /// <param name="s">The <see cref="DbConnectionSettings"/>.</param>
        /// <param name="msg">The message to display.</param>
        /// <returns>True if to retry the connection; false to abort.</returns>
        static bool PromptEditDbSettingsFile(DbConnectionSettings s, string msg)
        {
            if (!s.OpenFileForEdit())
                return false;

            const string instructions =
                "Please edit the database settings with the appropriate values. Press Retry when done editing, or Cancel to abort.";

            if (msg == null)
                msg = instructions;
            else
                msg += Environment.NewLine + Environment.NewLine + instructions;

            if (MessageBox.Show(msg, "Edit database settings", MessageBoxButtons.RetryCancel) == DialogResult.Cancel)
                return false;

            s.Reload();

            return true;
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
            // Clean up the garbage generated during the initialization phase
            GC.Collect();

            if (log.IsInfoEnabled)
                log.Info("Server done loading. Game loop has started...");

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
        public TickCount GetTime()
        {
            return TickCount.Now;
        }

        #endregion

        #region IServerSaveable Members

        /// <summary>
        /// Saves the state of this object and all <see cref="IServerSaveable"/> objects under it to the database.
        /// </summary>
        public void ServerSave()
        {
            if (log.IsInfoEnabled)
                log.InfoFormat("Starting save of world state...");

            // Get the start time so we can log how long the save took
            var saveStartTime = GetTime();

            // Save
            _world.ServerSave();

            // Get the end time and log the delta time
            var saveEndTime = GetTime();

            if (log.IsInfoEnabled)
                log.InfoFormat("World state saved. Save took a total of `{0}` milliseconds.", saveEndTime - saveStartTime);

            // Update the next auto save time
            _nextServerSaveTime = saveEndTime + ServerSettings.RoutineServerSaveRate;
        }

        #endregion
    }
}