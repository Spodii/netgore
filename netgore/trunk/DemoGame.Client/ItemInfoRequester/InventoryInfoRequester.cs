using System.Linq;
using DemoGame.DbObjs;
using NetGore;
using NetGore.IO;
using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// Manages requests for the item information for inventory items.
    /// </summary>
    public class InventoryInfoRequester : ItemInfoRequesterBase<InventorySlot>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryInfoRequester"/> class.
        /// </summary>
        /// <param name="inventory">The inventory.</param>
        /// <param name="socket">The socket.</param>
        public InventoryInfoRequester(Inventory inventory, INetworkSender socket) : base(socket)
        {
            Inventory = inventory;
        }

        /// <summary>
        /// Gets or sets the <see cref="Inventory"/> the requests are being made for.
        /// </summary>
        public Inventory Inventory { get; set; }

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="BitStream"/> containing the data
        /// needed to make a request for the given slot's <see cref="IItemTable"/>.
        /// </summary>
        /// <param name="slotToRequest">The slot to request the <see cref="IItemTable"/> for.</param>
        /// <returns>The <see cref="BitStream"/> containing the data needed to make a request for
        /// the given slot's <see cref="IItemTable"/>.</returns>
        protected override BitStream GetRequest(InventorySlot slotToRequest)
        {
            return ClientPacket.GetInventoryItemInfo(slotToRequest);
        }

        /// <summary>
        /// When overridden in the derived class, gets the object in the <paramref name="slot"/>. It doesn't matter what kind of object
        /// is used, just as long as it is an object that can be used to determine if the slot's contents have changed.
        /// </summary>
        /// <param name="slot">The slot of the item.</param>
        /// <returns>An object representing the item in the <paramref name="slot"/>. Can be null if the slot is empty.</returns>
        protected override object GetSlotObject(InventorySlot slot)
        {
            return Inventory[slot];
        }

        /// <summary>
        /// When overridden in the derived class, gets if the given <paramref name="slot"/> contains a null
        /// <see cref="IItemTable"/>. This allows for bypassing having to request the item info for a slot that
        /// is known to be empty.
        /// </summary>
        /// <param name="slot">The slot.</param>
        /// <returns>True if the <see cref="slot"/> is known to be null; otherwise false.</returns>
        protected override bool IsSlotNull(InventorySlot slot)
        {
            if (Inventory != null)
                return Inventory[slot] == null;

            return false;
        }
    }
}