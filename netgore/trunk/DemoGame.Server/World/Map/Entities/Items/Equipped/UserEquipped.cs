using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.Network;

namespace DemoGame.Server
{
    /// <summary>
    /// Contains and handles the collection of a single User's equipped items.
    /// </summary>
    public class UserEquipped : CharacterEquipped
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly User _user;

        /// <summary>
        /// UserEquipped constructor.
        /// </summary>
        /// <param name="user">User that this UserEquipped belongs to.</param>
        // ReSharper disable SuggestBaseTypeForParameter
        public UserEquipped(User user) : base(user) // ReSharper restore SuggestBaseTypeForParameter
        {
            _user = user;
        }

        User User
        {
            get { return _user; }
        }

        protected override void SendSlotUpdate(EquipmentSlot slot, GrhIndex? graphicIndex)
        {
            using (PacketWriter msg = ServerPacket.UpdateEquipmentSlot(slot, graphicIndex))
            {
                User.Send(msg);
            }
        }
    }
}