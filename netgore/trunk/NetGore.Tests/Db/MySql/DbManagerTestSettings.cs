using System.Linq;
using NetGore;
using NetGore.Tests;

namespace NetGore.Db.MySql.Tests
{
    /// <summary>
    /// Contains information needed to connect to and communicate with the test database for testing pooled connections.
    /// </summary>
    public static class DbManagerTestSettings
    {
        /// <summary>
        /// Creates a DbConnectionPool to be used in tests.
        /// </summary>
        /// <returns>A DbConnectionPool to be used in tests.</returns>
        public static DbConnectionPool CreateConnectionPool()
        {
            return new MySqlDbConnectionPool(TestDb.GetConnectionString());
        }

        /// <summary>
        /// Creates a DbManager to be used in tests.
        /// </summary>
        /// <returns>A DbManager to be used in tests.</returns>
        public static DbManager CreateDbManager()
        {
            return new DbManager(CreateConnectionPool());
        }
    }
}