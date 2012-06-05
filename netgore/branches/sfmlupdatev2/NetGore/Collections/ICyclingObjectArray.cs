using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// Interface for a specialized collection that returns a free index in an internal buffer but, unlike <see cref="DArray{T}"/>, it does
    /// not make any attempt to reuse indices and intentionally makes sure to try and avoid re-assigning an index for as long
    /// as possible. Only works on objects, and null values are always treated as an empty index.
    /// This collection is not thread-safe.
    /// </summary>
    /// <typeparam name="TKey">The type of the key. Will always be a integer value type.</typeparam>
    /// <typeparam name="TValue">The value to store.</typeparam>
    public interface ICyclingObjectArray<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>> where TValue : class
    {
        /// <summary>
        /// Gets or sets the object at a key. If setting and the <paramref name="value"/> is null, the <paramref name="key"/>
        /// will be cleared.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value at the <paramref name="key"/>, or null if empty.</returns>
        TValue this[TKey key] { get; set; }

        /// <summary>
        /// Gets or sets the object at a key. If setting and the <paramref name="value"/> is null, the <paramref name="key"/>
        /// will be cleared.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value at the <paramref name="key"/>, or null if empty.</returns>
        TValue this[int key] { get; set; }

        /// <summary>
        /// Gets the keys that are currently in use.
        /// </summary>
        IEnumerable<TKey> Keys { get; }

        /// <summary>
        /// Gets the value of the maximum supported index value.
        /// </summary>
        int MaxIndex { get; }

        /// <summary>
        /// Gets the value of the minimum supported index value.
        /// </summary>
        int MinIndex { get; }

        /// <summary>
        /// Gets the values in the collection.
        /// </summary>
        IEnumerable<TValue> Values { get; }

        /// <summary>
        /// Adds a value into the collection.
        /// </summary>
        /// <param name="value">The value to add.</param>
        /// <returns>The key at which the <paramref name="value"/> was added.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        TKey Add(TValue value);

        /// <summary>
        /// Gets if an object exists at the given key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>True if an object exists at the given <paramref name="key"/>; otherwise false.</returns>
        bool IsSet(TKey key);

        /// <summary>
        /// Gets if an object exists at the given key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>True if an object exists at the given <paramref name="key"/>; otherwise false.</returns>
        bool IsSet(int key);

        /// <summary>
        /// Gets the next free key and reserves the key for a short period.
        /// It is highly recommended to use <see cref="ICyclingObjectArray{T,U}.Add"/> instead whenever possible.
        /// </summary>
        /// <returns>The next free key.</returns>
        /// <exception cref="InvalidOperationException">No free indices could be found.</exception>
        TKey NextFreeKey();
    }
}