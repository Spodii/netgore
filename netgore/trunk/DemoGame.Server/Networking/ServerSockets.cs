using System;
using System.Linq;
using NetGore;
using NetGore.Network;

namespace DemoGame.Server
{
    /// <summary>
    /// Server socket manager
    /// </summary>
    public class ServerSockets : SocketManager
    {
        readonly LatencyTrackerServer _latencyTracker;
        readonly ServerPacketHandler _packetHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerSockets"/> class.
        /// </summary>
        /// <param name="server">The server.</param>
        public ServerSockets(Server server)
        {
            _packetHandler = new ServerPacketHandler(this, server);
            Listen(GameData.ServerTCPPort, ServerSettings.AllowRemoteConnections);

            _latencyTracker = new LatencyTrackerServer(GameData.ServerPingPort);
        }

        /// <summary>
        /// Updates the sockets
        /// </summary>
        public void Heartbeat()
        {
            ThreadAsserts.IsMainThread();

            // Close down connections
            foreach (var conn in _packetHandler.GetDisconnectedSockets())
            {
                UserAccount account = World.GetUserAccount(conn);
                if (account != null)
                    account.Dispose();
            }

            // Process received data
            var recvData = GetReceivedData();
            _packetHandler.Process(recvData);

            // Update the latency tracker
            _latencyTracker.Update();
        }

        /// <summary>
        /// Removes all connections that have not been assigned a user
        /// </summary>
        /// <param name="timeOut">Minimum amount of time a connection has to be established</param>
        public void RemoveInactiveConnections(int timeOut)
        {
            ThreadAsserts.IsMainThread();

            int currTime = Environment.TickCount;
            Remove(conn => (conn.Tag == null) && (currTime - conn.TimeCreated > timeOut));

            // FUTURE: Will also have to add in a check to remove any UserAccounts with no active user and have had no active user for a while
        }
    }
}