using System;
using System.Data.Common;
using NetGore.Collections;

namespace NetGore.Db
{
    /// <summary>
    /// Object that is used to wrap around a DbConnection that is used in an ObjectPool.
    /// </summary>
    public class PooledDbConnection : IPoolableDbConnection, IPoolable<PooledDbConnection>
    {
        DbConnection _connection;
        ObjectPool<PooledDbConnection> _objectPool;
        PoolData<PooledDbConnection> _poolData;

        /// <summary>
        /// Sets the DbConnection to be used by this IPoolableDbConnection. This should only be called by the
        /// DbConnectionPool, and can only be called once.
        /// </summary>
        /// <param name="connection">Connection to be used by the IPoolableDbConnection.</param>
        internal void SetConnection(DbConnection connection)
        {
            if (_connection != null)
                throw new MethodAccessException("Connection already set for this PooledDbConnection.");
            if (connection == null)
                throw new ArgumentNullException("connection");

            _connection = connection;
        }

        #region IPoolable<PooledDbConnection> Members

        /// <summary>
        /// Gets the PoolData associated with this poolable item
        /// </summary>
        PoolData<PooledDbConnection> IPoolable<PooledDbConnection>.PoolData
        {
            get { return _poolData; }
        }

        /// <summary>
        /// Notifies the item that it has been activated by the pool and that it will start being used.
        /// All preperation work that could not be done in the constructor should be done here.
        /// </summary>
        void IPoolable<PooledDbConnection>.Activate()
        {
            _connection.Open();
        }

        /// <summary>
        /// Notifies the item that it has been deactivated by the pool. The item may or may not ever be
        /// activated again, so clean up where needed.
        /// </summary>
        void IPoolable<PooledDbConnection>.Deactivate()
        {
            _connection.Close();
        }

        /// <summary>
        /// Sets the PoolData for this item. This is only called once in the object's lifetime;
        /// right after the object is constructed.
        /// </summary>
        /// <param name="objectPool">Pool that created this object</param>
        /// <param name="poolData">PoolData assigned to this object</param>
        void IPoolable<PooledDbConnection>.SetPoolData(ObjectPool<PooledDbConnection> objectPool,
                                                       PoolData<PooledDbConnection> poolData)
        {
            _objectPool = objectPool;
            _poolData = poolData;
        }

        #endregion

        #region IPoolableDbConnection Members

        /// <summary>
        /// Gets the IDbConnection for this IPoolableDbConnection.
        /// </summary>
        public DbConnection Connection
        {
            get { return _connection; }
        }

        /// <summary>
        /// Disposes of the PooledDbConnection, closing the connection and sending it back to the ObjectPool.
        /// </summary>
        public void Dispose()
        {
            _objectPool.Destroy(this);
        }

        #endregion
    }
}