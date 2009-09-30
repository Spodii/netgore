using System.Collections.Generic;
using System.Linq;
using DemoGame;
using DemoGame.Server.DbObjs;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectShopIDsQuery : DbQueryReader
    {
        static readonly string _queryStr = string.Format("SELECT `id` FROM `{0}`", ShopTable.TableName);

        /// <summary>
        /// DbQueryReader constructor.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectShopIDsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
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