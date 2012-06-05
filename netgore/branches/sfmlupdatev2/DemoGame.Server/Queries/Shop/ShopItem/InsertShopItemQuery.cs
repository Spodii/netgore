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
    public class InsertShopItemQuery : DbQueryNonReader<IShopItemTable>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertShopItemQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public InsertShopItemQuery(DbConnectionPool connectionPool)
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
            // INSERT IGNORE INTO {0} {1}

            var q = qb.Insert(ShopItemTable.TableName).IgnoreExists().AddAutoParam(ShopItemTable.DbColumns);
            return q.ToString();
        }

        public void Execute(ShopID shopID, ItemTemplateID itemID)
        {
            Execute(new ShopItemTable(itemID, shopID));
        }

        public void Execute(ShopID shopID, IEnumerable<ItemTemplateID> itemIDs)
        {
            foreach (var itemID in itemIDs)
            {
                Execute(shopID, itemID);
            }
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
            return CreateParameters(ShopItemTable.DbColumns);
        }

        /// <summary>
        /// Sets the parameters.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="item">The item.</param>
        protected override void SetParameters(DbParameterValues p, IShopItemTable item)
        {
            item.CopyValues(p);
        }
    }
}