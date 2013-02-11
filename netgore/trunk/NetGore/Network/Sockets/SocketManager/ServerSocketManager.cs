using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Lidgren.Network;
using log4net;
using NetGore.IO;

namespace NetGore.Network
{
    /// <summary>
    /// A general implementation of a manager for the sockets on the server.
    /// </summary>
    public class ServerSocketManager : IServerSocketManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly NetServer _local;

        /// <summary>
        /// The <see cref="BitStream"/> instance used for when passing data up to be processed.
        /// </summary>
        readonly BitStream _receiveBitStream = new BitStream(1024);

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerSocketManager"/> class.
        /// </summary>
        /// <param name="appIdentifier">The application identifier string.</param>
        /// <param name="port">The port to listen on.</param>
        public ServerSocketManager(string appIdentifier, int port)
        {
            var config = new NetPeerConfiguration(appIdentifier)
            { AcceptIncomingConnections = true, Port = port, MaximumConnections = 50 };

            // Disable some message types that will not be used by the server
            config.DisableMessageType(NetIncomingMessageType.NatIntroductionSuccess);
            config.DisableMessageType(NetIncomingMessageType.Receipt);
            config.DisableMessageType(NetIncomingMessageType.UnconnectedData);
            config.DisableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.DisableMessageType(NetIncomingMessageType.DiscoveryResponse);

            // Manually handle connection approval instead of just accepting everything
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            // Custom configuration
            InitNetPeerConfig(config);

            // Start
            _local = new NetServer(config);
            _local.Start();
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
        protected virtual string AcceptConnect(uint ip, ushort port, BitStream data)
        {
            // Always accept connections by default
            return null;
        }

       
        public NetPeerStatistics Statistics
        {
            get { return _local.Statistics; }
        }

        /// <summary>
        /// Gets the <see cref="NetServer"/> instance.
        /// Avoid using this object directly when possible.
        /// </summary>
        /// <returns>The <see cref="NetServer"/> instance.</returns>
        public NetServer GetNetServer()
        {
            return _local;
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional configuring of the <see cref="NetPeerConfiguration"/> instance
        /// that will be used for this <see cref="ServerSocketManager"/>.
        /// </summary>
        protected virtual void InitNetPeerConfig(NetPeerConfiguration config)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling received data from an <see cref="IIPSocket"/>.
        /// </summary>
        /// <param name="sender">The <see cref="IIPSocket"/> that the data came from.</param>
        /// <param name="data">The data that was received. This <see cref="BitStream"/> instance is reused internally, so it
        /// is vital that you do NOT hold a reference to it when this method returns. This should be no problem since you should
        /// not be holding onto raw received data anyways, but if you must, you can always make a deep copy.</param>
        protected virtual void OnReceiveData(IIPSocket sender, BitStream data)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling when the status of an <see cref="IIPSocket"/> changes.
        /// </summary>
        /// <param name="sender">The <see cref="IIPSocket"/> who's status has changed.</param>
        /// <param name="status">The new status.</param>
        /// <param name="reason">The reason for the status change.</param>
        protected virtual void OnReceiveStatusChanged(IIPSocket sender, NetConnectionStatus status, string reason)
        {
        }

        #region IServerSocketManager Members

        /// <summary>
        /// Gets the live connections to the server.
        /// </summary>
        public IEnumerable<IIPSocket> Connections
        {
            get { return _local.Connections.Select(x => x.Tag).OfType<IIPSocket>(); }
        }

        /// <summary>
        /// Gets the number of live connections to the server.
        /// </summary>
        public int ConnectionsCount
        {
            get { return _local.ConnectionsCount; }
        }

        /// <summary>
        /// Handles processing of the underlying connection(s) and promoting data to the upper layer to be handled
        /// by the application. Should be called once per frame.
        /// </summary>
        public void Heartbeat()
        {
            const string debugMsgFormat = "Debug message from `{0}` on `{1}`: {2}";

            NetIncomingMessage incMsg;
            while ((incMsg = _local.ReadMessage()) != null)
            {
                IIPSocket ipSocket;

                switch (incMsg.MessageType)
                {
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.VerboseDebugMessage:
                        if (log.IsDebugEnabled)
                        {
                            var debugMsg = incMsg.ReadString();
                            log.DebugFormat(debugMsgFormat, incMsg.SenderConnection, this, debugMsg);
                        }
                        break;

                    case NetIncomingMessageType.WarningMessage:
                        if (log.IsWarnEnabled)
                        {
                            var debugMsg = incMsg.ReadString();
                            log.WarnFormat(debugMsgFormat, incMsg.SenderConnection, this, debugMsg);
                        }
                        break;

                    case NetIncomingMessageType.ErrorMessage:
                        if (log.IsErrorEnabled)
                        {
                            var debugMsg = incMsg.ReadString();
                            log.ErrorFormat(debugMsgFormat, incMsg.SenderConnection, this, debugMsg);
                        }
                        break;

                    case NetIncomingMessageType.ConnectionApproval:

                        // Make sure that the IIPSocket has been created
                        ipSocket = IPSocket.Create(incMsg.SenderConnection);
                        Debug.Assert(ipSocket != null);

                        // Copy the received data into a BitStream before passing it up
                        _receiveBitStream.Reset();
                        _receiveBitStream.Write(incMsg);
                        _receiveBitStream.PositionBits = 0;

                        // Ask the acception handler method if this connection will be accepted
                        var rejectMessage = AcceptConnect(ipSocket.IP, ipSocket.Port, _receiveBitStream);

                        if (log.IsDebugEnabled)
                            log.DebugFormat("Received connection request from `{0}`. Accepted? {1}.", ipSocket,
                                string.IsNullOrEmpty(rejectMessage));

                        // Approve or deny the connection accordingly
                        if (string.IsNullOrEmpty(rejectMessage))
                            incMsg.SenderConnection.Approve();
                        else
                            incMsg.SenderConnection.Deny(rejectMessage);

                        break;

                    case NetIncomingMessageType.StatusChanged:

                        // Make sure that the IIPSocket has been created
                        ipSocket = IPSocket.Create(incMsg.SenderConnection);
                        Debug.Assert(ipSocket != null);

                        // Read the status and reason
                        var status = (NetConnectionStatus)incMsg.ReadByte();
                        var reason = incMsg.ReadString();

                        if (log.IsDebugEnabled)
                            log.DebugFormat("Socket `{0}` status changed to `{1}`. Reason: {2}", ipSocket, status, reason);

                        // Forward to the handler
                        OnReceiveStatusChanged(ipSocket, status, reason);

                        break;

                    case NetIncomingMessageType.Data:

                        // Get the IIPSocket for the connection (it definitely should be created by this point)
                        ipSocket = (IIPSocket)incMsg.SenderConnection.Tag;
                        Debug.Assert(ipSocket != null);

                        // Copy the received data into a BitStream before passing it up
                        _receiveBitStream.Reset();
                        _receiveBitStream.Write(incMsg);
                        _receiveBitStream.PositionBits = 0;

                        if (log.IsDebugEnabled)
                            log.DebugFormat("Received {0} bits from {1}.", incMsg.LengthBits, ipSocket);

                        // Forward the data to the data handler
                        OnReceiveData(ipSocket, _receiveBitStream);

                        break;
                }

                _local.Recycle(incMsg);
            }
        }

        /// <summary>
        /// Disconnects all active connections and rejects incoming connections.
        /// </summary>
        public void Shutdown()
        {
            if (_local != null)
                _local.Shutdown(string.Empty);
        }

        #endregion
    }
}