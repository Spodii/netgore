using System.Linq;
using System.Windows.Forms;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Interface for a strongly typed <see cref="Control"/>.
    /// </summary>
    /// <typeparam name="T">The Type of collection item.</typeparam>
    public interface ITypedControl<T>
    {
        /// <summary>
        /// Gets the string to display for an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The string to display.</returns>
        string ItemToString(T item);
    }
}