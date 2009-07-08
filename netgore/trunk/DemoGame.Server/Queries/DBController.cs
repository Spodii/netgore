using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.Db;
using NetGore.Db.MySql;
using DemoGame.Server.Queries;

namespace DemoGame.Server
{
    /// <summary>
    /// Provides an interface between all objects and all the database handling methods.
    /// </summary>
    public class DBController : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly Dictionary<Type, object> _queryObjects = new Dictionary<Type, object>();
        readonly DbConnectionPool _connectionPool;

        bool _disposed;

        public T GetQuery<T>()
        {
            return (T)_queryObjects[typeof(T)];
        }

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
            Type[] requiredConstructorParams = new Type[] { typeof(DbConnectionPool) };
            var types = TypeHelper.FindTypesWithAttribute(typeof(DBControllerQueryAttribute), requiredConstructorParams);

            // Create an instance of each of the types
            foreach (var type in types)
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
                    log.InfoFormat("Created instance of query class Type `{0}`.", type);
            }

            if (log.IsInfoEnabled)
                log.Info("DBController successfully initialized all queries.");
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