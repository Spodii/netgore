using System;
using NetGore.IO.Bits;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Timers;
using log4net;
using NetGore;
using NetGore.Collections;

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
    public class PacketWriter : BitStream, IPoolable<PacketWriter>, IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
        public PacketWriter()
            : base(BitStreamMode.Write, 128)
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