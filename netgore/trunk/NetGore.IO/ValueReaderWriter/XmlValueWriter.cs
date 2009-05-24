using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace NetGore.IO
{
    /// <summary>
    /// Implementation of the INamedValueWriter using Xml.
    /// </summary>
    public class XmlValueWriter : INamedValueWriter
    {
        readonly XmlWriter _writer;

        /// <summary>
        /// XmlValueWriter constructor.
        /// </summary>
        /// <param name="writer">XmlWriter to write the values to.</param>
        public XmlValueWriter(XmlWriter writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            _writer = writer;
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
    }
}
