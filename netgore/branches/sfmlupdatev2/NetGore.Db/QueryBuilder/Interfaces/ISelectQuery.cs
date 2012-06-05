using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Interface for an SQL SELECT query.
    /// </summary>
    public interface ISelectQuery : IColumnCollectionBuilder<ISelectQuery>, IJoinedSelectQuery
    {
        /// <summary>
        /// Forces all columns to be selected.
        /// </summary>
        /// <returns>The <see cref="ISelectQuery"/>.</returns>
        ISelectQuery AllColumns();

        /// <summary>
        /// Forces all columns to be selected.
        /// </summary>
        /// <param name="table">The table or table alias from which all columns will be selected.</param>
        /// <returns>The <see cref="ISelectQuery"/>.</returns>
        /// <exception cref="InvalidQueryException"><paramref name="table"/> is an invalid table name and table alias.</exception>
        ISelectQuery AllColumns(string table);

        /// <summary>
        /// Makes it so that only rows with distinct values are selected.
        /// </summary>
        /// <returns></returns>
        /// <returns>The <see cref="ISelectQuery"/>.</returns>
        ISelectQuery Distinct();
    }
}