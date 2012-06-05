using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Builds a collection of database column name and value pairs.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    public class ColumnValueCollectionBuilder<T> : IColumnValueCollectionBuilder<T>
    {
        readonly List<KeyValuePair<string, string>> _cvs;
        readonly T _owner;
        readonly IQueryBuilderSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnValueCollectionBuilder{T}"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="settings">The <see cref="IQueryBuilderSettings"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="owner"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="settings"/> is null.</exception>
        public ColumnValueCollectionBuilder(T owner, IQueryBuilderSettings settings)
        {
            // ReSharper disable CompareNonConstrainedGenericWithNull
            if (owner == null)
                throw new ArgumentNullException("owner");
            // ReSharper restore CompareNonConstrainedGenericWithNull

            if (settings == null)
                throw new ArgumentNullException("settings");

            _settings = settings;
            _owner = owner;
            _cvs = new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// Gets the <see cref="IQueryBuilderSettings"/> being used.
        /// </summary>
        public IQueryBuilderSettings Settings
        {
            get { return _settings; }
        }

        /// <summary>
        /// Gets the column name and value pairs in this collection.
        /// </summary>
        /// <returns>The column name and value pairs.</returns>
        public KeyValuePair<string, string>[] GetValues()
        {
            return _cvs.ToArray();
        }

        #region IColumnValueCollectionBuilder<T> Members

        /// <summary>
        /// Adds a column name and value pair to the collection. If the <see cref="column"/> already exists,
        /// it will be changed to use the new <see cref="value"/>.
        /// </summary>
        /// <param name="column">The name of the column.</param>
        /// <param name="value">The value for the <paramref name="column"/>.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="column"/> is not a valid column name.</exception>
        public T Add(string column, string value)
        {
            Settings.IsValidColumnName(column, true);

            _cvs.RemoveAll(x => Settings.ColumnNameComparer.Equals(x.Key, column));
            _cvs.Add(new KeyValuePair<string, string>(column, value));

            return _owner;
        }

        /// <summary>
        /// Adds a column name and value pair to the collection, where the key is the column name and the value is the
        /// column value. If the column already exists, it will be changed to use the new value.
        /// </summary>
        /// <param name="columnAndValue">The column name and value to add.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columnAndValue"/> contains an invalid column name.</exception>
        public T Add(KeyValuePair<string, string> columnAndValue)
        {
            return Add(columnAndValue.Key, columnAndValue.Value);
        }

        /// <summary>
        /// Adds multiple column name and value pairs to the collection, where the key is the column name and the value is the
        /// column value. If the column already exists, it will be changed to use the new value.
        /// </summary>
        /// <param name="columnsAndValues">The column name and value pairs to add.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException">One or more value in <paramref name="columnsAndValues"/> contains an
        /// invalid column name.</exception>
        public T Add(IEnumerable<KeyValuePair<string, string>> columnsAndValues)
        {
            if (columnsAndValues != null)
            {
                foreach (var c in columnsAndValues)
                {
                    Add(c);
                }
            }

            return _owner;
        }

        /// <summary>
        /// Adds multiple column name and value pairs to the collection, where the key is the column name and the value is the
        /// column value. If the column already exists, it will be changed to use the new value.
        /// </summary>
        /// <param name="columnsAndValues">The column name and value pairs to add.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException">One or more value in <paramref name="columnsAndValues"/> contains an
        /// invalid column name.</exception>
        public T Add(params KeyValuePair<string, string>[] columnsAndValues)
        {
            return Add((IEnumerable<KeyValuePair<string, string>>)columnsAndValues);
        }

        /// <summary>
        /// Adds a column where the value is automatically created as a query parameter. The name of the query parameter
        /// will be the same as the name of the column.
        /// </summary>
        /// <param name="column">The name of the column.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="column"/> is not a valid column name.</exception>
        public T AddAutoParam(string column)
        {
            return AddParam(column, Settings.Parameterize(column));
        }

        /// <summary>
        /// Adds multiple columns where the value is automatically created as a query parameter. The name of the query parameter
        /// will be the same as the name of the column.
        /// </summary>
        /// <param name="columns">The names of the columns.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        public T AddAutoParam(params string[] columns)
        {
            return AddAutoParam((IEnumerable<string>)columns);
        }

        /// <summary>
        /// Adds multiple columns where the value is automatically created as a query parameter. The name of the query parameter
        /// will be the same as the name of the column.
        /// </summary>
        /// <param name="columns">The names of the columns.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        public T AddAutoParam(IEnumerable<string> columns)
        {
            if (columns != null)
            {
                foreach (var c in columns)
                {
                    AddAutoParam(c);
                }
            }

            return _owner;
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
        public T AddParam(params KeyValuePair<string, string>[] columnsAndValues)
        {
            return AddParam((IEnumerable<KeyValuePair<string, string>>)columnsAndValues);
        }

        /// <summary>
        /// Adds a column name and value pair to the collection where the value will be turned into a query parameter.
        /// </summary>
        /// <param name="column">The name of the column.</param>
        /// <param name="value">The name to give the query parameter.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="column"/> is not a valid column name.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="value"/> is an invalid parameter name.</exception>
        public T AddParam(string column, string value)
        {
            Settings.IsValidColumnName(column, true);
            Settings.IsValidParameterName(value, true);

            _cvs.RemoveAll(x => Settings.ColumnNameComparer.Equals(x.Key, column));
            _cvs.Add(new KeyValuePair<string, string>(column, Settings.Parameterize(value)));

            return _owner;
        }

        /// <summary>
        /// Adds a column name and value pair to the collection where the value will be turned into a query parameter.
        /// </summary>
        /// <param name="columnAndValue">The column name and query parameter name pair.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columnAndValue"/> contains an invalid column name.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="columnAndValue"/> is an invalid parameter name.</exception>
        public T AddParam(KeyValuePair<string, string> columnAndValue)
        {
            return AddParam(columnAndValue.Key, columnAndValue.Value);
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
        public T AddParam(IEnumerable<KeyValuePair<string, string>> columnsAndValues)
        {
            if (columnsAndValues != null)
            {
                foreach (var c in columnsAndValues)
                {
                    Add(c);
                }
            }

            return _owner;
        }

        /// <summary>
        /// Removes multiple columns from from the collection.
        /// </summary>
        /// <param name="columns">The names of the columns to remove.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        public T Remove(IEnumerable<string> columns)
        {
            if (columns != null)
            {
                foreach (var c in columns)
                {
                    Remove(c);
                }
            }

            return _owner;
        }

        /// <summary>
        /// Removes multiple columns from from the collection.
        /// </summary>
        /// <param name="columns">The names of the columns to remove.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        public T Remove(params string[] columns)
        {
            return Remove((IEnumerable<string>)columns);
        }

        /// <summary>
        /// Removes a column from from the collection.
        /// </summary>
        /// <param name="column">The name of the column to remove.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="column"/> is not a valid column name.</exception>
        public T Remove(string column)
        {
            Settings.IsValidColumnName(column, true);

            _cvs.RemoveAll(x => Settings.ColumnNameComparer.Equals(x.Key, column));

            return _owner;
        }

        #endregion
    }
}