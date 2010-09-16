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
        long _recv;
        int _recvs;
        int _rejected;
        int _sends;
        long _sent;

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
        /// Gets the number of connections that have been made and accepted.
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
        public int Receives
        {
            get { return _recvs; }
        }

        /// <summary>
        /// Gets the number of bytes that have been received over the TCP channel.
        /// </summary>
        public long Recv
        {
            get { return Interlocked.Read(ref _recv); }
        }

        /// <summary>
        /// Gets the number of connections that attempted to be made but were rejected.
        /// </summary>
        public int RejectedConnections
        {
            get { return _rejected; }
        }

        /// <summary>
        /// Gets the number of messages that have been sent over the TCP channel. Message refers to a socket-level message, not individual
        /// game messages. Each send is usually one packet, but larger messages (if the socket allows it) can result in multiple packets
        /// for a single send.
        /// </summary>
        public int Sends
        {
            get { return _sends; }
        }

        /// <summary>
        /// Gets the number of bytes that have been sent over the TCP channel.
        /// </summary>
        public long Sent
        {
            get { return Interlocked.Read(ref _sent); }
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
        /// Adds to the <see cref="Recv"/> property value.
        /// </summary>
        /// <param name="value">The amount to add.</param>
        public void AddRecv(long value)
        {
            Interlocked.Add(ref _recv, value);
        }

        /// <summary>
        /// Adds to the <see cref="NetStats.RejectedConnections"/> property value.
        /// </summary>
        /// <param name="value">The amount to add.</param>
        public void AddRejectedConnections(int value)
        {
            Interlocked.Add(ref _rejected, value);
        }

        /// <summary>
        /// Adds to the <see cref="Sends"/> property value.
        /// </summary>
        /// <param name="value">The amount to add.</param>
        public void AddSends(int value)
        {
            Interlocked.Add(ref _sends, value);
        }

        /// <summary>
        /// Adds to the <see cref="Sent"/> property value.
        /// </summary>
        /// <param name="value">The amount to add.</param>
        public void AddSent(long value)
        {
            Interlocked.Add(ref _sent, value);
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

            return Sent == o.Sent && Sends == o.Sends && Recv == o.Recv && Receives == o.Receives && Connections == o.Connections &&
                   RejectedConnections == o.RejectedConnections;
        }

        /// <summary>
        /// Copies the values from another <see cref="NetStats"/> into this <see cref="NetStats"/> instance.
        /// </summary>
        /// <param name="source">The <see cref="NetStats"/> to copy the values from.</param>
        public void CopyValuesFrom(NetStats source)
        {
            _sent = source._sent;
            _sends = source._sends;
            _recv = source._recv;
            _recvs = source._recvs;

            _conns = source._conns;
            _rejected = source._rejected;
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
        /// Increments the <see cref="Receives"/> property value by one.
        /// </summary>
        public void IncrementReceives()
        {
            Interlocked.Increment(ref _recvs);
        }

        /// <summary>
        /// Increments the <see cref="RejectedConnections"/> property value by one.
        /// </summary>
        public void IncrementRejected()
        {
            Interlocked.Increment(ref _rejected);
        }

        /// <summary>
        /// Increments the <see cref="Sends"/> property value by one.
        /// </summary>
        public void IncrementSends()
        {
            Interlocked.Increment(ref _sends);
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

            _sent = 0;
            _sends = 0;
            _recv = 0;
            _recvs = 0;

            _conns = 0;
            _rejected = 0;
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
                _sent = left._sent - right._sent,
                _sends = left._sends - right._sends,
                _recv = left._recv - right._recv,
                _recvs = left._recvs - right._recvs,
                _conns = left._conns - right._conns,
                _rejected = left._rejected - right._rejected
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
                _sent = left._sent + right._sent,
                _sends = left._sends + right._sends,
                _recv = left._recv + right._recv,
                _recvs = left._recvs + right._recvs,
                _conns = left._conns + right._conns,
                _rejected = left._rejected + right._rejected
            };
        }
    }
}