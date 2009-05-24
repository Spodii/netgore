using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.IO.ValueReaderWriter
{
    /// <summary>
    /// Implementation of the IValueReader using a BitStream.
    /// </summary>
    public class BitStreamValueReader : IValueReader
    {
        readonly BitStream _reader;

        /// <summary>
        /// BitStreamValueReader constructor.
        /// </summary>
        /// <param name="reader">BitStream that will be used to read from.</param>
        public BitStreamValueReader(BitStream reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
            if (reader.Mode != BitStreamMode.Read)
                throw new ArgumentException("The BitStream must be set to Read.", "reader");

            _reader = reader;
        }

        /// <summary>
        /// Reads a 8-bit unsigned integer.
        /// </summary>
        /// <returns>Value read from the reader.</returns>
        public byte ReadByte()
        {
            return _reader.ReadByte();
        }

        /// <summary>
        /// Reads a 32-bit floating-point number.
        /// </summary>
        /// <returns>Value read from the reader.</returns>
        public float ReadFloat()
        {
            return _reader.ReadFloat();
        }

        /// <summary>
        /// Reads a 32-bit signed integer.
        /// </summary>
        /// <returns>Value read from the reader.</returns>
        public int ReadInt()
        {
            return _reader.ReadInt();
        }

        /// <summary>
        /// Reads a 8-bit signed integer.
        /// </summary>
        /// <returns>Value read from the reader.</returns>
        public sbyte ReadSByte()
        {
            return _reader.ReadSByte();
        }

        /// <summary>
        /// Reads a 16-bit signed integer.
        /// </summary>
        /// <returns>Value read from the reader.</returns>
        public short ReadShort()
        {
            return _reader.ReadShort();
        }

        /// <summary>
        /// Reads a variable-length string of up to 65535 characters in length.
        /// </summary>
        /// <returns>String read from the reader.</returns>
        public string ReadString()
        {
            return _reader.ReadString();
        }

        /// <summary>
        /// Reads a 32-bit unsigned integer.
        /// </summary>
        /// <returns>Value read from the reader.</returns>
        public uint ReadUInt()
        {
            return _reader.ReadUInt();
        }

        /// <summary>
        /// Reads a 16-bit unsigned integer.
        /// </summary>
        /// <returns>Value read from the reader.</returns>
        public ushort ReadUShort()
        {
            return _reader.ReadUShort();
        }
    }
}
