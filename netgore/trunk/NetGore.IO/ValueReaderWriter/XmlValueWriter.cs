using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;

namespace NetGore.IO
{
    /// <summary>
    /// Implementation of the INamedValueWriter using Xml.
    /// </summary>
    public class XmlValueWriter : IValueWriter
    {
        readonly XmlWriter _writer;
        bool _disposed;

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
        /// Writes a 32-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, int value)
        {
            Write(name, value.ToString());
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
            Write(name, value.ToString());
        }

        /// <summary>
        /// Writes a 32-bit floating-point number.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, float value)
        {
            Write(name, value.ToString());
        }

        /// <summary>
        /// Writes a 32-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, uint value)
        {
            Write(name, value.ToString());
        }

        /// <summary>
        /// Writes a 16-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, short value)
        {
            Write(name, value.ToString());
        }

        /// <summary>
        /// Writes a 16-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, ushort value)
        {
            Write(name, value.ToString());
        }

        /// <summary>
        /// Writes a 8-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, byte value)
        {
            Write(name, value.ToString());
        }

        /// <summary>
        /// Writes a 8-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, sbyte value)
        {
            Write(name, value.ToString());
        }

        public void Dispose()
        {
            if (_disposed)
                throw new MemberAccessException("Object already disposed!");

            _disposed = true;
            _writer.WriteEndElement();
        }

        #endregion
    }
}