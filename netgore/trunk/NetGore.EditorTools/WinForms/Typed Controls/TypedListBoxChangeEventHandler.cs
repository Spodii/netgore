using System.Linq;

namespace NetGore.EditorTools.WinForms
{
    public delegate void TypedListBoxChangeEventHandler<T>(TypedListBox<T> sender, T item);
}