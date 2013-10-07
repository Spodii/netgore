using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;
using NetGore.Db.QueryBuilder;
using NetGore.Features.Shops;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class DeleteShopItemsQuery : DbQueryNonReader<ShopID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteShopItemsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public DeleteShopItemsQuery(DbConnectionPool connectionPool)
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
            // DELETE FROM `{0}` WHERE `shop_id`=@id

            var f = qb.Functions;
            var s = qb.Settings;
            var q = qb.Delete(ShopItemTable.TableName).Where(f.Equals(s.EscapeColumn("shop_id"), s.Parameterize("id")));
            return q.ToString();
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>
        /// IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.
        /// </returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("id");
        }

        /// <summary>
        /// Sets the parameters.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="item">The item.</param>
        protected override void SetParameters(DbParameterValues p, ShopID item)
        {
            p["id"] = (int)item;
        }
    }
}