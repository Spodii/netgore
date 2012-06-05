using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Base class for an implementation of the <see cref="IInsertQuery"/>.
    /// </summary>
    public abstract class InsertQueryBase : IInsertQuery
    {
        readonly ColumnValueCollectionBuilder<IInsertQuery> _c;
        readonly IQueryBuilderSettings _settings;
        readonly string _table;
        bool _ignoreExists;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertQueryBase"/> class.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="settings">The <see cref="IQueryBuilderSettings"/>.</param>
        /// <exception cref="InvalidQueryException"><paramref name="table"/> is an invalid table name.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> is null.</exception>
        protected InsertQueryBase(string table, IQueryBuilderSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            _table = table;
            _settings = settings;

            Settings.IsValidTableName(table, true);

            _c = new ColumnValueCollectionBuilder<IInsertQuery>(this, settings);
        }

        /// <summary>
        /// Gets the <see cref="ColumnValueCollectionBuilder{T}"/> used by this query.
        /// </summary>
        protected ColumnValueCollectionBuilder<IInsertQuery> ColumnValueCollection
        {
            get { return _c; }
        }

        /// <summary>
        /// Gets if to ignore errors when the key already exists.
        /// </summary>
        public bool IgnoreExistsValue
        {
            get { return _ignoreExists; }
        }

        /// <summary>
        /// Gets the <see cref="IQueryBuilderSettings"/> to use.
        /// </summary>
        public IQueryBuilderSettings Settings
        {
            get { return _settings; }
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        public string Table
        {
            get { return _table; }
        }

        /// <summary>
        /// When overridden in the derived class, creates an <see cref="IInsertODKUQuery"/> instance.
        /// </summary>
        /// <param name="parent">The <see cref="IInsertQuery"/> to use as the parent.</param>
        /// <returns>The <see cref="IInsertODKUQuery"/> instance.</returns>
        protected abstract IInsertODKUQuery CreateInsertODKUQuery(IInsertQuery parent);

        /// <summary>
        /// Gets the column name and value pairs for the query.
        /// </summary>
        /// <returns>The column name and value pairs for the query.</returns>
        public KeyValuePair<string, string>[] GetColumnValueCollectionValues()
        {
            return ColumnValueCollection.GetValues();
        }

        #region IInsertQuery Members

        /// <summary>
        /// Adds a column name and value pair to the collection. If the <see cref="column"/> already exists,
        /// it will be changed to use the new <see cref="value"/>.
        /// </summary>
        /// <param name="column">The name of the column.</param>
        /// <param name="value">The value for the <paramref name="column"/>.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="column"/> is not a valid column name.</exception>
        public IInsertQuery Add(string column, string value)
        {
            return _c.Add(column, value);
        }

        /// <summary>
        /// Adds a column name and value pair to the collection, where the key is the column name and the value is the
        /// column value. If the column already exists, it will be changed to use the new value.
        /// </summary>
        /// <param name="columnAndValue">The column name and value to add.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columnAndValue"/> contains an invalid column name.</exception>
        public IInsertQuery Add(KeyValuePair<string, string> columnAndValue)
        {
            return _c.Add(columnAndValue);
        }

        /// <summary>
        /// Adds multiple column name and value pairs to the collection, where the key is the column name and the value is the
        /// column value. If the column already exists, it will be changed to use the new value.
        /// </summary>
        /// <param name="columnsAndValues">The column name and value pairs to add.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException">One or more value in <paramref name="columnsAndValues"/> contains an
        /// invalid column name.</exception>
        public IInsertQuery Add(IEnumerable<KeyValuePair<string, string>> columnsAndValues)
        {
            return _c.Add(columnsAndValues);
        }

        /// <summary>
        /// Adds multiple column name and value pairs to the collection, where the key is the column name and the value is the
        /// column value. If the column already exists, it will be changed to use the new value.
        /// </summary>
        /// <param name="columnsAndValues">The column name and value pairs to add.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException">One or more value in <paramref name="columnsAndValues"/> contains an
        /// invalid column name.</exception>
        public IInsertQuery Add(params KeyValuePair<string, string>[] columnsAndValues)
        {
            return _c.Add(columnsAndValues);
        }

        /// <summary>
        /// Adds a column where the value is automatically created as a query parameter. The name of the query parameter
        /// will be the same as the name of the column.
        /// </summary>
        /// <param name="column">The name of the column.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="column"/> is not a valid column name.</exception>
        public IInsertQuery AddAutoParam(string column)
        {
            return _c.AddAutoParam(column);
        }

        /// <summary>
        /// Adds multiple columns where the value is automatically created as a query parameter. The name of the query parameter
        /// will be the same as the name of the column.
        /// </summary>
        /// <param name="columns">The names of the columns.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        public IInsertQuery AddAutoParam(params string[] columns)
        {
            return _c.AddAutoParam(columns);
        }

        /// <summary>
        /// Adds multiple columns where the value is automatically created as a query parameter. The name of the query parameter
        /// will be the same as the name of the column.
        /// </summary>
        /// <param name="columns">The names of the columns.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        public IInsertQuery AddAutoParam(IEnumerable<string> columns)
        {
            return _c.AddAutoParam(columns);
        }

        /// <summary>
        /// Adds a column name and value pair to the collection where the value will be turned into a query parameter.
        /// </summary>
        /// <param name="column">The name of the column.</param>
        /// <param name="value">The name to give the query parameter.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="column"/> is not a valid column name.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="value"/> is an invalid parameter name.</exception>
        public IInsertQuery AddParam(string column, string value)
        {
            return _c.AddParam(column, value);
        }

        /// <summary>
        /// Adds a column name and value pair to the collection where the value will be turned into a query parameter.
        /// </summary>
        /// <param name="columnAndValue">The column name and query parameter name pair.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columnAndValue"/> contains an invalid column name.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="columnAndValue"/> is an invalid parameter name.</exception>
        public IInsertQuery AddParam(KeyValuePair<string, string> columnAndValue)
        {
            return _c.AddParam(columnAndValue);
        }

        /// <summary>
        /// Adds multiple column name and value pairs to the collection where the value will be turned into a query parameter.
        /// </summary>
        /// <param name="columnsAndValues">The column name and query parameter name pairs.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException">One or more values in <paramref name="columnsAndValues"/> contains an
        /// invalid column name.</exception>
        /// <exception cref="InvalidQueryException">One or more values in <paramref name="columnsAndValues"/> contains an
        /// invalid parameter name.</exception>
        public IInsertQuery AddParam(IEnumerable<KeyValuePair<string, string>> columnsAndValues)
        {
            return _c.AddParam(columnsAndValues);
        }

        /// <summary>
        /// Adds multiple column name and value pairs to the collection where the value will be turned into a query parameter.
        /// </summary>
        /// <param name="columnsAndValues">The column name and query parameter name pairs.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException">One or more values in <paramref name="columnsAndValues"/> contains an
        /// invalid column name.</exception>
        /// <exception cref="InvalidQueryException">One or more values in <paramref name="columnsAndValues"/> contains an
        /// invalid parameter name.</exception>
        public IInsertQuery AddParam(params KeyValuePair<string, string>[] columnsAndValues)
        {
            return _c.AddParam(columnsAndValues);
        }

        /// <summary>
        /// Makes it so the INSERT query does not throw an error when the key already exists.
        /// </summary>
        /// <returns>The <see cref="IInsertODKUQuery"/>.</returns>
        public IInsertQuery IgnoreExists()
        {
            _ignoreExists = true;

            return this;
        }

        /// <summary>
        /// Creates an ON DUPLICATE KEY UPDATE clause for the INSERT statement. This makes is so that when you try to insert
        /// into a table with a key that already exists, instead of the query failing, it will update the values of the
        /// existing row.
        /// </summary>
        /// <returns>The <see cref="IInsertODKUQuery"/>.</returns>
        public IInsertODKUQuery ODKU()
        {
            return CreateInsertODKUQuery(this);
        }

        /// <summary>
        /// Removes multiple columns from from the collection.
        /// </summary>
        /// <param name="columns">The names of the columns to remove.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        public IInsertQuery Remove(IEnumerable<string> columns)
        {
            return _c.Remove(columns);
        }

        /// <summary>
        /// Removes multiple columns from from the collection.
        /// </summary>
        /// <param name="columns">The names of the columns to remove.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        public IInsertQuery Remove(params string[] columns)
        {
            return _c.Remove(columns);
        }

        /// <summary>
        /// Removes a column from from the collection.
        /// </summary>
        /// <param name="column">The name of the column to remove.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="column"/> is not a valid column name.</exception>
        public IInsertQuery Remove(string column)
        {
            return _c.Remove(column);
        }

        #endregion
    }
}