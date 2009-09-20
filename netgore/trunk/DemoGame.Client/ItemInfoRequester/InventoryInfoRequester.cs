using System.Linq;
using NetGore.IO;
using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// Manages requests for the <see cref="ItemInfo"/> for inventory items.
    /// </summary>
    class InventoryInfoRequester : ItemInfoRequesterBase<InventorySlot>
    {
        Inventory _inventory;

        /// <summary>
        /// Gets or sets the <see cref="Inventory"/> the requests are being made for.
        /// </summary>
        public Inventory Inventory { get { return _inventory; } set { _inventory = value; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryInfoRequester"/> class.
        /// </summary>
        /// <param name="inventory">The inventory.</param>
        /// <param name="socket">The socket.</param>
        public InventoryInfoRequester(Inventory inventory, ISocketSender socket)
            : base(socket)
        {
            _inventory = inventory;
        }

        /// <summary>
        /// When overridden in the derived class, gets if the given <paramref name="slot"/> contains a null
        /// <see cref="ItemInfo"/>. This allows for bypassing having to request the item info for a slot that
        /// is known to be empty.
        /// </summary>
        /// <param name="slot">The slot.</param>
        /// <returns>
        /// True if the <see cref="slot"/> is known to be null; otherwise false.
        /// </returns>
        protected override bool IsSlotNull(InventorySlot slot)
        {
            if (Inventory != null)
                return Inventory[slot] == null;

            return false;
        }

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="BitStream"/> containing the data
        /// needed to make a request for the given slot's <see cref="ItemInfo"/>.
        /// </summary>
        /// <param name="slotToRequest">The slot to request the <see cref="ItemInfo"/> for.</param>
        /// <returns>The <see cref="BitStream"/> containing the data needed to make a request for
        /// the given slot's <see cref="ItemInfo"/>.</returns>
        protected override BitStream GetRequest(InventorySlot slotToRequest)
        {
            return ClientPacket.GetInventoryItemInfo(slotToRequest);
        }
    }
}