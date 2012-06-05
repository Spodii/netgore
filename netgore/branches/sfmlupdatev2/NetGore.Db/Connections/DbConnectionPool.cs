using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using NetGore.Collections;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db
{
    /// <summary>
    /// Base class for a pool of database connections.
    /// </summary>
    public abstract class DbConnectionPool : IDbConnectionPool
    {
        readonly string _connectionString;
        readonly ObjectPool<PooledDbConnection> _pool;
        readonly IDbQueryRunner _queryRunner;

        bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionPool"/> class.
        /// </summary>
        /// <param name="connectionString">ConnectionString used for the connections made in this DbConnectionPool.</param>
        protected DbConnectionPool(string connectionString)
        {
            _connectionString = connectionString;
            _pool = new ObjectPool<PooledDbConnection>(CreateNewObj, InitializePooledConnection, DeinitializePooledConnection,
                true);

            // Create our DbQueryRunner using a connection NOT in the pool (since the connection will remain opened)
            var conn = CreateConnection(ConnectionString);
            _queryRunner = new DbQueryRunner(this, conn);
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
            var ret = new PooledDbConnection(this);
            var conn = CreateConnection(ConnectionString);
            ret.SetConnection(conn);
            return ret;
        }

        /// <summary>
        /// Performs the deinitialization of a <see cref="PooledDbConnection"/> as it is released back into the object pool.
        /// </summary>
        /// <param name="conn">The <see cref="PooledDbConnection"/> to deinitialize.</param>
        protected virtual void DeinitializePooledConnection(PooledDbConnection conn)
        {
            conn.Connection.Close();
        }

        /// <summary>
        /// When overridden in the derived class, creates and returns a <see cref="DbParameter"/>
        /// that is compatible with the type of database used by connections in this pool.
        /// </summary>
        /// <param name="parameterName">Reference name of the parameter.</param>
        /// <returns>DbParameter that is compatible with the connections in this <see cref="IDbConnectionPool"/>.</returns>
        protected abstract DbParameter HandleCreateParameter(string parameterName);

        /// <summary>
        /// Performs the initialization of a <see cref="PooledDbConnection"/> as it is grabbed from the object pool.
        /// </summary>
        /// <param name="conn">The <see cref="PooledDbConnection"/> to initialize.</param>
        protected virtual void InitializePooledConnection(PooledDbConnection conn)
        {
            if (conn.Connection.State != ConnectionState.Open)
                conn.Connection.Open();
        }

        #region IDbConnectionPool Members

        /// <summary>
        /// Gets the integer value to use when inserting a row into a table on this connection, and the table defines an
        /// auto-increment column. This value will force the database to generate an auto-incremented value instead of explicitly
        /// setting the value for the field.
        /// </summary>
        public int AutoIncrementValue
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets the number of live objects in the pool.
        /// </summary>
        public int LiveObjects
        {
            get { return _pool.LiveObjects; }
        }

        /// <summary>
        /// Gets the <see cref="IQueryBuilder"/> to build queries for this connection.
        /// </summary>
        public abstract IQueryBuilder QueryBuilder { get; }

        /// <summary>
        /// Gets the <see cref="IDbQueryRunner"/> to use for this pool of database connections.
        /// </summary>
        IDbQueryRunner IDbConnectionPool.QueryRunner
        {
            get { return _queryRunner; }
        }

        /// <summary>
        /// Gets or sets the <see cref="IQueryStatsTracker"/> to use to track the statistics for queries executed by
        /// this object. If null, statistics will not be tracked.
        /// </summary>
        public IQueryStatsTracker QueryStats { get; set; }

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
        /// Creates and returns a <see cref="DbParameter"/> that is compatible with the type of database
        /// used by connections in this pool.
        /// </summary>
        /// <param name="parameterName">Reference name of the parameter.</param>
        /// <returns>DbParameter that is compatible with the connections in this <see cref="IDbConnectionPool"/>.</returns>
        public DbParameter CreateParameter(string parameterName)
        {
            return HandleCreateParameter(parameterName);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            if (!_queryRunner.IsDisposed)
                _queryRunner.Dispose();
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
        /// Gets the ID for the row that was inserted into the database. Only valid when the
        /// query contains an auto-increment column and the operation being performed is an insert.
        /// </summary>
        /// <param name="command">The <see cref="DbCommand"/> that was executed and the last inserted id needs to be acquired for.</param>
        /// <returns>
        /// The last inserted id for the executed <paramref name="command"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="command"/> is null.</exception>
        /// <exception cref="TypeException"><paramref name="command"/> is not of the excepted type.</exception>
        /// <exception cref="ArgumentException"><paramref name="command"/> is invalid in some other way.</exception>
        public abstract long GetLastInsertedId(DbCommand command);

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