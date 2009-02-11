using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DemoGame.Extensions;
using log4net;

namespace DemoGame.Server
{
    public abstract class Inventory : InventoryBase<ItemEntity>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the Character that this Inventory belongs to.
        /// </summary>
        public abstract Character Character { get; }

        /// <summary>
        /// Makes the Character drop the item from their Inventory.
        /// </summary>
        /// <param name="slot">Slot of the item to drop.</param>
        /// <returns>True if the item was successfully dropped, else false.</returns>
        public bool Drop(int slot)
        {
            // Get the item to drop
            ItemEntity dropItem = this[slot];

            // Check for an invalid item
            if (dropItem == null)
            {
                const string errmsg = "Could not drop item since no item exists at slot `{0}`.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, slot);
                return false;
            }

            // Remove the item from the inventory
            RemoveAt(slot);

            // Drop the item
            Character.DropItem(dropItem);

            return true;
        }
    }
}