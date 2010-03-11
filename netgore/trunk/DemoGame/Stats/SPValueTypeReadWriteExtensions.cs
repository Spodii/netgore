using System.Data;
using System.Linq;
using NetGore.IO;

namespace DemoGame
{
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