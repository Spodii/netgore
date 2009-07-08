using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using log4net;
using NetGore.IO;

namespace NetGore.Network
{
    /// <summary>
    /// Delegate for TCPSocketEvents
    /// </summary>
    /// <param name="sender">Sender TCPSocket</param>
    public delegate void TCPSocketEvent(TCPSocket sender);

    /// <summary>
    /// Delegate for TCPSocketEvents
    /// </summary>
    /// <typeparam name="T">Event args type</typeparam>
    /// <param name="sender">Sender TCPSocket</param>
    /// <param name="e">Event args</param>
    public delegate void TCPSocketEvent<T>(TCPSocket sender, T e);

    /// <summary>
    /// A single, thread-safe socket that uses TCP with the ability to send and receive information
    /// asynchronously. Contains an internal buffer/queue for both sending and receiving data so
    /// there is no need to buffer I/O calls. Due to the usage of an internal 2-byte header that is added
    /// to every internal Send (not to be confused with every call to Send()), this must be added to any
    /// other socket before being able to communicate with this socket. To send messages to this TCPSocket through
    /// a custom socket, include a ushort in front of every message block indicating the size of the message in
    /// bytes, not including the 2-byte header.
    /// </summary>
    public class TCPSocket : ITCPSocket
    {
        /// <summary>
        /// Maximum receive size
        /// </summary>
        public const int MaxRecvSize = 2048 - SizeHeaderLength;

        /// <summary>
        /// Maximum send size
        /// </summary>
        public const int MaxSendSize = 2048 - SizeHeaderLength;

        /// <summary>
        /// Size of the message length header in bytes
        /// </summary>
        public const int SizeHeaderLength = 2;

        /// <summary>
        /// Initial size of the receive queue 
        /// </summary>
        const int RecvQueueStartSize = 4;

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Object used to lock receives
        /// </summary>
        readonly object _recvLock = new object();

        /// <summary>
        /// Object used to lock sends
        /// </summary>
        readonly object _sendLock = new object();

        readonly SocketSendQueue _sendQueue = new SocketSendQueue(MaxSendSize);

        readonly int _timeCreated = Environment.TickCount;

        /// <summary>
        /// Remote address the socket is connected to
        /// </summary>
        string _address;

        /// <summary>
        /// If the TCPSocket is disposed
        /// </summary>
        bool _disposed;

        /// <summary>
        /// If the socket has been initialized
        /// </summary>
        bool _isInitialized = false;

        /// <summary>
        /// If the socket is currently busy sending data
        /// </summary>
        bool _isSending;

        /// <summary>
        /// Buffer used for receiving data. Data is written into this buffer
        /// when received. Any complete messages are written out into the
        /// receive queue while incomplete data will remain in the buffer.
        /// </summary>
        byte[] _recvBuffer;

        /// <summary>
        /// Current position of the receive buffer. Holds the first unused index of
        /// the receive buffer.
        /// </summary>
        int _recvBufferPos = 0;

        /// <summary>
        /// SocketAsyncEventArgs for receive events (same one used for every receive)
        /// </summary>
        SocketAsyncEventArgs _recvEventArgs;

        /// <summary>
        /// Queue of all the received messages that have not yet been handled
        /// </summary>
        Queue<byte[]> _recvQueue;

        /// <summary>
        /// Buffer for sending data. Memory is never cleared, so if a message is being sent
        /// then this will contain that data. If not, this will contain the last data sent.
        /// </summary>
        byte[] _sendBuffer;

        /// <summary>
        /// SocketAsyncEventArgs for send events (same one used for every send)
        /// </summary>
        SocketAsyncEventArgs _sendEventArgs;

        /// <summary>
        /// Socket used for the connection
        /// </summary>
        Socket _socket;

        /// <summary>
        /// Gets the Environment.TickCount of when the TCPSocket was created.
        /// </summary>
        public int TimeCreated
        {
            get { return _timeCreated; }
        }

        /// <summary>
        /// Wrapper for the socket BeginReceive method
        /// </summary>
        void BeginRecv()
        {
            if (log.IsInfoEnabled)
                log.InfoFormat("Beginning receive from {0}", Address);

            try
            {
                // Set up the event args
                _recvEventArgs.SetBuffer(_recvBufferPos, _recvBuffer.Length - _recvBufferPos);

                // Send, making sure the EndRecv is triggered even if it finishes non-async
                if (!_socket.ReceiveAsync(_recvEventArgs))
                    EndRecv(this, _recvEventArgs);
            }
            catch (Exception ex)
            {
                if (log.IsFatalEnabled)
                    log.Fatal("Failed to begin receive", ex);
                Debug.Fail("Failed to begin receive. Reason: " + ex);
                Dispose();
            }
        }

        /// <summary>
        /// Wrapper for the socket BeginSend method. When this is called, make sure _isSending is true!
        /// </summary>
        void BeginSend(byte[] dataToSend)
        {
#if DEBUG
            // Ensure _isSending was set properly... just in case
            bool debugIsSending;
            lock (_sendLock)
            {
                debugIsSending = _isSending;
            }

            if (!debugIsSending)
            {
                const string errmsg = "Called BeginSend() while _isSending == False. This should never happen!";
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg);
                Debug.Fail(errmsg);
                _isSending = false;
                return;
            }
#endif

            if (_socket == null)
            {
                const string errmsg = "BeginSend() failed since the socket is null (this.Disposed == `{0}`).";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, _disposed);
                lock (_sendLock)
                    _isSending = false;
                return;
            }

            if (dataToSend == null)
            {
                const string errmsg = "Sending failed since there is no message!";
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                Debug.Fail(errmsg);
                lock (_sendLock)
                    _isSending = false;
                return;
            }

            int msgLength = dataToSend.Length;

            // Copy over the contents of the message into the send buffer, along with the
            // 2 byte prefix that states the length of the message
            Buffer.BlockCopy(dataToSend, 0, _sendBuffer, SizeHeaderLength, msgLength);
            _sendBuffer[0] = (byte)((msgLength >> 8) & 255);
            _sendBuffer[1] = (byte)(msgLength & 255);

            // From now on, we will include the header size into the message length
            msgLength += SizeHeaderLength;

            try
            {
                // Update the size of the packet to send
                _sendEventArgs.SetBuffer(0, msgLength);

                // Send, making sure the EndSend is triggered even if it finishes non-async
                if (!_socket.SendAsync(_sendEventArgs))
                    EndSend(this, _sendEventArgs);
            }
            catch (Exception ex)
            {
                const string errmsg = "Failed to begin send. Socket will now close.";
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg, ex);
                Debug.Fail(errmsg + ex);
                Dispose();
                return;
            }

            if (log.IsInfoEnabled)
                log.InfoFormat("Beginning send of `{0}` bytes to `{1}`", msgLength, Address);

            if (OnSend != null)
                OnSend(this, msgLength);
        }

        /// <summary>
        /// Attempts to synchronously connect to a host
        /// </summary>
        /// <param name="host">Host address</param>
        /// <param name="port">Host port</param>
        /// <returns>True if the connection was successful, else false</returns>
        public bool Connect(string host, int port)
        {
            // Make sure we are not already connected
            if (IsConnected)
                throw new SocketException();

            try
            {
                // Attempt to synchronously connect to the host 
                var ips = Dns.GetHostAddresses(host);
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.Connect(ips[0], port);

                if (log.IsInfoEnabled)
                    log.InfoFormat("Connected to host at {0}:{1}", host, port);

                return true;
            }
            catch (Exception ex)
            {
                // Only a moderate problem since its likely the host is just down or invalid
                // and a connect failing should be a problem always handled
                if (log.IsWarnEnabled)
                    log.Warn("Failed to connect.", ex);

                return false;
            }
        }

        /// <summary>
        /// Ends an async receive and starts receiving again
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">SocketAsyncEventArgs</param>
        void EndRecv(object sender, SocketAsyncEventArgs e)
        {
            // Read from the socket
            int readLength = e.BytesTransferred;

            // 0 receive length = gracefully closed
            if (readLength == 0)
            {
                Dispose();
                return;
            }

            _recvBufferPos += readLength;

            if (log.IsInfoEnabled)
                log.InfoFormat("Received {0} bytes from {1}", readLength, Address);

            // Since data was added to the receive buffer we will need to check for new complete messages
            ProcessRecvBuffer();

            // We can start receiving again since we are done playing with the receive buffer
            if (_socket.Connected)
            {
                e.SetBuffer(_recvBufferPos, MaxRecvSize); // Inconsistant size with the other SetBuffer for recv
                if (!_socket.ReceiveAsync(e))
                    EndRecv(this, e);
            }
        }

        /// <summary>
        /// Ends an async send and starts a new send if theres data queued
        /// </summary>
        /// <param name="sender">Object the event came from</param>
        /// <param name="e">Event args</param>
        /// <returns>Length of the data sent</returns>
        void EndSend(object sender, SocketAsyncEventArgs e)
        {
#if DEBUG
            lock (_sendLock)
            {
                Debug.Assert(_isSending, "How the hell did EndSend() get called with _isSending = false? This is bad...");
            }
#endif

            // _isSending should be true, and this should be the only place that it can be set to false (except for when
            // BeginSend() fails, but BeginSend() should never be called until this sets _isSending to false anyways), so
            // we can assume it is safe to avoid locking since _isSending should NOT change

            // Check if we can begin sending again right away
            var dataToSend = _sendQueue.Dequeue();
            if (dataToSend != null)
            {
                // Data was enqueued, so we can send it
                BeginSend(dataToSend);
            }
            else
            {
                // Nothing enqueued or not allowed to send again, so do not start sending again
                _isSending = false;
            }
        }

        /// <summary>
        /// Creates the objects for the connection and allows use of it. Call
        /// only once a connection has been confirmed to be valid.
        /// </summary>
        internal void Initialize()
        {
            if (_isInitialized)
            {
                Debug.Fail("TCPSocket already initialized.");
                return;
            }

            _isInitialized = true;

            _socket.LingerState = new LingerOption(true, 2);
            _socket.NoDelay = true;
            _recvQueue = new Queue<byte[]>(RecvQueueStartSize);

            _recvBuffer = new byte[(MaxRecvSize + SizeHeaderLength) * 2];
            _sendBuffer = new byte[MaxSendSize + SizeHeaderLength];
            _socket.ReceiveBufferSize = MaxRecvSize + SizeHeaderLength;
            _socket.SendBufferSize = MaxSendSize + SizeHeaderLength;

            // Make the send/recv event args
            _sendEventArgs = new SocketAsyncEventArgs { UserToken = this, SocketFlags = SocketFlags.None };
            _sendEventArgs.SetBuffer(_sendBuffer, 0, 0);
            _sendEventArgs.Completed += EndSend;

            _recvEventArgs = new SocketAsyncEventArgs { UserToken = this, SocketFlags = SocketFlags.None };
            _recvEventArgs.SetBuffer(_recvBuffer, 0, MaxRecvSize);
            _recvEventArgs.Completed += EndRecv;

            // Start receiving right away
            BeginRecv();
        }

        /// <summary>
        /// Breaks apart all of the completed messages from the receive buffer and adds them
        /// to the receive message queue so they can be handled by the program, leaving the
        /// incomplete nessages in the buffer until completed
        /// </summary>
        void ProcessRecvBuffer()
        {
            int offset = 0;

            lock (_recvLock)
            {
                // Start at the start of the buffer and loop until either an incomplete
                // message is found or all of the buffer has been read
                while (offset < _recvBufferPos)
                {
                    // Grab the message length (significant byte first, insignificant second)
                    ushort msgLen = (ushort)((_recvBuffer[offset] << 8) | _recvBuffer[offset + 1]);

                    // Check for a valid length
                    if (msgLen == 0)
                    {
                        const string errmsg = "Received message with length of 0. This should never happen...";
                        if (log.IsFatalEnabled)
                            log.Fatal(errmsg);
                        Debug.Fail(errmsg);
                        break;
                    }

                    // Increase the offset to compensate for the size header
                    offset += 2;

                    // Check if theres enough data left to read to complete the message
                    if (msgLen <= _recvBufferPos - offset)
                    {
                        // A complete message was found so add it to the receive queue
                        var msg = new byte[msgLen];
                        Buffer.BlockCopy(_recvBuffer, offset, msg, 0, msgLen);
                        _recvQueue.Enqueue(msg);
                        offset += msgLen;
                    }
                    else
                    {
                        // Incomplete message
                        offset -= 2; // Decrease the offset from the size so we don't chop it off below
                        break;
                    }
                }

                // If the whole buffer was flushed (no incomplete messages) just move the write pointer
                if (offset == _recvBufferPos)
                    _recvBufferPos = 0;
                else if (offset > 0)
                {
                    // If there was an incomplete message, shift the buffer contents down,
                    // placing the first incomplete message at the first index
                    int tmpLen = _recvBufferPos - offset;
                    Buffer.BlockCopy(_recvBuffer, offset, _recvBuffer, 0, tmpLen);

                    // Move the receive buffer write pointer to the next free index
                    _recvBufferPos = tmpLen;
                }
            }
        }

        /// <summary>
        /// Sets the socket to be used by the connection
        /// </summary>
        /// <param name="newSocket">Socket to be used</param>
        internal void SetSocket(Socket newSocket)
        {
            if (newSocket == null)
            {
                Debug.Fail("newSocket is null.");
                return;
            }

            _socket = newSocket;
            _address = _socket.RemoteEndPoint.ToString();
        }

        /// <summary>
        /// Returns a System.String that represents the current TCPSocket.
        /// </summary>
        /// <returns>A System.String that represents the current TCPSocket.</returns>
        public override string ToString()
        {
            if (Tag != null)
                return string.Format("TCPSocket on {0} [tag: {1}]", Address, Tag);
            else
                return "TCPSocket on " + Address;
        }

        #region ITCPSocket Members

        /// <summary>
        /// Gets or sets the optional tag used to identify the socket or hold additional information. This tag
        /// is not used in any way by the TCPSocket itself.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Gets the remote endpoint address. Use this instead of Socket.RemoteEndPoint since
        /// this will be accessable even after the socket is terminated.
        /// </summary>
        public string Address
        {
            get { return _address; }
        }

        /// <summary>
        /// Gets if the socket is currently connected
        /// </summary>
        public bool IsConnected
        {
            get { return (_socket != null && _socket.Connected); }
        }

        /// <summary>
        /// Notifies the listeners when the TCPSocket has been disposed.
        /// </summary>
        public event TCPSocketEvent OnDispose;

        /// <summary>
        /// Notifies the listeners when the socket has successfully sent data, and how much data was sent.
        /// Because the TCPSocket attempts to combine queued sends, OnSend may not be raised for every send.
        /// </summary>
        public event TCPSocketEvent<int> OnSend;

        /// <summary>
        /// Gets the queue of complete received data
        /// </summary>
        /// <returns>Queue of received data if any, or null if no queued data</returns>
        public byte[][] GetRecvData()
        {
            byte[][] ret;

            if (_disposed)
                return null;

            if (!_isInitialized)
                throw new MethodAccessException("Called GetRecvData() before Initialize().");

            lock (_recvLock)
            {
                if (_recvQueue.Count == 0)
                {
                    // Nothing received, return null
                    ret = null;
                }
                else
                {
                    // Copy the items in the queue over to a new queue (to return), then clear the old queue
                    ret = _recvQueue.ToArray();
                    _recvQueue.Clear();
                }
            }

            return ret;
        }

        /// <summary>
        /// Asynchronously sends data to the socket.
        /// </summary>
        /// <param name="sourceStream">BitStream containing the data to send. The data in this BitStream will
        /// be copied to an internal BitStream, so after the Send call returns, the <paramref name="sourceStream"/>
        /// will be safe from the TCPSocket's control.</param>
        public void Send(BitStream sourceStream)
        {
            if (!_isInitialized && !_disposed)
                throw new Exception("Called Send() before Initialize()");

            bool willSend;

            // Determine if we will send the data now, or enqueue it for later
            lock (_sendLock)
            {
                if (_isSending)
                    willSend = false;
                else
                {
                    willSend = true;
                    _isSending = true; // We must set to true now while we have the lock
                }
            }

            // Actually send the data or enqueue it
            if (!willSend)
                _sendQueue.Enqueue(sourceStream);
            else
                BeginSend(sourceStream.GetBufferCopy());
        }

        /// <summary>
        /// Disposes all resources used by the TCPSocket.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;

            // Close down the socket
            if (_socket != null)
            {
                try
                {
                    _socket.Shutdown(SocketShutdown.Both);
                    _socket.Close();
                }
                catch (ObjectDisposedException)
                {
                }
                finally
                {
                    _socket = null;
                }
            }

            // Clear out the queues
            lock (_recvLock)
            {
                if (_recvQueue != null)
                    _recvQueue.Clear();
            }

            _isInitialized = false;

            if (OnDispose != null)
                OnDispose(this);
        }

        #endregion
    }
}