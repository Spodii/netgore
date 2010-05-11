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
        long _tcpSent;
        long _udpRecv;
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
            _tcpSent = source._tcpSent;
            _tcpRecv = source._tcpRecv;
            _udpSent = source._udpSent;
            _udpRecv = source._udpRecv;
            _conns = source._conns;
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
        /// Gets the number of bytes that have been received over the TCP channel.
        /// </summary>
        public long TCPRecv
        {
            get { return _tcpRecv; }
        }

        /// <summary>
        /// Gets the number of bytes that have been sent over the TCP channel.
        /// </summary>
        public long TCPSent
        {
            get { return _tcpSent; }
        }

        /// <summary>
        /// Gets the number of bytes that have been received over the UDP channel.
        /// </summary>
        public long UDPRecv
        {
            get { return _udpRecv; }
        }

        /// <summary>
        /// Gets the number of bytes that have been sent over the UDP channel.
        /// </summary>
        public long UDPSent
        {
            get { return _udpSent; }
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
        /// Adds to the <see cref="NetStats.UDPSent"/> property value.
        /// </summary>
        /// <param name="value">The amount to add.</param>
        public void AddUDPSent(long value)
        {
            Interlocked.Add(ref _udpSent, value);
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
        /// Resets all of the network stat values. Cannot be used if <see cref="NetStats.IsGlobalNetStats"/> is set.
        /// </summary>
        /// <exception cref="InvalidOperationException">This <see cref="NetStats"/> instance is the <see cref="NetStats.Global"/>
        /// instance.</exception>
        public void Reset()
        {
            if (IsGlobalNetStats)
                throw new InvalidOperationException("Cannot call Reset() on the global NetStats instance.");

            _tcpSent = 0;
            _tcpRecv = 0;
            _udpSent = 0;
            _udpRecv = 0;
            _conns = 0;
        }

        /// <summary>
        /// Checks if two <see cref="NetStats"/> contain the same values.
        /// </summary>
        /// <param name="a">The first argument.</param>
        /// <param name="b">The second argument.</param>
        /// <returns>True if <paramref name="a"/> has the same values as <paramref name="b"/>; false if one or more values
        /// are different, or either of the arguments are null.</returns>
        public static bool ValuesEqual(NetStats a, NetStats b)
        {
            if (a == null || b == null)
                return false;

            return a._tcpSent == b._tcpSent && a._tcpRecv == b._tcpRecv && a._udpSent == b._udpSent && a._udpRecv == b._udpRecv &&
                   a._conns == b._conns;
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
                _tcpSent = left._tcpRecv - right._tcpSent,
                _tcpRecv = left._tcpRecv - right._tcpRecv,
                _udpSent = left._udpRecv - right._udpSent,
                _udpRecv = left._udpRecv - right._udpRecv,
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
                _tcpSent = left._tcpRecv + right._tcpSent,
                _tcpRecv = left._tcpRecv + right._tcpRecv,
                _udpSent = left._udpRecv + right._udpSent,
                _udpRecv = left._udpRecv + right._udpRecv,
                _conns = left._conns + right._conns
            };
        }
    }
}