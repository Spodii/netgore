using System;
using System.Diagnostics;
using System.Linq;
using NetGore;
using NetGore.IO;
using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// The client socket manager.
    /// </summary>
    class ClientSockets : SocketManager, IGetTime, ISocketSender
    {
        const int _updateLatencyInterval = 5000;
        static ClientSockets _instance;
        readonly ClientPacketHandler _packetHandler;
        readonly int _udpPort;

        IIPSocket _conn = null;
        bool _isConnecting = false;
        int _lastPingTime;
        LatencyTrackerClient _latencyTracker;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSockets"/> class.
        /// </summary>
        /// <param name="gameplayScreen">The <see cref="GameplayScreen"/>.</param>
        /// <exception cref="MethodAccessException">An instance of this object has already been created.</exception>
        ClientSockets(GameplayScreen gameplayScreen)
        {
            if (_instance != null)
                throw new MethodAccessException("ClientSockets instance was already created. Use that instead.");

            _packetHandler = new ClientPacketHandler(this, gameplayScreen, DynamicEntityFactory.Instance);
            OnConnect += SocketManager_OnConnect;
            OnDisconnect += delegate { _isConnecting = false; };
            OnFailedConnect += delegate { _isConnecting = false; };

            // Bind the UDP port
            _udpPort = BindUDP();
        }

        /// <summary>
        /// Gets the last <see cref="ClientSockets"/> instance. Will be null if it has not been created yet.
        /// </summary>
        public static ClientSockets Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets if the socket is currently trying to connect, and is waiting for the connection to be made or rejected.
        /// </summary>
        public bool IsConnecting
        {
            get
            {
                if (_conn != null)
                    return false;

                return _isConnecting;
            }
        }

        /// <summary>
        /// Gets the latency of the connection in milliseconds.
        /// </summary>
        public int Latency
        {
            get { return _latencyTracker.Latency; }
        }

        /// <summary>
        /// Gets the ClientPacketHandler used to handle data from this ClientSockets.
        /// </summary>
        public ClientPacketHandler PacketHandler
        {
            get { return _packetHandler; }
        }

        /// <summary>
        /// Gets the TCPSocket used for the connection to the server if a connection is established.
        /// </summary>
        public IIPSocket Socket
        {
            get { return _conn; }
        }

        /// <summary>
        /// Starts the client's connection to the server, or does nothing if <see cref="ClientSockets.IsConnecting"/>
        /// or <see cref="ClientSockets.IsConnected"/> is true.
        /// </summary>
        public void Connect()
        {
            if (IsConnecting || IsConnected)
                return;

            _isConnecting = true;

            Connect(GameData.ServerIP, GameData.ServerTCPPort);
        }

        /// <summary>
        /// Updates the sockets.
        /// </summary>
        public void Heartbeat()
        {
            // Ensure the connection has been established first
            if (_conn == null)
                return;

            // Process received data
            var recvData = GetReceivedData();
            _packetHandler.Process(recvData);

            // Update the latency tracker
            _latencyTracker.Update();

            // Check if enough time has elapsed for sending another ping
            if (_lastPingTime + _updateLatencyInterval < GetTime())
                Ping();
        }

        /// <summary>
        /// Initializes the <see cref="ClientSockets"/> instance. This only needs to be called once.
        /// </summary>
        /// <param name="gameplayScreen">The <see cref="GameplayScreen"/>.</param>
        /// <exception cref="ArgumentNullException"><see cref="gameplayScreen"/> is null.</exception>
        public static void Initialize(GameplayScreen gameplayScreen)
        {
            if (gameplayScreen == null)
                throw new ArgumentNullException("gameplayScreen");

            if (Instance != null)
                return;

            _instance = new ClientSockets(gameplayScreen);
        }

        void Ping()
        {
            if (_latencyTracker == null)
            {
                Debug.Fail("LatencyTrackerClient has not been set up yet!");
                return;
            }

            _lastPingTime = GetTime();
            _latencyTracker.Ping();
        }

        /// <summary>
        /// Sets the active connection when the connection is made so it can be used.
        /// </summary>
        /// <param name="conn">Incoming connection.</param>
        void SocketManager_OnConnect(IIPSocket conn)
        {
            _conn = conn;
            _latencyTracker = new LatencyTrackerClient(GameData.ServerIP, GameData.ServerPingPort);
            Ping();

            _isConnecting = false;

            // Make sure the very first thing we send is the Client's UDP port so the server knows what
            // port to use when sending the data
            using (PacketWriter pw = ClientPacket.SetUDPPort(_udpPort))
            {
                Send(pw);
            }
        }

        #region IGetTime Members

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <returns>Current time.</returns>
        public int GetTime()
        {
            return _packetHandler.GetTime();
        }

        #endregion

        #region ISocketSender Members

        /// <summary>
        /// Gets a bool stating if the client is currently connected or not.
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if (_conn == null)
                    return false;

                return _conn.IsConnected;
            }
        }

        /// <summary>
        /// Sends data to the server.
        /// </summary>
        /// <param name="data">BitStream containing the data to send.</param>
        public void Send(BitStream data)
        {
            _conn.Send(data);
        }

        /// <summary>
        /// Asynchronously sends data to the socket.
        /// </summary>
        /// <param name="data">Data to send.</param>
        public void Send(byte[] data)
        {
            _conn.Send(data);
        }

        #endregion
    }
}