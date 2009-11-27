using System.Collections.Generic;
using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// An implementation of the <see cref="ObjectPool&lt;T&gt;"/> that is thread safe.
    /// </summary>
    /// <typeparam name="T">The type of object to pool.</typeparam>
    public class ThreadSafeObjectPool<T> : ObjectPool<T> where T : IPoolable<T>, new()
    {
        readonly object _threadSync = new object();

        /// <summary>
        /// Adds a node to the end of the pool and sets it as the highest node.
        /// Can be overridden to add a thread synchronization lock.
        /// </summary>
        /// <param name="node">The node to add.</param>
        protected internal override void AddNodeToEnd(LinkedListNode<T> node)
        {
            lock (_threadSync)
            {
                base.AddNodeToEnd(node);
            }
        }

        /// <summary>
        /// Checks if the <paramref name="poolData"/> contains the highest node. If it does, set it as the
        /// new highest node.
        /// Can be overridden to add a thread synchronization lock.
        /// </summary>
        /// <param name="poolData">The node pool data.</param>
        protected internal override void CheckSetHighNode(PoolData<T> poolData)
        {
            lock (_threadSync)
            {
                base.CheckSetHighNode(poolData);
            }
        }

        /// <summary>
        /// Clears out the object pool.
        /// Can be overridden to add a thread synchronization lock.
        /// </summary>
        protected internal override void ClearPool()
        {
            lock (_threadSync)
            {
                base.ClearPool();
            }
        }

        /// <summary>
        /// Frees the given <paramref name="node"/> and updates the highest node.
        /// Can be overridden to add a thread synchronization lock.
        /// </summary>
        /// <param name="node">The node being destroyed.</param>
        protected internal override void DestroyNode(LinkedListNode<T> node)
        {
            lock (_threadSync)
            {
                base.DestroyNode(node);
            }
        }

        /// <summary>
        /// Gets the next free node.
        /// Can be overridden to add a thread synchronization lock.
        /// </summary>
        /// <returns>Next free node if one exists, else null.</returns>
        protected internal override LinkedListNode<T> GetFree()
        {
            lock (_threadSync)
            {
                return base.GetFree();
            }
        }
    }
}