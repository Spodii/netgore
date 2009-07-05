using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

using NetGore.Db;

namespace DemoGame.Server.Queries
{
    public class InsertCharacterEquippedItemQuery : DbQueryNonReader<InsertCharacterEquippedItemQuery.QueryArgs>
    {
        static readonly string _queryString =
            string.Format("INSERT INTO `{0}` SET `character_id`=@characterID,`item_id`=@itemID,`slot`=@slot", DBTables.CharacterEquipped);

        public InsertCharacterEquippedItemQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@itemID", "@slot", "@userID");
        }

        protected override void SetParameters(DbParameterValues p, QueryArgs item)
        {
            p["@itemID"] = item.ItemID;
            p["@slot"] = item.Slot;
            p["@userID"] = item.UserID;
        }

        /// <summary>
        /// Arguments for the InsertUserEquippedQuery.
        /// </summary>
        public struct QueryArgs
        {
            /// <summary>
            /// The item's ID for the equipment slot, or null if the slot is to be set to empty.
            /// </summary>
            public readonly int? ItemID;

            /// <summary>
            /// The EquipmentSlot to be updated.
            /// </summary>
            public readonly EquipmentSlot Slot;

            /// <summary>
            /// The ID of the user that this equipment belongs to.
            /// </summary>
            public readonly uint UserID;

            /// <summary>
            /// UpdateUserEquipValues constructor.
            /// </summary>
            /// <param name="userID">The ID of the user that this equipment belongs to.</param>
            /// <param name="itemID">The item's ID for the equipment slot, or null if the slot is to be set
            /// to empty.</param>
            /// <param name="slot">The EquipmentSlot to be updated.</param>
            public QueryArgs(uint userID, int? itemID, EquipmentSlot slot)
            {
                UserID = userID;
                ItemID = itemID;
                Slot = slot;
            }
        }
    }
}