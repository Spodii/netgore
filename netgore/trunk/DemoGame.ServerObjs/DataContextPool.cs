using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;

namespace DemoGame.Db
{
    public static class DataContextPool
    {
        static string _connectionString;

        static readonly Dictionary<Thread, DemoGameDb> _pool = new Dictionary<Thread, DemoGameDb>();
        static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        public static void Initialize(string connectionString)
        {
            _connectionString = connectionString;
        }

        static DemoGameDb CreateNew()
        {
            var conn = new MySqlConnection(_connectionString);
            return new DemoGameDb(conn);
        }

        public static DemoGameDb GetDataContext()
        {
            DemoGameDb output;
            var thread = Thread.CurrentThread;

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
