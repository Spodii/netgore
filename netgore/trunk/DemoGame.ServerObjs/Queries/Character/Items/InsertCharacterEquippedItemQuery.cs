using System.Collections.Generic;
using System.Data.Common;
using NetGore.Db;

namespace DemoGame.Server.Queries
{
    [DBControllerQuery]
    public class InsertCharacterEquippedItemQuery : DbQueryNonReader<InsertCharacterEquippedItemQuery.QueryArgs>
    {
        static readonly string _queryString =
            string.Format("INSERT INTO `{0}` SET `character_id`=@characterID,`item_id`=@itemID,`slot`=@slot",
                          DBTables.CharacterEquipped);

        public InsertCharacterEquippedItemQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@itemID", "@slot", "@characterID");
        }

        protected override void SetParameters(DbParameterValues p, QueryArgs item)
        {
            p["@itemID"] = item.ItemID;
            p["@slot"] = item.Slot;
            p["@characterID"] = item.CharacterID;
        }

        /// <summary>
        /// Arguments for the InsertUserEquippedQuery.
        /// </summary>
        public struct QueryArgs
        {
            /// <summary>
            /// The ID of the Character that this equipment belongs to.
            /// </summary>
            public readonly CharacterID CharacterID;

            /// <summary>
            /// The item's ID for the equipment slot, or null if the slot is to be set to empty.
            /// </summary>
            public readonly ItemID? ItemID;

            /// <summary>
            /// The EquipmentSlot to be updated.
            /// </summary>
            public readonly EquipmentSlot Slot;

            /// <summary>
            /// InsertUserEquippedQuery constructor.
            /// </summary>
            /// <param name="characterID">The ID of the Character that this equipment belongs to.</param>
            /// <param name="itemID">The item's ID for the equipment slot, or null if the slot is to be set
            /// to empty.</param>
            /// <param name="slot">The EquipmentSlot to be updated.</param>
            public QueryArgs(CharacterID characterID, ItemID? itemID, EquipmentSlot slot)
            {
                CharacterID = characterID;
                ItemID = itemID;
                Slot = slot;
            }
        }
    }
}