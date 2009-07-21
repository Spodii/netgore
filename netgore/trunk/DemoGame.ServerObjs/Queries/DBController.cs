using System;
using System.Collections.Generic;
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
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly DbConnectionPool _connectionPool;
        readonly Dictionary<Type, object> _queryObjects = new Dictionary<Type, object>();

        bool _disposed;

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

            DBTableStatTypes.Initialize(this);

            // Find the classes marked with our attribute
            var requiredConstructorParams = new Type[] { typeof(DbConnectionPool) };
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
        }

        public T GetQuery<T>()
        {
            return (T)_queryObjects[typeof(T)];
        }

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

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            // Dispose of all the queries, where possible
            foreach (IDisposable disposableQuery in _queryObjects.OfType<IDisposable>())
            {
                disposableQuery.Dispose();
            }

            // Dispose of the DbConnectionPool
            _connectionPool.Dispose();
        }

        #endregion
    }
}