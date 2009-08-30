using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Timers;
using log4net;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.Network
{
    /// <summary>
    /// Assists in writing packets to a BitStream, along with writing and reading custom data types.
    /// Can be used with the PacketWriterPool to recycle the PacketWriters instead of creating a new
    /// one for each individual write. The PacketWriter must call Dispose() when done writing to return
    /// it back to the pool. If the DEBUG flag is defined, any PacketWriter that is not returned after
    /// a given amount of time will throw an Exception since it was likely lost and the equivilant of a memory leak.
    /// It is preferred that a PacketWriter is never used directly, and is only used from the PacketWriterPool.
    /// </summary>
    public class PacketWriter : BitStream, IPoolable<PacketWriter>, IValueWriter
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Writes an unsigned integer of up to 32 bits.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        /// <param name="bits">Number of bits to write.</param>
        void IValueWriter.Write(string name, uint value, int bits)
        {
            Write(value, bits);
        }

        /// <summary>
        /// Writes a boolean.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        void IValueWriter.Write(string name, bool value)
        {
            Write(value);
        }

        /// <summary>
        /// Writes a 32-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the BitStreamValueWriter.</param>
        /// <param name="value">Value to write.</param>
        void IValueWriter.Write(string name, uint value)
        {
            Write(value);
        }

        /// <summary>
        /// Writes a 16-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the BitStreamValueWriter.</param>
        /// <param name="value">Value to write.</param>
        void IValueWriter.Write(string name, short value)
        {
            Write(value);
        }

        /// <summary>
        /// Writes a 16-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the BitStreamValueWriter.</param>
        /// <param name="value">Value to write.</param>
        void IValueWriter.Write(string name, ushort value)
        {
            Write(value);
        }

        /// <summary>
        /// Writes a 8-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the BitStreamValueWriter.</param>
        /// <param name="value">Value to write.</param>
        void IValueWriter.Write(string name, byte value)
        {
            Write(value);
        }

        /// <summary>
        /// Writes a 8-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the BitStreamValueWriter.</param>
        /// <param name="value">Value to write.</param>
        void IValueWriter.Write(string name, sbyte value)
        {
            Write(value);
        }

        /// <summary>
        /// Writes a variable-length string of up to 65535 characters in length.
        /// </summary>
        /// <param name="name">Unused by the BitStreamValueWriter.</param>
        /// <param name="value">String to write.</param>
        void IValueWriter.Write(string name, string value)
        {
            Write(value);
        }

        /// <summary>
        /// Gets if this IValueReader supports using the name field to look up values. If false, values will have to
        /// be read back in the same order they were written and the name field will be ignored.
        /// </summary>
        bool IValueWriter.SupportsNameLookup
        {
            get { return false; }
        }

        /// <summary>
        /// Writes multiple values of the same type to the IValueWriter all under the same node name.
        /// Ordering is not guarenteed.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="nodeName">Name of the node that will contain the values.</param>
        /// <param name="values">IEnumerable of values to write. If this value is null, it will be treated
        /// the same as if it were an empty IEnumerable.</param>
        /// <param name="writeHandler">Delegate that writes the value to the IValueWriter.</param>
        void IValueWriter.WriteMany<T>(string nodeName, IEnumerable<T> values, WriteManyHandler<T> writeHandler)
        {
            // If this exception ever comes up, maybe it is time to add support for this. Implementation would probably
            // be nearly identical to that used in the BitStreamValueWriter.
            throw new NotSupportedException();
        }

        /// <summary>
        /// Writes multiple values of type <typeparamref name="T"/>, where each value will result in its own
        /// node being created. Ordering is not guarenteed.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="nodeName">Name of the node that will contain the values.</param>
        /// <param name="values">IEnumerable of values to write. If this value is null, it will be treated
        /// the same as if it were an empty IEnumerable.</param>
        /// <param name="writeHandler">Delegate that writes the value to the IValueWriter.</param>
        public void WriteManyNodes<T>(string nodeName, IEnumerable<T> values, WriteManyNodesHandler<T> writeHandler)
        {
            // If this exception ever comes up, maybe it is time to add support for this. Implementation would probably
            // be nearly identical to that used in the BitStreamValueWriter.
            throw new NotSupportedException();
        }

        /// <summary>
        /// Writes multiple values of type <typeparamref name="T"/>, where each value will result in its own
        /// node being created. Unlike the WriteMany for IEnumerables, this guarentees that ordering will be preserved.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="nodeName">Name of the node that will contain the values.</param>
        /// <param name="values">IEnumerable of values to write. If this value is null, it will be treated
        /// the same as if it were an empty IEnumerable.</param>
        /// <param name="writeHandler">Delegate that writes the value to the IValueWriter.</param>
        public void WriteManyNodes<T>(string nodeName, T[] values, WriteManyNodesHandler<T> writeHandler)
        {
            // If this exception ever comes up, maybe it is time to add support for this. Implementation would probably
            // be nearly identical to that used in the BitStreamValueWriter.
            throw new NotSupportedException();
        }

        /// <summary>
        /// Writes multiple values of the same type to the IValueWriter all under the same node name.
        /// Unlike the WriteMany for IEnumerables, this guarentees that ordering will be preserved.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="nodeName">Name of the node that will contain the values.</param>
        /// <param name="values">Array of values to write. If this value is null, it will be treated
        /// the same as if it were an empty array.</param>
        /// <param name="writeHandler">Delegate that writes the value to the IValueWriter.</param>
        void IValueWriter.WriteMany<T>(string nodeName, T[] values, WriteManyHandler<T> writeHandler)
        {
            // If this exception ever comes up, maybe it is time to add support for this. Implementation would probably
            // be nearly identical to that used in the BitStreamValueWriter.
            throw new NotSupportedException();
        }

        /// <summary>
        /// Writes the start of a child node in this IValueWriter.
        /// </summary>
        /// <param name="name">Name of the child node.</param>
        void IValueWriter.WriteStartNode(string name)
        {
            // If this exception ever comes up, maybe it is time to add support for this. Implementation would probably
            // be nearly identical to that used in the BitStreamValueWriter.
            throw new NotSupportedException();
        }

        /// <summary>
        /// Writes the end of a child node in this IValueWriter.
        /// </summary>
        /// <param name="name">Name of the child node.</param>
        void IValueWriter.WriteEndNode(string name)
        {
            // If this exception ever comes up, maybe it is time to add support for this. Implementation would probably
            // be nearly identical to that used in the BitStreamValueWriter.
            throw new NotSupportedException();
        }

        /// <summary>
        /// Writes a 32-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the BitStreamValueWriter.</param>
        /// <param name="value">Value to write.</param>
        void IValueWriter.Write(string name, int value)
        {
            Write(value);
        }

        /// <summary>
        /// Writes a signed integer of up to 32 bits.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        /// <param name="bits">Number of bits to write.</param>
        void IValueWriter.Write(string name, int value, int bits)
        {
            Write(value, bits);
        }

        /// <summary>
        /// Writes a 32-bit floating-point number.
        /// </summary>
        /// <param name="name">Unused by the BitStreamValueWriter.</param>
        /// <param name="value">Value to write.</param>
        void IValueWriter.Write(string name, float value)
        {
            Write(value);
        }

#if DEBUG
        /// <summary>
        /// Timer used to track a PacketWriter that has gone too long without being disposed
        /// </summary>
        readonly Timer _expireTimer = new Timer();
#endif

        ObjectPool<PacketWriter> _objectPool;
        PoolData<PacketWriter> _poolData;

#if DEBUG
        /// <summary>
        /// StackTrace that is used for finding where a lost PacketWriter originated from
        /// </summary>
        StackTrace _stackTrace;
#endif

        /// <summary>
        /// PacketWriter constructor
        /// </summary>
        public PacketWriter() : base(BitStreamMode.Write, 128)
        {
            ReadMode = BitStreamBufferMode.Static;
            WriteMode = BitStreamBufferMode.Dynamic;

#if DEBUG
            _expireTimer.Elapsed += ExpireTimerElapsed;
            _expireTimer.Interval = 1000 * 100; // 1000 * seconds
#endif
        }

        void ExpireTimerElapsed(object sender, ElapsedEventArgs e)
        {
#if DEBUG
            const string errmsg =
                "PacketWriter lived for far too long. Most likely it was never disposed of.{0}" +
                "Stack trace from PacketWriter's Activate():{0}{1}";

            string err = string.Format(errmsg, Environment.NewLine, _stackTrace);
            Debug.Fail(err);
            if (log.IsErrorEnabled)
                log.ErrorFormat(err, Environment.NewLine, _stackTrace);
#endif
        }

        #region IDisposable Members

        /// <summary>
        /// Returns the PacketWriter back to the ObjectPool whence it came. After this is called, it is
        /// recommended to never, ever try to interact with the object directly. Treat it like you would if
        /// the object was destroyed.
        /// </summary>
        public void Dispose()
        {
            _objectPool.Destroy(this);
        }

        #endregion

        #region IPoolable<PacketWriter> Members

        PoolData<PacketWriter> IPoolable<PacketWriter>.PoolData
        {
            get { return _poolData; }
        }

        void IPoolable<PacketWriter>.Activate()
        {
            Reset(BitStreamMode.Write);

#if DEBUG
            _stackTrace = new StackTrace();
            _expireTimer.Start();
#endif
        }

        void IPoolable<PacketWriter>.Deactivate()
        {
#if DEBUG
            _expireTimer.Stop();
#endif
        }

        void IPoolable<PacketWriter>.SetPoolData(ObjectPool<PacketWriter> objectPool, PoolData<PacketWriter> poolData)
        {
            _objectPool = objectPool;
            _poolData = poolData;
        }

        #endregion
    }
}