using System.Diagnostics;
using System.Linq;
using NetGore;
using NetGore.IO;
using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// Client socket manager
    /// </summary>
    public class ClientSockets : SocketManager, IGetTime, ISocketSender
    {
        const int _updateLatencyInterval = 5000;
        readonly ClientPacketHandler _packetHandler;
        readonly int _udpPort;
        IIPSocket _conn = null;
        int _lastPingTime;
        LatencyTrackerClient _latencyTracker;

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

        public ClientSockets(GameplayScreen gameplayScreen)
        {
            _packetHandler = new ClientPacketHandler(this, gameplayScreen);
            OnConnect += onConnect;

            // Bind the UDP port
            _udpPort = BindUDP();
        }

        /// <summary>
        /// Starts the client's connection to the server.
        /// </summary>
        public void Connect()
        {
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
        /// Sets the active connection when the connection is made so it can be used.
        /// </summary>
        /// <param name="conn">Incoming connection.</param>
        void onConnect(IIPSocket conn)
        {
            _conn = conn;
            _latencyTracker = new LatencyTrackerClient(GameData.ServerIP, GameData.ServerPingPort);
            Ping();

            // Make sure the very first thing we send is the Client's UDP port so the server knows what
            // port to use when sending the data
            using (PacketWriter pw = ClientPacket.SetUDPPort(_udpPort))
            {
                Send(pw);
            }
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