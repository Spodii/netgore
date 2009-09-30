using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame;
using DemoGame.Server.DbObjs;
using NetGore;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class SelectShopQuery : DbQueryReader<ShopID>
    {
        static readonly string _queryStr = string.Format("SELECT * FROM `{0}` WHERE `id`=@id", ShopTable.TableName);

        /// <summary>
        /// DbQueryReader constructor.
        /// </summary>
        /// <param name="connectionPool">DbConnectionPool to use for creating connections to execute the query on.</param>
        public SelectShopQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryStr)
        {
        }

        public IShopTable Execute(ShopID id)
        {
            ShopTable table;

            using (var r = ExecuteReader(id))
            {
                if (!r.Read())
                    throw new Exception(string.Format("No shop with id `{0}` found.", id));

                table = new ShopTable();
                table.ReadValues(r);
            }

            return table;
        }

        /// <summary>
        /// When overridden in the derived class, creates the parameters this class uses for creating database queries.
        /// </summary>
        /// <returns>IEnumerable of all the <see cref="DbParameter"/>s needed for this class to perform database queries.
        /// If null, no parameters will be used.</returns>
        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@id");
        }

        /// <summary>
        /// When overridden in the derived class, sets the database parameters values <paramref name="p"/>
        /// based on the values specified in the given <paramref name="item"/> parameter.
        /// </summary>
        /// <param name="p">Collection of database parameters to set the values for.</param>
        /// <param name="item">The value or object/struct containing the values used to execute the query.</param>
        protected override void SetParameters(DbParameterValues p, ShopID item)
        {
            p["@id"] = (int)item;
        }
    }
}