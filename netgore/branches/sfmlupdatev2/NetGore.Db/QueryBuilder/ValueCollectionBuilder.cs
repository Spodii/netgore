using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Builds a collection of values.
    /// </summary>
    /// <typeparam name="T">The type of return value.</typeparam>
    public class ValueCollectionBuilder<T> : IValueCollectionBuilder<T>
    {
        readonly List<string> _c = new List<string>();
        readonly T _owner;
        readonly IQueryBuilderSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueCollectionBuilder{T}"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="settings">The <see cref="IQueryBuilderSettings"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="owner"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> is null.</exception>
        public ValueCollectionBuilder(T owner, IQueryBuilderSettings settings)
        {
            // ReSharper disable CompareNonConstrainedGenericWithNull
            if (owner == null)
                throw new ArgumentNullException("owner");
            // ReSharper restore CompareNonConstrainedGenericWithNull

            if (settings == null)
                throw new ArgumentNullException("settings");

            _settings = settings;
            _owner = owner;
        }

        /// <summary>
        /// Gets the <see cref="IQueryBuilderSettings"/> to use.
        /// </summary>
        public IQueryBuilderSettings Settings
        {
            get { return _settings; }
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

        /// <summary>
        /// Adds a parameterized value to the collection.
        /// </summary>
        /// <param name="value">The name of the parameter.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public T AddParam(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            _c.Add(Settings.Parameterize(value));

            return _owner;
        }

        /// <summary>
        /// Adds parameterized values to the collection.
        /// </summary>
        /// <param name="values">The names of the parameters.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
        public T AddParam(IEnumerable<string> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            foreach (var v in values)
            {
                AddParam(v);
            }

            return _owner;
        }

        /// <summary>
        /// Adds parameterized values to the collection.
        /// </summary>
        /// <param name="values">The names of the parameters.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
        public T AddParam(params string[] values)
        {
            return AddParam((IEnumerable<string>)values);
        }

        #endregion
    }
}