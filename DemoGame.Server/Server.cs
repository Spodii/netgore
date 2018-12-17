using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using DemoGame.Server.Properties;
using DemoGame.Server.Queries;
using log4net;
using NetGore;
using NetGore.Db;
using NetGore.Db.MySql;
using NetGore.Features.Groups;
using NetGore.IO;
using NetGore.Network;

namespace DemoGame.Server
{
    /// <summary>
    /// The core component of the game server.
    /// </summary>
    public class Server : IGetTime, IServerSaveable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly Queue<string> _consoleCommandQueue = new Queue<string>();
        readonly object _consoleCommandSync = new object();
        readonly ConsoleCommands _consoleCommands;
        readonly IDbController _dbController;
        readonly GroupManager _groupManager;
        readonly ServerSockets _sockets;
        readonly TickCount _startupTime = TickCount.Now;
        readonly UserAccountManager _userAccountManager;
        readonly World _world;

        bool _hasStarted;
        bool _isRunning = true;
        TickCount _nextServerSaveTime;
        int _tick;

        /// <summary>
        /// Initializes a new instance of the <see cref="Server"/> class.
        /// </summary>
        /// <exception cref="NotSupportedException">NetGore does not support systems that are not in Little Endian mode!</exception>
        public Server()
        {
            ThreadAsserts.IsMainThread();

            // Check for some system settings
            if (!BitConverter.IsLittleEndian)
            {
                const string errmsg = "NetGore does not support systems that are not in Little Endian mode!";
                log.Fatal(errmsg);
                throw new NotSupportedException(errmsg);
            }

            // Check if patching is needed
            int numMissingPatches = -1;
            try
            {
                numMissingPatches = ServerDbPatcher.GetMissingPatches().Length;
            }
            catch (Exception ex)
            {
                log.WarnFormat("Failed to find DbPatches directory, so could not check if patching is required. Exception: {0}", ex);
            }

            if (numMissingPatches > 0)
            {
                log.ErrorFormat("There are `{0}` NetGore db patches not applied. Please backup your database then run /DemoGame.Server/DbPatches/Patch.bat.", numMissingPatches);
                log.ErrorFormat("Server will auto-close after 10 seconds, and will keep doing this until you patch.");
                Thread.Sleep(10 * 1000);
                return;
            }

            // Initialize the engine settings
            EngineSettingsInitializer.Initialize();

            // Create the DbController
            _dbController = CreateDbController();

            // Add the query stats tracker
            var queryStats = new BasicQueryStatsTracker { LogFilePath = ContentPaths.Build.Root.Join("querystats.txt") };
            queryStats.LogFileFrequency = 1000 * 5;
            _dbController.ConnectionPool.QueryStats = queryStats;

            // Validate the database
            DbTableValidator.ValidateTables(_dbController);
            ValidateDbControllerQueryAttributes();

            // Clean-up
            var cleaner = new ServerCleaner(this);
            cleaner.Run();

            // Create some objects
            _consoleCommands = new ConsoleCommands(this);
            _groupManager = new GroupManager((gm, x) => new Group(x));
            _userAccountManager = new UserAccountManager(DbController);
            _world = new World(this);
            _sockets = new ServerSockets(this);

            WorldStatsTracker.Instance.NetPeerToTrack = _sockets.GetNetServer();

            // Check for the password salt
            if (string.IsNullOrEmpty(ServerSettings.Default.PasswordSalt))
            {
                const string errmsg =
                    "No password salt has been defined in the server settings file. Make sure you define one before releasing.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg);
            }

            // Set the thread priority
            SetThreadPriority(ServerSettings.Default.ThreadPriority);

            // Validate the server's settings
            var ssv = new ServerSettingsValidator();
            ssv.Check(this);

            if (log.IsInfoEnabled)
                log.Info("Server loaded.");
        }

        /// <summary>
        /// Creates a new DbController instance if one does not already exist. Do not use this to just acquire a DbController instance when one may already exist -
        /// use DbControllerBase.GetInstance() instead.
        /// </summary>
        /// <returns></returns>
        public static ServerDbController CreateDbController()
        {
            ServerDbController db = null;
            
            // See if the instance already exists
            try
            {
                db = DbControllerBase.GetInstance() as ServerDbController;
            }
            catch (MemberAccessException)
            {
            }

            if (db == null)
            {
                var settings = new DbConnectionSettings();
                db = (ServerDbController)settings.CreateDbControllerPromptEditWhenInvalid(x => new ServerDbController(x.GetMySqlConnectionString()),
                    x => PromptEditDbSettingsFile(settings, x));
            }

            return db;
        }

        /// <summary>
        /// Notifies listeners when a console command has been executed.
        /// </summary>
        public event TypedEventHandler<Server, ServerConsoleCommandEventArgs> ConsoleCommandExecuted;

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
        /// Gets if the server is running.
        /// </summary>
        public bool IsRunning
        {
            get { return _isRunning; }
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

        public UserAccountManager UserAccountManager
        {
            get { return _userAccountManager; }
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
            if (!RequireServerRunning())
                return;

            ThreadAsserts.IsMainThread();

            if (conn == null)
            {
                if (log.IsErrorEnabled)
                    log.Error("conn is null.");
                return;
            }

            // Create the account
            GameMessage failReason;
            var success = UserAccountManager.TryCreateAccount(conn, name, password, email, out failReason);

            // Send the appropriate success message
            using (var pw = ServerPacket.CreateAccount(success, failReason))
            {
                conn.Send(pw, ServerMessageType.System);
            }
        }

        /// <summary>
        /// Handles the request to create a new account.
        /// </summary>
        /// <param name="conn">Connection that the request was made on.</param>
        /// <param name="name">The name of the character to create.</param>
        public void CreateAccountCharacter(IIPSocket conn, string name)
        {
            if (!RequireServerRunning())
                return;

            ThreadAsserts.IsMainThread();

            // Get the account
            var account = conn.Tag as IUserAccount;
            if (account == null)
                return;

            // Try to create the character
            string errorMessage;
            var success = UserAccountManager.TryAddCharacter(account.Name, name, out errorMessage);

            // Send the result to the client (which we have to do both when successful and failed)
            using (var pw = ServerPacket.CreateAccountCharacter(success, errorMessage))
            {
                conn.Send(pw, ServerMessageType.System);
            }

            // If we successfully created the character, reload and resync the character listing
            if (success)
            {
                account.LoadCharacterIDs();
                account.SendAccountCharacterInfos();
            }
        }

        /// <summary>
        /// Enqueues a console command string to be executed. When the command is executed, the results will be returned
        /// through the <see cref="Server.ConsoleCommandExecuted"/> event.
        /// </summary>
        /// <param name="commandString">The command to be executed.</param>
        public void EnqueueConsoleCommand(string commandString)
        {
            if (!RequireServerRunning())
                return;

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

            // Set the initial auto-save time
            _nextServerSaveTime = GetTime() + ServerSettings.Default.RoutineServerSaveRate;

            var worldStatsTracker = WorldStatsTracker.Instance;

            while (IsRunning)
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
                var updateElapsedTime = (long)GetTime() - loopStartTime;
                var sleepTime = ServerSettings.Default.ServerUpdateRate - updateElapsedTime;
                if (sleepTime > 0)
                    Thread.Sleep((int)sleepTime);

                ++_tick;
            }

            // Once the thread reaches this point, it means it is closing since the main loop has stopped

            // Update the world stats and events one last time before the server closes
            worldStatsTracker.Update();
            EventCounterManager.FlushAll();

            // Dispose
            if (ServerSockets != null)
                ServerSockets.Shutdown();

            if (World != null)
                World.Dispose();

            if (DbController != null)
                DbController.Dispose();
        }

        static void HandleFailedLogin(IIPSocket conn, AccountLoginResult loginResult, string name)
        {
            // Get the error message
            switch (loginResult)
            {
                case AccountLoginResult.AccountInUse:
                    if (log.IsInfoEnabled)
                        log.InfoFormat("Login for account `{0}` failed: Account in use.", name);
                    conn.Disconnect(GameMessage.LoginAccountInUse);
                    break;

                case AccountLoginResult.InvalidName:
                    if (log.IsInfoEnabled)
                        log.InfoFormat("Login for account `{0}` failed: Invalid name.", name);
                    conn.Disconnect(GameMessage.LoginInvalidName);
                    break;

                case AccountLoginResult.InvalidPassword:
                    if (log.IsInfoEnabled)
                        log.InfoFormat("Login for account `{0}` failed: Incorrect password.", name);
                    conn.Disconnect(GameMessage.LoginInvalidPassword);
                    break;

                default:
                    // If the value is undefined, just say its an invalid name
                    const string errmsg = "Invalid AccountLoginResult value `{0}`.";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, loginResult);
                    Debug.Fail(string.Format(errmsg, loginResult));
                    conn.Disconnect(GameMessage.LoginInvalidName);
                    break;
            }
        }

        /// <summary>
        /// Handles the login attempt of an account.
        /// </summary>
        /// <param name="conn">Connection that the login request was made on.</param>
        /// <param name="name">Name of the account.</param>
        /// <param name="password">Entered password for this account.</param>
        public void LoginAccount(IIPSocket conn, string name, string password)
        {
            if (!RequireServerRunning())
                return;

            ThreadAsserts.IsMainThread();

            if (conn == null)
            {
                if (log.IsErrorEnabled)
                    log.Error("conn is null.");
                return;
            }

            // Try to log in the account
            IUserAccount userAccount;
            var loginResult = UserAccountManager.Login(conn, name, password, out userAccount);

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
                userAccount.Dispose(GameMessage.AccountBanned, banMins, banReason);
                if (log.IsInfoEnabled)
                    log.InfoFormat("Disconnected account `{0}` after successful login since they have been banned.", name);
                return;
            }

            // Set the connection's tag to the account
            conn.Tag = userAccount;

            // Send the "Login Successful" message
            using (var pw = ServerPacket.LoginSuccessful())
            {
                conn.Send(pw, ServerMessageType.System);
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
                        ConsoleCommandExecuted.Raise(this, new ServerConsoleCommandEventArgs(cmd, ret));
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
        /// Ensures that the server is running.
        /// </summary>
        /// <returns>If false, the server is not running and the method should be aborted.</returns>
        bool RequireServerRunning()
        {
            if (IsRunning)
                return true;

            const string errmsg = "Cannot process request - server is not running.";
            if (log.IsErrorEnabled)
                log.Error(errmsg);

            return false;
        }

        /// <summary>
        /// Sets the priority of the current thread.
        /// </summary>
        static void SetThreadPriority(ThreadPriority priority)
        {
            try
            {
                var ct = Thread.CurrentThread;

                if (ct.Priority != priority)
                {
                    ct.Priority = priority;

                    if (log.IsInfoEnabled)
                        log.InfoFormat("Server thread priority changed to `{0}`.", priority);
                }
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to set server thread priority to `{0}`. Exception: {1}";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, priority, ex);
            }
        }

        /// <summary>
        /// Makes a request to shut down the server.
        /// </summary>
        public void Shutdown()
        {
            _isRunning = false;
        }

        /// <summary>
        /// Starts the <see cref="Server"/> loop. This will block the calling thread until the server has shut down, unless the
        /// server is already running or cannot be run, in which case this will return immediately.
        /// </summary>
        /// <returns>True if the server was started successfully and this method returned because the server was shut down;
        /// false if this method returned immediately because the server is already running or could not be started.</returns>
        public bool Start()
        {
            if (_dbController == null || _hasStarted)
                return false;

            _hasStarted = true;

            // Clean up the garbage generated during the initialization phase
            GC.Collect();

            if (log.IsInfoEnabled)
                log.Info("Server done loading. Game loop has started...");

            // Start the main game loop
            GameLoop();

            return true;
        }

        /// <summary>
        /// Ensures we have added the <see cref="DbControllerQueryAttribute"/> where needed.
        /// </summary>
        static void ValidateDbControllerQueryAttributes()
        {
            const string errmsg =
                "Type `{0}` fails to implement attribute `{1}`. Ensure this is okay. If you are unsure, add the attribute anyways.";
            var attribType = typeof(DbControllerQueryAttribute);

            new DbControllerQueryAttributeChecker(delegate(DbControllerQueryAttributeChecker sender, EventArgs<Type> e)
            {
                Debug.Fail(string.Format(errmsg, e.Item1, attribType));
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, e.Item1, attribType);
            });
        }

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
            _nextServerSaveTime = saveEndTime + ServerSettings.Default.RoutineServerSaveRate;
        }

        #endregion
    }
}