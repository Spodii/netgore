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
        static readonly string _queryString = string.Format("INSERT INTO `{0}` SET `character_id`=@characterID,`item_id`=@itemID", DBTables.CharacterInventory);

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