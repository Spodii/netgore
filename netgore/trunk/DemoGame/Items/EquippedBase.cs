using System.Collections.Generic;
using System.Linq;
using DemoGame;
using NetGore;
using NetGore.RPGComponents;

namespace DemoGame
{
    /// <summary>
    /// Base class for keeping track of a collection of equipped items.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="ItemEntityBase"/>.</typeparam>
    public abstract class EquippedBase<T> : EquippedBase<T, ItemType, EquipmentSlot> where T : ItemEntityBase
    {
        static readonly int _maxValue = EquipmentSlotHelper.Instance.MaxValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="EquippedBase&lt;T&gt;"/> class.
        /// </summary>
        protected EquippedBase() : base(_maxValue)
        {
        }

        /// <summary>
        /// When overridden in the derived class, gets the <see cref="EquipmentSlot"/> from an integer.
        /// </summary>
        /// <param name="value">The value to convert to type <see cref="EquipmentSlot"/>.</param>
        /// <returns>
        /// The <paramref name="value"/> as type <see cref="EquipmentSlot"/>.
        /// </returns>
        protected override EquipmentSlot EquipmentSlotFromInt(int value)
        {
            return (EquipmentSlot)value;
        }

        /// <summary>
        /// Gets an IEnumerable of EquipmentSlots possible for a given item.
        /// </summary>
        /// <param name="item">Item to get the possible EquipmentSlots for.</param>
        /// <returns>An IEnumerable of EquipmentSlots possible for the <paramref name="item"/>.</returns>
        protected override IEnumerable<EquipmentSlot> GetPossibleSlots(T item)
        {
            return item.Type.GetPossibleSlots();
        }

        /// <summary>
        /// When overridden in the derived class, gets the integer value of the <see cref="EquipmentSlot"/>.
        /// </summary>
        /// <param name="value">The value to convert to an int.</param>
        /// <returns>The <paramref name="value"/> as an int.</returns>
        protected override int ToInt(EquipmentSlot value)
        {
            return value.GetValue();
        }
    }
}