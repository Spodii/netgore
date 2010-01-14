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