using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using NetGore.Globalization;

// NOTE: This class won't work if you forget to call Dispose. Would be nice to use a destructor to fix that.

namespace NetGore.IO
{
    /// <summary>
    /// Implementation of the INamedValueWriter using Xml.
    /// </summary>
    public class XmlValueWriter : IValueWriter
    {
        readonly bool _disposeWriter;
        readonly Stack<string> _nodeStack = new Stack<string>(4);
        readonly XmlWriter _writer;
        bool _disposed;

        /// <summary>
        /// XmlValueWriter constructor.
        /// </summary>
        /// <param name="filePath">The path to the file to write to.</param>
        /// <param name="nodeName">Name to give the root node containing the values.</param>
        public XmlValueWriter(string filePath, string nodeName)
        {
            _disposeWriter = true;
            _writer = XmlWriter.Create(filePath, new XmlWriterSettings
            {
                Indent = true
            });

            if (_writer == null)
                throw new ArgumentException("filePath");

            _writer.WriteStartDocument();
            _writer.WriteStartElement(nodeName);
        }

        /// <summary>
        /// XmlValueWriter constructor.
        /// </summary>
        /// <param name="writer">XmlWriter to write the values to.</param>
        /// <param name="nodeName">Name to give the root node containing the values.</param>
        public XmlValueWriter(XmlWriter writer, string nodeName)
        {
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
        /// Gets if this IValueReader supports using the name field to look up values. If false, values will have to
        /// be read back in the same order they were written and the name field will be ignored.
        /// </summary>
        public bool SupportsNameLookup
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
        /// <filterpriority>2</filterpriority>
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