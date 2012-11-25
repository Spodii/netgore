using System.Linq;
using Lidgren.Network;
using NetGore.IO;

namespace NetGore.Network
{
    /// <summary>
    /// A connection made over IP that can be used to communicate with and control the connection with the
    /// other end.
    /// </summary>
    public class IPSocket : IIPSocket
    {
        readonly uint _address;
        readonly string _addressStr;
        readonly NetConnection _conn;
        readonly TickCount _timeCreated;

        /// <summary>
        /// Initializes a new instance of the <see cref="IPSocket"/> class.
        /// </summary>
        /// <param name="conn">The <see cref="NetConnection"/> that this <see cref="IPSocket"/> is for.</param>
        IPSocket(NetConnection conn)
        {
            _timeCreated = TickCount.Now;

            _conn = conn;

            var addressBytes = _conn.RemoteEndPoint.Address.GetAddressBytes();
            _address = IPAddressHelper.IPv4AddressToUInt(addressBytes, 0);

            _addressStr = IPAddressHelper.ToIPv4Address(_address) + ":" + _conn.RemoteEndPoint.Port;
        }

        /// <summary>
        /// Creates an <see cref="IPSocket"/> for a <see cref="NetConnection"/> and stores it in the
        /// <see cref="NetConnection.Tag"/> property. If the <see cref="IPSocket"/> for the given <see cref="NetConnection"/>
        /// already exists, a new one will not be created and the existing one will be returned.
        /// </summary>
        /// <param name="conn">The <see cref="NetConnection"/> to create the <see cref="IPSocket"/> for.</param>
        /// <returns>The <see cref="IPSocket"/> for the <paramref name="conn"/>.</returns>
        public static IPSocket Create(NetConnection conn)
        {
            // Check for an IPSocket already attached
            var ret = conn.Tag as IPSocket;
            if (ret != null)
                return ret;

            // Create the IPSocket instance and attach it to the tag
            ret = new IPSocket(conn);
            conn.Tag = ret;

            return ret;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return "IPSocket - " + Address;
        }

        #region IIPSocket Members

        /// <summary>
        /// Gets the IPv4 address and port that this IIPSocket is connected to as a string. This string is formatted
        /// as "xxx.xxx.xxx.xxx:yyyyy". Leading 0's from each segment are omitted.
        /// </summary>
        /// <example>27.0.1.160:12345</example>
        public string Address
        {
            get { return _addressStr; }
        }

        /// <summary>
        /// Gets the average round-time trip latency of this connection.
        /// </summary>
        public float AverageLatency
        {
            get { return _conn.AverageRoundtripTime * 1000f; }
        }

        /// <summary>
        /// Gets the IPv4 address as an unsigned 32-bit integer.
        /// </summary>
        public uint IP
        {
            get { return _address; }
        }

        /// <summary>
        /// Gets if this <see cref="IIPSocket"/> is currently connected.
        /// </summary>
        public bool IsConnected
        {
            get { return _conn.Status == NetConnectionStatus.Connected; }
        }

        /// <summary>
        /// Gets the port as a 16-bit unsigned integer.
        /// </summary>
        public ushort Port
        {
            get { return (ushort)_conn.RemoteEndPoint.Port; }
        }

        /// <summary>
        /// Gets the status of the <see cref="IIPSocket"/>'s connection.
        /// </summary>
        public NetConnectionStatus Status
        {
            get { return _conn.Status; }
        }

        /// <summary>
        /// Gets or sets the optional tag used to identify the socket or hold additional information. This tag
        /// is not used in any way by the <see cref="IIPSocket"/> itself.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Gets the time that this <see cref="IIPSocket"/> was created.
        /// </summary>
        public TickCount TimeCreated
        {
            get { return _timeCreated; }
        }

        /// <summary>
        /// Terminates this connection.
        /// </summary>
        /// <param name="reason">A string containing the reason why the connection was terminated. Can be null or empty, but
        /// recommended that a reason is provided.</param>
        public void Disconnect(string reason)
        {
            _conn.Disconnect(reason);
        }

        /// <summary>
        /// Sends data to the other end of the connection.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="deliveryMethod">The method to use to deliver the message. This determines how reliability, ordering,
        /// and sequencing will be handled.</param>
        /// <param name="sequenceChannel">The sequence channel to use to deliver the message. Only valid when
        /// <paramref name="deliveryMethod"/> is not equal to <see cref="NetDeliveryMethod.Unreliable"/> or
        /// <see cref="NetDeliveryMethod.ReliableUnordered"/>. Must also be a value greater than or equal to 0 and
        /// less than <see cref="NetConstants.NetChannelsPerDeliveryMethod"/>.</param>
        /// <returns>
        /// True if the <paramref name="data"/> was successfully enqueued for sending; otherwise false.
        /// </returns>
        /// <exception cref="NetException"><paramref name="deliveryMethod"/> equals <see cref="NetDeliveryMethod.Unreliable"/>
        /// or <see cref="NetDeliveryMethod.ReliableUnordered"/> and <paramref name="sequenceChannel"/> is non-zero.</exception>
        /// <exception cref="NetException"><paramref name="sequenceChannel"/> is less than 0 or greater than or equal to
        /// <see cref="NetConstants.NetChannelsPerDeliveryMethod"/>.</exception>
        /// <exception cref="NetException"><paramref name="deliveryMethod"/> equals <see cref="NetDeliveryMethod.Unknown"/>.</exception>
        public bool Send(BitStream data, NetDeliveryMethod deliveryMethod, int sequenceChannel = 0)
        {
            if (data == null || data.Length == 0)
                return false;

            var msg = SocketHelper.GetNetOutgoingMessage(_conn.Peer, data.LengthBytes);
            data.CopyTo(msg);

            var ret = _conn.SendMessage(msg, deliveryMethod, sequenceChannel);
            return ret == NetSendResult.Sent || ret == NetSendResult.Queued;
        }

        /// <summary>
        /// Sends data to the other end of the connection.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="deliveryMethod">The method to use to deliver the message. This determines how reliability, ordering,
        /// and sequencing will be handled.</param>
        /// <param name="sequenceChannel">The sequence channel to use to deliver the message. Only valid when
        /// <paramref name="deliveryMethod"/> is not equal to <see cref="NetDeliveryMethod.Unreliable"/> or
        /// <see cref="NetDeliveryMethod.ReliableUnordered"/>. Must also be a value between 0 and
        /// <see cref="NetConstants.NetChannelsPerDeliveryMethod"/>.</param>
        /// <returns>
        /// True if the <paramref name="data"/> was successfully enqueued for sending; otherwise false.
        /// </returns>
        /// <exception cref="NetException"><paramref name="deliveryMethod"/> equals <see cref="NetDeliveryMethod.Unreliable"/>
        /// or <see cref="NetDeliveryMethod.ReliableUnordered"/> and <paramref name="sequenceChannel"/> is non-zero.</exception>
        /// <exception cref="NetException"><paramref name="sequenceChannel"/> is less than 0 or greater than
        /// <see cref="NetConstants.NetChannelsPerDeliveryMethod"/>.</exception>
        /// <exception cref="NetException"><paramref name="deliveryMethod"/> equals <see cref="NetDeliveryMethod.Unknown"/>.</exception>
        public bool Send(byte[] data, NetDeliveryMethod deliveryMethod, int sequenceChannel = 0)
        {
            if (data == null || data.Length == 0)
                return false;

            var msg = SocketHelper.GetNetOutgoingMessage(_conn.Peer, data.Length);
            msg.Write(data);

            var ret = _conn.SendMessage(msg, deliveryMethod, sequenceChannel);
            return ret == NetSendResult.Sent || ret == NetSendResult.Queued;
        }

        #endregion
    }
}