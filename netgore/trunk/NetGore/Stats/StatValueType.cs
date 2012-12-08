using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;

namespace NetGore.Stats
{
    /// <summary>
    /// Represents the integer value of a single stat.
    /// </summary>
    [Serializable]
    public struct StatValueType : IComparable<StatValueType>, IConvertible, IFormattable, IComparable<int>, IEquatable<int>,
                                  IComparable<short>, IEquatable<short>
    {
        // NOTE: This struct uses a highly modified version of the CustomValueTypeTemplate. Copy with care.

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Represents the largest possible value of <see cref="StatValueType"/>. This field is constant.
        /// </summary>
        public const int MaxValue = short.MaxValue;

        /// <summary>
        /// Represents the smallest possible value of <see cref="StatValueType"/>. This field is constant.
        /// </summary>
        public const int MinValue = short.MinValue;

        /// <summary>
        /// The underlying value. This contains the actual value of the struct instance.
        /// </summary>
        readonly short _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatValueType"/> struct.
        /// </summary>
        /// <param name="value">Value to assign to the new <see cref="StatValueType"/>.</param>
        public StatValueType(int value)
        {
            const string errmsg =
                "Attempted to set StatValueType to `{0}`, but the {2}est StatValueType value supported is `{1}`." + 
                " Consider using a {2}er value type (e.g. Int32) for the StatValueType. The value has been altered to avoid under/overflow.";

            // Check for a valid range
            if (value < MinValue)
            {
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, value, MinValue, "small");
                Debug.Fail(string.Format(errmsg, value, MinValue, "small"));

                value = MinValue;
            }
            else if (value > MaxValue)
            {
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, value, MaxValue, "larg");
                Debug.Fail(string.Format(errmsg, value, MaxValue, "larg"));

                value = MaxValue;
            }

            _value = (short)value;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="other">Another object to compare to.</param>
        /// <returns>
        /// True if <paramref name="other"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public bool Equals(StatValueType other)
        {
            return other._value == _value;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// True if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is StatValueType && this == (StatValueType)obj;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        /// <summary>
        /// Gets the raw internal value of this <see cref="StatValueType"/>.
        /// </summary>
        /// <returns>The raw internal value.</returns>
        public short GetRawValue()
        {
            return _value;
        }

        /// <summary>
        /// Reads a <see cref="StatValueType"/> from an IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>The <see cref="StatValueType"/> read from the IValueReader.</returns>
        public static StatValueType Read(IValueReader reader, string name)
        {
            var value = reader.ReadShort(name);
            return new StatValueType(value);
        }

        /// <summary>
        /// Reads a <see cref="StatValueType"/> from an <see cref="IDataReader"/>.
        /// </summary>
        /// <param name="reader"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The <see cref="StatValueType"/> read from the <see cref="IDataReader"/>.</returns>
        public static StatValueType Read(IDataRecord reader, int i)
        {
            var value = reader.GetValue(i);
            if (value is short)
                return new StatValueType((short)value);

            var convertedValue = Convert.ToInt16(value);
            return new StatValueType(convertedValue);
        }

        /// <summary>
        /// Reads a <see cref="StatValueType"/> from an <see cref="IDataRecord"/>.
        /// </summary>
        /// <param name="reader"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The <see cref="StatValueType"/> read from the <see cref="IDataRecord"/>.</returns>
        public static StatValueType Read(IDataRecord reader, string name)
        {
            return Read(reader, reader.GetOrdinal(name));
        }

        /// <summary>
        /// Reads a <see cref="StatValueType"/> from a BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to read from.</param>
        /// <returns>The <see cref="StatValueType"/> read from the <see cref="BitStream"/>.</returns>
        public static StatValueType Read(BitStream bitStream)
        {
            var value = bitStream.ReadShort();
            return new StatValueType(value);
        }

        /// <summary>
        /// Converts the numeric value of this instance to its equivalent string representation.
        /// </summary>
        /// <returns>The string representation of the value of this instance, consisting of a sequence
        /// of digits ranging from 0 to 9, without leading zeroes.</returns>
        public override string ToString()
        {
            return _value.ToString();
        }

        /// <summary>
        /// Writes the <see cref="StatValueType"/> to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="name">Unique name of the <see cref="StatValueType"/> that will be used to distinguish it
        /// from other values when reading.</param>
        public void Write(IValueWriter writer, string name)
        {
            writer.Write(name, _value);
        }

        /// <summary>
        /// Writes the <see cref="StatValueType"/> to a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        public void Write(BitStream bitStream)
        {
            bitStream.Write(_value);
        }

        #region IComparable<int> Members

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: 
        ///                     Value 
        ///                     Meaning 
        ///                     Less than zero 
        ///                     This object is less than the <paramref name="other"/> parameter.
        ///                     Zero 
        ///                     This object is equal to <paramref name="other"/>. 
        ///                     Greater than zero 
        ///                     This object is greater than <paramref name="other"/>. 
        /// </returns>
        public int CompareTo(int other)
        {
            return _value.CompareTo(other);
        }

        #endregion

        #region IComparable<short> Members

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(short other)
        {
            return _value.CompareTo(other);
        }

        #endregion

        #region IComparable<StatValueType> Members

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared.
        /// The return value has the following meanings: 
        ///                     Value 
        ///                     Meaning 
        ///                     Less than zero 
        ///                     This object is less than the <paramref name="other"/> parameter.
        ///                     Zero 
        ///                     This object is equal to <paramref name="other"/>. 
        ///                     Greater than zero 
        ///                     This object is greater than <paramref name="other"/>. 
        /// </returns>
        public int CompareTo(StatValueType other)
        {
            return _value.CompareTo(other._value);
        }

        #endregion

        #region IConvertible Members

        /// <summary>
        /// Returns the <see cref="T:System.TypeCode"/> for this instance.
        /// </summary>
        /// <returns>
        /// The enumerated constant that is the <see cref="T:System.TypeCode"/> of the class or value type that implements this interface.
        /// </returns>
        public TypeCode GetTypeCode()
        {
            return _value.GetTypeCode();
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent Boolean value using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation
        /// that supplies culture-specific formatting information.</param>
        /// <returns>
        /// A Boolean value equivalent to the value of this instance.
        /// </returns>
        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToBoolean(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 8-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>
        /// An 8-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToByte(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent Unicode character using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>
        /// A Unicode character equivalent to the value of this instance.
        /// </returns>
        char IConvertible.ToChar(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToChar(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="T:System.DateTime"/> using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>
        /// A <see cref="T:System.DateTime"/> instance equivalent to the value of this instance.
        /// </returns>
        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToDateTime(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="T:System.Decimal"/> number using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information. </param>
        /// <returns>
        /// A <see cref="T:System.Decimal"/> number equivalent to the value of this instance.
        /// </returns>
        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToDecimal(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent double-precision floating-point number using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>
        /// A double-precision floating-point number equivalent to the value of this instance.
        /// </returns>
        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToDouble(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 16-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>
        /// An 16-bit signed integer equivalent to the value of this instance.
        /// </returns>
        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToInt16(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 32-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>
        /// An 32-bit signed integer equivalent to the value of this instance.
        /// </returns>
        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToInt32(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 64-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>
        /// An 64-bit signed integer equivalent to the value of this instance.
        /// </returns>
        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToInt64(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 8-bit signed integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>
        /// An 8-bit signed integer equivalent to the value of this instance.
        /// </returns>
        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToSByte(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent single-precision floating-point number using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information. </param>
        /// <returns>
        /// A single-precision floating-point number equivalent to the value of this instance.
        /// </returns>
        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToSingle(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent <see cref="T:System.String"/> using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>
        /// A <see cref="T:System.String"/> instance equivalent to the value of this instance.
        /// </returns>
        public string ToString(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToString(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an <see cref="T:System.Object"/> of the specified <see cref="T:System.Type"/> that has an equivalent value, using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="conversionType">The <see cref="T:System.Type"/> to which the value of this instance is converted.</param>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>
        /// An <see cref="T:System.Object"/> instance of type <paramref name="conversionType"/> whose value is equivalent to the value of this instance.
        /// </returns>
        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return ((IConvertible)_value).ToType(conversionType, provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 16-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>
        /// An 16-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToUInt16(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 32-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>
        /// An 32-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToUInt32(provider);
        }

        /// <summary>
        /// Converts the value of this instance to an equivalent 64-bit unsigned integer using the specified culture-specific formatting information.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider"/> interface implementation that supplies
        /// culture-specific formatting information.</param>
        /// <returns>
        /// An 64-bit unsigned integer equivalent to the value of this instance.
        /// </returns>
        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return ((IConvertible)_value).ToUInt64(provider);
        }

        #endregion

        #region IEquatable<int> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(int other)
        {
            return _value.Equals(other);
        }

        #endregion

        #region IEquatable<short> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(short other)
        {
            return _value.Equals(other);
        }

        #endregion

        #region IFormattable Members

        /// <summary>
        /// Formats the value of the current instance using the specified format.
        /// </summary>
        /// <param name="format">The <see cref="T:System.String"/> specifying the format to use.
        ///                     -or- 
        ///                 null to use the default format defined for the type of the <see cref="T:System.IFormattable"/> implementation. 
        /// </param>
        /// <param name="formatProvider">The <see cref="T:System.IFormatProvider"/> to use to format the value.
        ///                     -or- 
        ///                 null to obtain the numeric format information from the current locale setting of the operating system. 
        /// </param>
        /// <returns>
        /// A <see cref="T:System.String"/> containing the value of the current instance in the specified format.
        /// </returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider);
        }

        #endregion

        /// <summary>
        /// Casts a <see cref="StatValueType"/> to an <see cref="int"/>.
        /// </summary>
        /// <param name="StatValueType"><see cref="StatValueType"/> to cast.</param>
        /// <returns>The <see cref="int"/> value of the <see cref="StatValueType"/>.</returns>
        public static implicit operator short(StatValueType StatValueType)
        {
            return StatValueType._value;
        }

        /// <summary>
        /// Casts an <see cref="int"/> to a <see cref="StatValueType"/>.
        /// </summary>
        /// <param name="value">The <see cref="int"/> to cast.</param>
        /// <returns>The <see cref="StatValueType"/> casted from the <see cref="int"/>.</returns>
        public static implicit operator StatValueType(int value)
        {
            return new StatValueType(value);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="l">The left- argument.</param>
        /// <param name="r">The right-side argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(StatValueType l, StatValueType r)
        {
            return l.Equals(r);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="l">The left- argument.</param>
        /// <param name="r">The right-side argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(StatValueType l, StatValueType r)
        {
            return !(l == r);
        }

        public static bool operator <(StatValueType l, StatValueType r)
        {
            return l._value < r._value;
        }

        public static bool operator >(StatValueType l, StatValueType r)
        {
            return l._value > r._value;
        }

        public static bool operator <=(StatValueType l, StatValueType r)
        {
            return l._value <= r._value;
        }

        public static bool operator >=(StatValueType l, StatValueType r)
        {
            return l._value >= r._value;
        }
    }

    /// <summary>
    /// Adds extensions to some data I/O objects for performing read and write operations for the
    /// <see cref="StatValueType"/>. All of the operations are implemented in the <see cref="StatValueType"/>
    /// struct. These extensions are provided purely for the convenience of accessing all the I/O operations from the
    /// same place.
    /// </summary>
    public static class StatValueTypeReadWriteExtensions
    {
        /// <summary>
        /// Gets the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as a
        /// <see cref="StatValueType"/>.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The <see cref="IDictionary{TKey,TValue}"/>.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as a <see cref="StatValueType"/>.</returns>
        public static StatValueType AsStatValueType<T>(this IDictionary<T, string> dict, T key)
        {
            return Parser.Invariant.ParseStatValueType(dict[key]);
        }

        /// <summary>
        /// Tries to get the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as a
        /// <see cref="StatValueType"/>.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The <see cref="IDictionary{TKey, TValue}"/>.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <param name="defaultValue">The value to use if the value at the <paramref name="key"/> could not be parsed.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int, or the
        /// <paramref name="defaultValue"/> if the <paramref name="key"/> did not exist in the <paramref name="dict"/>
        /// or the value at the given <paramref name="key"/> could not be parsed.</returns>
        public static StatValueType AsStatValueType<T>(this IDictionary<T, string> dict, T key, StatValueType defaultValue)
        {
            string value;
            if (!dict.TryGetValue(key, out value))
                return defaultValue;

            StatValueType parsed;
            if (!Parser.Invariant.TryParse(value, out parsed))
                return defaultValue;

            return parsed;
        }

        /// <summary>
        /// Reads the <see cref="StatValueType"/> from an <see cref="IDataRecord"/>.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to read the <see cref="StatValueType"/> from.</param>
        /// <param name="i">The field index to read.</param>
        /// <returns>The <see cref="StatValueType"/> read from the <see cref="IDataRecord"/>.</returns>
        public static StatValueType GetStatValueType(this IDataRecord r, int i)
        {
            return StatValueType.Read(r, i);
        }

        /// <summary>
        /// Reads the <see cref="StatValueType"/> from an <see cref="IDataRecord"/>.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to read the <see cref="StatValueType"/> from.</param>
        /// <param name="name">The name of the field to read the value from.</param>
        /// <returns>The <see cref="StatValueType"/> read from the <see cref="IDataRecord"/>.</returns>
        public static StatValueType GetStatValueType(this IDataRecord r, string name)
        {
            return StatValueType.Read(r, name);
        }

        /// <summary>
        /// Parses the <see cref="StatValueType"/> from a string.
        /// </summary>
        /// <param name="parser">The <see cref="Parser"/> to use.</param>
        /// <param name="value">The string to parse.</param>
        /// <returns>The <see cref="StatValueType"/> parsed from the string.</returns>
        public static StatValueType ParseStatValueType(this Parser parser, string value)
        {
            return new StatValueType(parser.ParseShort(value));
        }

        /// <summary>
        /// Reads the <see cref="StatValueType"/> from a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bitStream"><see cref="BitStream"/> to read the <see cref="StatValueType"/> from.</param>
        /// <returns>The <see cref="StatValueType"/> read from the <see cref="BitStream"/>.</returns>
        public static StatValueType ReadStatValueType(this BitStream bitStream)
        {
            return StatValueType.Read(bitStream);
        }

        /// <summary>
        /// Reads the <see cref="StatValueType"/> from an IValueReader.
        /// </summary>
        /// <param name="valueReader"><see cref="IValueReader"/> to read the <see cref="StatValueType"/> from.</param>
        /// <param name="name">The unique name of the value to read.</param>
        /// <returns>The <see cref="StatValueType"/> read from the IValueReader.</returns>
        public static StatValueType ReadStatValueType(this IValueReader valueReader, string name)
        {
            return StatValueType.Read(valueReader, name);
        }

        /// <summary>
        /// Tries to parse the <see cref="StatValueType"/> from a string.
        /// </summary>
        /// <param name="parser">The <see cref="Parser"/> to use.</param>
        /// <param name="value">The string to parse.</param>
        /// <param name="outValue">If this method returns true, contains the parsed <see cref="StatValueType"/>.</param>
        /// <returns>True if the parsing was successfully; otherwise false.</returns>
        public static bool TryParse(this Parser parser, string value, out StatValueType outValue)
        {
            short tmp;
            var ret = parser.TryParse(value, out tmp);
            outValue = new StatValueType(tmp);
            return ret;
        }

        /// <summary>
        /// Writes a <see cref="StatValueType"/> to a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bitStream"><see cref="BitStream"/> to write to.</param>
        /// <param name="value"><see cref="StatValueType"/> to write.</param>
        public static void Write(this BitStream bitStream, StatValueType value)
        {
            value.Write(bitStream);
        }

        /// <summary>
        /// Writes a <see cref="StatValueType"/> to a <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="valueWriter"><see cref="IValueWriter"/> to write to.</param>
        /// <param name="name">Unique name of the <see cref="StatValueType"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value"><see cref="StatValueType"/> to write.</param>
        public static void Write(this IValueWriter valueWriter, string name, StatValueType value)
        {
            value.Write(valueWriter, name);
        }
    }
}