using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Builds a collection of values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValueCollectionBuilder<T> : IValueCollectionBuilder<T>
    {
        readonly List<string> _c = new List<string>();
        readonly T _owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueCollectionBuilder{T}"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <exception cref="ArgumentNullException"><paramref name="owner"/> is null.</exception>
        public ValueCollectionBuilder(T owner)
        {
            // ReSharper disable CompareNonConstrainedGenericWithNull
            if (owner == null)
                throw new ArgumentNullException("owner");
            // ReSharper restore CompareNonConstrainedGenericWithNull

            _owner = owner;
        }

        /// <summary>
        /// Gets the values in this collection.
        /// </summary>
        /// <returns>The values in this collection.</returns>
        public string[] GetValues()
        {
            return _c.ToArray();
        }

        #region IValueCollectionBuilder<T> Members

        /// <summary>
        /// Adds a raw value to the collection.
        /// </summary>
        /// <param name="value">The raw value string.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public T Add(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            _c.Add(value);

            return _owner;
        }

        /// <summary>
        /// Adds raw values to the collection.
        /// </summary>
        /// <param name="values">The raw value strings.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
        public T Add(IEnumerable<string> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            foreach (var v in values)
            {
                Add(v);
            }

            return _owner;
        }

        /// <summary>
        /// Adds raw values to the collection.
        /// </summary>
        /// <param name="values">The raw value strings.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
        public T Add(params string[] values)
        {
            return Add((IEnumerable<string>)values);
        }

        #endregion
    }
}