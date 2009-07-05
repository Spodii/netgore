using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

using NetGore.Db;

namespace DemoGame.Server.Queries
{
    public class DeleteCharacterEquippedItemQuery : DbQueryNonReader<int>
    {
        static readonly string _queryString =
            string.Format("DELETE FROM `{0}` WHERE `item_id`=@itemID LIMIT 1", DBTables.CharacterEquipped);

        public DeleteCharacterEquippedItemQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
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