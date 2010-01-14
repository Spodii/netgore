using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// A thread-safe cache that uses a hash table.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public class ThreadSafeHashFactory<TKey, TValue> : HashFactoryBase<TKey, TValue> where TValue : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadSafeHashFactory&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="valueCreator">The function used to create the values for the cache.</param>
        public ThreadSafeHashFactory(Func<TKey, TValue> valueCreator) : base(valueCreator)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadSafeHashFactory&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="valueCreator">The function used to create the values for the cache.</param>
        /// <param name="keyComparer">The key comparer.</param>
        public ThreadSafeHashFactory(Func<TKey, TValue> valueCreator, IEqualityComparer<TKey> keyComparer)
            : base(valueCreator, keyComparer)
        {
        }

        /// <summary>
        /// Gets if this cache is thread-safe.
        /// </summary>
        public override bool IsThreadSafe
        {
            get { return true; }
        }

        /// <summary>
        /// When overridden in the derived class, creates the IDictionary used for the cache.
        /// </summary>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns>The IDictionary used for the cache.</returns>
        protected override IDictionary<TKey, TValue> CreateCache(IEqualityComparer<TKey> equalityComparer)
        {
            return new TSDictionary<TKey, TValue>(equalityComparer);
        }
    }

    /// <summary>
    /// Base class for a cache that uses a hash table.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public abstract class HashFactoryBase<TKey, TValue> : IFactory<TKey, TValue> where TValue : class
    {
        readonly IDictionary<TKey, TValue> _cache;
        readonly Func<TKey, TValue> _valueCreator;

        /// <summary>
        /// Initializes a new instance of the <see cref="HashFactoryBase&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="valueCreator">The function used to create the values for the cache.</param>
        protected HashFactoryBase(Func<TKey, TValue> valueCreator) : this(valueCreator, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashFactoryBase&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="valueCreator">The function used to create the values for the cache.</param>
        /// <param name="keyComparer">The key comparer.</param>
        protected HashFactoryBase(Func<TKey, TValue> valueCreator, IEqualityComparer<TKey> keyComparer)
        {
            if (valueCreator == null)
                throw new ArgumentNullException("valueCreator");

            _valueCreator = valueCreator;

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            _cache = CreateCache(keyComparer ?? EqualityComparer<TKey>.Default);
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// When overridden in the derived class, creates the IDictionary used for the cache.
        /// </summary>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns>The IDictionary used for the cache.</returns>
        protected abstract IDictionary<TKey, TValue> CreateCache(IEqualityComparer<TKey> equalityComparer);

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
                if (_cache.TryGetValue(key, out value))
                    return value;

                // Create the value and add it to the cache
                value = _valueCreator(key);
                _cache.Add(key, value);

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
        public abstract bool IsThreadSafe { get; }

        #endregion
    }

    /// <summary>
    /// A cache that uses a hash table.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public class HashFactory<TKey, TValue> : HashFactoryBase<TKey, TValue> where TValue : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HashFactory&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="valueCreator">The function used to create the values for the cache.</param>
        public HashFactory(Func<TKey, TValue> valueCreator) : base(valueCreator)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashFactory&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        /// <param name="valueCreator">The function used to create the values for the cache.</param>
        /// <param name="keyComparer">The key comparer.</param>
        public HashFactory(Func<TKey, TValue> valueCreator, IEqualityComparer<TKey> keyComparer) : base(valueCreator, keyComparer)
        {
        }

        /// <summary>
        /// Gets if this cache is thread-safe.
        /// </summary>
        /// <value></value>
        public override bool IsThreadSafe
        {
            get { return true; }
        }

        /// <summary>
        /// When overridden in the derived class, creates the IDictionary used for the cache.
        /// </summary>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns>The IDictionary used for the cache.</returns>
        protected override IDictionary<TKey, TValue> CreateCache(IEqualityComparer<TKey> equalityComparer)
        {
            return new Dictionary<TKey, TValue>(equalityComparer);
        }
    }
}