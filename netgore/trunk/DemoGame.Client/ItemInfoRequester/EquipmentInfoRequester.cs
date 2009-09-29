using System.Linq;
using NetGore.IO;
using NetGore.Network;

namespace DemoGame.Client
{
    /// <summary>
    /// Manages requests for the <see cref="ItemInfo"/> for equipped items.
    /// </summary>
    class EquipmentInfoRequester : ItemInfoRequesterBase<EquipmentSlot>
    {
        UserEquipped _equipment;

        /// <summary>
        /// Gets or sets the <see cref="UserEquipped"/> the requests are being made for.
        /// </summary>
        public UserEquipped Equipment
        {
            get { return _equipment; }
            set { _equipment = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentInfoRequester"/> class.
        /// </summary>
        /// <param name="equipment">The equipment.</param>
        /// <param name="socket">The socket.</param>
        public EquipmentInfoRequester(UserEquipped equipment, ISocketSender socket) : base(socket)
        {
            _equipment = equipment;
        }

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="BitStream"/> containing the data
        /// needed to make a request for the given slot's <see cref="ItemInfo"/>.
        /// </summary>
        /// <param name="slotToRequest">The slot to request the <see cref="ItemInfo"/> for.</param>
        /// <returns>The <see cref="BitStream"/> containing the data needed to make a request for
        /// the given slot's <see cref="ItemInfo"/>.</returns>
        protected override BitStream GetRequest(EquipmentSlot slotToRequest)
        {
            return ClientPacket.GetEquipmentItemInfo(slotToRequest);
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
        protected override bool IsSlotNull(EquipmentSlot slot)
        {
            if (Equipment != null)
                return Equipment[slot] == null;

            return false;
        }
    }
}