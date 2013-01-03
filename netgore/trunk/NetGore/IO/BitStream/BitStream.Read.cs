using System;
using System.Diagnostics;
using System.Linq;

namespace NetGore.IO
{
    public partial class BitStream
    {
        /// <summary>
        /// Reads the next bit in the BitStream as an int.
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1).</returns>
        public int ReadBit()
        {
            return ReadUnsigned(1);
        }

        /// <summary>
        /// Reads the next bit in the BitStream as a byte.
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1).</returns>
        public byte ReadBitAsByte()
        {
            return (byte)ReadBit();
        }

        /// <summary>
        /// Reads the next bit in the BitStream as a char.
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1).</returns>
        public char ReadBitAsChar()
        {
            return (char)ReadBit();
        }

        /// <summary>
        /// Reads the next bit in the BitStream as a double.
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1).</returns>
        public double ReadBitAsDouble()
        {
            return ReadBit();
        }

        /// <summary>
        /// Reads the next bit in the BitStream as a float.
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1).</returns>
        public float ReadBitAsFloat()
        {
            return ReadBit();
        }

        /// <summary>
        /// Reads the next bit in the BitStream as an int.
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1).</returns>
        public int ReadBitAsInt()
        {
            return ReadBit();
        }

        /// <summary>
        /// Reads the next bit in the BitStream as a long.
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1).</returns>
        public long ReadBitAsLong()
        {
            return ReadBit();
        }

        /// <summary>
        /// Reads the next bit in the BitStream as a SByte.
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1).</returns>
        public sbyte ReadBitAsSByte()
        {
            return (sbyte)ReadBit();
        }

        /// <summary>
        /// Reads the next bit in the BitStream as a short.
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1).</returns>
        public short ReadBitAsShort()
        {
            return (short)ReadBit();
        }

        /// <summary>
        /// Reads the next bit in the BitStream as a string.
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1).</returns>
        public string ReadBitAsString()
        {
            return ReadBit().ToString();
        }

        /// <summary>
        /// Reads the next bit in the BitStream as a uint.
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1).</returns>
        public uint ReadBitAsUInt()
        {
            return (uint)ReadBit();
        }

        /// <summary>
        /// Reads the next bit in the BitStream as a ulong.
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1).</returns>
        public ulong ReadBitAsULong()
        {
            return (ulong)ReadBit();
        }

        /// <summary>
        /// Reads the next bit in the BitStream as a UShort.
        /// </summary>
        /// <returns>Value of the next bit in the BitStream (0 or 1).</returns>
        public ushort ReadBitAsUShort()
        {
            return (ushort)ReadBit();
        }

        /// <summary>
        /// Reads the specified number of bits from the BitStream.
        /// </summary>
        /// <param name="bitLength">The number of bits to read.</param>
        /// <returns>A BitStream filled with the bits read.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><c>bitLength</c> is less than zero.</exception>
        public BitStream ReadBits(int bitLength)
        {
            if (bitLength < 0)
                throw new ArgumentOutOfRangeException("bitLength");
            if (bitLength == 0)
                return new BitStream();

#if DEBUG
            var initialBitPosition = PositionBits;
#endif

            var retSizeBytes = Math.Max(16, BitOps.NextPowerOf2(bitLength / 8));
            var ret = new BitStream(retSizeBytes);

            var bitsRemaining = bitLength;

            // Read full 32-bit integers at a time
            while (bitsRemaining > _bitsUInt)
            {
                ret.Write(ReadUInt());
                bitsRemaining -= _bitsUInt;
            }

            // Read the remainder
            if (bitsRemaining > 0)
            {
                var partialValue = ReadUInt(bitsRemaining);
                ret.Write(partialValue, bitsRemaining);
            }

#if DEBUG
            Debug.Assert(ret.LengthBits == bitLength);
            Debug.Assert(PositionBits - initialBitPosition == bitLength);
#endif

            ret.PositionBits = 0;

            return ret;
        }

        /// <summary>
        /// Reads a bool (one bit) from the BitStream.
        /// </summary>
        /// <returns>True if the bit is set, else false.</returns>
        public bool ReadBool()
        {
            return ReadBit() != 0 ? true : false;
        }

        /// <summary>
        /// Reads an array of bools from the BitStream.
        /// </summary>
        /// <param name="dest">Array to store the read values in.</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array.</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array.</param>
        public void ReadBool(bool[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadBool();
            }
        }

        /// <summary>
        /// Reads an array of nullable bools from the BitStream.
        /// </summary>
        /// <param name="dest">Array to store the read values in.</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array.</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array.</param>
        public void ReadBool(bool?[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadNullableBool();
            }
        }

        /// <summary>
        /// Reads an array of bools.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <returns>
        /// The array of bools.
        /// </returns>
        public bool[] ReadBools(int count)
        {
            var ret = new bool[count];
            for (var i = 0; i < ret.Length; i++)
            {
                ret[i] = ReadBool();
            }
            return ret;
        }

        /// <summary>
        /// Reads <paramref name="numBits"/> bits from the BitStream and returns the result as a byte.
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 8).</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as a byte.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><c>numBits</c> is less than zero or greater than <see cref="_bitsByte"/>.</exception>
        public byte ReadByte(int numBits = 8)
        {
            if (numBits == 0)
                return 0;
            if (numBits > _bitsByte)
                throw new ArgumentOutOfRangeException("numBits", string.Format("Invalid number of bits specified ({0})", numBits));
            return (byte)ReadUnsigned(numBits);
        }

        /// <summary>
        /// Reads an array of bytes from the BitStream.
        /// </summary>
        /// <param name="dest">Array to store the read values in.</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array.</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array.</param>
        public void ReadByte(byte[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadByte();
            }
        }

        /// <summary>
        /// Reads an array of bytes from the BitStream.
        /// </summary>
        /// <param name="length">Number of bytes to read.</param>
        /// <returns>Array of bytes read with a length equal to <paramref name="length"/>.</returns>
        public byte[] ReadBytes(int length)
        {
            var b = new byte[length];
            ReadByte(b, 0, length);
            return b;
        }

        /// <summary>
        /// Reads 64 bits from the BitStream and returns the result as a double.
        /// </summary>
        /// <returns>Value of the next 64 bits in the BitStream as a double.</returns>
        public double ReadDouble()
        {
            var b = ReadBytes(sizeof(double));
            return BitConverter.ToDouble(b, 0);
        }

        /// <summary>
        /// Reads an array of doubles from the BitStream.
        /// </summary>
        /// <param name="dest">Array to store the read values in.</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array.</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array.</param>
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
        /// <returns>Value read from the reader.</returns>
        public T ReadEnum<T>() where T : struct, IComparable, IConvertible, IFormattable
        {
            if (UseEnumNames)
                return ReadEnumName<T>();
            else
                return ReadEnumValue<T>();
        }

        /// <summary>
        /// Reads an Enum of type <typeparamref name="T"/> using the Enum's name instead of the value.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <returns>Value read from the reader.</returns>
        public T ReadEnumName<T>() where T : struct, IComparable, IConvertible, IFormattable
        {
            return EnumHelper<T>.ReadName(this);
        }

        /// <summary>
        /// Reads an Enum of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The Type of Enum.</typeparam>
        /// <returns>Value read from the reader.</returns>
        public T ReadEnumValue<T>() where T : struct, IComparable, IConvertible, IFormattable
        {
            return EnumHelper<T>.ReadValue(this);
        }

        /// <summary>
        /// Reads 32 bits from the BitStream and returns the result as a float.
        /// </summary>
        /// <returns>Value of the next 32 bits in the BitStream as a float.</returns>
        public unsafe float ReadFloat()
        {
            int intValue = ReadUnsigned(_bitsUInt);
            return *((float*)&intValue);
        }

        /// <summary>
        /// Reads an array of floats from the BitStream.
        /// </summary>
        /// <param name="dest">Array to store the read values in.</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array.</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array.</param>
        public void ReadFloat(float[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadFloat();
            }
        }

        /// <summary>
        /// Reads an array of floats.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <returns>
        /// The array of floats.
        /// </returns>
        public float[] ReadFloats(int count)
        {
            var ret = new float[count];
            for (var i = 0; i < ret.Length; i++)
            {
                ret[i] = ReadFloat();
            }
            return ret;
        }

        /// <summary>
        /// Reads <paramref name="numBits"/> bits from the BitStream and returns the result as an int.
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 32).</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as an int.</returns>
        public int ReadInt(int numBits = _bitsInt)
        {
            return ReadSigned(numBits, _bitsInt);
        }

        /// <summary>
        /// Reads an array of ints from the BitStream.
        /// </summary>
        /// <param name="dest">Array to store the read values in.</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array.</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array.</param>
        public void ReadInt(int[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadInt();
            }
        }

        /// <summary>
        /// Reads an array of 32-bit integers.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <returns>
        /// The array of 32-bit integers read from the BitStream.
        /// </returns>
        public int[] ReadInts(int count)
        {
            var ret = new int[count];
            for (var i = 0; i < ret.Length; i++)
            {
                ret[i] = ReadInt();
            }
            return ret;
        }

        /// <summary>
        /// Reads 64 bits from the BitStream and returns the result as a long.
        /// </summary>
        /// <returns>Value of the next 64 bits in the BitStream as a long.</returns>
        public long ReadLong()
        {
            var b = ReadBytes(sizeof(long));
            return BitConverter.ToInt64(b, 0);
        }

        /// <summary>
        /// Reads an array of longs from the BitStream.
        /// </summary>
        /// <param name="dest">Array to store the read values in.</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array.</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array.</param>
        public void ReadLong(long[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadLong();
            }
        }

        /// <summary>
        /// Reads an array of nullable longs from the BitStream.
        /// </summary>
        /// <param name="dest">Array to store the read values in.</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array.</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array.</param>
        public void ReadLong(long?[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadNullableLong();
            }
        }

        /// <summary>
        /// Reads an array of longs.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <returns>
        /// The array of longs.
        /// </returns>
        public long[] ReadLongs(int count)
        {
            var ret = new long[count];
            for (var i = 0; i < ret.Length; i++)
            {
                ret[i] = ReadLong();
            }
            return ret;
        }

        /// <summary>
        /// Reads a nullable bool (one bit) from the BitStream.
        /// </summary>
        /// <returns>True if the bit is set, else false.</returns>
        public bool? ReadNullableBool()
        {
            var hasValue = ReadBool();
            if (!hasValue)
                return null;

            return ReadBit() != 0 ? true : false;
        }

        /// <summary>
        /// Reads a nullable byte from the BitStream and returns the result as a byte.
        /// </summary>
        /// <returns>Value of the next 8 bits in the BitStream as a byte.</returns>
        public byte? ReadNullableByte()
        {
            var hasValue = ReadBool();
            if (!hasValue)
                return null;

            return ReadByte();
        }

        /// <summary>
        /// Reads <paramref name="numBits"/> bits of a nullable value from the BitStream and returns the result as a byte.
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 8).</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as a byte.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><c>numBits</c> is less than zero or greater than <see cref="_bitsByte"/>.</exception>
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
        /// Reads an array of nullable bytes from the BitStream.
        /// </summary>
        /// <param name="dest">Array to store the read values in.</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array.</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array.</param>
        public void ReadNullableByte(byte?[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadNullableByte();
            }
        }

        /// <summary>
        /// Reads an array of nullable bytes from the BitStream.
        /// </summary>
        /// <param name="length">Number of bytes to read.</param>
        /// <returns>Array of bytes read with a length equal to <paramref name="length"/>.</returns>
        public byte?[] ReadNullableBytes(int length)
        {
            var b = new byte?[length];
            ReadNullableByte(b, 0, length);
            return b;
        }

        /// <summary>
        /// Reads 64 bits of a nullable value from the BitStream and returns the result as a double .
        /// </summary>
        /// <returns>Value of the next 64 bits in the BitStream as a double.</returns>
        public double? ReadNullableDouble()
        {
            var hasValue = ReadBool();
            if (!hasValue)
                return null;

            return ReadDouble();
        }

        /// <summary>
        /// Reads an array of nullable doubles from the BitStream.
        /// </summary>
        /// <param name="dest">Array to store the read values in.</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array.</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array.</param>
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
        /// Reads 32 bits of a nullable value from the BitStream and returns the result as a float.
        /// </summary>
        /// <returns>Value of the next 32 bits in the BitStream as a float.</returns>
        public float? ReadNullableFloat()
        {
            var hasValue = ReadBool();
            if (!hasValue)
                return null;

            return ReadFloat();
        }

        /// <summary>
        /// Reads an array of nullable floats from the BitStream.
        /// </summary>
        /// <param name="dest">Array to store the read values in.</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array.</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array.</param>
        public void ReadNullableFloat(float?[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadNullableFloat();
            }
        }

        /// <summary>
        /// Reads <paramref name="numBits"/> bits of a nullable value from the BitStream and returns the result as an int.
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 32).</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as an int.</returns>
        public int? ReadNullableInt(int numBits = _bitsInt)
        {
            var hasValue = ReadBool();
            if (!hasValue)
                return null;

            return ReadSigned(numBits, _bitsInt);
        }

        /// <summary>
        /// Reads an array of nullable ints from the BitStream.
        /// </summary>
        /// <param name="dest">Array to store the read values in.</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array.</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array.</param>
        public void ReadNullableInt(int?[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadNullableInt();
            }
        }

        /// <summary>
        /// Reads 64 bits of a nullable value from the BitStream and returns the result as a long.
        /// </summary>
        /// <returns>Value of the next 64 bits in the BitStream as a long.</returns>
        public long? ReadNullableLong()
        {
            var hasValue = ReadBool();
            if (!hasValue)
                return null;

            return ReadLong();
        }

        /// <summary>
        /// Reads <paramref name="numBits"/> bits of a nullable value from the BitStream and returns the result as a SByte.
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 8).</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as a SByte.</returns>
        public sbyte? ReadNullableSByte(int numBits = _bitsSByte)
        {
            var hasValue = ReadBool();
            if (!hasValue)
                return null;

            return (sbyte)ReadSigned(numBits, _bitsSByte);
        }

        /// <summary>
        /// Reads <paramref name="numBits"/> bits of a nullable value from the BitStream and returns the result as a short.
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 16).</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as a short.</returns>
        public short? ReadNullableShort(int numBits = _bitsShort)
        {
            var hasValue = ReadBool();
            if (!hasValue)
                return null;

            return (short)ReadSigned(numBits, _bitsShort);
        }

        /// <summary>
        /// Reads an array of nullable shorts from the BitStream.
        /// </summary>
        /// <param name="dest">Array to store the read values in.</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array.</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array.</param>
        public void ReadNullableShort(short?[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadNullableShort();
            }
        }

        /// <summary>
        /// Reads <paramref name="numBits"/> bits of a nullable value from the BitStream and returns the result as a uint.
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 32).</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as a uint.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><c>numBits</c> is less than zero or greater than <see cref="_bitsInt"/>.</exception>
        public uint? ReadNullableUInt(int numBits = _bitsUInt)
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
        /// Reads an array of nullable uints from the BitStream.
        /// </summary>
        /// <param name="dest">Array to store the read values in.</param>
        /// <param name="offset">Initial index to write the results to in the <paramref name="dest"/> array.</param>
        /// <param name="length">Number of values to read and write to the <paramref name="dest"/> array.</param>
        public void ReadNullableUInt(uint?[] dest, int offset, int length)
        {
            for (var i = offset; i < offset + length; i++)
            {
                dest[i] = ReadNullableUInt();
            }
        }

        /// <summary>
        /// Reads 64 bits of a nullable value from the BitStream and returns the result as a ulong.
        /// </summary>
        /// <returns>Value of the next 64 bits in the BitStream as a ulong.</returns>
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

            return ReadUShort();
        }

        /// <summary>
        /// Reads <paramref name="numBits"/> bits of a nullable value from the BitStream and returns the result as a ushort  
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 16)</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as a ushort</returns>
        /// <exception cref="ArgumentOutOfRangeException"><c>numBits</c> is less than zero or greater than <see cref="_bitsUShort"/>.</exception>
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
        /// Reads <paramref name="numBits"/> bits from the BitStream and returns the result as a SByte  
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 8)</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as a SByte</returns>
        public sbyte ReadSByte(int numBits = _bitsSByte)
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
        /// Reads an array of signed 8-bit integers.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <returns>
        /// The array of signed 8-bit integers read from the BitStream.
        /// </returns>
        public sbyte[] ReadSBytes(int count)
        {
            var ret = new sbyte[count];
            for (var i = 0; i < ret.Length; i++)
            {
                ret[i] = ReadSByte();
            }
            return ret;
        }

        /// <summary>
        /// Reads <paramref name="numBits"/> bits from the BitStream and returns the result as a short  
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 16)</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as a short</returns>
        public short ReadShort(int numBits = _bitsShort)
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
        /// Reads an array of 16-bit integers.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <returns>
        /// The array of 16-bit integers read from the BitStream.
        /// </returns>
        public short[] ReadShorts(int count)
        {
            var ret = new short[count];
            for (var i = 0; i < ret.Length; i++)
            {
                ret[i] = ReadShort();
            }
            return ret;
        }

        /// <summary>
        /// Reads a string containing a maximum of <paramref name="maxLength"/> characters from the BitStream
        /// </summary>
        /// <param name="maxLength">Maximum length of the string in characters. Must be equal to the specified
        /// number of characters used when writing this string.</param>
        /// <returns>String read from the BitStream</returns>
        public string ReadString(int maxLength = DefaultStringMaxLength)
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
        /// Reads an array of strings containing a maximum of DefaultStringMaxLength characters from the BitStream
        /// </summary>
        /// <param name="count">The number of strings to read.</param>
        /// <returns>Strings read from the BitStream.</returns>
        public string[] ReadStrings(int count)
        {
            var ret = new string[count];
            for (var i = 0; i < ret.Length; i++)
            {
                ret[i] = ReadString();
            }
            return ret;
        }

        /// <summary>
        /// Reads <paramref name="numBits"/> bits from the BitStream and returns the result as a uint  
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 32)</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as a uint</returns>
        /// <exception cref="ArgumentOutOfRangeException"><c>numBits</c> is less than zero or greater than <see cref="_bitsInt"/>.</exception>
        public uint ReadUInt(int numBits = _bitsUInt)
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
        /// Reads an array of unsigned 32-bit integers.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <returns>
        /// The array of unsigned 32-bit integers read from the BitStream.
        /// </returns>
        public uint[] ReadUInts(int count)
        {
            var ret = new uint[count];
            for (var i = 0; i < ret.Length; i++)
            {
                ret[i] = ReadUInt();
            }
            return ret;
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
        /// Reads an array of ulongs.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <returns>
        /// The array of ulongs.
        /// </returns>
        public ulong[] ReadULongs(int count)
        {
            var ret = new ulong[count];
            for (var i = 0; i < ret.Length; i++)
            {
                ret[i] = ReadULong();
            }
            return ret;
        }

        /// <summary>
        /// Reads <paramref name="numBits"/> bits from the BitStream and returns the result as a ushort  
        /// </summary>
        /// <param name="numBits">Number of bits to read from the BitStream (0 to 16)</param>
        /// <returns>Value of the next <paramref name="numBits"/> bits in the BitStream as a ushort</returns>
        /// <exception cref="ArgumentOutOfRangeException"><c>numBits</c> is less than zero or greater than <see cref="_bitsUShort"/>.</exception>
        public ushort ReadUShort(int numBits = _bitsUShort)
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

        /// <summary>
        /// Reads an array of unsigned 16-bit integers.
        /// </summary>
        /// <param name="count">The number of values to read.</param>
        /// <returns>
        /// The array of unsigned 16-bit integers read from the BitStream.
        /// </returns>
        public ushort[] ReadUShorts(int count)
        {
            var ret = new ushort[count];
            for (var i = 0; i < ret.Length; i++)
            {
                ret[i] = ReadUShort();
            }
            return ret;
        }
    }
}