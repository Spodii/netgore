using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SetAccountCurrentIPsNullQuery : DbQueryNonReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SetAccountCurrentIPsNullQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SetAccountCurrentIPsNullQuery(DbConnectionPool connectionPool)
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
            // UPDATE `{0}` SET `current_ip` = NULL

            var q = qb.Update(AccountTable.TableName).Add("current_ip", "NULL");
            return q.ToString();
        }
    }
}