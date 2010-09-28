using System.Linq;
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
    public class PacketWriter : BitStream, IPoolable
    {
        readonly IObjectPool<PacketWriter> _objectPool;

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketWriter"/> class.
        /// </summary>
        internal PacketWriter(IObjectPool<PacketWriter> objectPool) : base(128)
        {
            _objectPool = objectPool;
        }

        /// <summary>
        /// When overridden in the derived class, performs disposing of the object.
        /// </summary>
        protected override void HandleDispose()
        {
            _objectPool.Free(this);
        }

        #region IPoolable Members

        /// <summary>
        /// Gets or sets the index of the object in the pool. This value should never be used by anything
        /// other than the pool that owns this object.
        /// </summary>
        int IPoolable.PoolIndex { get; set; }

        #endregion
    }
}