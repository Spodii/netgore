using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.IO
{
    public partial class BitStream
    {
        #region IValueReader Members

        /// <summary>
        /// Gets if this <see cref="IValueReader"/> supports using the name field to look up values. If false,
        /// values will have to be read back in the same order they were written and the name field will be ignored.
        /// </summary>
        bool IValueReader.SupportsNameLookup
        {
            get { return false; }
        }

        /// <summary>
        /// Gets if this <see cref="IValueReader"/> supports reading nodes. If false, any attempt to use nodes
        /// in this IValueWriter will result in a NotSupportedException being thrown.
        /// </summary>
        bool IValueReader.SupportsNodes
        {
            get { return false; }
        }

        /// <summary>
        /// Gets if Enum I/O will be done with the Enum's name. If true, the name of the Enum value instead of the
        /// underlying integer value will be used. If false, the underlying integer value will be used. This
        /// only to Enum I/O that does not explicitly state which method to use.
        /// </summary>
        public bool UseEnumNames
        {
            get { return _useEnumNames; }
        }

        /// <summary>
        /// Reads a boolean.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        bool IValueReader.ReadBool(string name)
        {
            return ReadBool();
        }

        /// <summary>
        /// Reads a 8-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        byte IValueReader.ReadByte(string name)
        {
            return ReadByte();
        }

        /// <summary>
        /// Reads a 64-bit floating-point number.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        double IValueReader.ReadDouble(string name)
        {
            return ReadDouble();
        }

        /// <summary>
        /// Reads an Enum of type <typeparamref name="T"/>. Whether to use the Enum's underlying integer value or the
        /// name of the Enum value is determined from the <see cref="UseEnumNames"/> property.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        public T ReadEnum<T>(string name) where T : struct, IComparable, IConvertible, IFormattable
        {
            if (UseEnumNames)
                return ReadEnumName<T>();
            else
                return ReadEnumValue<T>();
        }

        /// <summary>
        /// Reads an Enum of type <typeparamref name="T"/> using the Enum's name instead of the value.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        public T ReadEnumName<T>(string name) where T : struct, IComparable, IConvertible, IFormattable
        {
            return ReadEnumName<T>();
        }

        /// <summary>
        /// Reads an Enum of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        public T ReadEnumValue<T>(string name) where T : struct, IComparable, IConvertible, IFormattable
        {
            return ReadEnumValue<T>();
        }

        /// <summary>
        /// Reads a 32-bit floating-point number.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        float IValueReader.ReadFloat(string name)
        {
            return ReadFloat();
        }

        /// <summary>
        /// Reads a 32-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        int IValueReader.ReadInt(string name)
        {
            return ReadInt();
        }

        /// <summary>
        /// Reads a signed integer of up to 32 bits.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="bits">Number of bits to read.</param>
        /// <returns>Value read from the reader.</returns>
        int IValueReader.ReadInt(string name, int bits)
        {
            return ReadInt(bits);
        }

        /// <summary>
        /// Reads a 64-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        long IValueReader.ReadLong(string name)
        {
            return ReadLong();
        }

        /// <summary>
        /// Unsupported by the BitStream.
        /// </summary>
        /// <typeparam name="T">The Type of value to read.</typeparam>
        /// <param name="nodeName">Unused by the BitStream.</param>
        /// <param name="readHandler">Delegate that reads the values from the IValueReader.</param>
        /// <returns>Array of the values read the IValueReader.</returns>
        /// <exception cref="NotSupportedException">This method is not supported by the <see cref="BitStream"/>.</exception>
        T[] IValueReader.ReadMany<T>(string nodeName, ReadManyHandler<T> readHandler)
        {
            throw CreateNodesNotSupportedException();
        }

        /// <summary>
        /// Unsupported by the BitStream.
        /// </summary>
        /// <typeparam name="T">The Type of nodes to read.</typeparam>
        /// <param name="nodeName">Unused by the BitStream.</param>
        /// <param name="readHandler">Delegate that reads the values from the IValueReader.</param>
        /// <returns>Array of the values read the IValueReader.</returns>
        /// <exception cref="NotSupportedException">This method is not supported by the <see cref="BitStream"/>.</exception>
        T[] IValueReader.ReadManyNodes<T>(string nodeName, ReadManyNodesHandler<T> readHandler)
        {
            throw CreateNodesNotSupportedException();
        }

        /// <summary>
        /// Reads multiple nodes that were written with WriteMany.
        /// </summary>
        /// <typeparam name="T">The Type of nodes to read.</typeparam>
        /// <param name="nodeName">The name of the root node containing the values.</param>
        /// <param name="readHandler">Delegate that reads the values from the IValueReader.</param>
        /// <param name="handleMissingKey">Allows for handling when a key is missing or invalid instead of throwing
        /// an <see cref="Exception"/>. This allows nodes to be read even if one or more of the expected
        /// items are missing. The returned array will contain null for these indicies. The int contained in the
        /// <see cref="Action{T}"/> contains the 0-based index of the index that failed. This parameter is only
        /// valid when <see cref="IValueReader.SupportsNameLookup"/> and <see cref="IValueReader.SupportsNodes"/> are true.
        /// Default is null.</param>
        /// <returns>
        /// Array of the values read the IValueReader.
        /// </returns>
        T[] IValueReader.ReadManyNodes<T>(string nodeName, ReadManyNodesHandler<T> readHandler,
                                          Action<int, Exception> handleMissingKey)
        {
            return ((IValueReader)this).ReadManyNodes(nodeName, readHandler);
        }

        /// <summary>
        /// Unsupported by the BitStream.
        /// </summary>
        /// <param name="key">Unused by the BitStream.</param>
        /// <returns>An IValueReader to read the child node.</returns>
        /// <exception cref="NotSupportedException">This method is not supported by the <see cref="BitStream"/>.</exception>
        IValueReader IValueReader.ReadNode(string key)
        {
            throw CreateNodesNotSupportedException();
        }

        /// <summary>
        /// Unsupported by the BitStream.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="count">The number of nodes to read. Must be greater than 0. An ArgumentOutOfRangeException will
        /// be thrown if this value exceeds the actual number of nodes available.</param>
        /// <returns>An IEnumerable of IValueReaders used to read the nodes.</returns>
        /// <exception cref="NotSupportedException">This method is not supported by the <see cref="BitStream"/>.</exception>
        IEnumerable<IValueReader> IValueReader.ReadNodes(string name, int count)
        {
            throw CreateNodesNotSupportedException();
        }

        /// <summary>
        /// Reads a 8-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        sbyte IValueReader.ReadSByte(string name)
        {
            return ReadSByte();
        }

        /// <summary>
        /// Reads a 16-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        short IValueReader.ReadShort(string name)
        {
            return ReadShort();
        }

        /// <summary>
        /// Reads a variable-length string of up to 65535 characters in length.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>String read from the reader.</returns>
        string IValueReader.ReadString(string name)
        {
            return ReadString();
        }

        /// <summary>
        /// Reads an unsigned integer of up to 32 bits.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="bits">Number of bits to read.</param>
        /// <returns>Value read from the reader.</returns>
        uint IValueReader.ReadUInt(string name, int bits)
        {
            return ReadUInt(bits);
        }

        /// <summary>
        /// Reads a 32-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        uint IValueReader.ReadUInt(string name)
        {
            return ReadUInt();
        }

        /// <summary>
        /// Reads a 64-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        ulong IValueReader.ReadULong(string name)
        {
            return ReadULong();
        }

        /// <summary>
        /// Reads a 16-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        ushort IValueReader.ReadUShort(string name)
        {
            return ReadUShort();
        }

        #endregion
    }
}