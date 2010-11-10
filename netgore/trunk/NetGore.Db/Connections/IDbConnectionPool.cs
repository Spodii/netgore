using System;
using System.Data.Common;
using System.Linq;
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

        /// <summary>
        /// Creates and returns a <see cref="DbParameter"/> that is compatible with the type of database
        /// used by connections in this pool.
        /// </summary>
        /// <param name="parameterName">Reference name of the parameter.</param>
        /// <returns>DbParameter that is compatible with the connections in this <see cref="IDbConnectionPool"/>.</returns>
        DbParameter CreateParameter(string parameterName);

        /// <summary>
        /// Gets or sets the <see cref="IQueryStatsTracker"/> to use to track the statistics for queries executed by
        /// this object. If null, statistics will not be tracked.
        /// </summary>
        IQueryStatsTracker QueryStats { get; set; }
    }
}