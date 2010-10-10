﻿using System.Linq;

namespace NetGore.Db.QueryBuilder
{
    /// <summary>
    /// Interface for an SQL INSERT query.
    /// </summary>
    public interface IInsertQuery : IColumnValueCollectionBuilder<IInsertQuery>
    {
        /// <summary>
        /// Creates an ON DUPLICATE KEY UPDATE clause for the INSERT statement. This makes is so that when you try to insert
        /// into a table with a key that already exists, instead of the query failing, it will update the values of the
        /// existing row.
        /// </summary>
        /// <returns>The <see cref="IInsertODKUQuery"/>.</returns>
        IInsertODKUQuery OnDuplicateKeyUpdate();
    }
}