using System.Diagnostics;
using System.Linq;
using NetGore;

namespace DemoGame.Server
{
    struct InventoryChangeInfo
    {
        readonly ItemEntity _item;
        readonly ItemValueTracker _oldValues;
        readonly InventorySlot _slot;

        public InventoryChangeInfo(ItemEntity item, ItemValueTracker oldValues, InventorySlot slot)
        {
            _slot = slot;
            _item = item;

            if (oldValues == null || oldValues.IsNull)
                _oldValues = null;
            else
                _oldValues = oldValues;

            Debug.Assert(_item != null || _oldValues != null,
                "item and oldValues can not both be null. " + "This would imply that the item changed from null to null.");
        }

        /// <summary>
        /// Gets the current item that has changed. If null, this means that the item has changed
        /// to null (ie been removed from the inventory).
        /// </summary>
        public ItemEntity Item
        {
            get { return _item; }
        }

        /// <summary>
        /// Gets the previous values of the item. If null, this means the item used to be null.
        /// </summary>
        public ItemValueTracker OldValues
        {
            get { return _oldValues; }
        }

        public InventorySlot Slot
        {
            get { return _slot; }
        }
    }
}