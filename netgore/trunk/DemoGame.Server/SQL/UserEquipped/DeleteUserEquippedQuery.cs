using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

using NetGore.Db;

namespace DemoGame.Server
{
    public class DeleteUserEquippedQuery : DbQueryNonReader<int>
    {
        const string _queryString =
            "DELETE FROM `" + InsertUserEquippedQuery.UserEquippedTable + "` WHERE `item_guid`=@itemGuid LIMIT 1";

        public DeleteUserEquippedQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@itemGuid");
        }

        protected override void SetParameters(DbParameterValues p, int item)
        {
            p["@itemGuid"] = item;
        }
    }
}