using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

// NOTE: This class won't work if you forget to call Dispose. Would be nice to use a destructor to fix that.

namespace NetGore.IO
{
    /// <summary>
    /// Implementation of the <see cref="IValueWriter"/> using Xml.
    /// </summary>
    public class XmlValueWriter : IValueWriter
    {
        readonly bool _disposeWriter;
        readonly Stack<string> _nodeStack = new Stack<string>(4);
        readonly bool _useEnumNames = true;
        readonly XmlWriter _writer;
        bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlValueWriter"/> class.
        /// </summary>
        /// <param name="filePath">The path to the file to write to.</param>
        /// <param name="nodeName">Name to give the root node containing the values.</param>
        public XmlValueWriter(string filePath, string nodeName) : this(filePath, nodeName, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlValueWriter"/> class.
        /// </summary>
        /// <param name="filePath">The path to the file to write to.</param>
        /// <param name="nodeName">Name to give the root node containing the values.</param>
        /// <param name="useEnumNames">If true, Enums I/O will be done using the Enum's name. If false,
        /// Enum I/O will use the underlying integer value of the Enum.</param>
        public XmlValueWriter(string filePath, string nodeName, bool useEnumNames)
        {
            _useEnumNames = useEnumNames;
            _disposeWriter = true;

            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            _writer = XmlWriter.Create(filePath, new XmlWriterSettings { Indent = true });

            if (_writer == null)
                throw new ArgumentException("filePath");

            _writer.WriteStartDocument();
            _writer.WriteStartElement(nodeName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlValueWriter"/> class.
        /// </summary>
        /// <param name="writer">XmlWriter to write the values to.</param>
        /// <param name="nodeName">Name to give the root node containing the values.</param>
        public XmlValueWriter(XmlWriter writer, string nodeName) : this(writer, nodeName, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlValueWriter"/> class.
        /// </summary>
        /// <param name="writer">XmlWriter to write the values to.</param>
        /// <param name="nodeName">Name to give the root node containing the values.</param>
        /// <param name="useEnumNames">If true, Enums I/O will be done using the Enum's name. If false,
        /// Enum I/O will use the underlying integer value of the Enum.</param>
        public XmlValueWriter(XmlWriter writer, string nodeName, bool useEnumNames)
        {
            _useEnumNames = useEnumNames;

            if (writer == null)
                throw new ArgumentNullException("writer");

            _writer = writer;
            _writer.WriteStartElement(nodeName);
        }

        /// <summary>
        /// XmlValueWriter constructor.
        /// </summary>
        /// <param name="writer">XmlWriter to write the values to.</param>
        /// <param name="nodeName">Name to give the root node containing the values.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="attributeValue">Value of the attribute.</param>
        public XmlValueWriter(XmlWriter writer, string nodeName, string attributeName, string attributeValue)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
            if (string.IsNullOrEmpty(attributeName))
                throw new ArgumentNullException("attributeName");
            if (string.IsNullOrEmpty(attributeValue))
                throw new ArgumentNullException("attributeValue");

            _writer = writer;
            _writer.WriteStartElement(nodeName);
            _writer.WriteAttributeString(attributeName, attributeValue);
        }

        #region IValueWriter Members

        /// <summary>
        /// Writes a 64-bit floating-point number.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, double value)
        {
            Write(name, Parser.Invariant.ToString(value));
        }

        /// <summary>
        /// Writes a variable-length string of up to 65535 characters in length.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">String to write.</param>
        public void Write(string name, string value)
        {
            _writer.WriteElementString(name, value);
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
        /// Gets if this IValueWriter supports using the name field to look up values. If false, values will have to
        /// be read back in the same order they were written and the name field will be ignored.
        /// </summary>
        public bool SupportsNameLookup
        {
            get { return true; }
        }

        /// <summary>
        /// Gets if this IValueWriter supports reading nodes. If false, any attempt to use nodes in this IValueWriter
        /// will result in a NotSupportedException being thrown.
        /// </summary>
        public bool SupportsNodes
        {
            get { return true; }
        }

        /// <summary>
        /// Writes the start of a child node in this IValueWriter.
        /// </summary>
        /// <param name="name">Name of the child node.</param>
        public void WriteStartNode(string name)
        {
            _nodeStack.Push(name);
            _writer.WriteStartElement(name);
        }

        /// <summary>
        /// Writes the end of a child node in this IValueWriter.
        /// </summary>
        /// <param name="name">Name of the child node.</param>
        public void WriteEndNode(string name)
        {
            if (_nodeStack.Count == 0)
            {
                const string errmsg = "Already at the root node.";
                throw new ArgumentException(errmsg);
            }

            string expectedName = _nodeStack.Pop();
            if (name != expectedName)
            {
                const string errmsg = "Node name `{0}` does not match the expected name `{1}`.";
                throw new ArgumentException(string.Format(errmsg, name, expectedName), "name");
            }

            _writer.WriteEndElement();
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
            int count;
            if (values != null)
                count = values.Count();
            else
                count = 0;

            WriteStartNode(nodeName);
            {
                Write(XmlValueHelper.CountValueKey, count);
                if (values != null && count > 0)
                {
                    int i = 0;
                    foreach (T value in values)
                    {
                        writeHandler(XmlValueHelper.GetItemKey(i), value);
                        i++;
                    }
                }
            }
            WriteEndNode(nodeName);
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
            int count;
            if (values != null)
                count = values.Count();
            else
                count = 0;

            WriteStartNode(nodeName);
            {
                Write("Count", count);
                if (values != null && count > 0)
                {
                    int i = 0;
                    foreach (T value in values)
                    {
                        string childNodeName = XmlValueHelper.GetItemKey(i);
                        WriteStartNode(childNodeName);
                        writeHandler(this, value);
                        WriteEndNode(childNodeName);
                        i++;
                    }
                }
            }
            WriteEndNode(nodeName);
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
            int count;
            if (values != null)
                count = values.Length;
            else
                count = 0;

            WriteStartNode(nodeName);
            {
                Write(XmlValueHelper.CountValueKey, count);
                if (values != null && count > 0)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        string childNodeName = XmlValueHelper.GetItemKey(i);
                        WriteStartNode(childNodeName);
                        writeHandler(this, values[i]);
                        WriteEndNode(childNodeName);
                    }
                }
            }
            WriteEndNode(nodeName);
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
            int count;
            if (values != null)
                count = values.Length;
            else
                count = 0;

            WriteStartNode(nodeName);
            {
                Write(XmlValueHelper.CountValueKey, count);
                if (values != null && count > 0)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        writeHandler(XmlValueHelper.GetItemKey(i), values[i]);
                    }
                }
            }
            WriteEndNode(nodeName);
        }

        /// <summary>
        /// Writes a 32-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, int value)
        {
            Write(name, Parser.Invariant.ToString(value));
        }

        /// <summary>
        /// Writes a 64-bit usigned integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, ulong value)
        {
            Write(name, Parser.Invariant.ToString(value));
        }

        /// <summary>
        /// Writes a 64-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, long value)
        {
            Write(name, Parser.Invariant.ToString(value));
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
            Write(name, value);
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
            Write(name, value);
        }

        /// <summary>
        /// Writes an Enum of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="writer">The writer used to write the enum value.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void WriteEnumValue<T>(IEnumValueWriter<T> writer, string name, T value)
            where T : struct, IComparable, IConvertible, IFormattable
        {
            writer.WriteEnum(this, name, value);
        }

        /// <summary>
        /// Writes an Enum of type <typeparamref name="T"/>. Whether to use the Enum's underlying integer value or
        /// the name of the Enum value is determined from the <see cref="IValueWriter.UseEnumNames"/> property.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="writer">The writer used to write the enum value.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void WriteEnum<T>(IEnumValueWriter<T> writer, string name, T value)
            where T : struct, IComparable, IConvertible, IFormattable
        {
            EnumIOHelper<T>.WriteEnum(this, writer, name, value);
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
            string str = EnumIOHelper<T>.ToName(value);
            Write(name, str);
        }

        /// <summary>
        /// Writes a boolean.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, bool value)
        {
            Write(name, Parser.Invariant.ToString(value));
        }

        /// <summary>
        /// Writes a 32-bit floating-point number.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, float value)
        {
            Write(name, Parser.Invariant.ToString(value));
        }

        /// <summary>
        /// Writes a 32-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, uint value)
        {
            Write(name, Parser.Invariant.ToString(value));
        }

        /// <summary>
        /// Writes a 16-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, short value)
        {
            Write(name, Parser.Invariant.ToString(value));
        }

        /// <summary>
        /// Writes a 16-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, ushort value)
        {
            Write(name, Parser.Invariant.ToString(value));
        }

        /// <summary>
        /// Writes a 8-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, byte value)
        {
            Write(name, Parser.Invariant.ToString(value));
        }

        /// <summary>
        /// Writes a 8-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, sbyte value)
        {
            Write(name, Parser.Invariant.ToString(value));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (_disposed)
                throw new MemberAccessException("Object already disposed!");

            _disposed = true;
            _writer.WriteEndElement();

            if (_disposeWriter)
            {
                _writer.WriteEndDocument();
                _writer.Close();
            }
        }

        #endregion
    }
}