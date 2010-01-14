using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Collections
{
    /// <summary>
    /// Interface for an object that caches instances of objects for their corresponding key. Values are created
    /// on-demand as their key is requested, and never re-created unless the collection is cleared.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public interface ICache<TKey, TValue> where TValue : class
    {
        /// <summary>
        /// Gets the item from the cache with the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of the item to get.</param>
        /// <returns>The value for the given <paramref name="key"/>, or null if the key is invalid.</returns>
        TValue this[TKey key] { get; }

        /// <summary>
        /// Clears all of the cached items.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets if this cache is safe to use from multiple threads at once. If this value is false, this HashCache
        /// should never be accessed from multiple threads.
        /// </summary>
        bool IsThreadSafe { get; }
    }
}
