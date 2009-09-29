using System;
using System.Linq;

namespace NetGore.RPGComponents
{
    public delegate void EquippedEventHandler<TItem, TItemType, TEquipmentSlot>(
        EquippedBase<TItem, TItemType, TEquipmentSlot> equippedBase, TItem item, TEquipmentSlot slot)
        where TItem : ItemEntityBase<TItemType> where TItemType : struct, IComparable, IConvertible, IFormattable
        where TEquipmentSlot : struct, IComparable, IConvertible, IFormattable;
}