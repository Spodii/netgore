using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using NetGore.Db;

namespace DemoGame.Server
{
    public class InsertUserEquippedQuery : DbQueryNonReader<InsertUserEquippedValues>
    {
        public const string UserEquippedTable = "user_equipped";

        const string _queryString =
            "INSERT INTO `" + UserEquippedTable + "` SET `user_guid`=@userGuid,`item_guid`=@itemGuid,`slot`=@slot";

        public InsertUserEquippedQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@itemGuid", "@slot", "@userGuid");
        }

        protected override void SetParameters(DbParameterValues p, InsertUserEquippedValues item)
        {
            p["@itemGuid"] = item.ItemGuid;
            p["@slot"] = item.Slot;
            p["@userGuid"] = item.UserGuid;
        }
    }
}