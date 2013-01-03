using System;
using System.Linq;

namespace NetGore.IO
{
    public partial class BitStream
    {
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
        /// Writes a nullable uint (32 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the uint to write</param>
        public void Write(uint? value)
        {
            Write(value.HasValue);
            if (value.HasValue)
                Write(value.Value);
        }

        /// <summary>
        /// Writes a uint (32 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value to write to the BitStream</param>
        /// <param name="numBits">Number of bits to write</param>
        public void Write(uint value, int numBits = _bitsUInt)
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
        /// Writes a nullable short (16 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the short to write</param>
        public void Write(short? value)
        {
            Write(value.HasValue);
            if (value.HasValue)
                Write(value.Value);
        }

        /// <summary>
        /// Writes a short (16 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value to write to the BitStream</param>
        /// <param name="numBits">Number of bits to write</param>
        public void Write(short value, int numBits = _bitsShort)
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
        public void Write(ushort? value)
        {
            Write(value.HasValue);
            if (value.HasValue)
                Write(value.Value);
        }

        /// <summary>
        /// Writes a ushort (16 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value to write to the BitStream</param>
        /// <param name="numBits">Number of bits to write</param>
        public void Write(ushort value, int numBits = _bitsUShort)
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
        /// Writes a nullable sbyte (8 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the sbyte to write</param>
        public void Write(sbyte? value)
        {
            Write(value.HasValue);
            if (value.HasValue)
                Write(value.Value);
        }

        /// <summary>
        /// Writes a sbyte (8 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value to write to the BitStream</param>
        /// <param name="numBits">Number of bits to write</param>
        public void Write(sbyte value, int numBits = _bitsSByte)
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
        /// Writes a nullable byte (8 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value of the byte to write</param>
        public void Write(byte? value)
        {
            Write(value.HasValue);
            if (value.HasValue)
                Write(value.Value);
        }

        /// <summary>
        /// Writes a byte (8 bits) to the BitStream
        /// </summary>
        /// <param name="value">Value to write to the BitStream</param>
        /// <param name="numBits">Number of bits to write</param>
        public void Write(byte value, int numBits = _bitsByte)
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
        public override void Write(byte[] value, int offset, int length)
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
        /// Writes a string with a maximum length of <paramref name="maxLength"/> characters to the BitStream
        /// </summary>
        /// <param name="value">Value of the string to write</param>
        /// <param name="maxLength">Maximum number of characters the string may contain</param>
        /// <exception cref="ArgumentOutOfRangeException">The length of <paramref name="value"/> is greater than
        /// <paramref name="maxLength"/>.</exception>
        public void Write(string value, uint maxLength = DefaultStringMaxLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                WriteUnsigned(0, GetStringLengthBits(maxLength));
                return;
            }

            var b = StringToByteArray(value);

            if (b.Length > maxLength)
                throw new ArgumentOutOfRangeException("value", "UTF8-encoded string length exceeds maximum length (note that foreign chars can take multiple bytes).");

            WriteUnsigned(b.Length, GetStringLengthBits(maxLength));
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
        public unsafe void Write(float value)
        {
            int intValue = *((int*)&value);
            WriteUnsigned(intValue, _bitsUInt);
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
        /// Writes all of an existing BitStream's buffer to this BitStream.
        /// </summary>
        /// <param name="source">BitStream to read from.</param>
        public void Write(BitStream source)
        {
            // Write over the full bytes from the source
            var numFullBytes = source.LengthBits / _bitsByte;
            if (numFullBytes > 0)
                Write(source._buffer, 0, numFullBytes);

            // Write the remaining bits that don't make up a full byte
            var remainingBits = source.LengthBits % _bitsByte;
            if (remainingBits > 0)
            {
                var remainingData = source._buffer[source.Length - 1];
                for (var j = _highBit; j > _highBit - remainingBits; j--)
                {
                    Write((remainingData & (1 << j)) != 0);
                }
            }
        }

        /// <summary>
        /// Writes a value to the BitStream.
        /// </summary>
        /// <param name="value">Value to write to the BitStream.</param>
        /// <param name="numBits">Number of bits to write (1 to 32).</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="numBits"/> is less than one or greater than
        /// <see cref="_bitsInt"/>.</exception>
        public void Write(int value, int numBits)
        {
            if (numBits > _bitsInt || numBits < 1)
                throw new ArgumentOutOfRangeException("numBits", "Value must be between 1 and 32.");

            WriteSigned(value, numBits);
        }

        /// <summary>
        /// Writes a nullable value to the BitStream.
        /// </summary>
        /// <param name="value">Value to write to the BitStream.</param>
        /// <param name="numBits">Number of bits to write (1 to 32).</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="numBits"/> is less than one or greater than
        /// <see cref="_bitsInt"/>.</exception>
        public void Write(int? value, int numBits)
        {
            if (numBits > _bitsInt || numBits < 1)
                throw new ArgumentOutOfRangeException("numBits", "Value must be between 1 and 32.");

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
            WriteUnsigned(bit != 0 ? 1 : 0, 1);
        }

        /// <summary>
        /// Writes an Enum of type <typeparamref name="T"/>. Whether to use the Enum's underlying integer value or
        /// the name of the Enum value is determined from the <see cref="IValueWriter.UseEnumNames"/> property.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="value">Value to write.</param>
        public void WriteEnum<T>(T value) where T : struct, IComparable, IConvertible, IFormattable
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
        /// <param name="value">Value to write.</param>
        public void WriteEnumName<T>(T value) where T : struct, IComparable, IConvertible, IFormattable
        {
            EnumHelper<T>.WriteName(this, value);
        }

        /// <summary>
        /// Writes an Enum of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <param name="value">Value to write.</param>
        public void WriteEnumValue<T>(T value) where T : struct, IComparable, IConvertible, IFormattable
        {
            EnumHelper<T>.WriteValue(this, value);
        }
    }
}