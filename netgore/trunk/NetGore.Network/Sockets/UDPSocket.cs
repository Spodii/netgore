using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace NetGore.Network
{
    /// <summary>
    /// A single, basic, thread-safe socket that uses UDP. Each UDPSocket will both send and listen on the
    /// same port it is created on.
    /// </summary>
    public class UDPSocket : IDisposable
    {
        /// <summary>
        /// Length of the custom packet header in bytes.
        /// </summary>
        const int _headerSize = 0;

        /// <summary>
        /// Length of the maximum packet size in bytes.
        /// </summary>
        const int _maxPacketSize = 1024 - _headerSize;

        /// <summary>
        /// The port used by this UDPSocket.
        /// </summary>
        int _port;

        /// <summary>
        /// Buffer for receiving data.
        /// </summary>
        readonly byte[] _receiveBuffer;

        /// <summary>
        /// Queue of received data that has yet to be handled.
        /// </summary>
        readonly Queue<byte[]> _receiveQueue = new Queue<byte[]>(2);

        /// <summary>
        /// Buffer for sending data.
        /// </summary>
        readonly byte[] _sendBuffer;

        /// <summary>
        /// Underlying Socket used by this UDPSocket.
        /// </summary>
        readonly Socket _socket;

        /// <summary>
        /// EndPoint the Socket binded to.
        /// </summary>
        EndPoint _bindEndPoint;

        /// <summary>
        /// EndPoint for the last packet received.
        /// </summary>
        EndPoint _remoteEndPoint = new IPEndPoint(0, 0);

        /// <summary>
        /// Gets the port used by this UDPSocket.
        /// </summary>
        public int Port
        {
            get { return _port; }
        }

        /// <summary>
        /// UDPSocket constructor.
        /// </summary>
        public UDPSocket()
        {
            _sendBuffer = new byte[_maxPacketSize + _headerSize];
            _receiveBuffer = new byte[_maxPacketSize + _headerSize];
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }

        /// <summary>
        /// UDPSocket constructor.
        /// </summary>
        /// <param name="port">The port used by this UDPSocket.</param>
        public UDPSocket(int port) : this()
        {
            SetPort(port);
        }

        /// <summary>
        /// Prefixes the header to a packet.
        /// </summary>
        /// <param name="data">Packet to add the header to.</param>
        /// <param name="length">Length of the packet in bytes.</param>
        /// <returns>Byte array containing the <paramref name="data"/> with the packet header prefixed.</returns>
        byte[] AddHeader(byte[] data, ushort length)
        {
            // No headers needed right now
            return data;
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
        /// Gets the EndPoint for an IP address and port.
        /// </summary>
        /// <param name="address">IP address to get the EndPoint for.</param>
        /// <param name="port">Port to get the EndPoint for.</param>
        /// <returns>The EndPoint for an IP address and port.</returns>
        public static EndPoint GetEndPoint(string address, int port)
        {
            IPAddress ipAddress = IPAddress.Parse(address);

            if (ipAddress == null)
                throw new ArgumentException("address");

            IPEndPoint endPoint = new IPEndPoint(ipAddress, port);
            return endPoint;
        }

        /// <summary>
        /// Gets the EndPoint for an IP address.
        /// </summary>
        /// <param name="address">IP address to get the EndPoint for.</param>
        /// <returns>The EndPoint for an IP address.</returns>
        public EndPoint GetEndPoint(string address)
        {
            return GetEndPoint(address, Port);
        }

        /// <summary>
        /// Gets the queued data received by this UDPSocket.
        /// </summary>
        /// <returns>The queued data received by this UDPSocket, or null if empty.</returns>
        public byte[][] GetRecvData()
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
        /// Changes the Port for this UDPSocket.
        /// </summary>
        /// <param name="newPort">New port.</param>
        public void SetPort(int newPort)
        {
            if (Port == newPort)
                return;

            // Check for a valid range
            if (newPort < IPEndPoint.MinPort || newPort > IPEndPoint.MaxPort)
                throw new ArgumentOutOfRangeException("newPort");

            _port = newPort;

            // Close down the old connection
            if (_socket.IsBound)
                _socket.Disconnect(true);

            // Bind to the new port
            _bindEndPoint = new IPEndPoint(IPAddress.Any, Port);
            _socket.Bind(_bindEndPoint);

            // Begin receiving again
            BeginReceiveFrom();
        }

        /// <summary>
        /// Callback for ReceiveFrom.
        /// </summary>
        /// <param name="result">Async result.</param>
        void ReceiveFromCallback(IAsyncResult result)
        {
            byte[] received = null;

            try
            {
                // Read the received data and put it into a temporary buffer
                int bytesRead = _socket.EndReceiveFrom(result, ref _remoteEndPoint);
                received = new byte[bytesRead];
                Buffer.BlockCopy(_receiveBuffer, 0, received, 0, bytesRead);
            }
            catch (SocketException e)
            {
                // TODO: Handle
            }
            finally
            {
                // Start receiving again
                BeginReceiveFrom();
            }

            // Push the received data into the receive queue
            if (received != null)
            {
                lock (_receiveQueue)
                    _receiveQueue.Enqueue(received);
            }
            else
            {
                // TODO: Failed to receive error message
            }
        }

        /// <summary>
        /// Sends data to the specified <paramref name="address"/>.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="address">Address to send the data to.</param>
        /// <returns>EndPoint for the <paramref name="address"/> so it can be reused later instead of
        /// being parsed every Send.</returns>
        public EndPoint Send(byte[] data, string address)
        {
            EndPoint endPoint = GetEndPoint(address);
            Send(data, endPoint);
            return endPoint;
        }

        /// <summary>
        /// Sends data to the specified <paramref name="endPoint"/>.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="length">Length of the data to send in bytes.</param>
        /// <param name="endPoint">EndPoint to send the data to.</param>
        public void Send(byte[] data, ushort length, EndPoint endPoint)
        {
            if (endPoint == null)
                throw new ArgumentNullException("endPoint");
            if (data == null || data.Length == 0)
                throw new ArgumentNullException("data");
            if (data.Length > _maxPacketSize)
                throw new ArgumentOutOfRangeException("data", "Data is too large to send.");

            data = AddHeader(data, length);
            _socket.SendTo(data, data.Length + _headerSize, SocketFlags.None, endPoint);
        }

        /// <summary>
        /// Sends data to the specified <paramref name="endPoint"/>.
        /// </summary>
        /// <param name="data">Data to send.</param>
        /// <param name="endPoint">EndPoint to send the data to.</param>
        public void Send(byte[] data, EndPoint endPoint)
        {
            Send(data, (ushort)data.Length, endPoint);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (_socket != null)
                _socket.Close();
        }
    }
}