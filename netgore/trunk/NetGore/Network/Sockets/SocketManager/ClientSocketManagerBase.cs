using System.Diagnostics;
using System.Reflection;
using Lidgren.Network;
using log4net;
using NetGore.IO;

namespace NetGore.Network
{
    public class ClientSocketManagerBase : IClientSocketManager
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly NetClient _local;

        IIPSocket _remote;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientSocketManagerBase"/> class.
        /// </summary>
        /// <param name="appIdentifier">The application identifier string.</param>
        public ClientSocketManagerBase(string appIdentifier)
        {
            var config = new NetPeerConfiguration(appIdentifier) { AcceptIncomingConnections = true };
            _local = new NetClient(config);
            _local.Start();
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

                        // TODO: !! Optimize this to not generate garbage, possibly by always using the same BitStream to hold the data
                        BitStream bs = new BitStream(BitStreamMode.Write, 64);
                        bs.Write(incMsg);
                        bs.Mode = BitStreamMode.Read;

                        if (log.IsDebugEnabled)
                            log.DebugFormat("Received {0} bits from {1}.", incMsg.LengthBits, ipSocket);

                        OnReceiveData(ipSocket, bs);
                        break;
                }

                _local.Recycle(incMsg);
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling received data from an <see cref="IIPSocket"/>.
        /// </summary>
        /// <param name="sender">The <see cref="IIPSocket"/> that the data came from.</param>
        /// <param name="data">The data that was received.</param>
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