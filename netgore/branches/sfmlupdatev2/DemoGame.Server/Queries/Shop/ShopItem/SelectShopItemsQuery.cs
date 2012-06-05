using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;
using NetGore.Features.Shops;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectShopItemsQuery : DbQueryReader<ShopID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectShopItemsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectShopItemsQuery(DbConnectionPool connectionPool)
            : base(connectionPool, CreateQuery(connectionPool.QueryBuilder))
        {
            QueryAsserts.ContainsColumns(ShopItemTable.DbColumns, "shop_id");
        }

        /// <summary>
        /// Creates the query for this class.
        /// </summary>
        /// <param name="qb">The <see cref="IQueryBuilder"/> instance.</param>
        /// <returns>The query for this class.</returns>
        static string CreateQuery(IQueryBuilder qb)
        {
            // SELECT * FROM `{0}` WHERE `shop_id`=@shopID

            var f = qb.Functions;
            var s = qb.Settings;
            var q =
                qb.Select(ShopItemTable.TableName).AllColumns().Where(f.Equals(s.EscapeColumn("shop_id"), s.Parameterize("shopID")));
            return q.ToString();
        }

        public IEnumerable<IShopItemTable> Execute(ShopID shopID)
        {
            var ret = new List<IShopItemTable>();

            using (var r = ExecuteReader(shopID))
            {
                while (r.Read())
                {
                    var tableValues = new ShopItemTable();
                    tableValues.ReadValues(r);
                    ret.Add(tableValues);
                }
            }

            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("shopID");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, ShopID item)
        {
            p["shopID"] = (int)item;
        }
    }
}