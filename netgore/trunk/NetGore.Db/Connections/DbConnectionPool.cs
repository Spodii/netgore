using System;
using System.Data.Common;
using System.Linq;
using NetGore.Collections;

namespace NetGore.Db
{
    /// <summary>
    /// Base class for a pool of database connections.
    /// </summary>
    public abstract class DbConnectionPool : IObjectPool<PooledDbConnection>, IDisposable
    {
        readonly string _connectionString;
        readonly ObjectPool<PooledDbConnection> _pool;

        bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionPool"/> class.
        /// </summary>
        /// <param name="connectionString">ConnectionString used for the connections made in this DbConnectionPool.</param>
        protected DbConnectionPool(string connectionString)
        {
            _connectionString = connectionString;
            _pool = new ObjectPool<PooledDbConnection>(CreateNewObj, x => x.Connection.Open(), x => x.Connection.Close(), true);
        }

        /// <summary>
        /// Gets the ConnectionString used for the connections made in this DbConnectionPool.
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
        }

        /// <summary>
        /// When overridden in the derived class, creates the DbConnection to be used with this ObjectPool.
        /// </summary>
        /// <param name="connectionString">ConnectionString to create the DbConnection with.</param>
        /// <returns>DbConnection to be used with this ObjectPool.</returns>
        protected abstract DbConnection CreateConnection(string connectionString);
        
        /// <summary>
        /// Creates a new <see cref="PooledDbConnection"/> for the <see cref="_pool"/>.
        /// </summary>
        /// <param name="objectPool">The owner object pool.</param>
        /// <returns>A new <see cref="PooledDbConnection"/> for the <see cref="_pool"/>.</returns>
        PooledDbConnection CreateNewObj(IObjectPool<PooledDbConnection> objectPool)
        {
            var ret = new PooledDbConnection(objectPool);
            var conn = CreateConnection(ConnectionString);
            ret.SetConnection(conn);
            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, creates and returns a DbParameter that is compatible with
        /// the type of database used by connections in this pool.
        /// </summary>
        /// <param name="parameterName">Reference name of the parameter.</param>
        /// <returns>DbParameter that is compatible with the connections in this DbConnectionPool.</returns>
        public abstract DbParameter CreateParameter(string parameterName);

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
        }

        #endregion

        #region IObjectPool<PooledDbConnection> Members

        /// <summary>
        /// Gets the number of live objects in the pool.
        /// </summary>
        public int LiveObjects
        {
            get { return _pool.LiveObjects; }
        }

        /// <summary>
        /// Gets a free object instance from the pool.
        /// </summary>
        /// <returns>A free object instance from the pool.</returns>
        public PooledDbConnection Acquire()
        {
            return _pool.Acquire();
        }

        /// <summary>
        /// Frees all live objects in the pool.
        /// </summary>
        public void Clear()
        {
            _pool.Clear();
        }

        /// <summary>
        /// Frees the object so the pool can reuse it. After freeing an object, it should not be used
        /// in any way, and be treated like it has been disposed. No exceptions will be thrown for trying to free
        /// an object that does not belong to this pool.
        /// </summary>
        /// <param name="poolObject">The object to be freed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="poolObject"/> is null.</exception>
        public void Free(PooledDbConnection poolObject)
        {
            _pool.Free(poolObject);
        }

        /// <summary>
        /// Frees the object so the pool can reuse it. After freeing an object, it should not be used
        /// in any way, and be treated like it has been disposed.
        /// </summary>
        /// <param name="poolObject">The object to be freed.</param>
        /// <param name="throwArgumentException">Whether or not an <see cref="ArgumentException"/> will be thrown for
        /// objects that do not belong to this pool.</param>
        /// <exception cref="ArgumentException"><paramref name="throwArgumentException"/> is tru and the 
        /// <paramref name="poolObject"/> does not belong to this pool.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="poolObject"/> is null.</exception>
        public void Free(PooledDbConnection poolObject, bool throwArgumentException)
        {
            _pool.Free(poolObject, throwArgumentException);
        }

        /// <summary>
        /// Frees all live objects in the pool that match the given <paramref name="condition"/>.
        /// </summary>
        /// <param name="condition">The condition used to determine if an object should be freed.</param>
        /// <returns>The number of objects that were freed.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="condition"/> is null.</exception>
        public int FreeAll(Func<PooledDbConnection, bool> condition)
        {
            return _pool.FreeAll(condition);
        }

        /// <summary>
        /// Performs the <paramref name="action"/> on all live objects in the object pool.
        /// </summary>
        /// <param name="action">The action to perform on all live objects in the object pool.</param>
        public void Perform(Action<PooledDbConnection> action)
        {
            _pool.Perform(action);
        }

        #endregion
    }
}