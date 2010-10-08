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
    public class EnumTable
    {
        /// <summary>
        /// Creates an <see cref="IEnumTable{TKey, TValue}"/> instance.
        /// </summary>
        /// <typeparam name="TKey">The type of key. Must be an enum.</typeparam>
        /// <typeparam name="TValue">The type of value.</typeparam>
        /// <returns>The <see cref="IEnumTable{TKey, TValue}"/> instance.</returns>
        public static IEnumTable<TKey, TValue> Create<TKey, TValue>() where TKey : struct, IComparable, IConvertible, IFormattable
        {
            return new EnumTable<TKey, TValue>();
        }
    }

    /// <summary>
    /// An implementation of the <see cref="IEnumTable{TKey, TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of key. Must be an enum.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    class EnumTable<TKey, TValue> : IEnumTable<TKey, TValue> where TKey : struct, IComparable, IConvertible, IFormattable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The minimum number of elements that must be skipped in an enum for the <see cref="RawTable"/> to not be used.
        /// If the number of skipped elements is less than this value, then the <see cref="RawTable"/> will always be used.
        /// </summary>
        const int _minSkippedElementsToNotUseRawTable = 10;

        /// <summary>
        /// The maximum percentage of used values (usedValues / totalValues) for the <see cref="RawTable"/> to be used.
        /// If the percent is less than this value, AND the number of skipped elements is greater than
        /// <see cref="_minSkippedElementsToNotUseRawTable"/>, then the <see cref="RawTable"/> will not be used.
        /// </summary>
        const float _reqValueUsagePercentForRawTable = 0.3f;

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
        /// Contains what type of table we should use for the <typeparamref name="TKey"/>.
        /// </summary>
        static readonly TableTypes _tableTypeToUse;

        /// <summary>
        /// The enum table being used for this instance.
        /// </summary>
        readonly IEnumTable<TKey, TValue> _table;

        /// <summary>
        /// Initializes the <see cref="EnumTable{TKey,TValue}"/> class.
        /// </summary>
        static EnumTable()
        {
            // Cache the TStatType <-> int conversion func to speed up calls slightly
            _enumToInt = EnumHelper<TKey>.GetToIntFunc();
            Debug.Assert(_enumToInt != null);

            _intToEnum = EnumHelper<TKey>.GetFromIntFunc();
            Debug.Assert(_intToEnum != null);

            // Cache the unique key count since it is a little costly, but a small value to store and we can make use of it later
            _numUniqueKeyValues = EnumHelper<TKey>.Values.Select(ToInt).Distinct().Count();
            Debug.Assert(_numUniqueKeyValues >= 0);

            // Cache what table type to use for the key
            _tableTypeToUse = FindTableTypeToUse();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumTable{TKey, TValue}"/> class.
        /// </summary>
        public EnumTable()
        {
            _table = CreateTable(_tableTypeToUse);
        }

        /// <summary>
        /// Creates the <see cref="IEnumTable{TKey, TValue}"/> from the <see cref="TableTypes"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The table instance.</returns>
        static IEnumTable<TKey, TValue> CreateTable(TableTypes type)
        {
            switch (_tableTypeToUse)
            {
                case TableTypes.Raw:
                    return new RawTable();

                case TableTypes.Dictionary:
                    return new DictionaryTable();

                default:
                    const string errmsg = "Invalid TableTypes value `{0}`.";
                    if (log.IsFatalEnabled)
                        log.FatalFormat(errmsg, type);
                    Debug.Fail(string.Format(errmsg, type));
                    throw new Exception("Encountered an unexpected internal error - EnumTable could not be created.");
            }
        }

        /// <summary>
        /// Finds the type of table to use for the key.
        /// </summary>
        /// <returns>The type of table to use for the key.</returns>
        static TableTypes FindTableTypeToUse()
        {
            var minValue = EnumHelper<TKey>.MinValue;
            var maxValue = EnumHelper<TKey>.MaxValue;
            var valueRange = maxValue - minValue + 1;
            var numSkippedValues = valueRange - _numUniqueKeyValues;

            Debug.Assert(numSkippedValues >= 0);

            // Always use RawTable if the number of skipped values is low
            if (numSkippedValues <= _minSkippedElementsToNotUseRawTable)
                return TableTypes.Raw;

            // If the percent of used values is too low, use a lookup table
            var usedValuesPercent = _numUniqueKeyValues / valueRange;
            if (usedValuesPercent < _reqValueUsagePercentForRawTable)
                return TableTypes.Dictionary;

            // Otherwise, use a raw table
            return TableTypes.Raw;
        }

        /// <summary>
        /// Gets a key from an integer value.
        /// </summary>
        /// <param name="value">The integer value.</param>
        /// <returns>The key.</returns>
        protected static TKey FromInt(int value)
        {
            return _intToEnum(value);
        }

        /// <summary>
        /// Gets the <see cref="Exception"/> for when trying to use an invalid key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="arg">The argument's name.</param>
        /// <returns>The <see cref="Exception"/>.</returns>
        protected static Exception GetInvalidKeyException(TKey key, string arg)
        {
            const string errmsg = "Invalid key `{0}` - the key is not a defined enum value.";
            return new ArgumentOutOfRangeException(arg, string.Format(errmsg, key));
        }

        /// <summary>
        /// Gets an integer value from a key.
        /// </summary>
        /// <param name="value">The key.</param>
        /// <returns>The integer value.</returns>
        protected static int ToInt(TKey value)
        {
            return _enumToInt(value);
        }

        #region IEnumTable<TKey,TValue> Members

        /// <summary>
        /// Gets or sets the value at the given key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value at the given <paramref name="key"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="key"/> is not a valid defined enum value.</exception>
        TValue IEnumTable<TKey, TValue>.this[TKey key]
        {
            get { return _table[key]; }
            set { _table[key] = value; }
        }

        /// <summary>
        /// Gets if the given key is a valid key.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if a valid key; otherwise false. An invalid key is often the result of trying to use an enum value
        /// that is not defined by the enum.</returns>
        public bool IsValidKey(TKey key)
        {
            return _table.IsValidKey(key);
        }

        /// <summary>
        /// Tries to get the value at the given key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns true, contains the value for the given <paramref name="key"/>.</param>
        /// <returns>True if the value was successfully acquired; otherwise false.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return _table.TryGetValue(key, out value);
        }

        /// <summary>
        /// Tries to set the value at the given key.
        /// </summary>
        /// <param name="key">The key of the value to set.</param>
        /// <param name="value">The value to assign at the <paramref name="key"/>.</param>
        /// <returns>True if the value was successfully set; otherwise false.</returns>
        public bool TrySetValue(TKey key, TValue value)
        {
            return _table.TrySetValue(key, value);
        }

        #endregion

        /// <summary>
        /// A table that uses a dictionary instead of an array. Usage of a dictionary results in more memory and processing
        /// overhead, but allows the usage of enums where the enum values vary greatly.
        /// </summary>
        class DictionaryTable : IEnumTable<TKey, TValue>
        {
            readonly IDictionary<int, TValue> _buffer = new Dictionary<int, TValue>();

            /// <summary>
            /// Initializes a new instance of the <see cref="DictionaryTable"/> class.
            /// </summary>
            public DictionaryTable()
            {
                // Add the initial values to the buffer
                var value = default(TValue);
                foreach (var i in EnumHelper<TKey>.Values.Select(GetIndex).Distinct())
                {
                    _buffer.Add(i, value);
                }
            }

            /// <summary>
            /// Gets the index for a key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>The index for the <paramref name="key"/>.</returns>
            static int GetIndex(TKey key)
            {
                return ToInt(key);
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
                        throw GetInvalidKeyException(key, "key");

                    return _buffer[i];
                }
                set
                {
                    var i = GetIndex(key);

                    if (!IsValidKey(key))
                        throw GetInvalidKeyException(key, "key");

                    _buffer[i] = value;
                }
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
        /// A table that converts the enum value directly to an integer and uses that as the array index. This is the
        /// ideal collection and provides the best performance and lowest memory footprint, but suffers greatly when elements
        /// are skipped.
        /// </summary>
        class RawTable : IEnumTable<TKey, TValue>
        {
            static readonly int _indexOffset;

            /// <summary>
            /// A <see cref="BitArray"/> of the valid indicies in the <see cref="_buffer"/>. If null, assume any value in
            /// the range of the <see cref="_buffer"/> is a valid index.
            /// </summary>
            static readonly BitArray _validIndicies;

            readonly TValue[] _buffer;

            /// <summary>
            /// Initializes the <see cref="EnumTable{TKey, TValue}.RawTable"/> class.
            /// </summary>
            static RawTable()
            {
                // Cache the index offset
                _indexOffset = -EnumHelper<TKey>.MinValue;

                // Create a BitArray of the valid indicies that we can use. If no values are skipped, we do not
                // need to create the BitArray.
                var minValue = EnumHelper<TKey>.MinValue;
                var maxValue = EnumHelper<TKey>.MaxValue;
                var valueRange = maxValue - minValue + 1;
                var numSkippedValues = valueRange - _numUniqueKeyValues;

                Debug.Assert(numSkippedValues >= 0);

                if (numSkippedValues > 0)
                {
                    _validIndicies = new BitArray(valueRange);
                    _validIndicies.SetAll(false);

                    foreach (var i in EnumHelper<TKey>.Values.Select(GetIndex))
                    {
                        _validIndicies[i] = true;
                    }
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="EnumTable{TKey, TValue}.RawTable"/> class.
            /// </summary>
            public RawTable()
            {
                var bufferSize = GetCollectionSize();
                _buffer = new TValue[bufferSize];
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
                var i = ToInt(key);
                i += _indexOffset;
                return i;
            }

            /// <summary>
            /// Gets if the given index is a valid key.
            /// </summary>
            /// <param name="index">The index.</param>
            /// <returns>True if the <paramref name="index"/> is a valid key; otherwise false.</returns>
            bool IsValidIndex(int index)
            {
                return index >= 0 && index < _buffer.Length && (_validIndicies == null || _validIndicies[index]);
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
                        throw GetInvalidKeyException(key, "key");

                    return _buffer[i];
                }
                set
                {
                    var i = GetIndex(key);

                    if (!IsValidIndex(i))
                        throw GetInvalidKeyException(key, "key");

                    _buffer[i] = value;
                }
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
        /// The different table types that can be used for the <see cref="IEnumTable{TKey, TValue}"/>.
        /// </summary>
        enum TableTypes : byte
        {
            /// <summary>
            /// Uses <see cref="RawTable"/>.
            /// </summary>
            Raw,

            /// <summary>
            /// Uses <see cref="DictionaryTable"/>.
            /// </summary>
            Dictionary
        }
    }
}