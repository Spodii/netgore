using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using NetGore.Db;

namespace DemoGame.Server
{
    public class InsertUserEquippedQuery : DbQueryNonReader<InsertUserEquippedQuery.QueryArgs>
    {
        public const string UserEquippedTable = "user_equipped";

        const string _queryString =
            "INSERT INTO `" + UserEquippedTable + "` SET `user_guid`=@userGuid,`item_guid`=@itemGuid,`slot`=@slot";

        public InsertUserEquippedQuery(DbConnectionPool connectionPool) : base(connectionPool, _queryString)
        {
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@itemGuid", "@slot", "@userGuid");
        }

        protected override void SetParameters(DbParameterValues p, QueryArgs item)
        {
            p["@itemGuid"] = item.ItemGuid;
            p["@slot"] = item.Slot;
            p["@userGuid"] = item.UserGuid;
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
            public readonly ushort UserGuid;

            /// <summary>
            /// UpdateUserEquipValues constructor.
            /// </summary>
            /// <param name="userGuid">The guid of the user that this equipment belongs to.</param>
            /// <param name="itemGuid">The item's guid for the equipment slot, or null if the slot is to be set
            /// to empty.</param>
            /// <param name="slot">The EquipmentSlot to be updated.</param>
            public QueryArgs(ushort userGuid, int? itemGuid, EquipmentSlot slot)
            {
                UserGuid = userGuid;
                ItemGuid = itemGuid;
                Slot = slot;
            }
        }
    }
}