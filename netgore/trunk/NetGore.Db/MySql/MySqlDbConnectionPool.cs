using System.Data.Common;
using MySql.Data.MySqlClient;

namespace NetGore.Db.MySql
{
    /// <summary>
    /// Pool of MySql database connections.
    /// </summary>
    public class MySqlDbConnectionPool : DbConnectionPool
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbConnectionPool"/> class.
        /// </summary>
        /// <param name="connectionString">ConnectionString to create the MySqlConnections with.</param>
        public MySqlDbConnectionPool(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates the DbConnection to be used with this ObjectPool.
        /// </summary>
        /// <param name="connectionString">ConnectionString to create the DbConnection with.</param>
        /// <returns>DbConnection to be used with this ObjectPool.</returns>
        protected override DbConnection CreateConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        /// <summary>
        /// When overridden in the derived class, creates and returns a DbParameter that is compatible with
        /// the type of database used by connections in this pool.
        /// </summary>
        /// <param name="parameterName">Reference name of the parameter.</param>
        /// <returns>DbParameter that is compatible with the connections in this DbConnectionPool.</returns>
        public override DbParameter CreateParameter(string parameterName)
        {
            return new MySqlParameter(parameterName, null);
        }
    }
}