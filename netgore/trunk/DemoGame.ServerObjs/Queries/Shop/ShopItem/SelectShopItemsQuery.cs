using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectShopItemsQuery : DbQueryReader<ShopID>
    {
        static readonly string _queryStr = string.Format("SELECT * FROM `{0}` WHERE `shop_id`=@shopID", ShopItemTable.TableName);

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectShopItemsQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public SelectShopItemsQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
            QueryAsserts.ContainsColumns(ShopItemTable.DbColumns, "shop_id");
        }

        public IEnumerable<IShopItemTable> Execute(ShopID shopID)
        {
            var ret = new List<IShopItemTable>();

            using (IDataReader r = ExecuteReader(shopID))
            {
                while (r.Read())
                {
                    ShopItemTable tableValues = new ShopItemTable();
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
            return CreateParameters("@shopID");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, ShopID item)
        {
            p["@shopID"] = (int)item;
        }
    }
}