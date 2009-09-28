using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// Base class for a class that helps with writing and reading enum values.
    /// </summary>
    /// <typeparam name="T">The Type of Enum.</typeparam>
    public abstract class EnumHelper<T> : IEnumValueReader<T>, IEnumValueWriter<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        static readonly Type[] _supportedTypes = new Type[]
        { typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int) };

        static readonly T[] _values;

        readonly int _bitsRequired;
        readonly int _maxValue;
        readonly int _minValue;

        /// <summary>
        /// Gets the defined values the Enum of type <typeparamref name="T"/>.
        /// </summary>
        public static IEnumerable<T> Values
        {
            get { return _values; }
        }

        /// <summary>
        /// Gets the number of bits required to write the enums values.
        /// </summary>
        public int BitsRequired
        {
            get { return _bitsRequired; }
        }

        /// <summary>
        /// Gets the maximum defined enum value.
        /// </summary>
        public int MaxValue
        {
            get { return _maxValue; }
        }

        /// <summary>
        /// Gets the minimum defined enum value.
        /// </summary>
        public int MinValue
        {
            get { return _minValue; }
        }

        static EnumHelper()
        {
            if (!typeof(T).IsEnum)
                throw CreateGenericTypeIsNotEnumException();

            _values = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumHelper&lt;T&gt;"/> class.
        /// </summary>
        protected EnumHelper()
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Type parameter must be an enum.");

            Type underlyingType = Enum.GetUnderlyingType(typeof(T));
            if (!_supportedTypes.Contains(underlyingType))
                throw new ArgumentException("The given enum type parameter's underlying type is not supported.");

            var valuesAsInt = _values.Select(x => Convert.ToInt32(x));

            _minValue = valuesAsInt.Min();
            _maxValue = valuesAsInt.Max();
            Debug.Assert(_minValue <= _maxValue);

            int diff = _maxValue - _minValue;
            Debug.Assert(diff >= 0);
            Debug.Assert(diff >= uint.MinValue && diff <= uint.MaxValue);

            _bitsRequired = BitOps.RequiredBits((uint)diff);
        }

        static MethodAccessException CreateGenericTypeIsNotEnumException()
        {
            const string errmsg = "Type parameter T ({0}) must be an Enum.";
            return new MethodAccessException(string.Format(errmsg, typeof(T)));
        }

        /// <summary>
        /// When overridden in the derived class, casts an int to type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The <paramref name="value"/> casted to type <typeparamref name="T"/>.</returns>
        protected abstract T FromInt(int value);

        public static bool IsDefined(T value)
        {
            if (!typeof(T).IsEnum)
                throw CreateGenericTypeIsNotEnumException();

            return Enum.IsDefined(typeof(T), value);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object.
        /// </summary>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <returns>The enum value parsed from <paramref name="value"/>.</returns>
        public static T Parse(string value)
        {
            if (!typeof(T).IsEnum)
                throw CreateGenericTypeIsNotEnumException();

            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="ignoreCase">If true, ignore case; otherwise, regard case.</param>
        /// <returns>The enum value parsed from <paramref name="value"/>.</returns>
        public static T Parse(string value, bool ignoreCase)
        {
            if (!typeof(T).IsEnum)
                throw CreateGenericTypeIsNotEnumException();

            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bitStream">The <see cref="BitStream"/> to read from.</param>
        /// <returns>The value read from the <see cref="bitStream"/>.</returns>
        public T ReadValue(BitStream bitStream)
        {
            int v = (int)(bitStream.ReadUInt(_bitsRequired) + _minValue);
            return FromInt(v);
        }

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from a <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>The value read from the <paramref name="reader"/>.</returns>
        public T ReadValue(IValueReader reader, string name)
        {
            int v = (int)(reader.ReadUInt(name, _bitsRequired) + _minValue);
            return FromInt(v);
        }

        /// <summary>
        /// When overridden in the derived class, casts type <typeparamref name="T"/> to an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <paramref name="value"/> casted to an int.</returns>
        protected abstract int ToInt(T value);

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="outValue">When this method returns true, contains the parsed enum value.</param>
        /// <returns>The enum value parsed from <paramref name="value"/>.</returns>
        public static bool TryParse(string value, out T outValue)
        {
            if (!typeof(T).IsEnum)
                throw CreateGenericTypeIsNotEnumException();

            try
            {
                outValue = (T)Enum.Parse(typeof(T), value);
            }
            catch (ArgumentException)
            {
                outValue = default(T);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Converts the string representation of the name or numeric value of one or more enumerated
        /// constants to an equivalent enumerated object.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="value">A string containing the name or value to convert.</param>
        /// <param name="ignoreCase">If true, ignore case; otherwise, regard case.</param>
        /// <param name="outValue">When this method returns true, contains the parsed enum value.</param>
        /// <returns>The enum value parsed from <paramref name="value"/>.</returns>
        public static bool TryParse(string value, bool ignoreCase, out T outValue)
        {
            if (!typeof(T).IsEnum)
                throw CreateGenericTypeIsNotEnumException();

            try
            {
                outValue = (T)Enum.Parse(typeof(T), value, ignoreCase);
            }
            catch (ArgumentException)
            {
                outValue = default(T);
                return false;
            }

            return true;
        }

        public static T ReadName(BitStream bitStream)
        {
            string str = bitStream.ReadString();
            T value = EnumIOHelper.FromName<T>(str);
            return value;
        }

        public static T ReadName(IValueReader reader, string name)
        {
            string str = reader.ReadString(name);
            T value = EnumIOHelper.FromName<T>(str);
            return value;
        }

        public static void WriteName(BitStream bitStream, T value)
        {
            string str = EnumIOHelper.ToName(value);
            bitStream.Write(str);
        }

        public static void WriteName(IValueWriter writer, string name, T value)
        {
            string str = EnumIOHelper.ToName(value);
            writer.Write(name, str);
        }

        /// <summary>
        /// Writes a value of type <typeparamref name="T"/> to a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bitStream">The <see cref="BitStream"/> to write to.</param>
        /// <param name="value">The value to write.</param>
        public void WriteValue(BitStream bitStream, T value)
        {
            uint v = (uint)(ToInt(value) - _minValue);
            bitStream.Write(v, _bitsRequired);
        }

        /// <summary>
        /// Writes a value of type <typeparamref name="T"/> to a <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">The value to write.</param>
        public void WriteValue(IValueWriter writer, string name, T value)
        {
            uint v = (uint)(ToInt(value) - _minValue);
            writer.Write(name, v, _bitsRequired);
        }

        #region IEnumReader<T> Members

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from the given <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read from.</param>
        /// <param name="name">The name of the value to read.</param>
        /// <returns>The value read from the <paramref name="reader"/> with the given <paramref name="name"/>.</returns>
        T IEnumValueReader<T>.ReadEnum(IValueReader reader, string name)
        {
            int value = reader.ReadInt(name, _bitsRequired);
            return FromInt(value);
        }

        #endregion

        #region IEnumWriter<T> Members

        /// <summary>
        /// Writes an Enum of type <typeparamref name="T"/> to the given <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer"><see cref="IValueWriter"/> to write to.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">The value to write.</param>
        void IEnumValueWriter<T>.WriteEnum(IValueWriter writer, string name, T value)
        {
            int v = ToInt(value);
            writer.Write(name, v, _bitsRequired);
        }

        #endregion
    }
}