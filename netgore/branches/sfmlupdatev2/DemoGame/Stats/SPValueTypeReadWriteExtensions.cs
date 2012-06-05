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
        /// Reads the <see cref="SPValueType"/> from an <see cref="IDataRecord"/>.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to read the <see cref="SPValueType"/> from.</param>
        /// <param name="i">The field index to read.</param>
        /// <returns>The <see cref="SPValueType"/> read from the <see cref="IDataRecord"/>.</returns>
        public static SPValueType GetSPValueType(this IDataRecord r, int i)
        {
            return SPValueType.Read(r, i);
        }

        /// <summary>
        /// Reads the <see cref="SPValueType"/> from an <see cref="IDataRecord"/>.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to read the <see cref="SPValueType"/> from.</param>
        /// <param name="name">The name of the field to read the value from.</param>
        /// <returns>The <see cref="SPValueType"/> read from the <see cref="IDataRecord"/>.</returns>
        public static SPValueType GetSPValueType(this IDataRecord r, string name)
        {
            return SPValueType.Read(r, name);
        }

        /// <summary>
        /// Reads the <see cref="SPValueType"/> from a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bitStream"><see cref="BitStream"/> to read the <see cref="SPValueType"/> from.</param>
        /// <returns>The <see cref="SPValueType"/> read from the <see cref="BitStream"/>.</returns>
        public static SPValueType ReadSPValueType(this BitStream bitStream)
        {
            return SPValueType.Read(bitStream);
        }

        /// <summary>
        /// Reads the <see cref="SPValueType"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="valueReader"><see cref="IValueReader"/> to read the <see cref="SPValueType"/> from.</param>
        /// <param name="name">The unique name of the value to read.</param>
        /// <returns>The <see cref="SPValueType"/> read from the <see cref="IValueReader"/>.</returns>
        public static SPValueType ReadSPValueType(this IValueReader valueReader, string name)
        {
            return SPValueType.Read(valueReader, name);
        }

        /// <summary>
        /// Writes a <see cref="SPValueType"/> to a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bitStream"><see cref="BitStream"/> to write to.</param>
        /// <param name="value"><see cref="SPValueType"/> to write.</param>
        public static void Write(this BitStream bitStream, SPValueType value)
        {
            value.Write(bitStream);
        }

        /// <summary>
        /// Writes a <see cref="SPValueType"/> to a <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="valueWriter"><see cref="IValueWriter"/> to write to.</param>
        /// <param name="name">Unique name of the <see cref="SPValueType"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value"><see cref="SPValueType"/> to write.</param>
        public static void Write(this IValueWriter valueWriter, string name, SPValueType value)
        {
            value.Write(valueWriter, name);
        }
    }
}