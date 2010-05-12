using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using log4net;

namespace NetGore.Network
{
    /// <summary>
    /// A single, basic, thread-safe socket that uses UDP. Each <see cref="UDPSocket"/> will both send and listen
    /// on the same port it is created on.
    /// </summary>
    public class UDPSocket : IUDPSocket
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Length of the maximum packet size in bytes.
        /// </summary>
        public const int MaxPacketSize = 1024 - _headerSize;

        /// <summary>
        /// Length of the custom packet header in bytes.
        /// </summary>
        const int _headerSize = 0;

        static readonly byte[] _defaultConnectData = new byte[] { 0 };

        /// <summary>
        /// Local cache of the global <see cref="NetStats"/> instance.
        /// </summary>
        static readonly NetStats _netStats = NetStats.Global;

        /// <summary>
        /// Buffer for receiving data.
        /// </summary>
        readonly byte[] _receiveBuffer;

        /// <summary>
        /// Queue of received data that has yet to be handled.
        /// </summary>
        readonly Queue<AddressedPacket> _receiveQueue = new Queue<AddressedPacket>(2);

        /// <summary>
        /// Underlying Socket used by this UDPSocket.
        /// </summary>
        readonly Socket _socket;

        /// <summary>
        /// EndPoint the Socket binded to.
        /// </summary>
        EndPoint _bindEndPoint;

        bool _disposed = false;

        /// <summary>
        /// The port used by this UDPSocket.
        /// </summary>
        int _port;

        /// <summary>
        /// Initializes a new instance of the <see cref="UDPSocket"/> class.
        /// </summary>
        public UDPSocket()
        {
            _receiveBuffer = new byte[MaxPacketSize + _headerSize];
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        /// <summary>
        /// Starts the receiving.
        /// </summary>
        void BeginReceiveFrom()
        {
            try
            {
                _socket.BeginReceiveFrom(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, ref _bindEndPoint,
                                         ReceiveFromCallback, this);
            }
            catch (ObjectDisposedException ex)
            {
                Debug.Fail(ex.ToString());
                Dispose();
                return;
            }
        }

        /// <summary>
        /// Callback for <see cref="Socket.BeginReceiveFrom"/>.
        /// </summary>
        /// <param name="result">Async result.</param>
        void ReceiveFromCallback(IAsyncResult result)
        {
            byte[] received;
            EndPoint remoteEndPoint = new IPEndPoint(0, 0);

            int bytesRead;
            try
            {
                // Read the received data and put it into a temporary buffer
                bytesRead = _socket.EndReceiveFrom(result, ref remoteEndPoint);
                received = new byte[bytesRead];
                Buffer.BlockCopy(_receiveBuffer, 0, received, 0, bytesRead);
            }
            catch (ObjectDisposedException)
            {
                Dispose();
                return;
            }
            catch (SocketException e)
            {
                if (log.IsErrorEnabled)
                    log.Error(e);

                Dispose();
                return;
            }
            finally
            {
                // Start receiving again
                if (!IsDisposed)
                    BeginReceiveFrom();
            }

            _netStats.AddUDPRecv(bytesRead);
            _netStats.IncrementUDPReceives();

            // Push the received data into the receive queue
            var addressedPacket = new AddressedPacket(received, remoteEndPoint);
            lock (_receiveQueue)
                _receiveQueue.Enqueue(addressedPacket);
        }

        #region IUDPSocket Members

        /// <summary>
        /// Notifies listeners when the <see cref="ITCPSocket"/> has been disposed.
        /// </summary>
        public event UDPSocketEventHandler Disposed;

        /// <summary>
        /// Gets if this object has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _disposed; }
        }

        /// <summary>
        /// Gets the maximum size of the data that can be sent in a single send.
        /// </summary>
        int IUDPSocket.MaxSendSize
        {
            get { return MaxPacketSize; }
        }

        /// <summary>
        /// Binds the <see cref="IUDPSocket"/> to a random available port.
        /// </summary>
        /// <returns>
        /// Port that the <see cref="IUDPSocket"/> binded to.
        /// </returns>
        public int Bind()
        {
            return Bind(0);
        }

        /// <summary>
        /// Binds the <see cref="IUDPSocket"/> to the <paramref name="port"/>.
        /// </summary>
        /// <param name="port">Port to bind to.</param>
        /// <returns>
        /// Port that the <see cref="IUDPSocket"/> binded to.
        /// </returns>
        public int Bind(int port)
        {
            // TODO: This will probably crash if Bind has already been called

            // Close down the old connection
            if (_socket.IsBound)
                _socket.Disconnect(true);

            // Bind
            _bindEndPoint = new IPEndPoint(IPAddress.Any, port);
            _socket.Bind(_bindEndPoint);

            // Get the port
            var endPoint = _socket.LocalEndPoint;
            if (endPoint == null)
            {
                const string errmsg = "Failed to bind the UDPSocket!";
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg);
                Debug.Fail(errmsg);
                throw new Exception(errmsg);
            }
            _port = ((IPEndPoint)endPoint).Port;

            // Begin receiving
            BeginReceiveFrom();

            return _port;
        }

        /// <summary>
        /// Connects the <see cref="IUDPSocket"/> to the specified host.
        /// </summary>
        /// <param name="host">The host to connect to.</param>
        public void Connect(EndPoint host)
        {
            Connect(host, _defaultConnectData);
        }

        /// <summary>
        /// Connects the <see cref="IUDPSocket"/> to the specified host.
        /// </summary>
        /// <param name="host">The host to connect to.</param>
        /// <param name="data">The initial data to send.</param>
        public void Connect(EndPoint host, byte[] data)
        {
            if (_bindEndPoint == null)
                _bindEndPoint = new IPEndPoint(0, 0);

            Send(data, host);

            BeginReceiveFrom();
        }

        /// <summary>
        /// Connects the <see cref="IUDPSocket"/> to the specified host.
        /// </summary>
        /// <param name="ip">The IP address of the host to connect to.</param>
        /// <param name="port">The port of the host to connect to.</param>
        public void Connect(string ip, int port)
        {
            Connect(ip, port, _defaultConnectData);
        }

        /// <summary>
        /// Connects the <see cref="IUDPSocket"/> to the specified host.
        /// </summary>
        /// <param name="ip">The IP address of the host to connect to.</param>
        /// <param name="port">The port of the host to connect to.</param>
        /// <param name="data">The initial data to send.</param>
        public void Connect(string ip, int port, byte[] data)
        {
            var hostAddresses = Dns.GetHostAddresses(ip);
            var hostAddr = hostAddresses.FirstOrDefault();

            if (hostAddr == null)
                throw new ArgumentException(string.Format("Could not find any host addresses for ip `{0}`.", ip));

            var ep = new IPEndPoint(hostAddr, port);
            Connect(ep, data);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            if (_socket != null)
                _socket.Close();

            if (Disposed != null)
                Disposed(this);

            _receiveQueue.Clear();
        }

        /// <summary>
        /// Gets the queued data received by this <see cref="IUDPSocket"/>.
        /// </summary>
        /// <returns>
        /// The queued data received by this <see cref="IUDPSocket"/>, or null if empty.
        /// </returns>
        public IEnumerable<AddressedPacket> GetRecvData()
        {
            lock (_receiveQueue)
            {
                var length = _receiveQueue.Count;
                if (length == 0)
                    return null;

                var packets = _receiveQueue.ToArray();
                _receiveQueue.Clear();
                return packets;
            }
        }

        /// <summary>
        /// Sends data to the specified <paramref name="endPoint"/>.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="length">Length of the data to send in bytes.</param>
        /// <param name="endPoint">The <see cref="EndPoint"/> to send the data to.</param>
        public void Send(byte[] data, int length, EndPoint endPoint)
        {
            if (endPoint == null)
                throw new ArgumentNullException("endPoint");
            if (data == null || data.Length == 0)
                throw new ArgumentNullException("data");
            if (length > MaxPacketSize)
                throw new ArgumentOutOfRangeException("data", "Data is too large to send.");

            if (log.IsDebugEnabled)
                log.DebugFormat("Send `{0}` bytes to `{1}`", data.Length, endPoint);

            // Get the total length of the packet to send (message + header)
            var totalLength = length + _headerSize;

            try
            {
                _socket.SendTo(data, totalLength, SocketFlags.None, endPoint);
            }
            catch (ObjectDisposedException ex)
            {
                Debug.Fail(ex.ToString());
                Dispose();
                return;
            }

            _netStats.AddUDPSent(totalLength);
            _netStats.IncrementUDPSends();
        }

        /// <summary>
        /// Sends data to the specified <paramref name="endPoint"/>.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="endPoint">The <see cref="EndPoint"/> to send the data to.</param>
        public void Send(byte[] data, EndPoint endPoint)
        {
            Send(data, data.Length, endPoint);
        }

        #endregion
    }
}