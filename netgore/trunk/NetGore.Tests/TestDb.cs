using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;
using NetGore.Db.ClassCreator;
using NetGore.Tests.Properties;

namespace NetGore.Tests
{
    /// <summary>
    /// Contains the information for the database used to run unit tests.
    /// </summary>
    public static class TestDb
    {
        static string _connectionString;

        /// <summary>
        /// Gets the database to run the tests on.
        /// </summary>
        public static string Database
        {
            get { return "netgoretests"; }
        }

        /// <summary>
        /// Creates a <see cref="MySqlClassGenerator"/> for the test database.
        /// </summary>
        /// <returns>A <see cref="MySqlClassGenerator"/> for the test database.</returns>
        public static MySqlClassGenerator CreateMySqlClassGenerator()
        {
            return new MySqlClassGenerator(Host, User, Password, Database);
        }

        /// <summary>
        /// Executes a raw query.
        /// </summary>
        /// <param name="query">The query to execute.</param>
        public static void Execute(string query)
        {
            var conn = Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
            Close(conn);
        }

        /// <summary>
        /// Gets the database host.
        /// </summary>
        public static string Host
        {
            get { return "localhost"; }
        }

        /// <summary>
        /// Gets the database user's password.
        /// </summary>
        public static string Password
        {
            get { return ""; }
        }

        /// <summary>
        /// Gets the database user.
        /// </summary>
        public static string User
        {
            get { return "root"; }
        }

        /// <summary>
        /// Closes a connection to the test database.
        /// </summary>
        /// <param name="conn">The connection to close.</param>
        public static void Close(DbConnection conn)
        {
            conn.Close();
            conn.Dispose();
        }

        /// <summary>
        /// Gets the connection string to use to connect to the test database.
        /// </summary>
        /// <returns>The connection string to use to connect to the test database.</returns>
        public static string GetConnectionString()
        {
            if (_connectionString == null)
            {
                var sb = new MySqlConnectionStringBuilder
                { UserID = User, Password = Password, Database = Database, Server = Host };
                _connectionString = sb.ToString();
            }

            return _connectionString;
        }

        /// <summary>
        /// Opens a connection to the test database.
        /// </summary>
        /// <returns>The connection to use to talk to the test database.</returns>
        public static DbConnection Open()
        {
            MySqlConnection conn = new MySqlConnection(GetConnectionString());
            conn.Open();
            return conn;
        }
    }
}