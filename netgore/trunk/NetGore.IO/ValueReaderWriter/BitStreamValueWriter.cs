using System;
using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// Implementation of the IValueWriter using a BitStream.
    /// </summary>
    public class BitStreamValueWriter : IValueWriter
    {
        readonly BitStream _writer;

        /// <summary>
        /// BitStreamValueReader constructor.
        /// </summary>
        /// <param name="writer">BitStream that will be written to.</param>
        public BitStreamValueWriter(BitStream writer)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");
            if (writer.Mode != BitStreamMode.Write)
                throw new ArgumentException("The BitStream must be set to Write.", "writer");

            _writer = writer;
        }

        #region IValueWriter Members

        /// <summary>
        /// Writes an unsigned integer of up to 32 bits.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        /// <param name="bits">Number of bits to write.</param>
        public void Write(string name, uint value, int bits)
        {
            _writer.Write(value, bits);
        }

        /// <summary>
        /// Writes a boolean.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, bool value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Writes a 32-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the BitStreamValueWriter.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, uint value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Writes a 16-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the BitStreamValueWriter.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, short value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Writes a 16-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the BitStreamValueWriter.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, ushort value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Writes a 8-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the BitStreamValueWriter.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, byte value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Writes a 8-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the BitStreamValueWriter.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, sbyte value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Writes a variable-length string of up to 65535 characters in length.
        /// </summary>
        /// <param name="name">Unused by the BitStreamValueWriter.</param>
        /// <param name="value">String to write.</param>
        public void Write(string name, string value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Gets if this IValueReader supports using the name field to look up values. If false, values will have to
        /// be read back in the same order they were written and the name field will be ignored.
        /// </summary>
        public bool SupportsNameLookup
        {
            get { return false; }
        }

        /// <summary>
        /// Writes a 32-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the BitStreamValueWriter.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, int value)
        {
            _writer.Write(value);
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
            _writer.Write(value, bits);
        }

        /// <summary>
        /// Writes a 32-bit floating-point number.
        /// </summary>
        /// <param name="name">Unused by the BitStreamValueWriter.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, float value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
        }

        #endregion
    }
}