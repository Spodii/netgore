using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

using log4net;
using NetGore;
using NetGore.Network;

// TODO: When an item stops moving, send the position again to ensure it is valid

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

        readonly AllianceManager _allianceManager;
        readonly DBController _dbController;

        /// <summary>
        /// Stopwatch to track the total elapsed time the game has been running
        /// </summary>
        readonly Stopwatch _gameTimer = new Stopwatch();

        /// <summary>
        /// Thread for managing console input
        /// </summary>
        readonly Thread _inputThread;

        readonly ItemTemplates _itemTemplates;
        readonly NPCDropManager _npcDropManager;
        readonly NPCTemplateManager _npcManager;
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

        public AllianceManager AllianceManager
        {
            get { return _allianceManager; }
        }

        /// <summary>
        /// Gets the DBController used to communicate with the database by this server.
        /// </summary>
        public DBController DBController
        {
            get { return _dbController; }
        }

        /// <summary>
        /// Gets the ItemTemplates
        /// </summary>
        public ItemTemplates ItemTemplates
        {
            get { return _itemTemplates; }
        }

        public NPCDropManager NPCDropManager
        {
            get { return _npcDropManager; }
        }

        /// <summary>
        /// Gets the global NPCTemplateManager
        /// </summary>
        public NPCTemplateManager NPCTemplateManager
        {
            get { return _npcManager; }
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
            // Get the server settings
            const string settingsPath = "Settings.xml";
            ServerSettings settings = new ServerSettings(settingsPath);

            // Open the database connection
            try
            {
                _dbController = new DBController(settings.SqlConnectionString());
            }
            catch (Exception ex)
            {
                const string msg = "Failed to create connection to MySql database.";
                Debug.Fail(msg);
                if (log.IsFatalEnabled)
                    log.Fatal(msg, ex);
                Dispose();
                return;
            }

            // Load the game data and such
            GameData.Load();
            ItemEntity.Initialize(DBController);
            _allianceManager = new AllianceManager(DBController);
            _itemTemplates = new ItemTemplates(DBController.SelectItemTemplates);
            _npcDropManager = new NPCDropManager(DBController.SelectNPCDrops, _itemTemplates);
            _npcManager = new NPCTemplateManager(DBController.SelectNPCTemplate, AllianceManager, _npcDropManager);

            // Create the world and sockets
            _world = new World(this);
            _sockets = new ServerSockets(this);

            // Start the input thread
            _inputThread = new Thread(HandleInput) { Name = "Input Handler" };
            _inputThread.Start();

            // Start the main game loop
            GameLoop();
        }

        /// <summary>
        /// Lock used to ensure that only one user is logging in at a time. The main intention of this is to prevent
        /// a race condition allowing a User to log in twice with the same character.
        /// </summary>
        readonly object _loginLock = new object();

        /// <summary>
        /// Handles the login attempt of a user.
        /// </summary>
        /// <param name="conn">Connection that the login request was made on.</param>
        /// <param name="name">Name of the user.</param>
        /// <param name="password">Entered password for this user.</param>
        public void LoginUser(IIPSocket conn, string name, string password)
        {
            if (conn == null)
            {
                if (log.IsErrorEnabled)
                    log.Error("conn is null.");
                return;
            }

            // Check that the account is valid, and a valid password was specified
            if (!User.IsValidAccount(DBController.SelectUserPassword, name, password))
            {
                if (log.IsInfoEnabled)
                    log.InfoFormat("Login for user `{0}` failed due to invalid name or password.", name);

                using (PacketWriter pw = ServerPacket.LoginUnsuccessful(GameMessage.LoginInvalidNamePassword))
                    conn.Send(pw);

                return;
            }

            lock (_loginLock)
            {
                // Check if the user is already logged in
                if (World.FindUser(name) != null)
                {
                    if (log.IsInfoEnabled)
                        log.InfoFormat("Login for user `{0}` failed since they are already online.", name);

                    using (PacketWriter pw = ServerPacket.LoginUnsuccessful(GameMessage.LoginUserAlreadyOnline))
                        conn.Send(pw);

                    return;
                }

                // Send the "Login Successful" message
                using (PacketWriter pw = ServerPacket.LoginSuccessful())
                    conn.Send(pw);

                // Create the User
                new User(conn, World, name);
            }
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
                    Console.WriteLine(" - " + resultStr);
            }
        }

        public void Shutdown()
        {
            _isRunning = false;
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes of the server
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
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