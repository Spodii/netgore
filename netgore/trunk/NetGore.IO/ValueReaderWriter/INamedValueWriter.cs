using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.IO
{
    /// <summary>
    /// Interface for an object that can write basic values for read-back later by using the unique name
    /// given to each individual value.
    /// </summary>
    public interface INamedValueWriter
    {
        /// <summary>
        /// Writes a 32-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        void Write(string name, int value);

        /// <summary>
        /// Writes a 32-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        void Write(string name, uint value);

        /// <summary>
        /// Writes a 16-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        void Write(string name, short value);

        /// <summary>
        /// Writes a 16-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        void Write(string name, ushort value);

        /// <summary>
        /// Writes a 8-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        void Write(string name, byte value);

        /// <summary>
        /// Writes a 8-bit signed integer.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        void Write(string name, sbyte value);

        /// <summary>
        /// Writes a 32-bit floating-point number.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        void Write(string name, float value);

        /// <summary>
        /// Writes a variable-length string of up to 65535 characters in length.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">String to write.</param>
        void Write(string name, string value);
    }
}
