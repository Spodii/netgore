using System;
using NetGore.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using NetGore;
using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// Client socket manager
    /// </summary>
    public class ClientSockets : SocketManager, IGetTime, ISocketSender
    {
        readonly ClientPacketHandler _packetHandler;
        TCPSocket _conn = null;

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
        /// Gets the ClientPacketHandler used to handle data from this ClientSockets.
        /// </summary>
        public ClientPacketHandler PacketHandler
        {
            get { return _packetHandler; }
        }

        /// <summary>
        /// Gets the TCPSocket used for the connection to the server if a connection is established.
        /// </summary>
        public TCPSocket Socket
        {
            get { return _conn; }
        }

        public ClientSockets(GameplayScreen gameplayScreen)
        {
            _packetHandler = new ClientPacketHandler(this, gameplayScreen);
            OnConnect += onConnect;
        }

        /// <summary>
        /// Starts the client's connection to the server
        /// </summary>
        public void Connect()
        {
            Connect("127.0.0.1", 44445);
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
        void onConnect(TCPSocket conn)
        {
            _conn = conn;
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