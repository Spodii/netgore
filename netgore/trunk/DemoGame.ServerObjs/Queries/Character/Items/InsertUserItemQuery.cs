using System.Collections.Generic;
using System.Data.Common;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class InsertCharacterInventoryItemQuery : DbQueryNonReader<InsertCharacterInventoryItemQuery.QueryArgs>
    {
        static readonly string _queryString = string.Format(
            "INSERT INTO `{0}` SET `character_id`=@characterID,`item_id`=@itemID", DBTables.CharacterInventory);

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
            p["@characterID"] = item.CharacterID;
        }

        /// <summary>
        /// Arguments for the InsertCharacterInventoryItemQuery.
        /// </summary>
        public struct QueryArgs
        {
            public readonly CharacterID CharacterID;
            public readonly ItemID ItemID;

            public QueryArgs(CharacterID characterID, ItemID itemID)
            {
                CharacterID = characterID;
                ItemID = itemID;
            }
        }
    }
}