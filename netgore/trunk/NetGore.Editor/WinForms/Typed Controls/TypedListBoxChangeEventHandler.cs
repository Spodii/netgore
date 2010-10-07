using System.Linq;

namespace NetGore.Editor.WinForms
{
    /// <summary>
    /// Delegate for handling events from the <see cref="TypedListBox{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of item that the <see cref="TypedListBox{T}"/> handles.</typeparam>
    /// <param name="sender">The <see cref="TypedListBox{T}"/> the event came from.</param>
    /// <param name="item">The item related to the event.</param>
    public delegate void TypedListBoxChangeEventHandler<T>(TypedListBox<T> sender, T item);
}