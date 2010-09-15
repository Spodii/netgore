using System;
using System.Diagnostics;
using System.Reflection;
using Lidgren.Network;
using log4net;
using NetGore.IO;

namespace NetGore.Network
{
    /// <summary>
    /// A general implementation of a manager for the sockets on the client.
    /// </summary>
    public class ClientSocketManager : IClientSocketManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly NetClient _local;

        IIPSocket _remote;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSocketManager"/> class.
        /// </summary>
        /// <param name="appIdentifier">The application identifier string.</param>
        public ClientSocketManager(string appIdentifier)
        {
            var config = new NetPeerConfiguration(appIdentifier) { AcceptIncomingConnections = false };

            // Disable throttling
            config.ThrottleBytesPerSecond = 0;
            config.ThrottlePeakBytes = 0;

            // Disable some message types that will not be used by the client
            config.DisableMessageType(NetIncomingMessageType.NatIntroductionSuccess);
            config.DisableMessageType(NetIncomingMessageType.Receipt);
            config.DisableMessageType(NetIncomingMessageType.UnconnectedData);
            config.DisableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.DisableMessageType(NetIncomingMessageType.DiscoveryResponse);

            // Custom configuration
            InitNetPeerConfig(config);

            // Start
            _local = new NetClient(config);
            _local.Start();
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional configuring of the <see cref="NetPeerConfiguration"/> instance
        /// that will be used for this <see cref="ClientSocketManager"/>.
        /// </summary>
        protected virtual void InitNetPeerConfig(NetPeerConfiguration config)
        {
        }

        /// <summary>
        /// When overridden in the derived class, parses the string sent from the server containing the reason why we
        /// were disconnected. The message is specified when the server calls <see cref="IIPSocket.Disconnect"/>
        /// with parameters.
        /// </summary>
        /// <param name="msg">The message that was sent containing the reason for disconnecting. Can be empty if the
        /// server gave no reason.</param>
        /// <returns>The parsed <paramref name="msg"/>, or the original <paramref name="msg"/> if no known way to parse
        /// it was found.</returns>
        protected virtual string ParseCustomDisconnectMessage(string msg)
        {
            // No parsing support by default
            return msg;
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

                Debug.Assert(RemoteSocket == null || incMsg.SenderConnection == null || incMsg.SenderConnection.Tag == RemoteSocket, "Why did we get a message from a different connection?");

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

                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)incMsg.ReadByte();
                        ipSocket = (IIPSocket)incMsg.SenderConnection.Tag;

                        string reason = incMsg.ReadString();

                        // Set the ClientDisconnected value based on the status
                        switch (status)
                        {
                            case NetConnectionStatus.Connecting:
                            case NetConnectionStatus.Connected:
                            case NetConnectionStatus.None:
                                // Reset the status
                                _clientDisconnected = false;
                                break;

                            case NetConnectionStatus.Disconnecting:
                                // Disconnecting only is set when we are the ones disconnecting
                                _clientDisconnected = true;
                                break;
                        }

                        // Handle parsing a custom disconnect message
                        if (status == NetConnectionStatus.Disconnected)
                            reason = ParseCustomDisconnectMessage(reason);

                        // Don't let them set it to null
                        if (reason == null)
                            reason = string.Empty;

                        if (log.IsDebugEnabled)
                            log.DebugFormat("Socket `{0}` status changed to `{1}`. Reason: {2}", ipSocket, status, reason);

                        OnReceiveStatusChanged(ipSocket, status, reason);

                        if (StatusChanged != null)
                            StatusChanged(this, status, reason);

                        break;

                    case NetIncomingMessageType.Data:
                        ipSocket = (IIPSocket)incMsg.SenderConnection.Tag;

                        if (log.IsDebugEnabled)
                            log.DebugFormat("Received {0} bits from {1}.", incMsg.LengthBits, ipSocket);

                        // Copy the received data into a BitStream before passing it up
                        _receiveBitStream.Reset();
                        _receiveBitStream.Write(incMsg);
                        _receiveBitStream.PositionBits = 0;

                        OnReceiveData(ipSocket, _receiveBitStream);
                        break;
                }

                _local.Recycle(incMsg);
            }
        }

        /// <summary>
        /// The <see cref="BitStream"/> instance used for when passing data up to be processed.
        /// </summary>
        readonly BitStream _receiveBitStream = new BitStream(1024);

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

        /// <summary>
        /// Gets the <see cref="IIPSocket"/> used to communicate with the other end of the connection. Will be null if no
        /// connection has been established.
        /// </summary>
        public IIPSocket RemoteSocket
        {
            get { return _remote; }
        }

        bool _clientDisconnected = false;

        /// <summary>
        /// Gets if it was us, the client, who terminated the connection to the server. This will only be true when
        /// the client is in the process of disconnecting or has disconnected, and will always be false when establishing
        /// a connection or connected.
        /// </summary>
        public bool ClientDisconnected
        {
            get { return _clientDisconnected; }
        }

        /// <summary>
        /// Gets if we are currently connected to the server.
        /// </summary>
        public bool IsConnected
        {
            get { var l = _remote;
                return l != null && l.IsConnected; }
        }

        /// <summary>
        /// Notifies listeners when the status of the connection has changed.
        /// </summary>
        public event ClientSocketManagerStatusChangedEventHandler StatusChanged;

        /// <summary>
        /// Attempts to connects to the server.
        /// </summary>
        /// <param name="host">The server address.</param>
        /// <param name="port">The server port.</param>
        /// <param name="approvalMessage">The initial message to send to the server when making the connection. This can
        /// be utilized by the server to provide additional information for determining whether or not to accept the connection.
        /// Or, can be null to send nothing.</param>
        /// <returns>
        /// True if the connection attempt was successfully started. Does not mean that the connection was established, but
        /// just that it can be attempted. Will return false if a connection is already established or being established.
        /// </returns>
        public bool Connect(string host, int port, BitStream approvalMessage = null)
        {
            if (_local.ConnectionsCount > 0)
                return false;

            NetConnection conn;

            if (approvalMessage != null && approvalMessage.LengthBits > 0)
            {
                // Connect with approval message
                var netOutMsg = _local.CreateMessage();
                approvalMessage.CopyTo(netOutMsg);

                conn = _local.Connect(host, port, netOutMsg);
            }
            else
            {
                // Connect without approval message
                conn = _local.Connect(host, port);
            }

            // Check if connection successful
            if (conn == null)
                return false;

            // Store the remote connection as an IPSocket
            _remote = IPSocket.Create(conn);
           
            return true;
        }

        /// <summary>
        /// Disconnects from the server.
        /// </summary>
        public void Disconnect()
        {
            _local.Disconnect(string.Empty);
        }
    }
}