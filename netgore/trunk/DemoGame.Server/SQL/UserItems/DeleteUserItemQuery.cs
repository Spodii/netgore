using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using NetGore.Db;

namespace DemoGame.Server
{
    public class DeleteUserItemQuery : DbQueryNonReader<int>
    {
        const string _queryString = "DELETE QUICK FROM `user_inventory` WHERE `item_guid`=@item_guid LIMIT 1";

        public DeleteUserItemQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@item_guid");
        }

        protected override void SetParameters(DbParameterValues p, int item)
        {
            p["@item_guid"] = item;
        }
    }
}