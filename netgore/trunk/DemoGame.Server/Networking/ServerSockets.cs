using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.Properties;
using Lidgren.Network;
using log4net;
using NetGore;
using NetGore.IO;
using NetGore.Network;

namespace DemoGame.Server
{
    /// <summary>
    /// Server socket manager
    /// </summary>
    public class ServerSockets : ServerSocketManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly IMessageProcessorManager _messageProcessorManager;
        readonly ServerPacketHandler _packetHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerSockets"/> class.
        /// </summary>
        /// <param name="server">The <see cref="Server"/> instance.</param>
        public ServerSockets(Server server) : base(CommonConfig.NetworkAppIdentifier, CommonConfig.ServerPort)
        {
            _packetHandler = new ServerPacketHandler(server);

            // When debugging, use the StatMessageProcessorManager instead (same thing as the other, but provides network statistics)
#if DEBUG
            var m = new StatMessageProcessorManager(_packetHandler, EnumHelper<ClientPacketID>.BitsRequired);
            m.Stats.EnableFileOutput(ContentPaths.Build.Root.Join("netstats_in" + EngineSettings.DataFileSuffix));
            _messageProcessorManager = m;
#else
            _messageProcessorManager = new MessageProcessorManager(_packetHandler, EnumHelper<ClientPacketID>.BitsRequired);
#endif
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
            var maxConnsPerIP = ServerSettings.Default.MaxConnectionsPerIP;
            if (maxConnsPerIP > 0)
            {
                var connsFromIP = Connections.Count(x => x.IP == ip);
                if (connsFromIP >= maxConnsPerIP)
                    return GameMessageHelper.AsString(GameMessage.DisconnectTooManyConnectionsFromIP);
            }

            return base.AcceptConnect(ip, port, data);
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional configuring of the <see cref="NetPeerConfiguration"/> instance
        /// that will be used for this <see cref="ServerSocketManager"/>.
        /// </summary>
        protected override void InitNetPeerConfig(NetPeerConfiguration config)
        {
            base.InitNetPeerConfig(config);

            config.PingInterval = CommonConfig.PingInterval;
            config.ConnectionTimeout = CommonConfig.ConnectionTimeout;

            // Settings unique to the server (not set on the client)
            config.MaximumConnections = ServerSettings.Default.MaxConnections;
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling received data from an <see cref="IIPSocket"/>.
        /// </summary>
        /// <param name="sender">The <see cref="IIPSocket"/> that the data came from.</param>
        /// <param name="data">The data that was received.</param>
        protected override void OnReceiveData(IIPSocket sender, BitStream data)
        {
            base.OnReceiveData(sender, data);

            try
            {
                // Process the data
                _messageProcessorManager.Process(sender, data);
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to process received data from `{0}`. Exception: {1}";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, sender, ex);
                Debug.Fail(string.Format(errmsg, sender, ex));
            }
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
                    using (var pw = ServerPacket.SetGameTime(DateTime.Now))
                    {
                        sender.Send(pw, ServerMessageType.GUI);
                    }
                    break;
            }
        }
    }
}