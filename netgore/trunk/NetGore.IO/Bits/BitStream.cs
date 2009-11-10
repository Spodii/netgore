using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NetGore;

// FUTURE: Add a Debug check to see if Write operations result in data loss

namespace NetGore.IO
{
    /// <summary>
    /// A stream that supports performing I/O on a bit level. No parts of this class are
    /// guarenteed to be thread safe.
    /// </summary>
    public class BitStream : IValueReader, IValueWriter
    {
        /// <summary>
        /// The number of bits in a byte.
        /// </summary>
        const int _bitsByte = sizeof(byte) * 8;

        /// <summary>
        /// The number of bits in an int.
        /// </summary>
        const int _bitsInt = sizeof(int) * 8;

        /// <summary>
        /// The number of bits in a sbyte.
        /// </summary>
        const int _bitsSByte = sizeof(sbyte) * 8;

        /// <summary>
        /// The number of bits in a short.
        /// </summary>
        const int _bitsShort = sizeof(short) * 8;

        /// <summary>
        /// The number of bits in a uint.
        /// </summary>
        const int _bitsUInt = sizeof(uint) * 8;

        /// <summary>
        /// The number of bits in a ushort.
        /// </summary>
        const int _bitsUShort = sizeof(ushort) * 8;

        /// <summary>
        /// The 0-based index of the most significant bit in a byte.
        /// </summary>
        const int _highBit = (sizeof(byte) * 8) - 1;

        /// <summary>
        /// Default maximum length of a string when the maximum length is not specified.
        /// </summary>
        public const ushort DefaultStringMaxLength = ushort.MaxValue - 1;

        /// <summary>
        /// Data buffer
        /// </summary>
        byte[] _buffer;

        /// <summary>
        /// Current position in the buffer. When reading, this is where the work
        /// buffer came from. When writing, this is where the work buffer will go.
        /// </summary>
        int _bufferPos = 0;

        /// <summary>
        /// Default BitStreamBufferMode for reading
        /// </summary>
        static BitStreamBufferMode _defaultBufferReadMode = BitStreamBufferMode.Dynamic;

        /// <summary>
        /// Default BitStreamBufferMode for writing
        /// </summary>
        static BitStreamBufferMode _defaultBufferWriteMode = BitStreamBufferMode.Dynamic;

        /// <summary>
        /// Highest index that has been written to
        /// </summary>
        int _highestWrittenIndex = -1;

        /// <summary>
        /// Current I/O mode being used
        /// </summary>
        BitStreamMode _mode;

        /// <summary>
        /// How the buffer is handled when using reading operations 
        /// </summary>
        BitStreamBufferMode _readMode = _defaultBufferReadMode;

        readonly bool _useEnumNames = false;

        /// <summary>
        /// Buffer used to read and write the current bits from. Even though
        /// the variable is an int, it is used as a byte.
        /// </summary>
        int _workBuffer = 0;

        /// <summary>
        /// Current bit position for the working buffer on a scale of 7 to 0. 
        /// 7 is farthest left bit, 0 is farthest right bit.
        /// </summary>
        int _workBufferPos = _highBit;

        /// <summary>
        /// How the buffer is handled when using writing operations
        /// </summary>
        BitStreamBufferMode _writeMode = _defaultBufferWriteMode;

        /// <summary>
        /// Initializes a new instance of the <see cref="BitStream"/> class.
        /// </summary>
        /// <param name="buffer">The initial buffer (default mode set to read). A shallow copy of this object
        /// is used, so altering the buffer directly will alter the stream.</param>
        public BitStream(byte[] buffer) : this(buffer, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitStream"/> class.
        /// </summary>
        /// <param name="buffer">The initial buffer (default mode set to read). A shallow copy of this object
        /// is used, so altering the buffer directly will alter the stream.</param>
        /// <param name="useEnumNames">If true, Enums I/O will be done using the Enum's name. If false,
        /// Enum I/O will use the underlying integer value of the Enum.</param>
        public BitStream(byte[] buffer, bool useEnumNames)
        {
            _useEnumNames = useEnumNames;

            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (buffer.Length == 0)
                throw new ArgumentException("buffer.Length == 0", "buffer");

            _mode = BitStreamMode.Read;
            SetBuffer(buffer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitStream"/> class.
        /// </summary>
        /// <param name="mode">Initial I/O mode to create the bit stream in</param>
        /// <param name="bufferSize">Initial size of the internal buffer in bytes (must be greater than 0)</param>
        /// <param name="useEnumNames">If true, Enums I/O will be done using the Enum's name. If false,
        /// Enum I/O will use the underlying integer value of the Enum.</param>
        public BitStream(BitStreamMode mode, int bufferSize, bool useEnumNames)
        {
            _useEnumNames = useEnumNames;

            // If the buffer size is invalid, silently fix it
            if (bufferSize < 1)
            {
                Debug.Fail("bufferSize less than 1.");
                bufferSize = 2;
            }

            _mode = mode;
            _buffer = new byte[bufferSize];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitStream"/> class.
        /// </summary>
        /// <param name="mode">Initial I/O mode to create the bit stream in</param>
        /// <param name="bufferSize">Initial size of the internal buffer in bytes (must be greater than 0)</param>
        public BitStream(BitStreamMode mode, int bufferSize) : this(mode, bufferSize, false)
        {
        }

        /// <summary>
        /// Gets the length of the internal buffer in bytes.
        /// </summary>
        public int BufferLength
        {
            get { return _buffer.Length; }
        }

        /// <summary>
        /// Gets or sets the default BitStreamMode used for the read buffer for new BitStreams.
        /// </summary>
        public static BitStreamBufferMode DefaultReadBufferMode
        {
            get { return _defaultBufferReadMode; }
            set { _defaultBufferReadMode = value; }
        }

        /// <summary>
        /// Gets or sets the default BitStreamMode used for the write buffer for new BitStreams.
        /// </summary>
        public static BitStreamBufferMode DefaultWriteBufferMode
        {
            get { return _defaultBufferWriteMode; }
            set { _defaultBufferWriteMode = value; }
        }

        /// <summary>
        /// Gets the index of the highest byte written to.
        /// </summary>
        public int HighestWrittenIndex
        {
            get { return _highestWrittenIndex; }
        }

        /// <summary>
        /// Gets the length of the bit stream in bytes. For writing, this is the highest
        /// byte written to the buffer, plus another byte if there are any partial bits. For reading, 
        /// this is the length of the internal buffer in bytes.
        /// </summary>
        public int Length
        {
            get
            {
                if (Mode == BitStreamMode.Write)
                {
                    if (_workBufferPos == _highBit)
                        return HighestWrittenIndex + 1;
                    else
                        return HighestWrittenIndex + 2;
                }
                else
                    return _buffer.Length;
            }
        }

        /// <summary>
        /// Gets the length of the bit stream in bits. For writing, this is the highest
        /// bit, including any partial bits that have not yet been written to the buffer. 
        /// For reading, this is the length of the internal buffer in bits.
        /// </summary>
        public int LengthBits
        {
            get
            {
                if (Mode == BitStreamMode.Write)
                    return (HighestWrittenIndex + 1) * _bitsByte + (_highBit - _workBufferPos);
                else
                    return _buffer.Length * _bitsByte;
            }
        }

        /// <summary>
        /// Gets or sets the current I/O mode being used. If the mode changes, then the working buffer
        /// will be flushed (if writing) and the position will be reset back to the start.
        /// </summary>
        public BitStreamMode Mode
        {
            get { return _mode; }
            set
            {
                // Check that the mode is different
                if (_mode == value)
                    return;

                // If we're in the middle of writing, flush the work buffer to save it
                if (_workBufferPos != _highBit && Mode == BitStreamMode.Write)
                    FlushWorkBuffer();

                // Reset the stream
                _mode = value;
                _bufferPos = 0;
                _workBufferPos = _highBit;

                // Set the working buffer
                if (_mode == BitStreamMode.Read)
                    _workBuffer = _buffer[_bufferPos];
                else
                    _workBuffer = 0;
            }
        }

        /// <summary>
        /// Gets the position of the buffer in bits. When written to the byte buffer, if there are any bits
        /// in the working bit buffer, this value will be rounded up to the nearest byte (8 bits).
        /// </summary>
        public int PositionBits
        {
            get { return _bufferPos * _bitsByte + (_highBit - _workBufferPos); }
        }

        /// <summary>
        /// Gets the position of the internal buffer in bytes
        /// </summary>
        public int PositionBytes
        {
            get { return _bufferPos; }
        }

        /// <summary>
        /// Gets or sets how the buffer is handled when using reading operations
        /// </summary>
        public BitStreamBufferMode ReadMode
        {
            get { return _readMode; }
            set { _readMode = value; }
        }

        /// <summary>
        /// Gets or sets how the buffer is handled when using writing operations
        /// </summary>
        public BitStreamBufferMode WriteMode
        {
            get { return _writeMode; }
            set { _writeMode = value; }
        }

        /// <summary>
        /// If a number of bits can fit into the buffer without overflowing. If
        /// the buffer is set to automatically expand, this will always be true.
        /// </summary>
        /// <param name="numBits">Number of bits to check to fit into the buffer</param>
        /// <returns>True if the bits can fit, else false</returns>
        public bool CanFitBits(int numBits)
        {
            if (numBits < 1)
            {
                Debug.Fail("numBits less than 1.");
                return true;
            }

            return _bufferPos + Math.Ceiling(((numBits - _workBufferPos - 1f) / _bitsByte)) < _buffer.Length;
        }

        /// <summary>
        /// Gets a NotSupportedException to use for when trying to use nodes for the IValueReader and IValueWriter.
        /// </summary>
        /// <returns>A NotSupportedException to use for when trying to use nodes.</returns>
        protected internal static NotSupportedException CreateNodesNotSupportedException()
        {
            const string errmsg =
                "Nodes are not supported by the BitStream. If you want node support, use the" +
                " BinaryValueReader and BinaryValueWriter instead of the BitStream directly.";
            return new NotSupportedException(errmsg);
        }

        /// <summary>
        /// Gets a part of the buffer, returns it, and shifts all bytes in the buffer down
        /// to fill in the gap. Can be used to grab a segment of the buffer without having to
        /// flush the current partial bit being written (as is the case with GetBuffer()), and
        /// without having to get the whole buffer. Keep in mind that the requested segment will
        /// be completely removed, which will also result in the stream's position to change.
        /// </summary>
        /// <param name="segmentLength">Length of the requested segment, in bytes, from the start of the buffer</param>
        /// <returns>A deep copy of the requested buffer</returns>
        public byte[] DequeueBuffer(int segmentLength)
        {
            var bufLen = Length;

            if (segmentLength > bufLen)
            {
                throw new ArgumentOutOfRangeException("segmentLength",
                                                      "The dequeue segment length must be less or equal to than the BitStream.Length.");
            }
            if (segmentLength < 1)
                throw new ArgumentOutOfRangeException("segmentLength", "The dequeue segment length must be greater than 0.");

            // Create the segment and copy over the bytes
            var segment = new byte[segmentLength];
            Buffer.BlockCopy(_buffer, 0, segment, 0, segmentLength);

            // Shift down the contents in the buffer to fill in the gap
            Buffer.BlockCopy(_buffer, segmentLength + 1, _buffer, 0, bufLen - segmentLength);

            // We also have to alter the buffer position tracker
            _bufferPos -= segmentLength;
            _highestWrittenIndex -= segmentLength;

            // Return the segment copy now that we are done mucking with the buffer
            return segment;
        }

        /// <summary>
        /// Expands the buffer if the BitStreamMode for the current Mode is set to Dynamic, or throws an
        /// OverflowException if the Mode is set to Static.
        /// </summary>
        void ExpandBuffer()
        {
            switch (Mode)
            {
                case BitStreamMode.Read:
                    if (ReadMode == BitStreamBufferMode.Static)
                        throw new OverflowException("Can not read past end of buffer.");
                    break;

                case BitStreamMode.Write:
                    if (WriteMode == BitStreamBufferMode.Static)
                        throw new OverflowException("Can not write past end of buffer.");
                    break;
            }

            var newSize = Math.Max(_buffer.Length + 8, _buffer.Length * 2);
            Array.Resize(ref _buffer, newSize);
        }

        /// <summary>
        /// Writes out the current work buffer to the buffer and sets up a new one.
        /// </summary>
        void FlushWorkBuffer()
        {
            RequireMode(BitStreamMode.Write);

            // Check for a new highest written index
            if (_bufferPos > HighestWrittenIndex)
                _highestWrittenIndex = _bufferPos;

            // Write out the work buffer to the buffer, and increase the buffer offset
            _buffer[_bufferPos] = (byte)_workBuffer;
            _bufferPos++;

            // Reset the work buffer
            _workBufferPos = _highBit;
            _workBuffer = 0;
        }

        /// <summary>
        /// Gets the byte buffer (shallow copy) used by the BitStream. When called in Write mode, 
        /// this will flush out the working buffer (if one exists), so do not call this until 
        /// you are sure you are done writing! The buffer may contain empty indices unless 
        /// you call TrimExcess first.
        /// </summary>
        /// <returns>Byte buffer used by the BitStream (shallow copy).</returns>
        public byte[] GetBuffer()
        {
            // Flush the working buffer
            if (_workBufferPos != _highBit && Mode == BitStreamMode.Write)
                FlushWorkBuffer();

            return _buffer;
        }

        /// <summary>
        /// Gets a deep copy of the data in the BitStream's buffer. When called in Write mode, 
        /// this will flush out the working buffer (if one exists), so do not call this until 
        /// you are sure you are done writing!
        /// </summary>
        /// <returns>Byte buffer containing a copy of BitStream's buffer.</returns>
        public byte[] GetBufferCopy()
        {
            // FUTURE: I can probably make the WorkBuffer not flush by just copying it over to the buffer copy

            // Get the buffer
            var buffer = GetBuffer();

            // Create the byte array to hold the copy and transfer over the data
            var length = Length;
            var ret = new byte[length];
            Buffer.BlockCopy(buffer, 0, ret, 0, length);

            // Return the copy
            return ret;
        }

        /// <summary>
        /// Gets the number of bits required to store the length of a string with the specified maximum size.
        /// </summary>
        /// <param name="maxLength">Maximum length of the string.</param>
        /// <returns>Number of bits required to store the length of the string.</returns>
        static int GetStringLengthBits(uint maxLength)
        {
            return BitOps.RequiredBits(maxLength);
        }

        /// <summary>
        /// When overridden in the derived class, performs disposing of the object.
        /// </summary>
        protected virtual void HandleDispose()
        {
        }

        /// <summary>
        /// Increases the position to the next perfect byte. If you
        /// plan on doing multiple reads or writes in a row by a multiple
        /// of 8 bits, this can help increase the I/O performance.
        /// </summary>
        public void MoveToNextByte()
        {
            // Don't move if we're already on a perfect byte
            if (_workBufferPos == _highBit)
                return;

            if (Mode == BitStreamMode.Write)
                FlushWorkBuffer();
            else
            {
                _workBufferPos = _highBit;
                _bufferPos++;
                _workBuffer = _buffer[_bufferPos];
            }
        }

        /// <summary>
        /// Reads the next bit in the BitStream as an int
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1)</returns>
        public int ReadBit()
        {
            RequireMode(BitStreamMode.Read);

            // Check if the buffer will overflow
            if (_bufferPos >= _buffer.Length)
                ExpandBuffer();

            // Get the bit value as 0 or 1
            var ret = ((_workBuffer & (1 << _workBufferPos)) != 0) ? 1 : 0;

            // Decrease the work buffer position
            _workBufferPos--;

            // If the work buffer ran out, grab the next one
            if (_workBufferPos == -1)
                RefillWorkBuffer();

            // Return the bit value
            return ret;
        }

        /// <summary>
        /// Reads the next bit in the BitStream as a byte
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1)</returns>
        public byte ReadBitAsByte()
        {
            return (byte)ReadBit();
        }

        /// <summary>
        /// Reads the next bit in the BitStream as a char
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1)</returns>
        public char ReadBitAsChar()
        {
            return (char)ReadBit();
        }

        /// <summary>
        /// Reads the next bit in the BitStream as a double
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1)</returns>
        public double ReadBitAsDouble()
        {
            return ReadBit();
        }

        /// <summary>
        /// Reads the next bit in the BitStream as a float
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1)</returns>
        public float ReadBitAsFloat()
        {
            return ReadBit();
        }

        /// <summary>
        /// Reads the next bit in the BitStream as an int
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1)</returns>
        public int ReadBitAsInt()
        {
            return ReadBit();
        }

        /// <summary>
        /// Reads the next bit in the BitStream as a long
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1)</returns>
        public long ReadBitAsLong()
        {
            return ReadBit();
        }

        /// <summary>
        /// Reads the next bit in the BitStream as a SByte
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1)</returns>
        public sbyte ReadBitAsSByte()
        {
            return (sbyte)ReadBit();
        }

        /// <summary>
        /// Reads the next bit in the BitStream as a short
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1)</returns>
        public short ReadBitAsShort()
        {
            return (short)ReadBit();
        }

        /// <summary>
        /// Reads the next bit in the BitStream as a string
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1)</returns>
        public string ReadBitAsString()
        {
            return ReadBit().ToString();
        }

        /// <summary>
        /// Reads the next bit in the BitStream as a uint
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1)</returns>
        public uint ReadBitAsUInt()
        {
            return (uint)ReadBit();
        }

        /// <summary>
        /// Reads the next bit in the BitStream as a ulong
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1)</returns>
        public ulong ReadBitAsULong()
        {
            return (ulong)ReadBit();
        }

        /// <summary>
        /// Reads the next bit in the BitStream as a UShort
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1)</returns>
        public ushort ReadBitAsUShort()
        {
            return (ushort)ReadBit();
        }

        /// <summary>
        /// Reads the specified number of bits from the BitStream.
        /// </summary>
        /// <param name="bitLength">The number of bits to read.</param>
        /// <returns>A BitStream filled with the bits read.</returns>
        public BitStream ReadBits(int bitLength)
        {
            if (bitLength < 0)
                throw new ArgumentOutOfRangeException("bitLength");
            if (bitLength == 0)
                return new BitStream(BitStreamMode.Read, 1);
            if (PositionBits + bitLength > LengthBits)
                throw new ArgumentOutOfRangeException("bitLength");

#if DEBUG
            var initialBitPosition = PositionBits;
#endif

            var fullBytes = bitLength / _bitsByte;
            var remainingBits = bitLength % _bitsByte;
            var requiredBytes = fullBytes;
            if (remainingBits > 0)
                requiredBytes++;

            Debug.Assert(remainingBits < _bitsByte);
            Debug.Assert(remainingBits >= 0);
            Debug.Assert(fullBytes >= 0);
            Debug.Assert(requiredBytes > 0);

            var ret = new BitStream(BitStreamMode.Write, requiredBytes) { WriteMode = BitStreamBufferMode.Static };

            if (fullBytes > 0)
            {
                var bytes = ReadBytes(fullBytes);
                ret.Write(bytes, 0, bytes.Length);
            }

            if (remainingBits > 0)
            {
                var value = ReadByte(remainingBits);
                ret.Write(value, remainingBits);
            }

#if DEBUG
            Debug.Assert(ret.LengthBits == bitLength);
            Debug.Assert(PositionBits - initialBitPosition == bitLength);
#endif

            ret.Mode = BitStreamMode.Read;

            return ret;
        }

        /// <summary>
        /// Reads a bool (one bit) from the BitStream
        /// </summary>
        /// <returns>True if the bit is set, else false</returns>
        public bool ReadBool()
        {
            return ReadBit() != 0 ? true : false;
        }

        /// <summary>
        /// Reads an array of bools from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadBool(bool[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadBool();
            }
        }

        /// <summary>
        /// Reads an array of nullable bools from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadBool(bool?[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadNullableBool();
            }
        }

        /// <summary>
        /// Reads a byte from the BitStream and returns the result as a byte 
        /// </summary>
        /// <returns>Value of the next 8 bits in the BitStream as a byte</returns>
        public byte ReadByte()
        {
            return ReadByte(8);
        }

        /// <summary>
        /// Reads <paramref name="numBits"/> bits from the BitStream and returns the result as a byte  
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 8)</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as a byte</returns>
        public byte ReadByte(int numBits)
        {
            if (numBits == 0)
                return 0;
            if (numBits > _bitsByte)
                throw new ArgumentOutOfRangeException("numBits", string.Format("Invalid number of bits specified ({0})", numBits));
            return (byte)ReadUnsigned(numBits);
        }

        /// <summary>
        /// Reads an array of bytes from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadByte(byte[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadByte();
            }
        }

        /// <summary>
        /// Reads an array of bytes from the BitStream
        /// </summary>
        /// <param name="length">Number of bytes to read</param>
        /// <returns>Array of bytes read with a length equal to <paramref name="length"/></returns>
        public byte[] ReadBytes(int length)
        {
            var b = new byte[length];
            ReadByte(b, 0, length);
            return b;
        }

        /// <summary>
        /// Reads 64 bits from the BitStream and returns the result as a double 
        /// </summary>
        /// <returns>Value of the next 64 bits in the BitStream as a double</returns>
        public double ReadDouble()
        {
            var b = ReadBytes(sizeof(double));
            return BitConverter.ToDouble(b, 0);
        }

        /// <summary>
        /// Reads an array of doubles from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadDouble(double[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadDouble();
            }
        }

        /// <summary>
        /// Reads an Enum of type <typeparamref name="T"/>. Whether to use the Enum's underlying integer value or the
        /// name of the Enum value is determined from the <see cref="UseEnumNames"/> property.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="reader">The reader used to read the enum.</param>
        /// <returns>Value read from the reader.</returns>
        public T ReadEnum<T>(IEnumValueReader<T> reader) where T : struct, IComparable, IConvertible, IFormattable
        {
            if (UseEnumNames)
                return ReadEnumName<T>();
            else
                return ReadEnumValue(reader);
        }

        /// <summary>
        /// Reads an Enum of type <typeparamref name="T"/> using the Enum's name instead of the value.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <returns>Value read from the reader.</returns>
        public T ReadEnumName<T>() where T : struct, IComparable, IConvertible, IFormattable
        {
            var str = ReadString();
            var value = EnumIOHelper.FromName<T>(str);
            return value;
        }

        /// <summary>
        /// Reads an Enum of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="reader">The reader used to read the enum.</param>
        /// <returns>Value read from the reader.</returns>
        public T ReadEnumValue<T>(IEnumValueReader<T> reader) where T : struct, IComparable, IConvertible, IFormattable
        {
            return reader.ReadEnum(this, null);
        }

        /// <summary>
        /// Reads 32 bits from the BitStream and returns the result as a float 
        /// </summary>
        /// <returns>Value of the next 32 bits in the BitStream as a float</returns>
        public float ReadFloat()
        {
            var b = ReadBytes(sizeof(float));
            return BitConverter.ToSingle(b, 0);
        }

        /// <summary>
        /// Reads an array of floats from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadFloat(float[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadFloat();
            }
        }

        /// <summary>
        /// Reads 32 bits from the BitStream and returns the result as an int 
        /// </summary>
        /// <returns>Value of the next 32 bits in the BitStream as a int</returns>
        public int ReadInt()
        {
            return ReadInt(_bitsInt);
        }

        /// <summary>
        /// Reads <paramref name="numBits"/> bits from the BitStream and returns the result as an int  
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 32)</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as an int</returns>
        public int ReadInt(int numBits)
        {
            return ReadSigned(numBits, _bitsInt);
        }

        /// <summary>
        /// Reads an array of ints from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadInt(int[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadInt();
            }
        }

        /// <summary>
        /// Reads 64 bits from the BitStream and returns the result as a long 
        /// </summary>
        /// <returns>Value of the next 64 bits in the BitStream as a long</returns>
        public long ReadLong()
        {
            var b = ReadBytes(sizeof(long));
            return BitConverter.ToInt64(b, 0);
        }

        /// <summary>
        /// Reads an array of longs from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadLong(long[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadLong();
            }
        }

        /// <summary>
        /// Reads an array of nullable longs from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadLong(long?[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadNullableLong();
            }
        }

        /// <summary>
        /// Reads a nullable bool (one bit) from the BitStream
        /// </summary>
        /// <returns>True if the bit is set, else false</returns>
        public bool? ReadNullableBool()
        {
            var hasValue = ReadBool();
            if (!hasValue)
                return null;

            return ReadBit() != 0 ? true : false;
        }

        /// <summary>
        /// Reads a nullable byte from the BitStream and returns the result as a byte 
        /// </summary>
        /// <returns>Value of the next 8 bits in the BitStream as a byte</returns>
        public byte? ReadNullableByte()
        {
            var hasValue = ReadBool();
            if (!hasValue)
                return null;

            return ReadByte(8);
        }

        /// <summary>
        /// Reads <paramref name="numBits"/> bits of a nullable value from the BitStream and returns the result as a byte  
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 8)</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as a byte</returns>
        public byte? ReadNullableByte(int numBits)
        {
            if (numBits == 0)
                return 0;
            if (numBits > _bitsByte)
                throw new ArgumentOutOfRangeException("numBits", string.Format("Invalid number of bits specified ({0})", numBits));

            var hasValue = ReadBool();
            if (!hasValue)
                return null;

            return (byte)ReadUnsigned(numBits);
        }

        /// <summary>
        /// Reads an array of nullable bytes from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadNullableByte(byte?[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadNullableByte();
            }
        }

        /// <summary>
        /// Reads an array of nullable bytes from the BitStream
        /// </summary>
        /// <param name="length">Number of bytes to read</param>
        /// <returns>Array of bytes read with a length equal to <paramref name="length"/></returns>
        public byte?[] ReadNullableBytes(int length)
        {
            var b = new byte?[length];
            ReadNullableByte(b, 0, length);
            return b;
        }

        /// <summary>
        /// Reads 64 bits of a nullable value from the BitStream and returns the result as a double 
        /// </summary>
        /// <returns>Value of the next 64 bits in the BitStream as a double</returns>
        public double? ReadNullableDouble()
        {
            var hasValue = ReadBool();
            if (!hasValue)
                return null;

            return ReadDouble();
        }

        /// <summary>
        /// Reads an array of nullable doubles from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadNullableDouble(double?[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                var hasValue = ReadBool();
                if (!hasValue)
                    dest[i] = null;
                else
                    dest[i] = ReadDouble();
            }
        }

        /// <summary>
        /// Reads 32 bits of a nullable value from the BitStream and returns the result as a float 
        /// </summary>
        /// <returns>Value of the next 32 bits in the BitStream as a float</returns>
        public float? ReadNullableFloat()
        {
            var hasValue = ReadBool();
            if (!hasValue)
                return null;

            return ReadFloat();
        }

        /// <summary>
        /// Reads an array of nullable floats from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadNullableFloat(float?[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadNullableFloat();
            }
        }

        /// <summary>
        /// Reads 32 bits of a nullable value from the BitStream and returns the result as an int 
        /// </summary>
        /// <returns>Value of the next 32 bits in the BitStream as a int</returns>
        public int? ReadNullableInt()
        {
            return ReadNullableInt(_bitsInt);
        }

        /// <summary>
        /// Reads <paramref name="numBits"/> bits of a nullable value from the BitStream and returns the result as an int  
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 32)</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as an int</returns>
        public int? ReadNullableInt(int numBits)
        {
            var hasValue = ReadBool();
            if (!hasValue)
                return null;

            return ReadSigned(numBits, _bitsInt);
        }

        /// <summary>
        /// Reads an array of nullable ints from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadNullableInt(int?[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadNullableInt();
            }
        }

        /// <summary>
        /// Reads 64 bits of a nullable value from the BitStream and returns the result as a long 
        /// </summary>
        /// <returns>Value of the next 64 bits in the BitStream as a long</returns>
        public long? ReadNullableLong()
        {
            var hasValue = ReadBool();
            if (!hasValue)
                return null;

            return ReadLong();
        }

        /// <summary>
        /// Reads 8 bits of a nullable value from the BitStream and returns the result as a SByte 
        /// </summary>
        /// <returns>Value of the next 8 bits in the BitStream as a SByte</returns>
        public sbyte? ReadNullableSByte()
        {
            return ReadNullableSByte(_bitsSByte);
        }

        /// <summary>
        /// Reads <paramref name="numBits"/> bits of a nullable value from the BitStream and returns the result as a SByte  
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 8)</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as a SByte</returns>
        public sbyte? ReadNullableSByte(int numBits)
        {
            var hasValue = ReadBool();
            if (!hasValue)
                return null;

            return (sbyte)ReadSigned(numBits, _bitsSByte);
        }

        /// <summary>
        /// Reads 16 bits of a nullable value from the BitStream and returns the result as a short
        /// </summary>
        /// <returns>Value of the next 16 bits in the BitStream as a short</returns>
        public short? ReadNullableShort()
        {
            return ReadNullableShort(_bitsShort);
        }

        /// <summary>
        /// Reads <paramref name="numBits"/> bits of a nullable value from the BitStream and returns the result as a short  
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 16)</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as a short</returns>
        public short? ReadNullableShort(int numBits)
        {
            var hasValue = ReadBool();
            if (!hasValue)
                return null;

            return (short)ReadSigned(numBits, _bitsShort);
        }

        /// <summary>
        /// Reads an array of nullable shorts from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadNullableShort(short?[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadNullableShort();
            }
        }

        /// <summary>
        /// Reads 32 bits of a nullable value from the BitStream and returns the result as a uint 
        /// </summary>
        /// <returns>Value of the next 32 bits in the BitStream as a uint</returns>
        public uint? ReadNullableUInt()
        {
            return ReadNullableUInt(_bitsUInt);
        }

        /// <summary>
        /// Reads <paramref name="numBits"/> bits of a nullable value from the BitStream and returns the result as a uint  
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 32)</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as a uint</returns>
        public uint? ReadNullableUInt(int numBits)
        {
            if (numBits == 0)
                return 0;
            if (numBits < 0 || numBits > _bitsInt)
                throw new ArgumentOutOfRangeException("numBits", string.Format("Invalid number of bits specified ({0})", numBits));

            var hasValue = ReadBool();
            if (!hasValue)
                return null;

            return (uint)ReadUnsigned(numBits);
        }

        /// <summary>
        /// Reads an array of nullable uints from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadNullableUInt(uint?[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadNullableUInt();
            }
        }

        /// <summary>
        /// Reads 64 bits of a nullable value from the BitStream and returns the result as a ulong 
        /// </summary>
        /// <returns>Value of the next 64 bits in the BitStream as a ulong</returns>
        public ulong? ReadNullableULong()
        {
            var hasValue = ReadBool();
            if (!hasValue)
                return null;

            return ReadULong();
        }

        /// <summary>
        /// Reads an array of nullable ulongs from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadNullableULong(ulong?[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadNullableULong();
            }
        }

        /// <summary>
        /// Reads 16 bits of a nullable value from the BitStream and returns the result as a ushort 
        /// </summary>
        /// <returns>Value of the next 16 bits in the BitStream as a ushort</returns>
        public ushort? ReadNullableUShort()
        {
            var hasValue = ReadBool();
            if (!hasValue)
                return null;

            return ReadUShort(_bitsUShort);
        }

        /// <summary>
        /// Reads <paramref name="numBits"/> bits of a nullable value from the BitStream and returns the result as a ushort  
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 16)</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as a ushort</returns>
        public ushort? ReadNullableUShort(int numBits)
        {
            if (numBits == 0)
                return 0;
            if (numBits < 0 || numBits > _bitsUShort)
                throw new ArgumentOutOfRangeException("numBits", string.Format("Invalid number of bits specified ({0})", numBits));

            var hasValue = ReadBool();
            if (!hasValue)
                return null;

            return (ushort)ReadUnsigned(numBits);
        }

        /// <summary>
        /// Reads an array of nullable ushorts from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadNullableUShort(ushort?[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadNullableUShort();
            }
        }

        /// <summary>
        /// Reads 8 bits from the BitStream and returns the result as a SByte 
        /// </summary>
        /// <returns>Value of the next 8 bits in the BitStream as a SByte</returns>
        public sbyte ReadSByte()
        {
            return ReadSByte(_bitsSByte);
        }

        /// <summary>
        /// Reads <paramref name="numBits"/> bits from the BitStream and returns the result as a SByte  
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 8)</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as a SByte</returns>
        public sbyte ReadSByte(int numBits)
        {
            return (sbyte)ReadSigned(numBits, _bitsSByte);
        }

        /// <summary>
        /// Reads an array of SBytes from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadSByte(sbyte[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadSByte();
            }
        }

        /// <summary>
        /// Reads an array of nullable SBytes from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadSByte(sbyte?[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadNullableSByte();
            }
        }

        /// <summary>
        /// Reads 16 bits from the BitStream and returns the result as a short
        /// </summary>
        /// <returns>Value of the next 16 bits in the BitStream as a short</returns>
        public short ReadShort()
        {
            return ReadShort(_bitsShort);
        }

        /// <summary>
        /// Reads <paramref name="numBits"/> bits from the BitStream and returns the result as a short  
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 16)</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as a short</returns>
        public short ReadShort(int numBits)
        {
            return (short)ReadSigned(numBits, _bitsShort);
        }

        /// <summary>
        /// Reads an array of shorts from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadShort(short[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadShort();
            }
        }

        /// <summary>
        /// Reads a signed value from the BitStream.
        /// </summary>
        /// <param name="numBits">Number of bits to read.</param>
        /// <param name="maxBits">Number of bits in the desired value type.</param>
        /// <returns></returns>
        int ReadSigned(int numBits, int maxBits)
        {
            if (numBits > maxBits)
                throw new ArgumentOutOfRangeException("numBits",
                                                      string.Format("numBits ({0}) must be <= maxBits ({1}).", numBits, maxBits));

            if (numBits == 1)
                return ReadBitAsInt();

            var signWithValue = ReadUnsigned(numBits);
            var sign = signWithValue & (1 << (numBits - 1));
            var value = signWithValue & (~sign);

            // Check for a negative value
            if (sign != 0)
            {
                // Any time that value == 0 and the sign is set, it must have been because we wrote
                // a perfect negative square, since why the hell would we waste a beautiful little sign
                // on 0? For -0? Ha! Screw you.
                if (value == 0)
                    return sign << (maxBits - numBits);

                // Sign is set, value is non-zero, negate that piggy!
                value = -value;
            }

            Debug.Assert((value == 0) || ((value < 0) == (sign != 0)), "Value is improperly signed.");

            return value;
        }

        /// <summary>
        /// Reads a string containing a maximum of DefaultStringMaxLength characters from the BitStream
        /// </summary>
        /// <returns>String read from the BitStream</returns>
        public string ReadString()
        {
            return ReadString(DefaultStringMaxLength);
        }

        /// <summary>
        /// Reads a string containing a maximum of <paramref name="maxLength"/> characters from the BitStream
        /// </summary>
        /// <param name="maxLength">Maximum length of the string in characters. Must be equal to the specified
        /// number of characters used when writing this string.</param>
        /// <returns>String read from the BitStream</returns>
        public string ReadString(int maxLength)
        {
            return ReadString((uint)maxLength);
        }

        /// <summary>
        /// Reads a string containing a maximum of <paramref name="maxLength"/> characters from the BitStream
        /// </summary>
        /// <param name="maxLength">Maximum length of the string in characters. Must be equal to the specified
        /// number of characters used when writing this string.</param>
        /// <returns>String read from the BitStream</returns>
        public string ReadString(uint maxLength)
        {
            var length = ReadUInt(GetStringLengthBits(maxLength));
            if (length == 0)
                return string.Empty;

            var b = new byte[length];
            ReadByte(b, 0, (int)length);

            return StringFromByteArray(b);
        }

        /// <summary>
        /// Reads an array of strings from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadString(string[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadString();
            }
        }

        /// <summary>
        /// Reads 32 bits from the BitStream and returns the result as a uint 
        /// </summary>
        /// <returns>Value of the next 32 bits in the BitStream as a uint</returns>
        public uint ReadUInt()
        {
            return ReadUInt(_bitsUInt);
        }

        /// <summary>
        /// Reads <paramref name="numBits"/> bits from the BitStream and returns the result as a uint  
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 32)</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as a uint</returns>
        public uint ReadUInt(int numBits)
        {
            if (numBits == 0)
                return 0;
            if (numBits < 0 || numBits > _bitsInt)
                throw new ArgumentOutOfRangeException("numBits", string.Format("Invalid number of bits specified ({0})", numBits));
            return (uint)ReadUnsigned(numBits);
        }

        /// <summary>
        /// Reads an array of uints from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadUInt(uint[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadUInt();
            }
        }

        /// <summary>
        /// Reads 64 bits from the BitStream and returns the result as a ulong 
        /// </summary>
        /// <returns>Value of the next 64 bits in the BitStream as a ulong</returns>
        public ulong ReadULong()
        {
            var b = ReadBytes(sizeof(ulong));
            return BitConverter.ToUInt64(b, 0);
        }

        /// <summary>
        /// Reads an array of ulongs from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadULong(ulong[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadULong();
            }
        }

        /// <summary>
        /// Reads from the BitStream
        /// </summary>
        /// <param name="numBits">Number of bits to read (1 to 32 bits)</param>
        /// <returns>Base 10 value of the bits read</returns>
        int ReadUnsigned(int numBits)
        {
            if (numBits > _bitsInt || numBits < 1)
                throw new ArgumentOutOfRangeException("numBits", string.Format("numBits contains an invalid range ({0})", numBits));

            RequireMode(BitStreamMode.Read);

            // Check if the buffer will overflow
            if (!CanFitBits(numBits))
                ExpandBuffer();

            int ret;
            if (numBits % _bitsByte == 0 && _workBufferPos == 7)
            {
                // Optimal scenario - buffer is fresh and we're grabbing whole bytes,
                // so just skip the buffer and copy the memory
                switch (numBits)
                {
                    case _bitsByte: // Perfect byte copy
                        ret = _buffer[_bufferPos + 0];
                        _bufferPos += 1;
                        break;

                    case _bitsByte * 2: // Perfect short copy
                        ret = (_buffer[_bufferPos + 0] << _bitsByte) | _buffer[_bufferPos + 1];
                        _bufferPos += 2;
                        break;

                    case _bitsByte * 3: // Perfect bigger-than-short-but-not-quite-int-but-we-love-him-anyways copy
                        ret = (_buffer[_bufferPos + 0] << (_bitsByte * 2)) | (_buffer[_bufferPos + 1] << _bitsByte) |
                              _buffer[_bufferPos + 2];
                        _bufferPos += 3;
                        break;

                    case _bitsByte * 4: // Perfect int copy
                        ret = (_buffer[_bufferPos + 0] << (_bitsByte * 3)) | (_buffer[_bufferPos + 1] << (_bitsByte * 2)) |
                              (_buffer[_bufferPos + 2] << _bitsByte) | _buffer[_bufferPos + 3];
                        _bufferPos += 4;
                        break;

                    default: // Compiler complains if we don't assign ret...
                        throw new Exception("Huh... how did this happen?");
                }

                // Get the new work buffer
                RefillWorkBufferWithoutProgressing();
            }
            else
            {
                /*
                // This commented block is a very basic, very slow, but very stable alternative
                ret = 0;
                int retPos = numBits - 1;
                while (retPos >= 0)
                {
                    if (ReadBool())
                        ret |= 1 << retPos;
                    --retPos;
                }
                */

                // Loop until all the bits have been read
                ret = 0;
                var retPos = numBits - 1;
                do
                {
                    // Check which will run out of bits first - the buffer or the value
                    if (retPos <= _workBufferPos)
                    {
                        // Full read (_workBufferPos > -1, done reading)
                        // Set the bits onto the return value at the correct position

                        // Old method:
                        // ret |= GetBits(_workBuffer, _workBufferPos, retPos);

                        // Optimized and inlined (about 2.5x faster):
                        var value = _workBuffer >> (_workBufferPos - retPos);
                        var mask = (1 << (retPos + 1)) - 1;
                        ret |= value & mask;

                        // Decrease the work buffer and refill it if needed
                        _workBufferPos -= retPos + 1;
                        if (_workBufferPos == -1)
                            RefillWorkBuffer();
                        break;
                    }
                    else
                    {
                        // Partial read (_workBufferPos = -1, will need to read again)
                        // Set the bits onto the return value at the correct position

                        // Old method:
                        // ret |= GetBits(_workBuffer, _workBufferPos, _workBufferPos) << (retPos - _workBufferPos);

                        // Optimized and inlined (about 10x faster):
                        var offset = retPos - _workBufferPos;
                        var mask = (1 << (_workBufferPos + 1)) - 1;
                        ret |= (_workBuffer & mask) << offset;

                        // Get the new work buffer and prepare for another read
                        retPos -= _workBufferPos + 1;
                        RefillWorkBuffer();
                    }
                }
                while (true);
            }

            return ret;
        }

        /// <summary>
        /// Reads 16 bits from the BitStream and returns the result as a ushort 
        /// </summary>
        /// <returns>Value of the next 16 bits in the BitStream as a ushort</returns>
        public ushort ReadUShort()
        {
            return ReadUShort(_bitsUShort);
        }

        /// <summary>
        /// Reads <paramref name="numBits"/> bits from the BitStream and returns the result as a ushort  
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 16)</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as a ushort</returns>
        public ushort ReadUShort(int numBits)
        {
            if (numBits == 0)
                return 0;
            if (numBits < 0 || numBits > _bitsUShort)
                throw new ArgumentOutOfRangeException("numBits", string.Format("Invalid number of bits specified ({0})", numBits));
            return (ushort)ReadUnsigned(numBits);
        }

        /// <summary>
        /// Reads an array of ushorts from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadUShort(ushort[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadUShort();
            }
        }

        void RefillWorkBuffer()
        {
            RequireMode(BitStreamMode.Read);

            _bufferPos++;
            RefillWorkBufferWithoutProgressing();
        }

        void RefillWorkBufferWithoutProgressing()
        {
            RequireMode(BitStreamMode.Read);

            _workBufferPos = _highBit;
            if (_bufferPos < _buffer.Length)
                _workBuffer = _buffer[_bufferPos];
            else
                _workBuffer = 0;
        }

        /// <summary>
        /// Requires the BitStream to be in the specified mode, throwing an InvalidOperationException
        /// if the current BitStreamMode does not match the <paramref name="requiredMode"/>.
        /// </summary>
        /// <param name="requiredMode">Required BitStreamMode for the BitStream to be in.</param>
        void RequireMode(BitStreamMode requiredMode)
        {
            if (Mode != requiredMode)
            {
                const string errmsg = "Operation requires the BitStream to be in mode `{0}`.";
                var s = string.Format(errmsg, requiredMode);
                throw new InvalidOperationException(s);
            }
        }

        /// <summary>
        /// Resets the BitStream content and variables to a "like-new" state. The read/write buffer modes, along
        /// with the Mode are not altered by this.
        /// </summary>
        public void Reset()
        {
            // FUTURE: I can probably avoid clearing the array when in write mode since it will just be overwritten. Could use something like an IsDirty bool, which would clear the buffer if the mode changes.

            // We have to check if anything was even written to the buffer since, if nothing was,
            // _highestWrittenIndex will be -1
            if (HighestWrittenIndex >= 0)
                Array.Clear(_buffer, 0, HighestWrittenIndex);

            _bufferPos = 0;
            _workBufferPos = _highBit;
            _workBuffer = 0;
            _highestWrittenIndex = -1;
        }

        /// <summary>
        /// Resets the bit stream content and variables to a "like-new" state.
        /// </summary>
        /// <param name="mode">Type of BitStreamMode to reset to.</param>
        public void Reset(BitStreamMode mode)
        {
            _mode = mode;
            Reset();
        }

        /// <summary>
        /// Moves the stream's cursor to the specified location.
        /// </summary>
        /// <param name="origin">Origin to move from.</param>
        /// <param name="bits">Number of bits to move.</param>
        public void SeekFromCurrentPosition(BitStreamSeekOrigin origin, int bits)
        {
            // Check if the buffer position needs to roll over
            if (_workBufferPos == -1)
            {
                Debug.Fail("I don't think this should ever be called...");
                if (Mode == BitStreamMode.Write)
                    FlushWorkBuffer();
            }

            // If writing, flush the current work buffer
            // It is vital we do this before moving the buffer
            if (Mode == BitStreamMode.Write && _workBufferPos != _highBit)
            {
                _buffer[_bufferPos] = (byte)_workBuffer;
                if (HighestWrittenIndex < _bufferPos)
                    _highestWrittenIndex = _bufferPos;
            }

            // Now that the work buffer has been flushed and we're safe, we can seek
            switch (origin)
            {
                case BitStreamSeekOrigin.Beginning:
                    _bufferPos = 0;
                    _workBufferPos = 7;
                    SeekFromCurrentPosition(bits);
                    break;

                case BitStreamSeekOrigin.Current:
                    SeekFromCurrentPosition(bits);
                    break;
            }
        }

        /// <summary>
        /// Moves the buffer by a number of bits.
        /// </summary>
        /// <param name="bits">Number of bits to move.</param>
        void SeekFromCurrentPosition(int bits)
        {
            // Check if we have anything to move
            if (bits == 0)
                return;

            if (bits % _bitsByte == 0)
            {
                // Move by bulk
                _bufferPos += bits / _bitsByte;
            }
            else
            {
                var moveBits = bits;

                // SeekFromCurrentPosition forward
                while (moveBits > 0)
                {
                    _workBufferPos--;
                    moveBits--;
                    if (_workBufferPos == -1)
                    {
                        _workBufferPos = _highBit;
                        _bufferPos++;
                    }
                }

                // SeekFromCurrentPosition backwards
                while (moveBits < 0)
                {
                    _workBufferPos++;
                    moveBits++;
                    if (_workBufferPos == _bitsByte)
                    {
                        _workBufferPos = 0;
                        _bufferPos--;
                    }
                }
            }

            // Validate the buffer position
            if (_bufferPos < 0)
                throw new OverflowException("Can not seek past start of buffer");

            if (_bufferPos >= _buffer.Length)
                ExpandBuffer();

            // Get the new work buffer
            _workBuffer = _buffer[_bufferPos];
        }

        /// <summary>
        /// Sets a new internal buffer for the bit stream
        /// </summary>
        /// <param name="buffer">New buffer</param>
        public void SetBuffer(byte[] buffer)
        {
            _buffer = buffer;
            _bufferPos = 0;
            _workBufferPos = _highBit;
            _highestWrittenIndex = -1;
            _workBuffer = _buffer[0];
        }

        static string StringFromByteArray(byte[] value)
        {
            return ASCIIEncoding.ASCII.GetString(value);
        }

        /// <summary>
        /// Converts a string to a byte array.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>The byte array for the given string <paramref name="s"/>.</returns>
        static byte[] StringToByteArray(string s)
        {
            return ASCIIEncoding.ASCII.GetBytes(s);
        }

        /// <summary>
        /// Trims the excess indices off of the end of the buffer. Any existing
        /// working write buffer will be flushed into the buffer, so do not call until
        /// done writing.
        /// </summary>
        public void TrimExcess()
        {
            // Flush write buffer
            if (_workBufferPos != _highBit && Mode == BitStreamMode.Write)
                FlushWorkBuffer();

            // Scale down
            Array.Resize(ref _buffer, HighestWrittenIndex + 1);
        }

        /// <summary>
        /// Writes a double (64 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the double to write</param>
        public void Write(double value)
        {
            var b = BitConverter.GetBytes(value);
            Write(b, 0, b.Length);
        }

        /// <summary>
        /// Writes a nullable double (64 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the double to write</param>
        public void Write(double? value)
        {
            Write(value.HasValue);
            if (value.HasValue)
                Write(value.Value);
        }

        /// <summary>
        /// Writes an array of doubles to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(double[] value, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes an array of nullable doubles to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(double?[] value, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes a long (64 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the long to write</param>
        public void Write(long value)
        {
            var b = BitConverter.GetBytes(value);
            Write(b, 0, b.Length);
        }

        /// <summary>
        /// Writes a nullable long (64 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the long to write</param>
        public void Write(long? value)
        {
            Write(value.HasValue);
            if (value.HasValue)
                Write(value.Value);
        }

        /// <summary>
        /// Writes an array of longs to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(long[] value, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes an array of nullable longs to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(long?[] value, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes a ulong (64 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the ulong to write</param>
        public void Write(ulong value)
        {
            var b = BitConverter.GetBytes(value);
            Write(b, 0, b.Length);
        }

        /// <summary>
        /// Writes a nullable ulong (64 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the ulong to write</param>
        public void Write(ulong? value)
        {
            Write(value.HasValue);
            if (value.HasValue)
                Write(value.Value);
        }

        /// <summary>
        /// Writes an array of ulongs to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(ulong[] value, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes an array of nullable ulongs to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(ulong?[] value, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes an int (32 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the int to write</param>
        public void Write(int value)
        {
            WriteSigned(value, _bitsInt);
        }

        /// <summary>
        /// Writes a nullable int (32 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the int to write</param>
        public void Write(int? value)
        {
            Write(value.HasValue);
            if (value.HasValue)
                WriteSigned(value.Value, _bitsInt);
        }

        /// <summary>
        /// Writes an array of ints to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(int[] value, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes an array of nullable ints to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(int?[] value, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes a uint (32 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the uint to write</param>
        public void Write(uint value)
        {
            Write(value, _bitsUInt);
        }

        /// <summary>
        /// Writes a nullable uint (32 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the uint to write</param>
        public void Write(uint? value)
        {
            Write(value.HasValue);
            if (value.HasValue)
                Write(value.Value, _bitsUInt);
        }

        /// <summary>
        /// Writes a uint (32 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value to write to the BitStream</param>
        /// <param name="numBits">Number of bits to write</param>
        public void Write(uint value, int numBits)
        {
            WriteUnsigned((int)value, numBits);
        }

        /// <summary>
        /// Writes a nullable uint (32 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value to write to the BitStream</param>
        /// <param name="numBits">Number of bits to write</param>
        public void Write(uint? value, int numBits)
        {
            Write(value.HasValue);
            if (value.HasValue)
                WriteUnsigned((int)value.Value, numBits);
        }

        /// <summary>
        /// Writes an array of uints to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(uint[] value, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes an array of nullable uints to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(uint?[] value, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes a short (16 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the short to write</param>
        public void Write(short value)
        {
            Write(value, _bitsShort);
        }

        /// <summary>
        /// Writes a nullable short (16 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the short to write</param>
        public void Write(short? value)
        {
            Write(value.HasValue);
            if (value.HasValue)
                Write(value.Value, _bitsShort);
        }

        /// <summary>
        /// Writes a short (16 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value to write to the BitStream</param>
        /// <param name="numBits">Number of bits to write</param>
        public void Write(short value, int numBits)
        {
            WriteSigned(value, numBits);
        }

        /// <summary>
        /// Writes a nullable short (16 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value to write to the BitStream</param>
        /// <param name="numBits">Number of bits to write</param>
        public void Write(short? value, int numBits)
        {
            Write(value.HasValue);
            if (value.HasValue)
                WriteSigned(value.Value, numBits);
        }

        /// <summary>
        /// Writes an array of shorts to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(short[] value, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes an array of nullable shorts to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(short?[] value, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes a ushort (16 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the ushort to write</param>
        public void Write(ushort value)
        {
            Write(value, _bitsUShort);
        }

        /// <summary>
        /// Writes a ushort (16 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the ushort to write</param>
        public void Write(ushort? value)
        {
            Write(value.HasValue);
            if (value.HasValue)
                Write(value.Value, _bitsUShort);
        }

        /// <summary>
        /// Writes a ushort (16 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value to write to the BitStream</param>
        /// <param name="numBits">Number of bits to write</param>
        public void Write(ushort value, int numBits)
        {
            WriteUnsigned(value, numBits);
        }

        /// <summary>
        /// Writes a nullable ushort (16 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value to write to the BitStream</param>
        /// <param name="numBits">Number of bits to write</param>
        public void Write(ushort? value, int numBits)
        {
            Write(value.HasValue);
            if (value.HasValue)
                WriteUnsigned(value.Value, numBits);
        }

        /// <summary>
        /// Writes an array of ushort (16 bits) to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(ushort[] value, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes an array of nullable ushort (16 bits) to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(ushort?[] value, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes a sbyte (8 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the sbyte to write</param>
        public void Write(sbyte value)
        {
            Write(value, _bitsSByte);
        }

        /// <summary>
        /// Writes a nullable sbyte (8 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the sbyte to write</param>
        public void Write(sbyte? value)
        {
            Write(value.HasValue);
            if (value.HasValue)
                Write(value.Value, _bitsSByte);
        }

        /// <summary>
        /// Writes a sbyte (8 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value to write to the BitStream</param>
        /// <param name="numBits">Number of bits to write</param>
        public void Write(sbyte value, int numBits)
        {
            WriteSigned(value, numBits);
        }

        /// <summary>
        /// Writes a nullable sbyte (8 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value to write to the BitStream</param>
        /// <param name="numBits">Number of bits to write</param>
        public void Write(sbyte? value, int numBits)
        {
            Write(value.HasValue);
            if (value.HasValue)
                WriteSigned(value.Value, numBits);
        }

        /// <summary>
        /// Writes an array of sbyte (8 bits) to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(sbyte[] value, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes an array of nullable sbytes (8 bits) to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(sbyte?[] value, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes a byte (8 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the byte to write</param>
        public void Write(byte value)
        {
            Write(value, _bitsByte);
        }

        /// <summary>
        /// Writes a nullable byte (8 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the byte to write</param>
        public void Write(byte? value)
        {
            Write(value.HasValue);
            if (value.HasValue)
                Write(value.Value, _bitsByte);
        }

        /// <summary>
        /// Writes a byte (8 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value to write to the BitStream</param>
        /// <param name="numBits">Number of bits to write</param>
        public void Write(byte value, int numBits)
        {
            WriteUnsigned(value, numBits);
        }

        /// <summary>
        /// Writes a nullable byte (8 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value to write to the BitStream</param>
        /// <param name="numBits">Number of bits to write</param>
        public void Write(byte? value, int numBits)
        {
            Write(value.HasValue);
            if (value.HasValue)
                WriteUnsigned(value.Value, numBits);
        }

        /// <summary>
        /// Writes an array of bytes (8 bits) to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(byte[] value, int offset, int length)
        {
            // FUTURE: Would be more efficient if I grab 4 bytes at a time when possible
            for (var i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes an array of nullable bytes (8 bits) to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(byte?[] value, int offset, int length)
        {
            // FUTURE: Would be more efficient if I grab 4 bytes at a time when possible
            for (var i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes a string with a maximum length of DefaultStringMaxLength characters to the BitStream
        /// </summary>
        /// <param name="value">Value of the string to write</param>
        public void Write(string value)
        {
            Write(value, DefaultStringMaxLength);
        }

        /// <summary>
        /// Writes a string with a maximum length of <paramref name="maxLength"/> characters to the BitStream
        /// </summary>
        /// <param name="value">Value of the string to write</param>
        /// <param name="maxLength">Maximum number of characters the string may contain</param>
        public void Write(string value, uint maxLength)
        {
            if (value.Length > maxLength)
                throw new ArgumentOutOfRangeException("value", "String length exceeds maximum length.");

            WriteUnsigned(value.Length, GetStringLengthBits(maxLength));
            var b = StringToByteArray(value);
            Write(b, 0, b.Length);
        }

        /// <summary>
        /// Writes a string with a maximum length of <paramref name="maxLength"/> characters to the BitStream
        /// </summary>
        /// <param name="value">Value of the string to write</param>
        /// <param name="maxLength">Maximum number of characters the string may contain</param>
        public void Write(string value, int maxLength)
        {
            Write(value, (uint)maxLength);
        }

        /// <summary>
        /// Writes an array of strings to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(string[] value, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes a bool (1 bit) to the BitStream
        /// </summary>
        /// <param name="value">Value of the bool to write</param>
        public void Write(bool value)
        {
            if (value)
                WriteBit(1);
            else
                WriteBit(0);
        }

        /// <summary>
        /// Writes a nullable bool (1 bit) to the BitStream
        /// </summary>
        /// <param name="value">Value of the bool to write</param>
        public void Write(bool? value)
        {
            Write(value.HasValue);
            if (value.HasValue)
            {
                if (value.Value)
                    WriteBit(1);
                else
                    WriteBit(0);
            }
        }

        /// <summary>
        /// Writes an array of bools to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(bool[] value, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes an array of nullable bools to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(bool?[] value, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes a float (32 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the float to write</param>
        public void Write(float value)
        {
            var b = BitConverter.GetBytes(value);
            Write(b, 0, b.Length);
        }

        /// <summary>
        /// Writes a nullable float (32 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the float to write</param>
        public void Write(float? value)
        {
            Write(value.HasValue);
            if (value.HasValue)
                Write(value.Value);
        }

        /// <summary>
        /// Writes an array of floats to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(float[] value, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes an array of nullable floats to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(float?[] value, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes all of an existing BitStream's buffer to this BitStream
        /// </summary>
        /// <param name="source">BitStream to read from</param>
        public void Write(BitStream source)
        {
            // Write over the finished buffer from the source
            if (source.HighestWrittenIndex > -1)
                Write(source._buffer, 0, source.HighestWrittenIndex + 1);

            // Write over any partial bits, if needed
            if (source.Mode == BitStreamMode.Write && source._workBufferPos != _highBit)
            {
                for (var i = _highBit; i > source._workBufferPos; i--)
                {
                    WriteBit((source._workBuffer & (1 << i)) != 0);
                }
            }
        }

        /// <summary>
        /// Writes a value to the BitStream.
        /// </summary>
        /// <param name="value">Value to write to the BitStream.</param>
        /// <param name="numBits">Number of bits to write (1 to 32).</param>
        public void Write(int value, int numBits)
        {
            if (numBits > _bitsInt || numBits < 1)
                throw new ArgumentOutOfRangeException("numBits", "Value must be between 1 and 32.");
            RequireMode(BitStreamMode.Write);

            WriteSigned(value, numBits);
        }

        /// <summary>
        /// Writes a nullable value to the BitStream.
        /// </summary>
        /// <param name="value">Value to write to the BitStream.</param>
        /// <param name="numBits">Number of bits to write (1 to 32).</param>
        public void Write(int? value, int numBits)
        {
            if (numBits > _bitsInt || numBits < 1)
                throw new ArgumentOutOfRangeException("numBits", "Value must be between 1 and 32.");
            RequireMode(BitStreamMode.Write);

            Write(value.HasValue);
            if (value.HasValue)
                WriteSigned(value.Value, numBits);
        }

        /// <summary>
        /// Writes a bit to the BitStream
        /// </summary>
        /// <param name="bit">Value of the bit to write</param>
        public void WriteBit(double bit)
        {
            WriteBit(bit != 0);
        }

        /// <summary>
        /// Writes a bit to the BitStream
        /// </summary>
        /// <param name="bit">Value of the bit to write</param>
        public void WriteBit(long bit)
        {
            WriteBit(bit != 0);
        }

        /// <summary>
        /// Writes a bit to the BitStream
        /// </summary>
        /// <param name="bit">Value of the bit to write</param>
        public void WriteBit(ulong bit)
        {
            WriteBit(bit != 0);
        }

        /// <summary>
        /// Writes a bit to the BitStream
        /// </summary>
        /// <param name="bit">Value of the bit to write</param>
        public void WriteBit(uint bit)
        {
            WriteBit(bit != 0);
        }

        /// <summary>
        /// Writes a bit to the BitStream
        /// </summary>
        /// <param name="bit">Value of the bit to write</param>
        public void WriteBit(short bit)
        {
            WriteBit(bit != 0);
        }

        /// <summary>
        /// Writes a bit to the BitStream
        /// </summary>
        /// <param name="bit">Value of the bit to write</param>
        public void WriteBit(ushort bit)
        {
            WriteBit(bit != 0);
        }

        /// <summary>
        /// Writes a bit to the BitStream
        /// </summary>
        /// <param name="bit">Value of the bit to write</param>
        public void WriteBit(sbyte bit)
        {
            WriteBit(bit != 0);
        }

        /// <summary>
        /// Writes a bit to the BitStream
        /// </summary>
        /// <param name="bit">Value of the bit to write</param>
        public void WriteBit(byte bit)
        {
            WriteBit(bit != 0);
        }

        /// <summary>
        /// Writes a bit to the BitStream
        /// </summary>
        /// <param name="bit">Value of the bit to write</param>
        public void WriteBit(float bit)
        {
            WriteBit(bit != 0);
        }

        /// <summary>
        /// Writes a bit to the BitStream
        /// </summary>
        /// <param name="bit">Value of the bit to write</param>
        public void WriteBit(bool bit)
        {
            WriteBit(bit ? 1 : 0);
        }

        /// <summary>
        /// Writes a bit to the BitStream
        /// </summary>
        /// <param name="bit">Value of the bit to write</param>
        public void WriteBit(int bit)
        {
            RequireMode(BitStreamMode.Write);

            // Check if the buffer will overflow
            if (_bufferPos >= _buffer.Length)
                ExpandBuffer();

            if (bit != 0)
            {
                // Set the bit with OR operator at the bit's position
                _workBuffer |= 1 << _workBufferPos;
            }
            else
            {
                // Remove the bit by moving the bit over to the position, reversing the
                // bits with NOT, then ANDing to the buffer, forcing the target bit to 0
                _workBuffer &= (~(1 << _workBufferPos));
            }

            // Move the work buffer bit position, flushing if full
            _workBufferPos--;
            if (_workBufferPos == -1)
                FlushWorkBuffer();
        }

        /// <summary>
        /// Writes an Enum of type <typeparamref name="T"/>. Whether to use the Enum's underlying integer value or
        /// the name of the Enum value is determined from the <see cref="IValueWriter.UseEnumNames"/> property.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="writer">The writer used to write the enum value.</param>
        /// <param name="value">Value to write.</param>
        public void WriteEnum<T>(IEnumValueWriter<T> writer, T value) where T : struct, IComparable, IConvertible, IFormattable
        {
            if (UseEnumNames)
                WriteEnumName(value);
            else
                WriteEnumValue(writer, value);
        }

        /// <summary>
        /// Writes an Enum of type <typeparamref name="T"/> using the name of the Enum instead of the value.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="value">Value to write.</param>
        public void WriteEnumName<T>(T value) where T : struct, IComparable, IConvertible, IFormattable
        {
            var str = EnumIOHelper.ToName(value);
            Write(str);
        }

        /// <summary>
        /// Writes an Enum of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="writer">The writer used to write the enum value.</param>
        /// <param name="value">Value to write.</param>
        public void WriteEnumValue<T>(IEnumValueWriter<T> writer, T value)
            where T : struct, IComparable, IConvertible, IFormattable
        {
            writer.WriteEnum(this, null, value);
        }

        void WriteSigned(int value, int numBits)
        {
            if (numBits > _bitsInt || numBits < 1)
                throw new ArgumentOutOfRangeException("numBits", "numBits must be between 1 and 32.");

            RequireMode(BitStreamMode.Write);

            // Treat single bits as just that - a single bit
            // No sign will be written for 1-bit numbers, since that'd just be stupid... you stupid
            if (numBits == 1)
            {
                WriteBit(value);
                return;
            }

            // Special case scenario - if lowest value of a 2's complement, we can't negate it (legally)
            // Funny enough, it actually makes it easier for us since only int.MinValue will throw an
            // exception, but every other case of a perfect negative power of 2 will fall victim to this
            // same problem, but our reader fixes it with ease. See ReadSigned for more info.
            if (value == int.MinValue)
            {
                WriteBit(value);
                Write(0, numBits - 1);
                return;
            }

            // Get a 1-bit mask for the bit at the sign
            var signBit = 1 << (numBits - 1);

            // If negative, negate the value. While we're at it, we might as well also just 
            // pack the sign into the value so we can write the whole value in one call. This
            // is essentially the same as if we were to just use:
            //
            //  WriteBit(value < 0);
            //  if (value < 0)
            //      value = -value;
            //  WriteUnsigned(value, numBits - 1)
            //
            // Sure, who cares about one extra write call, right? Well, when we split up the
            // sign and value, this fucks us over for 8, 16, 24, and 32 bit signed writes
            // since then are no longer able to take advantage of just pushing the value into
            // the buffer instead of throwing bits all over the place.
            if (value < 0)
            {
                value = -value;

                // Ensure the sign bit is set
                value |= signBit;
            }
            else
            {
                // Ensure the sign bit is not set
                value &= ~signBit;
            }

            WriteUnsigned(value, numBits);
        }

        void WriteUnsigned(int value, int numBits)
        {
            if (numBits > _bitsInt || numBits < 1)
                throw new ArgumentOutOfRangeException("numBits", "Value must be between 1 and 32.");

            RequireMode(BitStreamMode.Write);

            // Check if the buffer will overflow
            if (!CanFitBits(numBits))
                ExpandBuffer();

            if (_workBufferPos == _highBit && (numBits % _bitsByte == 0))
            {
                // Optimal scenario - buffer is fresh and we're writing a byte, so
                // we can just do a direct memory copy
                switch (numBits)
                {
                    case _bitsByte: // Perfect byte writing
                        _buffer[_bufferPos++] = (byte)(value);
                        break;

                    case _bitsByte * 2: // Perfect short writing
                        _buffer[_bufferPos++] = (byte)(value >> 8);
                        _buffer[_bufferPos++] = (byte)(value);
                        break;

                    case _bitsByte * 3: // Perfect 24-bit writing
                        _buffer[_bufferPos++] = (byte)(value >> 16);
                        _buffer[_bufferPos++] = (byte)(value >> 8);
                        _buffer[_bufferPos++] = (byte)(value);
                        break;

                    case _bitsByte * 4: // Perfect int writing
                        _buffer[_bufferPos++] = (byte)(value >> 24);
                        _buffer[_bufferPos++] = (byte)(value >> 16);
                        _buffer[_bufferPos++] = (byte)(value >> 8);
                        _buffer[_bufferPos++] = (byte)(value);
                        break;

                    default:
                        throw new Exception("Huh... how did this happen?");
                }

                // Update the highest written index
                if (_bufferPos - 1 > HighestWrittenIndex)
                    _highestWrittenIndex = _bufferPos - 1;
            }
            else
            {
                /*
                // This commented block is a very basic, very slow, but very stable alternative
                int valueBitPos = numBits - 1;
                while (valueBitPos >= 0)
                {
                    WriteBit(value & (1 << valueBitPos));
                    valueBitPos--;
                }
                */

                // Non-optimal scenario - we have to use some sexy bit hacking
                var valueBitPos = numBits - 1;
                do
                {
                    if (valueBitPos <= _workBufferPos)
                    {
                        // All bits can fit into the buffer
                        var valueMask = ((1 << (valueBitPos + 1)) - 1);
                        _workBuffer |= (value & valueMask) << (_workBufferPos - valueBitPos);
                        _workBufferPos -= valueBitPos + 1;

                        // If the buffer is at -1, increase the position
                        if (_workBufferPos == -1)
                            FlushWorkBuffer();
                        break;
                    }
                    else
                    {
                        // Only some of the bits can fit into the buffer
                        var valueMask = ((1 << (_workBufferPos + 1)) - 1);
                        _workBuffer |= (value >> (valueBitPos - _workBufferPos)) & valueMask;
                        valueBitPos -= _workBufferPos + 1;

                        // Obviously the buffer is full if not all bits fit in it
                        FlushWorkBuffer();
                    }
                }
                while (true);
            }
        }

        #region IValueReader Members

        /// <summary>
        /// Reads an Enum of type <typeparamref name="T"/> using the Enum's name instead of the value.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        public T ReadEnumName<T>(string name) where T : struct, IComparable, IConvertible, IFormattable
        {
            var str = ReadString();
            var value = EnumIOHelper.FromName<T>(str);
            return value;
        }

        /// <summary>
        /// Reads an Enum of type <typeparamref name="T"/>. Whether to use the Enum's underlying integer value or the
        /// name of the Enum value is determined from the <see cref="UseEnumNames"/> property.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="reader">The reader used to read the enum.</param>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        public T ReadEnum<T>(IEnumValueReader<T> reader, string name) where T : struct, IComparable, IConvertible, IFormattable
        {
            return EnumIOHelper.ReadEnum(this, reader, name);
        }

        /// <summary>
        /// Reads a 8-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        byte IValueReader.ReadByte(string name)
        {
            return ReadByte();
        }

        /// <summary>
        /// Reads a 32-bit floating-point number.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        float IValueReader.ReadFloat(string name)
        {
            return ReadFloat();
        }

        /// <summary>
        /// Reads a 64-bit floating-point number.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        double IValueReader.ReadDouble(string name)
        {
            return ReadDouble();
        }

        /// <summary>
        /// Unsupported by the BitStream.
        /// </summary>
        /// <typeparam name="T">The Type of value to read.</typeparam>
        /// <param name="nodeName">Unused by the BitStream.</param>
        /// <param name="readHandler">Delegate that reads the values from the IValueReader.</param>
        /// <returns>Array of the values read the IValueReader.</returns>
        T[] IValueReader.ReadMany<T>(string nodeName, ReadManyHandler<T> readHandler)
        {
            throw CreateNodesNotSupportedException();
        }

        /// <summary>
        /// Unsupported by the BitStream.
        /// </summary>
        /// <typeparam name="T">The Type of nodes to read.</typeparam>
        /// <param name="nodeName">Unused by the BitStream.</param>
        /// <param name="readHandler">Delegate that reads the values from the IValueReader.</param>
        /// <returns>Array of the values read the IValueReader.</returns>
        T[] IValueReader.ReadManyNodes<T>(string nodeName, ReadManyNodesHandler<T> readHandler)
        {
            throw CreateNodesNotSupportedException();
        }

        /// <summary>
        /// Reads a 32-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        int IValueReader.ReadInt(string name)
        {
            return ReadInt();
        }

        /// <summary>
        /// Reads an Enum of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="reader">The reader used to read the enum.</param>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        public T ReadEnumValue<T>(IEnumValueReader<T> reader, string name)
            where T : struct, IComparable, IConvertible, IFormattable
        {
            return reader.ReadEnum(this, name);
        }

        /// <summary>
        /// Reads a signed integer of up to 32 bits.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="bits">Number of bits to read.</param>
        /// <returns>Value read from the reader.</returns>
        int IValueReader.ReadInt(string name, int bits)
        {
            return ReadInt(bits);
        }

        /// <summary>
        /// Reads a 8-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        sbyte IValueReader.ReadSByte(string name)
        {
            return ReadSByte();
        }

        /// <summary>
        /// Reads an unsigned integer of up to 32 bits.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="bits">Number of bits to read.</param>
        /// <returns>Value read from the reader.</returns>
        uint IValueReader.ReadUInt(string name, int bits)
        {
            return ReadUInt(bits);
        }

        /// <summary>
        /// Unsupported by the BitStream.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="count">The number of nodes to read. Must be greater than 0. An ArgumentOutOfRangeException will
        /// be thrown if this value exceeds the actual number of nodes available.</param>
        /// <returns>An IEnumerable of IValueReaders used to read the nodes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Count is less than 0.</exception>
        IEnumerable<IValueReader> IValueReader.ReadNodes(string name, int count)
        {
            throw CreateNodesNotSupportedException();
        }

        /// <summary>
        /// Unsupported by the BitStream.
        /// </summary>
        /// <param name="key">Unused by the BitStream.</param>
        /// <returns>An IValueReader to read the child node.</returns>
        /// <exception cref="ArgumentException">Zero or more than one values found for the given
        /// <paramref name="key"/>.</exception>
        IValueReader IValueReader.ReadNode(string key)
        {
            throw CreateNodesNotSupportedException();
        }

        /// <summary>
        /// Gets if this <see cref="IValueReader"/> supports reading nodes. If false, any attempt to use nodes
        /// in this IValueWriter will result in a NotSupportedException being thrown.
        /// </summary>
        bool IValueReader.SupportsNodes
        {
            get { return false; }
        }

        /// <summary>
        /// Reads a boolean.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        bool IValueReader.ReadBool(string name)
        {
            return ReadBool();
        }

        /// <summary>
        /// Reads a 16-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        short IValueReader.ReadShort(string name)
        {
            return ReadShort();
        }

        /// <summary>
        /// Reads a variable-length string of up to 65535 characters in length.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>String read from the reader.</returns>
        string IValueReader.ReadString(string name)
        {
            return ReadString();
        }

        /// <summary>
        /// Reads a 32-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        uint IValueReader.ReadUInt(string name)
        {
            return ReadUInt();
        }

        /// <summary>
        /// Reads a 64-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        ulong IValueReader.ReadULong(string name)
        {
            return ReadULong();
        }

        /// <summary>
        /// Reads a 64-bit signed integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        long IValueReader.ReadLong(string name)
        {
            return ReadLong();
        }

        /// <summary>
        /// Reads a 16-bit unsigned integer.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        /// <returns>Value read from the reader.</returns>
        ushort IValueReader.ReadUShort(string name)
        {
            return ReadUShort();
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
        /// Gets if this <see cref="IValueReader"/> supports using the name field to look up values. If false,
        /// values will have to be read back in the same order they were written and the name field will be ignored.
        /// </summary>
        bool IValueReader.SupportsNameLookup
        {
            get { return false; }
        }

        #endregion

        #region IValueWriter Members

        /// <summary>
        /// Gets if this <see cref="IValueWriter"/> supports reading nodes. If false, any attempt to use nodes
        /// in this IValueWriter will result in a NotSupportedException being thrown.
        /// </summary>
        bool IValueWriter.SupportsNodes
        {
            get { return false; }
        }

        /// <summary>
        /// Gets if this <see cref="IValueWriter"/> supports using the name field to look up values. If false,
        /// values will have to be read back in the same order they were written and the name field will be ignored.
        /// </summary>
        bool IValueWriter.SupportsNameLookup
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
        /// Writes an Enum of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="writer">The writer used to write the enum value.</param>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="value">Value to write.</param>
        public void WriteEnumValue<T>(IEnumValueWriter<T> writer, string name, T value)
            where T : struct, IComparable, IConvertible, IFormattable
        {
            writer.WriteEnum(this, name, value);
        }

        /// <summary>
        /// Writes an Enum of type <typeparamref name="T"/>. Whether to use the Enum's underlying integer value or
        /// the name of the Enum value is determined from the <see cref="IValueWriter.UseEnumNames"/> property.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="writer">The writer used to write the enum value.</param>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="value">Value to write.</param>
        public void WriteEnum<T>(IEnumValueWriter<T> writer, string name, T value)
            where T : struct, IComparable, IConvertible, IFormattable
        {
            EnumIOHelper.WriteEnum(this, writer, name, value);
        }

        /// <summary>
        /// Writes an Enum of type <typeparamref name="T"/> using the name of the Enum instead of the value.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="name">Unused by the BitStream.</param>
        /// <param name="value">Value to write.</param>
        public void WriteEnumName<T>(string name, T value) where T : struct, IComparable, IConvertible, IFormattable
        {
            var str = EnumIOHelper.ToName(value);
            Write(str);
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
        /// Unsupported by the BitStream.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        void IValueWriter.WriteStartNode(string name)
        {
            throw CreateNodesNotSupportedException();
        }

        /// <summary>
        /// Unsupported by the BitStream.
        /// </summary>
        /// <param name="name">Unused by the BitStream.</param>
        void IValueWriter.WriteEndNode(string name)
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
        void IValueWriter.WriteMany<T>(string nodeName, IEnumerable<T> values, WriteManyHandler<T> writeHandler)
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
        void IValueWriter.WriteManyNodes<T>(string nodeName, T[] values, WriteManyNodesHandler<T> writeHandler)
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
        void IValueWriter.WriteMany<T>(string nodeName, T[] values, WriteManyHandler<T> writeHandler)
        {
            throw CreateNodesNotSupportedException();
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

        #endregion
    }
}