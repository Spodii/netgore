using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

using NetGore.Db;

namespace DemoGame.Server.Queries
{
    public class DeleteCharacterInventoryItemQuery : DbQueryNonReader<int>
    {
        static readonly string _queryString = string.Format("DELETE FROM `{0}` WHERE `item_id`=@itemID LIMIT 1", DBTables.CharacterInventory);

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