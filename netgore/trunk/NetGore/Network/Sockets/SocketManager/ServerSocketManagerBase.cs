using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Lidgren.Network;
using log4net;
using NetGore.IO;

namespace NetGore.Network
{
    public class ServerSocketManagerBase : IServerSocketManager
    {
        // TODO: !! Implement support for limiting the number of connections from a single IP

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly NetServer _local;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerSocketManagerBase"/> class.
        /// </summary>
        /// <param name="appIdentifier">The application identifier string.</param>
        /// <param name="port">The port to listen on.</param>
        public ServerSocketManagerBase(string appIdentifier, int port)
        {
            var config = new NetPeerConfiguration(appIdentifier) { AcceptIncomingConnections = true, Port = port };
            _local = new NetServer(config);
            _local.Start();
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
                BitStream bs;

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

                        // Get the connection message and put it in a BitStream
                        // TODO: !! Optimize this to not generate garbage, possibly by always using the same BitStream to hold the data
                        bs = new BitStream(BitStreamMode.Write, 64);
                        bs.Write(incMsg);
                        bs.Mode = BitStreamMode.Read;

                        // Ask the acception handler method if this connection will be accepted
                        bool approved = AcceptConnect(ipSocket.IP, ipSocket.Port, bs);

                        if (log.IsDebugEnabled)
                            log.DebugFormat("Received connection request from `{0}`. Accepted? {1}.", ipSocket, approved);

                        // Approve or deny the connection accordingly
                        if (approved)
                            incMsg.SenderConnection.Approve();
                        else
                            incMsg.SenderConnection.Deny(string.Empty); // TODO: !! Format into a GameMessage and utilize the reason string

                        break;

                    case NetIncomingMessageType.StatusChanged:

                        // Make sure that the IIPSocket has been created
                        ipSocket = IPSocket.Create(incMsg.SenderConnection);
                        Debug.Assert(ipSocket != null);

                        // Read the status and reason
                        NetConnectionStatus status = (NetConnectionStatus)incMsg.ReadByte();
                        string reason = incMsg.ReadString();

                        if (log.IsDebugEnabled)
                            log.DebugFormat("Socket `{0}` status changed to `{1}`. Reason: {2}", ipSocket, status, reason);

                        // Forward to the handler
                        OnReceiveStatusChanged(ipSocket, status, reason);

                        break;

                    case NetIncomingMessageType.Data:

                        // Get the IIPSocket for the connection (it definitely should be created by this point)
                        ipSocket = (IIPSocket)incMsg.SenderConnection.Tag;
                        Debug.Assert(ipSocket != null);

                        // Read the received data and place it into a BitStream
                        // TODO: !! Optimize this to not generate garbage, possibly by always using the same BitStream to hold the data
                        bs = new BitStream(BitStreamMode.Write, 64);
                        bs.Write(incMsg);
                        bs.Mode = BitStreamMode.Read;

                        if (log.IsDebugEnabled)
                            log.DebugFormat("Received {0} bits from {1}.", incMsg.LengthBits, ipSocket);

                        // Forward the data to the data handler
                        OnReceiveData(ipSocket, bs);

                        break;
                }

                _local.Recycle(incMsg);
            }
        }

        /// <summary>
        /// Determines whether or not a connection request should be accepted.
        /// </summary>
        /// <param name="ip">The IPv4 address the remote connection is coming from.</param>
        /// <param name="port">The port that the remote connection is coming from.</param>
        /// <param name="data">The data sent along with the connection request.</param>
        /// <returns>
        /// True if the connection should be accepted; otherwise false.
        /// </returns>
        protected virtual bool AcceptConnect(uint ip, ushort port, BitStream data)
        {
            // Always accept connections by default
            return true;
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
        /// Gets the number of live connections to the server.
        /// </summary>
        public int ConnectionsCount
        {
            get { return _local.ConnectionsCount; }
        }

        /// <summary>
        /// Gets the live connections to the server.
        /// </summary>
        public IEnumerable<IIPSocket> Connections
        {
            get { return _local.Connections.Select(x => x.Tag).OfType<IIPSocket>(); }
        }
    }
}