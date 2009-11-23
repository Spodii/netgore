using System.Linq;

namespace NetGore.EditorTools
{
    public delegate void TypedListBoxChangeEventHandler<T>(TypedListBox<T> sender, T item);
}