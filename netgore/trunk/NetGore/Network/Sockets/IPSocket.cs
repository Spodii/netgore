using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using log4net;
using NetGore.IO;

namespace NetGore.Network
{
    /// <summary>
    /// A socket that can send data over both a reliable and unreliable connection to the same destination.
    /// </summary>
    public class IPSocket : IIPSocket
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Value given to the remote UDP port when it has not been set.
        /// </summary>
        const int _unsetRemoteUDPPortValue = 0;

        readonly ITCPSocket _tcpSocket;
        readonly IUDPSocket _udpSocket;

        bool _disposed = false;
        int _remoteUDPPort = _unsetRemoteUDPPortValue;
        EndPoint _udpEndPoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="IPSocket"/> class.
        /// </summary>
        /// <param name="tcpSocket">The TCP socket.</param>
        /// <param name="udpSocket">The UDP socket.</param>
        public IPSocket(ITCPSocket tcpSocket, IUDPSocket udpSocket)
        {
            if (tcpSocket == null)
                throw new ArgumentNullException("tcpSocket");
            if (udpSocket == null)
                throw new ArgumentNullException("udpSocket");

            _tcpSocket = tcpSocket;
            _udpSocket = udpSocket;

            _tcpSocket.Tag = this;
        }

        /// <summary>
        /// Gets the <see cref="ITCPSocket"/> used in this <see cref="IPSocket"/>.
        /// </summary>
        internal ITCPSocket TCPSocket
        {
            get { return _tcpSocket; }
        }

        /// <summary>
        /// Gets the <see cref="IUDPSocket"/> used in this <see cref="IPSocket"/>.
        /// </summary>
        internal IUDPSocket UDPSocket
        {
            get { return _udpSocket; }
        }

        /// <summary>
        /// Creates an EndPoint.
        /// </summary>
        /// <param name="address">IP address for the EndPoint.</param>
        /// <param name="port">Port for the EndPoint.</param>
        /// <returns>An EndPoint with the given <paramref name="address"/> and <paramref name="port"/>.</returns>
        static EndPoint CreateEndPoint(string address, int port)
        {
            IPAddress ipAddress = IPAddress.Parse(address);

            if (ipAddress == null)
                throw new ArgumentException("address");

            IPEndPoint endPoint = new IPEndPoint(ipAddress, port);
            return endPoint;
        }

        /// <summary>
        /// Joins two arrays together.
        /// </summary>
        /// <typeparam name="T">Type of the arrays to join.</typeparam>
        /// <param name="first">First array to join.</param>
        /// <param name="second">Second array to join.</param>
        /// <returns>The two arrays joined together.</returns>
        static T[] JoinArrays<T>(T[] first, T[] second)
        {
            var joined = new T[first.Length + second.Length];
            first.CopyTo(joined, 0);
            second.CopyTo(joined, first.Length);
            return joined;
        }

        #region IIPSocket Members

        /// <summary>
        /// Gets the IPv4 address and port that this IIPSocket is connected to as a string. This string is formatted
        /// as "xxx.xxx.xxx.xxx:yyyyy". Trailing 0's from each segment are omitted.
        /// </summary>
        public string Address
        {
            get { return _tcpSocket.Address; }
        }

        /// <summary>
        /// Gets the IPv4 address as an unsigned 32-bit integer.
        /// </summary>
        public uint IP
        {
            get { return _tcpSocket.IP; }
        }

        /// <summary>
        /// Gets if this IIPSocket is currently connected.
        /// </summary>
        public bool IsConnected
        {
            get { return _tcpSocket.IsConnected; }
        }

        /// <summary>
        /// Gets the maximum size of a message to the reliable channel.
        /// </summary>
        public int MaxReliableMessageSize
        {
            get { return TCPSocket.MaxSendSize; }
        }

        /// <summary>
        /// Gets the maximum size of a message to the unreliable channel.
        /// </summary>
        public int MaxUnreliableMessageSize
        {
            get { return UDPSocket.MaxSendSize; }
        }

        /// <summary>
        /// Gets the port as a 16-bit unsigned integer.
        /// </summary>
        public ushort Port
        {
            get { return _tcpSocket.Port; }
        }

        /// <summary>
        /// Gets or sets the optional tag used to identify the socket or hold additional information. This tag
        /// is not used in any way by the IIPSocket itself.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Gets the time that this IIPSocket was created.
        /// </summary>
        public int TimeCreated
        {
            get { return _tcpSocket.TimeCreated; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            if (_tcpSocket != null)
                _tcpSocket.Dispose();
        }

        /// <summary>
        /// Gets the queue of received data.
        /// </summary>
        /// <returns>Queue of received data if any, or null if no queued data.</returns>
        public byte[][] GetRecvData()
        {
            // Get the messages from both sockets
            var fromTCP = TCPSocket.GetRecvData();

            var udpData = UDPSocket.GetRecvData();
            byte[][] fromUDP;
            if (udpData == null)
                fromUDP = null;
            else
                fromUDP = udpData.Select(x => x.Data).ToArray();

            // If they're both null, return null
            if (fromTCP == null && fromUDP == null)
                return null;

            // If they're both not null, we have to join them together
            // If only one is null, we can just return that one
            if (fromTCP == null)
                return fromUDP;
            else
            {
                if (fromUDP != null)
                    return JoinArrays(fromUDP, fromTCP);
                else
                    return fromTCP;
            }
        }

        /// <summary>
        /// Sends data over a reliable stream.
        /// </summary>
        /// <param name="data">Data to send.</param>
        public void Send(BitStream data)
        {
            Send(data, true);
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
            // Send reliable data over TCP
            if (reliable)
            {
                _tcpSocket.Send(data);
                return;
            }

            // If the remote UDP port hasn't been set yet, we can still fall back on TCP at least instead of just
            // dropping the send completely
            if (_remoteUDPPort == _unsetRemoteUDPPortValue)
            {
                const string errmsg = "Tried sending over unreliable stream before setting the port.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                Send(data, true);
                return;
            }

            // Create the EndPoint if it has not already been created
            if (_udpEndPoint == null)
                _udpEndPoint = CreateEndPoint(Address.Split(':')[0], _remoteUDPPort);

            _udpSocket.Send(data, data.Length, _udpEndPoint);
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
            // Send reliable data over TCP
            if (reliable)
            {
                _tcpSocket.Send(data);
                return;
            }

            // If the remote UDP port hasn't been set yet, we can still fall back on TCP at least instead of just
            // dropping the send completely
            if (_remoteUDPPort == _unsetRemoteUDPPortValue)
            {
                const string errmsg = "Tried sending over unreliable stream before setting the port.";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                Send(data, true);
                return;
            }

            // Create the EndPoint if it has not already been created
            if (_udpEndPoint == null)
                _udpEndPoint = CreateEndPoint(Address.Split(':')[0], _remoteUDPPort);

            _udpSocket.Send(data.GetBuffer(), data.Length, _udpEndPoint);
        }

        /// <summary>
        /// Sends data over the reliable stream.
        /// </summary>
        /// <param name="data">Data to send.</param>
        public void Send(byte[] data)
        {
            Send(data, true);
        }

        /// <summary>
        /// Sets the port used to communicate with the remote connection over an unreliable stream.
        /// </summary>
        /// <param name="port">Port for the unreliable stream.</param>
        public void SetRemoteUnreliablePort(int port)
        {
            _remoteUDPPort = port;
        }

        #endregion
    }
}