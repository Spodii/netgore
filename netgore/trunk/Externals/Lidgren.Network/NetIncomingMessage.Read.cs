/* Copyright (c) 2010 Michael Lidgren

Permission is hereby granted, free of charge, to any person obtaining a copy of this software
and associated documentation files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or
substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE
USE OR OTHER DEALINGS IN THE SOFTWARE.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace Lidgren.Network
{
    public partial class NetIncomingMessage
    {
        const string c_readOverflowError =
            "Trying to read past the buffer size - likely caused by mismatching Write/Reads, different size or order.";

        static readonly Dictionary<Type, MethodInfo> s_readMethods;

        internal int m_readPosition;

        static NetIncomingMessage()
        {
            var integralTypes = typeof(Byte).Assembly.GetTypes();

            s_readMethods = new Dictionary<Type, MethodInfo>();
            var methods = typeof(NetIncomingMessage).GetMethods(BindingFlags.Instance | BindingFlags.Public);
            foreach (var mi in methods)
            {
                if (mi.GetParameters().Length == 0 && mi.Name.StartsWith("Read", StringComparison.InvariantCulture))
                {
                    var n = mi.Name.Substring(4);
                    foreach (var it in integralTypes)
                    {
                        if (it.Name == n)
                            s_readMethods[it] = mi;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the read position in the buffer, in bits (not bytes)
        /// </summary>
        public long Position
        {
            get { return m_readPosition; }
            set { m_readPosition = (int)value; }
        }

        /// <summary>
        /// Gets the position in the buffer in bytes; note that the bits of the first returned byte may already have been read - check the Position property to make sure.
        /// </summary>
        public int PositionInBytes
        {
            get { return (m_readPosition / 8); }
        }

        public void ReadBits(byte[] into, int offset, int numberOfBits)
        {
            NetException.Assert(m_bitLength - m_readPosition >= numberOfBits, c_readOverflowError);
            NetException.Assert(offset + NetUtility.BytesToHoldBits(numberOfBits) <= into.Length);

            var numberOfWholeBytes = numberOfBits / 8;
            var extraBits = numberOfBits - (numberOfWholeBytes * 8);

            NetBitWriter.ReadBytes(m_data, numberOfWholeBytes, m_readPosition, into, offset);
            m_readPosition += (8 * numberOfWholeBytes);

            if (extraBits > 0)
                into[offset + numberOfWholeBytes] = ReadByte(extraBits);

            return;
        }

        //
        // 1 bit
        //
        public bool ReadBoolean()
        {
            NetException.Assert(m_bitLength - m_readPosition >= 1, c_readOverflowError);
            var retval = NetBitWriter.ReadByte(m_data, 1, m_readPosition);
            m_readPosition += 1;
            return (retval > 0 ? true : false);
        }

        //
        // 8 bit 
        //
        public byte ReadByte()
        {
            NetException.Assert(m_bitLength - m_readPosition >= 8, c_readOverflowError);
            var retval = NetBitWriter.ReadByte(m_data, 8, m_readPosition);
            m_readPosition += 8;
            return retval;
        }

        public byte ReadByte(int numberOfBits)
        {
            var retval = NetBitWriter.ReadByte(m_data, numberOfBits, m_readPosition);
            m_readPosition += numberOfBits;
            return retval;
        }

        public byte[] ReadBytes(int numberOfBytes)
        {
            NetException.Assert(m_bitLength - m_readPosition >= (numberOfBytes * 8), c_readOverflowError);

            var retval = new byte[numberOfBytes];
            NetBitWriter.ReadBytes(m_data, numberOfBytes, m_readPosition, retval, 0);
            m_readPosition += (8 * numberOfBytes);
            return retval;
        }

        public void ReadBytes(byte[] into, int offset, int numberOfBytes)
        {
            NetException.Assert(m_bitLength - m_readPosition >= (numberOfBytes * 8), c_readOverflowError);
            NetException.Assert(offset + numberOfBytes <= into.Length);

            NetBitWriter.ReadBytes(m_data, numberOfBytes, m_readPosition, into, offset);
            m_readPosition += (8 * numberOfBytes);
            return;
        }

        public double ReadDouble()
        {
            NetException.Assert(m_bitLength - m_readPosition >= 64, c_readOverflowError);

            if ((m_readPosition & 7) == 0) // read directly
            {
                // read directly
                var retval = BitConverter.ToDouble(m_data, m_readPosition >> 3);
                m_readPosition += 64;
                return retval;
            }

            var bytes = ReadBytes(8);
            return BitConverter.ToDouble(bytes, 0); // endianness is handled inside BitConverter.ToSingle
        }

        public float ReadFloat()
        {
            return ReadSingle();
        }

        /// <summary>
        /// Reads a stored IPv4 endpoint description
        /// </summary>
        public IPEndPoint ReadIPEndpoint()
        {
            var len = ReadByte();
            var addressBytes = ReadBytes(len);
            int port = ReadUInt16();

            var address = new IPAddress(addressBytes);
            return new IPEndPoint(address, port);
        }

        //
        // 16 bit
        //
        public Int16 ReadInt16()
        {
            NetException.Assert(m_bitLength - m_readPosition >= 16, c_readOverflowError);
            var retval = NetBitWriter.ReadUInt32(m_data, 16, m_readPosition);
            m_readPosition += 16;
            return (short)retval;
        }

        //
        // 32 bit
        //
        public Int32 ReadInt32()
        {
            NetException.Assert(m_bitLength - m_readPosition >= 32, c_readOverflowError);
            var retval = NetBitWriter.ReadUInt32(m_data, 32, m_readPosition);
            m_readPosition += 32;
            return (Int32)retval;
        }

        public Int32 ReadInt32(int numberOfBits)
        {
            NetException.Assert((numberOfBits > 0 && numberOfBits <= 32), "ReadInt() can only read between 1 and 32 bits");
            NetException.Assert(m_bitLength - m_readPosition >= numberOfBits, c_readOverflowError);

            var retval = NetBitWriter.ReadUInt32(m_data, numberOfBits, m_readPosition);
            m_readPosition += numberOfBits;

            if (numberOfBits == 32)
                return (int)retval;

            var signBit = 1 << (numberOfBits - 1);
            if ((retval & signBit) == 0)
                return (int)retval; // positive

            // negative
            unchecked
            {
                var mask = ((uint)-1) >> (33 - numberOfBits);
                var tmp = (retval & mask) + 1;
                return -((int)tmp);
            }
        }

        public Int64 ReadInt64()
        {
            NetException.Assert(m_bitLength - m_readPosition >= 64, c_readOverflowError);
            unchecked
            {
                var retval = ReadUInt64();
                var longRetval = (long)retval;
                return longRetval;
            }
        }

        public Int64 ReadInt64(int numberOfBits)
        {
            NetException.Assert(((numberOfBits > 0) && (numberOfBits < 65)), "ReadInt64(bits) can only read between 1 and 64 bits");
            return (long)ReadUInt64(numberOfBits);
        }

        /// <summary>
        /// Reads an integer written using WriteRangedInteger() using the same min/max values
        /// </summary>
        public int ReadRangedInteger(int min, int max)
        {
            var range = (uint)(max - min);
            var numBits = NetUtility.BitsToHoldUInt(range);

            var rvalue = ReadUInt32(numBits);
            return (int)(min + rvalue);
        }

        /// <summary>
        /// Reads a float written using WriteRangedSingle() using the same MIN and MAX values
        /// </summary>
        public float ReadRangedSingle(float min, float max, int numberOfBits)
        {
            var range = max - min;
            var maxVal = (1 << numberOfBits) - 1;
            float encodedVal = ReadUInt32(numberOfBits);
            var unit = encodedVal / maxVal;
            return min + (unit * range);
        }

        [CLSCompliant(false)]
        public sbyte ReadSByte()
        {
            NetException.Assert(m_bitLength - m_readPosition >= 8, c_readOverflowError);
            var retval = NetBitWriter.ReadByte(m_data, 8, m_readPosition);
            m_readPosition += 8;
            return (sbyte)retval;
        }

        /// <summary>
        /// Reads a float written using WriteSignedSingle()
        /// </summary>
        public float ReadSignedSingle(int numberOfBits)
        {
            var encodedVal = ReadUInt32(numberOfBits);
            var maxVal = (1 << numberOfBits) - 1;
            return ((encodedVal + 1) / (float)(maxVal + 1) - 0.5f) * 2.0f;
        }

        public float ReadSingle()
        {
            NetException.Assert(m_bitLength - m_readPosition >= 32, c_readOverflowError);

            if ((m_readPosition & 7) == 0) // read directly
            {
                // endianness is handled inside BitConverter.ToSingle
                var retval = BitConverter.ToSingle(m_data, m_readPosition >> 3);
                m_readPosition += 32;
                return retval;
            }

            var bytes = ReadBytes(4);
            return BitConverter.ToSingle(bytes, 0); // endianness is handled inside BitConverter.ToSingle
        }

        /// <summary>
        /// Reads a string
        /// </summary>
        public string ReadString()
        {
            var byteLen = (int)ReadVariableUInt32();

            if (byteLen == 0)
                return String.Empty;

            NetException.Assert(m_bitLength - m_readPosition >= (byteLen * 8), c_readOverflowError);

            if ((m_readPosition & 7) == 0)
            {
                // read directly
                var retval = Encoding.UTF8.GetString(m_data, m_readPosition >> 3, byteLen);
                m_readPosition += (8 * byteLen);
                return retval;
            }

            var bytes = ReadBytes(byteLen);
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        [CLSCompliant(false)]
        public UInt16 ReadUInt16()
        {
            NetException.Assert(m_bitLength - m_readPosition >= 16, c_readOverflowError);
            var retval = NetBitWriter.ReadUInt32(m_data, 16, m_readPosition);
            m_readPosition += 16;
            return (ushort)retval;
        }

        [CLSCompliant(false)]
        public UInt32 ReadUInt32()
        {
            NetException.Assert(m_bitLength - m_readPosition >= 32, c_readOverflowError);
            var retval = NetBitWriter.ReadUInt32(m_data, 32, m_readPosition);
            m_readPosition += 32;
            return retval;
        }

        [CLSCompliant(false)]
        public UInt32 ReadUInt32(int numberOfBits)
        {
            NetException.Assert((numberOfBits > 0 && numberOfBits <= 32), "ReadUInt() can only read between 1 and 32 bits");
            //NetException.Assert(m_bitLength - m_readBitPtr >= numberOfBits, "tried to read past buffer size");

            var retval = NetBitWriter.ReadUInt32(m_data, numberOfBits, m_readPosition);
            m_readPosition += numberOfBits;
            return retval;
        }

        //
        // 64 bit
        //
        [CLSCompliant(false)]
        public UInt64 ReadUInt64()
        {
            NetException.Assert(m_bitLength - m_readPosition >= 64, c_readOverflowError);

            ulong low = NetBitWriter.ReadUInt32(m_data, 32, m_readPosition);
            m_readPosition += 32;
            ulong high = NetBitWriter.ReadUInt32(m_data, 32, m_readPosition);

            var retval = low + (high << 32);

            m_readPosition += 32;
            return retval;
        }

        [CLSCompliant(false)]
        public UInt64 ReadUInt64(int numberOfBits)
        {
            NetException.Assert((numberOfBits > 0 && numberOfBits <= 64), "ReadUInt() can only read between 1 and 64 bits");
            NetException.Assert(m_bitLength - m_readPosition >= numberOfBits, c_readOverflowError);

            ulong retval;
            if (numberOfBits <= 32)
                retval = NetBitWriter.ReadUInt32(m_data, numberOfBits, m_readPosition);
            else
            {
                retval = NetBitWriter.ReadUInt32(m_data, 32, m_readPosition);
                retval |= NetBitWriter.ReadUInt32(m_data, numberOfBits - 32, m_readPosition) << 32;
            }
            m_readPosition += numberOfBits;
            return retval;
        }

        /// <summary>
        /// Reads a float written using WriteUnitSingle()
        /// </summary>
        public float ReadUnitSingle(int numberOfBits)
        {
            var encodedVal = ReadUInt32(numberOfBits);
            var maxVal = (1 << numberOfBits) - 1;
            return (encodedVal + 1) / (float)(maxVal + 1);
        }

        /// <summary>
        /// Reads a Int32 written using WriteVariableInt32()
        /// </summary>
        public int ReadVariableInt32()
        {
            var num1 = 0;
            var num2 = 0;
            while (true)
            {
                if (num2 == 0x23)
                    throw new FormatException("Bad 7-bit encoded integer");

                var num3 = ReadByte();
                num1 |= (num3 & 0x7f) << (num2 & 0x1f);
                num2 += 7;
                if ((num3 & 0x80) == 0)
                {
                    var sign = (num1 << 31) >> 31;
                    return sign ^ (num1 >> 1);
                }
            }
        }

        /// <summary>
        /// Reads a UInt32 written using WriteVariableUInt32()
        /// </summary>
        [CLSCompliant(false)]
        public uint ReadVariableUInt32()
        {
            var num1 = 0;
            var num2 = 0;
            while (true)
            {
                if (num2 == 0x23)
                    throw new FormatException("Bad 7-bit encoded integer");

                var num3 = ReadByte();
                num1 |= (num3 & 0x7f) << num2;
                num2 += 7;
                if ((num3 & 0x80) == 0)
                    return (uint)num1;
            }
        }

        /// <summary>
        /// Reads a UInt32 written using WriteVariableInt64()
        /// </summary>
        [CLSCompliant(false)]
        public UInt64 ReadVariableUInt64()
        {
            UInt64 num1 = 0;
            var num2 = 0;
            while (true)
            {
                if (num2 == 0x23)
                    throw new FormatException("Bad 7-bit encoded integer");

                var num3 = ReadByte();
                num1 |= ((UInt64)num3 & 0x7f) << num2;
                num2 += 7;
                if ((num3 & 0x80) == 0)
                    return num1;
            }
        }

        /// <summary>
        /// Pads data with enough bits to reach a full byte. Decreases cpu usage for subsequent byte writes.
        /// </summary>
        public void SkipPadBits()
        {
            m_readPosition = ((m_readPosition + 7) >> 3) * 8;
        }

        /// <summary>
        /// Pads data with the specified number of bits.
        /// </summary>
        public void SkipPadBits(int numberOfBits)
        {
            m_readPosition += numberOfBits;
        }
    }
}