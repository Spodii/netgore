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
    /// Handles multiple connections from a <see cref="TCPSocket"/> for both server and client environments.
    /// </summary>
    public class SocketManager : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// List of all the current connections.
        /// </summary>
        readonly List<IPSocket> _connections = new List<IPSocket>();

        /// <summary>
        /// Lock used to synchronize the Connections list.
        /// </summary>
        readonly object _connectionsLock = new object();

        /// <summary>
        /// The UDPSocket used by all connections.
        /// </summary>
        readonly IUDPSocket _udpSocket = new UDPSocket();

        bool _disposed;

        /// <summary>
        /// Socket for accepting connections
        /// </summary>
        ListenSocket _listenSocket;

        /// <summary>
        /// Gets if we are currently listening for and accepting connections. Listening has to be set up by calling
        /// <see cref="SocketManager.Listen"/>.
        /// </summary>
        public bool IsListening { get { return _listenSocket != null && _listenSocket.IsAlive; } }

        /// <summary>
        /// Maximum allowed connections from a single IP address
        /// </summary>
        int _maxDupeIP = 3;

        /// <summary>
        /// Notifies listeners when a connection with Connect() failed to be made to a host.
        /// </summary>
        public event SocketManagerEventHandler ConnectFailed;

        /// <summary>
        /// Notifies listeners when a connection with <see cref="SocketManager.Connect"/> was successfully made with a host.
        /// </summary>
        public event SocketManagerSocketEventHandler Connected;

        /// <summary>
        /// Notifies listeners when a connection has been made on the listen socket. That is, someone else has connected
        /// with us.
        /// </summary>
        public event SocketManagerSocketEventHandler ConnectedFrom;

        /// <summary>
        /// Notifies listeners when a connection has been terminated.
        /// </summary>
        public event SocketManagerSocketEventHandler Disconnected;

        /// <summary>
        /// Gets an IEnumerable of all open and established connections.
        /// </summary>
        IEnumerable<IPSocket> Connections
        {
            get { return _connections; }
        }

        /// <summary>
        /// Finds all connections matching the given predicate.
        /// </summary>
        /// <param name="pred">The predicate for the connections to match.</param>
        /// <returns>The connections that match the given <paramref name="pred"/>.</returns>
        public IEnumerable<IIPSocket> FindConnections(Predicate<IIPSocket> pred)
        {
            List<IIPSocket> ret = new List<IIPSocket>();

            lock (_connectionsLock)
            {
                foreach (var conn in Connections)
                {
                    if (pred(conn))
                        ret.Add(conn);
                }
            }

            return ret;
        }

        /// <summary>
        /// Gets if the listen socket is correctly working and accepting connections.
        /// </summary>
        public bool IsAlive
        {
            get { return _listenSocket.IsAlive; }
        }

        /// <summary>
        /// Gets or sets the maximum number of duplicate IP addresses allowed before the connections get automatically rejected.
        /// </summary>
        public int MaxDupeIP
        {
            get { return _maxDupeIP; }
            set
            {
                if (_maxDupeIP == value)
                    return;

                _maxDupeIP = value;
                if (log.IsInfoEnabled)
                    log.InfoFormat("MaxDupeIP changed to {0}", _maxDupeIP);
            }
        }

        /// <summary>
        /// Creates a socket and connects it to the specified server.
        /// </summary>
        /// <param name="host">Host address (IP or domain name) to connect to.</param>
        /// <param name="port">Port number to connect to.</param>
        /// <returns>Socket used to create the connection or null if unsuccessful.</returns>
        public IIPSocket Connect(string host, int port)
        {
            // Create the socket to connect with
            var conn = new TCPSocket();
            if (log.IsInfoEnabled)
                log.InfoFormat("Connecting to host at {0}:{1}", host, port);

            IPSocket ipSocket = null;

            try
            {
                if (conn.Connect(host, port))
                {
                    // Create the connection and add it to the connections list
                    conn.Initialize();
                    conn.Disposed += SocketDisposeHandler;
                    ipSocket = new IPSocket(conn, _udpSocket);

                    lock (_connectionsLock)
                    {
                        _connections.Add(ipSocket);
                    }

                    // Notify listeners
                    OnConnected(ipSocket);

                    if (Connected != null)
                        Connected(this, ipSocket);
                }
                else
                {
                    if (log.IsInfoEnabled)
                        log.Info("Unable to connect to host");

                    OnConnectFailed();

                    if (ConnectFailed != null)
                        ConnectFailed(this);
                }
            }
            catch (SocketException ex)
            {
                if (log.IsWarnEnabled)
                    log.Warn("Unable to connect to host: {0}", ex);

                OnConnectFailed();

                if (ConnectFailed != null)
                    ConnectFailed(this);
            }

            return ipSocket;
        }

        /// <summary>
        /// Establishes the UDP connection for a client when requested by the server.
        /// </summary>
        /// <param name="host">The host to connect to.</param>
        /// <param name="port">The port to connect to.</param>
        /// <param name="challenge">The received challenge value from the server.</param>
        public void ConnectUDP(string host, int port, int challenge)
        {
            _udpSocket.Connect(host, port, BitConverter.GetBytes(challenge));
        }

        /// <summary>
        /// Returns the number of connections found from a single IP. This method itself is not thread-safe, so it
        /// must only be called where a lock on _connectionsLock is set.
        /// </summary>
        /// <param name="targetConn">Connection whos IP is to be checked.</param>
        int ConnectionsFromIP(ITCPSocket targetConn)
        {
            if (targetConn == null || string.IsNullOrEmpty(targetConn.Address))
            {
                Debug.Fail("targetConn is null or invalid");
                return 0;
            }

            return Connections.Count(x => x.IP == targetConn.IP);
        }

        /// <summary>
        /// Closes and disposes all connections made, leaving just the listen socket (if one exists).
        /// </summary>
        public void Disconnect()
        {
            Stack<IPSocket> connsToRemove;

            // Get the connections to remove
            lock (_connectionsLock)
            {
                connsToRemove = new Stack<IPSocket>(Connections);
            }

            // Remove each of the connections
            foreach (var ipSocket in connsToRemove)
            {
                ipSocket.TCPSocket.Dispose();
            }

#if DEBUG
            // Check that all connections we tried to move were removed
            lock (_connectionsLock)
            {
                var remaining = connsToRemove.Intersect(_connections);
                Debug.Assert(remaining.Count() == 0,
                             "Why is there items left in Connections after calling Dispose() on all of them?");
            }
#endif
        }

        /// <summary>
        /// Disposes of the SocketManager.
        /// </summary>
        /// <param name="disposeManaged">If true, disposes of managed resources.</param>
        void Dispose(bool disposeManaged)
        {
            if (_disposed)
                return;

            _disposed = true;

            if (!disposeManaged)
                return;

            // Close down all the connections
            Disconnect();

            // Close down the listen socket
            if (_listenSocket != null)
                _listenSocket.Dispose();
        }

        static readonly IEnumerable<SocketReceiveData> _emptySocketReceiveData = Enumerable.Empty<SocketReceiveData>();

        /// <summary>
        /// Gets all received data queues from all active connections.
        /// </summary>
        /// <param name="connData">Contains the data received from connection-oriented protocols (such as TCP).</param>
        /// <param name="nonConnData">Contains the data received from connectionless protocols (such as UDP).</param>
        protected void GetReceivedData(out IEnumerable<SocketReceiveData> connData, out IEnumerable<AddressedPacket> nonConnData)
        {
            // Grab the data from connectionless protocols
            nonConnData = _udpSocket.GetRecvData();

            // Grab the data from connection-oriented protocols
            List<SocketReceiveData> retConnData = null;
            lock (_connectionsLock)
            {
                // Loop through each socket
                foreach (var conn in Connections)
                {
                    // Get the queued receive data from the socket, continue if has data
                    var data = conn.GetRecvData();
                    if (data != null && data.Length > 0)
                    {
                        // Create the return list if needed
                        if (retConnData == null)
                            retConnData = new List<SocketReceiveData>();

                        // Add the data to the return list
                        retConnData.Add(new SocketReceiveData(conn, data));
                    }
                }
            }

            connData = retConnData ?? _emptySocketReceiveData;
        }

        /// <summary>
        /// Set up the server and starts listening for connections.
        /// </summary>
        /// <param name="tcpPort">TCP port to listen on.</param>
        /// <param name="udpPort">UDP port to bind to.</param>
        /// <param name="allowRemoteConnections">If true, remote connections will be allowed. If false, only
        /// local connections will be allowed. When true, you must ensure that your firewall does not block
        /// the given ports.</param>
        public void Listen(int tcpPort, int udpPort, bool allowRemoteConnections)
        {
            // If a ListenSocket already exists, dispose of it
            if (_listenSocket != null)
                _listenSocket.Dispose();

            // Create the new ListenSocket
            _listenSocket = new ListenSocket(tcpPort, allowRemoteConnections);
            _listenSocket.ConnectionAccepted += SocketAcceptHandler;

            // Check if the ListenSocket was created successfully
            if (!_listenSocket.IsAlive)
            {
                if (log.IsFatalEnabled)
                    log.Fatal("Unable to create listen socket.");
                Debug.Fail("Unable to create listen socket.");
                return;
            }

            _udpSocket.Bind(udpPort);

            if (log.IsInfoEnabled)
                log.Info("Listen socket accepting connections");
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling the corresponding event without
        /// the overhead of using event hooks. Therefore, it is recommended that this overload is used instead of
        /// the corresponding event when possible.
        /// </summary>
        protected virtual void OnConnectFailed()
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling the corresponding event without
        /// the overhead of using event hooks. Therefore, it is recommended that this overload is used instead of
        /// the corresponding event when possible.
        /// </summary>
        /// <param name="conn">Connection on which the event occured.</param>
        protected virtual void OnConnected(IIPSocket conn)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling the corresponding event without
        /// the overhead of using event hooks. Therefore, it is recommended that this overload is used instead of
        /// the corresponding event when possible.
        /// </summary>
        /// <param name="conn">Connection on which the event occured.</param>
        protected virtual void OnConnectedFrom(IIPSocket conn)
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling the corresponding event without
        /// the overhead of using event hooks. Therefore, it is recommended that this overload is used instead of
        /// the corresponding event when possible.
        /// </summary>
        /// <param name="conn">Connection on which the event occured.</param>
        protected virtual void OnDisconnected(IIPSocket conn)
        {
        }

        /// <summary>
        /// Removes all the Connections that satisfy the condition.
        /// </summary>
        /// <param name="match">The System.Predicate delegate that defines the conditions of the elements to remove.</param>
        public void Remove(Func<IIPSocket, bool> match)
        {
            var connsToRemove = new Stack<IPSocket>(8);

            lock (_connectionsLock)
            {
                foreach (var ipSocket in _connections)
                {
                    if (match(ipSocket))
                        connsToRemove.Push(ipSocket);
                }
            }

            // Get the number of matches found
            var removeCount = connsToRemove.Count();

            // If no connections match the condition, we can leave now
            if (removeCount == 0)
                return;

            // Call dispose on all the connections being removed
            foreach (var conn in connsToRemove)
            {
                conn.Dispose();
            }

            // Close itself should result in the connection being removed from the Connections list, but
            // we will check just in case
#if DEBUG
            lock (_connectionsLock)
            {
                var remaining = connsToRemove.Intersect(_connections);
                Debug.Assert(remaining.Count() == 0, "One or more connections failed to be removed from the Connections list.");
            }
#endif
        }

        /// <summary>
        /// Handles accepted connections from the listener socket
        /// </summary>
        /// <param name="sender">The <see cref="ListenSocket"/> that accepted the connection.</param>
        /// <param name="conn">TCPSocket that the new connection is on</param>
        void SocketAcceptHandler(ListenSocket sender, TCPSocket conn)
        {
            IPSocket ipSocket;

            lock (_connectionsLock)
            {
                // Check for too many connections from this address
                if (_maxDupeIP > 0 && _maxDupeIP < _connections.Count && ConnectionsFromIP(conn) > _maxDupeIP)
                {
                    if (log.IsWarnEnabled)
                    {
                        log.WarnFormat("Connection from address {0} rejected - too many connections from this IP [{1}/{2}]",
                                       conn.Address, _connections.Count, _maxDupeIP);
                    }
                    conn.Dispose();
                    return;
                }

                // Initialize the connection and add it to the connections list
                conn.Initialize();
                conn.Disposed += SocketDisposeHandler;
                ipSocket = new IPSocket(conn, _udpSocket);
                _connections.Add(ipSocket);
            }

            // Notify listeners
            OnConnectedFrom(ipSocket);

            if (ConnectedFrom != null)
                ConnectedFrom(this, ipSocket);
        }

        /// <summary>
        /// Handles TCPSocket.OnDispose events
        /// </summary>
        /// <param name="sender"></param>
        void SocketDisposeHandler(TCPSocket sender)
        {
            if (log.IsInfoEnabled)
                log.InfoFormat("Address `{0}` connection closed.", sender.Address);

            IPSocket ipSocket;

            // Remove the connect from the active connections list
            bool wasRemoved;
            lock (_connectionsLock)
            {
                ipSocket = (IPSocket)sender.Tag;
                wasRemoved = _connections.Remove(ipSocket);
            }

            // If it was removed, call the OnDisconnect, else produce an error message
            if (wasRemoved)
            {
                OnDisconnected(ipSocket);

                if (Disconnected != null)
                    Disconnected(this, ipSocket);
            }
            else
            {
                const string errmsg = "Closed connection on address `{0}`, but for some reason it wasn't in the Connections list.";
                Debug.Fail(string.Format(errmsg, sender.Address));
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, sender.Address);
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Closes down all connections and disposes all resources used by
        /// the socket manager
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}