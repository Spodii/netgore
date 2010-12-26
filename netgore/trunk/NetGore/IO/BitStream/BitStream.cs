using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

// FUTURE: Add a Debug check to see if Write operations result in data loss

namespace NetGore.IO
{
    /// <summary>
    /// A stream that supports performing I/O on a bit level. No parts of this class are
    /// guarenteed to be thread safe.
    /// </summary>
    public partial class BitStream : Stream, IValueReader, IValueWriter
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

        const int _defaultBufferSize = 64;

        /// <summary>
        /// The 0-based index of the most significant bit in a byte.
        /// </summary>
        const int _highBit = (sizeof(byte) * 8) - 1;

        readonly bool _useEnumNames = false;

        /// <summary>
        /// Data buffer
        /// </summary>
        byte[] _buffer;

        /// <summary>
        /// The number of bits in the buffer that contain valid data.
        /// </summary>
        int _lengthBits;

        /// <summary>
        /// Current position in the buffer in bits.
        /// </summary>
        int _positionBits = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="BitStream"/> class.
        /// </summary>
        /// <param name="buffer">The initial buffer (default mode set to read). A shallow copy of this object
        /// is used, so altering the buffer directly will also alter the stream.</param>
        /// <param name="useEnumNames">If true, Enums I/O will be done using the Enum's name. If false,
        /// Enum I/O will use the underlying integer value of the Enum.</param>
        /// <exception cref="ArgumentNullException"><paramref name="buffer"/> is null.</exception>
        public BitStream(byte[] buffer, bool useEnumNames = false)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            _useEnumNames = useEnumNames;
            SetBuffer(buffer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitStream"/> class.
        /// </summary>
        /// <param name="bufferByteSize">Initial size of the internal buffer in bytes (must be greater than 0),</param>
        /// <param name="useEnumNames">If true, Enums I/O will be done using the Enum's name. If false,
        /// Enum I/O will use the underlying integer value of the Enum.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferByteSize"/> is less than or equal to 0.</exception>
        public BitStream(int bufferByteSize = _defaultBufferSize, bool useEnumNames = false)
        {
            if (bufferByteSize <= 0)
                throw new ArgumentOutOfRangeException("bufferByteSize", "BufferByteSize must be greater than 0.");

            _useEnumNames = useEnumNames;
            _buffer = new byte[bufferByteSize];
        }

        /// <summary>
        /// Gets or sets the length of the internal buffer in bytes. When setting the value, the value
        /// must be greater than or equal to the <see cref="BitStream.LengthBytes"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than <see cref="BitStream.LengthBytes"/>.</exception>
        public int BufferLength
        {
            get { return _buffer.Length; }
            set
            {
                if (value < Length)
                {
                    const string errmsg =
                        "value must be greater than or equal to the LengthBytes." +
                        " If you want to truncate the buffer below this value, change the length first before changing the buffer length.";
                    throw new ArgumentOutOfRangeException("value", errmsg);
                }

                Array.Resize(ref _buffer, value);
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
        /// </summary>
        /// <returns>
        /// true if the stream supports reading; otherwise, false.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override bool CanRead
        {
            get { return true; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
        /// </summary>
        /// <returns>
        /// true if the stream supports seeking; otherwise, false.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override bool CanSeek
        {
            get { return true; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
        /// </summary>
        /// <returns>
        /// true if the stream supports writing; otherwise, false.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override bool CanWrite
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the length of the BitStream in full bytes. This value is always rounded up when there
        /// are partial bytes written. For example, if you write anything, even just 1 bit, it will be greater than 0.
        /// </summary>
        public override long Length
        {
            get { return LengthBytes; }
        }

        /// <summary>
        /// Gets or sets the length of the BitStream in bits. If the length is set to a value less than the
        /// <see cref="PositionBits"/>, then the <see cref="PositionBits"/> will be changed to be equal
        /// to the new length.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><c>value</c> is out of range.</exception>
        public int LengthBits
        {
            get { return _lengthBits; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value");

                _lengthBits = value;

                // When the position exceeds the length, update the position
                if (_positionBits > _lengthBits)
                    _positionBits = _lengthBits;
            }
        }

        /// <summary>
        /// Gets the length of the BitStream in full bytes. This value is always rounded up when there
        /// are partial bytes written. For example, if you write anything, even just 1 bit, it will be greater than 0.
        /// </summary>
        public int LengthBytes
        {
            get { return (int)Math.Ceiling(_lengthBits / (float)_bitsByte); }
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets the position within the current stream.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The current position within the stream.
        /// </returns>
        public override long Position
        {
            get { return PositionBytes; }
            set { PositionBits = (int)(value * _bitsByte); }
        }

        /// <summary>
        /// Gets or sets the position of the buffer in bits.
        /// </summary>
        public int PositionBits
        {
            get { return _positionBits; }
            set
            {
                _positionBits = value;

                // When the position exceeds the length, update the length
                if (_positionBits > _lengthBits)
                    _lengthBits = _positionBits;
            }
        }

        /// <summary>
        /// Gets the position of the stream in bytes.
        /// This value is always rounded down when on a partial bit.
        /// </summary>
        public int PositionBytes
        {
            get
            {
                var ret = PositionBits / _bitsByte;
                Debug.Assert(ret == (int)Math.Floor(PositionBits / (float)_bitsByte));
                return ret;
            }
        }

        /// <summary>
        /// Checks if a number of bits can fit into the buffer without overflowing. If
        /// the buffer is set to automatically expand, this will always be true.
        /// </summary>
        /// <param name="numBits">Number of bits to check to fit into the buffer.</param>
        /// <returns>True if the bits can fit, else false.</returns>
        bool CanFitBits(int numBits)
        {
            if (numBits < 1)
            {
                Debug.Fail("numBits less than 1.");
                return true;
            }

            // Check if the bits can fit
            return PositionBits + numBits <= BufferLength * _bitsByte;
        }

        /// <summary>
        /// Gets a NotSupportedException to use for when trying to use nodes for the IValueReader and IValueWriter.
        /// </summary>
        /// <returns>A NotSupportedException to use for when trying to use nodes.</returns>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "BinaryValueWriter")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "BitStream")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "BinaryValueReader")]
        protected internal static NotSupportedException CreateNodesNotSupportedException()
        {
            const string errmsg =
                "Nodes are not supported by the BitStream. If you want node support, use the" +
                " BinaryValueReader and BinaryValueWriter instead of the BitStream directly.";
            return new NotSupportedException(errmsg);
        }

        /// <summary>
        /// Expands the buffer.
        /// </summary>
        void ExpandBuffer()
        {
            // The amount we expand will always be enough to fit in at least a 32-bit integer, and that is the largest
            // number of bits we write to or read from the buffer at any one time, so this will always be enough.

            // Always increase the buffer by at least 6 bytes.
            var newSize = _buffer.Length + 6;

            // Round up to the next power of two (if not already there)
            newSize = BitOps.NextPowerOf2(newSize);

            // Resize the buffer
            Array.Resize(ref _buffer, newSize);
        }

        /// <summary>
        /// Unused by the <see cref="BitStream"/>.
        /// </summary>
        public override void Flush()
        {
        }

        /// <summary>
        /// Gets the byte buffer (shallow copy) used by the BitStream.
        /// </summary>
        /// <returns>Byte buffer used by the BitStream (shallow copy).</returns>
        public byte[] GetBuffer()
        {
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
            // Create the byte array to hold the copy and transfer over the data
            var len = LengthBytes;
            var ret = new byte[len];
            Buffer.BlockCopy(_buffer, 0, ret, 0, len);

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
            var bitsToMove = _bitsByte - (PositionBits % _bitsByte);
            if (bitsToMove == 0)
                return;

            SeekBits(bitsToMove, SeekOrigin.Current);
        }

        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position
        /// within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array
        /// with the values between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1)
        /// replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data
        /// read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many
        /// bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is
        /// larger than the buffer length.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="offset"/> or <paramref name="count"/> is negative.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="buffer" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><c>offset</c> is less than zero.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><c>count</c> is less than zero.</exception>
        /// <exception cref="ArgumentException">The sum of the offset and count is greater than the buffer length.</exception>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (offset < 0)
                throw new ArgumentOutOfRangeException("offset");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");
            if (offset + count > buffer.Length)
                throw new ArgumentException("The sum of the offset and count is greater than the buffer length.");

            // Find the number of full bytes remaining
            var bytesRemaining = (int)Math.Floor(((float)LengthBits - PositionBits) / _bitsByte);
            Debug.Assert(bytesRemaining >= 0);

            // Find the number of bytes we should read
            var bytesToRead = Math.Min(count, bytesRemaining);
            Debug.Assert(bytesToRead >= 0);

            // Read one byte at a time
            for (var i = 0; i < bytesToRead; i++)
            {
                buffer[i + offset] = ReadByte();
            }

            return bytesToRead;
        }

        /// <summary>
        /// Reads a signed value from the BitStream.
        /// </summary>
        /// <param name="numBits">Number of bits to read.</param>
        /// <param name="maxBits">Number of bits in the desired value type.</param>
        /// <returns>The read value.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><c>numBits</c> is out of range.</exception>
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
        /// Reads from the BitStream.
        /// </summary>
        /// <param name="numBits">Number of bits to read (1 to 32 bits).</param>
        /// <returns>Base 10 value of the bits read.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><c>numBits</c> is greater than <see cref="_bitsInt"/> or less
        /// than one.</exception>
        /// <exception cref="ArithmeticException">Unexpected error encountered while reading <paramref name="numBits"/>.</exception>
        int ReadUnsigned(int numBits)
        {
            if (numBits > _bitsInt || numBits < 1)
                throw new ArgumentOutOfRangeException("numBits", string.Format("numBits contains an invalid range ({0})", numBits));

            // Check if the buffer will overflow
            if (!CanFitBits(numBits))
                ExpandBuffer();

            int ret;
            if ((numBits % _bitsByte == 0) && (PositionBits % _bitsByte == 0))
            {
                var bufferIndex = (PositionBits / _bitsByte);

                // Optimal scenario - buffer is fresh and we're grabbing whole bytes,
                // so just skip the buffer and copy the memory
                switch (numBits)
                {
                    case _bitsByte * 1: // Perfect byte copy
                        ret = _buffer[bufferIndex + 0];
                        break;

                    case _bitsByte * 2: // Perfect short copy
                        ret = (_buffer[bufferIndex + 0] << _bitsByte) | _buffer[bufferIndex + 1];
                        break;

                    case _bitsByte * 3: // Perfect bigger-than-short-but-not-quite-int-but-we-love-him-anyways copy
                        ret = (_buffer[bufferIndex + 0] << (_bitsByte * 2)) | (_buffer[bufferIndex + 1] << _bitsByte) |
                              _buffer[bufferIndex + 2];
                        break;

                    case _bitsByte * 4: // Perfect int copy
                        ret = (_buffer[bufferIndex + 0] << (_bitsByte * 3)) | (_buffer[bufferIndex + 1] << (_bitsByte * 2)) |
                              (_buffer[bufferIndex + 2] << _bitsByte) | _buffer[bufferIndex + 3];
                        break;

                    default: // Compiler complains if we don't assign ret...
                        throw new ArithmeticException("Huh... how did this happen?");
                }

                // Update the stream position
                PositionBits += numBits;
            }
            else
            {
                // Loop until all the bits have been read
                ret = 0;
                var retPos = numBits - 1;
                do
                {
                    // Check how much of the current byte in the buffer can be read
                    var bufferIndex = (PositionBits / _bitsByte);
                    var bufferByteBitsLeft = _bitsByte - (PositionBits % _bitsByte);

                    // *** Find the number of bits to read ***
                    int bitsToRead;

                    // Check if we can fit the whole piece of the buffer into the return value
                    if (bufferByteBitsLeft > retPos + 1)
                    {
                        // Only use the chunk of the buffer we can fit into the return value
                        bitsToRead = retPos + 1;
                    }
                    else
                    {
                        // Use the remainder of the bits at the current buffer position
                        bitsToRead = bufferByteBitsLeft;
                    }

                    // *** Add the bits to the return value ***

                    // Create the mask to apply to the value from the buffer
                    var mask = (1 << bitsToRead) - 1;

                    // Grab the value from the buffer and shift it over so it starts with the bits we are interested in
                    var value = _buffer[bufferIndex] >> (bufferByteBitsLeft - bitsToRead);

                    // Apply the mask to zero out values we are not interested in
                    value &= mask;

                    // Shift the value to the left to align it to where we want it on the return value
                    value <<= (retPos - (bitsToRead - 1));

                    // OR the value from the buffer onto the return value
                    ret |= value;

                    // Decrease the retPos by the number of bits we read and increase the stream position
                    retPos -= bitsToRead;
                    PositionBits += bitsToRead;
                }
                while (retPos >= 0);

                Debug.Assert(retPos == -1);
            }

            return ret;
        }

        /// <summary>
        /// Resets the BitStream content and variables to a "like-new" state. The read/write buffer modes, along
        /// with the Mode are not altered by this.
        /// </summary>
        public void Reset()
        {
            _lengthBits = 0;
            _positionBits = 0;
        }

        /// <summary>
        /// Sets the position within the current stream.
        /// </summary>
        /// <param name="byteOffset">A byte offset relative to the <paramref name="origin"/> parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"/> indicating the reference point
        /// used to obtain the new position.</param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        public override long Seek(long byteOffset, SeekOrigin origin)
        {
            return SeekBits((int)(byteOffset * _bitsByte), origin) * _bitsByte;
        }

        /// <summary>
        /// Sets the position within the current stream.
        /// </summary>
        /// <param name="bitOffset">A bit offset relative to the <paramref name="origin"/> parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"/> indicating the reference point
        /// used to obtain the new position.</param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        public int SeekBits(int bitOffset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    PositionBits = bitOffset;
                    break;

                case SeekOrigin.Current:
                    PositionBits += bitOffset;
                    break;

                case SeekOrigin.End:
                    PositionBits = LengthBits - bitOffset;
                    break;
            }

            return PositionBits;
        }

        /// <summary>
        /// Sets a new internal buffer for the BitStream.
        /// </summary>
        /// <param name="buffer">The new buffer.</param>
        void SetBuffer(byte[] buffer)
        {
            Reset();

            _buffer = buffer;
            LengthBits = buffer.Length * _bitsByte;
            PositionBits = 0;
        }

        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        public override void SetLength(long value)
        {
            LengthBits = (int)(value * _bitsByte);
        }

        /// <summary>
        /// Converts a byte array to a string.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        /// <returns>The string created from the <paramref name="bytes"/>.</returns>
        public static string StringFromByteArray(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Converts a string to a byte array.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>The byte array for the <paramref name="str"/>.</returns>
        public static byte[] StringToByteArray(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }

        /// <summary>
        /// Trims down the internal buffer to only fit the desired data. This will result in the internal buffer
        /// length being equal to <see cref="BitStream.Length"/>.
        /// </summary>
        public void TrimExcess()
        {
            Array.Resize(ref _buffer, (int)Length);
        }

        /// <summary>
        /// Writes a signed value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="numBits">The number of bits.</param>
        /// <exception cref="ArgumentOutOfRangeException"><c>numBits</c> is greater than <see cref="_bitsInt"/> or less than one.</exception>
        void WriteSigned(int value, int numBits)
        {
            if (numBits > _bitsInt || numBits < 1)
                throw new ArgumentOutOfRangeException("numBits", "numBits must be between 1 and 32.");

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

        /// <summary>
        /// Writes an unsigned value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="numBits">The number of bits.</param>
        /// <exception cref="ArgumentOutOfRangeException"><c>numBits</c> is greater than <see cref="_bitsInt"/> or less than one.</exception>
        /// <exception cref="ArithmeticException">Unexpected error occured when handling <paramref name="numBits"/>.</exception>
        void WriteUnsigned(int value, int numBits)
        {
            if (numBits > _bitsInt || numBits < 1)
                throw new ArgumentOutOfRangeException("numBits", "Value must be between 1 and 32.");

            // Check if the buffer will overflow
            if (!CanFitBits(numBits))
                ExpandBuffer();

            if ((PositionBits % _bitsByte == 0) && (numBits % _bitsByte == 0))
            {
                var bufferIndex = (PositionBits / _bitsByte);

                // Optimal scenario - buffer is fresh and we're writing a byte, so
                // we can just do a direct memory copy
                switch (numBits)
                {
                    case _bitsByte * 1: // Perfect byte writing
                        _buffer[bufferIndex + 0] = (byte)(value >> (_bitsByte * 0));
                        break;

                    case _bitsByte * 2: // Perfect short writing
                        _buffer[bufferIndex + 0] = (byte)(value >> (_bitsByte * 1));
                        _buffer[bufferIndex + 1] = (byte)(value >> (_bitsByte * 0));
                        break;

                    case _bitsByte * 3: // Perfect 24-bit writing
                        _buffer[bufferIndex + 0] = (byte)(value >> (_bitsByte * 2));
                        _buffer[bufferIndex + 1] = (byte)(value >> (_bitsByte * 1));
                        _buffer[bufferIndex + 2] = (byte)(value >> (_bitsByte * 0));
                        break;

                    case _bitsByte * 4: // Perfect int writing
                        _buffer[bufferIndex + 0] = (byte)(value >> (_bitsByte * 3));
                        _buffer[bufferIndex + 1] = (byte)(value >> (_bitsByte * 2));
                        _buffer[bufferIndex + 2] = (byte)(value >> (_bitsByte * 1));
                        _buffer[bufferIndex + 3] = (byte)(value >> (_bitsByte * 0));
                        break;

                    default:
                        throw new ArithmeticException("Huh... how did this happen?");
                }

                // Update the stream position
                PositionBits += numBits;
            }
            else
            {
                // Non-optimal scenario - we have to use some sexy bit hacking

                // Loop until all the bits have been written
                var valuePos = numBits - 1;
                do
                {
                    // Check how much of the current byte in the buffer can be written
                    var bufferIndex = (PositionBits / _bitsByte);
                    var bufferByteBitsLeft = _bitsByte - (PositionBits % _bitsByte);

                    // *** Find the number of bits to write ***
                    int bitsToWrite;

                    // Check if we can fit the whole piece of the value into the buffer
                    if (valuePos + 1 > bufferByteBitsLeft)
                    {
                        // Write to the remaining bits at the current buffer byte
                        bitsToWrite = bufferByteBitsLeft;
                    }
                    else
                    {
                        // Only write the remaining bits in the value
                        bitsToWrite = valuePos + 1;
                    }

                    // *** Add the bits to the buffer ***

                    // Create the mask to apply to the value
                    var mask = (1 << bitsToWrite) - 1;

                    // Shift over the value so that it starts with only the bits we are intersted in
                    var valueToWrite = value >> ((valuePos + 1) - bitsToWrite);

                    // Apply the mask to zero out values we are not interested in
                    valueToWrite &= mask;

                    // Shift the value to the left to align it to where we want it in the buffer
                    valueToWrite <<= bufferByteBitsLeft - bitsToWrite;

                    // Grab the current buffer value
                    var newBufferValue = _buffer[bufferIndex];

                    // Make sure we zero out the bits that we are about to write to so that way we can
                    // be sure that, when overwriting existing values, 0's are still written properly even if
                    // the bit in the buffer is a 1. We do this by taking the mask we have from earlier (which contains
                    // the mask, stored in the right-most bits) and shifting it over to the position we want (giving
                    // us a mask for the bits we are writing), then using a bitwise NOT to create a mask for all the
                    // other bits. Finally, use a bitwise AND to zero out all bits that we will be writing to.
                    var clearMask = (byte)(mask << (bufferByteBitsLeft - bitsToWrite));
                    clearMask = (byte)~clearMask;
                    newBufferValue &= clearMask;

                    // OR the value onto the buffer value
                    newBufferValue |= (byte)valueToWrite;

                    // Copy the value back into the buffer
                    _buffer[bufferIndex] = newBufferValue;

                    // Decrease the valuePos by the number of bits we wrote, and increase the stream position
                    valuePos -= bitsToWrite;
                    PositionBits += bitsToWrite;
                }
                while (valuePos >= 0);

                Debug.Assert(valuePos == -1);
            }
        }
    }
}