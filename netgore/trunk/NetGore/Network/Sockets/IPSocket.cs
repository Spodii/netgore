using System;
using System.Linq;
using System.Reflection;
using Lidgren.Network;
using log4net;
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
        readonly NetConnection _conn;
        readonly TickCount _timeCreated;

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
            // TODO: !! Use a pool
            ret = new IPSocket(conn);
            conn.Tag = ret;

            return ret;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IPSocket"/> class.
        /// </summary>
        /// <param name="conn">The <see cref="NetConnection"/> that this <see cref="IPSocket"/> is for.</param>
        IPSocket(NetConnection conn)
        {
            _timeCreated = TickCount.Now;

            _conn = conn;

            var addressBytes = _conn.RemoteEndpoint.Address.GetAddressBytes();
            _address = IPAddressHelper.IPv4AddressToUInt(addressBytes, 0);
        }

        void Send(BitStream data, NetDeliveryMethod method)
        {
            var msg = _conn.Owner.CreateMessage();
            data.CopyTo(msg);
            _conn.SendMessage(msg, method);
        }

        void Send(byte[] data, NetDeliveryMethod method)
        {
            var msg = _conn.Owner.CreateMessage();
            msg.Write(data);
            _conn.SendMessage(msg, method);
        }

        #region IIPSocket Members

        /// <summary>
        /// Gets the IPv4 address and port that this IIPSocket is connected to as a string. This string is formatted
        /// as "xxx.xxx.xxx.xxx:yyyyy". Trailing 0's from each segment are omitted.
        /// </summary>
        public string Address
        {
            get { return IPAddressHelper.ToIPv4Address(_address) + ":" + _conn.RemoteEndpoint.Port; }
        }

        /// <summary>
        /// Gets the status of the <see cref="IIPSocket"/>'s connection.
        /// </summary>
        public NetConnectionStatus Status
        {
            get { return _conn.Status; }
        }

        /// <summary>
        /// Gets the average round-time trip latency of this connection.
        /// </summary>
        public float AverageLatency
        {
            get { return _conn.AverageRoundtripTime; }
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
            get { return (ushort)_conn.RemoteEndpoint.Port; }
        }

        /// <summary>
        /// Gets or sets the optional tag used to identify the socket or hold additional information. This tag
        /// is not used in any way by the <see cref="IIPSocket"/> itself.
        /// </summary>
        public object Tag
        {
            get;set;
        }

        /// <summary>
        /// Gets the time that this <see cref="IIPSocket"/> was created.
        /// </summary>
        public TickCount TimeCreated
        {
            get { return _timeCreated; }
        }

        /// <summary>
        /// Sends data over a reliable stream.
        /// </summary>
        /// <param name="data">Data to send.</param>
        public void Send(BitStream data)
        {
            Send(data, NetDeliveryMethod.ReliableOrdered);
        }

        /// <summary>
        /// Sends data over a stream.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="reliable">If true, the data is guarenteed to be received completely and in order. If false,
        /// the data may be received out of order, or not at all. All data is guarenteed to be received in full if
        /// it is received.</param>
        public void Send(BitStream data, bool reliable)
        {
            if (reliable)
                Send(data, NetDeliveryMethod.ReliableOrdered);
            else
                Send(data, NetDeliveryMethod.Unreliable);
        }

        /// <summary>
        /// Sends data over the reliable stream.
        /// </summary>
        /// <param name="data">Data to send.</param>
        public void Send(byte[] data)
        {
            Send(data, NetDeliveryMethod.ReliableOrdered);
        }

        /// <summary>
        /// Sends data over a stream.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="reliable">If true, the data is guarenteed to be received completely and in order. If false,
        /// the data may be received out of order, or not at all. All data is guarenteed to be received in full if
        /// it is received.</param>
        public void Send(byte[] data, bool reliable)
        {
            if (reliable)
                Send(data, NetDeliveryMethod.ReliableOrdered);
            else
            Send(data, NetDeliveryMethod.Unreliable);
        }

        /// <summary>
        /// Terminates this connection.
        /// </summary>
        public void Disconnect()
        {
            _conn.Disconnect(string.Empty);
        }

        #endregion
    }
}