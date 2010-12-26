using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore.Collections;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db
{
    /// <summary>
    /// Object that is used to wrap around a <see cref="DbConnection"/> that is used in an <see cref="IDbConnectionPool"/>.
    /// </summary>
    public class PooledDbConnection : IPoolableDbConnection, IPoolable
    {
        readonly IDbConnectionPool _pool;

        DbConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="PooledDbConnection"/> class.
        /// </summary>
        /// <param name="pool">The <see cref="IDbConnectionPool"/> that this instance belongs to.</param>
        internal PooledDbConnection(IDbConnectionPool pool)
        {
            _pool = pool;
        }

        /// <summary>
        /// Sets the <see cref="DbConnection"/> to be used by this <see cref="IPoolableDbConnection"/>.
        /// This should only be called by the <see cref="DbConnectionPool"/>, and can only be called once.
        /// </summary>
        /// <param name="connection">Connection to be used by the <see cref="IPoolableDbConnection"/>.</param>
        /// <exception cref="MethodAccessException">Connection already set for this <see cref="PooledDbConnection"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="connection" /> is <c>null</c>.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "PooledDbConnection")]
        internal void SetConnection(DbConnection connection)
        {
            if (_connection != null)
                throw new MethodAccessException("Connection already set for this PooledDbConnection.");
            if (connection == null)
                throw new ArgumentNullException("connection");

            _connection = connection;
        }

        #region IPoolable Members

        /// <summary>
        /// Gets or sets the index of the object in the pool. This value should never be used by anything
        /// other than the pool that owns this object.
        /// </summary>
        int IPoolable.PoolIndex { get; set; }

        #endregion

        #region IPoolableDbConnection Members

        /// <summary>
        /// Gets the <see cref="DbConnection"/> for this <see cref="IPoolableDbConnection"/>.
        /// </summary>
        public DbConnection Connection
        {
            get { return _connection; }
        }

        /// <summary>
        /// Gets the <see cref="IQueryBuilder"/> to build queries for this connection.
        /// </summary>
        public IQueryBuilder QueryBuilder
        {
            get { return _pool.QueryBuilder; }
        }

        /// <summary>
        /// Gets the <see cref="IDbQueryRunner"/> to use for this pooled database connection.
        /// </summary>
        public IDbQueryRunner QueryRunner
        {
            get { return _pool.QueryRunner; }
        }

        /// <summary>
        /// Gets the <see cref="IQueryStatsTracker"/> to use to track the statistics for queries executed by
        /// this object. If null, statistics will not be tracked.
        /// </summary>
        public IQueryStatsTracker QueryStats
        {
            get { return _pool.QueryStats; }
        }

        /// <summary>
        /// Disposes of the <see cref="IPoolableDbConnection"/>, closing the connection and sending it back to the
        /// <see cref="IDbConnectionPool"/>.
        /// </summary>
        public void Dispose()
        {
            _pool.Free(this);
        }

        #endregion
    }
}