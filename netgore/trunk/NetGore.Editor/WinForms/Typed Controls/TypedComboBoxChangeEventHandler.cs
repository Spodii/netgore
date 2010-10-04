using System.Linq;

namespace NetGore.Editor.WinForms
{
    public delegate void TypedComboBoxChangeEventHandler<T>(TypedComboBox<T> sender, T item);
}