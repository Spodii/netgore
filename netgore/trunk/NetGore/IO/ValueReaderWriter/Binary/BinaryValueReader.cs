using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// Implementation of the <see cref="IValueReader"/> using a <see cref="BitStream"/> to perform binary I/O.
    /// </summary>
    public class BinaryValueReader : IValueReader
    {
        readonly BitStream _reader;
        readonly bool _useEnumNames = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryValueReader"/> class.
        /// </summary>
        /// <param name="reader">BitStream that will be used to read from.</param>
        /// <param name="useEnumNames">If true, Enums I/O will be done using the Enum's name. If false,
        /// Enum I/O will use the underlying integer value of the Enum.</param>
        /// <exception cref="ArgumentNullException"><paramref name="reader" /> is <c>null</c>.</exception>
        BinaryValueReader(BitStream reader, bool useEnumNames = true)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            _useEnumNames = useEnumNames;
            _reader = reader;
        }

        /// <summary>
        /// Creates a <see cref="BinaryValueReader"/> for reading a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="reader">BitStream that will be used to read from.</param>
        /// <param name="useEnumNames">If true, Enums I/O will be done using the Enum's name. If false,
        /// Enum I/O will use the underlying integer value of the Enum.</param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public static BinaryValueReader Create(BitStream reader, bool useEnumNames = true)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            return new BinaryValueReader(reader, useEnumNames);
        }

        public static BinaryValueReader Create(byte[] bytes, bool useEnumNames = true)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            BitStream bs = new BitStream(bytes, useEnumNames);
            return new BinaryValueReader(bs, useEnumNames);
        }

        /// <summary>
        /// Creates a <see cref="BinaryValueReader"/> for reading a file.
        /// </summary>
        /// <param name="filePath">The path of the file to read from.</param>
        /// <param name="useEnumNames">If true, Enums I/O will be done using the Enum's name. If false,
        /// Enum I/O will use the underlying integer value of the Enum.</param>
        /// <exception cref="ArgumentNullException"><paramref name="filePath"/> is null or empty.</exception>
        /// <exception cref="IOException"><paramref name="filePath"/> is invalid, inaccessible, or no file exists at that path.</exception>
        public static BinaryValueReader CreateFromFile(string filePath, bool useEnumNames = true)
        {
            if (filePath == null)
                throw new ArgumentNullException("filePath");

            var bytes = File.ReadAllBytes(filePath);
            var reader = new BitStream(bytes);
            return new BinaryValueReader(reader, useEnumNames);
        }

        /// <summary>
        /// Creates a <see cref="BinaryValueReader"/> for reading a string.
        /// </summary>
        /// <param name="data">The string to read.</param>
        /// <param name="useEnumNames">If true, Enums I/O will be done using the Enum's name. If false,
        /// Enum I/O will use the underlying integer value of the Enum.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        public static BinaryValueReader CreateFromString(string data, bool useEnumNames = true)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            var bytes = BitStream.StringToByteArray(data);
            var reader = new BitStream(bytes);
            return new BinaryValueReader(reader, useEnumNames);
        }

        /// <summary>
        /// Reads the next node.
        /// </summary>
        /// <returns>An IValueReader for reading the next node.</returns>
        IValueReader ReadNode()
        {
            var bitLength = ReadUInt(null);
            var bs = _reader.ReadBits((int)bitLength);
            return new BinaryValueReader(bs, UseEnumNames);
        }

        #region IValueReader Members

        /// <summary>
        /// Gets if this IValueReader supports using the name field to look up values. If false, values will have to
        /// be read back in the same order they were written and the name field will be ignored.
        /// </summary>
        public bool SupportsNameLookup
        {
            get { return false; }
        }

        /// <summary>
        /// Gets if this IValueReader supports reading nodes. If false, any attempt to use nodes in this IValueReader
        /// will result in a NotSupportedException being thrown.
        /// </summary>
        public bool SupportsNodes
        {
            get { return true; }
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
        /// <param name="name">Unused by the <see cref="BinaryValueReader"/>.</param>
        /// <returns>Value read from the reader.</returns>
        public bool ReadBool(string name)
        {
            return _reader.ReadBool();
        }

        /// <summary>
        /// Reads a 8-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueReader"/>.</param>
        /// <returns>Value read from the reader.</returns>
        public byte ReadByte(string name)
        {
            return _reader.ReadByte();
        }

        /// <summary>
        /// Reads a 64-bit floating-point number.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public double ReadDouble(string name)
        {
            return _reader.ReadDouble();
        }

        /// <summary>
        /// Reads an Enum of type <typeparamref name="T"/>. Whether to use the Enum's underlying integer value or the
        /// name of the Enum value is determined from the <see cref="UseEnumNames"/> property.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public T ReadEnum<T>(string name) where T : struct, IComparable, IConvertible, IFormattable
        {
            if (UseEnumNames)
                return ReadEnumName<T>(name);
            else
                return ReadEnumValue<T>(name);
        }

        /// <summary>
        /// Reads an Enum of type <typeparamref name="T"/> using the Enum's name instead of the value.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public T ReadEnumName<T>(string name) where T : struct, IComparable, IConvertible, IFormattable
        {
            return EnumHelper<T>.ReadName(this, name);
        }

        /// <summary>
        /// Reads an Enum of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public T ReadEnumValue<T>(string name) where T : struct, IComparable, IConvertible, IFormattable
        {
            return EnumHelper<T>.ReadValue(this, name);
        }

        /// <summary>
        /// Reads a 32-bit floating-point number.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueReader"/>.</param>
        /// <returns>Value read from the reader.</returns>
        public float ReadFloat(string name)
        {
            return _reader.ReadFloat();
        }

        /// <summary>
        /// Reads a 32-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueReader"/>.</param>
        /// <returns>Value read from the reader.</returns>
        public int ReadInt(string name)
        {
            return _reader.ReadInt();
        }

        /// <summary>
        /// Reads a signed integer of up to 32 bits.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueReader"/>.</param>
        /// <param name="bits">Number of bits to read.</param>
        /// <returns>Value read from the reader.</returns>
        public int ReadInt(string name, int bits)
        {
            return _reader.ReadInt(bits);
        }

        /// <summary>
        /// Reads a 64-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueReader"/>.</param>
        /// <returns>Value read from the reader.</returns>
        public long ReadLong(string name)
        {
            return _reader.ReadLong();
        }

        /// <summary>
        /// Reads multiple values that were written with WriteMany.
        /// </summary>
        /// <typeparam name="T">The Type of value to read.</typeparam>
        /// <param name="nodeName">Unused by the <see cref="BinaryValueReader"/>.</param>
        /// <param name="readHandler">Delegate that reads the values from the IValueReader.</param>
        /// <returns>Array of the values read the IValueReader.</returns>
        public T[] ReadMany<T>(string nodeName, ReadManyHandler<T> readHandler)
        {
            var nodeReader = ReadNode(null);
            var count = nodeReader.ReadInt(null);

            var ret = new T[count];
            for (var i = 0; i < count; i++)
            {
                ret[i] = readHandler(nodeReader, null);
            }

            return ret;
        }

        /// <summary>
        /// Reads multiple nodes that were written with WriteMany.
        /// </summary>
        /// <typeparam name="T">The Type of nodes to read.</typeparam>
        /// <param name="nodeName">Unused by the <see cref="BinaryValueReader"/>.</param>
        /// <param name="readHandler">Delegate that reads the values from the IValueReader.</param>
        /// <returns>Array of the values read the IValueReader.</returns>
        public T[] ReadManyNodes<T>(string nodeName, ReadManyNodesHandler<T> readHandler)
        {
            var nodeReader = ReadNode(nodeName);
            var count = nodeReader.ReadInt(null);

            var ret = new T[count];
            for (var i = 0; i < count; i++)
            {
                var childNodeReader = nodeReader.ReadNode(null);
                ret[i] = readHandler(childNodeReader);
            }

            return ret;
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
            return ReadManyNodes(nodeName, readHandler);
        }

        /// <summary>
        /// Reads a single child node, while enforcing the idea that there should only be one node
        /// in the key. If there is more than one node for the given <paramref name="key"/>, an
        /// ArgumentException will be thrown.
        /// </summary>
        /// <param name="key">Unused by the <see cref="BinaryValueReader"/>.</param>
        /// <returns>An IValueReader to read the child node.</returns>
        /// <exception cref="ArgumentException">Zero or more than one values found for the given
        /// <paramref name="key"/>.</exception>
        public IValueReader ReadNode(string key)
        {
            return ReadNode();
        }

        /// <summary>
        /// Reads one or more child nodes from the IValueReader.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueReader"/>.</param>
        /// <param name="count">The number of nodes to read. Must be greater than 0. An ArgumentOutOfRangeException will
        /// be thrown if this value exceeds the actual number of nodes available.</param>
        /// <returns>An IEnumerable of IValueReaders used to read the nodes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Count is less than 0.</exception>
        public IEnumerable<IValueReader> ReadNodes(string name, int count)
        {
            if (count == 0)
                return Enumerable.Empty<IValueReader>();

            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            var ret = new IValueReader[count];
            for (var i = 0; i < count; i++)
            {
                ret[i] = ReadNode();
            }

            return ret;
        }

        /// <summary>
        /// Reads a 8-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueReader"/>.</param>
        /// <returns>Value read from the reader.</returns>
        public sbyte ReadSByte(string name)
        {
            return _reader.ReadSByte();
        }

        /// <summary>
        /// Reads a 16-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueReader"/>.</param>
        /// <returns>Value read from the reader.</returns>
        public short ReadShort(string name)
        {
            return _reader.ReadShort();
        }

        /// <summary>
        /// Reads a variable-length string of up to 65535 characters in length.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueReader"/>.</param>
        /// <returns>String read from the reader.</returns>
        public string ReadString(string name)
        {
            return _reader.ReadString();
        }

        /// <summary>
        /// Reads an unsigned integer of up to 32 bits.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueReader"/>.</param>
        /// <param name="bits">Number of bits to read.</param>
        /// <returns>Value read from the reader.</returns>
        public uint ReadUInt(string name, int bits)
        {
            return _reader.ReadUInt(bits);
        }

        /// <summary>
        /// Reads a 32-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueReader"/>.</param>
        /// <returns>Value read from the reader.</returns>
        public uint ReadUInt(string name)
        {
            return _reader.ReadUInt();
        }

        /// <summary>
        /// Reads a 64-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueReader"/>.</param>
        /// <returns>Value read from the reader.</returns>
        public ulong ReadULong(string name)
        {
            return _reader.ReadULong();
        }

        /// <summary>
        /// Reads a 16-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueReader"/>.</param>
        /// <returns>Value read from the reader.</returns>
        public ushort ReadUShort(string name)
        {
            return _reader.ReadUShort();
        }

        #endregion
    }
}