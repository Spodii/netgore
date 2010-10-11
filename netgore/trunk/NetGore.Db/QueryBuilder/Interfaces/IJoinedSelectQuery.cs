using System;
using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Interface for a SQL SELECT query that has performed or is able to perform a JOIN operation.
    /// </summary>
    public interface IJoinedSelectQuery : IQueryResultFilter
    {
        /// <summary>
        /// Performs an INNER JOIN with another table.
        /// </summary>
        /// <param name="table">The name of the table to join.</param>
        /// <param name="alias">The alias of the joined table.</param>
        /// <param name="joinCondition">The raw SQL join condition.</param>
        /// <returns>The <see cref="IJoinedSelectQuery"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="table"/> is an invalid table name.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="alias"/> is an invalid table alias.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="joinCondition"/> is null or empty.</exception>
        IJoinedSelectQuery InnerJoin(string table, string alias, string joinCondition);

        /// <summary>
        /// Performs an INNER JOIN with another table.
        /// </summary>
        /// <param name="table">The name of the table to join.</param>
        /// <param name="alias">The alias of the joined table.</param>
        /// <param name="thisJoinColumn">The name of the column on the <paramref name="table"/> that is being joined on.</param>
        /// <param name="otherTable">The name or alias of the other table to join.</param>
        /// <param name="otherTableJoinColumn">The name of the column on the <paramref name="otherTable"/> that is to be
        /// joined.</param>
        /// <returns>The <see cref="IJoinedSelectQuery"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="table"/> is an invalid table name.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="alias"/> is an invalid table alias.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="thisJoinColumn"/> is an invalid column name.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="otherTable"/> is an invalid table name or alias.</exception>
        /// <exception cref="InvalidQueryException"><paramref name="otherTableJoinColumn"/> is an invalid column name.</exception>
        IJoinedSelectQuery InnerJoinOnColumn(string table, string alias, string thisJoinColumn, string otherTable,
                                             string otherTableJoinColumn);
    }
}