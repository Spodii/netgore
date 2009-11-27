using System;
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
    /// Assists in writing packets to a BitStream. Can be used with the PacketWriterPool to recycle the
    /// PacketWriters instead of creating a new one for each individual write. The PacketWriter
    /// must call Dispose() when done writing to return it back to the pool. If the DEBUG flag is
    /// defined, any PacketWriter that is not returned after a given amount of time will throw an
    /// Exception since it was likely lost and the equivilant of a memory leak. It is preferred that a
    /// PacketWriter is never constructed directly, and is only used from the PacketWriterPool.
    /// </summary>
    public class PacketWriter : BitStream, IPoolable<PacketWriter>, IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

#if DEBUG
        /// <summary>
        /// How many milliseconds must elapse before a PacketWriter is considered lost and undisposed.
        /// </summary>
        const int _disposeTimeoutTime = 1000 * 15;
#endif

#if DEBUG
        /// <summary>
        /// Timer used to track a PacketWriter that has gone too long without being disposed
        /// </summary>
        readonly Timer _expireTimer = new Timer();
#endif

        IObjectPool<PacketWriter> _objectPool;
        PoolData<PacketWriter> _poolData;

#if DEBUG
        /// <summary>
        /// StackTrace that is used for finding where a lost PacketWriter originated from
        /// </summary>
        StackTrace _stackTrace;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketWriter"/> class.
        /// </summary>
        public PacketWriter() : base(BitStreamMode.Write, 128, false)
        {
            ReadMode = BitStreamBufferMode.Static;
            WriteMode = BitStreamBufferMode.Dynamic;

#if DEBUG
            _expireTimer.AutoReset = true;
            _expireTimer.Elapsed += ExpireTimerElapsed;
            _expireTimer.Interval = _disposeTimeoutTime;
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

            _expireTimer.Stop();
#endif
        }

        /// <summary>
        /// Returns the PacketWriter back to the ObjectPool whence it came. After this is called, it is
        /// recommended to never, ever try to interact with the object directly. Treat it like you would if
        /// the object was destroyed.
        /// </summary>
        public void Dispose()
        {
            HandleDispose();
        }

        /// <summary>
        /// When overridden in the derived class, performs disposing of the object.
        /// </summary>
        protected override void HandleDispose()
        {
            _objectPool.Destroy(this);
        }

        #region IPoolable<PacketWriter> Members

        /// <summary>
        /// Gets the PoolData associated with this poolable item.
        /// </summary>
        PoolData<PacketWriter> IPoolable<PacketWriter>.PoolData
        {
            get { return _poolData; }
        }

        /// <summary>
        /// Notifies the item that it has been activated by the pool and that it will start being used.
        /// All preperation work that could not be done in the constructor should be done here.
        /// </summary>
        void IPoolable<PacketWriter>.Activate()
        {
            Reset(BitStreamMode.Write);

#if DEBUG
            _stackTrace = new StackTrace();
            _expireTimer.Start();
#endif
        }

        /// <summary>
        /// Notifies the item that it has been deactivated by the pool. The item may or may not ever be
        /// activated again, so clean up where needed.
        /// </summary>
        void IPoolable<PacketWriter>.Deactivate()
        {
#if DEBUG
            _expireTimer.Stop();
#endif
        }

        /// <summary>
        /// Sets the PoolData for this item. This is only called once in the object's lifetime;
        /// right after the object is constructed.
        /// </summary>
        /// <param name="objectPool">Pool that created this object.</param>
        /// <param name="poolData">PoolData assigned to this object.</param>
        void IPoolable<PacketWriter>.SetPoolData(IObjectPool<PacketWriter> objectPool, PoolData<PacketWriter> poolData)
        {
            _objectPool = objectPool;
            _poolData = poolData;
        }

        #endregion
    }
}