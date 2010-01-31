using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// Handles when an item has been added to or removed from a <see cref="DArray{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of item.</typeparam>
    /// <param name="sender">The <see cref="DArray{T}"/> the event came from.</param>
    /// <param name="item">The item the event is related to.</param>
    /// <param name="index">The index the item was added to or removed from in the <see cref="DArray{T}"/>.</param>
    public delegate void DArrayModifyEventHandler<T>(DArray<T> sender, T item, int index);
}