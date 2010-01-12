using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NetGore.Collections
{
    /// <summary>
    /// A basic thread-safe factory that uses the same value for every key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public abstract class ThreadSafeSingletonFactory<TKey, TValue> : IFactory<TKey, TValue>
    {
        readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        readonly Dictionary<TKey, TValue> _cache = new Dictionary<TKey, TValue>();

        /// <summary>
        /// When overridden in the derived class, creates the value for the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key to create the value for.</param>
        /// <returns></returns>
        protected abstract TValue CreateInstance(TKey key);

        /// <summary>
        /// Gets the value for the given <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value for the given <paramref name="key"/>.</returns>
        public TValue Get(TKey key)
        {
            TValue value;
            bool containsValue;

            _lock.EnterReadLock();
            try
            {
                containsValue = _cache.TryGetValue(key, out value);
            }
            finally
            {
                _lock.ExitReadLock();
            }

            if (!containsValue)
            {
                value = CreateInstance(key);

                _lock.EnterWriteLock();
                try
                {
                    if (!_cache.ContainsKey(key))
                    {
                        _cache.Add(key, value);
                    }
                    else
                    {
                        value = _cache[key];
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
}
