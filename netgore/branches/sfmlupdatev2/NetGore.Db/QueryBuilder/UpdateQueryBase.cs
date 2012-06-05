using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Base class for an implementation of the <see cref="IUpdateQuery"/>.
    /// </summary>
    public abstract class UpdateQueryBase : IUpdateQuery
    {
        readonly ColumnValueCollectionBuilder<IUpdateQuery> _c;
        readonly IQueryBuilderSettings _settings;
        readonly string _table;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateQueryBase"/> class.
        /// </summary>
        /// <param name="table">The table name.</param>
        /// <param name="settings">The <see cref="IQueryBuilderSettings"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> is null.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="table"/> is not a valid table name.</exception>
        protected UpdateQueryBase(string table, IQueryBuilderSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            _table = table;
            _settings = settings;

            Settings.IsValidTableName(table, true);

            _c = new ColumnValueCollectionBuilder<IUpdateQuery>(this, settings);
        }

        /// <summary>
        /// Gets the <see cref="ColumnValueCollectionBuilder{T}"/> used by this object.
        /// </summary>
        protected ColumnValueCollectionBuilder<IUpdateQuery> ColumnValueCollection
        {
            get { return _c; }
        }

        /// <summary>
        /// Gets the <see cref="IQueryBuilderSettings"/> to use.
        /// </summary>
        public IQueryBuilderSettings Settings
        {
            get { return _settings; }
        }

        /// <summary>
        /// Gets the table name.
        /// </summary>
        public string Table
        {
            get { return _table; }
        }

        /// <summary>
        /// When overridden in the derived class, creates an <see cref="IQueryResultFilter"/> instance.
        /// </summary>
        /// <param name="parent">The parent object for the <see cref="IQueryResultFilter"/>.</param>
        /// <returns>The <see cref="IQueryResultFilter"/> instance.</returns>
        protected abstract IQueryResultFilter CreateQueryResultFilter(object parent);

        /// <summary>
        /// Gets the column name and value pairs in this query.
        /// </summary>
        /// <returns>The column name and value pairs in this query.</returns>
        public KeyValuePair<string, string>[] GetColumnValueCollectionValues()
        {
            return ColumnValueCollection.GetValues();
        }

        #region IUpdateQuery Members

        /// <summary>
        /// Adds a column name and value pair to the collection. If the <see cref="column"/> already exists,
        /// it will be changed to use the new <see cref="value"/>.
        /// </summary>
        /// <param name="column">The name of the column.</param>
        /// <param name="value">The value for the <paramref name="column"/>.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="column"/> is not a valid column name.</exception>
        public IUpdateQuery Add(string column, string value)
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
        public IUpdateQuery Add(KeyValuePair<string, string> columnAndValue)
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
        public IUpdateQuery Add(IEnumerable<KeyValuePair<string, string>> columnsAndValues)
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
        public IUpdateQuery Add(params KeyValuePair<string, string>[] columnsAndValues)
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
        public IUpdateQuery AddAutoParam(string column)
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
        public IUpdateQuery AddAutoParam(params string[] columns)
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
        public IUpdateQuery AddAutoParam(IEnumerable<string> columns)
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
        public IUpdateQuery AddParam(string column, string value)
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
        public IUpdateQuery AddParam(KeyValuePair<string, string> columnAndValue)
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
        public IUpdateQuery AddParam(IEnumerable<KeyValuePair<string, string>> columnsAndValues)
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
        public IUpdateQuery AddParam(params KeyValuePair<string, string>[] columnsAndValues)
        {
            return _c.AddParam(columnsAndValues);
        }

        /// <summary>
        /// Limits the number of rows being returned or operated on.
        /// </summary>
        /// <param name="amount">The row limit. Must be greater than 0.</param>
        /// <returns>The <see cref="IQueryResultFilter"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="amount"/> is not greater than 0.</exception>
        public IQueryResultFilter Limit(int amount)
        {
            return CreateQueryResultFilter(this).Limit(amount);
        }

        /// <summary>
        /// Orders the rows in the query.
        /// </summary>
        /// <param name="value">The condition to order by.</param>
        /// <param name="order">The direction to order the results.</param>
        /// <returns>The <see cref="IQueryResultFilter"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null or empty.</exception>
        /// <exception cref="ArgumentException"><paramref name="order"/> is not a defined <see cref="OrderByType"/> enum value.</exception>
        public IQueryResultFilter OrderBy(string value, OrderByType order = OrderByType.Ascending)
        {
            return CreateQueryResultFilter(this).OrderBy(value, order);
        }

        /// <summary>
        /// Orders the rows in the query based on a single column.
        /// </summary>
        /// <param name="columnName">The name of the column to order by.</param>
        /// <param name="order">The direction to order the results.</param>
        /// <returns>The <see cref="IQueryResultFilter"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columnName"/> is not a valid column name.</exception>
        /// <exception cref="ArgumentException"><paramref name="order"/> is not a defined <see cref="OrderByType"/> enum value.</exception>
        public IQueryResultFilter OrderByColumn(string columnName, OrderByType order = OrderByType.Ascending)
        {
            return CreateQueryResultFilter(this).OrderByColumn(columnName, order);
        }

        /// <summary>
        /// Removes multiple columns from from the collection.
        /// </summary>
        /// <param name="columns">The names of the columns to remove.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        public IUpdateQuery Remove(IEnumerable<string> columns)
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
        public IUpdateQuery Remove(params string[] columns)
        {
            return _c.Remove(columns);
        }

        /// <summary>
        /// Removes a column from from the collection.
        /// </summary>
        /// <param name="column">The name of the column to remove.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="column"/> is not a valid column name.</exception>
        public IUpdateQuery Remove(string column)
        {
            return _c.Remove(column);
        }

        /// <summary>
        /// Filters the rows being returned or operated on.
        /// </summary>
        /// <param name="condition">The raw SQL condition to use to filter the rows.</param>
        /// <returns>The <see cref="IQueryResultFilter"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="condition"/> is null or empty.</exception>
        public IQueryResultFilter Where(string condition)
        {
            return CreateQueryResultFilter(this).Where(condition);
        }

        #endregion
    }
}