using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

using NetGore.Db;

namespace DemoGame.Server
{
    public class InsertCharacterEquippedItemQuery : DbQueryNonReader<InsertCharacterEquippedItemQuery.QueryArgs>
    {
        const string _queryString =
            "INSERT INTO `character_equipped` SET `character_guid`=@characterID,`item_guid`=@itemID,`slot`=@slot";

        public InsertCharacterEquippedItemQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@itemID", "@slot", "@userID");
        }

        protected override void SetParameters(DbParameterValues p, QueryArgs item)
        {
            p["@itemID"] = item.ItemGuid;
            p["@slot"] = item.Slot;
            p["@userID"] = item.UserGuid;
        }

        /// <summary>
        /// Arguments for the InsertUserEquippedQuery.
        /// </summary>
        public struct QueryArgs
        {
            /// <summary>
            /// The item's guid for the equipment slot, or null if the slot is to be set to empty.
            /// </summary>
            public readonly int? ItemGuid;

            /// <summary>
            /// The EquipmentSlot to be updated.
            /// </summary>
            public readonly EquipmentSlot Slot;

            /// <summary>
            /// The guid of the user that this equipment belongs to.
            /// </summary>
            public readonly uint UserGuid;

            /// <summary>
            /// UpdateUserEquipValues constructor.
            /// </summary>
            /// <param name="userGuid">The guid of the user that this equipment belongs to.</param>
            /// <param name="itemGuid">The item's guid for the equipment slot, or null if the slot is to be set
            /// to empty.</param>
            /// <param name="slot">The EquipmentSlot to be updated.</param>
            public QueryArgs(uint userGuid, int? itemGuid, EquipmentSlot slot)
            {
                UserGuid = userGuid;
                ItemGuid = itemGuid;
                Slot = slot;
            }
        }
    }
}