using System.Linq;

namespace NetGore.EditorTools.WinForms
{
    public delegate void TypedComboBoxChangeEventHandler<T>(TypedComboBox<T> sender, T item);
}