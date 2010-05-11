using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NetGore.Network
{
    /// <summary>
    /// Provides statistics related to the networking since the program has started.
    /// </summary>
    public class NetStats
    {
        readonly static NetStats _global;

        uint _tcpSent;
        uint _tcpRecv;
        uint _udpSent;
        uint _udpRecv;
        uint _conns;

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
        /// Creates a deep copy of this <see cref="NetStats"/>.
        /// </summary>
        /// <returns>A deep copy of this <see cref="NetStats"/>.</returns>
        public NetStats DeepCopy()
        {
            return new NetStats(this);
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

            return a._tcpSent == b._tcpSent && a._tcpRecv == b._tcpRecv &&
                a._udpSent == b._udpSent && a._udpRecv == b._udpRecv &&
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

        /// <summary>
        /// Gets if this is the global <see cref="NetStats"/> instance.
        /// </summary>
        public bool IsGlobalNetStats { get { return this == Global; } }

        /// <summary>
        /// Gets the global <see cref="NetStats"/> instance that contains the accumulated counts since the application started.
        /// </summary>
        public static NetStats Global { get { return _global; } }

        /// <summary>
        /// Gets the number of bytes that have been sent over the TCP channel.
        /// </summary>
        public uint TCPSent { get { return _tcpSent; } }

        /// <summary>
        /// Gets the number of bytes that have been received over the TCP channel.
        /// </summary>
        public uint TCPRecv { get { return _tcpRecv; } }

        /// <summary>
        /// Gets the number of bytes that have been sent over the UDP channel.
        /// </summary>
        public uint UDPSent { get { return _udpSent; } }

        /// <summary>
        /// Gets the number of bytes that have been received over the UDP channel.
        /// </summary>
        public uint UDPRecv { get { return _udpRecv; } }

        /// <summary>
        /// Gets the number of TCP connections that have been made.
        /// </summary>
        public uint Connections { get { return _conns; } }
    }
}
