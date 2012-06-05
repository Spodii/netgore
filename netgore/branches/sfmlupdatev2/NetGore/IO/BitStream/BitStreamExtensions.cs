using System.Linq;

namespace NetGore.IO
{
    public static class BitStreamExtensions
    {
        /// <summary>
        /// Reads an array of values from a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to read from.</param>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of read values.</returns>
        public static double[] ReadDoubles(this BitStream bs, int count)
        {
            var ret = new double[count];
            for (var i = 0; i < count; i++)
            {
                ret[i] = bs.ReadDouble();
            }
            return ret;
        }

        /// <summary>
        /// Reads an array of values from a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to read from.</param>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of read values.</returns>
        public static float[] ReadFloats(this BitStream bs, int count)
        {
            var ret = new float[count];
            for (var i = 0; i < count; i++)
            {
                ret[i] = bs.ReadFloat();
            }
            return ret;
        }

        /// <summary>
        /// Reads an array of values from a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to read from.</param>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of read values.</returns>
        public static int[] ReadInts(this BitStream bs, int count)
        {
            var ret = new int[count];
            for (var i = 0; i < count; i++)
            {
                ret[i] = bs.ReadInt();
            }
            return ret;
        }

        /// <summary>
        /// Reads an array of values from a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to read from.</param>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of read values.</returns>
        public static long[] ReadLongs(this BitStream bs, int count)
        {
            var ret = new long[count];
            for (var i = 0; i < count; i++)
            {
                ret[i] = bs.ReadLong();
            }
            return ret;
        }

        /// <summary>
        /// Reads an array of values from a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to read from.</param>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of read values.</returns>
        public static sbyte[] ReadSBytes(this BitStream bs, int count)
        {
            var ret = new sbyte[count];
            for (var i = 0; i < count; i++)
            {
                ret[i] = bs.ReadSByte();
            }
            return ret;
        }

        /// <summary>
        /// Reads an array of values from a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to read from.</param>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of read values.</returns>
        public static short[] ReadShorts(this BitStream bs, int count)
        {
            var ret = new short[count];
            for (var i = 0; i < count; i++)
            {
                ret[i] = bs.ReadShort();
            }
            return ret;
        }

        /// <summary>
        /// Reads an array of values from a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to read from.</param>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of read values.</returns>
        public static string[] ReadStrings(this BitStream bs, int count)
        {
            var ret = new string[count];
            for (var i = 0; i < count; i++)
            {
                ret[i] = bs.ReadString();
            }
            return ret;
        }

        /// <summary>
        /// Reads an array of values from a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to read from.</param>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of read values.</returns>
        public static uint[] ReadUInts(this BitStream bs, int count)
        {
            var ret = new uint[count];
            for (var i = 0; i < count; i++)
            {
                ret[i] = bs.ReadUInt();
            }
            return ret;
        }

        /// <summary>
        /// Reads an array of values from a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to read from.</param>
        /// <param name="count">The number of values to read.</param>
        /// <returns>The array of read values.</returns>
        public static ushort[] ReadUShorts(this BitStream bs, int count)
        {
            var ret = new ushort[count];
            for (var i = 0; i < count; i++)
            {
                ret[i] = bs.ReadUShort();
            }
            return ret;
        }

        /// <summary>
        /// Writes an array of values to a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to write to.</param>
        /// <param name="values">The array of values to write.</param>
        public static void Write(this BitStream bs, byte[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                bs.Write(values[i]);
            }
        }

        /// <summary>
        /// Writes an array of values to a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to write to.</param>
        /// <param name="values">The array of values to write.</param>
        public static void Write(this BitStream bs, sbyte[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                bs.Write(values[i]);
            }
        }

        /// <summary>
        /// Writes an array of values to a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to write to.</param>
        /// <param name="values">The array of values to write.</param>
        public static void Write(this BitStream bs, short[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                bs.Write(values[i]);
            }
        }

        /// <summary>
        /// Writes an array of values to a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to write to.</param>
        /// <param name="values">The array of values to write.</param>
        public static void Write(this BitStream bs, ushort[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                bs.Write(values[i]);
            }
        }

        /// <summary>
        /// Writes an array of values to a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to write to.</param>
        /// <param name="values">The array of values to write.</param>
        public static void Write(this BitStream bs, int[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                bs.Write(values[i]);
            }
        }

        /// <summary>
        /// Writes an array of values to a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to write to.</param>
        /// <param name="values">The array of values to write.</param>
        public static void Write(this BitStream bs, uint[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                bs.Write(values[i]);
            }
        }

        /// <summary>
        /// Writes an array of values to a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to write to.</param>
        /// <param name="values">The array of values to write.</param>
        public static void Write(this BitStream bs, string[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                bs.Write(values[i]);
            }
        }

        /// <summary>
        /// Writes an array of values to a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to write to.</param>
        /// <param name="values">The array of values to write.</param>
        public static void Write(this BitStream bs, float[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                bs.Write(values[i]);
            }
        }

        /// <summary>
        /// Writes an array of values to a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to write to.</param>
        /// <param name="values">The array of values to write.</param>
        public static void Write(this BitStream bs, long[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                bs.Write(values[i]);
            }
        }

        /// <summary>
        /// Writes an array of values to a <see cref="BitStream"/>.
        /// </summary>
        /// <param name="bs">The <see cref="BitStream"/> to write to.</param>
        /// <param name="values">The array of values to write.</param>
        public static void Write(this BitStream bs, double[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                bs.Write(values[i]);
            }
        }
    }
}