using System.Collections.Generic;
using System.Threading;
using MySql.Data.MySqlClient;

namespace DemoGame.Db
{
    /// <summary>
    /// A pool for the DataContext for the game database.
    /// </summary>
    public static class DataContextPool
    {
        /// <summary>
        /// Connection string for new database connection objects.
        /// </summary>
        static readonly string _connectionString;

        /// <summary>
        /// Lock for the pool Dictionary.
        /// </summary>
        static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        /// <summary>
        /// Pool matching the DataContext to a Thread.
        /// </summary>
        static readonly Dictionary<Thread, DemoGameDb> _pool = new Dictionary<Thread, DemoGameDb>();

        /// <summary>
        /// DataContextPool static constructor.
        /// </summary>
        static DataContextPool()
        {
            // TODO: Grab these values from a property file...
            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder
                                              { UserID = "root", Password = "", Database = "demogame", Server = "localhost" };

            _connectionString = sb.ToString();
        }

        /// <summary>
        /// Creates a new DataContext.
        /// </summary>
        /// <returns>The new DataContext.</returns>
        static DemoGameDb CreateNew()
        {
            MySqlConnection conn = new MySqlConnection(_connectionString);
            return new DemoGameDb(conn);
        }

        /// <summary>
        /// Gets a DataContext that can be used.
        /// </summary>
        /// <returns>A DataContext that can be used.</returns>
        public static DemoGameDb GetDataContext()
        {
            // Grabs a DataContext by the Thread calling this
            // Allows for a single DataContext per Thread

            DemoGameDb output;
            Thread thread = Thread.CurrentThread;

            _lock.EnterUpgradeableReadLock();
            try
            {
                if (!_pool.TryGetValue(thread, out output))
                {
                    output = CreateNew();
                    _lock.EnterWriteLock();
                    _pool.Add(thread, output);
                    _lock.ExitWriteLock();
                }
            }
            finally
            {
                _lock.ExitUpgradeableReadLock();
            }

            return output;
        }
    }
}