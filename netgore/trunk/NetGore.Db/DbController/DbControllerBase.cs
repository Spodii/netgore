using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db
{
    /// <summary>
    /// Provides an interface between all objects and all the database handling methods.
    /// </summary>
    public abstract class DbControllerBase : IDbController
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static readonly List<DbControllerBase> _instances = new List<DbControllerBase>();

        readonly DbConnectionPool _connectionPool;
        readonly string _database;
        readonly Dictionary<Type, object> _queryObjects = new Dictionary<Type, object>();

        bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbControllerBase"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionString"/> is null or empty.</exception>
        /// <exception cref="DatabaseConnectionException">The connection failed to be created.</exception>
        protected DbControllerBase(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");

            // Create the connection pool
            try
            {
                _connectionPool = CreateConnectionPool(connectionString);
            }
            catch (DbException ex)
            {
                throw CreateConnectionException(connectionString, ex);
            }

            if (log.IsInfoEnabled)
                log.InfoFormat("Database connection pool created.");

            // Test the connection
            _database = TestConnectionPoolAndGetDatabase(connectionString, _connectionPool);

            // Create the query objects
            PopulateQueryObjects();

            // Add this connection to the list of instances
            lock (_instances)
                _instances.Add(this);
        }

        /// <summary>
        /// Creates the <see cref="DatabaseConnectionException"/> for when the connection fails.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <returns>The <see cref="DatabaseConnectionException"/> for when the connection fails.</returns>
        /// <exception cref="DatabaseConnectionException">The database connection failed to be created.</exception>
        static DatabaseConnectionException CreateConnectionException(string connectionString, Exception innerException)
        {
            const string errmsg = "Failed to create connection to database: {0}{1}{0}{0}Connection string:{0}{2}";
            var msg = string.Format(errmsg, Environment.NewLine, innerException.Message, connectionString);
            if (log.IsFatalEnabled)
                log.Fatal(msg, innerException);
            throw new DatabaseConnectionException(msg, innerException);
        }

        /// <summary>
        /// When overridden in the derived class, creates a <see cref="DbConnectionPool"/> for this
        /// <see cref="DbControllerBase"/>.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>A DbConnectionPool for this <see cref="DbControllerBase"/>.</returns>
        protected abstract DbConnectionPool CreateConnectionPool(string connectionString);

        /// <summary>
        /// Gets an implementation of the <see cref="FindForeignKeysQuery"/> that works for this
        /// <see cref="DbControllerBase"/>.
        /// </summary>
        /// <param name="dbConnectionPool">The <see cref="DbConnectionPool"/> to use when creating the query.</param>
        /// <returns>The <see cref="FindForeignKeysQuery"/> to execute the query.</returns>
        protected abstract FindForeignKeysQuery GetFindForeignKeysQuery(DbConnectionPool dbConnectionPool);

        /// <summary>
        /// Gets an instance of the <see cref="DbControllerBase"/>. A <see cref="DbControllerBase"/> must have already
        /// been constructed for this to work.
        /// </summary>
        /// <returns>An available instance of the <see cref="DbControllerBase"/>.</returns>
        /// <exception cref="MemberAccessException">No <see cref="DbControllerBase"/>s have been created yet, or
        /// all created <see cref="DbControllerBase"/>s have already been disposed.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "DbController")]
        public static DbControllerBase GetInstance()
        {
            lock (_instances)
            {
                if (_instances.Count == 0)
                    throw new MemberAccessException("Constructor on the DbController has not yet been called!");

                return _instances[_instances.Count - 1];
            }
        }

        /// <summary>
        /// Gets the SQL query string used for when testing if the database connection is valid. This string should
        /// be very simple and fool-proof, and work no matter what contents are in the database since this is just
        /// to test if the connection is working.
        /// </summary>
        /// <returns>The SQL query string used for when testing if the database connection is valid.</returns>
        protected abstract string GetTestQueryCommand();

        /// <summary>
        /// Populates the <see cref="_queryObjects"/> with the query objects.
        /// </summary>
        /// <exception cref="InstantiateTypeException">A type failed to be instantiated.</exception>
        void PopulateQueryObjects()
        {
            if (log.IsInfoEnabled)
                log.InfoFormat("Creating query objects for database connection.");

            // Find the classes marked with our attribute
            var requiredConstructorParams = new Type[] { typeof(DbConnectionPool) };
            var types = TypeHelper.FindTypesWithAttribute(typeof(DbControllerQueryAttribute), requiredConstructorParams);

            // Create an instance of each of the types
            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type, _connectionPool);
                if (instance == null)
                {
                    const string errmsg = "Failed to create instance of Type `{0}`.";
                    if (log.IsFatalEnabled)
                        log.Fatal(string.Format(errmsg, type));
                    Debug.Fail(string.Format(errmsg, type));
                    throw new InstantiateTypeException(type);
                }

                // Add the instance to the collection
                _queryObjects.Add(type, instance);

                if (log.IsDebugEnabled)
                    log.DebugFormat("Created instance of query class Type `{0}`.", type.Name);
            }

            if (log.IsInfoEnabled)
                log.Info("DbController successfully initialized all queries.");
        }

        /// <summary>
        /// Tests the database connection to make sure it works.
        /// </summary>
        /// <param name="connectionString">The connection string used.</param>
        /// <param name="pool">The pool of connections.</param>
        /// <returns>The name of the database.</returns>
        /// <exception cref="DatabaseConnectionException">The database connection failed to be created.</exception>
        string TestConnectionPoolAndGetDatabase(string connectionString, IObjectPool<PooledDbConnection> pool)
        {
            string database;

            try
            {
                using (var poolableConn = pool.Acquire())
                {
                    database = poolableConn.Connection.Database;

                    using (var cmd = poolableConn.Connection.CreateCommand())
                    {
                        cmd.CommandText = GetTestQueryCommand();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (DbException ex)
            {
                throw CreateConnectionException(connectionString, ex);
            }

            return database;
        }

        #region IDbController Members

        /// <summary>
        /// Gets the <see cref="DbConnectionPool"/> used by this <see cref="IDbController"/>.
        /// </summary>
        public DbConnectionPool ConnectionPool
        {
            get { return _connectionPool; }
        }

        /// <summary>
        /// Gets the name of the database that this <see cref="IDbController"/> instance is connected to.
        /// </summary>
        public string Database
        {
            get { return _database; }
        }

        /// <summary>
        /// Gets the <see cref="IQueryBuilder"/> to build queries for this connection.
        /// </summary>
        public abstract IQueryBuilder QueryBuilder { get; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            lock (_instances)
                _instances.Remove(this);

            // Dispose of all the queries, where possible
            foreach (var disposableQuery in _queryObjects.OfType<IDisposable>())
            {
                disposableQuery.Dispose();
            }

            // Dispose of the DbConnectionPool
            _connectionPool.Dispose();
        }

        /// <summary>
        /// Gets the schema, table, and column tuples for columns containing a reference to the specified primary key.
        /// </summary>
        /// <param name="database">Database or schema object that the <paramref name="table"/> belongs to.</param>
        /// <param name="table">The table of the primary key.</param>
        /// <param name="column">The column of the primary key.</param>
        /// <returns>An IEnumerable of the name of the tables and columns that reference a the given primary key.</returns>
        public IEnumerable<SchemaTableColumn> GetPrimaryKeyReferences(string database, string table, string column)
        {
            var query = GetFindForeignKeysQuery(_connectionPool);
            var results = query.Execute(database, table, column);

            return results;
        }

        /// <summary>
        /// Gets a query that was marked with the attribute DbControllerQueryAttribute.
        /// </summary>
        /// <typeparam name="T">The Type of query.</typeparam>
        /// <returns>The query instance of type <typeparamref name="T"/>.</returns>
        /// <exception cref="ArgumentException">Type <typeparamref name="T"/> was not found in the query cache.</exception>
        public T GetQuery<T>()
        {
            object value;
            if (!_queryObjects.TryGetValue(typeof(T), out value))
            {
                const string errmsg =
                    "Failed to find a query of Type `{0}`. Make sure the attribute `{1}`" + " is attached to the specified class.";
                var err = string.Format(errmsg, typeof(T), typeof(DbControllerQueryAttribute));
                log.Fatal(err);
                Debug.Fail(err);
                throw new ArgumentException(err);
            }

            return (T)value;
        }

        /// <summary>
        /// Finds all of the column names in the given <paramref name="table"/>.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns>All of the column names in the given <paramref name="table"/>.</returns>
        public IEnumerable<string> GetTableColumns(string table)
        {
            var ret = new List<string>();

            using (var conn = _connectionPool.Acquire())
            {
                using (var cmd = conn.Connection.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT * FROM `{0}` WHERE 0=1", table);
                    using (var r = cmd.ExecuteReader())
                    {
                        var fields = r.FieldCount;
                        for (var i = 0; i < fields; i++)
                        {
                            var fieldName = r.GetName(i);
                            ret.Add(fieldName);
                        }
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, removes all of the primary keys from a table where there is no foreign keys for the
        /// respective primary key.
        /// For safety reasons, if a column has no foreign keys, the query will be aborted.
        /// </summary>
        /// <param name="schema">The schema or database name of the table.</param>
        /// <param name="table">The table to check.</param>
        /// <param name="column">The primary key column.</param>
        /// <returns>The number of rows removed, or -1 if there were no foreign keys for the given column in the first place.</returns>
        public abstract int RemoveUnreferencedPrimaryKeys(string schema, string table, string column);

        /// <summary>
        /// Executes raw sql directly against the database with no parameterization or pooling.
        /// Only use this for quick one-off queries, such as queries to run during load/unload of the server or editor-specific tasks.
        /// </summary>
        /// <param name="sql">The sql to execute.</param>
        public abstract void ExecuteNonQuery(string sql);

        /// <summary>
        /// Executes raw sql directly against the database with no parameterization or pooling.
        /// Only use this for quick one-off queries, such as queries to run during load/unload of the server or editor-specific tasks.
        /// </summary>
        /// <param name="sqls">The sql queries to execute. Gets wrapped up in a transaction.</param>
        public abstract void ExecuteNonQueries(params string[] sqls);

        /// <summary>
        /// Executes raw sql directly against the database with no parameterization or pooling.
        /// Only use this for quick one-off queries, such as queries to run during load/unload of the server or editor-specific tasks.
        /// </summary>
        /// <param name="sql">The sql to execute.</param>
        /// <param name="readFunc">The function used to describe how to the results of each row.</param>
        /// <typeparam name="T">The return type for read rows.</typeparam>
        public abstract List<T> ExecuteQuery<T>(string sql, Func<DbDataReader, T> readFunc);

        #endregion
    }
}