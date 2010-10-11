using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Base class for an implementation of the <see cref="IInsertODKUQuery"/>.
    /// </summary>
    public abstract class InsertODKUQueryBase : IInsertODKUQuery
    {
        readonly ColumnValueCollectionBuilder<IInsertODKUQuery> _c;
        readonly IInsertQuery _parent;
        readonly IQueryBuilderSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertODKUQueryBase"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="settings">The <see cref="IQueryBuilderSettings"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parent"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> is null.</exception>
        protected InsertODKUQueryBase(IInsertQuery parent, IQueryBuilderSettings settings)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");
            if (settings == null)
                throw new ArgumentNullException("settings");

            _parent = parent;
            _settings = settings;

            _c = new ColumnValueCollectionBuilder<IInsertODKUQuery>(this, settings);
        }

        /// <summary>
        /// Gets the <see cref="ColumnValueCollectionBuilder{T}"/> used by this query.
        /// </summary>
        protected ColumnValueCollectionBuilder<IInsertODKUQuery> ColumnValueCollectionBuilder
        {
            get { return _c; }
        }

        /// <summary>
        /// Gets the parent <see cref="IInsertQuery"/> that this query was created for.
        /// </summary>
        public IInsertQuery Parent
        {
            get { return _parent; }
        }

        /// <summary>
        /// Gets the <see cref="IQueryBuilderSettings"/> to use.
        /// </summary>
        public IQueryBuilderSettings Settings
        {
            get { return _settings; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the column name and value pairs from the
        /// <see cref="InsertODKUQueryBase.Parent"/>.
        /// </summary>
        /// <returns>The column name and value pairs from the <see cref="InsertODKUQueryBase.Parent"/>.</returns>
        protected abstract KeyValuePair<string, string>[] GetColumnCollectionValuesFromInsert();

        #region IInsertODKUQuery Members

        /// <summary>
        /// Adds a column name and value pair to the collection. If the <see cref="column"/> already exists,
        /// it will be changed to use the new <see cref="value"/>.
        /// </summary>
        /// <param name="column">The name of the column.</param>
        /// <param name="value">The value for the <paramref name="column"/>.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="column"/> is not a valid column name.</exception>
        public IInsertODKUQuery Add(string column, string value)
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
        public IInsertODKUQuery Add(KeyValuePair<string, string> columnAndValue)
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
        public IInsertODKUQuery Add(IEnumerable<KeyValuePair<string, string>> columnsAndValues)
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
        public IInsertODKUQuery Add(params KeyValuePair<string, string>[] columnsAndValues)
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
        public IInsertODKUQuery AddAutoParam(string column)
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
        public IInsertODKUQuery AddAutoParam(params string[] columns)
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
        public IInsertODKUQuery AddAutoParam(IEnumerable<string> columns)
        {
            return _c.AddAutoParam(columns);
        }

        /// <summary>
        /// Copies the column names and values from the insert statement into the update statement.
        /// </summary>
        /// <example>
        /// For the following INSERT query:
        ///     INSERT INTO myTable (a,b,c) VALUES (@a,@b,1)
        ///     
        /// You will get the ON DUPLICATE KEY UPDATE query:
        ///     ... ON DUPLICATE KEY UPDATE SET a=@a,b=@b,c=1
        /// </example>
        /// <returns>The <see cref="IInsertODKUQuery"/>.</returns>
        public IInsertODKUQuery AddFromInsert()
        {
            return AddFromInsert((string[])null);
        }

        /// <summary>
        /// Copies the column names and values from the insert statement into the update statement.
        /// </summary>
        /// <param name="except">The name of the column to exclude from the update statement.</param>
        /// <example>
        /// For the following INSERT query:
        ///     INSERT INTO myTable (a,b,c) VALUES (@a,@b,1)
        ///     
        /// You will get the ON DUPLICATE KEY UPDATE query:
        ///     ... ON DUPLICATE KEY UPDATE SET a=@a,b=@b,c=1
        /// </example>
        /// <returns>The <see cref="IInsertODKUQuery"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="except"/> is not a valid column name.</exception>
        public IInsertODKUQuery AddFromInsert(string except)
        {
            Add(GetColumnCollectionValuesFromInsert());

            if (except != null)
                Remove(except);

            return this;
        }

        /// <summary>
        /// Copies the column names and values from the insert statement into the update statement.
        /// </summary>
        /// <param name="except">The names of the columns to exclude from the update statement.</param>
        /// <example>
        /// For the following INSERT query:
        ///     INSERT INTO myTable (a,b,c) VALUES (@a,@b,1)
        ///     
        /// You will get the ON DUPLICATE KEY UPDATE query:
        ///     ... ON DUPLICATE KEY UPDATE SET a=@a,b=@b,c=1
        /// </example>
        /// <returns>The <see cref="IInsertODKUQuery"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="except"/> contains one or more invalid column names.</exception>
        public IInsertODKUQuery AddFromInsert(IEnumerable<string> except)
        {
            Add(GetColumnCollectionValuesFromInsert());

            if (except != null)
                Remove(except);

            return this;
        }

        /// <summary>
        /// Copies the column names and values from the insert statement into the update statement.
        /// </summary>
        /// <param name="except">The names of the columns to exclude from the update statement.</param>
        /// <example>
        /// For the following INSERT query:
        ///     INSERT INTO myTable (a,b,c) VALUES (@a,@b,1)
        ///     
        /// You will get the ON DUPLICATE KEY UPDATE query:
        ///     ... ON DUPLICATE KEY UPDATE SET a=@a,b=@b,c=1
        /// </example>
        /// <returns>The <see cref="IInsertODKUQuery"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="except"/> contains one or more invalid column names.</exception>
        public IInsertODKUQuery AddFromInsert(params string[] except)
        {
            Add(GetColumnCollectionValuesFromInsert());

            if (except != null)
                Remove(except);

            return this;
        }

        /// <summary>
        /// Adds a column name and value pair to the collection where the value will be turned into a query parameter.
        /// </summary>
        /// <param name="column">The name of the column.</param>
        /// <param name="value">The name to give the query parameter.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="column"/> is not a valid column name.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="value"/> is an invalid parameter name.</exception>
        public IInsertODKUQuery AddParam(string column, string value)
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
        public IInsertODKUQuery AddParam(KeyValuePair<string, string> columnAndValue)
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
        public IInsertODKUQuery AddParam(IEnumerable<KeyValuePair<string, string>> columnsAndValues)
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
        public IInsertODKUQuery AddParam(params KeyValuePair<string, string>[] columnsAndValues)
        {
            return _c.AddParam(columnsAndValues);
        }

        /// <summary>
        /// Removes multiple columns from from the collection.
        /// </summary>
        /// <param name="columns">The names of the columns to remove.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        public IInsertODKUQuery Remove(IEnumerable<string> columns)
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
        public IInsertODKUQuery Remove(params string[] columns)
        {
            return _c.Remove(columns);
        }

        /// <summary>
        /// Removes a column from from the collection.
        /// </summary>
        /// <param name="column">The name of the column to remove.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="column"/> is not a valid column name.</exception>
        public IInsertODKUQuery Remove(string column)
        {
            return _c.Remove(column);
        }

        #endregion
    }
}