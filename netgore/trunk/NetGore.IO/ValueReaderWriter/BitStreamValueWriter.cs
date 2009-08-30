using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace NetGore.IO
{
    /// <summary>
    /// Implementation of the IValueWriter using a BitStream.
    /// </summary>
    public class BitStreamValueWriter : IValueWriter
    {
        readonly string _destinationFile = null;
        readonly BitStream _writer;
        Stack<int> _nodeOffsetStack = null;

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

        /// <summary>
        /// FileBitStreamValueWriter constructor.
        /// </summary>
        /// <param name="filePath">Path to the file to write to.</param>
        public BitStreamValueWriter(string filePath) : this(new BitStream(BitStreamMode.Write, 8192))
        {
            _destinationFile = filePath;
            File.WriteAllBytes(filePath, new byte[]
            {
                0
            });
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
        /// Writes the start of a child node in this IValueWriter.
        /// </summary>
        /// <param name="name">Unused by the BitStreamValueWriter.</param>
        public void WriteStartNode(string name)
        {
            const uint reservedValue = 0;

            if (_nodeOffsetStack == null)
                _nodeOffsetStack = new Stack<int>(4);

            int bitOffset = _writer.PositionBits;
            Write(null, reservedValue);
            _nodeOffsetStack.Push(bitOffset);
        }

        /// <summary>
        /// Writes the end of a child node in this IValueWriter.
        /// </summary>
        /// <param name="name">Unused by the BitStreamValueWriter.</param>
        public void WriteEndNode(string name)
        {
            if (_nodeOffsetStack == null || _nodeOffsetStack.Count == 0)
            {
                const string errmsg = "Already at the root node.";
                throw new ArgumentException(errmsg);
            }

            int nodeStart = _nodeOffsetStack.Pop();
            int nodeEnd = _writer.PositionBits;
            uint nodeLength = (uint)(nodeEnd - nodeStart - 32);

            Debug.Assert(nodeLength >= 0);

            _writer.Seek(BitStreamSeekOrigin.Beginning, nodeStart);
            _writer.Write(nodeLength);
            _writer.Seek(BitStreamSeekOrigin.Beginning, nodeEnd);
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
                Write(null, count);
                if (values != null && count > 0)
                {
                    foreach (var value in values)
                        writeHandler(null, value);
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
            // TODO: !! ...
            throw new NotImplementedException();
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
            // TODO: !! ...
            throw new NotImplementedException();
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
                Write("Count", count);
                if (values != null && count > 0)
                {
                    for (int i = 0; i < values.Length; i++)
                        writeHandler(null, values[i]);
                }
            }
            WriteEndNode(nodeName);
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
        public virtual void Dispose()
        {
            if (_destinationFile != null)
            {
                var bytes = _writer.GetBufferCopy();
                File.WriteAllBytes(_destinationFile, bytes);
            }
        }

        #endregion
    }
}