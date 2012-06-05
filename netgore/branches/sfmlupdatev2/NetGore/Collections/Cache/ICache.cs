using System.Collections.Generic;
using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// Interface for an object that caches instances of objects for their corresponding key. Values are created
    /// on-demand as their key is requested, and never re-created unless the collection is cleared.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public interface ICache<in TKey, out TValue> where TValue : class
    {
        /// <summary>
        /// Gets the item from the cache with the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of the item to get.</param>
        /// <returns>The value for the given <paramref name="key"/>, or null if the key is invalid.</returns>
        TValue this[TKey key] { get; }

        /// <summary>
        /// Gets if this cache is safe to use from multiple threads at once. If this value is false, this HashCache
        /// should never be accessed from multiple threads.
        /// </summary>
        bool IsThreadSafe { get; }

        /// <summary>
        /// Clears all of the cached items.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets if the given <paramref name="key"/> is loaded in the cache. If the <paramref name="key"/> is not
        /// loaded, this method will not generate the value for the <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key to check if the value is in the cache.</param>
        /// <returns>True if the value for the given <paramref name="key"/> is in the cache; otherwise false.</returns>
        bool ContainsKey(TKey key);

        /// <summary>
        /// Gets all the cached values in this collection. The returned collection must be immutable to avoid any
        /// conflicts with if the cache changes.
        /// </summary>
        /// <returns>An immutable collection of all the cached values in this collection.</returns>
        IEnumerable<TValue> GetCachedValues();

        /// <summary>
        /// Ensures all of the <paramref name="keys"/> have been loaded into the cache.
        /// </summary>
        /// <param name="keys">All of the keys to load into the cache.</param>
        void PrepareKeys(IEnumerable<TKey> keys);
    }
}