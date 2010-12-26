using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// A thread-safe cache that uses a hash table.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public class ThreadSafeHashCache<TKey, TValue> : ICache<TKey, TValue> where TValue : class
    {
        /// <summary>
        /// The actual cache.
        /// </summary>
        readonly IDictionary<TKey, TValue> _cache;

        /// <summary>
        /// Object used to synchronize reading from and writing to the <see cref="_cache"/>.
        /// </summary>
        readonly object _cacheSync = new object();

        /// <summary>
        /// Object used to synchronize calls to <see cref="_valueCreator"/> to ensure that each cache item
        /// is only created once.
        /// </summary>
        readonly object _constructSync = new object();

        /// <summary>
        /// Func used to create the cache items.
        /// </summary>
        readonly Func<TKey, TValue> _valueCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadSafeHashCache{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="valueCreator">The function used to create the values for the cache.</param>
        /// <param name="keyComparer">The key comparer.</param>
        /// <exception cref="ArgumentNullException"><paramref name="valueCreator" /> is <c>null</c>.</exception>
        public ThreadSafeHashCache(Func<TKey, TValue> valueCreator, IEqualityComparer<TKey> keyComparer = null)
        {
            if (valueCreator == null)
                throw new ArgumentNullException("valueCreator");

            _valueCreator = valueCreator;
            _cache = new Dictionary<TKey, TValue>(keyComparer ?? EqualityComparer<TKey>.Default);
        }

        #region ICache<TKey,TValue> Members

        /// <summary>
        /// Gets the item from the cache with the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key of the item to get.</param>
        /// <returns>The value for the given <paramref name="key"/>.</returns>
        /// <exception cref="KeyNotFoundException">The key is not valid or does not exist.</exception>
        public TValue this[TKey key]
        {
            get
            {
                // Try to grab the item from the cache
                bool gotValue;
                TValue value;

                lock (_cacheSync)
                {
                    gotValue = _cache.TryGetValue(key, out value);
                }

                // If we got the value, return it
                if (gotValue)
                    return value;

                lock (_constructSync)
                {
                    // Now that we have the object construction lock, we first test again to see if the value exists
                    // in the cache since it may have been created since the last time we grabbed the cache sync. If
                    // it doesn't exist in the cache, we can create it. The _constructSync allows us to ensure
                    // only one cache item is constructed at a time. Note that the object construction lock does
                    // not have any impact on the cache reading, so we don't stall reads while we create the item.
                    // This is very important since cache items may have a large construction time. Only problem
                    // with this approach is we can't create multiple cache items at once, but that is only ever
                    // really a problem when you have a huge cache with items with a large construction time.
                    lock (_cacheSync)
                    {
                        gotValue = _cache.TryGetValue(key, out value);
                    }

                    if (!gotValue)
                    {
                        // Create the cache item instance
                        value = _valueCreator(key);

                        // Grab the cache sync so we can update the cache again
                        lock (_cacheSync)
                        {
                            Debug.Assert(!_cache.ContainsKey(key));
                            _cache.Add(key, value);
                        }
                    }
                }

                return value;
            }
        }

        /// <summary>
        /// Gets if this cache is safe to use from multiple threads at once. If this value is false, this HashCache
        /// should never be accessed from multiple threads.
        /// </summary>
        public bool IsThreadSafe
        {
            get { return true; }
        }

        /// <summary>
        /// Clears all of the cached items.
        /// </summary>
        public void Clear()
        {
            _cache.Clear();
        }

        /// <summary>
        /// Gets if the given <paramref name="key"/> is loaded in the cache. If the <paramref name="key"/> is not
        /// loaded, this method will not generate the value for the <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key to check if the value is in the cache.</param>
        /// <returns>True if the value for the given <paramref name="key"/> is in the cache; otherwise false.</returns>
        public bool ContainsKey(TKey key)
        {
            bool ret;

            lock (_cacheSync)
            {
                ret = _cache.ContainsKey(key);
            }

            return ret;
        }

        /// <summary>
        /// Gets all the cached values in this collection. The returned collection must be immutable to avoid any
        /// conflicts with if the cache changes.
        /// </summary>
        /// <returns>An immutable collection of all the cached values in this collection.</returns>
        public IEnumerable<TValue> GetCachedValues()
        {
            TValue[] values;

            lock (_cacheSync)
            {
                values = _cache.Values.ToArray();
            }

            return values;
        }

        /// <summary>
        /// Ensures all of the <paramref name="keys"/> have been loaded into the cache.
        /// </summary>
        /// <param name="keys">All of the keys to load into the cache.</param>
        public void PrepareKeys(IEnumerable<TKey> keys)
        {
            lock (_constructSync)
            {
                TKey[] keysToCreate;

                // Get the keys that need to be created so we can allow reading again asap
                lock (_cacheSync)
                {
                    keysToCreate = keys.Where(x => !_cache.ContainsKey(x)).ToArray();
                }

                if (keysToCreate.Length == 0)
                    return;

                // Create the values for all the keys
                var values = new TValue[keysToCreate.Length];
                for (var i = 0; i < keysToCreate.Length; i++)
                {
                    values[i] = _valueCreator(keysToCreate[i]);
                }

                // Grab the cache lock again so we can add the new values
                lock (_cacheSync)
                {
                    for (var i = 0; i < keysToCreate.Length; i++)
                    {
                        Debug.Assert(!_cache.ContainsKey(keysToCreate[i]));
                        _cache.Add(keysToCreate[i], values[i]);
                    }
                }
            }
        }

        #endregion
    }
}