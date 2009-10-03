using System.Linq;
using DemoGame;
using NetGore;

namespace DemoGame
{
    public delegate void EquippedEventHandler<T>(EquippedBase<T> equippedBase, T item, EquipmentSlot slot)
        where T : ItemEntityBase;
}