using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.IO
{
    /// <summary>
    /// Interface for an object that reads values written by an IValueReader.
    /// </summary>
    public interface IValueReader
    {
        /// <summary>
        /// Reads a 32-bit signed integer.
        /// </summary>
        /// <returns>Value read from the reader.</returns>
        int ReadInt();

        /// <summary>
        /// Reads a 32-bit unsigned integer.
        /// </summary>
        /// <returns>Value read from the reader.</returns>
        uint ReadUInt();

        /// <summary>
        /// Reads a 16-bit signed integer.
        /// </summary>
        /// <returns>Value read from the reader.</returns>
        short ReadShort();

        /// <summary>
        /// Reads a 16-bit unsigned integer.
        /// </summary>
        /// <returns>Value read from the reader.</returns>
        ushort ReadUShort();

        /// <summary>
        /// Reads a 8-bit unsigned integer.
        /// </summary>
        /// <returns>Value read from the reader.</returns>
        byte ReadByte();

        /// <summary>
        /// Reads a 8-bit signed integer.
        /// </summary>
        /// <returns>Value read from the reader.</returns>
        sbyte ReadSByte();

        /// <summary>
        /// Reads a 32-bit floating-point number.
        /// </summary>
        /// <returns>Value read from the reader.</returns>
        float ReadFloat();

        /// <summary>
        /// Reads a variable-length string of up to 65535 characters in length.
        /// </summary>
        /// <returns>String read from the reader.</returns>
        string ReadString();
    }
}
