using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace NetGore.Db.MySql
{
    /// <summary>
    /// Pool of MySql database connections.
    /// </summary>
    public class MySqlDbConnectionPool : DbConnectionPool<MySqlConnection>
    {
        public MySqlDbConnectionPool(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates the DbConnection to be used with this ObjectPool.
        /// </summary>
        /// <param name="connectionString">ConnectionString to create the DbConnection with.</param>
        /// <returns>DbConnection to be used with this ObjectPool.</returns>
        protected override MySqlConnection CreateConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }
    }
}
