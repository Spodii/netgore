using System;
using System.Diagnostics;
using System.Text;

// FUTURE: Add a Debug check to see if Write operations result in data loss

namespace NetGore.IO
{
    /// <summary>
    /// A stream that supports performing I/O on a bit level. No parts of this class are
    /// guarenteed to be thread safe.
    /// </summary>
    public class BitStream
    {
        /// <summary>
        /// Default maximum length of a string when the maximum length is not specified.
        /// </summary>
        public const ushort DefaultStringMaxLength = ushort.MaxValue - 1;

        const int _bitsByte = sizeof(byte) * 8;
        const int _bitsInt = sizeof(int) * 8;
        const int _bitsSByte = sizeof(sbyte) * 8;
        const int _bitsShort = sizeof(short) * 8;
        const int _bitsUInt = sizeof(uint) * 8;
        const int _bitsUShort = sizeof(ushort) * 8;
        const int _highBit = (sizeof(byte) * 8) - 1;

        /// <summary>
        /// Default BitStreamBufferMode for reading
        /// </summary>
        static BitStreamBufferMode _defaultBufferReadMode = BitStreamBufferMode.Dynamic;

        /// <summary>
        /// Default BitStreamBufferMode for writing
        /// </summary>
        static BitStreamBufferMode _defaultBufferWriteMode = BitStreamBufferMode.Dynamic;

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
        /// Gets the length of the internal buffer in bytes.
        /// </summary>
        public int BufferLength
        {
            get { return _buffer.Length; }
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
        /// BitStream constructor without internal buffer construction
        /// </summary>
        /// <param name="buffer">Initial buffer (default mode set to read)</param>
        public BitStream(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (buffer.Length == 0)
                throw new ArgumentException("buffer.Length == 0", "buffer");

            _mode = BitStreamMode.Read;
            SetBuffer(buffer);
        }

        /// <summary>
        /// BitStream constructor with internal buffer construction
        /// </summary>
        /// <param name="mode">Initial I/O mode to create the bit stream in</param>
        /// <param name="bufferSize">Initial size of the internal buffer in bytes (must be greater than 0)</param>
        public BitStream(BitStreamMode mode, int bufferSize)
        {
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
        /// Converts a signed long to a series of bytes
        /// </summary>
        /// <param name="value">Signed long value</param>
        static long BytesToLong(byte[] value)
        {
            if (value == null)
            {
                Debug.Fail("value is null.");
                return 0;
            }

            return BitConverter.ToInt64(value, 0);
        }

        /// <summary>
        /// Converts a series of bytes to an unsigned long
        /// </summary>
        /// <param name="value">8 bytes for the unsigned long</param>
        static ulong BytesToULong(byte[] value)
        {
            if (value == null)
            {
                Debug.Fail("value is null.");
                return 0;
            }

            return BitConverter.ToUInt64(value, 0);
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
            int bufLen = Length;

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
        /// Convert a double to a long
        /// </summary>
        /// <param name="value">Double value of the long</param>
        static long DoubleToLong(double value)
        {
            return BitOps.DoubleToLong(value);
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

            int newSize = Math.Max(_buffer.Length + 8, _buffer.Length * 2);
            Array.Resize(ref _buffer, newSize);
        }

        /// <summary>
        /// Convert a float to an int
        /// </summary>
        /// <param name="value">Float value of the int</param>
        static int FloatToInt(float value)
        {
            return BitOps.FloatToInt(value);
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

        static string FromByteArray(byte[] value)
        {
            /*
            // This commented-out method is no good since results may not be the same over the network
            char[] c = new char[value.Length];
            for (int i = 0; i < value.Length; i++)
                c[i] = (char)value[i];
            return new string(c);
            */
            return ASCIIEncoding.ASCII.GetString(value);
        }

        /*
        /// <summary>
        /// Takes a partial series of bits from a int and returns it
        /// in the first bits of a new int
        /// </summary>
        /// <param name="value">Value to grab the bits from</param>
        /// <param name="bitPosition">Bit position of the largest bit where 0 is
        /// the right-most (smallest) bit</param>
        /// <param name="length">Number of bits to read (0 = 1 bit, 1 = 2 bits, etc)</param>
        static int GetBits(int value, int bitPosition, int length)
        {
            // Unused method, but preserved because of references to it in comments
            value >>= bitPosition - length;
            int mask = (1 << (length + 1)) - 1;
            return value & mask;
        }
        */

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
        /// you are sure you are done writing! The buffer may contain empty indices unless 
        /// you call TrimExcess first.
        /// </summary>
        /// <returns>Byte buffer containing a copy of BitStream's buffer.</returns>
        public byte[] GetBufferCopy()
        {
            // FUTURE: I can probably make the WorkBuffer not flush by just copying it over to the buffer copy

            // Get the buffer
            var buffer = GetBuffer();

            // Create the byte array to hold the copy and transfer over the data
            var ret = new byte[buffer.Length];
            Buffer.BlockCopy(buffer, 0, ret, 0, buffer.Length);

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
        /// Convert an int to a float
        /// </summary>
        /// <param name="value">Int value of the float</param>
        static float IntToFloat(int value)
        {
            return BitOps.IntToFloat(value);
        }

        /// <summary>
        /// Converts a series of bytes to a signed long
        /// </summary>
        /// <param name="value">8 bytes for the signed long</param>
        static byte[] LongToBytes(long value)
        {
            var b = new byte[8];
            for (int i = 0; i < sizeof(long); i++)
            {
                b[i] = (byte)(value >> (i * 8));
            }
            return b;
        }

        /// <summary>
        /// Convert a long to a double
        /// </summary>
        /// <param name="value">Long value of the double</param>
        static double LongToDouble(long value)
        {
            return BitOps.LongToDouble(value);
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
            int ret = ((_workBuffer & (1 << _workBufferPos)) != 0) ? 1 : 0;

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
            for (int i = offset; i < offset + length; i++)
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
            for (int i = offset; i < offset + length; i++)
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
            for (int i = offset; i < offset + length; i++)
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
            return LongToDouble(ReadLong());
        }

        /// <summary>
        /// Reads an array of doubles from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadDouble(double[] dest, int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
            {
                dest[i] = LongToDouble(ReadLong());
            }
        }

        /// <summary>
        /// Reads 32 bits from the BitStream and returns the result as a float 
        /// </summary>
        /// <returns>Value of the next 32 bits in the BitStream as a float</returns>
        public float ReadFloat()
        {
            return IntToFloat(ReadInt());
        }

        /// <summary>
        /// Reads an array of floats from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadFloat(float[] dest, int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
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
            for (int i = offset; i < offset + length; i++)
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
            return BytesToLong(ReadBytes(sizeof(long)));
        }

        /// <summary>
        /// Reads an array of longs from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadLong(long[] dest, int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
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
            for (int i = offset; i < offset + length; i++)
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
            bool hasValue = ReadBool();
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
            bool hasValue = ReadBool();
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

            bool hasValue = ReadBool();
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
            for (int i = offset; i < offset + length; i++)
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
            bool hasValue = ReadBool();
            if (!hasValue)
                return null;

            return LongToDouble(ReadLong());
        }

        /// <summary>
        /// Reads an array of nullable doubles from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadNullableDouble(double?[] dest, int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
            {
                bool hasValue = ReadBool();
                if (!hasValue)
                    dest[i] = null;
                else
                    dest[i] = LongToDouble(ReadLong());
            }
        }

        /// <summary>
        /// Reads 32 bits of a nullable value from the BitStream and returns the result as a float 
        /// </summary>
        /// <returns>Value of the next 32 bits in the BitStream as a float</returns>
        public float? ReadNullableFloat()
        {
            bool hasValue = ReadBool();
            if (!hasValue)
                return null;

            return IntToFloat(ReadInt());
        }

        /// <summary>
        /// Reads an array of nullable floats from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadNullableFloat(float?[] dest, int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
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
            bool hasValue = ReadBool();
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
            for (int i = offset; i < offset + length; i++)
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
            bool hasValue = ReadBool();
            if (!hasValue)
                return null;

            return BytesToLong(ReadBytes(sizeof(long)));
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
            bool hasValue = ReadBool();
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
            bool hasValue = ReadBool();
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
            for (int i = offset; i < offset + length; i++)
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

            bool hasValue = ReadBool();
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
            for (int i = offset; i < offset + length; i++)
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
            bool hasValue = ReadBool();
            if (!hasValue)
                return null;

            return BytesToULong(ReadBytes(sizeof(ulong)));
        }

        /// <summary>
        /// Reads an array of nullable ulongs from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadNullableULong(ulong?[] dest, int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
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
            bool hasValue = ReadBool();
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

            bool hasValue = ReadBool();
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
            for (int i = offset; i < offset + length; i++)
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
            for (int i = offset; i < offset + length; i++)
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
            for (int i = offset; i < offset + length; i++)
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
            for (int i = offset; i < offset + length; i++)
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

            int signWithValue = ReadUnsigned(numBits);
            int sign = signWithValue & (1 << (numBits - 1));
            int value = signWithValue & (~sign);

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
            uint length = ReadUInt(GetStringLengthBits(maxLength));
            if (length == 0)
                return string.Empty;

            var b = new byte[length];
            ReadByte(b, 0, (int)length);

            return FromByteArray(b);
        }

        /// <summary>
        /// Reads an array of strings from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadString(string[] dest, int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
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
            for (int i = offset; i < offset + length; i++)
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
            return BytesToULong(ReadBytes(sizeof(ulong)));
        }

        /// <summary>
        /// Reads an array of ulongs from the BitStream
        /// </summary>
        /// <param name="dest">Array to store the read values in</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array</param>
        public void ReadULong(ulong[] dest, int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
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
                int retPos = numBits - 1;
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
                        int value = _workBuffer >> (_workBufferPos - retPos);
                        int mask = (1 << (retPos + 1)) - 1;
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
                        int offset = retPos - _workBufferPos;
                        int mask = (1 << (_workBufferPos + 1)) - 1;
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
            for (int i = offset; i < offset + length; i++)
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
                string s = string.Format("Operation requires the BitStream to be in mode `{0}`.", requiredMode);
                throw new InvalidOperationException(s);
            }
        }

        /// <summary>
        /// Resets the BitStream content and variables to a "like-new" state. The read/write buffer modes, along
        /// with the Mode are not altered by this.
        /// </summary>
        public void Reset()
        {
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
        /// Resets the bit stream content and variables to a "like-new" state
        /// </summary>
        /// <param name="mode">Type of BitStreamMode to reset to</param>
        public void Reset(BitStreamMode mode)
        {
            _mode = mode;
            Reset();
        }

        /// <summary>
        /// Moves the current bit position. Can only be used if
        /// the mode is reading. SeekOrigin.End may not be used
        /// due to the potential issues that can arise from using it.
        /// </summary>
        /// <param name="origin">Origin to move from</param>
        /// <param name="bits">Number of bits to move</param>
        public void Seek(BitStreamSeekOrigin origin, int bits)
        {
            switch (origin)
            {
                case BitStreamSeekOrigin.Beginning:
                    _bufferPos = 0;
                    _workBufferPos = 7;
                    Seek(bits);
                    break;

                case BitStreamSeekOrigin.Current:
                    Seek(bits);
                    break;
            }
        }

        /// <summary>
        /// Moves the buffer by a number of bits
        /// </summary>
        /// <param name="bits">Number of bits to move</param>
        void Seek(int bits)
        {
            // Check if we have anything to move
            if (bits == 0)
                return;

            // Check if the buffer position needs to roll over
            if (_workBufferPos == -1)
            {
                Debug.Fail("I don't think this should ever be called...");
                if (Mode == BitStreamMode.Write)
                    FlushWorkBuffer();
            }

            // If writing, flush the current work buffer
            if (Mode == BitStreamMode.Write && _workBufferPos != _highBit)
            {
                _buffer[_bufferPos] = (byte)_workBuffer;
                if (HighestWrittenIndex < _bufferPos)
                    _highestWrittenIndex = _bufferPos;
            }

            if (bits % _bitsByte == 0)
            {
                // Move by bulk
                _bufferPos += bits / _bitsByte;
            }
            else
            {
                int moveBits = bits;

                // Seek forward
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

                // Seek backwards
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

        static byte[] ToByteArray(string value)
        {
            /*
            // This commented-out method is no good since results may not be the same over the network
            char[] c = value.ToCharArray();
            byte[] b = new byte[c.Length];
            for (int i = 0; i < c.Length; i++)
                b[i] = (byte)c[i];
            return b;
            */
            return ASCIIEncoding.ASCII.GetBytes(value);
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
        /// Converts an unsigned long to a series of bytes
        /// </summary>
        /// <param name="value">Unsigned long value</param>
        static byte[] ULongToBytes(ulong value)
        {
            var b = new byte[sizeof(ulong)];
            for (int i = 0; i < sizeof(ulong); i++)
            {
                b[i] = (byte)(value >> (i * sizeof(ulong)));
            }
            return b;
        }

        /// <summary>
        /// Writes a double (64 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the double to write</param>
        [Obsolete("Fails to pass unit tests or has not been thuroughly tested.", false)]
        public void Write(double value)
        {
            Write(DoubleToLong(value));
        }

        /// <summary>
        /// Writes a nullable double (64 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the double to write</param>
        [Obsolete("Fails to pass unit tests or has not been thuroughly tested.", false)]
        public void Write(double? value)
        {
            Write(value.HasValue);
            if (value.HasValue)
                Write(DoubleToLong(value.Value));
        }

        /// <summary>
        /// Writes an array of doubles to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        [Obsolete("Fails to pass unit tests or has not been thuroughly tested.", false)]
        public void Write(double[] value, int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
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
        [Obsolete("Fails to pass unit tests or has not been thuroughly tested.", false)]
        public void Write(double?[] value, int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes a long (64 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the long to write</param>
        [Obsolete("Fails to pass unit tests or has not been thuroughly tested.", false)]
        public void Write(long value)
        {
            Write(LongToBytes(value), 0, sizeof(long));
        }

        /// <summary>
        /// Writes a nullable long (64 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the long to write</param>
        [Obsolete("Fails to pass unit tests or has not been thuroughly tested.", false)]
        public void Write(long? value)
        {
            Write(value.HasValue);
            if (value.HasValue)
                Write(LongToBytes(value.Value), 0, sizeof(long));
        }

        /// <summary>
        /// Writes an array of longs to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        [Obsolete("Fails to pass unit tests or has not been thuroughly tested.", false)]
        public void Write(long[] value, int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
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
        [Obsolete("Fails to pass unit tests or has not been thuroughly tested.", false)]
        public void Write(long?[] value, int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
            {
                Write(value[i]);
            }
        }

        /// <summary>
        /// Writes a ulong (64 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the ulong to write</param>
        [Obsolete("Fails to pass unit tests or has not been thuroughly tested.", false)]
        public void Write(ulong value)
        {
            Write(ULongToBytes(value), 0, sizeof(ulong));
        }

        /// <summary>
        /// Writes a nullable ulong (64 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the ulong to write</param>
        [Obsolete("Fails to pass unit tests or has not been thuroughly tested.", false)]
        public void Write(ulong? value)
        {
            Write(value.HasValue);
            if (value.HasValue)
                Write(ULongToBytes(value.Value), 0, sizeof(ulong));
        }

        /// <summary>
        /// Writes an array of ulongs to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        [Obsolete("Fails to pass unit tests or has not been thuroughly tested.", false)]
        public void Write(ulong[] value, int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
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
        [Obsolete("Fails to pass unit tests or has not been thuroughly tested.", false)]
        public void Write(ulong?[] value, int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
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
            for (int i = offset; i < offset + length; i++)
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
            for (int i = offset; i < offset + length; i++)
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
            for (int i = offset; i < offset + length; i++)
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
            for (int i = offset; i < offset + length; i++)
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
            for (int i = offset; i < offset + length; i++)
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
            for (int i = offset; i < offset + length; i++)
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
            for (int i = offset; i < offset + length; i++)
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
            for (int i = offset; i < offset + length; i++)
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
            for (int i = offset; i < offset + length; i++)
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
            for (int i = offset; i < offset + length; i++)
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
            for (int i = offset; i < offset + length; i++)
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
            for (int i = offset; i < offset + length; i++)
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
            var b = ToByteArray(value);
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
            for (int i = offset; i < offset + length; i++)
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
            for (int i = offset; i < offset + length; i++)
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
            for (int i = offset; i < offset + length; i++)
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
            Write(FloatToInt(value));
        }

        /// <summary>
        /// Writes a nullable float (32 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the float to write</param>
        public void Write(float? value)
        {
            Write(value.HasValue);
            if (value.HasValue)
                Write(FloatToInt(value.Value));
        }

        /// <summary>
        /// Writes an array of floats to the BitStream
        /// </summary>
        /// <param name="value">Array of values to write</param>
        /// <param name="offset">Initial index of array <paramref name="value"/> to start with</param>
        /// <param name="length">Number of indices to write</param>
        public void Write(float[] value, int offset, int length)
        {
            for (int i = offset; i < offset + length; i++)
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
            for (int i = offset; i < offset + length; i++)
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
                for (int i = _highBit; i > source._workBufferPos; i--)
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
            int signBit = 1 << (numBits - 1);

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
                int valueBitPos = numBits - 1;
                do
                {
                    if (valueBitPos <= _workBufferPos)
                    {
                        // All bits can fit into the buffer
                        int valueMask = ((1 << (valueBitPos + 1)) - 1);
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
                        int valueMask = ((1 << (_workBufferPos + 1)) - 1);
                        _workBuffer |= (value >> (valueBitPos - _workBufferPos)) & valueMask;
                        valueBitPos -= _workBufferPos + 1;

                        // Obviously the buffer is full if not all bits fit in it
                        FlushWorkBuffer();
                    }
                }
                while (true);
            }
        }
    }
}