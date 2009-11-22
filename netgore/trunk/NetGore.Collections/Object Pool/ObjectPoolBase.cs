using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore.Collections
{
    public interface IObjectPool<T> : IEnumerable<T> where T : IPoolable<T>, new()
    {
        /// <summary>
        /// Gets the number of live objects in the pool.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Clears all nodes, dead and alive, from the pool.
        /// </summary>
        void Clear();

        /// <summary>
        /// Clears all nodes, dead and alive, from the pool.
        /// </summary>
        /// <param name="destroy">If true, the Destroy() method will be raised on all the alive objects.
        /// Default is true.</param>
        void Clear(bool destroy);

        /// <summary>
        /// Gets an item to use from the pool.
        /// </summary>
        /// <returns>Item to use.</returns>
        T Create();

        /// <summary>
        /// Deactivates the <paramref name="item"/> in the pool, making it no longer in use.
        /// </summary>
        /// <param name="item">Item to deactivate.</param>
        void Destroy(T item);
    }

    public class ThreadSafeObjectPool<T> : ObjectPool<T> where T : IPoolable<T>, new()
    {
        readonly object _threadSync = new object();

        protected internal override void AddNodeToEnd(LinkedListNode<T> node)
        {
            lock (_threadSync)
            {
                base.AddNodeToEnd(node);
            }
        }

        protected internal override void CheckSetHighNode(PoolData<T> poolData)
        {
            lock (_threadSync)
            {
                base.CheckSetHighNode(poolData);
            }
        }

        protected internal override void ClearPool()
        {
            lock (_threadSync)
            {
                base.ClearPool();
            }
        }

        protected internal override void DestroyNode(LinkedListNode<T> node)
        {
            lock (_threadSync)
            {
                base.DestroyNode(node);
            }
        }

        protected internal override LinkedListNode<T> GetFree()
        {
            lock (_threadSync)
            {
                return base.GetFree();
            }
        }
    }

    /// <summary>
    /// Basic object pool layout. More complex pools can override this to allow for overriding of the
    /// Create() and Destroy() methods to do some custom handling of individual items (ie setting parameters).
    /// </summary>
    /// <typeparam name="T">Type of the item to be pooled.</typeparam>
    public class ObjectPool<T> : IObjectPool<T> where T : IPoolable<T>, new()
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Stack of all the LinkedListNodes that have been freed and need to be reused
        /// </summary>
        readonly Stack<LinkedListNode<T>> _free = new Stack<LinkedListNode<T>>(16);

        /// <summary>
        /// LinkedList containing all the active pool objects
        /// </summary>
        readonly LinkedList<T> _pool = new LinkedList<T>();

        /// <summary>
        /// Highest used node in the pool, which lets us end enumerating earlier. A null value
        /// means that the whole pool will be iterated through.
        /// </summary>
        LinkedListNode<T> _highNode = null;

        /// <summary>
        /// Adds a node to the end of the pool and sets it as the highest node.
        /// Can be overridden to add a thread synchronization lock.
        /// </summary>
        /// <param name="node">The node to add.</param>
        protected internal virtual void AddNodeToEnd(LinkedListNode<T> node)
        {
            // TODO: Override, lock

            // Add the node to the end of the item pool list
            _pool.AddLast(node);

            // This is clearly the highest node in use since it has the highest index
            _highNode = node;
        }

        /// <summary>
        /// Checks if the <paramref name="poolData"/> contains the highest node. If it does, set it as the
        /// new highest node.
        /// Can be overridden to add a thread synchronization lock.
        /// </summary>
        /// <param name="poolData">The node pool data.</param>
        protected internal virtual void CheckSetHighNode(PoolData<T> poolData)
        {
            // TODO: Override, lock

            if (poolData.PoolIndex > _highNode.Value.PoolData.PoolIndex)
                _highNode = poolData.PoolNode;
        }

        /// <summary>
        /// Clears out the object pool.
        /// Can be overridden to add a thread synchronization lock.
        /// </summary>
        protected internal virtual void ClearPool()
        {
            // TODO: Override, lock
            _pool.Clear();
        }

        /// <summary>
        /// Frees the given <paramref name="node"/> and updates the highest node.
        /// Can be overridden to add a thread synchronization lock.
        /// </summary>
        /// <param name="node">The node being destroyed.</param>
        protected internal virtual void DestroyNode(LinkedListNode<T> node)
        {
            // TODO: Override, lock

            // Push to the free list for later use
            _free.Push(node);

            // If we destroyed the highest node, find the next highest node
            if (node == _highNode)
                GetNextHighNode();
        }

        /// <summary>
        /// Gets the next free node.
        /// Can be overridden to add a thread synchronization lock.
        /// </summary>
        /// <returns>Next free node if one exists, else null.</returns>
        protected internal virtual LinkedListNode<T> GetFree()
        {
            // If the stack count is 0, theres no free nodes
            if (_free.Count == 0)
                return null;

            // Pop off and return the next item in the stack
            return _free.Pop();
        }

        /// <summary>
        /// Updates the <see cref="_highNode"/> by looping backwards through the nodes until either
        /// the list has been depleted or a live node is found
        /// </summary>
        void GetNextHighNode()
        {
            // While the current high node is not activated and is not the first in the
            // pool, move to the previous node
            while (!_highNode.Value.PoolData.IsActivated && _highNode != _pool.First)
            {
                _highNode = _highNode.Previous;
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling by the ObjectPoolBase when a new
        /// object is created for the pool. This is only called when a new object is created, not activated. This is
        /// called only once for each unique pool item, after IPoolable.SetPoolData() and before IPoolable.Activate().
        /// </summary>
        /// <param name="item">New object that was created.</param>
        protected virtual void HandleNewPoolObject(T item)
        {
        }

        #region IObjectPool<T> Members

        /// <summary>
        /// Clears all nodes, dead and alive, from the pool.
        /// </summary>
        public void Clear()
        {
            Clear(true);
        }

        /// <summary>
        /// Clears all nodes, dead and alive, from the pool.
        /// </summary>
        /// <param name="destroy">If true, the Destroy() method will be raised on all
        /// the alive objects.</param>
        public void Clear(bool destroy)
        {
            // Call Destroy() on the alive nodes if needed
            if (destroy)
            {
                foreach (var obj in this)
                {
                    obj.Deactivate();
                }
            }

            // Remove the nodes
            ClearPool();
        }

        /// <summary>
        /// Gets an item to use from the pool.
        /// </summary>
        /// <returns>Item to use.</returns>
        public virtual T Create()
        {
            var node = GetFree();
            PoolData<T> poolData;
            T item;

            // If null, we have to create the item
            if (node == null)
            {
                // Create the item
                item = new T();

                // Create the node
                node = new LinkedListNode<T>(item);

                // Create the pool data and assign it the item and node
                // The index is assigned by the size of the pool at the current time, giving us
                // an easy counter that will increment from 0
                poolData = new PoolData<T>(node, _pool.Count);

                // Make the item assign the pool data reference
                item.SetPoolData(this, poolData);

                // Allow for additional handling
                HandleNewPoolObject(item);

                AddNodeToEnd(node);
            }
            else
            {
                item = node.Value;
                poolData = item.PoolData;

                // If the index of this node is higher than the index of the highest node,
                // set this node to the highest node
                CheckSetHighNode(poolData);
            }

            // Activate the item
            poolData.Activate();

            return item;
        }

        /// <summary>
        /// Deactivates the <paramref name="item"/> in the pool, making it no longer in use.
        /// </summary>
        /// <param name="item">Item to deactivate.</param>
        public virtual void Destroy(T item)
        {
            var poolData = item.PoolData;
            var node = poolData.PoolNode;

            // Call the deactivation
            poolData.Deactivate();

            if (node == null)
            {
                const string errmsg = "item.PoolData.PoolNode is null.";
                Debug.Fail(errmsg);
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg);
                throw new Exception(errmsg);
            }

            DestroyNode(node);
        }

        /// <summary>
        /// Gets the number of live items in the ObjectPool.
        /// </summary>
        public int Count
        {
            get { return _pool.Count - _free.Count; }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection
        /// </summary>
        /// <returns>An System.Collections.IEnumerator object of type T that can be used 
        /// to iterate through the collection</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection
        /// </summary>
        /// <returns>An System.Collections.IEnumerator object that can be used 
        /// to iterate through the collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion

        /// <summary>
        /// Enumerator for an ObjectPoolBase
        /// </summary>
        public struct Enumerator : IEnumerator<T>
        {
            readonly LinkedListNode<T> _high;
            readonly LinkedList<T> _list;
            T _current;

            LinkedListNode<T> _node;

            /// <summary>
            /// ObjectPoolBase enumerator constructor
            /// </summary>
            /// <param name="pool">ObjectPoolBase to enumerate through</param>
            internal Enumerator(ObjectPool<T> pool)
            {
                _high = pool._highNode;
                _list = pool._pool;
                _current = default(T);

                // If the highest used node isn't a valid one, set the _node
                // to null from the start to instantly break the enumerating
                // since there are no elements to enumerate through
                if (_high == null || (_high == _list.First && !_high.Value.PoolData.IsActivated))
                    _node = null;
                else
                    _node = _list.First;
            }

            #region IEnumerator<T> Members

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator
            /// </summary>
            public T Current
            {
                get { return _current; }
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator
            /// </summary>
            object IEnumerator.Current
            {
                get { return _current; }
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection
            /// </summary>
            /// <returns>True if the enumerator was successfully advanced to the next element; 
            /// False if the enumerator has passed the end of the collection.</returns>
            public bool MoveNext()
            {
                // Use a temporary variable to store the value so we don't
                // change the value until we change to a valid value
                T newValue;

                // Loop until a null node or we break from inside the loop
                while (_node != null)
                {
                    newValue = _node.Value;

                    // If we reached the highest valid node, use the value but
                    // set the node as null which will break the enumeration
                    // on the next MoveNext()
                    if (_node == _high)
                    {
                        _node = null;
                        _current = newValue;
                        return true;
                    }

                    // Move to the next node
                    _node = _node.Next;

                    // If we found a live node, set the value as the 
                    // current value and use it
                    if (newValue.PoolData.IsActivated)
                    {
                        _current = newValue;
                        return true;
                    }
                }

                // Node null found
                return false;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before 
            /// the first element in the collection
            /// </summary>
            void IEnumerator.Reset()
            {
                _current = default(T);
                _node = _list.First;
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, 
            /// releasing, or resetting unmanaged resources
            /// </summary>
            public void Dispose()
            {
            }

            #endregion
        }
    }
}