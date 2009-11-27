using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SetAccountCurrentIPsNullQuery : DbQueryNonReader
    {
        static readonly string _queryStr = string.Format("UPDATE `{0}` SET `current_ip` = NULL", AccountTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SetAccountCurrentIPsNullQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SetAccountCurrentIPsNullQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
        }
    }
}