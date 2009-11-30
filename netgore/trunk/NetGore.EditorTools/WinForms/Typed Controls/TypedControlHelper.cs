using System.Linq;
using System.Windows.Forms;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Helper methods for a strongly typed <see cref="Control"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class TypedControlHelper<T>
    {
        /// <summary>
        /// Tries to get the given item as Type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="value">When this method returns true, contains the value as type <typeparamref name="T"/>.</param>
        /// <returns>True if the conversion was successful; otherwise false.</returns>
        public static bool TryGetItemAsTyped(object item, out T value)
        {
            // Check if using the TypedListControlItem container
            if (item is TypedListControlItem<T>)
            {
                value = ((TypedListControlItem<T>)item).Value;
                return true;
            }

            // Check if it is just the actual type we are looking for
            if (item is T)
            {
                value = (T)item;
                return true;
            }

            // Failure :(
            value = default(T);
            return false;
        }
    }
}