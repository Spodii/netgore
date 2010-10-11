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