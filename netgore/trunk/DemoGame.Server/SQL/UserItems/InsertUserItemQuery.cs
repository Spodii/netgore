using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using NetGore.Db;

namespace DemoGame.Server
{
    public class InsertUserItemQuery : DbQueryNonReader<InsertUserItemQuery.QueryArgs>
    {
        const string _queryString = "INSERT INTO `user_inventory` SET `user_guid`=@user_guid,`item_guid`=@item_guid";

        public InsertUserItemQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@item_guid", "@user_guid");
        }

        protected override void SetParameters(DbParameterValues p, QueryArgs item)
        {
            p["@item_guid"] = item.ItemGuid;
            p["@user_guid"] = item.UserGuid;
        }

        /// <summary>
        /// Arguments for the InsertUserItemQuery.
        /// </summary>
        public struct QueryArgs
        {
            public readonly int ItemGuid;
            public readonly ushort UserGuid;

            public QueryArgs(ushort userGuid, int itemGuid)
            {
                UserGuid = userGuid;
                ItemGuid = itemGuid;
            }
        }
    }
}