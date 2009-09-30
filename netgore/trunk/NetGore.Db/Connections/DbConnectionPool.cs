using System;
using System.Data.Common;
using System.Linq;
using NetGore;
using NetGore.Collections;

namespace NetGore.Db
{
    /// <summary>
    /// Base class for a pool of database connections.
    /// </summary>
    public abstract class DbConnectionPool : ObjectPool<PooledDbConnection>, IDisposable
    {
        readonly string _connectionString;
        bool _disposed;

        /// <summary>
        /// Gets the ConnectionString used for the connections made in this DbConnectionPool.
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
        }

        /// <summary>
        /// DbConnectionPool constructor
        /// </summary>
        /// <param name="connectionString">ConnectionString used for the connections made in this DbConnectionPool.</param>
        protected DbConnectionPool(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// When overridden in the derived class, creates the DbConnection to be used with this ObjectPool.
        /// </summary>
        /// <param name="connectionString">ConnectionString to create the DbConnection with.</param>
        /// <returns>DbConnection to be used with this ObjectPool.</returns>
        protected abstract DbConnection CreateConnection(string connectionString);

        /// <summary>
        /// When overridden in the derived class, creates and returns a DbParameter that is compatible with
        /// the type of database used by connections in this pool.
        /// </summary>
        /// <param name="parameterName">Reference name of the parameter.</param>
        /// <returns>DbParameter that is compatible with the connections in this DbConnectionPool.</returns>
        public abstract DbParameter CreateParameter(string parameterName);

        /// <summary>
        /// When overridden in the derived class, allows for additional handling by the ObjectPoolBase when a new
        /// object is created for the pool. This is only called when a new object is created, not activated. This is
        /// called only once for each unique pool item, after IPoolable.SetPoolData() and before IPoolable.Activate().
        /// </summary>
        /// <param name="item">New object that was created.</param>
        protected override void HandleNewPoolObject(PooledDbConnection item)
        {
            item.SetConnection(CreateConnection(ConnectionString));
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            foreach (var item in this)
            {
                item.Dispose();
            }
        }

        #endregion
    }
}