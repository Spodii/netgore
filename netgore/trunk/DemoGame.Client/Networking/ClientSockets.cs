using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
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
        readonly ClientPacketHandler _packetHandler;
        IIPSocket _conn = null;
        readonly int _udpPort;

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
        /// Starts the client's connection to the server
        /// </summary>
        public void Connect()
        {
            Connect(GameData.ServerIP, GameData.ServerTCPPort);
        }

        /// <summary>
        /// Updates the sockets
        /// </summary>
        public void Heartbeat()
        {
            // Process received data
            var recvData = GetReceivedData();
            _packetHandler.Process(recvData);
        }

        /// <summary>
        /// Sets the active connection when the connection is made so it can be used
        /// </summary>
        /// <param name="conn">Incoming connection</param>
        void onConnect(IIPSocket conn)
        {
            _conn = conn;
            
            // Make sure the very first thing we send is the Client's UDP port so the server knows what
            // port to use when sending the data
            using (var pw = ClientPacket.SetUDPPort(_udpPort))
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
        /// Sends data to the server
        /// </summary>
        /// <param name="data">BitStream containing the data to send</param>
        public void Send(BitStream data)
        {
            _conn.Send(data);
        }

        #endregion
    }
}