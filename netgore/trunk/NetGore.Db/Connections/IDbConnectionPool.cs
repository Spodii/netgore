using System;
using NetGore.Collections;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db
{
    /// <summary>
    /// Interface for a pool of database collections.
    /// </summary>
    public interface IDbConnectionPool : IObjectPool<PooledDbConnection>, IDisposable
    {
        /// <summary>
        /// Gets the <see cref="IQueryBuilder"/> to build queries for this connection.
        /// </summary>
        IQueryBuilder QueryBuilder { get; }
    }
}