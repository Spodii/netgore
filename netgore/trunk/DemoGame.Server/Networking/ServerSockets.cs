using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using NetGore.Network;

namespace DemoGame.Server
{
    /// <summary>
    /// Server socket manager
    /// </summary>
    public class ServerSockets : SocketManager
    {
        readonly ServerPacketHandler _packetHandler;

        /// <summary>
        /// Constructor for the socket
        /// </summary>
        public ServerSockets(Server server)
        {
            _packetHandler = new ServerPacketHandler(this, server);
            Listen(GameData.ServerTCPPort);
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
        /// Removes all connections that have not been assigned a user
        /// </summary>
        /// <param name="timeOut">Minimum amount of time a connection has to be established</param>
        public void RemoveInactiveConnections(int timeOut)
        {
            int currTime = Environment.TickCount;
            Remove(conn => (conn.Tag == null) && (currTime - conn.TimeCreated > timeOut));
        }
    }
}