using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using Platyform.Extensions;

namespace DemoGame.Server
{
    /// <summary>
    /// Contains the values needed to update a User's equipment slot.
    /// </summary>
    public class InsertUserEquippedValues
    {
        readonly int? _itemGuid;
        readonly EquipmentSlot _slot;
        readonly ushort _userGuid;

        /// <summary>
        /// Gets the item's guid for the equipment slot, or null if the slot is to be set
        /// to empty.
        /// </summary>
        public int? ItemGuid
        {
            get { return _itemGuid; }
        }

        /// <summary>
        /// Gets the EquipmentSlot to be updated.
        /// </summary>
        public EquipmentSlot Slot
        {
            get { return _slot; }
        }

        /// <summary>
        /// Gets the guid of the user that this equipment belongs to.
        /// </summary>
        public ushort UserGuid
        {
            get { return _userGuid; }
        }

        /// <summary>
        /// UpdateUserEquipValues constructor.
        /// </summary>
        /// <param name="userGuid">The guid of the user that this equipment belongs to.</param>
        /// <param name="itemGuid">The item's guid for the equipment slot, or null if the slot is to be set
        /// to empty.</param>
        /// <param name="slot">The EquipmentSlot to be updated.</param>
        public InsertUserEquippedValues(ushort userGuid, int? itemGuid, EquipmentSlot slot)
        {
            _userGuid = userGuid;
            _itemGuid = itemGuid;
            _slot = slot;
        }
    }
}