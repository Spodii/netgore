using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// An implementation of an <see cref="IValueWriter"/> that allows you to specify during the construction of the object
    /// what format to use instead of specifying a specific <see cref="IValueReader"/> and <see cref="IValueWriter"/> directly.
    /// The results are the exact same as if the underlying <see cref="IValueReader"/> and <see cref="IValueWriter"/> are
    /// used directly. Because of this, you can, for example, use a <see cref="GenericValueReader"/> to read a file written with an
    /// <see cref="XmlValueWriter"/> as long as the <see cref="GenericValueReader"/> can recognize the format. However, it is
    /// recommended that you always use a <see cref="GenericValueReader"/> and <see cref="GenericValueWriter"/> directly when
    /// possible to avoid any issues with format recognition and provide easier alteration of the used formats in the future.
    /// </summary>
    public class GenericValueWriter : IValueWriter
    {
        readonly IValueWriter _writer;

        /// <summary>
        /// Initializes the <see cref="GenericValueWriter"/> class.
        /// </summary>
        static GenericValueWriter()
        {
            DefaultFormat = GenericValueIOFormat.Xml;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericValueReader"/> class.
        /// </summary>
        /// <param name="filePath">The path to the file to load.</param>
        /// <param name="rootNodeName">The name of the root node. Not used by all formats, but should always be included anyways.</param>
        /// <param name="format">The <see cref="GenericValueIOFormat"/> that defines what output format to use. If null,
        /// <see cref="GenericValueWriter.DefaultFormat"/> will be used.</param>
        /// <param name="useEnumNames">Whether or not enum names should be used. If true, enum names will always be used. If false, the
        /// enum values will be used instead. If null, the default value for the underlying <see cref="IValueWriter"/> will be used.</param>
        /// <exception cref="ArgumentNullException"><paramref name="filePath"/> is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="format"/> contains an invalid value.</exception>
        GenericValueWriter(string filePath, string rootNodeName, GenericValueIOFormat? format = null, bool? useEnumNames = null)
        {
            // Get the encoding format to use
            GenericValueIOFormat formatToUse;
            if (!format.HasValue)
                formatToUse = DefaultFormat;
            else
                formatToUse = format.Value;

            // Create the IValueWriter of the needed type
            switch (formatToUse)
            {
                case GenericValueIOFormat.Binary:
                    if (useEnumNames.HasValue)
                        _writer = BinaryValueWriter.Create(filePath, useEnumNames.Value);
                    else
                        _writer = BinaryValueWriter.Create(filePath);
                    break;

                case GenericValueIOFormat.Xml:
                    if (useEnumNames.HasValue)
                        _writer = XmlValueWriter.Create(filePath, rootNodeName, useEnumNames.Value);
                    else
                        _writer = XmlValueWriter.Create(filePath, rootNodeName);
                    break;

                default:
                    const string errmsg = "Invalid GenericValueIOFormat value `{0}`.";
                    Debug.Fail(string.Format(errmsg, format));
                    throw new ArgumentOutOfRangeException("format", string.Format(errmsg, format));
            }

            Debug.Assert(_writer != null);
        }

        /// <summary>
        /// Gets or sets the <see cref="GenericValueIOFormat"/> to use for writing. By default, this value is equal to
        /// <see cref="GenericValueIOFormat.Xml"/>.
        /// </summary>
        public static GenericValueIOFormat DefaultFormat { get; set; }

        /// <summary>
        /// Creates a <see cref="GenericValueReader"/>.
        /// </summary>
        /// <param name="filePath">The path to the file to load.</param>
        /// <param name="rootNodeName">The name of the root node. Not used by all formats, but should always be included anyways.</param>
        /// <param name="format">The <see cref="GenericValueIOFormat"/> that defines what output format to use. If null,
        /// <see cref="GenericValueWriter.DefaultFormat"/> will be used.</param>
        /// <param name="useEnumNames">Whether or not enum names should be used. If true, enum names will always be used. If false, the
        /// enum values will be used instead. If null, the default value for the underlying <see cref="IValueWriter"/> will be used.</param>
        /// <returns>The <see cref="GenericValueReader"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="filePath"/> is null or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="format"/> contains an invalid value.</exception>
        public static GenericValueWriter Create(string filePath, string rootNodeName, GenericValueIOFormat? format = null,
                                                bool? useEnumNames = null)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("filePath");

            return new GenericValueWriter(filePath, rootNodeName, format, useEnumNames);
        }

        #region IValueWriter Members

        /// <summary>
        /// Gets if this <see cref="IValueWriter"/> supports using the name field to look up values. If false,
        /// values will have to be read back in the same order they were written and the name field will be ignored.
        /// </summary>
        public bool SupportsNameLookup
        {
            get { return _writer.SupportsNameLookup; }
        }

        /// <summary>
        /// Gets if this <see cref="IValueWriter"/> supports reading nodes. If false, any attempt to use nodes
        /// in this IValueWriter will result in a NotSupportedException being thrown.
        /// </summary>
        public bool SupportsNodes
        {
            get { return _writer.SupportsNodes; }
        }

        /// <summary>
        /// Gets if Enum I/O will be done with the Enum's name. If true, the name of the Enum value instead of the
        /// underlying integer value will be used. If false, the underlying integer value will be used. This
        /// only to Enum I/O that does not explicitly state which method to use.
        /// </summary>
        public bool UseEnumNames
        {
            get { return _writer.UseEnumNames; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _writer.Dispose();
        }

        /// <summary>
        /// Writes a 32-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, int value)
        {
            _writer.Write(name, value);
        }

        /// <summary>
        /// Writes a 64-bit usigned integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, ulong value)
        {
            _writer.Write(name, value);
        }

        /// <summary>
        /// Writes a 64-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, long value)
        {
            _writer.Write(name, value);
        }

        /// <summary>
        /// Writes a signed integer of up to 32 bits.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        /// <param name="bits">Number of bits to write.</param>
        public void Write(string name, int value, int bits)
        {
            _writer.Write(name, value, bits);
        }

        /// <summary>
        /// Writes an unsigned integer of up to 32 bits.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        /// <param name="bits">Number of bits to write.</param>
        public void Write(string name, uint value, int bits)
        {
            _writer.Write(name, value, bits);
        }

        /// <summary>
        /// Writes a boolean.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, bool value)
        {
            _writer.Write(name, value);
        }

        /// <summary>
        /// Writes a 32-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, uint value)
        {
            _writer.Write(name, value);
        }

        /// <summary>
        /// Writes a 16-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, short value)
        {
            _writer.Write(name, value);
        }

        /// <summary>
        /// Writes a 16-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, ushort value)
        {
            _writer.Write(name, value);
        }

        /// <summary>
        /// Writes a 8-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, byte value)
        {
            _writer.Write(name, value);
        }

        /// <summary>
        /// Writes a 8-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, sbyte value)
        {
            _writer.Write(name, value);
        }

        /// <summary>
        /// Writes a 32-bit floating-point number.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, float value)
        {
            _writer.Write(name, value);
        }

        /// <summary>
        /// Writes a 64-bit floating-point number.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, double value)
        {
            _writer.Write(name, value);
        }

        /// <summary>
        /// Writes a variable-length string of up to 65535 characters in length.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">String to write.</param>
        public void Write(string name, string value)
        {
            _writer.Write(name, value);
        }

        /// <summary>
        /// Writes the end of a child node in this IValueWriter.
        /// </summary>
        /// <param name="name">Name of the child node.</param>
        /// <exception cref="InvalidOperationException">Already at the root node.</exception>
        public void WriteEndNode(string name)
        {
            _writer.WriteEndNode(name);
        }

        /// <summary>
        /// Writes an Enum of type <typeparamref name="T"/>. Whether to use the Enum's underlying integer value or
        /// the name of the Enum value is determined from the <see cref="IValueWriter.UseEnumNames"/> property.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void WriteEnum<T>(string name, T value) where T : struct, IComparable, IConvertible, IFormattable
        {
            _writer.WriteEnum(name, value);
        }

        /// <summary>
        /// Writes an Enum of type <typeparamref name="T"/> using the name of the Enum instead of the value.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void WriteEnumName<T>(string name, T value) where T : struct, IComparable, IConvertible, IFormattable
        {
            _writer.WriteEnumName(name, value);
        }

        /// <summary>
        /// Writes an Enum of type <typeparamref name="T"/> using the value of the Enum instead of the name.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void WriteEnumValue<T>(string name, T value) where T : struct, IComparable, IConvertible, IFormattable
        {
            _writer.WriteEnumValue(name, value);
        }

        /// <summary>
        /// Writes multiple values of the same type to the IValueWriter all under the same node name.
        /// Ordering is not guarenteed.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="nodeName">Name of the node that will contain the values.</param>
        /// <param name="values">IEnumerable of values to write. If this value is null, it will be treated
        /// the same as if it were an empty IEnumerable.</param>
        /// <param name="writeHandler">Delegate that writes the value to the IValueWriter.</param>
        public void WriteMany<T>(string nodeName, IEnumerable<T> values, WriteManyHandler<T> writeHandler)
        {
            _writer.WriteMany(nodeName, values, writeHandler);
        }

        /// <summary>
        /// Writes multiple values of the same type to the IValueWriter all under the same node name.
        /// Unlike the WriteMany for IEnumerables, this guarentees that ordering will be preserved.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="nodeName">Name of the node that will contain the values.</param>
        /// <param name="values">Array of values to write. If this value is null, it will be treated
        /// the same as if it were an empty array.</param>
        /// <param name="writeHandler">Delegate that writes the value to the IValueWriter.</param>
        public void WriteMany<T>(string nodeName, T[] values, WriteManyHandler<T> writeHandler)
        {
            _writer.WriteMany(nodeName, values, writeHandler);
        }

        /// <summary>
        /// Writes multiple values of type <typeparamref name="T"/>, where each value will result in its own
        /// node being created. Ordering is not guarenteed.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="nodeName">Name of the node that will contain the values.</param>
        /// <param name="values">IEnumerable of values to write. If this value is null, it will be treated
        /// the same as if it were an empty IEnumerable.</param>
        /// <param name="writeHandler">Delegate that writes the value to the IValueWriter.</param>
        public void WriteManyNodes<T>(string nodeName, IEnumerable<T> values, WriteManyNodesHandler<T> writeHandler)
        {
            _writer.WriteManyNodes(nodeName, values, writeHandler);
        }

        /// <summary>
        /// Writes multiple values of type <typeparamref name="T"/>, where each value will result in its own
        /// node being created. Unlike the WriteMany for IEnumerables, this guarentees that ordering will be preserved.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="nodeName">Name of the node that will contain the values.</param>
        /// <param name="values">IEnumerable of values to write. If this value is null, it will be treated
        /// the same as if it were an empty IEnumerable.</param>
        /// <param name="writeHandler">Delegate that writes the value to the IValueWriter.</param>
        public void WriteManyNodes<T>(string nodeName, T[] values, WriteManyNodesHandler<T> writeHandler)
        {
            _writer.WriteManyNodes(nodeName, values, writeHandler);
        }

        /// <summary>
        /// Writes the start of a child node in this IValueWriter.
        /// </summary>
        /// <param name="name">Name of the child node.</param>
        public void WriteStartNode(string name)
        {
            _writer.WriteStartNode(name);
        }

        #endregion
    }
}