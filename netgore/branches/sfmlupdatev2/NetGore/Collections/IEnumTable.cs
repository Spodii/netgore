using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// Interface for a class that defines a fixed-size table of values accessed by an enum.
    /// </summary>
    /// <typeparam name="TKey">The type of key. Must be an enum.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public interface IEnumTable<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
        where TKey : struct, IComparable, IConvertible, IFormattable
    {
        /// <summary>
        /// Gets or sets the value at the given key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value at the given <paramref name="key"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="key"/> is not a valid defined enum value.</exception>
        TValue this[TKey key] { get; set; }

        /// <summary>
        /// Sets every index in the table to the default value.
        /// </summary>
        void Clear();

        /// <summary>
        /// Creates a deep copy of this enum table.
        /// </summary>
        /// <returns>A deep copy of this enum table.</returns>
        IEnumTable<TKey, TValue> DeepCopy();

        /// <summary>
        /// Gets if the given key is a valid key.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if a valid key; otherwise false. An invalid key is often the result of trying to use an enum value
        /// that is not defined by the enum.</returns>
        bool IsValidKey(TKey key);

        /// <summary>
        /// Sets every index in the table to the given value.
        /// </summary>
        /// <param name="value">The value to set all indices.</param>
        void SetAll(TValue value);

        /// <summary>
        /// Tries to get the value at the given key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns true, contains the value for the given <paramref name="key"/>.</param>
        /// <returns>True if the value was successfully acquired; otherwise false.</returns>
        bool TryGetValue(TKey key, out TValue value);

        /// <summary>
        /// Tries to set the value at the given key.
        /// </summary>
        /// <param name="key">The key of the value to set.</param>
        /// <param name="value">The value to assign at the <paramref name="key"/>.</param>
        /// <returns>True if the value was successfully set; otherwise false.</returns>
        bool TrySetValue(TKey key, TValue value);
    }
}