using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

using NetGore.Db;

namespace DemoGame.Server
{
    public class DeleteCharacterInventoryItemQuery : DbQueryNonReader<int>
    {
        const string _queryString = "DELETE QUICK FROM `character_inventory` WHERE `item_guid`=@itemID LIMIT 1";

        public DeleteCharacterInventoryItemQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@itemID");
        }

        protected override void SetParameters(DbParameterValues p, int itemID)
        {
            p["@itemID"] = itemID;
        }
    }
}