using System;
using System.Collections.Generic;
using System.Text;

namespace NetGore.Db.MySql
{
    /// <summary>
    /// A <see cref="DbControllerBase"/> for MySql.
    /// </summary>
    public class MySqlDbController : DbControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlDbController"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public MySqlDbController(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates a DbConnectionPool for this DbController.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>A DbConnectionPool for this DbController.</returns>
        protected override DbConnectionPool CreateConnectionPool(string connectionString)
        {
            return new MySqlDbConnectionPool(connectionString);
        }

        /// <summary>
        /// Gets the SQL query string used for when testing if the database connection is valid. This string should
        /// be very simple and fool-proof, and work no matter what contents are in the database since this is just
        /// to test if the connection is working.
        /// </summary>
        /// <returns>The SQL query string used for when testing if the database connection is valid.</returns>
        protected override string GetTestQueryCommand()
        {
            return "SELECT 1+5";
        }

        /// <summary>
        /// Gets an implementation of the <see cref="FindForeignKeysQuery"/> that works for this
        /// <see cref="DbControllerBase"/>.
        /// </summary>
        /// <param name="dbConnectionPool">The <see cref="DbConnectionPool"/> to use when creating the query.</param>
        /// <returns>The <see cref="FindForeignKeysQuery"/> to execute the query.</returns>
        protected override FindForeignKeysQuery GetFindForeignKeysQuery(DbConnectionPool dbConnectionPool)
        {
            return new MySqlFindForeignKeysQuery(dbConnectionPool);
        }
    }
}
