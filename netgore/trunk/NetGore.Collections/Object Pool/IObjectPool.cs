using System.Collections.Generic;
using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// Interface for an object pool.
    /// </summary>
    /// <typeparam name="T">The type of object be pooled.</typeparam>
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
}