using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Interface for an object that can be used to build a collection of database column name and value pairs.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    public interface IColumnValueCollectionBuilder<out T>
    {
        /// <summary>
        /// Adds a column name and value pair to the collection. If the <see cref="column"/> already exists,
        /// it will be changed to use the new <see cref="value"/>.
        /// </summary>
        /// <param name="column">The name of the column.</param>
        /// <param name="value">The value for the <paramref name="column"/>.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="column"/> is not a valid column name.</exception>
        T Add(string column, string value);

        /// <summary>
        /// Adds a column name and value pair to the collection, where the key is the column name and the value is the
        /// column value. If the column already exists, it will be changed to use the new value.
        /// </summary>
        /// <param name="columnAndValue">The column name and value to add.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columnAndValue"/> contains an invalid column name.</exception>
        T Add(KeyValuePair<string, string> columnAndValue);

        /// <summary>
        /// Adds multiple column name and value pairs to the collection, where the key is the column name and the value is the
        /// column value. If the column already exists, it will be changed to use the new value.
        /// </summary>
        /// <param name="columnsAndValues">The column name and value pairs to add.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException">One or more value in <paramref name="columnsAndValues"/> contains an
        /// invalid column name.</exception>
        T Add(IEnumerable<KeyValuePair<string, string>> columnsAndValues);

        /// <summary>
        /// Adds multiple column name and value pairs to the collection, where the key is the column name and the value is the
        /// column value. If the column already exists, it will be changed to use the new value.
        /// </summary>
        /// <param name="columnsAndValues">The column name and value pairs to add.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException">One or more value in <paramref name="columnsAndValues"/> contains an
        /// invalid column name.</exception>
        T Add(params KeyValuePair<string, string>[] columnsAndValues);

        /// <summary>
        /// Adds a column where the value is automatically created as a query parameter. The name of the query parameter
        /// will be the same as the name of the column.
        /// </summary>
        /// <param name="column">The name of the column.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="column"/> is not a valid column name.</exception>
        T AddAutoParam(string column);

        /// <summary>
        /// Adds multiple columns where the value is automatically created as a query parameter. The name of the query parameter
        /// will be the same as the name of the column.
        /// </summary>
        /// <param name="columns">The names of the columns.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        T AddAutoParam(params string[] columns);

        /// <summary>
        /// Adds multiple columns where the value is automatically created as a query parameter. The name of the query parameter
        /// will be the same as the name of the column.
        /// </summary>
        /// <param name="columns">The names of the columns.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        T AddAutoParam(IEnumerable<string> columns);

        /// <summary>
        /// Adds a column name and value pair to the collection where the value will be turned into a query parameter.
        /// </summary>
        /// <param name="column">The name of the column.</param>
        /// <param name="value">The name to give the query parameter.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="column"/> is not a valid column name.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="value"/> is an invalid parameter name.</exception>
        T AddParam(string column, string value);

        /// <summary>
        /// Adds a column name and value pair to the collection where the value will be turned into a query parameter.
        /// </summary>
        /// <param name="columnAndValue">The column name and query parameter name pair.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columnAndValue"/> contains an invalid column name.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="columnAndValue"/> is an invalid parameter name.</exception>
        T AddParam(KeyValuePair<string, string> columnAndValue);

        /// <summary>
        /// Adds multiple column name and value pairs to the collection where the value will be turned into a query parameter.
        /// </summary>
        /// <param name="columnsAndValues">The column name and query parameter name pairs.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException">One or more values in <paramref name="columnsAndValues"/> contains an
        /// invalid column name.</exception>
        /// <exception cref="InvalidQueryException">One or more values in <paramref name="columnsAndValues"/> contains an
        /// invalid parameter name.</exception>
        T AddParam(IEnumerable<KeyValuePair<string, string>> columnsAndValues);

        /// <summary>
        /// Adds multiple column name and value pairs to the collection where the value will be turned into a query parameter.
        /// </summary>
        /// <param name="columnsAndValues">The column name and query parameter name pairs.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException">One or more values in <paramref name="columnsAndValues"/> contains an
        /// invalid column name.</exception>
        /// <exception cref="InvalidQueryException">One or more values in <paramref name="columnsAndValues"/> contains an
        /// invalid parameter name.</exception>
        T AddParam(params KeyValuePair<string, string>[] columnsAndValues);

        /// <summary>
        /// Removes multiple columns from from the collection.
        /// </summary>
        /// <param name="columns">The names of the columns to remove.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        T Remove(IEnumerable<string> columns);

        /// <summary>
        /// Removes multiple columns from from the collection.
        /// </summary>
        /// <param name="columns">The names of the columns to remove.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        T Remove(params string[] columns);

        /// <summary>
        /// Removes a column from from the collection.
        /// </summary>
        /// <param name="column">The name of the column to remove.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="column"/> is not a valid column name.</exception>
        T Remove(string column);
    }
}