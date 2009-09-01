using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Server.Queries;
using log4net;
using NetGore;
using NetGore.Db;
using NetGore.Db.MySql;

namespace DemoGame.Server
{
    /// <summary>
    /// Provides an interface between all objects and all the database handling methods.
    /// </summary>
    public class DBController : IDisposable
    {
        static readonly List<DBController> _instances = new List<DBController>();
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly DbConnectionPool _connectionPool;
        readonly Dictionary<Type, object> _queryObjects = new Dictionary<Type, object>();

        bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="DBController"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public DBController(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");

            // Create the connection pool
            try
            {
                _connectionPool = new MySqlDbConnectionPool(connectionString);
            }
            catch (Exception ex)
            {
                const string msg = "Failed to create connection to MySql database.";
                Debug.Fail(msg);
                if (log.IsFatalEnabled)
                    log.Fatal(msg, ex);
                Dispose();
                return;
            }

            if (log.IsInfoEnabled)
                log.InfoFormat("Database connection pool created.");

            // Find the classes marked with our attribute
            var requiredConstructorParams = new Type[]
            {
                typeof(DbConnectionPool)
            };
            var types = TypeHelper.FindTypesWithAttribute(typeof(DBControllerQueryAttribute), requiredConstructorParams, false);

            // Create an instance of each of the types
            foreach (Type type in types)
            {
                object instance = Activator.CreateInstance(type, _connectionPool);
                if (instance == null)
                {
                    const string errmsg = "Failed to create instance of Type `{0}`.";
                    string err = string.Format(errmsg, type);
                    if (log.IsFatalEnabled)
                        log.Fatal(err);
                    Debug.Fail(err);
                    throw new Exception(err);
                }

                // Add the instance to the collection
                _queryObjects.Add(type, instance);

                if (log.IsInfoEnabled)
                    log.InfoFormat("Created instance of query class Type `{0}`.", type.Name);
            }

            if (log.IsInfoEnabled)
                log.Info("DBController successfully initialized all queries.");

            lock (_instances)
            {
                _instances.Add(this);
            }
        }

        /// <summary>
        /// Gets an instance of the DbController. A DbController must have already been constructed for this to work.
        /// </summary>
        /// <returns>An instance of the DbController.</returns>
        public static DBController GetInstance()
        {
            lock (_instances)
            {
                if (_instances.Count == 0)
                    throw new MemberAccessException("Constructor on the DbController has not yet been called!");

                return _instances[_instances.Count - 1];
            }
        }

        /// <summary>
        /// Gets the name of the table and column that reference a the given primary key.
        /// </summary>
        /// <param name="database">Database of the <paramref name="table"/>.</param>
        /// <param name="table">The table of the primary key.</param>
        /// <param name="column">The column of the primary key.</param>
        /// <returns>An IEnumerable of the name of the tables and columns that reference a the given primary key.</returns>
        public IEnumerable<TableColumnPair> GetPrimaryKeyReferences(string database, string table, string column)
        {
            return GetQuery<FindReferencedTableColumnsQuery>().Execute(database, table, column);
        }

        /// <summary>
        /// Gets a query that was marked with the attribute DBControllerQueryAttribute.
        /// </summary>
        /// <typeparam name="T">The Type of query.</typeparam>
        /// <returns>The query instance of type <typeparamref name="T"/>.</returns>
        public T GetQuery<T>()
        {
            object value;
            if (!_queryObjects.TryGetValue(typeof(T), out value))
            {
                const string errmsg =
                    "Failed to find a query of Type `{0}`. Make sure the attribute `{1}`" + " is attached to the specified class.";
                string err = string.Format(errmsg, typeof(T), typeof(DBControllerQueryAttribute));
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

            using (PooledDbConnection conn = _connectionPool.Create())
            {
                using (DbCommand cmd = conn.Connection.CreateCommand())
                {
                    cmd.CommandText = string.Format("SELECT * FROM `{0}` WHERE 0=1", table);
                    using (DbDataReader r = cmd.ExecuteReader())
                    {
                        int fields = r.FieldCount;
                        for (int i = 0; i < fields; i++)
                        {
                            string fieldName = r.GetName(i);
                            ret.Add(fieldName);
                        }
                    }
                }
            }

            return ret;
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            lock (_instances)
                _instances.Remove(this);

            // Dispose of all the queries, where possible
            foreach (IDisposable disposableQuery in _queryObjects.OfType<IDisposable>())
            {
                disposableQuery.Dispose();
            }

            // Dispose of the DbConnectionPool
            _connectionPool.Dispose();
        }

        #endregion

        [DBControllerQuery]
        // ReSharper disable ClassNeverInstantiated.Local
            class FindReferencedTableColumnsQuery : DbQueryReader<FindReferencedTableColumnsQuery.QueryArgs>
            // ReSharper restore ClassNeverInstantiated.Local
        {
            const string _queryStr =
                "SELECT `TABLE_NAME`, `COLUMN_NAME`" + " FROM information_schema.KEY_COLUMN_USAGE" +
                " WHERE `TABLE_SCHEMA` = @db AND" + " `REFERENCED_TABLE_SCHEMA` = @db AND" +
                " `REFERENCED_TABLE_NAME` = @table AND" + " `REFERENCED_COLUMN_NAME` = @column;";

            /// <summary>
            /// Initializes a new instance of the <see cref="FindReferencedTableColumnsQuery"/> class.
            /// </summary>
            /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
            public FindReferencedTableColumnsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
            {
            }

            public IEnumerable<TableColumnPair> Execute(string database, string table, string column)
            {
                var ret = new List<TableColumnPair>();

                QueryArgs args = new QueryArgs(database, table, column);
                using (IDataReader r = ExecuteReader(args))
                {
                    while (r.Read())
                    {
                        string retTable = r.GetString("TABLE_NAME");
                        string retColumn = r.GetString("COLUMN_NAME");
                        ret.Add(new TableColumnPair(retTable, retColumn));
                    }
                }

                return ret;
            }

            /// <summary>
            /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
            /// </summary>
            /// <returns>IEnumerable of all the DbParameters needed for this class to perform database queries. If null,
            /// no parameters will be used.</returns>
            protected override IEnumerable<DbParameter> InitializeParameters()
            {
                return CreateParameters("@db", "@table", "@column");
            }

            /// <summary>
            /// When overridden in the derived class, sets the database parameters based on the specified item.
            /// </summary>
            /// <param name="p">Collection of database parameters to set the values for.</param>
            /// <param name="item">Item used to execute the query.</param>
            protected override void SetParameters(DbParameterValues p, QueryArgs item)
            {
                p["@db"] = item.Database;
                p["@table"] = item.Table;
                p["@column"] = item.Column;
            }

            public struct QueryArgs
            {
                public readonly string Column;
                public readonly string Database;
                public readonly string Table;

                public QueryArgs(string database, string table, string column)
                {
                    Database = database;
                    Table = table;
                    Column = column;
                }
            }
        }
    }
}