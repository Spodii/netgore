using System.Diagnostics;
using System.Linq;
using NetGore;

namespace DemoGame.Client
{
    public class UserEquipped : EquippedBase<ItemEntity>
    {
        public override bool CanEquip(ItemEntity item)
        {
            return true;
        }

        protected override bool CanRemove(EquipmentSlot slot)
        {
            return true;
        }

        public void ClearSlot(EquipmentSlot slot)
        {
            if (!TrySetSlot(slot, null))
                Debug.Fail("Failed to unset the slot for some reason.");
        }

        public void SetSlot(EquipmentSlot slot, GrhIndex graphicIndex)
        {
            if (!TrySetSlot(slot, new ItemEntity(graphicIndex, 1, 0)))
                Debug.Fail("Failed to set the slot for some reason.");
        }
    }
}