using System.Linq;
using NetGore;

namespace DemoGame.Server
{
    /// <summary>
    /// An inventory for a single User on the Server
    /// </summary>
    class UserInventory : CharacterInventory
    {
        readonly UserInventoryUpdater _inventoryUpdater;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInventory"/> class.
        /// </summary>
        /// <param name="user">User that the inventory is for</param>
        public UserInventory(Character user) : base(user)
        {
            _inventoryUpdater = new UserInventoryUpdater(this);
        }

        /// <summary>
        /// When overridden in the derived class, notifies the Client that a slot in the Inventory has changed.
        /// </summary>
        /// <param name="slot">The slot that changed.</param>
        protected override void SendSlotUpdate(InventorySlot slot)
        {
            _inventoryUpdater.SlotChanged(slot);
        }

        /// <summary>
        /// Updates the client controlling the User that this Inventory belongs to with all the
        /// most accurate inventory information.
        /// </summary>
        public void UpdateClient()
        {
            _inventoryUpdater.Update();
        }
    }
}