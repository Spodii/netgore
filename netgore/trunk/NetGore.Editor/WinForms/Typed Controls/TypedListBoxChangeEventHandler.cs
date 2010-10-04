using System.Linq;

namespace NetGore.Editor.WinForms
{
    public delegate void TypedListBoxChangeEventHandler<T>(TypedListBox<T> sender, T item);
}