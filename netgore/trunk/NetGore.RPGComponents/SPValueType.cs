using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using NetGore;
using NetGore.IO;
using NetGore.RPGComponents;

namespace NetGore.RPGComponents
{
    /// <summary>
    /// Contains the point value of a Character's status.
    /// </summary>
    public struct SPValueType
    {
        public const short MaxValue = short.MaxValue;
        public const short MinValue = short.MinValue;

        readonly short _value;

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
        /// Reads an SPValueType from an IDataReader.
        /// </summary>
        /// <param name="reader">IDataReader to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The SPValueType read from the IDataReader.</returns>
        public static SPValueType Read(IDataReader reader, int i)
        {
            var value = reader.GetValue(i);
            if (value is short)
                return new SPValueType((short)value);

            var convertedValue = Convert.ToInt16(value);
            return new SPValueType(convertedValue);
        }

        /// <summary>
        /// Reads an SPValueType from an IDataReader.
        /// </summary>
        /// <param name="reader">IDataReader to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The SPValueType read from the IDataReader.</returns>
        public static SPValueType Read(IDataReader reader, string name)
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

        public static implicit operator short(SPValueType v)
        {
            return v._value;
        }

        public static implicit operator SPValueType(short v)
        {
            return new SPValueType(v);
        }

        public static implicit operator SPValueType(int v)
        {
            Debug.Assert(v <= short.MaxValue);
            Debug.Assert(v >= short.MinValue);

            return new SPValueType((short)v);
        }
    }

    /// <summary>
    /// Adds extensions to some data I/O objects for performing Read and Write operations for the SPValueType.
    /// All of the operations are implemented in the SPValueType struct. These extensions are provided
    /// purely for the convenience of accessing all the I/O operations from the same place.
    /// </summary>
    public static class SPValueTypeReadWriteExtensions
    {
        /// <summary>
        /// Reads the CustomValueType from an IDataReader.
        /// </summary>
        /// <param name="dataReader">IDataReader to read the CustomValueType from.</param>
        /// <param name="i">The field index to read.</param>
        /// <returns>The CustomValueType read from the IDataReader.</returns>
        public static SPValueType GetSPValueType(this IDataReader dataReader, int i)
        {
            return SPValueType.Read(dataReader, i);
        }

        /// <summary>
        /// Reads the CustomValueType from an IDataReader.
        /// </summary>
        /// <param name="dataReader">IDataReader to read the CustomValueType from.</param>
        /// <param name="name">The name of the field to read the value from.</param>
        /// <returns>The CustomValueType read from the IDataReader.</returns>
        public static SPValueType GetSPValueType(this IDataReader dataReader, string name)
        {
            return SPValueType.Read(dataReader, name);
        }

        /// <summary>
        /// Reads the CustomValueType from a BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to read the CustomValueType from.</param>
        /// <returns>The CustomValueType read from the BitStream.</returns>
        public static SPValueType ReadSPValueType(this BitStream bitStream)
        {
            return SPValueType.Read(bitStream);
        }

        /// <summary>
        /// Reads the CustomValueType from an IValueReader.
        /// </summary>
        /// <param name="valueReader">IValueReader to read the CustomValueType from.</param>
        /// <param name="name">The unique name of the value to read.</param>
        /// <returns>The CustomValueType read from the IValueReader.</returns>
        public static SPValueType ReadSPValueType(this IValueReader valueReader, string name)
        {
            return SPValueType.Read(valueReader, name);
        }

        /// <summary>
        /// Writes a SPValueType to a BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="value">SPValueType to write.</param>
        public static void Write(this BitStream bitStream, SPValueType value)
        {
            value.Write(bitStream);
        }

        /// <summary>
        /// Writes a SPValueType to a IValueWriter.
        /// </summary>
        /// <param name="valueWriter">IValueWriter to write to.</param>
        /// <param name="name">Unique name of the SPValueType that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">SPValueType to write.</param>
        public static void Write(this IValueWriter valueWriter, string name, SPValueType value)
        {
            value.Write(valueWriter, name);
        }
    }
}