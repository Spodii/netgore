using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Handles an event from the <see cref="EquippedBase{T}"/>.
    /// </summary>
    /// <param name="equippedBase">The <see cref="EquippedBase{T}"/>.</param>
    /// <param name="item">The item the event is related to.</param>
    /// <param name="slot">The slot of the item the event is related to.</param>
    public delegate void EquippedEventHandler<T>(EquippedBase<T> equippedBase, T item, EquipmentSlot slot) where T : ItemEntityBase;
}