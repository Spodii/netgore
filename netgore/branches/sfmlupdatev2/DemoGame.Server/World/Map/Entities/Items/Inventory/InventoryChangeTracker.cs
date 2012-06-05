using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore;

namespace DemoGame.Server
{
    class InventoryChangeTracker
    {
        readonly ItemValueTracker[] _buffer = new ItemValueTracker[GameData.MaxInventorySize];

        readonly CharacterInventory _inventory;

        /// <summary>
        /// Initializes the <see cref="InventoryChangeTracker"/> class.
        /// </summary>
        static InventoryChangeTracker()
        {
            Debug.Assert(GameData.MaxInventorySize <= byte.MaxValue, "Too many inventory slots to index them with a byte.");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryChangeTracker"/> class.
        /// </summary>
        /// <param name="inventory">The <see cref="CharacterInventory"/> to track the changes for.</param>
        public InventoryChangeTracker(CharacterInventory inventory)
        {
            _inventory = inventory;
        }

        /// <summary>
        /// Gets the <see cref="CharacterInventory"/> that this <see cref="InventoryChangeTracker"/> is tracking.
        /// </summary>
        public CharacterInventory Inventory
        {
            get { return _inventory; }
        }

        /// <summary>
        /// Gets the <see cref="InventoryChangeInfo"/>s for the changes that have been made.
        /// </summary>
        /// <returns>The <see cref="InventoryChangeInfo"/>s for the changes that have been made.</returns>
        public IEnumerable<InventoryChangeInfo> GetChanges()
        {
            // Iterate through every slot
            for (var i = 0; i < _buffer.Length; i++)
            {
                var invItem = _inventory[new InventorySlot(i)];
                var tracker = _buffer[i];

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