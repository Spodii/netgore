using System;
using System.Linq;
using System.Threading;

namespace NetGore.Network
{
    /// <summary>
    /// Provides statistics related to the networking since the program has started.
    /// This class is fully thread-safe.
    /// </summary>
    public class NetStats
    {
        static readonly NetStats _global;

        int _conns;
        long _tcpRecv;
        int _tcpRecvs;
        int _tcpSends;
        long _tcpSent;
        long _udpRecv;
        int _udpRecvs;
        int _udpSends;
        long _udpSent;

        /// <summary>
        /// Initializes the <see cref="NetStats"/> class.
        /// </summary>
        static NetStats()
        {
            _global = new NetStats();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetStats"/> class.
        /// </summary>
        public NetStats()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetStats"/> class.
        /// </summary>
        /// <param name="source">The <see cref="NetStats"/> to copy the inital values from.</param>
        public NetStats(NetStats source)
        {
            CopyValuesFrom(source);
        }

        /// <summary>
        /// Gets the number of TCP connections that have been made.
        /// </summary>
        public int Connections
        {
            get { return _conns; }
        }

        /// <summary>
        /// Gets the global <see cref="NetStats"/> instance that contains the accumulated counts since the application started.
        /// </summary>
        public static NetStats Global
        {
            get { return _global; }
        }

        /// <summary>
        /// Gets if this is the global <see cref="NetStats"/> instance.
        /// </summary>
        public bool IsGlobalNetStats
        {
            get { return this == Global; }
        }

        /// <summary>
        /// Gets the number of messages that have been received over the TCP channel. Message refers to a socket-level message, not individual
        /// game messages. Each send is usually one packet, but larger messages (if the socket allows it) can result in multiple packets
        /// for a single receive.
        /// </summary>
        public int TCPReceives
        {
            get { return _tcpRecvs; }
        }

        /// <summary>
        /// Gets the number of bytes that have been received over the TCP channel.
        /// </summary>
        public long TCPRecv
        {
            get { return Interlocked.Read(ref _tcpRecv); }
        }

        /// <summary>
        /// Gets the number of messages that have been sent over the TCP channel. Message refers to a socket-level message, not individual
        /// game messages. Each send is usually one packet, but larger messages (if the socket allows it) can result in multiple packets
        /// for a single send.
        /// </summary>
        public int TCPSends
        {
            get { return _tcpSends; }
        }

        /// <summary>
        /// Gets the number of bytes that have been sent over the TCP channel.
        /// </summary>
        public long TCPSent
        {
            get { return Interlocked.Read(ref _tcpSent); }
        }

        /// <summary>
        /// Gets the number of messages that have been received over the UDP channel. Message refers to a socket-level message, not individual
        /// game messages. Each send is usually one packet, but larger messages (if the socket allows it) can result in multiple packets
        /// for a single receive.
        /// </summary>
        public int UDPReceives
        {
            get { return _udpRecvs; }
        }

        /// <summary>
        /// Gets the number of bytes that have been received over the UDP channel.
        /// </summary>
        public long UDPRecv
        {
            get { return Interlocked.Read(ref _udpRecv); }
        }

        /// <summary>
        /// Gets the number of messages that have been sent over the UDP channel. Message refers to a socket-level message, not individual
        /// game messages. Each send is usually one packet, but larger messages (if the socket allows it) can result in multiple packets
        /// for a single send.
        /// </summary>
        public int UDPSends
        {
            get { return _udpSends; }
        }

        /// <summary>
        /// Gets the number of bytes that have been sent over the UDP channel.
        /// </summary>
        public long UDPSent
        {
            get { return Interlocked.Read(ref _udpSent); }
        }

        /// <summary>
        /// Adds to the <see cref="NetStats.Connections"/> property value.
        /// </summary>
        /// <param name="value">The amount to add.</param>
        public void AddConnections(int value)
        {
            Interlocked.Add(ref _conns, value);
        }

        /// <summary>
        /// Adds to the <see cref="NetStats.TCPRecv"/> property value.
        /// </summary>
        /// <param name="value">The amount to add.</param>
        public void AddTCPRecv(long value)
        {
            Interlocked.Add(ref _tcpRecv, value);
        }

        /// <summary>
        /// Adds to the <see cref="NetStats.TCPSends"/> property value.
        /// </summary>
        /// <param name="value">The amount to add.</param>
        public void AddTCPSends(int value)
        {
            Interlocked.Add(ref _tcpSends, value);
        }

        /// <summary>
        /// Adds to the <see cref="NetStats.TCPSent"/> property value.
        /// </summary>
        /// <param name="value">The amount to add.</param>
        public void AddTCPSent(long value)
        {
            Interlocked.Add(ref _tcpSent, value);
        }

        /// <summary>
        /// Adds to the <see cref="NetStats.UDPRecv"/> property value.
        /// </summary>
        /// <param name="value">The amount to add.</param>
        public void AddUDPRecv(long value)
        {
            Interlocked.Add(ref _udpRecv, value);
        }

        /// <summary>
        /// Adds to the <see cref="NetStats.UDPSends"/> property value.
        /// </summary>
        /// <param name="value">The amount to add.</param>
        public void AddUDPSends(int value)
        {
            Interlocked.Add(ref _udpSends, value);
        }

        /// <summary>
        /// Adds to the <see cref="NetStats.UDPSent"/> property value.
        /// </summary>
        /// <param name="value">The amount to add.</param>
        public void AddUDPSent(long value)
        {
            Interlocked.Add(ref _udpSent, value);
        }

        /// <summary>
        /// Checks if two <see cref="NetStats"/> contain the same values.
        /// </summary>
        /// <param name="o">The <see cref="NetStats"/> to compare the values to.</param>
        /// <returns>True if <paramref name="o"/> has the same values as this instance; false if <paramref name="o"/> is null
        /// or one or more values are different.</returns>
        public bool AreValuesEqual(NetStats o)
        {
            if (o == null)
                return false;

            return TCPSent == o.TCPSent && TCPSends == o.TCPSends && TCPRecv == o.TCPRecv && TCPReceives == o.TCPReceives &&
                   UDPSent == o.UDPSent && UDPSends == o.UDPSends && UDPRecv == o.UDPRecv && UDPReceives == o.UDPReceives &&
                   Connections == o.Connections;
        }

        /// <summary>
        /// Copies the values from another <see cref="NetStats"/> into this <see cref="NetStats"/> instance.
        /// </summary>
        /// <param name="source">The <see cref="NetStats"/> to copy the values from.</param>
        public void CopyValuesFrom(NetStats source)
        {
            _tcpSent = source._tcpSent;
            _tcpSends = source._tcpSends;
            _tcpRecv = source._tcpRecv;
            _tcpRecvs = source._tcpRecvs;

            _udpSent = source._udpSent;
            _udpSends = source._udpSends;
            _udpRecv = source._udpRecv;
            _udpRecvs = source._udpRecvs;

            _conns = source._conns;
        }

        /// <summary>
        /// Creates a deep copy of this <see cref="NetStats"/>.
        /// </summary>
        /// <returns>A deep copy of this <see cref="NetStats"/>.</returns>
        public NetStats DeepCopy()
        {
            return new NetStats(this);
        }

        /// <summary>
        /// Increments the <see cref="NetStats.Connections"/> property value by one.
        /// </summary>
        public void IncrementConnections()
        {
            Interlocked.Increment(ref _conns);
        }

        /// <summary>
        /// Increments the <see cref="NetStats.TCPReceives"/> property value by one.
        /// </summary>
        public void IncrementTCPReceives()
        {
            Interlocked.Increment(ref _tcpRecvs);
        }

        /// <summary>
        /// Increments the <see cref="NetStats.TCPSends"/> property value by one.
        /// </summary>
        public void IncrementTCPSends()
        {
            Interlocked.Increment(ref _tcpSends);
        }

        /// <summary>
        /// Increments the <see cref="NetStats.UDPReceives"/> property value by one.
        /// </summary>
        public void IncrementUDPReceives()
        {
            Interlocked.Increment(ref _udpRecvs);
        }

        /// <summary>
        /// Increments the <see cref="NetStats.UDPSends"/> property value by one.
        /// </summary>
        public void IncrementUDPSends()
        {
            Interlocked.Increment(ref _udpSends);
        }

        /// <summary>
        /// Resets all of the network stat values. Cannot be used if <see cref="NetStats.IsGlobalNetStats"/> is set.
        /// </summary>
        /// <exception cref="InvalidOperationException">This <see cref="NetStats"/> instance is the <see cref="NetStats.Global"/>
        /// instance.</exception>
        public void Reset()
        {
            if (IsGlobalNetStats)
                throw new InvalidOperationException("Cannot call Reset() on the global NetStats instance.");

            _tcpSent = 0;
            _tcpSends = 0;
            _tcpRecv = 0;
            _tcpRecvs = 0;

            _udpSent = 0;
            _udpSends = 0;
            _udpRecv = 0;
            _udpRecvs = 0;

            _conns = 0;
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The result of the operator.</returns>
        public static NetStats operator -(NetStats left, NetStats right)
        {
            return new NetStats
            {
                _tcpSent = left._tcpSent - right._tcpSent,
                _tcpSends = left._tcpSends - right._tcpSends,
                _tcpRecv = left._tcpRecv - right._tcpRecv,
                _tcpRecvs = left._tcpRecvs - right._tcpRecvs,
                _udpSent = left._udpSent - right._udpSent,
                _udpSends = left._udpSends - right._udpSends,
                _udpRecv = left._udpRecv - right._udpRecv,
                _udpRecvs = left._udpRecvs - right._udpRecvs,
                _conns = left._conns - right._conns
            };
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The result of the operator.</returns>
        public static NetStats operator +(NetStats left, NetStats right)
        {
            return new NetStats
            {
                _tcpSent = left._tcpSent + right._tcpSent,
                _tcpSends = left._tcpSends + right._tcpSends,
                _tcpRecv = left._tcpRecv + right._tcpRecv,
                _tcpRecvs = left._tcpRecvs + right._tcpRecvs,
                _udpSent = left._udpSent + right._udpSent,
                _udpSends = left._udpSends + right._udpSends,
                _udpRecv = left._udpRecv + right._udpRecv,
                _udpRecvs = left._udpRecvs + right._udpRecvs,
                _conns = left._conns + right._conns
            };
        }
    }
}