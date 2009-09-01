using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// Delegate for reading many items.
    /// </summary>
    /// <typeparam name="T">The Type of value.</typeparam>
    /// <param name="r">IValueReader to read from.</param>
    /// <param name="name">The item to read.</param>
    /// <returns>The value read from the IValueReader <paramref name="r"/>.</returns>
    public delegate T ReadManyHandler<T>(IValueReader r, string name);

    /// <summary>
    /// Delegate for reading many nodes.
    /// </summary>
    /// <typeparam name="T">The Type of node.</typeparam>
    /// <param name="r">IValueReader to read from.</param>
    /// <returns>The node read from the IValueReader <paramref name="r"/>.</returns>
    public delegate T ReadManyNodesHandler<T>(IValueReader r);

    /// <summary>
    /// Interface for an object that reads values written by an IValueReader.
    /// </summary>
    public interface IValueReader
    {
        /// <summary>
        /// Gets if this IValueReader supports using the name field to look up values. If false, values will have to
        /// be read back in the same order they were written and the name field will be ignored.
        /// </summary>
        bool SupportsNameLookup { get; }

        /// <summary>
        /// Gets if this IValueReader supports reading nodes. If false, any attempt to use nodes in this IValueReader
        /// will result in a NotSupportedException being thrown.
        /// </summary>
        bool SupportsNodes { get; }

        /// <summary>
        /// Reads a boolean.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        bool ReadBool(string name);

        /// <summary>
        /// Reads a 8-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        byte ReadByte(string name);

        /// <summary>
        /// Reads a 32-bit floating-point number.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        float ReadFloat(string name);

        /// <summary>
        /// Reads a 64-bit floating-point number.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        double ReadDouble(string name);

        /// <summary>
        /// Reads a 32-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        int ReadInt(string name);

        /// <summary>
        /// Reads a 64-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        long ReadLong(string name);

        /// <summary>
        /// Reads a 64-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        ulong ReadULong(string name);

        /// <summary>
        /// Reads a signed integer of up to 32 bits.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <param name="bits">Number of bits to read.</param>
        /// <returns>Value read from the reader.</returns>
        int ReadInt(string name, int bits);

        /// <summary>
        /// Reads multiple values that were written with WriteMany.
        /// </summary>
        /// <typeparam name="T">The Type of value to read.</typeparam>
        /// <param name="nodeName">The name of the node containing the values.</param>
        /// <param name="readHandler">Delegate that reads the values from the IValueReader.</param>
        /// <returns>Array of the values read the IValueReader.</returns>
        T[] ReadMany<T>(string nodeName, ReadManyHandler<T> readHandler);

        /// <summary>
        /// Reads multiple nodes that were written with WriteMany.
        /// </summary>
        /// <typeparam name="T">The Type of nodes to read.</typeparam>
        /// <param name="nodeName">The name of the root node containing the values.</param>
        /// <param name="readHandler">Delegate that reads the values from the IValueReader.</param>
        /// <returns>Array of the values read the IValueReader.</returns>
        T[] ReadManyNodes<T>(string nodeName, ReadManyNodesHandler<T> readHandler);

        /// <summary>
        /// Reads a single child node, while enforcing the idea that there should only be one node
        /// in the key. If there is more than one node for the given <paramref name="key"/>, an
        /// ArgumentException will be thrown.
        /// </summary>
        /// <param name="key">The key of the child node to read.</param>
        /// <returns>An IValueReader to read the child node.</returns>
        /// <exception cref="ArgumentException">Zero or more than one values found for the given
        /// <paramref name="key"/>.</exception>
        IValueReader ReadNode(string key);

        /// <summary>
        /// Reads one or more child nodes from the IValueReader.
        /// </summary>
        /// <param name="name">Name of the nodes to read.</param>
        /// <param name="count">The number of nodes to read. If this value is 0, an empty IEnumerable of IValueReaders
        /// will be returned, even if the key could not be found.</param>
        /// <returns>An IEnumerable of IValueReaders used to read the nodes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Count is less than 0.</exception>
        IEnumerable<IValueReader> ReadNodes(string name, int count);

        /// <summary>
        /// Reads a 8-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        sbyte ReadSByte(string name);

        /// <summary>
        /// Reads a 16-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        short ReadShort(string name);

        /// <summary>
        /// Reads a variable-length string of up to 65535 characters in length.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>String read from the reader.</returns>
        string ReadString(string name);

        /// <summary>
        /// Reads a 32-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        uint ReadUInt(string name);

        /// <summary>
        /// Reads an unsigned integer of up to 32 bits.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <param name="bits">Number of bits to read.</param>
        /// <returns>Value read from the reader.</returns>
        uint ReadUInt(string name, int bits);

        /// <summary>
        /// Reads a 16-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        ushort ReadUShort(string name);
    }
}