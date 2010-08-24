using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            Listen(GameData.ServerTCPPort, GameData.ServerUDPPort, ServerSettings.AllowRemoteConnections);

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
                var account = World.GetUserAccount(conn);
                if (account != null)
                    account.Dispose();
            }

            // Get the received data
            IEnumerable<AddressedPacket> nonConnData;
            IEnumerable<SocketReceiveData> connData;
            GetReceivedData(out connData, out nonConnData);

            // Process the unreliable data separately
            ProcessUnreliableData(nonConnData);

            // Process the received reliable data. Any important data is going to be here, since TCP is used in client -> server
            // communication for basically everything (the server only sends unreliable data to the client, not vise versa).
            _packetHandler.Process(connData);

            // Update the latency tracker
            _latencyTracker.Update();
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling the corresponding event without
        /// the overhead of using event hooks. Therefore, it is recommended that this overload is used instead of
        /// the corresponding event when possible.
        /// </summary>
        /// <param name="conn">Connection on which the event occured.</param>
        protected override void OnConnectedFrom(IIPSocket conn)
        {
            base.OnConnectedFrom(conn);

            // Get the UDP connection set up
            using (var pw = ServerPacket.RequestUDPConnection(conn.GetHashCode()))
            {
                conn.Send(pw, true);
            }

            // Send the current server time
            using (var pw = ServerPacket.SetGameTime(DateTime.Now))
            {
                conn.Send(pw);
            }
        }

        /// <summary>
        /// Processes the unreliable data received from a connectionless protocol (UDP). Since all data the server receives
        /// from the client related to the game is done over a reliable protocol (TCP), unreliable data is mostly just for
        /// handling networking-related tasks and not actual game data.
        /// </summary>
        /// <param name="data">The received data from the connectionless protocol (UDP).</param>
        void ProcessUnreliableData(IEnumerable<AddressedPacket> data)
        {
            if (data == null || data.IsEmpty())
                return;

            foreach (var v in data)
            {
                // Read the challenge value
                var challenge = BitConverter.ToInt32(v.Data, 0);

                // Grab the IPEndPoint and get the numeric value of the IP address
                var ipEP = ((IPEndPoint)v.RemoteEndPoint);
                var ipAsUInt = IPAddressHelper.IPv4AddressToUInt(ipEP.Address.GetAddressBytes(), 0);

                // Find all connections from the given IP
                var connsFromIP = FindConnections(x => x.IP == ipAsUInt);
                foreach (var c in connsFromIP)
                {
                    // Check which connection from the given IP has a hash that matches the received challenge hash
                    if (c.GetHashCode() == challenge)
                    {
                        c.SetRemoteUnreliablePort(ipEP.Port);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Removes all connections that have not been assigned a user.
        /// </summary>
        /// <param name="timeOut">Minimum amount of time a connection has to be established.</param>
        public void RemoveInactiveConnections(uint timeOut)
        {
            ThreadAsserts.IsMainThread();

            var currTime = TickCount.Now;
            Remove(conn => (conn.Tag == null) && (currTime - conn.TimeCreated > timeOut));

            // FUTURE: Will also have to add in a check to remove any UserAccounts with no active user and have had no active user for a while
        }
    }
}