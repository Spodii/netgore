using System;
using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// <see cref="EventArgs"/> for the <see cref="EquippedBase{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of item.</typeparam>
    public class EquippedEventArgs<T> : EventArgs where T : ItemEntityBase
    {
        readonly T _item;
        readonly EquipmentSlot _slot;

        /// <summary>
        /// Initializes a new instance of the <see cref="EquippedEventArgs{T}"/> class.
        /// </summary>
        /// <param name="item">The item the event is related to.</param>
        /// <param name="slot">The slot of the item the event is related to.</param>
        public EquippedEventArgs(T item, EquipmentSlot slot)
        {
            _item = item;
            _slot = slot;
        }

        /// <summary>
        /// Gets the item the event is related to.
        /// </summary>
        public T Item
        {
            get { return _item; }
        }

        /// <summary>
        /// Gets the slot of the item the event is related to.
        /// </summary>
        public EquipmentSlot Slot
        {
            get { return _slot; }
        }
    }
}