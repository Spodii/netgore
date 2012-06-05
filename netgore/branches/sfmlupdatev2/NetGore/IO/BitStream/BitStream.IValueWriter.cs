using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.IO
{
    public partial class BitStream
    {
        #region IValueWriter Members

        /// <summary>
        /// Gets if this <see cref="IValueWriter"/> supports using the name field to look up values. If false,
        /// values will have to be read back in the same order they were written and the name field will be ignored.
        /// </summary>
        bool IValueWriter.SupportsNameLookup
        {
            get { return false; }
        }

        /// <summary>
        /// Gets if this <see cref="IValueWriter"/> supports reading nodes. If false, any attempt to use nodes
        /// in this IValueWriter will result in a NotSupportedException being thrown.
        /// </summary>
        bool IValueWriter.SupportsNodes
        {
            get { return false; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            HandleDispose();
        }

        /// <summary>
        /// Writes an unsigned integer of up to 32 bits.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="value">Value to write.</param>
        /// <param name="bits">Number of bits to write.</param>
        void IValueWriter.Write(string name, uint value, int bits)
        {
            Write(value, bits);
        }

        /// <summary>
        /// Writes a boolean.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="value">Value to write.</param>
        void IValueWriter.Write(string name, bool value)
        {
            Write(value);
        }

        /// <summary>
        /// Writes a 32-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="value">Value to write.</param>
        void IValueWriter.Write(string name, uint value)
        {
            Write(value);
        }

        /// <summary>
        /// Writes a 64-bit usigned integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="value">Value to write.</param>
        void IValueWriter.Write(string name, ulong value)
        {
            Write(value);
        }

        /// <summary>
        /// Writes a 64-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="value">Value to write.</param>
        void IValueWriter.Write(string name, long value)
        {
            Write(value);
        }

        /// <summary>
        /// Writes a 16-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="value">Value to write.</param>
        void IValueWriter.Write(string name, short value)
        {
            Write(value);
        }

        /// <summary>
        /// Writes a 16-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="value">Value to write.</param>
        void IValueWriter.Write(string name, ushort value)
        {
            Write(value);
        }

        /// <summary>
        /// Writes a 8-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="value">Value to write.</param>
        void IValueWriter.Write(string name, byte value)
        {
            Write(value);
        }

        /// <summary>
        /// Writes a 8-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="value">Value to write.</param>
        void IValueWriter.Write(string name, sbyte value)
        {
            Write(value);
        }

        /// <summary>
        /// Writes a variable-length string of up to 65535 characters in length.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="value">String to write.</param>
        void IValueWriter.Write(string name, string value)
        {
            Write(value);
        }

        /// <summary>
        /// Writes a 32-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="value">Value to write.</param>
        void IValueWriter.Write(string name, int value)
        {
            Write(value);
        }

        /// <summary>
        /// Writes a signed integer of up to 32 bits.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="value">Value to write.</param>
        /// <param name="bits">Number of bits to write.</param>
        void IValueWriter.Write(string name, int value, int bits)
        {
            Write(value, bits);
        }

        /// <summary>
        /// Writes a 32-bit floating-point number.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="value">Value to write.</param>
        void IValueWriter.Write(string name, float value)
        {
            Write(value);
        }

        /// <summary>
        /// Writes a 64-bit floating-point number.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="value">Value to write.</param>
        void IValueWriter.Write(string name, double value)
        {
            Write(value);
        }

        /// <summary>
        /// Unsupported by the BitStream.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <exception cref="NotSupportedException">This method is not supported by the <see cref="BitStream"/>.</exception>
        void IValueWriter.WriteEndNode(string name)
        {
            throw CreateNodesNotSupportedException();
        }

        /// <summary>
        /// Writes an Enum of type <typeparamref name="T"/>. Whether to use the Enum's underlying integer value or
        /// the name of the Enum value is determined from the <see cref="IValueWriter.UseEnumNames"/> property.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="value">Value to write.</param>
        public void WriteEnum<T>(string name, T value) where T : struct, IComparable, IConvertible, IFormattable
        {
            if (UseEnumNames)
                WriteEnumName(value);
            else
                WriteEnumValue(value);
        }

        /// <summary>
        /// Writes an Enum of type <typeparamref name="T"/> using the name of the Enum instead of the value.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="value">Value to write.</param>
        public void WriteEnumName<T>(string name, T value) where T : struct, IComparable, IConvertible, IFormattable
        {
            WriteEnumName(value);
        }

        /// <summary>
        /// Writes an Enum of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="value">Value to write.</param>
        public void WriteEnumValue<T>(string name, T value) where T : struct, IComparable, IConvertible, IFormattable
        {
            WriteEnumValue(value);
        }

        /// <summary>
        /// Unsupported by the BitStream.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="nodeName">Unused by the BitStream.</param>
        /// <param name="values">IEnumerable of values to write. If this value is null, it will be treated
        /// the same as if it were an empty IEnumerable.</param>
        /// <param name="writeHandler">Delegate that writes the value to the IValueWriter.</param>
        /// <exception cref="NotSupportedException">This method is not supported by the <see cref="BitStream"/>.</exception>
        void IValueWriter.WriteMany<T>(string nodeName, IEnumerable<T> values, WriteManyHandler<T> writeHandler)
        {
            throw CreateNodesNotSupportedException();
        }

        /// <summary>
        /// Unsupported by the BitStream.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="nodeName">Unused by the BitStream.</param>
        /// <param name="values">Array of values to write. If this value is null, it will be treated
        /// the same as if it were an empty array.</param>
        /// <param name="writeHandler">Delegate that writes the value to the IValueWriter.</param>
        /// <exception cref="NotSupportedException">This method is not supported by the <see cref="BitStream"/>.</exception>
        void IValueWriter.WriteMany<T>(string nodeName, T[] values, WriteManyHandler<T> writeHandler)
        {
            throw CreateNodesNotSupportedException();
        }

        /// <summary>
        /// Unsupported by the BitStream.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="nodeName">Unused by the BitStream.</param>
        /// <param name="values">IEnumerable of values to write. If this value is null, it will be treated
        /// the same as if it were an empty IEnumerable.</param>
        /// <param name="writeHandler">Delegate that writes the value to the IValueWriter.</param>
        /// <exception cref="NotSupportedException">This method is not supported by the <see cref="BitStream"/>.</exception>
        void IValueWriter.WriteManyNodes<T>(string nodeName, IEnumerable<T> values, WriteManyNodesHandler<T> writeHandler)
        {
            throw CreateNodesNotSupportedException();
        }

        /// <summary>
        /// Unsupported by the BitStream.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="nodeName">Unused by the BitStream.</param>
        /// <param name="values">IEnumerable of values to write. If this value is null, it will be treated
        /// the same as if it were an empty IEnumerable.</param>
        /// <param name="writeHandler">Delegate that writes the value to the IValueWriter.</param>
        /// <exception cref="NotSupportedException">This method is not supported by the <see cref="BitStream"/>.</exception>
        void IValueWriter.WriteManyNodes<T>(string nodeName, T[] values, WriteManyNodesHandler<T> writeHandler)
        {
            throw CreateNodesNotSupportedException();
        }

        /// <summary>
        /// Unsupported by the BitStream.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <exception cref="NotSupportedException">This method is not supported by the <see cref="BitStream"/>.</exception>
        void IValueWriter.WriteStartNode(string name)
        {
            throw CreateNodesNotSupportedException();
        }

        #endregion
    }
}