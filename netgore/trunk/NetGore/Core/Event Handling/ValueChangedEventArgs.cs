using System;
using System.Linq;

namespace NetGore
{
    /// <summary>
    /// An immutable implementation of <see cref="EventArgs"/> that contains a previous value and current value.
    /// Typically used for events that handle when a value has changed.
    /// </summary>
    [Serializable]
    public class ValueChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Creates an instance of the <see cref="ValueChangedEventArgs{T}"/> class.
        /// </summary>
        /// <typeparam name="T">The type of the values.</typeparam>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns>An instance of the <see cref="ValueChangedEventArgs{T}"/> class.</returns>
        public static ValueChangedEventArgs<T> Create<T>(T oldValue, T newValue)
        {
            return new ValueChangedEventArgs<T>(oldValue, newValue);
        }
    }

    /// <summary>
    /// An immutable implementation of <see cref="EventArgs"/> that contains a previous value and current value.
    /// Typically used for events that handle when a value has changed.
    /// </summary>
    /// <typeparam name="T">The type of the values.</typeparam>
    [Serializable]
    public class ValueChangedEventArgs<T> : ValueChangedEventArgs
    {
        readonly T _newValue;
        readonly T _oldValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueChangedEventArgs{T}"/> class.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        public ValueChangedEventArgs(T oldValue, T newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        /// <summary>
        /// Gets the new value.
        /// </summary>
        public T NewValue
        {
            get { return _newValue; }
        }

        /// <summary>
        /// Gets the old value.
        /// </summary>
        public T OldValue
        {
            get { return _oldValue; }
        }
    }
}