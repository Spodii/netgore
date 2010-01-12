using System.Collections.Generic;
using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// A basic non-thread-safe factory that uses the same value for every key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public abstract class SingletonFactory<TKey, TValue> : IFactory<TKey, TValue>
    {
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

            if (!_cache.TryGetValue(key, out value))
            {
                value = CreateInstance(key);
                _cache.Add(key, value);
            }

            return value;
        }
    }
}