using System.Linq;
using NetGore.Db;
using NetGore.Db.MySql;

namespace NetGore.Tests.Db.MySql
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
    }
}