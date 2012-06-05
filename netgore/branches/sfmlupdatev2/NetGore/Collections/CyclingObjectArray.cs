using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// A specialized collection that returns a free index in an internal buffer but, unlike <see cref="DArray{T}"/>, it does
    /// not make any attempt to reuse indices and intentionally makes sure to try and avoid re-assigning an index for as long
    /// as possible. Only works on objects, and null values are always treated as an empty index.
    /// This collection is not thread-safe.
    /// </summary>
    public static class CyclingObjectArray
    {
        /// <summary>
        /// Creates an <see cref="ICyclingObjectArray{T,U}"/> instance.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to store.</typeparam>
        /// <returns>The <see cref="ICyclingObjectArray{T,U}"/> instance using key of the corresponding type.</returns>
        public static ICyclingObjectArray<byte, TValue> CreateUsingByteKey<TValue>() where TValue : class
        {
            return new CyclingObjectArrayByte<TValue>();
        }

        /// <summary>
        /// Creates an <see cref="ICyclingObjectArray{T,U}"/> instance.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to store.</typeparam>
        /// <returns>The <see cref="ICyclingObjectArray{T,U}"/> instance using key of the corresponding type.</returns>
        public static ICyclingObjectArray<int, TValue> CreateUsingIntKey<TValue>() where TValue : class
        {
            return new CyclingObjectArrayInt<TValue>();
        }

        /// <summary>
        /// Creates an <see cref="ICyclingObjectArray{T,U}"/> instance.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to store.</typeparam>
        /// <returns>The <see cref="ICyclingObjectArray{T,U}"/> instance using key of the corresponding type.</returns>
        public static ICyclingObjectArray<sbyte, TValue> CreateUsingSByteKey<TValue>() where TValue : class
        {
            return new CyclingObjectArraySByte<TValue>();
        }

        /// <summary>
        /// Creates an <see cref="ICyclingObjectArray{T,U}"/> instance.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to store.</typeparam>
        /// <returns>The <see cref="ICyclingObjectArray{T,U}"/> instance using key of the corresponding type.</returns>
        public static ICyclingObjectArray<short, TValue> CreateUsingShortKey<TValue>() where TValue : class
        {
            return new CyclingObjectArrayShort<TValue>();
        }

        /// <summary>
        /// Creates an <see cref="ICyclingObjectArray{T,U}"/> instance.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to store.</typeparam>
        /// <returns>The <see cref="ICyclingObjectArray{T,U}"/> instance using key of the corresponding type.</returns>
        public static ICyclingObjectArray<ushort, TValue> CreateUsingUShortKey<TValue>() where TValue : class
        {
            return new CyclingObjectArrayUShort<TValue>();
        }

        /// <summary>
        /// <see cref="CyclingObjectArrayInternalBase{T,U}"/> for an byte key.
        /// </summary>
        /// <typeparam name="TValue">The type of value.</typeparam>
        class CyclingObjectArrayByte<TValue> : CyclingObjectArrayInternalBase<byte, TValue> where TValue : class
        {
            /// <summary>
            /// When overridden in the derived class, gets the value of the maximum supported index.
            /// </summary>
            public override int MaxIndex
            {
                get { return byte.MaxValue; }
            }

            /// <summary>
            /// Gets the value of the maximum supported index.
            /// </summary>
            public override int MinIndex
            {
                get { return byte.MinValue; }
            }

            /// <summary>
            /// When overridden in the derived class, converts an int to a key.
            /// </summary>
            /// <param name="i">The byte.</param>
            /// <returns>The key.</returns>
            protected override byte FromInt(int i)
            {
                Debug.Assert(i >= byte.MinValue && i <= byte.MaxValue);
                return (byte)i;
            }

            /// <summary>
            /// When overridden in the derived class, converts the key to an int.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>The byte.</returns>
            protected override int ToInt(byte key)
            {
                return key;
            }
        }

        /// <summary>
        /// <see cref="CyclingObjectArrayInternalBase{T,U}"/> for an int key.
        /// </summary>
        /// <typeparam name="TValue">The type of value.</typeparam>
        class CyclingObjectArrayInt<TValue> : CyclingObjectArrayInternalBase<int, TValue> where TValue : class
        {
            /// <summary>
            /// When overridden in the derived class, gets the value of the maximum supported index.
            /// </summary>
            public override int MaxIndex
            {
                get
                {
                    // Stay 1 away from the max value since we use an int for the index iterator and don't want to overflow
                    return int.MaxValue - 1;
                }
            }

            /// <summary>
            /// Gets the value of the maximum supported index.
            /// </summary>
            public override int MinIndex
            {
                get
                {
                    // Stay 1 away from the min value since we use an int for the index iterator and don't want to underflow
                    return int.MinValue + 1;
                }
            }

            /// <summary>
            /// When overridden in the derived class, converts an int to a key.
            /// </summary>
            /// <param name="i">The int.</param>
            /// <returns>The key.</returns>
            protected override int FromInt(int i)
            {
                return i;
            }

            /// <summary>
            /// When overridden in the derived class, converts the key to an int.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>The int.</returns>
            protected override int ToInt(int key)
            {
                return key;
            }
        }

        /// <summary>
        /// Base class for an implementation of the <see cref="ICyclingObjectArray{TKey, TValue}"/>.
        /// </summary>
        /// <typeparam name="TKey">The type of the key. Will always be a integer value type.</typeparam>
        /// <typeparam name="TValue">The value to store.</typeparam>
        abstract class CyclingObjectArrayInternalBase<TKey, TValue> : ICyclingObjectArray<TKey, TValue> where TValue : class
        {
            readonly IDictionary<TKey, TValue> _d = new Dictionary<TKey, TValue>();

            int _currIndex = 0;

            /// <summary>
            /// When overridden in the derived class, converts an int to a key.
            /// </summary>
            /// <param name="i">The int.</param>
            /// <returns>The key.</returns>
            protected abstract TKey FromInt(int i);

            /// <summary>
            /// When overridden in the derived class, converts the key to an int.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>The int.</returns>
            protected abstract int ToInt(TKey key);

            #region ICyclingObjectArray<TKey,TValue> Members

            /// <summary>
            /// Gets or sets the object at a key. If setting and the <paramref name="value"/> is null, the <paramref name="key"/>
            /// will be cleared.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>The value at the <paramref name="key"/>, or null if empty.</returns>
            public TValue this[TKey key]
            {
                get
                {
                    TValue v;
                    if (_d.TryGetValue(key, out v))
                        return v;

                    return default(TValue);
                }
                set
                {
                    if (value == null)
                    {
                        _d.Remove(key);
                        return;
                    }

                    if (_d.ContainsKey(key))
                        _d[key] = value;
                    else
                        _d.Add(key, value);
                }
            }

            /// <summary>
            /// Gets or sets the object at a key. If setting and the <paramref name="value"/> is null, the <paramref name="key"/>
            /// will be cleared.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>The value at the <paramref name="key"/>, or null if empty.</returns>
            TValue ICyclingObjectArray<TKey, TValue>.this[int key]
            {
                get { return this[FromInt(key)]; }
                set { this[FromInt(key)] = value; }
            }

            /// <summary>
            /// Gets the keys that are currently in use.
            /// </summary>
            public IEnumerable<TKey> Keys
            {
                get { return _d.Keys; }
            }

            /// <summary>
            /// Gets the value of the maximum supported index value.
            /// </summary>
            public abstract int MaxIndex { get; }

            /// <summary>
            /// Gets the value of the minimum supported index value.
            /// </summary>
            public abstract int MinIndex { get; }

            /// <summary>
            /// Gets the values in the collection.
            /// </summary>
            public IEnumerable<TValue> Values
            {
                get { return _d.Values; }
            }

            /// <summary>
            /// Adds a value into the collection.
            /// </summary>
            /// <param name="value">The value to add.</param>
            /// <returns>The key at which the <paramref name="value"/> was added.</returns>
            /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
            public TKey Add(TValue value)
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                var key = NextFreeKey();
                this[key] = value;
                return key;
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
            /// </returns>
            /// <filterpriority>1</filterpriority>
            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            {
                return _d.GetEnumerator();
            }

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
            /// </returns>
            /// <filterpriority>2</filterpriority>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            /// <summary>
            /// Gets if an object exists at the given key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>True if an object exists at the given <paramref name="key"/>; otherwise false.</returns>
            public bool IsSet(TKey key)
            {
                return _d.ContainsKey(key);
            }

            /// <summary>
            /// Gets if an object exists at the given key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>True if an object exists at the given <paramref name="key"/>; otherwise false.</returns>
            public bool IsSet(int key)
            {
                return IsSet(FromInt(key));
            }

            /// <summary>
            /// Gets the next free key and reserves the key for a short period.
            /// It is highly recommended to use <see cref="ICyclingObjectArray{T,U}.Add"/> instead whenever possible.
            /// </summary>
            /// <returns>The next free key.</returns>
            /// <exception cref="InvalidOperationException">No free indices could be found.</exception>
            public TKey NextFreeKey()
            {
                var startIndex = _currIndex;

                while (++_currIndex != startIndex)
                {
                    // Roll over
                    if (_currIndex > MaxIndex)
                        _currIndex = MinIndex;

                    // Check if in use
                    var key = FromInt(_currIndex);
                    if (!IsSet(key))
                        return key;
                }

                throw new InvalidOperationException("No free indices could be found - the collection is full.");
            }

            #endregion
        }

        /// <summary>
        /// <see cref="CyclingObjectArrayInternalBase{T,U}"/> for an sbyte key.
        /// </summary>
        /// <typeparam name="TValue">The type of value.</typeparam>
        class CyclingObjectArraySByte<TValue> : CyclingObjectArrayInternalBase<sbyte, TValue> where TValue : class
        {
            /// <summary>
            /// When overridden in the derived class, gets the value of the maximum supported index.
            /// </summary>
            public override int MaxIndex
            {
                get { return sbyte.MaxValue; }
            }

            /// <summary>
            /// Gets the value of the maximum supported index.
            /// </summary>
            public override int MinIndex
            {
                get { return sbyte.MinValue; }
            }

            /// <summary>
            /// When overridden in the derived class, converts an int to a key.
            /// </summary>
            /// <param name="i">The sbyte.</param>
            /// <returns>The key.</returns>
            protected override sbyte FromInt(int i)
            {
                Debug.Assert(i >= sbyte.MinValue && i <= sbyte.MaxValue);
                return (sbyte)i;
            }

            /// <summary>
            /// When overridden in the derived class, converts the key to an int.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>The sbyte.</returns>
            protected override int ToInt(sbyte key)
            {
                return key;
            }
        }

        /// <summary>
        /// <see cref="CyclingObjectArrayInternalBase{T,U}"/> for an short key.
        /// </summary>
        /// <typeparam name="TValue">The type of value.</typeparam>
        class CyclingObjectArrayShort<TValue> : CyclingObjectArrayInternalBase<short, TValue> where TValue : class
        {
            /// <summary>
            /// When overridden in the derived class, gets the value of the maximum supported index.
            /// </summary>
            public override int MaxIndex
            {
                get { return short.MaxValue; }
            }

            /// <summary>
            /// Gets the value of the maximum supported index.
            /// </summary>
            public override int MinIndex
            {
                get { return short.MinValue; }
            }

            /// <summary>
            /// When overridden in the derived class, converts an int to a key.
            /// </summary>
            /// <param name="i">The short.</param>
            /// <returns>The key.</returns>
            protected override short FromInt(int i)
            {
                Debug.Assert(i >= short.MinValue && i <= short.MaxValue);
                return (short)i;
            }

            /// <summary>
            /// When overridden in the derived class, converts the key to an int.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>The short.</returns>
            protected override int ToInt(short key)
            {
                return key;
            }
        }

        /// <summary>
        /// <see cref="CyclingObjectArrayInternalBase{T,U}"/> for an ushort key.
        /// </summary>
        /// <typeparam name="TValue">The type of value.</typeparam>
        class CyclingObjectArrayUShort<TValue> : CyclingObjectArrayInternalBase<ushort, TValue> where TValue : class
        {
            /// <summary>
            /// When overridden in the derived class, gets the value of the maximum supported index.
            /// </summary>
            public override int MaxIndex
            {
                get { return ushort.MaxValue; }
            }

            /// <summary>
            /// Gets the value of the maximum supported index.
            /// </summary>
            public override int MinIndex
            {
                get { return ushort.MinValue; }
            }

            /// <summary>
            /// When overridden in the derived class, converts an int to a key.
            /// </summary>
            /// <param name="i">The ushort.</param>
            /// <returns>The key.</returns>
            protected override ushort FromInt(int i)
            {
                Debug.Assert(i >= ushort.MinValue && i <= ushort.MaxValue);
                return (ushort)i;
            }

            /// <summary>
            /// When overridden in the derived class, converts the key to an int.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>The ushort.</returns>
            protected override int ToInt(ushort key)
            {
                return key;
            }
        }
    }
}