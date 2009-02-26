using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using NetGore.Collections;

namespace NetGore.Db
{
    /// <summary>
    /// Object that is used to wrap around a DbConnection that is used in an ObjectPool.
    /// </summary>
    /// <typeparam name="T">Type of DbConnection to be pooled.</typeparam>
    public class PooledDbConnection<T> : IPoolableDbConnection<T> where T : DbConnection
    {
        PoolData<PooledDbConnection<T>> _poolData;
        ObjectPool<PooledDbConnection<T>> _objectPool;
        T _connection;

        /// <summary>
        /// Sets the DbConnection to be used by this IPoolableDbConnection. This should only be called by the
        /// DbConnectionPool, and can only be called once.
        /// </summary>
        /// <param name="connection">Connection to be used by the IPoolableDbConnection.</param>
        void IPoolableDbConnection<T>.SetConnection(T connection)
        {
            if (_connection != null)
                throw new MethodAccessException("Connection already set for this PooledDbConnection.");
            if (connection == null)
                throw new ArgumentNullException("connection");

            _connection = connection;
        }

        /// <summary>
        /// Gets the IDbConnection for this IPoolableDbConnection.
        /// </summary>
        public IDbConnection Connection { get { return _connection; } }

        /// <summary>
        /// Gets the PoolData associated with this poolable item
        /// </summary>
        PoolData<PooledDbConnection<T>> IPoolable<PooledDbConnection<T>>.PoolData
        {
            get { return _poolData; }
        }

        /// <summary>
        /// Notifies the item that it has been activated by the pool and that it will start being used.
        /// All preperation work that could not be done in the constructor should be done here.
        /// </summary>
        void IPoolable<PooledDbConnection<T>>.Activate()
        {
            _connection.Open();
        }

        /// <summary>
        /// Notifies the item that it has been deactivated by the pool. The item may or may not ever be
        /// activated again, so clean up where needed.
        /// </summary>
        void IPoolable<PooledDbConnection<T>>.Deactivate()
        {
            _connection.Close();
        }

        /// <summary>
        /// Sets the PoolData for this item. This is only called once in the object's lifetime;
        /// right after the object is constructed.
        /// </summary>
        /// <param name="objectPool">Pool that created this object</param>
        /// <param name="poolData">PoolData assigned to this object</param>
        void IPoolable<PooledDbConnection<T>>.SetPoolData(ObjectPool<PooledDbConnection<T>> objectPool, PoolData<PooledDbConnection<T>> poolData)
        {
            _objectPool = objectPool;
            _poolData = poolData;
        }

        /// <summary>
        /// Disposes of the PooledDbConnection, closing the connection and sending it back to the ObjectPool.
        /// </summary>
        public void Dispose()
        {
            _objectPool.Destroy(this);
        }
    }
}
