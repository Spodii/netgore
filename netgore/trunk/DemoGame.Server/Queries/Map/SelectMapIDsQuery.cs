using System.Collections.Generic;
using System.Linq;
using DemoGame.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;
using NetGore.World;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectMapIDsQuery : DbQueryReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectMapIDsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectMapIDsQuery(DbConnectionPool connectionPool) : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // SELECT `id` FROM `{0}`

            var q = qb.Select(MapTable.TableName).Add("id");
            return q.ToString();
        }

        public IEnumerable<MapID> Execute()
        {
            var ret = new List<MapID>();
            using (var r = ExecuteReader())
            {
                while (r.Read())
                {
                    var i = r.GetMapID(0);
                    ret.Add(i);
                }
            }

            return ret;
        }
    }
}