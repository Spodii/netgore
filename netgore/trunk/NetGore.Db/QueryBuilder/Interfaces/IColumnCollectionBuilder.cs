using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Interface for an object that can be used to build a collection of database column names.
    /// </summary>
    /// <typeparam name="T">The type to return.</typeparam>
    public interface IColumnCollectionBuilder<out T>
    {
        /// <summary>
        /// Adds a column to the collection if it does not already exist.
        /// </summary>
        /// <param name="column">The name of the column to add.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="column"/> is not a valid column name.</exception>
        T Add(string column);

        /// <summary>
        /// Adds multiple columns to the collection.
        /// </summary>
        /// <param name="columns">The names of the columns to add.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        T Add(IEnumerable<string> columns);

        /// <summary>
        /// Adds multiple columns to the collection.
        /// </summary>
        /// <param name="columns">The names of the columns to add.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        T Add(params string[] columns);

        /// <summary>
        /// Adds a function call to the collection.
        /// </summary>
        /// <param name="sql">The function's SQL.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sql"/> is null or empty.</exception>
        T AddFunc(string sql);

        /// <summary>
        /// Adds a function call to the collection.
        /// </summary>
        /// <param name="sql">The function's SQL.</param>
        /// <param name="alias">The alias to give the function call.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="sql"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="alias"/> is not a valid column alias.</exception>
        T AddFunc(string sql, string alias);

        /// <summary>
        /// Removes a column from the collection.
        /// </summary>
        /// <param name="column">The name of the column to remove.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="column"/> is not a valid column name.</exception>
        T Remove(string column);

        /// <summary>
        /// Removes multiple columns from the collection.
        /// </summary>
        /// <param name="columns">The names of the columns to remove.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        T Remove(IEnumerable<string> columns);

        /// <summary>
        /// Removes multiple columns from the collection.
        /// </summary>
        /// <param name="columns">The names of the columns to remove.</param>
        /// <returns>The return object.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columns"/> contains one or more columns with
        /// invalid column name.</exception>
        T Remove(params string[] columns);
    }
}