using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

using NetGore.Db;

namespace DemoGame.Server
{
    public class InsertCharacterInventoryItemQuery : DbQueryNonReader<InsertCharacterInventoryItemQuery.QueryArgs>
    {
        const string _queryString = "INSERT INTO `character_inventory`"
            + " SET `character_id`=@characterID,`item_id`=@itemID";

        public InsertCharacterInventoryItemQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@itemID", "@characterID");
        }

        protected override void SetParameters(DbParameterValues p, QueryArgs item)
        {
            p["@itemID"] = item.ItemID;
            p["@characterID"] = item.UserID;
        }

        /// <summary>
        /// Arguments for the InsertUserItemQuery.
        /// </summary>
        public struct QueryArgs
        {
            public readonly int ItemID;
            public readonly uint UserID;

            public QueryArgs(uint userID, int itemID)
            {
                UserID = userID;
                ItemID = itemID;
            }
        }
    }
}