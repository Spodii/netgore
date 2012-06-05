using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NetGore.IO;

namespace DemoGame
{
    /// <summary>
    /// Contains the value of a Character's Status Points (i.e. Health Points, Mana Points, etc).
    /// </summary>
    [TypeConverter(typeof(SPValueTypeConverter))]
    public struct SPValueType : IEquatable<SPValueType>
    {
        public const short MaxValue = short.MaxValue;
        public const short MinValue = short.MinValue;

        readonly short _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="SPValueType"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public SPValueType(short value)
        {
            _value = value;
        }

        /// <summary>
        /// Reads an SPValueType from an IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>The SPValueType read from the IValueReader.</returns>
        public static SPValueType Read(IValueReader reader, string name)
        {
            var value = reader.ReadShort(name);
            return new SPValueType(value);
        }

        /// <summary>
        /// Reads an SPValueType from an <see cref="IDataRecord"/>.
        /// </summary>
        /// <param name="reader"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The SPValueType read from the <see cref="IDataRecord"/>.</returns>
        public static SPValueType Read(IDataRecord reader, int i)
        {
            var value = reader.GetValue(i);
            if (value is short)
                return new SPValueType((short)value);

            var convertedValue = Convert.ToInt16(value);
            return new SPValueType(convertedValue);
        }

        /// <summary>
        /// Reads an SPValueType from an <see cref="IDataRecord"/>.
        /// </summary>
        /// <param name="reader"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The SPValueType read from the <see cref="IDataRecord"/>.</returns>
        public static SPValueType Read(IDataRecord reader, string name)
        {
            return Read(reader, reader.GetOrdinal(name));
        }

        /// <summary>
        /// Reads an SPValueType from an IValueReader.
        /// </summary>
        /// <param name="bitStream">BitStream to read from.</param>
        /// <returns>The SPValueType read from the BitStream.</returns>
        public static SPValueType Read(BitStream bitStream)
        {
            var value = bitStream.ReadShort();
            return new SPValueType(value);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return _value.ToString();
        }

        /// <summary>
        /// Writes the SPValueType to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="name">Unique name of the SPValueType that will be used to distinguish it
        /// from other values when reading.</param>
        public void Write(IValueWriter writer, string name)
        {
            writer.Write(name, _value);
        }

        /// <summary>
        /// Writes the SPValueType to an IValueWriter.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        public void Write(BitStream bitStream)
        {
            bitStream.Write(_value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="DemoGame.SPValueType"/> to <see cref="System.Int16"/>.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator short(SPValueType v)
        {
            return v._value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int16"/> to <see cref="DemoGame.SPValueType"/>.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SPValueType(short v)
        {
            return new SPValueType(v);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int32"/> to <see cref="DemoGame.SPValueType"/>.
        /// </summary>
        /// <param name="v">The v.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SPValueType(int v)
        {
            Debug.Assert(v <= short.MaxValue);
            Debug.Assert(v >= short.MinValue);

            return new SPValueType((short)v);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(SPValueType left, SPValueType right)
        {
            return left._value == right._value;
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(SPValueType left, SPValueType right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(SPValueType other)
        {
            return this == other;
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
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is SPValueType && this == (SPValueType)obj;
        }
    }
}