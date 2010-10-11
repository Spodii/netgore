using System;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Interface for generating the result filter for a query.
    /// </summary>
    public interface IQueryResultFilter
    {
        /// <summary>
        /// Limits the number of rows being returned or operated on.
        /// </summary>
        /// <param name="amount">The row limit. Must be greater than 0.</param>
        /// <returns>The <see cref="IQueryResultFilter"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="amount"/> is not greater than 0.</exception>
        IQueryResultFilter Limit(int amount);

        /// <summary>
        /// Orders the rows in the query.
        /// </summary>
        /// <param name="value">The condition to order by.</param>
        /// <param name="order">The direction to order the results.</param>
        /// <returns>The <see cref="IQueryResultFilter"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null or empty.</exception>
        /// <exception cref="ArgumentException"><paramref name="order"/> is not a defined <see cref="OrderByType"/> enum value.</exception>
        IQueryResultFilter OrderBy(string value, OrderByType order = OrderByType.Ascending);

        /// <summary>
        /// Orders the rows in the query based on a single column.
        /// </summary>
        /// <param name="columnName">The name of the column to order by.</param>
        /// <param name="order">The direction to order the results.</param>
        /// <returns>The <see cref="IQueryResultFilter"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="columnName"/> is not a valid column name.</exception>
        /// <exception cref="ArgumentException"><paramref name="order"/> is not a defined <see cref="OrderByType"/> enum value.</exception>
        IQueryResultFilter OrderByColumn(string columnName, OrderByType order = OrderByType.Ascending);

        /// <summary>
        /// Filters the rows being returned or operated on.
        /// </summary>
        /// <param name="condition">The raw SQL condition to use to filter the rows.</param>
        /// <returns>The <see cref="IQueryResultFilter"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="condition"/> is null or empty.</exception>
        IQueryResultFilter Where(string condition);
    }
}