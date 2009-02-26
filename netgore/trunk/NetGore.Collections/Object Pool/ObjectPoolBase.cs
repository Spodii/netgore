using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore.Collections
{
    /// <summary>
    /// Basic object pool layout. More complex pools can override this to allow for overriding of the
    /// Create() and Destroy() methods to do some custom handling of individual items (ie setting parameters)
    /// </summary>
    /// <typeparam name="T">Type of the item to be pooled</typeparam>
    public class ObjectPool<T> : IEnumerable<T> where T : IPoolable<T>, new()
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
        /// Object used to lock the pool on thread-unsafe sections of code
        /// </summary>
        readonly object _poolLock = new object();

        /// <summary>
        /// Highest used node in the pool, which lets us end enumerating earlier. A null value
        /// means that the whole pool will be iterated through.
        /// </summary>
        LinkedListNode<T> _highNode = null;

        /// <summary>
        /// Clears all nodes, dead and alive, from the pool
        /// </summary>
        /// <param name="destroy">If true, the Destroy() method will be raised on all
        /// the alive objects</param>
        public void Clear(bool destroy)
        {
            // Call Destroy() on the alive nodes if needed
            if (destroy)
            {
                foreach (T obj in this)
                {
                    obj.Deactivate();
                }
            }

            // Remove the nodes
            lock (_poolLock)
            {
                _pool.Clear();
            }
        }

        /// <summary>
        /// Clears all nodes, dead and alive, from the pool
        /// </summary>
        public void Clear()
        {
            Clear(true);
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

        /// <summary>
        /// Gets an item to use from the pool
        /// </summary>
        /// <returns>Item to use</returns>
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

                lock (_poolLock)
                {
                    // Add the node to the end of the item pool list
                    _pool.AddLast(node);

                    // This is clearly the highest node in use since it has the highest index
                    _highNode = node;
                }
            }
            else
            {
                item = node.Value;
                poolData = item.PoolData;

                lock (_poolLock)
                {
                    // If the index of this node is higher than the index of the highest node,
                    // set this node to the highest node
                    if (poolData.PoolIndex > _highNode.Value.PoolData.PoolIndex)
                        _highNode = node;
                }
            }

            // Activate the item
            poolData.Activate();

            return item;
        }

        /// <summary>
        /// Deactivates the item, placing it back in the pool for later use
        /// </summary>
        /// <param name="item">Item to deactivate</param>
        public virtual void Destroy(T item)
        {
            var poolData = item.PoolData;
            var node = poolData.PoolNode;

            if (node == null)
            {
                const string errmsg = "item.PoolData.PoolNode is null.";
                Debug.Fail(errmsg);
                if (log.IsFatalEnabled)
                    log.Fatal(errmsg);
                throw new Exception(errmsg);
            }

            // Call the deactivation
            poolData.Deactivate();

            lock (_poolLock)
            {
                // Push to the free list for later use
                _free.Push(node);

                // If we destroyed the highest node, find the next highest node
                if (node == _highNode)
                    GetNextHighNode();
            }
        }

        /// <summary>
        /// Gets the next free node
        /// </summary>
        /// <returns>Next free LinkedListNode if one exists, else null</returns>
        LinkedListNode<T> GetFree()
        {
            lock (_poolLock)
            {
                // If the stack count is 0, theres no free nodes
                if (_free.Count == 0)
                    return null;

                // Pop off and return the next item in the stack
                return _free.Pop();
            }
        }

        /// <summary>
        /// Updates the HighNode by looping backwards through the nodes until either
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

        #region IEnumerable<T> Members

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