using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// Interface for an object that can be placed in an object pool
    /// </summary>
    /// <typeparam name="T">Type of the object being placed in the pool (often the same as the
    /// class the interface is placed on)</typeparam>
    public interface IPoolable<T> where T : IPoolable<T>, new()
    {
        /// <summary>
        /// Gets the PoolData associated with this poolable item
        /// </summary>
        PoolData<T> PoolData { get; }

        /// <summary>
        /// Notifies the item that it has been activated by the pool and that it will start being used.
        /// All preperation work that could not be done in the constructor should be done here.
        /// </summary>
        void Activate();

        /// <summary>
        /// Notifies the item that it has been deactivated by the pool. The item may or may not ever be
        /// activated again, so clean up where needed.
        /// </summary>
        void Deactivate();

        /// <summary>
        /// Sets the PoolData for this item. This is only called once in the object's lifetime;
        /// right after the object is constructed.
        /// </summary>
        /// <param name="objectPool">Pool that created this object</param>
        /// <param name="poolData">PoolData assigned to this object</param>
        void SetPoolData(ObjectPool<T> objectPool, PoolData<T> poolData);
    }
}