using System.Linq;
using NetGore;

namespace DemoGame
{
    public delegate void EquippedEventHandler<T>(EquippedBase<T> equippedBase, T item, EquipmentSlot slot)
        where T : ItemEntityBase;
}