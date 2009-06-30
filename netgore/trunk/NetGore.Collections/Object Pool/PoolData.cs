using System.Collections.Generic;

namespace NetGore.Collections
{
    /// <summary>
    /// Contains the pool information for an individual item in the pool
    /// </summary>
    /// <typeparam name="T">Type of the object being placed in the pool (often the same as the
    /// class the interface is placed on)</typeparam>
    public sealed class PoolData<T> where T : IPoolable<T>, new()
    {
        readonly int _poolIndex;

        readonly LinkedListNode<T> _poolNode;
        bool _isActivated = false;

        /// <summary>
        /// Gets if the pool item is currently activated
        /// </summary>
        public bool IsActivated
        {
            get { return _isActivated; }
        }

        /// <summary>
        /// Gets the item associated with this pool data
        /// </summary>
        public T Item
        {
            get { return _poolNode.Value; }
        }

        /// <summary>
        /// Gets the index of the item in the pool
        /// </summary>
        public int PoolIndex
        {
            get { return _poolIndex; }
        }

        /// <summary>
        /// Gets the LinkedListNode for this item in the pool
        /// </summary>
        internal LinkedListNode<T> PoolNode
        {
            get { return _poolNode; }
        }

        /// <summary>
        /// PoolData constructor
        /// </summary>
        /// <param name="node">LinkedListNode for this item in the pool</param>
        /// <param name="index">Index of the item in the pool</param>
        internal PoolData(LinkedListNode<T> node, int index)
        {
            _poolIndex = index;
            _poolNode = node;
        }

        /// <summary>
        /// Notifies the item to be activated (call this instead of IPoolable.Activate() 
        /// to ensure IsActivated is set)
        /// </summary>
        internal void Activate()
        {
            _isActivated = true;
            Item.Activate();
        }

        /// <summary>
        /// Notifies the item to be deactivated (call this instead of IPoolable.Deactivate() 
        /// to ensure IsActivated is set)
        /// </summary>
        internal void Deactivate()
        {
            _isActivated = false;
            Item.Deactivate();
        }
    }
}