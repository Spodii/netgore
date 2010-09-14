using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Lidgren.Network;

// FUTURE: Add a Debug check to see if Write operations result in data loss

namespace NetGore.IO
{
    /// <summary>
    /// A stream that supports performing I/O on a bit level. No parts of this class are
    /// guarenteed to be thread safe.
    /// </summary>
    public partial class BitStream : IValueReader, IValueWriter
    {
        /// <summary>
        /// Default maximum length of a string when the maximum length is not specified.
        /// </summary>
        public const ushort DefaultStringMaxLength = ushort.MaxValue - 1;

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
        /// Default BitStreamBufferMode for reading
        /// </summary>
        static BitStreamBufferMode _defaultBufferReadMode = BitStreamBufferMode.Dynamic;

        /// <summary>
        /// Default BitStreamBufferMode for writing
        /// </summary>
        static BitStreamBufferMode _defaultBufferWriteMode = BitStreamBufferMode.Dynamic;

        readonly bool _useEnumNames = false;

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
        /// The number of bits in the buffer that contain valid data to read.
        /// </summary>
        int _readLengthBits;

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
        /// Initializes a new instance of the <see cref="BitStream"/> class.
        /// </summary>
        /// <param name="buffer">The initial buffer (default mode set to read). A shallow copy of this object
        /// is used, so altering the buffer directly will alter the stream.</param>
        /// <param name="useEnumNames">If true, Enums I/O will be done using the Enum's name. If false,
        /// Enum I/O will use the underlying integer value of the Enum.</param>
        public BitStream(byte[] buffer, bool useEnumNames = false)
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
        public BitStream(BitStreamMode mode, int bufferSize, bool useEnumNames = false)
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
        /// byte written to the buffer, plus another byte if there are any partial bits.
        /// For reading, this is the length of the data that contains useful information.
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
                    return (int)Math.Ceiling(_readLengthBits / 8f);
            }
        }

        /// <summary>
        /// Gets or sets the length of the bit stream in bits. For writing, this is the highest
        /// bit, including any partial bits that have not yet been written to the buffer. 
        /// For reading, this is the length of the data that contains useful information.
        /// </summary>
        public int LengthBits
        {
            get
            {
                if (Mode == BitStreamMode.Write)
                    return (HighestWrittenIndex + 1) * _bitsByte + (_highBit - _workBufferPos);
                else
                    return _readLengthBits;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");

                if (Mode == BitStreamMode.Write)
                    Seek(BitStreamSeekOrigin.Beginning, value);
                else
                    _readLengthBits = value;
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

                if (Mode == BitStreamMode.Write)
                    _readLengthBits = LengthBits;

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
        /// Copies the contents of the BitStream to a <see cref="NetOutgoingMessage"/>. This works in both
        /// <see cref="BitStreamMode.Read"/> and <see cref="BitStreamMode.Write"/>.
        /// </summary>
        /// <param name="target">The <see cref="NetOutgoingMessage"/> to copy the contents of this <see cref="BitStream"/> to.</param>
        public void CopyTo(NetOutgoingMessage target)
        {
#if DEBUG
            var startMsgLen = target.LengthBits;
#endif
            int i = 0;

            // Write full 32-bit integers
            while (i + 3 <= HighestWrittenIndex)
            {
                var v = (_buffer[i] << 24) | (_buffer[i+1] << 16) | (_buffer[i+2] << 8)|(_buffer[i+3]);
                i += 4;
                target.Write((uint)v);
            }

            // Write full 8-bit integers
            while (i <= HighestWrittenIndex)
            {
                target.Write(_buffer[i]);
                i++;
            }

            // Write the partial chunk of the work buffer
            if (Mode == BitStreamMode.Write && _workBufferPos != _highBit)
            {
                for (var j = _highBit; j > _workBufferPos; j--)
                {
                    target.Write((_workBuffer & (1 << j)) != 0);
                }
            }

#if DEBUG
            Debug.Assert(target.LengthBits - startMsgLen == LengthBits);
#endif
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
            _readLengthBits = 0;
        }

        /// <summary>
        /// Resets the bit stream content and variables to a "like-new" state.
        /// </summary>
        /// <param name="mode">Type of BitStreamMode to reset to.</param>
        public void Reset(BitStreamMode mode)
        {
            Reset();
            _mode = mode;
        }

        /// <summary>
        /// Moves the stream's cursor to the specified location.
        /// </summary>
        /// <param name="origin">Origin to move from.</param>
        /// <param name="bits">Number of bits to move.</param>
        public void Seek(BitStreamSeekOrigin origin, int bits)
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
                    Seek(bits);
                    break;

                case BitStreamSeekOrigin.Current:
                    Seek(bits);
                    break;
            }
        }

        /// <summary>
        /// Moves the buffer by a number of bits.
        /// </summary>
        /// <param name="bits">Number of bits to move.</param>
        void Seek(int bits)
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
        /// Sets a new internal buffer for the BitStream.
        /// </summary>
        /// <param name="buffer">The new buffer.</param>
        void SetBuffer(byte[] buffer)
        {
            Reset();

            _buffer = buffer;
            _workBuffer = _buffer[0];
            _readLengthBits = buffer.Length * 8;
        }

        /// <summary>
        /// Converts a byte array to a string.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        /// <returns>The string created from the <paramref name="bytes"/>.</returns>
        static string StringFromByteArray(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Converts a string to a byte array.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>The byte array for the <paramref name="str"/>.</returns>
        static byte[] StringToByteArray(string str)
        {
            return Encoding.UTF8.GetBytes(str);
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
    }
}