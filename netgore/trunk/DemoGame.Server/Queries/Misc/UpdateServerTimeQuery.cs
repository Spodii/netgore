using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class UpdateServerTimeQuery : DbQueryNonReader
    {
        /// <summary>
        /// DbQueryNonReader constructor.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public UpdateServerTimeQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // UPDATE `{0}` SET `server_time`=NOW()

            var f = qb.Functions;
            var q = qb.Update(ServerTimeTable.TableName).Add("server_time", f.Now());
            return q.ToString();
        }
    }
}