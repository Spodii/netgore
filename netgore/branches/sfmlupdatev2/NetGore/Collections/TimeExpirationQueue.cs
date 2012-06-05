using System.Collections.Generic;
using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// A collection that keeps track of when items were added to it. After the given elapsed amount of time, the item
    /// will be removed from the collection, and perform additional processing as defined by the derived class.
    /// </summary>
    /// <typeparam name="T">The type of collection item.</typeparam>
    public abstract class TimeExpirationQueue<T>
    {
        readonly Dictionary<T, TickCount> _items = new Dictionary<T, TickCount>();

        TickCount _lastUpdateTime = TickCount.MinValue;

        /// <summary>
        /// Gets a deep copy of the items in this collection and the time at which they will expire.
        /// </summary>
        public IEnumerable<KeyValuePair<T, TickCount>> Items
        {
            get { return _items.ToImmutable(); }
        }

        /// <summary>
        /// Gets the time that this collection was last successfully updated.
        /// </summary>
        public TickCount LastUpdated
        {
            get { return _lastUpdateTime; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the minimum amount of time in milliseconds that must elapsed
        /// between calls to Update. If this amount of time has not elapsed, calls to Update will just return 0.
        /// </summary>
        protected abstract TickCount UpdateRate { get; }

        /// <summary>
        /// Adds an item to start tracking. If the <paramref name="item"/> already exists in the collection, its
        /// time until deletion will be reset.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <param name="currentTime">The current time.</param>
        /// <param name="life">How long the <paramref name="item"/> will live in the collection before expiring.</param>
        public void Add(T item, TickCount currentTime, uint life)
        {
            var v = currentTime + life;

            if (!_items.ContainsKey(item))
                _items.Add(item, v);
            else
                _items[item] = v;
        }

        /// <summary>
        /// Clears the collection.
        /// </summary>
        /// <param name="expire">If true, all items will also be expired. If false, they will only be removed
        /// from the collection.</param>
        public void Clear(bool expire)
        {
            if (expire)
            {
                foreach (var item in _items.Keys)
                {
                    ExpireItem(item);
                }
            }

            _items.Clear();
        }

        /// <summary>
        /// When overridden in the derived class, handles when an item has expired since it has been in this collection
        /// for longer the allowed time.
        /// </summary>
        /// <param name="item">The item that has expired.</param>
        protected abstract void ExpireItem(T item);

        /// <summary>
        /// Removes an item from the collection.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>True if the item was successfully removed; false if the <paramref name="item"/> was not
        /// in the collection.</returns>
        public bool Remove(T item)
        {
            return _items.Remove(item);
        }

        /// <summary>
        /// Removes the expired map items.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <returns>The number of items that were removed. Will be zero if not enough time has elapsed since the last
        /// call to update, or if just no items were removed.</returns>
        public int Update(TickCount currentTime)
        {
            // Check if enough time has elapsed to update
            if (_lastUpdateTime + UpdateRate > currentTime)
                return 0;

            _lastUpdateTime = currentTime;

            // Get the items to remove
            var toRemove = _items.Where(x => x.Value <= currentTime).Select(x => x.Key).ToArray();
            if (toRemove == null || toRemove.Length == 0)
                return 0;

            // Start removing the items
            foreach (var item in toRemove)
            {
                // Remove from the collection
                _items.Remove(item);

                // Perform the expiration of the item
                ExpireItem(item);
            }

            return toRemove.Length;
        }
    }
}