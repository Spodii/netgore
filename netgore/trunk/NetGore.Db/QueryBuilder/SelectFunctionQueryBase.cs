using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Base class for an implementation of the <see cref="ISelectFunctionQuery"/>.
    /// </summary>
    public abstract class SelectFunctionQueryBase : ISelectFunctionQuery
    {
        readonly ValueCollectionBuilder<ISelectFunctionQuery> _c;
        readonly string _function;
        readonly IQueryBuilderSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectQueryBase"/> class.
        /// </summary>
        /// <param name="function">The table.</param>
        /// <param name="settings">The <see cref="IQueryBuilderSettings"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="function"/> is null or empty.</exception>
        protected SelectFunctionQueryBase(string function, IQueryBuilderSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            if (string.IsNullOrEmpty(function))
                throw new ArgumentNullException("function");

            _function = function;
            _settings = settings;

            _c = new ValueCollectionBuilder<ISelectFunctionQuery>(this);
        }

        /// <summary>
        /// Gets the function name.
        /// </summary>
        public string Function
        {
            get { return _function; }
        }

        /// <summary>
        /// Gets the <see cref="IQueryBuilderSettings"/> to use.
        /// </summary>
        public IQueryBuilderSettings Settings
        {
            get { return _settings; }
        }

        /// <summary>
        /// Gets the <see cref="ValueCollectionBuilder{T}"/> used by this object.
        /// </summary>
        protected ValueCollectionBuilder<ISelectFunctionQuery> ValueCollection
        {
            get { return _c; }
        }

        #region ISelectFunctionQuery Members

        /// <summary>
        /// Adds a raw value to the collection.
        /// </summary>
        /// <param name="value">The raw value string.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public ISelectFunctionQuery Add(string value)
        {
            return _c.Add(value);
        }

        /// <summary>
        /// Adds raw values to the collection.
        /// </summary>
        /// <param name="values">The raw value strings.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
        public ISelectFunctionQuery Add(IEnumerable<string> values)
        {
            return _c.Add(values);
        }

        /// <summary>
        /// Adds raw values to the collection.
        /// </summary>
        /// <param name="values">The raw value strings.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
        public ISelectFunctionQuery Add(params string[] values)
        {
            return _c.Add(values);
        }

        #endregion
    }
}