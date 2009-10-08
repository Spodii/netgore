using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using log4net;
using NetGore;

namespace NetGore.Network
{
    /// <summary>
    /// A single, basic, thread-safe socket that uses UDP. Each UDPSocket will both send and listen on the
    /// same port it is created on.
    /// </summary>
    public class UDPSocket : IDisposable
    {
        /// <summary>
        /// Length of the maximum packet size in bytes.
        /// </summary>
        public const int MaxPacketSize = 1024 - _headerSize;

        /// <summary>
        /// Length of the custom packet header in bytes.
        /// </summary>
        const int _headerSize = 0;

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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

        /// <summary>
        /// The port used by this UDPSocket.
        /// </summary>
        int _port;

        /// <summary>
        /// UDPSocket constructor.
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
            _socket.BeginReceiveFrom(_receiveBuffer, 0, _receiveBuffer.Length, SocketFlags.None, ref _bindEndPoint,
                                     ReceiveFromCallback, this);
        }

        /// <summary>
        /// Binds the <see cref="UDPSocket"/> to a random available port.
        /// </summary>
        /// <returns>Port that the <see cref="UDPSocket"/> bound to.</returns>
        public int Bind()
        {
            return Bind(0);
        }

        /// <summary>
        /// Binds the <see cref="UDPSocket"/> to the <paramref name="port"/>.
        /// </summary>
        /// <param name="port">Port to bind to.</param>
        /// <returns>Port that the <see cref="UDPSocket"/> bound to.</returns>
        public int Bind(int port)
        {
            // NOTE: This will probably crash if Bind has already been called

            // Close down the old connection
            if (_socket.IsBound)
                _socket.Disconnect(true);

            // Bind
            _bindEndPoint = new IPEndPoint(IPAddress.Any, port);
            _socket.Bind(_bindEndPoint);

            // Get the port
            EndPoint endPoint = _socket.LocalEndPoint;
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
        /// Gets the queued data received by this UDPSocket.
        /// </summary>
        /// <returns>The queued data received by this UDPSocket, or null if empty.</returns>
        public AddressedPacket[] GetRecvData()
        {
            lock (_receiveQueue)
            {
                int length = _receiveQueue.Count;
                if (length == 0)
                    return null;

                var packets = _receiveQueue.ToArray();
                _receiveQueue.Clear();
                return packets;
            }
        }

        /// <summary>
        /// Callback for ReceiveFrom.
        /// </summary>
        /// <param name="result">Async result.</param>
        void ReceiveFromCallback(IAsyncResult result)
        {
            byte[] received = null;
            EndPoint remoteEndPoint = new IPEndPoint(0, 0);

            try
            {
                // Read the received data and put it into a temporary buffer
                int bytesRead = _socket.EndReceiveFrom(result, ref remoteEndPoint);
                received = new byte[bytesRead];
                Buffer.BlockCopy(_receiveBuffer, 0, received, 0, bytesRead);

                if (log.IsDebugEnabled)
                    log.DebugFormat("Received {0} bytes from {1}", bytesRead, remoteEndPoint);
            }
            catch (SocketException e)
            {
                if (log.IsErrorEnabled)
                    log.Error(e);
                Debug.Fail(e.ToString());
            }
            finally
            {
                // Start receiving again
                BeginReceiveFrom();
            }

            // Push the received data into the receive queue
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            if (received != null)
                // ReSharper restore ConditionIsAlwaysTrueOrFalse
            {
                lock (_receiveQueue)
                    _receiveQueue.Enqueue(new AddressedPacket(received, remoteEndPoint));
            }
        }

        /// <summary>
        /// Sends data to the specified <paramref name="endPoint"/>.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="length">Length of the data to send in bytes.</param>
        /// <param name="endPoint">EndPoint to send the data to.</param>
        public void Send(byte[] data, int length, EndPoint endPoint)
        {
            if (endPoint == null)
                throw new ArgumentNullException("endPoint");
            if (data == null || data.Length == 0)
                throw new ArgumentNullException("data");
            if (length > MaxPacketSize)
                throw new ArgumentOutOfRangeException("data", "Data is too large to send.");

            _socket.SendTo(data, length + _headerSize, SocketFlags.None, endPoint);

            if (log.IsDebugEnabled)
                log.DebugFormat("Sent `{0}` bytes to `{1}`", length, endPoint);
        }

        /// <summary>
        /// Sends data to the specified <paramref name="endPoint"/>.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="endPoint">EndPoint to send the data to.</param>
        public void Send(byte[] data, EndPoint endPoint)
        {
            Send(data, data.Length, endPoint);
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (_socket != null)
                _socket.Close();
        }

        #endregion
    }
}