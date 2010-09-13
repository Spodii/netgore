using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Lidgren.Network;
using NetGore;
using NetGore.IO;
using NetGore.Network;

namespace DemoGame.Server
{
    /// <summary>
    /// Server socket manager
    /// </summary>
    public class ServerSockets : ServerSocketManagerBase
    {
        readonly ServerPacketHandler _packetHandler;
        readonly IMessageProcessorManager _messageProcessorManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerSockets"/> class.
        /// </summary>
        /// <param name="server">The <see cref="Server"/> instance.</param>
        public ServerSockets(Server server) : base(GameData.NetworkAppIdentifier, GameData.ServerUDPPort)
        {
            _packetHandler = new ServerPacketHandler(server);

            // When debugging, use the StatMessageProcessorManager instead (same thing as the other, but provides network statistics)
#if DEBUG
            var m = new StatMessageProcessorManager(_packetHandler, EnumHelper<ClientPacketID>.BitsRequired);
            m.Stats.EnableFileOutput(ContentPaths.Build.Root.Join("netstats_in" + EngineSettings.DataFileSuffix));
            _messageProcessorManager = m;
#else
            _ppManager = new MessageProcessorManager(_packetHandler, EnumHelper<ClientPacketID>.BitsRequired);
#endif
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling received data from an <see cref="IIPSocket"/>.
        /// </summary>
        /// <param name="sender">The <see cref="IIPSocket"/> that the data came from.</param>
        /// <param name="data">The data that was received.</param>
        protected override void OnReceiveData(IIPSocket sender, BitStream data)
        {
            base.OnReceiveData(sender, data);

            // Process the data
            _messageProcessorManager.Process(sender, data);
        }

        /// <summary>
        /// Determines whether or not a connection request should be accepted.
        /// </summary>
        /// <param name="ip">The IPv4 address the remote connection is coming from.</param>
        /// <param name="port">The port that the remote connection is coming from.</param>
        /// <param name="data">The data sent along with the connection request.</param>
        /// <returns>
        /// If null or empty, the connection will be accepted. Otherwise, return a non-empty string containing the reason
        /// as to why the connection is to be rejected to reject the connection.
        /// </returns>
        protected override string AcceptConnect(uint ip, ushort port, BitStream data)
        {
            // Check for too many connections from the IP
            if (ServerSettings.MaxConnectionsPerIP > 0)
            {
                var connsFromIP = Connections.Count(x => x.IP == ip);
                if (connsFromIP >= ServerSettings.MaxConnectionsPerIP)
                {
                    return GameMessageHelper.AsString(GameMessage.DisconnectTooManyConnectionsFromIP);
                }
            }

            return base.AcceptConnect(ip, port, data);
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling when the status of an <see cref="IIPSocket"/> changes.
        /// </summary>
        /// <param name="sender">The <see cref="IIPSocket"/> who's status has changed.</param>
        /// <param name="status">The new status.</param>
        /// <param name="reason">The reason for the status change.</param>
        protected override void OnReceiveStatusChanged(IIPSocket sender, NetConnectionStatus status, string reason)
        {
            base.OnReceiveStatusChanged(sender, status, reason);

            switch (status)
            {
                case NetConnectionStatus.Disconnected:
                    // If there was an account on the socket, destroy it
                    var acc = World.GetUserAccount(sender, false);
                    if (acc != null)
                        acc.Dispose();
                    break;

                case NetConnectionStatus.Connected:
                    // Send the server time to the client
                    // TODO: !! Probably will have to add something on the client to allow them to request updates to the time every few (10?) minutes to stay synchronized
                    using (var pw = ServerPacket.SetGameTime(DateTime.Now))
                    {
                        sender.Send(pw);
                    }
                    break;
            }
        }
    }
}