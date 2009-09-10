using System.Diagnostics;
using System.Linq;

namespace DemoGame.Server
{
    struct InventoryChangeInfo
    {
        readonly ItemEntity _item;
        readonly ItemValueTracker _oldValues;
        readonly InventorySlot _slot;

        /// <summary>
        /// Gets the current characterID that has changed. If null, this means that the characterID has changed
        /// to null (ie been removed from the inventory).
        /// </summary>
        public ItemEntity Item
        {
            get { return _item; }
        }

        /// <summary>
        /// Gets the previous values of the characterID. If null, this means the characterID the characterID used to be null.
        /// </summary>
        public ItemValueTracker OldValues
        {
            get { return _oldValues; }
        }

        public InventorySlot Slot
        {
            get { return _slot; }
        }

        public InventoryChangeInfo(ItemEntity item, ItemValueTracker oldValues, InventorySlot slot)
        {
            _slot = slot;
            _item = item;

            if (oldValues == null || oldValues.IsNull)
                _oldValues = null;
            else
                _oldValues = oldValues;

            Debug.Assert(_item != null || _oldValues != null,
                         "characterID and oldValues can not both be null. " + "This would imply that the characterID changed from null to null.");
        }
    }
}