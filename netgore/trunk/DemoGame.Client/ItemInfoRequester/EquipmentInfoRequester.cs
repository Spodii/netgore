using System.Linq;
using DemoGame.DbObjs;
using NetGore.IO;
using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// Manages requests for the item information for equipped items.
    /// </summary>
    public class EquipmentInfoRequester : ItemInfoRequesterBase<EquipmentSlot>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentInfoRequester"/> class.
        /// </summary>
        /// <param name="equipment">The equipment.</param>
        /// <param name="socket">The socket.</param>
        public EquipmentInfoRequester(UserEquipped equipment, INetworkSender socket) : base(socket)
        {
            Equipment = equipment;
        }

        /// <summary>
        /// Gets or sets the <see cref="UserEquipped"/> the requests are being made for.
        /// </summary>
        public UserEquipped Equipment { get; set; }

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="BitStream"/> containing the data
        /// needed to make a request for the given slot's <see cref="IItemTable"/>.
        /// </summary>
        /// <param name="slotToRequest">The slot to request the <see cref="IItemTable"/> for.</param>
        /// <returns>The <see cref="BitStream"/> containing the data needed to make a request for
        /// the given slot's <see cref="IItemTable"/>.</returns>
        protected override BitStream GetRequest(EquipmentSlot slotToRequest)
        {
            return ClientPacket.GetEquipmentItemInfo(slotToRequest);
        }

        /// <summary>
        /// When overridden in the derived class, gets the object in the <paramref name="slot"/>. It doesn't matter what kind of object
        /// is used, just as long as it is an object that can be used to determine if the slot's contents have changed.
        /// </summary>
        /// <param name="slot">The slot of the item.</param>
        /// <returns>An object representing the item in the <paramref name="slot"/>. Can be null if the slot is empty.</returns>
        protected override object GetSlotObject(EquipmentSlot slot)
        {
            return Equipment[slot];
        }

        /// <summary>
        /// When overridden in the derived class, gets if the given <paramref name="slot"/> contains a null
        /// <see cref="IItemTable"/>. This allows for bypassing having to request the item info for a slot that
        /// is known to be empty.
        /// </summary>
        /// <param name="slot">The slot.</param>
        /// <returns>True if the <see cref="slot"/> is known to be null; otherwise false.</returns>
        protected override bool IsSlotNull(EquipmentSlot slot)
        {
            if (Equipment != null)
                return Equipment[slot] == null;

            return false;
        }
    }
}