using System;
using System.Data.Common;
using System.Linq;
using NetGore.Collections;

namespace NetGore.Db
{
    /// <summary>
    /// Object that is used to wrap around a DbConnection that is used in an ObjectPool.
    /// </summary>
    public class PooledDbConnection : IPoolableDbConnection, IPoolable
    {
        readonly IObjectPool<PooledDbConnection> _objectPool;

        DbConnection _connection;

        internal PooledDbConnection(IObjectPool<PooledDbConnection> objectPool)
        {
            _objectPool = objectPool;
        }

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

        #region IPoolable Members

        /// <summary>
        /// Gets or sets the index of the object in the pool. This value should never be used by anything
        /// other than the pool that owns this object.
        /// </summary>
        int IPoolable.PoolIndex { get; set; }

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
            _objectPool.Free(this);
        }

        #endregion
    }
}