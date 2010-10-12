using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Base class for an implementation of the <see cref="ICallProcedureQuery"/>.
    /// </summary>
    public abstract class CallProcedureQueryBase : ICallProcedureQuery
    {
        readonly ValueCollectionBuilder<ICallProcedureQuery> _c;
        readonly string _procedure;
        readonly IQueryBuilderSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectQueryBase"/> class.
        /// </summary>
        /// <param name="procedure">The name of the stored procedure.</param>
        /// <param name="settings">The <see cref="IQueryBuilderSettings"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="procedure"/> is null or empty.</exception>
        protected CallProcedureQueryBase(string procedure, IQueryBuilderSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            if (string.IsNullOrEmpty(procedure))
                throw new ArgumentNullException("procedure");

            _procedure = procedure;
            _settings = settings;

            _c = new ValueCollectionBuilder<ICallProcedureQuery>(this, settings);
        }

        /// <summary>
        /// Gets the procedure name.
        /// </summary>
        public string Procedure
        {
            get { return _procedure; }
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
        protected ValueCollectionBuilder<ICallProcedureQuery> ValueCollection
        {
            get { return _c; }
        }

        #region ICallProcedureQuery Members

        /// <summary>
        /// Adds a raw value to the collection.
        /// </summary>
        /// <param name="value">The raw value string.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public ICallProcedureQuery Add(string value)
        {
            return _c.Add(value);
        }

        /// <summary>
        /// Adds raw values to the collection.
        /// </summary>
        /// <param name="values">The raw value strings.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
        public ICallProcedureQuery Add(IEnumerable<string> values)
        {
            return _c.Add(values);
        }

        /// <summary>
        /// Adds raw values to the collection.
        /// </summary>
        /// <param name="values">The raw value strings.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
        public ICallProcedureQuery Add(params string[] values)
        {
            return _c.Add(values);
        }

        /// <summary>
        /// Adds a parameterized value to the collection.
        /// </summary>
        /// <param name="value">The name of the parameter.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public ICallProcedureQuery AddParam(string value)
        {
            return _c.AddParam(value);
        }

        /// <summary>
        /// Adds parameterized values to the collection.
        /// </summary>
        /// <param name="values">The names of the parameters.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
        public ICallProcedureQuery AddParam(IEnumerable<string> values)
        {
            return _c.AddParam(values);
        }

        /// <summary>
        /// Adds parameterized values to the collection.
        /// </summary>
        /// <param name="values">The names of the parameters.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
        public ICallProcedureQuery AddParam(params string[] values)
        {
            return _c.AddParam(values);
        }

        #endregion
    }
}