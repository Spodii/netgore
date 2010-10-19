using System.Linq;
using NetGore;

namespace DemoGame.Server
{
    /// <summary>
    /// Contains and handles the collection of a single <see cref="User"/>'s equipped items.
    /// </summary>
    public class UserEquipped : CharacterEquipped
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserEquipped"/> class.
        /// </summary>
        /// <param name="user">User that this UserEquipped belongs to.</param>
        public UserEquipped(User user) : base(user)
        {
        }

        /// <summary>
        /// Gets the <see cref="User"/> this <see cref="UserEquipped"/> belongs to.
        /// </summary>
        User User
        {
            get { return (User)Character; }
        }

        /// <summary>
        /// When overridden in the derived class, notifies the owner of this object instance
        /// that an equipment slot has changed.
        /// </summary>
        /// <param name="slot">The slot that changed.</param>
        /// <param name="graphicIndex">The new graphic index of the slot.</param>
        protected override void SendSlotUpdate(EquipmentSlot slot, GrhIndex? graphicIndex)
        {
            using (var msg = ServerPacket.UpdateEquipmentSlot(slot, graphicIndex))
            {
                User.Send(msg, ServerMessageType.GUIItems);
            }
        }
    }
}