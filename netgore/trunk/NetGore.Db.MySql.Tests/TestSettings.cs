using MySql.Data.MySqlClient;

namespace NetGore.Db.MySql.Tests
{
    /// <summary>
    /// Static class containing values to be used for the tests.
    /// </summary>
    public static class TestSettings
    {
        /// <summary>
        /// Database to be used for the tests.
        /// </summary>
        public const string Database = "demogame";

        /// <summary>
        /// Database password to be used for the tests.
        /// </summary>
        public const string Password = "";

        /// <summary>
        /// Server to be used for the tests.
        /// </summary>
        public const string Server = "localhost";

        /// <summary>
        /// Database user ID to be used for the tests.
        /// </summary>
        public const string UserID = "root";

        static readonly string _connectionString = new MySqlConnectionStringBuilder
        {
            Server = Server,
            UserID = UserID,
            Database = Database,
            Password = Password
        }.ToString();

        /// <summary>
        /// Gets the ConnectionString to be used for the tests.
        /// </summary>
        public static string ConnectionString
        {
            get { return _connectionString; }
        }

        /// <summary>
        /// Creates a DbConnectionPool to be used in tests.
        /// </summary>
        /// <returns>A DbConnectionPool to be used in tests.</returns>
        public static DbConnectionPool CreateConnectionPool()
        {
            return new MySqlDbConnectionPool(ConnectionString);
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