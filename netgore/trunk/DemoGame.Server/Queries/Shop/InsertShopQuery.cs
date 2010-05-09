using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DemoGame.DbObjs;
using DemoGame.Server.DbObjs;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DbControllerQuery]
    public class InsertShopQuery : DbQueryNonReader<IShopTable>
    {
        static readonly string _queryStr = string.Format("INSERT INTO `{0}` {1}", ShopTable.TableName, FormatParametersIntoValuesString(ShopTable.DbColumns));

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertShopQuery"/> class.
        /// </summary>
        /// <param name="connectionPool">The connection pool.</param>
        public InsertShopQuery(DbConnectionPool connectionPool)
            : base(connectionPool, _queryStr)
        {
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
            return CreateParameters(ShopTable.DbColumns.Select(x => "@" + x));
        }

        /// <summary>
        /// Sets the parameters.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="item">The item.</param>
        protected override void SetParameters(DbParameterValues p, IShopTable item)
        {
            item.CopyValues(p);
        }
    }
}