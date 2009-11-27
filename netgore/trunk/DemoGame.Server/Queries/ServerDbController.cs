using System.Linq;
using NetGore.Db;
using NetGore.Db.MySql;

namespace DemoGame.Server.Queries
{
    /// <summary>
    /// The <see cref="DbControllerBase"/> implementation used by the server.
    /// </summary>
    public class ServerDbController : MySqlDbController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerDbController"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ServerDbController(string connectionString) : base(connectionString)
        {
        }
    }
}