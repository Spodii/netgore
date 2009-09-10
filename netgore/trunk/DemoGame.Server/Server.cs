using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using DemoGame.Server.Queries;
using log4net;
using NetGore;
using NetGore.Network;
using NetGore.Scripting;

// TODO: When an characterID stops moving, send the position again to ensure it is valid

namespace DemoGame.Server
{
    /// <summary>
    /// The core component of the game server.
    /// </summary>
    public class Server : IDisposable, IGetTime
    {
        /// <summary>
        /// Millisecond rate at which the server updates. The server update rate does not affect the rate
        /// at which physics is update, so modifying the update rate will not affect the game
        /// speed. Server update rate is used to determine how frequently the server checks
        /// for performing updates and how long it is able to "sleep". It is recommended
        /// a high update rate is used to allow for more precise updating.
        /// </summary>
        const long _serverUpdateRate = 5; // 200 FPS

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly DBController _dbController;

        /// <summary>
        /// Stopwatch to track the total elapsed time the game has been running
        /// </summary>
        readonly Stopwatch _gameTimer = new Stopwatch();

        /// <summary>
        /// Thread for managing console input
        /// </summary>
        readonly Thread _inputThread;

        /// <summary>
        /// Lock used to ensure that only one account is logging in at a time. The main intention of this is to prevent
        /// a race condition allowing an account to log in twice from two places at once.
        /// </summary>
        readonly object _loginLock = new object();

        readonly ServerSockets _sockets;

        readonly int _startupTime = Environment.TickCount;

        /// <summary>
        /// World managed by the server
        /// </summary>
        readonly World _world;

        bool _disposed;

        /// <summary>
        /// If the server is running
        /// </summary>
        bool _isRunning = true;

        /// <summary>
        /// Gets the DBController used to communicate with the database by this server.
        /// </summary>
        public DBController DBController
        {
            get { return _dbController; }
        }

        public bool IsDisposed
        {
            get { return _disposed; }
        }

        public ServerSockets ServerSockets
        {
            get { return _sockets; }
        }

        /// <summary>
        /// Gets the Environment.TickCount time that the server started
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
        /// Server constructor
        /// </summary>
        public Server()
        {
            DBConnectionSettings settings = new DBConnectionSettings();
            _dbController = new DBController(settings.SqlConnectionString());
            DBTableValidator.ValidateTables(_dbController);

            // Load the game data and such
            GameData.Load();
            ItemEntity.Initialize(DBController);
            AllianceManager.Initialize(DBController);
            ItemTemplateManager.Initialize(DBController);
            CharacterTemplateManager.Initialize(DBController);
            InitializeScripts();

            // Create the world and sockets
            _world = new World(this);
            _sockets = new ServerSockets(this);

            // Clean-up
            new ServerRuntimeCleaner(this);

            if (log.IsInfoEnabled)
                log.Info("Server is loaded");

            _inputThread = new Thread(HandleInput) { Name = "Input Handler", IsBackground = true };
        }

        /// <summary>
        /// Creates a ScriptTypeCollection with the specified name.
        /// </summary>
        /// <param name="name">Name of the ScriptTypeCollection.</param>
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
        /// Main game loop for the server
        /// </summary>
        void GameLoop()
        {
            long lastRemoveConnsTime = 0;

            _gameTimer.Reset();
            _gameTimer.Start();

            while (_isRunning)
            {
                // Store the loop start time so we can calculate how long the loop took
                long loopStartTime = _gameTimer.ElapsedMilliseconds;

                // Check to remove inactive connections
                if (_gameTimer.ElapsedMilliseconds - lastRemoveConnsTime > 60000)
                {
                    lastRemoveConnsTime = _gameTimer.ElapsedMilliseconds;
                    ServerSockets.RemoveInactiveConnections(5000);
                }

                // Update the networking
                ServerSockets.Heartbeat();

                // Update the world
                _world.Update();

                // Check if we can afford sleeping the thread
                long sleepTime = _serverUpdateRate - (_gameTimer.ElapsedMilliseconds - loopStartTime);
                if (sleepTime > 0)
                    Thread.Sleep((int)sleepTime);
            }

            _gameTimer.Stop();
        }

        /// <summary>
        /// Handles the server console input
        /// </summary>
        void HandleInput()
        {
            ConsoleCommands consoleCommands = new ConsoleCommands(this);

            while (_isRunning)
            {
                string input = Console.ReadLine();
                string resultStr = consoleCommands.ExecuteCommand(input);
                if (!string.IsNullOrEmpty(resultStr))
                    Console.WriteLine(resultStr);
            }
        }

        /// <summary>
        /// Initializes the scripts.
        /// </summary>
        static void InitializeScripts()
        {
            CreateScriptTypeCollection("AI");
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
                loginResult = UserAccount.Login(DBController, conn, name, password, out userAccount);
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
        /// Shuts down the Server.
        /// </summary>
        public void Shutdown()
        {
            _isRunning = false;
        }

        /// <summary>
        /// Starts the Server loop.
        /// </summary>
        public void Start()
        {
            if (log.IsInfoEnabled)
                log.Info("Server is starting");

            // Start the input thread
            _inputThread.Start();

            // Start the main game loop
            GameLoop();
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes of the server
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
        /// Gets the current game time where time 0 is when the application started
        /// </summary>
        /// <returns>Current game time in milliseconds</returns>
        public int GetTime()
        {
            return (int)_gameTimer.ElapsedMilliseconds;
        }

        #endregion
    }
}