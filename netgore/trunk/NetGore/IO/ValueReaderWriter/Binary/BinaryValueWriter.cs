using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore.IO
{
    /// <summary>
    /// Implementation of the <see cref="IValueWriter"/> using a <see cref="BitStream"/> to perform binary I/O.
    /// </summary>
    public class BinaryValueWriter : IValueWriter
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// If we are writing to file, contains the final destination path we want to use.
        /// </summary>
        readonly string _filePath;

        readonly bool _useEnumNames = true;
        readonly BitStream _writer;

        bool _isDisposed;
        Stack<int> _nodeOffsetStack = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryValueWriter"/> class.
        /// </summary>
        /// <param name="writer"><see cref="BitStream"/> that will be written to.</param>
        /// <param name="useEnumNames">If true, Enums I/O will be done using the Enum's name. If false,
        /// Enum I/O will use the underlying integer value of the Enum.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        BinaryValueWriter(BitStream writer, bool useEnumNames = true)
        {
            _useEnumNames = useEnumNames;

            if (writer == null)
                throw new ArgumentNullException("writer");

            _writer = writer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryValueWriter"/> class.
        /// </summary>
        /// <param name="filePath">Path to the file to write to.</param>
        /// <param name="useEnumNames">If true, Enums I/O will be done using the Enum's name. If false,
        /// Enum I/O will use the underlying integer value of the Enum.</param>
        BinaryValueWriter(string filePath, bool useEnumNames = true) : this(new BitStream(8192))
        {
            _useEnumNames = useEnumNames;
            _filePath = filePath;
        }

        /// <summary>
        /// Gets if this <see cref="BinaryValueWriter"/> has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Creates a <see cref="BinaryValueWriter"/> to write to a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="writer"><see cref="BitStream"/> that will be written to.</param>
        /// <param name="useEnumNames">If true, Enums I/O will be done using the Enum's name. If false,
        /// Enum I/O will use the underlying integer value of the Enum.</param>
        /// <returns>The <see cref="BinaryValueWriter"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public static BinaryValueWriter Create(BitStream writer, bool useEnumNames = true)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            return new BinaryValueWriter(writer, useEnumNames);
        }

        /// <summary>
        /// Creates a <see cref="BinaryValueWriter"/> to write to a file.
        /// </summary>
        /// <param name="filePath">Path to the file to write to.</param>
        /// <param name="useEnumNames">If true, Enums I/O will be done using the Enum's name. If false,
        /// Enum I/O will use the underlying integer value of the Enum.</param>
        /// <returns>The <see cref="BinaryValueWriter"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="filePath"/> is null or empty.</exception>
        public static BinaryValueWriter Create(string filePath, bool useEnumNames = true)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("filePath");

            return new BinaryValueWriter(filePath, useEnumNames);
        }

        /// <summary>
        /// Creates a BinaryValueWriter, passes it to the func to let the writing take place, and returns the generated contents.
        /// </summary>
        /// <param name="writeFunc">The func used to do the actual writing of the contents.</param>
        /// <param name="useEnumNames">If true, Enums I/O will be done using the Enum's name. If false,
        /// Enum I/O will use the underlying integer value of the Enum.</param>
        /// <returns>The BinaryValueWriter contents.</returns>
        public static byte[] CreateAndWrite(Action<BinaryValueWriter> writeFunc, bool useEnumNames = true)
        {
            using (BitStream bs = new BitStream())
            {
                using (BinaryValueWriter w = Create(bs, useEnumNames))
                {
                    writeFunc(w);
                }
                bs.TrimExcess();
                return bs.GetBuffer();
            }
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="BinaryValueWriter"/> is reclaimed by garbage collection.
        /// </summary>
        ~BinaryValueWriter()
        {
            if (log.IsWarnEnabled)
            {
                const string errmsg =
                    "Finalizer called on object `{0}`. This should be avoided at all costs by properly using Dispose().";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));
            }

            Dispose();
        }

        #region IValueWriter Members

        /// <summary>
        /// Gets if this IValueWriter supports using the name field to look up values. If false, values will have to
        /// be read back in the same order they were written and the name field will be ignored.
        /// </summary>
        public bool SupportsNameLookup
        {
            get { return false; }
        }

        /// <summary>
        /// Gets if this IValueWriter supports reading nodes. If false, any attempt to use nodes in this IValueWriter
        /// will result in a NotSupportedException being thrown.
        /// </summary>
        public bool SupportsNodes
        {
            get { return true; }
        }

        /// <summary>
        /// Gets if Enum I/O will be done with the Enum's name. If true, the name of the Enum value instead of the
        /// underlying integer value will be used. If false, the underlying integer value will be used. This
        /// only to Enum I/O that does not explicitly state which method to use.
        /// </summary>
        public bool UseEnumNames
        {
            get { return _useEnumNames; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (IsDisposed)
            {
                const string errmsg = "Tried to dispose of already-disposed object `{0}`.";
                if (log.IsWarnEnabled)
                    log.WarnFormat(errmsg, this);
                Debug.Fail(string.Format(errmsg, this));
                return;
            }

            _isDisposed = true;

            GC.SuppressFinalize(this);

            // Flush out the writer to the file
            if (_filePath != null)
            {
                var bytes = _writer.GetBufferCopy();

                // Write to a temp file first
                var tempFile = new TempFile();
                File.WriteAllBytes(tempFile.FilePath, bytes);

                // Move to the actual destination
                tempFile.MoveTo(_filePath);
            }
        }

        /// <summary>
        /// Writes an unsigned integer of up to 32 bits.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueWriter"/>.</param>
        /// <param name="value">Value to write.</param>
        /// <param name="bits">Number of bits to write.</param>
        public void Write(string name, uint value, int bits)
        {
            _writer.Write(value, bits);
        }

        /// <summary>
        /// Writes a boolean.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueWriter"/>.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, bool value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Writes a 32-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueWriter"/>.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, uint value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Writes a 16-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueWriter"/>.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, short value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Writes a 16-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueWriter"/>.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, ushort value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Writes a 8-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueWriter"/>.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, byte value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Writes a 8-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueWriter"/>.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, sbyte value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Writes a 64-bit floating-point number.
        /// </summary>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, double value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Writes a variable-length string of up to 65535 characters in length.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueWriter"/>.</param>
        /// <param name="value">String to write.</param>
        public void Write(string name, string value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Writes a 32-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueWriter"/>.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, int value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Writes a 64-bit usigned integer.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueWriter"/>.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, ulong value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Writes a 64-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueWriter"/>.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, long value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Writes a signed integer of up to 32 bits.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueWriter"/>.</param>
        /// <param name="value">Value to write.</param>
        /// <param name="bits">Number of bits to write.</param>
        public void Write(string name, int value, int bits)
        {
            _writer.Write(value, bits);
        }

        /// <summary>
        /// Writes a 32-bit floating-point number.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueWriter"/>.</param>
        /// <param name="value">Value to write.</param>
        public void Write(string name, float value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Writes the end of a child node in this <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueWriter"/>.</param>
        /// <exception cref="InvalidOperationException">Already at the root node.</exception>
        public void WriteEndNode(string name)
        {
            if (_nodeOffsetStack == null || _nodeOffsetStack.Count == 0)
            {
                const string errmsg = "Already at the root node.";
                throw new InvalidOperationException(errmsg);
            }

            var nodeStart = _nodeOffsetStack.Pop();
            var nodeEnd = _writer.PositionBits;
            var nodeLength = (uint)(nodeEnd - nodeStart - 32);

            Debug.Assert(nodeLength >= 0);

            _writer.SeekBits(nodeStart, SeekOrigin.Begin);
            Debug.Assert(_writer.PositionBits == nodeStart);

            _writer.Write(nodeLength);

            _writer.SeekBits(nodeEnd, SeekOrigin.Begin);
            Debug.Assert(_writer.PositionBits == nodeEnd);
        }

        /// <summary>
        /// Writes an Enum of type <typeparamref name="T"/>. Whether to use the Enum's underlying integer value or
        /// the name of the Enum value is determined from the <see cref="IValueWriter.UseEnumNames"/> property.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void WriteEnum<T>(string name, T value) where T : struct, IComparable, IConvertible, IFormattable
        {
            if (UseEnumNames)
                WriteEnumName(name, value);
            else
                WriteEnumValue(name, value);
        }

        /// <summary>
        /// Writes an Enum of type <typeparamref name="T"/> using the name of the Enum instead of the value.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void WriteEnumName<T>(string name, T value) where T : struct, IComparable, IConvertible, IFormattable
        {
            EnumHelper<T>.WriteName(this, name, value);
        }

        /// <summary>
        /// Writes an Enum of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public void WriteEnumValue<T>(string name, T value) where T : struct, IComparable, IConvertible, IFormattable
        {
            EnumHelper<T>.WriteValue(this, name, value);
        }

        /// <summary>
        /// Writes multiple values of the same type to the IValueWriter all under the same node name.
        /// Ordering is not guarenteed.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="nodeName">Unused by the BinaryValueWriter.</param>
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
                    {
                        writeHandler(null, value);
                    }
                }
            }
            WriteEndNode(nodeName);
        }

        /// <summary>
        /// Writes multiple values of the same type to the IValueWriter all under the same node name.
        /// Unlike the WriteMany for IEnumerables, this guarentees that ordering will be preserved.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="nodeName">Unused by the <see cref="BinaryValueWriter"/>.</param>
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
                Write(null, count);
                if (values != null && count > 0)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        writeHandler(null, values[i]);
                    }
                }
            }
            WriteEndNode(nodeName);
        }

        /// <summary>
        /// Writes multiple values of type <typeparamref name="T"/>, where each value will result in its own
        /// node being created. Ordering is not guarenteed.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="nodeName">Unused by the BinaryValueWriter.</param>
        /// <param name="values">IEnumerable of values to write. If this value is null, it will be treated
        /// the same as if it were an empty IEnumerable.</param>
        /// <param name="writeHandler">Delegate that writes the value to the IValueWriter.</param>
        public void WriteManyNodes<T>(string nodeName, IEnumerable<T> values, WriteManyNodesHandler<T> writeHandler)
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
                    {
                        WriteStartNode(null);
                        writeHandler(this, value);
                        WriteEndNode(null);
                    }
                }
            }
            WriteEndNode(nodeName);
        }

        /// <summary>
        /// Writes multiple values of type <typeparamref name="T"/>, where each value will result in its own
        /// node being created. Unlike the WriteMany for IEnumerables, this guarentees that ordering will be preserved.
        /// </summary>
        /// <typeparam name="T">The Type of value to write.</typeparam>
        /// <param name="nodeName">Unused by the <see cref="BinaryValueWriter"/>.</param>
        /// <param name="values">IEnumerable of values to write. If this value is null, it will be treated
        /// the same as if it were an empty IEnumerable.</param>
        /// <param name="writeHandler">Delegate that writes the value to the IValueWriter.</param>
        public void WriteManyNodes<T>(string nodeName, T[] values, WriteManyNodesHandler<T> writeHandler)
        {
            int count;
            if (values != null)
                count = values.Length;
            else
                count = 0;

            WriteStartNode(nodeName);
            {
                Write(null, count);
                if (values != null && count > 0)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        WriteStartNode(null);
                        writeHandler(this, values[i]);
                        WriteEndNode(null);
                    }
                }
            }
            WriteEndNode(nodeName);
        }

        /// <summary>
        /// Writes the start of a child node in this <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="name">Unused by the <see cref="BinaryValueWriter"/>.</param>
        public void WriteStartNode(string name)
        {
            const uint reservedValue = 0;

            if (_nodeOffsetStack == null)
                _nodeOffsetStack = new Stack<int>(4);

            var bitOffset = _writer.PositionBits;
            Write(null, reservedValue);
            _nodeOffsetStack.Push(bitOffset);
        }

        #endregion
    }
}