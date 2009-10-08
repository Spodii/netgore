using System.Linq;
using NetGore;
using NetGore.Db;
using NetGore.Db.MySql;

namespace DemoGame.Server.Queries
{
    /// <summary>
    /// The <see cref="DbControllerBase"/> implementation used by the server.
    /// </summary>
    public class DbController : DbControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbControllerBase"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public DbController(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// When overridden in the derived class, creates a DbConnectionPool for this DbController.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>
        /// A DbConnectionPool for this DbController.
        /// </returns>
        protected override DbConnectionPool CreateConnectionPool(string connectionString)
        {
            return new MySqlDbConnectionPool(connectionString);
        }
    }
}