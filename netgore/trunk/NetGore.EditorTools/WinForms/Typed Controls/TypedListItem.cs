using System.Linq;

namespace NetGore.EditorTools
{
    /// <summary>
    /// Container for storing an item that has a custom ToString.
    /// </summary>
    /// <typeparam name="T">The type of item.</typeparam>
    public sealed class TypedListControlItem<T>
    {
        readonly ITypedControl<T> _owner;
        readonly T _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypedListControlItem{T}"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="value">The value.</param>
        public TypedListControlItem(ITypedControl<T> owner, T value)
        {
            _owner = owner;
            _value = value;
        }

        /// <summary>
        /// Gets the contained item.
        /// </summary>
        public T Value
        {
            get { return _value; }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return _owner.ItemToString(Value);
        }
    }
}