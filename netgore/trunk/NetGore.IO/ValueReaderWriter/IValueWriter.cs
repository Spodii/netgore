using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.IO
{
    /// <summary>
    /// Interface for an object that can write basic values for read-back later without needing any sort of
    /// reference name.
    /// </summary>
    public interface IValueWriter
    {
        /// <summary>
        /// Writes a 32-bit signed integer.
        /// </summary>
        /// <param name="value">Value to write.</param>
        void Write(int value);

        /// <summary>
        /// Writes a 32-bit unsigned integer.
        /// </summary>
        /// <param name="value">Value to write.</param>
        void Write(uint value);

        /// <summary>
        /// Writes a 16-bit signed integer.
        /// </summary>
        /// <param name="value">Value to write.</param>
        void Write(short value);

        /// <summary>
        /// Writes a 16-bit unsigned integer.
        /// </summary>
        /// <param name="value">Value to write.</param>
        void Write(ushort value);

        /// <summary>
        /// Writes a 8-bit unsigned integer.
        /// </summary>
        /// <param name="value">Value to write.</param>
        void Write(byte value);

        /// <summary>
        /// Writes a 8-bit signed integer.
        /// </summary>
        /// <param name="value">Value to write.</param>
        void Write(sbyte value);

        /// <summary>
        /// Writes a 32-bit floating-point number.
        /// </summary>
        /// <param name="value">Value to write.</param>
        void Write(float value);

        /// <summary>
        /// Writes a variable-length string of up to 65535 characters in length.
        /// </summary>
        /// <param name="value">String to write.</param>
        void Write(string value);
    }
}
