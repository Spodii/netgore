using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore.Collections
{
    /// <summary>
    /// An implementation of the <see cref="IEnumTable{TKey, TValue}"/>.
    /// </summary>
    public static class EnumTable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The minimum number of elements that must be skipped in an enum for the <see cref="RawTable{T,U}"/> to not be used.
        /// If the number of skipped elements is less than this value, then the <see cref="RawTable{T,U}"/> will always be used.
        /// </summary>
        const int _minSkippedElementsToNotUseRawTable = 10;

        /// <summary>
        /// The maximum percentage of used values (usedValues / totalValues) for the <see cref="RawTable{T,U}"/> to be used.
        /// If the percent is less than this value, AND the number of skipped elements is greater than
        /// <see cref="_minSkippedElementsToNotUseRawTable"/>, then the <see cref="RawTable{T,U}"/> will not be used.
        /// </summary>
        const float _reqValueUsagePercentForRawTable = 0.3f;

        /// <summary>
        /// Creates an <see cref="IEnumTable{TKey, TValue}"/> instance.
        /// </summary>
        /// <typeparam name="TKey">The type of key. Must be an enum.</typeparam>
        /// <typeparam name="TValue">The type of value.</typeparam>
        /// <returns>The <see cref="IEnumTable{TKey, TValue}"/> instance.</returns>
        public static IEnumTable<TKey, TValue> Create<TKey, TValue>() where TKey : struct, IComparable, IConvertible, IFormattable
        {
            var tableType = KeyInfo<TKey>.TableTypeToUse;
            return (IEnumTable<TKey, TValue>)Create<TKey, TValue>(tableType);
        }

        /// <summary>
        /// Creates the <see cref="IEnumTable{TKey, TValue}"/> from the <see cref="TableType"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The table instance.</returns>
        /// <exception cref="ArgumentException">type</exception>
        static object Create<TKey, TValue>(TableType type) where TKey : struct, IComparable, IConvertible, IFormattable
        {
            switch (type)
            {
                case TableType.Raw:
                    if (typeof(TValue) == typeof(bool))
                        return new RawTableBool<TKey>();
                    else
                        return new RawTable<TKey, TValue>();

                case TableType.Dictionary:
                    return new DictionaryTable<TKey, TValue>();

                default:
                    const string errmsg = "Invalid TableTypes value `{0}`.";
                    if (log.IsFatalEnabled)
                        log.FatalFormat(errmsg, type);
                    Debug.Fail(string.Format(errmsg, type));
                    throw new ArgumentException(string.Format(errmsg, type), "type");
            }
        }

        /// <summary>
        /// A table that uses a dictionary instead of an array. Usage of a dictionary results in more memory and processing
        /// overhead, but allows the usage of enums where the enum values vary greatly.
        /// </summary>
        sealed class DictionaryTable<TKey, TValue> : IEnumTable<TKey, TValue>
            where TKey : struct, IComparable, IConvertible, IFormattable
        {
            readonly IDictionary<int, TValue> _buffer;

            /// <summary>
            /// Initializes a new instance of the <see cref="DictionaryTable{TKey, TValue}"/> class.
            /// </summary>
            public DictionaryTable()
            {
                _buffer = new Dictionary<int, TValue>();

                // Add the initial values to the buffer
                var value = default(TValue);
                foreach (var i in EnumHelper<TKey>.Values.Select(GetIndex).Distinct())
                {
                    _buffer.Add(i, value);
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="DictionaryTable{TKey, TValue}"/> class.
            /// </summary>
            /// <param name="other">The table to copy from.</param>
            DictionaryTable(DictionaryTable<TKey, TValue> other)
            {
                _buffer = new Dictionary<int, TValue>(other._buffer);
            }

            /// <summary>
            /// Gets the index for a key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>The index for the <paramref name="key"/>.</returns>
            static int GetIndex(TKey key)
            {
                return KeyInfo<TKey>.ToInt(key);
            }

            /// <summary>
            /// Gets if the given index is a valid key.
            /// </summary>
            /// <param name="index">The index.</param>
            /// <returns>True if the <paramref name="index"/> is a valid key; otherwise false.</returns>
            bool IsValidIndex(int index)
            {
                return _buffer.ContainsKey(index);
            }

            #region IEnumTable<TKey,TValue> Members

            /// <summary>
            /// Gets or sets the value at the given key.
            /// </summary>
            /// <param name="key">The key of the value to get or set.</param>
            /// <returns>The value at the given <paramref name="key"/>.</returns>
            /// <exception cref="ArgumentOutOfRangeException">The <paramref name="key"/> is not a valid defined enum value.</exception>
            public TValue this[TKey key]
            {
                get
                {
                    var i = GetIndex(key);

                    if (!IsValidKey(key))
                        throw KeyInfo<TKey>.GetInvalidKeyException(key, "key");

                    return _buffer[i];
                }
                set
                {
                    var i = GetIndex(key);

                    if (!IsValidKey(key))
                        throw KeyInfo<TKey>.GetInvalidKeyException(key, "key");

                    _buffer[i] = value;
                }
            }

            /// <summary>
            /// Sets every index in the table to the default value.
            /// </summary>
            public void Clear()
            {
                SetAll(default(TValue));
            }

            /// <summary>
            /// Creates a deep copy of this enum table.
            /// </summary>
            /// <returns>A deep copy of this enum table.</returns>
            public IEnumTable<TKey, TValue> DeepCopy()
            {
                return new DictionaryTable<TKey, TValue>(this);
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
            /// </returns>
            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            {
                var values = _buffer.Select(x => new KeyValuePair<TKey, TValue>(KeyInfo<TKey>.FromInt(x.Key), x.Value));
                foreach (var v in values)
                {
                    yield return v;
                }
            }

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
            /// </returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            /// <summary>
            /// Gets if the given key is a valid key.
            /// </summary>
            /// <param name="key">The key to check.</param>
            /// <returns>True if a valid key; otherwise false. An invalid key is often the result of trying to use an enum value
            /// that is not defined by the enum.</returns>
            public bool IsValidKey(TKey key)
            {
                var i = GetIndex(key);
                return IsValidIndex(i);
            }

            /// <summary>
            /// Sets every index in the table to the given value.
            /// </summary>
            /// <param name="value">The value to set all indices.</param>
            public void SetAll(TValue value)
            {
                var keys = EnumHelper<TKey>.Values.Select(GetIndex);
                foreach (var key in keys)
                {
                    _buffer[key] = value;
                }
            }

            /// <summary>
            /// Tries to get the value at the given key.
            /// </summary>
            /// <param name="key">The key of the value to get.</param>
            /// <param name="value">When this method returns true, contains the value for the given <paramref name="key"/>.</param>
            /// <returns>True if the value was successfully acquired; otherwise false.</returns>
            public bool TryGetValue(TKey key, out TValue value)
            {
                var i = GetIndex(key);
                return _buffer.TryGetValue(i, out value);
            }

            /// <summary>
            /// Tries to set the value at the given key.
            /// </summary>
            /// <param name="key">The key of the value to set.</param>
            /// <param name="value">The value to assign at the <paramref name="key"/>.</param>
            /// <returns>True if the value was successfully set; otherwise false.</returns>
            public bool TrySetValue(TKey key, TValue value)
            {
                var i = GetIndex(key);

                if (!IsValidKey(key))
                    return false;

                _buffer[i] = value;

                return true;
            }

            #endregion
        }

        /// <summary>
        /// Provides static information for the keys to be used on an <see cref="EnumTable"/>. This is provided as a generic
        /// class with just the key so that way the information for each key is created once and only once, instead of once for
        /// each key/value pair. This also makes it easier to share info across all implementations.
        /// </summary>
        /// <typeparam name="TKey">The type of key.</typeparam>
        static class KeyInfo<TKey> where TKey : struct, IComparable, IConvertible, IFormattable
        {
            /// <summary>
            /// Converts <typeparamref name="TKey"/> to int.
            /// </summary>
            static readonly Func<TKey, int> _enumToInt;

            /// <summary>
            /// Converts an int to a <typeparamref name="TKey"/>.
            /// </summary>
            static readonly Func<int, TKey> _intToEnum;

            /// <summary>
            /// The number of unique, defined values in the <typeparamref name="TKey"/>.
            /// </summary>
            static readonly int _numUniqueKeyValues;

            /// <summary>
            /// The index offset for the RawTable.
            /// </summary>
            static readonly int _rawTableIndexOffset;

            /// <summary>
            /// Contains what type of table we should use for the <typeparamref name="TKey"/>.
            /// </summary>
            static readonly TableType _tableTypeToUse;

            /// <summary>
            /// A <see cref="BitArray"/> of the valid indicies in the RawTable._buffer. If null, assume any value in
            /// the range of the RawTable._buffer is a valid index.
            /// </summary>
            static BitArray _rawTableBitArray;

            /// <summary>
            /// If the <see cref="_rawTableBitArray"/> has been created.
            /// </summary>
            static bool _rawTableBitArrayCreated = false;

            /// <summary>
            /// Initializes the <see cref="KeyInfo{TKey}"/> class.
            /// </summary>
            static KeyInfo()
            {
                // Cache the TStatType <-> int conversion func to speed up calls slightly
                _enumToInt = EnumHelper<TKey>.GetToIntFunc();
                Debug.Assert(_enumToInt != null);

                _intToEnum = EnumHelper<TKey>.GetFromIntFunc();
                Debug.Assert(_intToEnum != null);

                // Cache the unique key count since it is a little costly, but a small value to store and we can make use of it later
                _numUniqueKeyValues = EnumHelper<TKey>.Values.Select(ToInt).Distinct().Count();
                Debug.Assert(_numUniqueKeyValues >= 0);

                // Cache the RawTable index offset
                _rawTableIndexOffset = -EnumHelper<TKey>.MinValue;

                // Cache what table should be used for this key type
                _tableTypeToUse = FindTableTypeToUse();
            }

            /// <summary>
            /// Gets the number of unique, defined values in the <typeparamref name="TKey"/>.
            /// </summary>
            static int NumUniqueKeyValues
            {
                get { return _numUniqueKeyValues; }
            }

            /// <summary>
            /// Gets the <see cref="EnumTable.TableType"/> to use for this key.
            /// </summary>
            public static TableType TableTypeToUse
            {
                get { return _tableTypeToUse; }
            }

            /// <summary>
            /// Creates the RawTable <see cref="BitArray"/>.
            /// </summary>
            /// <returns>The RawTable <see cref="BitArray"/>.</returns>
            static BitArray CreateRawTableBitArray()
            {
                BitArray ret = null;

                // Create a BitArray of the valid indicies that we can use. If no values are skipped, we do not
                // need to create the BitArray.
                var minValue = EnumHelper<TKey>.MinValue;
                var maxValue = EnumHelper<TKey>.MaxValue;
                var valueRange = maxValue - minValue + 1;
                var numSkippedValues = valueRange - NumUniqueKeyValues;

                Debug.Assert(numSkippedValues >= 0);

                if (numSkippedValues > 0)
                {
                    ret = new BitArray(valueRange, false);

                    foreach (var i in EnumHelper<TKey>.Values.Select(RawTableGetIndex))
                    {
                        ret[i] = true;
                    }
                }

                return ret;
            }

            /// <summary>
            /// Finds the type of table to use for the key.
            /// </summary>
            /// <returns>The type of table to use for the key.</returns>
            static TableType FindTableTypeToUse()
            {
                var minValue = EnumHelper<TKey>.MinValue;
                var maxValue = EnumHelper<TKey>.MaxValue;
                var valueRange = maxValue - minValue + 1;
                var numSkippedValues = valueRange - NumUniqueKeyValues;

                Debug.Assert(numSkippedValues >= 0);

                // Always use RawTable if the number of skipped values is low
                if (numSkippedValues <= _minSkippedElementsToNotUseRawTable)
                    return TableType.Raw;

                // If the percent of used values is too low, use a lookup table
                var usedValuesPercent = NumUniqueKeyValues / valueRange;
                if (usedValuesPercent < _reqValueUsagePercentForRawTable)
                    return TableType.Dictionary;

                // Otherwise, use a raw table
                return TableType.Raw;
            }

            /// <summary>
            /// Gets a key from an integer value.
            /// </summary>
            /// <param name="value">The integer value.</param>
            /// <returns>The key.</returns>
            public static TKey FromInt(int value)
            {
                return _intToEnum(value);
            }

            /// <summary>
            /// Gets the <see cref="ArgumentOutOfRangeException"/> for when trying to use an invalid key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="arg">The argument's name.</param>
            /// <returns>The <see cref="ArgumentOutOfRangeException"/>.</returns>
            public static ArgumentOutOfRangeException GetInvalidKeyException(TKey key, string arg)
            {
                const string errmsg = "Invalid key `{0}` - the key is not a defined enum value.";
                return new ArgumentOutOfRangeException(arg, string.Format(errmsg, key));
            }

            /// <summary>
            /// Gets a <see cref="BitArray"/> of the valid indicies in the RawTable._buffer. If null, assume any value in
            /// the range of the RawTable._buffer is a valid index.
            /// </summary>
            /// <returns>The <see cref="BitArray"/>.</returns>
            public static BitArray GetRawTableBitArray()
            {
                // We create this on-demand instead of at the constructor so that way we don't end up creating it
                // for keys that do not use it

                if (!_rawTableBitArrayCreated)
                {
                    _rawTableBitArray = CreateRawTableBitArray();
                    _rawTableBitArrayCreated = true;
                }

                return _rawTableBitArray;
            }

            /// <summary>
            /// Gets the index for a key in a RawTable.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>The index for the <paramref name="key"/>.</returns>
            public static int RawTableGetIndex(TKey key)
            {
                var i = ToInt(key);
                i += _rawTableIndexOffset;
                return i;
            }

            /// <summary>
            /// Gets the key for an index in a RawTable.
            /// </summary>
            /// <param name="index">The index.</param>
            /// <returns>The key for the <paramref name="index"/>.</returns>
            public static TKey RawTableGetKey(int index)
            {
                index -= _rawTableIndexOffset;
                var key = FromInt(index);
                return key;
            }

            /// <summary>
            /// Gets an integer value from a key.
            /// </summary>
            /// <param name="value">The key.</param>
            /// <returns>The integer value.</returns>
            public static int ToInt(TKey value)
            {
                return _enumToInt(value);
            }
        }

        /// <summary>
        /// A table that converts the enum value directly to an integer and uses that as the array index. This is the
        /// ideal collection and provides the best performance and lowest memory footprint, but suffers greatly when elements
        /// are skipped.
        /// </summary>
        sealed class RawTable<TKey, TValue> : IEnumTable<TKey, TValue>
            where TKey : struct, IComparable, IConvertible, IFormattable
        {
            static readonly BitArray _validIndices = KeyInfo<TKey>.GetRawTableBitArray();

            readonly TValue[] _buffer;

            /// <summary>
            /// Initializes a new instance of the <see cref="RawTable{TKey, TValue}"/> class.
            /// </summary>
            public RawTable()
            {
                var bufferSize = GetCollectionSize();
                _buffer = new TValue[bufferSize];
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="RawTable{TKey, TValue}"/> class.
            /// </summary>
            /// <param name="other">The table to copy from.</param>
            RawTable(RawTable<TKey, TValue> other)
            {
                var bufferSize = GetCollectionSize();
                _buffer = new TValue[bufferSize];

                other._buffer.CopyTo(_buffer, 0);
            }

            /// <summary>
            /// Gets the size to make the collection.
            /// </summary>
            /// <returns>The size to make the collection.</returns>
            static int GetCollectionSize()
            {
                return (EnumHelper<TKey>.MaxValue - EnumHelper<TKey>.MinValue) + 1;
            }

            /// <summary>
            /// Gets the index for a key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>The index for the <paramref name="key"/>.</returns>
            static int GetIndex(TKey key)
            {
                return KeyInfo<TKey>.RawTableGetIndex(key);
            }

            /// <summary>
            /// Gets the key for an index.
            /// </summary>
            /// <param name="index">The index.</param>
            /// <returns>The key for the <paramref name="index"/>.</returns>
            static TKey GetKey(int index)
            {
                return KeyInfo<TKey>.RawTableGetKey(index);
            }

            /// <summary>
            /// Gets if the given index is a valid key.
            /// </summary>
            /// <param name="index">The index.</param>
            /// <returns>True if the <paramref name="index"/> is a valid key; otherwise false.</returns>
            bool IsValidIndex(int index)
            {
                return index >= 0 && index < _buffer.Length && (_validIndices == null || _validIndices[index]);
            }

            #region IEnumTable<TKey,TValue> Members

            /// <summary>
            /// Gets or sets the value at the given key.
            /// </summary>
            /// <param name="key">The key of the value to get or set.</param>
            /// <returns>The value at the given <paramref name="key"/>.</returns>
            /// <exception cref="ArgumentOutOfRangeException">The <paramref name="key"/> is not a valid defined enum value.</exception>
            public TValue this[TKey key]
            {
                get
                {
                    var i = GetIndex(key);

                    if (!IsValidIndex(i))
                        throw KeyInfo<TKey>.GetInvalidKeyException(key, "key");

                    return _buffer[i];
                }
                set
                {
                    var i = GetIndex(key);

                    if (!IsValidIndex(i))
                        throw KeyInfo<TKey>.GetInvalidKeyException(key, "key");

                    _buffer[i] = value;
                }
            }

            /// <summary>
            /// Sets every index in the table to the default value.
            /// </summary>
            public void Clear()
            {
                SetAll(default(TValue));
            }

            /// <summary>
            /// Creates a deep copy of this enum table.
            /// </summary>
            /// <returns>A deep copy of this enum table.</returns>
            public IEnumTable<TKey, TValue> DeepCopy()
            {
                return new RawTable<TKey, TValue>(this);
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
            /// </returns>
            public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            {
                for (var i = 0; i < _buffer.Length; i++)
                {
                    if (_validIndices == null || _validIndices[i])
                    {
                        var key = GetKey(i);
                        var value = _buffer[i];
                        var v = new KeyValuePair<TKey, TValue>(key, value);
                        yield return v;
                    }
                }
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
            /// Gets if the given key is a valid key.
            /// </summary>
            /// <param name="key">The key to check.</param>
            /// <returns>True if a valid key; otherwise false. An invalid key is often the result of trying to use an enum value
            /// that is not defined by the enum.</returns>
            public bool IsValidKey(TKey key)
            {
                var i = GetIndex(key);
                return IsValidIndex(i);
            }

            /// <summary>
            /// Sets every index in the table to the given value.
            /// </summary>
            /// <param name="value">The value to set all indices.</param>
            public void SetAll(TValue value)
            {
                for (var i = 0; i < _buffer.Length; i++)
                {
                    if (_validIndices == null || _validIndices[i])
                        _buffer[i] = value;
                }
            }

            /// <summary>
            /// Tries to get the value at the given key.
            /// </summary>
            /// <param name="key">The key of the value to get.</param>
            /// <param name="value">When this method returns true, contains the value for the given <paramref name="key"/>.</param>
            /// <returns>True if the value was successfully acquired; otherwise false.</returns>
            public bool TryGetValue(TKey key, out TValue value)
            {
                var i = GetIndex(key);

                if (!IsValidIndex(i))
                {
                    value = default(TValue);
                    return false;
                }

                value = _buffer[i];
                return true;
            }

            /// <summary>
            /// Tries to set the value at the given key.
            /// </summary>
            /// <param name="key">The key of the value to set.</param>
            /// <param name="value">The value to assign at the <paramref name="key"/>.</param>
            /// <returns>True if the value was successfully set; otherwise false.</returns>
            public bool TrySetValue(TKey key, TValue value)
            {
                var i = GetIndex(key);

                if (!IsValidIndex(i))
                    return false;

                _buffer[i] = value;
                return true;
            }

            #endregion
        }

        /// <summary>
        /// A specialized version of the <see cref="RawTable{T,U}"/> that uses a <see cref="BitArray"/> to store boolean values.
        /// </summary>
        /// <typeparam name="TKey">The type of key. Must be an enum.</typeparam>
        sealed class RawTableBool<TKey> : IEnumTable<TKey, bool> where TKey : struct, IComparable, IConvertible, IFormattable
        {
            static readonly BitArray _validIndices = KeyInfo<TKey>.GetRawTableBitArray();

            readonly BitArray _buffer;

            /// <summary>
            /// Initializes a new instance of the <see cref="RawTableBool{TKey}"/> class.
            /// </summary>
            public RawTableBool()
            {
                var bufferSize = GetCollectionSize();
                _buffer = new BitArray(bufferSize, false);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="RawTableBool{TKey}"/> class.
            /// </summary>
            /// <param name="other">The table to copy from.</param>
            RawTableBool(RawTableBool<TKey> other)
            {
                _buffer = new BitArray(other._buffer);
            }

            /// <summary>
            /// Gets the size to make the collection.
            /// </summary>
            /// <returns>The size to make the collection.</returns>
            static int GetCollectionSize()
            {
                return (EnumHelper<TKey>.MaxValue - EnumHelper<TKey>.MinValue) + 1;
            }

            /// <summary>
            /// Gets the index for a key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>The index for the <paramref name="key"/>.</returns>
            static int GetIndex(TKey key)
            {
                return KeyInfo<TKey>.RawTableGetIndex(key);
            }

            /// <summary>
            /// Gets the key for an index.
            /// </summary>
            /// <param name="index">The index.</param>
            /// <returns>The key for the <paramref name="index"/>.</returns>
            static TKey GetKey(int index)
            {
                return KeyInfo<TKey>.RawTableGetKey(index);
            }

            /// <summary>
            /// Gets if the given index is a valid key.
            /// </summary>
            /// <param name="index">The index.</param>
            /// <returns>True if the <paramref name="index"/> is a valid key; otherwise false.</returns>
            bool IsValidIndex(int index)
            {
                return index >= 0 && index < _buffer.Length && (_validIndices == null || _validIndices[index]);
            }

            #region IEnumTable<TKey,bool> Members

            /// <summary>
            /// Gets or sets the value at the given key.
            /// </summary>
            /// <param name="key">The key of the value to get or set.</param>
            /// <returns>The value at the given <paramref name="key"/>.</returns>
            /// <exception cref="ArgumentOutOfRangeException">The <paramref name="key"/> is not a valid defined enum value.</exception>
            public bool this[TKey key]
            {
                get
                {
                    var i = GetIndex(key);

                    if (!IsValidIndex(i))
                        throw KeyInfo<TKey>.GetInvalidKeyException(key, "key");

                    return _buffer[i];
                }
                set
                {
                    var i = GetIndex(key);

                    if (!IsValidIndex(i))
                        throw KeyInfo<TKey>.GetInvalidKeyException(key, "key");

                    _buffer[i] = value;
                }
            }

            /// <summary>
            /// Sets every index in the table to the default value.
            /// </summary>
            public void Clear()
            {
                SetAll(default(bool));
            }

            /// <summary>
            /// Creates a deep copy of this enum table.
            /// </summary>
            /// <returns>A deep copy of this enum table.</returns>
            public IEnumTable<TKey, bool> DeepCopy()
            {
                return new RawTableBool<TKey>(this);
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
            /// </returns>
            /// <filterpriority>1</filterpriority>
            public IEnumerator<KeyValuePair<TKey, bool>> GetEnumerator()
            {
                for (var i = 0; i < _buffer.Length; i++)
                {
                    if (_validIndices == null || _validIndices[i])
                    {
                        var key = GetKey(i);
                        var value = _buffer[i];
                        var v = new KeyValuePair<TKey, bool>(key, value);
                        yield return v;
                    }
                }
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
            /// Gets if the given key is a valid key.
            /// </summary>
            /// <param name="key">The key to check.</param>
            /// <returns>True if a valid key; otherwise false. An invalid key is often the result of trying to use an enum value
            /// that is not defined by the enum.</returns>
            public bool IsValidKey(TKey key)
            {
                var i = GetIndex(key);
                return IsValidIndex(i);
            }

            /// <summary>
            /// Sets every index in the table to the given value.
            /// </summary>
            /// <param name="value">The value to set all indices.</param>
            public void SetAll(bool value)
            {
                _buffer.SetAll(value);
            }

            /// <summary>
            /// Tries to get the value at the given key.
            /// </summary>
            /// <param name="key">The key of the value to get.</param>
            /// <param name="value">When this method returns true, contains the value for the given <paramref name="key"/>.</param>
            /// <returns>True if the value was successfully acquired; otherwise false.</returns>
            public bool TryGetValue(TKey key, out bool value)
            {
                var i = GetIndex(key);

                if (!IsValidIndex(i))
                {
                    value = default(bool);
                    return false;
                }

                value = _buffer[i];
                return true;
            }

            /// <summary>
            /// Tries to set the value at the given key.
            /// </summary>
            /// <param name="key">The key of the value to set.</param>
            /// <param name="value">The value to assign at the <paramref name="key"/>.</param>
            /// <returns>True if the value was successfully set; otherwise false.</returns>
            public bool TrySetValue(TKey key, bool value)
            {
                var i = GetIndex(key);

                if (!IsValidIndex(i))
                    return false;

                _buffer[i] = value;
                return true;
            }

            #endregion
        }

        /// <summary>
        /// The different table types that can be used for the <see cref="IEnumTable{TKey, TValue}"/>.
        /// </summary>
        enum TableType : byte
        {
            /// <summary>
            /// Uses <see cref="RawTable{TKey, TValue}"/>.
            /// </summary>
            Raw,

            /// <summary>
            /// Uses <see cref="DictionaryTable{TKey, TValue}"/>.
            /// </summary>
            Dictionary
        }
    }
}