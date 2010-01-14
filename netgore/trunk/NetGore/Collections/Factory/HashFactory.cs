using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// A cache that uses a hash table.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public class HashFactory<TKey, TValue> : IFactory<TKey, TValue> where TValue : class
    {
        readonly IDictionary<TKey, TValue> _cache;
        readonly Func<TKey, TValue> _valueCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="HashFactory&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="valueCreator">The function used to create the values for the cache.</param>
        public HashFactory(Func<TKey, TValue> valueCreator)
            : this(valueCreator, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashFactory&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="valueCreator">The function used to create the values for the cache.</param>
        /// <param name="keyComparer">The key comparer.</param>
        public HashFactory(Func<TKey, TValue> valueCreator, IEqualityComparer<TKey> keyComparer)
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
                TValue value;
                if (!_cache.TryGetValue(key, out value))
                {
                    // Create the value and add it to the cache
                    value = _valueCreator(key);
                    _cache.Add(key, value);
                }

                return value;
            }
        }

        /// <summary>
        /// Clears all of the cached items.
        /// </summary>
        public void Clear()
        {
            _cache.Clear();
        }

        /// <summary>
        /// Gets if this cache is thread-safe.
        /// </summary>
        public bool IsThreadSafe { get { return false; } }

        #endregion
    }
}