using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore;

namespace NetGore.IO
{
    /// <summary>
    /// Base class for a class that helps with performing operations with and on Enums.
    /// </summary>
    /// <typeparam name="T">The Type of Enum.</typeparam>
    public abstract class EnumIOHelper<T> : IEnumValueReader<T>, IEnumValueWriter<T>
        where T : struct, IComparable, IConvertible, IFormattable
    {
        static readonly Type[] _supportedTypes = new Type[] { typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(int) };

        readonly int _bitsRequired;
        readonly int _maxValue;
        readonly int _minValue;

        /// <summary>
        /// Gets the defined values in this Enum.
        /// </summary>
        public static IEnumerable<T> Values { get { return EnumHelper<T>.Values; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumHelper&lt;T&gt;"/> class.
        /// </summary>
        protected EnumIOHelper()
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Type parameter must be an enum.");

            var underlyingType = Enum.GetUnderlyingType(typeof(T));
            if (!_supportedTypes.Contains(underlyingType))
                throw new ArgumentException("The given enum type parameter's underlying type is not supported.");

            var valuesAsInt = EnumHelper<T>.Values.Select(x => Convert.ToInt32(x));

            _minValue = valuesAsInt.Min();
            _maxValue = valuesAsInt.Max();
            Debug.Assert(_minValue <= _maxValue);

            var diff = _maxValue - _minValue;
            Debug.Assert(diff >= 0);
            Debug.Assert(diff >= uint.MinValue);

            _bitsRequired = BitOps.RequiredBits((uint)diff);
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

        /// <summary>
        /// When overridden in the derived class, casts an int to type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="value">The int value.</param>
        /// <returns>The <paramref name="value"/> casted to type <typeparamref name="T"/>.</returns>
        protected abstract T FromInt(int value);

        /// <summary>
        /// Reads the Enum value using the name of the Enum instead of the underlying integer value.
        /// </summary>
        /// <param name="bitStream">The BitStream to read the value from.</param>
        /// <returns>The value read from the <paramref name="bitStream"/>.</returns>
        public static T ReadName(BitStream bitStream)
        {
            var str = bitStream.ReadString();
            var value = FromName(str);
            return value;
        }

        /// <summary>
        /// Reads the Enum value using the underlying integer value of the Enum instead of the name.
        /// </summary>
        /// <param name="reader">The IValueReader to read the value from.</param>
        /// <param name="name">The name of the value to read.</param>
        /// <returns>The value read from the <paramref name="reader"/>.</returns>
        public static T ReadName(IValueReader reader, string name)
        {
            var str = reader.ReadString(name);
            var value = FromName(str);
            return value;
        }

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bitStream">The <see cref="BitStream"/> to read from.</param>
        /// <returns>The value read from the <see cref="bitStream"/>.</returns>
        public T ReadValue(BitStream bitStream)
        {
            var v = (int)(bitStream.ReadUInt(_bitsRequired) + _minValue);
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
            var v = (int)(reader.ReadUInt(name, _bitsRequired) + _minValue);
            return FromInt(v);
        }

        /// <summary>
        /// When overridden in the derived class, casts type <typeparamref name="T"/> to an int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <paramref name="value"/> casted to an int.</returns>
        protected abstract int ToInt(T value);

        /// <summary>
        /// Writes the Enum value using the name of the Enum instead of the underlying integer value.
        /// </summary>
        /// <param name="bitStream">The BitStream to write the value to.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteName(BitStream bitStream, T value)
        {
            var str = ToName(value);
            bitStream.Write(str);
        }

        /// <summary>
        /// Writes the Enum value using the underlying integer value of the Enum instead of the name.
        /// </summary>
        /// <param name="writer">The IValueWriter to write the value to.</param>
        /// <param name="name">The name of the value to write.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteName(IValueWriter writer, string name, T value)
        {
            var str = ToName(value);
            writer.Write(name, str);
        }

        /// <summary>
        /// Writes a value of type <typeparamref name="T"/> to a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bitStream">The <see cref="BitStream"/> to write to.</param>
        /// <param name="value">The value to write.</param>
        public void WriteValue(BitStream bitStream, T value)
        {
            var signedV = ToInt(value) - _minValue;
            Debug.Assert(signedV >= uint.MinValue);
            var v = (uint)signedV;
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
            var signedV = ToInt(value) - _minValue;
            Debug.Assert(signedV >= uint.MinValue);
            var v = (uint)signedV;
            writer.Write(name, v, _bitsRequired);
        }

        #region IEnumValueReader<T> Members

        /// <summary>
        /// Reads a value of type <typeparamref name="T"/> from the given <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read from.</param>
        /// <param name="name">The name of the value to read.</param>
        /// <returns>The value read from the <paramref name="reader"/> with the given <paramref name="name"/>.</returns>
        T IEnumValueReader<T>.ReadEnum(IValueReader reader, string name)
        {
            return ReadValue(reader, name);
        }

        #endregion

        #region IEnumValueWriter<T> Members

        /// <summary>
        /// Writes an Enum of type <typeparamref name="T"/> to the given <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer"><see cref="IValueWriter"/> to write to.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">The value to write.</param>
        void IEnumValueWriter<T>.WriteEnum(IValueWriter writer, string name, T value)
        {
            WriteValue(writer, name, value);
        }

        #endregion


        /// <summary>
        /// Gets the Enum of the given type from its name. 
        /// </summary>
        /// <typeparam name="T">The Type of <see cref="Enum"/>.</typeparam>
        /// <param name="value">The name of the <see cref="Enum"/> value.</param>
        /// <returns>The parsed enum.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="T"/> is not an <see cref="Enum"/> -or-
        /// <paramref name="value"/> is an valid enum name -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        public static T FromName(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Reads an <see cref="Enum"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <typeparam name="T">The Type of <see cref="Enum"/>.</typeparam>
        /// <param name="reader">The <see cref="IValueReader"/> to read the value from.</param>
        /// <param name="enumReader">The enum value reader.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>The read and parsed enum.</returns>
        public static T ReadEnum(IValueReader reader, IEnumValueReader<T> enumReader, string name)
        {
            if (reader.UseEnumNames)
                return reader.ReadEnumName<T>(name);
            else
                return reader.ReadEnumValue(enumReader, name);
        }

        /// <summary>
        /// Gets the name of the <see cref="Enum"/>.
        /// </summary>
        /// <typeparam name="T">The Type of <see cref="Enum"/>.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>The string name of the <paramref name="value"/>.</returns>
        public static string ToName(T value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Writes an <see cref="Enum"/> to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <typeparam name="T">The Type of <see cref="Enum"/>.</typeparam>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the value to.</param>
        /// <param name="enumWriter">The enum value writer.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">The enum to write.</param>
        public static void WriteEnum(IValueWriter writer, IEnumValueWriter<T> enumWriter, string name, T value)
        {
            if (writer.UseEnumNames)
                writer.WriteEnumName(name, value);
            else
                writer.WriteEnumValue(enumWriter, name, value);
        }
    }
}