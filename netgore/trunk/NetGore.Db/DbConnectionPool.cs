using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using NetGore.Collections;

namespace NetGore.Db
{
    /// <summary>
    /// Base for a pool of database connections.
    /// </summary>
    /// <typeparam name="T">Type of DbConnection to be managed by this DbConnectionPool.</typeparam>
    public abstract class DbConnectionPool<T> : ObjectPool<PooledDbConnection<T>> where T : DbConnection, new()
    {
        readonly string _connectionString;

        /// <summary>
        /// Gets the ConnectionString used for the connections made in this DbConnectionPool.
        /// </summary>
        public string ConnectionString { get { return _connectionString; } }

        /// <summary>
        /// DbConnectionPool constructor
        /// </summary>
        /// <param name="connectionString">ConnectionString used for the connections made in this DbConnectionPool.</param>
        protected DbConnectionPool(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling by the ObjectPoolBase when a new
        /// object is created for the pool. This is only called when a new object is created, not activated. This is
        /// called only once for each unique pool item, after IPoolable.SetPoolData() and before IPoolable.Activate().
        /// </summary>
        /// <param name="item">New object that was created.</param>
        protected override void HandleNewPoolObject(PooledDbConnection<T> item)
        {
            IPoolableDbConnection<T> poolableDbConnection = item;
            poolableDbConnection.SetConnection(CreateConnection(ConnectionString));
        }

        /// <summary>
        /// When overridden in the derived class, creates the DbConnection to be used with this ObjectPool.
        /// </summary>
        /// <param name="connectionString">ConnectionString to create the DbConnection with.</param>
        /// <returns>DbConnection to be used with this ObjectPool.</returns>
        protected abstract T CreateConnection(string connectionString);
    }
}
