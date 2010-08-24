using System;
using System.Collections.Generic;
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

        readonly ITCPSocket _tcpSocket;
        readonly IUDPSocket _udpSocket;

        bool _disposed = false;
        ushort? _remoteUDPPort = null;
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

            _udpSocket = udpSocket;

            _tcpSocket = tcpSocket;
            _tcpSocket.Disposed += TCPSocket_Disposed;
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
            var ipAddress = IPAddress.Parse(address);

            if (ipAddress == null)
                throw new ArgumentException("address");

            var endPoint = new IPEndPoint(ipAddress, port);
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

        /// <summary>
        /// Handles when the <see cref="TCPSocket"/> is disposed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        void TCPSocket_Disposed(TCPSocket sender)
        {
            Dispose();
        }

        #region IIPSocket Members

        /// <summary>
        /// Notifies listeners when this <see cref="IIPSocket"/> has been disposed.
        /// </summary>
        public event IIPSocketEventHandler Disposed;

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
        /// Gets the time that this <see cref="IIPSocket"/> was created.
        /// </summary>
        public TickCount TimeCreated
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

            if (_tcpSocket != null && !_tcpSocket.IsDisposed)
                _tcpSocket.Dispose();

            if (Disposed != null)
                Disposed(this);
        }

        /// <summary>
        /// Gets the queue of received data. This only includes data from a connection-oriented socket. Data from a connectionless protocol
        /// is not included, even if it was the owner of this <see cref="IIPSocket"/> that sent the data over a connectionless protocol.
        /// communication.
        /// </summary>
        /// <returns>Queue of received reliable data if any, or null if no queued data.</returns>
        public byte[][] GetRecvData()
        {
            var fromTCP = TCPSocket.GetRecvData();
            return fromTCP;
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
            if (!_remoteUDPPort.HasValue)
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
                _udpEndPoint = CreateEndPoint(Address.Split(':')[0], _remoteUDPPort.Value);

            UDPSocket.Send(data, data.Length, _udpEndPoint);
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
            if (!_remoteUDPPort.HasValue)
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
                _udpEndPoint = CreateEndPoint(Address.Split(':')[0], _remoteUDPPort.Value);

            UDPSocket.Send(data.GetBuffer(), data.Length, _udpEndPoint);
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
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="port"/> must be between <see cref="ushort.MinValue"/>
        /// and <see cref="ushort.MaxValue"/>.</exception>
        public void SetRemoteUnreliablePort(int port)
        {
            if (port < ushort.MinValue || port > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("port", "Value must be between ushort.MinValue and ushort.MaxValue.");

            _remoteUDPPort = (ushort)port;
        }

        #endregion
    }
}