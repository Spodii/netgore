using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace DemoGame.Server
{
    class InventoryChangeTracker
    {
        readonly ItemValueTracker[] _buffer = new ItemValueTracker[Inventory.MaxInventorySize];

        readonly Inventory _inventory;

        /// <summary>
        /// Gets the Inventory that this InventoryChangeTracker is tracking
        /// </summary>
        public Inventory Inventory
        {
            get { return _inventory; }
        }

        public InventoryChangeTracker(Inventory inventory)
        {
            Debug.Assert(Inventory.MaxInventorySize <= byte.MaxValue, "Too many inventory slots to index them with a byte.");

            _inventory = inventory;
        }

        public IEnumerable<InventoryChangeInfo> GetChanges()
        {
            // Iterate through every slot
            for (int i = 0; i < _buffer.Length; i++)
            {
                ItemEntity invItem = _inventory[new InventorySlot(i)];
                ItemValueTracker tracker = _buffer[i];

                // If the values are already equal, skip it
                if (ItemValueTracker.AreValuesEqual(invItem, tracker))
                    continue;

                // Yield return the changed item results
                yield return new InventoryChangeInfo(invItem, tracker, new InventorySlot(i));

                // Update the item with the new values, creating the tracker if needed
                if (tracker == null)
                    _buffer[i] = new ItemValueTracker();
                _buffer[i].SetValues(invItem);
            }
        }
    }
}