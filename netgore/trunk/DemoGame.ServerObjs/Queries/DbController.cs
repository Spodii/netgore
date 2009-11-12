using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.Collections;
using NetGore.Db;
using NetGore.Db.MySql;

namespace DemoGame.Server.Queries
{
    /// <summary>
    /// The <see cref="DbControllerBase"/> implementation used by the server.
    /// </summary>
    public class DbController : MySqlDbController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbControllerBase"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public DbController(string connectionString) : base(connectionString)
        {
        }
    }
}