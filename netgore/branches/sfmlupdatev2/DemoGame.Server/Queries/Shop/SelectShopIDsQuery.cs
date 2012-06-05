using System.Collections.Generic;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;
using NetGore.Features.Shops;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectShopIDsQuery : DbQueryReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectShopIDsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectShopIDsQuery(DbConnectionPool connectionPool)
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
            // SELECT `id` FROM `{0}`

            var q = qb.Select(ShopTable.TableName).Add("id");
            return q.ToString();
        }

        public IEnumerable<ShopID> Execute()
        {
            var ret = new List<ShopID>();

            using (var r = ExecuteReader())
            {
                while (r.Read())
                {
                    var shopID = r.GetShopID(0);
                    ret.Add(shopID);
                }
            }

            return ret;
        }
    }
}