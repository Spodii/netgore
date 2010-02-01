using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace NetGore.Collections
{
    /// <summary>
    /// A thread-safe cache that uses a hash table.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public class ThreadSafeHashCache<TKey, TValue> : ICache<TKey, TValue> where TValue : class
    {
        readonly IDictionary<TKey, TValue> _cache;
        readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        readonly Func<TKey, TValue> _valueCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadSafeHashCache{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="valueCreator">The function used to create the values for the cache.</param>
        public ThreadSafeHashCache(Func<TKey, TValue> valueCreator) : this(valueCreator, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadSafeHashCache{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="valueCreator">The function used to create the values for the cache.</param>
        /// <param name="keyComparer">The key comparer.</param>
        public ThreadSafeHashCache(Func<TKey, TValue> valueCreator, IEqualityComparer<TKey> keyComparer)
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
                _lock.EnterReadLock();
                try
                {
                    gotValue = _cache.TryGetValue(key, out value);
                }
                finally
                {
                    _lock.ExitReadLock();
                }

                // If we didn't get the value, we will try again to grab it with a write lock to make sure nobody
                // created it between our last attempt and now. This will make sure we don't create the value
                // more than once for the same key.
                if (!gotValue)
                {
                    _lock.EnterWriteLock();

                    try
                    {
                        if (!_cache.TryGetValue(key, out value))
                        {
                            // Yet again, we didn't get the value. But since we have exclusive access to the cache, we
                            // know that we can create it and add it without anyone else trying to touch the cache. This
                            // is less than ideal performance for adding since we are preventing anyone else touching
                            // the cache while we create and add the value, but this should not be an issue since creates
                            // are a one-time thing.
                            value = _valueCreator(key);
                            Debug.Assert(!_cache.ContainsKey(key));
                            _cache.Add(key, value);
                        }
                    }
                    finally
                    {
                        _lock.ExitWriteLock();
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
            get { return false; }
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

            _lock.EnterReadLock();
            try
            {
                ret = _cache.ContainsKey(key);
            }
            finally
            {
                _lock.ExitReadLock();
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

            _lock.EnterReadLock();
            try
            {
                values = _cache.Values.ToArray();
            }
            finally
            {
                _lock.ExitReadLock();
            }

            return values;
        }

        /// <summary>
        /// Ensures all of the <paramref name="keys"/> have been loaded into the cache.
        /// </summary>
        /// <param name="keys">All of the keys to load into the cache.</param>
        public void PrepareKeys(IEnumerable<TKey> keys)
        {
            _lock.EnterWriteLock();
            try
            {
                foreach (var key in keys)
                {
                    if (!_cache.ContainsKey(key))
                    {
                        var value = _valueCreator(key);
                        Debug.Assert(!_cache.ContainsKey(key));
                        _cache.Add(key, value);
                    }
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        #endregion
    }
}