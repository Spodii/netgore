using System;
using System.Data;
using System.Runtime.InteropServices;
using NetGore.IO;

namespace DemoGame.Server
{
    /// <summary>
    /// Defines a value used to determine the chance that an ItemTemplate will be created.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct ItemChance : IComparable, IFormattable
    {
        /// <summary>
        /// Represents the largest possible value of ItemChance. This field is constant.
        /// </summary>
        const int _maxValue = 10000;

        /// <summary>
        /// Represents the smallest possible value of ItemChance. This field is constant.
        /// </summary>
        const int _minValue = ushort.MinValue;

        static readonly Random _random = new Random();

        /// <summary>
        /// The underlying value. This contains the actual value of the struct instance.
        /// </summary>
        readonly ushort _value;

        /// <summary>
        /// ItemChance constructor.
        /// </summary>
        /// <param name="percent">The chance, in percentage, to assign to this ItemChance, where 0.0f is 0% and 1.0f
        /// is a 100% chance. Must be between 0.0f and 1.0f.</param>
        public ItemChance(float percent)
        {
            if (percent < 0.0f || percent > 1.0f)
                throw new ArgumentOutOfRangeException("percent");

            _value = (ushort)Math.Round(percent * _maxValue);

            // Ensure there were no rounding errors
            if (_value > _maxValue)
                _value = _maxValue;
        }

        /// <summary>
        /// ItemChance constructor.
        /// </summary>
        /// <param name="value">Value to assign to the new ItemChance.</param>
        ItemChance(int value)
        {
            if (value < _minValue || value > _maxValue)
                throw new ArgumentOutOfRangeException("value");

            _value = (ushort)value;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// True if <paramref name="other"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="other">Another object to compare to. 
        /// </param><filterpriority>2</filterpriority>
        public bool Equals(ItemChance other)
        {
            return other._value == _value;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// True if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. 
        /// </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (obj.GetType() != typeof(ItemChance))
                return false;
            return Equals((ItemChance)obj);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        /// <summary>
        /// Reads an ItemChance from an IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>The ItemChance read from the IValueReader.</returns>
        public static ItemChance Read(IValueReader reader, string name)
        {
            ushort value = reader.ReadUShort(name);
            return new ItemChance(value);
        }

        /// <summary>
        /// Reads an ItemChance from an IDataReader.
        /// </summary>
        /// <param name="reader">IDataReader to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The ItemChance read from the IDataReader.</returns>
        public static ItemChance Read(IDataReader reader, int i)
        {
            object value = reader.GetValue(i);
            if (value is ushort)
                return new ItemChance((ushort)value);

            ushort convertedValue = Convert.ToUInt16(value);
            return new ItemChance(convertedValue);
        }

        /// <summary>
        /// Reads an ItemChance from an IDataReader.
        /// </summary>
        /// <param name="reader">IDataReader to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The ItemChance read from the IDataReader.</returns>
        public static ItemChance Read(IDataReader reader, string name)
        {
            return Read(reader, reader.GetOrdinal(name));
        }

        /// <summary>
        /// Reads an ItemChance from an IValueReader.
        /// </summary>
        /// <param name="bitStream">BitStream to read from.</param>
        /// <returns>The ItemChance read from the BitStream.</returns>
        public static ItemChance Read(BitStream bitStream)
        {
            ushort value = bitStream.ReadUShort();
            return new ItemChance(value);
        }

        /// <summary>
        /// Performs a test against the ItemChance.
        /// </summary>
        /// <returns>True if the test passed; otherwise false.</returns>
        public bool Test()
        {
            int randValue = _random.Next(_minValue + 1, _maxValue + 1);
            return randValue <= _value;
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
        /// Writes the ItemChance to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="name">Unique name of the ItemChance that will be used to distinguish it
        /// from other values when reading.</param>
        public void Write(IValueWriter writer, string name)
        {
            writer.Write(name, _value);
        }

        /// <summary>
        /// Writes the ItemChance to an IValueWriter.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        public void Write(BitStream bitStream)
        {
            bitStream.Write(_value);
        }

        #region IComparable Members

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has these meanings: 
        ///                     Value 
        ///                     Meaning 
        ///                     Less than zero 
        ///                     This instance is less than <paramref name="obj"/>. 
        ///                     Zero 
        ///                     This instance is equal to <paramref name="obj"/>. 
        ///                     Greater than zero 
        ///                     This instance is greater than <paramref name="obj"/>. 
        /// </returns>
        /// <param name="obj">An object to compare with this instance. 
        ///                 </param><exception cref="T:System.ArgumentException"><paramref name="obj"/> is not the same type as this instance. 
        ///                 </exception><filterpriority>2</filterpriority>
        public int CompareTo(object obj)
        {
            return _value.CompareTo(obj);
        }

        #endregion

        #region IFormattable Members

        /// <summary>
        /// Formats the value of the current instance using the specified format.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> containing the value of the current instance in the specified format.
        /// </returns>
        /// <param name="format">The <see cref="T:System.String"/> specifying the format to use.
        ///                     -or- 
        ///                 null to use the default format defined for the type of the <see cref="T:System.IFormattable"/> implementation. 
        ///                 </param><param name="formatProvider">The <see cref="T:System.IFormatProvider"/> to use to format the value.
        ///                     -or- 
        ///                 null to obtain the numeric format information from the current locale setting of the operating system. 
        ///                 </param><filterpriority>2</filterpriority>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider);
        }

        #endregion

        /// <summary>
        /// Implements operator >.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the left argument is greater than the right.</returns>
        public static bool operator >(ItemChance left, ItemChance right)
        {
            return left._value > right._value;
        }

        /// <summary>
        /// Implements operator <.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the right argument is greater than the left.</returns>
        public static bool operator <(ItemChance left, ItemChance right)
        {
            return left._value < right._value;
        }

        /// <summary>
        /// Implements operator >=.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the left argument is greater than or equal to the right.</returns>
        public static bool operator >=(int left, ItemChance right)
        {
            return left >= right._value;
        }

        /// <summary>
        /// Implements operator <=.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the right argument is greater than or equal to the left.</returns>
        public static bool operator <=(int left, ItemChance right)
        {
            return left <= right._value;
        }

        /// <summary>
        /// Implements operator >=.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the left argument is greater than or equal to the right.</returns>
        public static bool operator >=(ItemChance left, ItemChance right)
        {
            return left._value >= right._value;
        }

        /// <summary>
        /// Implements operator <=.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the right argument is greater than or equal to the left.</returns>
        public static bool operator <=(ItemChance left, ItemChance right)
        {
            return left._value <= right._value;
        }

        /// <summary>
        /// Implements operator !=.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the two arguments are not equal.</returns>
        public static bool operator !=(ItemChance left, ItemChance right)
        {
            return left._value != right._value;
        }

        /// <summary>
        /// Implements operator ==.
        /// </summary>
        /// <param name="left">Left side argument.</param>
        /// <param name="right">Right side argument.</param>
        /// <returns>If the two arguments are equal.</returns>
        public static bool operator ==(ItemChance left, ItemChance right)
        {
            return left._value == right._value;
        }
    }

    /// <summary>
    /// Adds extensions to some data I/O objects for performing Read and Write operations for the ItemChance.
    /// All of the operations are implemented in the ItemChance struct. These extensions are provided
    /// purely for the convenience of accessing all the I/O operations from the same place.
    /// </summary>
    public static class ItemChanceReadWriteExtensions
    {
        /// <summary>
        /// Reads the CustomValueType from an IDataReader.
        /// </summary>
        /// <param name="dataReader">IDataReader to read the CustomValueType from.</param>
        /// <param name="i">The field index to read.</param>
        /// <returns>The CustomValueType read from the IDataReader.</returns>
        public static ItemChance GetItemChance(this IDataReader dataReader, int i)
        {
            return ItemChance.Read(dataReader, i);
        }

        /// <summary>
        /// Reads the CustomValueType from an IDataReader.
        /// </summary>
        /// <param name="dataReader">IDataReader to read the CustomValueType from.</param>
        /// <param name="name">The name of the field to read the value from.</param>
        /// <returns>The CustomValueType read from the IDataReader.</returns>
        public static ItemChance GetItemChance(this IDataReader dataReader, string name)
        {
            return ItemChance.Read(dataReader, name);
        }

        /// <summary>
        /// Reads the CustomValueType from a BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to read the CustomValueType from.</param>
        /// <returns>The CustomValueType read from the BitStream.</returns>
        public static ItemChance ReadItemChance(this BitStream bitStream)
        {
            return ItemChance.Read(bitStream);
        }

        /// <summary>
        /// Reads the CustomValueType from an IValueReader.
        /// </summary>
        /// <param name="valueReader">IValueReader to read the CustomValueType from.</param>
        /// <param name="name">The unique name of the value to read.</param>
        /// <returns>The CustomValueType read from the IValueReader.</returns>
        public static ItemChance ReadItemChance(this IValueReader valueReader, string name)
        {
            return ItemChance.Read(valueReader, name);
        }

        /// <summary>
        /// Writes a ItemChance to a BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="value">ItemChance to write.</param>
        public static void Write(this BitStream bitStream, ItemChance value)
        {
            value.Write(bitStream);
        }

        /// <summary>
        /// Writes a ItemChance to a IValueWriter.
        /// </summary>
        /// <param name="valueWriter">IValueWriter to write to.</param>
        /// <param name="name">Unique name of the ItemChance that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">ItemChance to write.</param>
        public static void Write(this IValueWriter valueWriter, string name, ItemChance value)
        {
            value.Write(valueWriter, name);
        }
    }
}