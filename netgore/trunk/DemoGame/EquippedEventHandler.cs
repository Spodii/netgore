using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Platyform.Extensions;

namespace DemoGame
{
    public delegate void EquippedEventHandler<T>(EquippedBase<T> equippedBase, T item, EquipmentSlot slot)
        where T : ItemEntityBase;
}