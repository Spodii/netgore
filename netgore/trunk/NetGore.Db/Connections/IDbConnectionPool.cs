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
        /// Gets the integer value to use when inserting a row into a table on this connection, and the table defines an
        /// auto-increment column. This value will force the database to generate an auto-incremented value instead of explicitly
        /// setting the value for the field.
        /// </summary>
        int AutoIncrementValue { get; }

        /// <summary>
        /// Gets the <see cref="IQueryBuilder"/> to build queries for this connection.
        /// </summary>
        IQueryBuilder QueryBuilder { get; }

        /// <summary>
        /// Gets the <see cref="IDbQueryRunner"/> to use for this pool of database connections.
        /// </summary>
        IDbQueryRunner QueryRunner { get; }

        /// <summary>
        /// Gets or sets the <see cref="IQueryStatsTracker"/> to use to track the statistics for queries executed by
        /// this object. If null, statistics will not be tracked.
        /// </summary>
        IQueryStatsTracker QueryStats { get; set; }

        /// <summary>
        /// Creates and returns a <see cref="DbParameter"/> that is compatible with the type of database
        /// used by connections in this pool.
        /// </summary>
        /// <param name="parameterName">Reference name of the parameter.</param>
        /// <returns>DbParameter that is compatible with the connections in this <see cref="IDbConnectionPool"/>.</returns>
        DbParameter CreateParameter(string parameterName);

        /// <summary>
        /// Gets the ID for the row that was inserted into the database. Only valid when the
        /// query contains an auto-increment column and the operation being performed is an insert.
        /// </summary>
        /// <param name="command">The <see cref="DbCommand"/> that was executed and the last inserted id needs to be acquired for.</param>
        /// <returns>The last inserted id for the executed <paramref name="command"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="command"/> is null.</exception>
        /// <exception cref="TypeException"><paramref name="command"/> is not of the excepted type.</exception>
        /// <exception cref="ArgumentException"><paramref name="command"/> is invalid in some other way.</exception>
        long GetLastInsertedId(DbCommand command);
    }
}