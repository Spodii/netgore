using System;

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
        /// Makes it so that only rows with distinct values are selected.
        /// </summary>
        /// <returns></returns>
        /// <returns>The <see cref="ISelectQuery"/>.</returns>
        ISelectQuery Distinct();
    }
}