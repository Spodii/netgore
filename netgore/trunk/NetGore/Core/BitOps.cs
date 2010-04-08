using System.Linq;

namespace NetGore
{
    /// <summary>
    /// Helper methods for performing operations on bits.
    /// </summary>
    public static class BitOps
    {
        /// <summary>
        /// Table used for reversing bits.
        /// </summary>
        static readonly byte[] BitReverseTable256 = new byte[]
        {
            0x00, 0x80, 0x40, 0xC0, 0x20, 0xA0, 0x60, 0xE0, 0x10, 0x90, 0x50, 0xD0, 0x30, 0xB0, 0x70, 0xF0, 0x08, 0x88, 0x48, 0xC8,
            0x28, 0xA8, 0x68, 0xE8, 0x18, 0x98, 0x58, 0xD8, 0x38, 0xB8, 0x78, 0xF8, 0x04, 0x84, 0x44, 0xC4, 0x24, 0xA4, 0x64, 0xE4
            , 0x14, 0x94, 0x54, 0xD4, 0x34, 0xB4, 0x74, 0xF4, 0x0C, 0x8C, 0x4C, 0xCC, 0x2C, 0xAC, 0x6C, 0xEC, 0x1C, 0x9C, 0x5C,
            0xDC, 0x3C, 0xBC, 0x7C, 0xFC, 0x02, 0x82, 0x42, 0xC2, 0x22, 0xA2, 0x62, 0xE2, 0x12, 0x92, 0x52, 0xD2, 0x32, 0xB2, 0x72
            , 0xF2, 0x0A, 0x8A, 0x4A, 0xCA, 0x2A, 0xAA, 0x6A, 0xEA, 0x1A, 0x9A, 0x5A, 0xDA, 0x3A, 0xBA, 0x7A, 0xFA, 0x06, 0x86,
            0x46, 0xC6, 0x26, 0xA6, 0x66, 0xE6, 0x16, 0x96, 0x56, 0xD6, 0x36, 0xB6, 0x76, 0xF6, 0x0E, 0x8E, 0x4E, 0xCE, 0x2E, 0xAE
            , 0x6E, 0xEE, 0x1E, 0x9E, 0x5E, 0xDE, 0x3E, 0xBE, 0x7E, 0xFE, 0x01, 0x81, 0x41, 0xC1, 0x21, 0xA1, 0x61, 0xE1, 0x11,
            0x91, 0x51, 0xD1, 0x31, 0xB1, 0x71, 0xF1, 0x09, 0x89, 0x49, 0xC9, 0x29, 0xA9, 0x69, 0xE9, 0x19, 0x99, 0x59, 0xD9, 0x39
            , 0xB9, 0x79, 0xF9, 0x05, 0x85, 0x45, 0xC5, 0x25, 0xA5, 0x65, 0xE5, 0x15, 0x95, 0x55, 0xD5, 0x35, 0xB5, 0x75, 0xF5,
            0x0D, 0x8D, 0x4D, 0xCD, 0x2D, 0xAD, 0x6D, 0xED, 0x1D, 0x9D, 0x5D, 0xDD, 0x3D, 0xBD, 0x7D, 0xFD, 0x03, 0x83, 0x43, 0xC3
            , 0x23, 0xA3, 0x63, 0xE3, 0x13, 0x93, 0x53, 0xD3, 0x33, 0xB3, 0x73, 0xF3, 0x0B, 0x8B, 0x4B, 0xCB, 0x2B, 0xAB, 0x6B,
            0xEB, 0x1B, 0x9B, 0x5B, 0xDB, 0x3B, 0xBB, 0x7B, 0xFB, 0x07, 0x87, 0x47, 0xC7, 0x27, 0xA7, 0x67, 0xE7, 0x17, 0x97, 0x57
            , 0xD7, 0x37, 0xB7, 0x77, 0xF7, 0x0F, 0x8F, 0x4F, 0xCF, 0x2F, 0xAF, 0x6F, 0xEF, 0x1F, 0x9F, 0x5F, 0xDF, 0x3F, 0xBF,
            0x7F, 0xFF
        };

        /// <summary>
        /// Table used for finding the required number of bits for a value.
        /// </summary>
        static readonly byte[] LogTable256 = new byte[]
        {
            0, 0, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5,
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6
            , 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7
            , 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7
            , 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7
        };

        /// <summary>
        /// Counts the number of set (bit = 1) bits in a given value.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>Number of set (1) bits.</returns>
        public static int CountBits(long value)
        {
            int i;
            for (i = 0; value != 0; i++)
            {
                value &= value - 1;
            }

            return i;
        }

        /// <summary>
        /// Counts the number of set (bit = 1) bits in a given value.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>Number of set (1) bits.</returns>
        public static int CountBits(ulong value)
        {
            int i;
            for (i = 0; value != 0; i++)
            {
                value &= value - 1;
            }

            return i;
        }

        /// <summary>
        /// Counts the number of set (bit = 1) bits in a given value.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>Number of set (1) bits.</returns>
        public static int CountBits(int value)
        {
            int i;
            for (i = 0; value != 0; i++)
            {
                value &= value - 1;
            }

            return i;
        }

        /// <summary>
        /// Counts the number of set (bit = 1) bits in a given value.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>Number of set (1) bits.</returns>
        public static int CountBits(uint value)
        {
            int i;
            for (i = 0; value != 0; i++)
            {
                value &= value - 1;
            }

            return i;
        }

        /// <summary>
        /// Counts the number of set (bit = 1) bits in a given value.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>Number of set (1) bits.</returns>
        public static int CountBits(short value)
        {
            int i;
            for (i = 0; value != 0; i++)
            {
                value &= (short)(value - 1);
            }

            return i;
        }

        /// <summary>
        /// Counts the number of set (bit = 1) bits in a given value.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>Number of set (1) bits.</returns>
        public static int CountBits(ushort value)
        {
            int i;
            for (i = 0; value != 0; i++)
            {
                value &= (ushort)(value - 1);
            }

            return i;
        }

        /// <summary>
        /// Counts the number of set (bit = 1) bits in a given value.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>Number of set (1) bits.</returns>
        public static int CountBits(byte value)
        {
            int i;
            for (i = 0; value != 0; i++)
            {
                value &= (byte)(value - 1);
            }

            return i;
        }

        /// <summary>
        /// Counts the number of set (bit = 1) bits in a given value.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>Number of set (1) bits.</returns>
        public static int CountBits(sbyte value)
        {
            int i;
            for (i = 0; value != 0; i++)
            {
                value &= (sbyte)(value - 1);
            }

            return i;
        }

        /// <summary>
        /// Checks if a value is a power of two.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True if a power of 2, else false.</returns>
        public static bool IsPowerOf2(long value)
        {
            return (value > 0) && ((value & (value - 1)) == 0);
        }

        /// <summary>
        /// Checks if a value is a power of two.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True if a power of 2, else false.</returns>
        public static bool IsPowerOf2(ulong value)
        {
            return (value > 0) && ((value & (value - 1)) == 0);
        }

        /// <summary>
        /// Checks if a value is a power of two.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True if a power of 2, else false.</returns>
        public static bool IsPowerOf2(int value)
        {
            return (value > 0) && ((value & (value - 1)) == 0);
        }

        /// <summary>
        /// Checks if a value is a power of two.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True if a power of 2, else false.</returns>
        public static bool IsPowerOf2(uint value)
        {
            return (value > 0) && ((value & (value - 1)) == 0);
        }

        /// <summary>
        /// Checks if a value is a power of two.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True if a power of 2, else false.</returns>
        public static bool IsPowerOf2(short value)
        {
            return (value > 0) && ((value & (value - 1)) == 0);
        }

        /// <summary>
        /// Checks if a value is a power of two.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True if a power of 2, else false.</returns>
        public static bool IsPowerOf2(ushort value)
        {
            return (value > 0) && ((value & (value - 1)) == 0);
        }

        /// <summary>
        /// Checks if a value is a power of two.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True if a power of 2, else false.</returns>
        public static bool IsPowerOf2(byte value)
        {
            return (value > 0) && ((value & (value - 1)) == 0);
        }

        /// <summary>
        /// Checks if a value is a power of two.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True if a power of 2, else false.</returns>
        public static bool IsPowerOf2(sbyte value)
        {
            return (value > 0) && ((value & (value - 1)) == 0);
        }

        /// <summary>
        /// Finds the next highest power of 2 for a given value unless the value is already a power of 2.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>Next highest power of 2 of the value.</returns>
        public static int NextPowerOf2(int value)
        {
            value--;
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;
            return value + 1;
        }

        /// <summary>
        /// Finds the next highest power of 2 for a given value unless the value is already a power of 2.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>Next highest power of 2 of the value.</returns>
        public static uint NextPowerOf2(uint value)
        {
            value--;
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;
            return value + 1;
        }

        /// <summary>
        /// Finds the parity of a given value.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True for even, False for odd.</returns>
        public static bool Parity(long value)
        {
            long i;
            for (i = 0; value != 0; value >>= 1)
            {
                i += value & 1;
            }
            return i % 2 == 1;
        }

        /// <summary>
        /// Finds the parity of a given value.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True for even, False for odd.</returns>
        public static bool Parity(ulong value)
        {
            ulong i;
            for (i = 0; value != 0; value >>= 1)
            {
                i += value & 1;
            }
            return i % 2 == 1;
        }

        /// <summary>
        /// Finds the parity of a given value.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True for even, False for odd.</returns>
        public static bool Parity(int value)
        {
            int i;
            for (i = 0; value != 0; value >>= 1)
            {
                i += value & 1;
            }
            return i % 2 == 1;
        }

        /// <summary>
        /// Finds the parity of a given value.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True for even, False for odd.</returns>
        public static bool Parity(uint value)
        {
            uint i;
            for (i = 0; value != 0; value >>= 1)
            {
                i += value & 1;
            }
            return i % 2 == 1;
        }

        /// <summary>
        /// Finds the parity of a given value.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True for even, False for odd.</returns>
        public static bool Parity(short value)
        {
            int i;
            for (i = 0; value != 0; value >>= 1)
            {
                i += value & 1;
            }
            return i % 2 == 1;
        }

        /// <summary>
        /// Finds the parity of a given value.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True for even, False for odd.</returns>
        public static bool Parity(ushort value)
        {
            int i;
            for (i = 0; value != 0; value >>= 1)
            {
                i += value & 1;
            }
            return i % 2 == 1;
        }

        /// <summary>
        /// Finds the parity of a given value.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True for even, False for odd.</returns>
        public static bool Parity(sbyte value)
        {
            int i;
            for (i = 0; value != 0; value >>= 1)
            {
                i += value & 1;
            }
            return i % 2 == 1;
        }

        /// <summary>
        /// Finds the parity of a given value.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>True for even, False for odd.</returns>
        public static bool Parity(byte value)
        {
            return ((((ulong)(value * 0x0101010101010101) & 0x8040201008040201) % 0x1FF) & 1) != 0;
        }

        /// <summary>
        /// Finds the number of bits required to store a 0-base numeric value (log base 2).
        /// </summary>
        /// <param name="value">Greatest integer value that needs to be supported.</param>
        /// <returns>Number of bits required for the value.</returns>
        public static int RequiredBits(uint value)
        {
            int ret;
            uint t;
            uint tt;

            // The process is simple. First, we check if any of the top 16 bits are set. If so,
            // we know the most significant set bit is in one of the two most significant bytes. If not,
            // we know the most significant set bit is in one of the two least significant bytes. In both
            // cases, we then check which of the two bytes the most significant set bit is in by checking
            // first the most significant of the two bytes, then the least significant. A lookup table is used
            // to find the bits required for a byte, then we just add the appropriate number of bits to correspond
            // to which byte the most significant bit was found in.

            if ((tt = value >> 16) != 0)
            {
                // Highest set bit resides in the top 16 bits
                if ((t = tt >> 8) != 0)
                {
                    // Most significant byte (bits 24 to 31) contains highest set bit
                    ret = 24 + LogTable256[t];
                }
                else
                {
                    // Second most significant byte (bits 16 to 23) contains highest set bit
                    ret = 16 + LogTable256[tt];
                }
            }
            else
            {
                // All top 16 bits are unset
                if ((t = value >> 8) != 0)
                {
                    // Third most significant byte (bits 8 to 15) contains highest set bit
                    ret = 8 + LogTable256[t];
                }
                else
                {
                    // Least significant byte (bits 0 to 7) contains highest set bit
                    ret = LogTable256[value];
                }
            }

            return ret + 1;
        }

        /// <summary>
        /// Reverses the bit order of a variable (ie: 0100 1000 becomes 0001 0010)
        /// </summary>
        /// <param name="source">Source value to reverse</param>
        /// <returns>Input value with reversed bits</returns>
        public static byte ReverseBits(byte source)
        {
            return (byte)(((source * 0x0802 & 0x22110) | (source * 0x8020 & 0x88440)) * 0x10101 >> 16);
        }

        /// <summary>
        /// Reverses the bit order of a variable (ie: 0100 1000 becomes 0001 0010)
        /// </summary>
        /// <param name="source">Source value to reverse</param>
        /// <returns>Input value with reversed bits</returns>
        public static int ReverseBits(int source)
        {
            return (BitReverseTable256[source & 0xff] << 24) | (BitReverseTable256[(source >> 8) & 0xff] << 16) |
                   (BitReverseTable256[(source >> 16) & 0xff] << 8) | (BitReverseTable256[(source >> 24) & 0xff]);
        }

        /// <summary>
        /// Reverses the bit order of a variable (ie: 0100 1000 becomes 0001 0010)
        /// </summary>
        /// <param name="source">Source value to reverse</param>
        /// <returns>Input value with reversed bits</returns>
        public static uint ReverseBits(uint source)
        {
            return
                (uint)
                ((BitReverseTable256[source & 0xff] << 24) | (BitReverseTable256[(source >> 8) & 0xff] << 16) |
                 (BitReverseTable256[(source >> 16) & 0xff] << 8) | (BitReverseTable256[(source >> 24) & 0xff]));
        }

        /// <summary>
        /// Reverses the bit order of a variable (ie: 0100 1000 becomes 0001 0010)
        /// </summary>
        /// <param name="source">Source value to reverse</param>
        /// <returns>Input value with reversed bits</returns>
        public static ushort ReverseBits(ushort source)
        {
            source = (ushort)(((source >> 1) & 0x5555) | ((source & 0x5555) << 1));
            source = (ushort)(((source >> 2) & 0x3333) | ((source & 0x3333) << 2));
            source = (ushort)(((source >> 4) & 0x0F0F) | ((source & 0x0F0F) << 4));
            return (ushort)((source >> 8) | (source << 8));
        }

        /// <summary>
        /// Reverses the bit order of a variable (ie: 0100 1000 becomes 0001 0010)
        /// </summary>
        /// <param name="source">Source value to reverse</param>
        /// <returns>Input value with reversed bits</returns>
        public static short ReverseBits(short source)
        {
            source = (short)(((source >> 1) & 0x5555) | ((source & 0x5555) << 1));
            source = (short)(((source >> 2) & 0x3333) | ((source & 0x3333) << 2));
            source = (short)(((source >> 4) & 0x0F0F) | ((source & 0x0F0F) << 4));
            return (short)((source >> 8) | (source << 8));
        }
    }
}