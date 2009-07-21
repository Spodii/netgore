 // FUTURE: There may be a bug in picking up items, from threading conflict, that will allow two people to pick up the same item

namespace DemoGame.Server
{
    /// <summary>
    /// An inventory for a single User on the Server
    /// </summary>
    class UserInventory : CharacterInventory
    {
        readonly UserInventoryUpdater _inventoryUpdater;

        /// <summary>
        /// UserInventory constructor
        /// </summary>
        /// <param name="user">User that the inventory is for</param>
        public UserInventory(User user) : base(user)
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