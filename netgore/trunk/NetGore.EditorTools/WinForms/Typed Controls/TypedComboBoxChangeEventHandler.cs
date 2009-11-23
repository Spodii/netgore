using System.Linq;

namespace NetGore.EditorTools
{
    public delegate void TypedComboBoxChangeEventHandler<T>(TypedComboBox<T> sender, T item);
}