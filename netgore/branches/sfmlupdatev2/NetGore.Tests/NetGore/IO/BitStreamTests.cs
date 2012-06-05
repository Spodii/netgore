using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NetGore.IO;
using NUnit.Framework;

#pragma warning disable 618,612
// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace NetGore.Tests.IO
{
    [TestFixture]
    public class BitStreamTests
    {
        /// <summary>
        /// Maximum number of iterations to use for I/O tests.
        /// </summary>
        const long _maxRangeIterations = 100000;

        static void AreBitsEqual(int a, int b, int bits)
        {
            var mask = (1 << bits) - 1;
            var ba = a & mask;
            var bb = b & mask;
            Assert.AreEqual(ba, bb);
        }

        static void AreBitsEqual(uint a, uint b, int bits)
        {
            var mask = ((uint)1 << bits) - 1;
            var ba = a & mask;
            var bb = b & mask;
            Assert.AreEqual(ba, bb);
        }

        static void BatchIOTester<T>(IEnumerable<T> values, Func<BitStream, T> readHandler, Action<BitStream, T> writeHandler)
        {
            var bs = new BitStream(655360);
            var valueQueue = new Queue<T>(values.Count());

            bs.PositionBits = 0;

            foreach (var value in values)
            {
                writeHandler(bs, value);
                valueQueue.Enqueue(value);
            }

            bs.PositionBits = 0;

            while (valueQueue.Count > 0)
            {
                var expected = valueQueue.Dequeue();
                var value = readHandler(bs);
                try
                {
                    Assert.AreEqual(expected, value);
                }
                catch
                {
                    Debug.Fail("Would be a good time to see the values... *hint hint*");
                    throw;
                }
            }
        }

        static int GetBitsAmountForBitTest(int iterator, int numBits)
        {
            return 1 + (iterator % (numBits - 1));
        }

        static long GetFieldValue<T>(string fieldName)
        {
            var fieldInfo = typeof(T).GetField(fieldName);
            var value = fieldInfo.GetValue(null);
            return Convert.ToInt64(value);
        }

        static int GetSignedPartialBitValue(int value, int bits, int maxBits)
        {
            if (bits == 1)
                return (value != 0) ? 1 : 0;

            var signed = value < 0;

            // Pseudo-write
            // We have to abort early here to avoid exceptions
            if (value == int.MinValue)
                return int.MinValue & ((1 << (bits - 1)) - 1);

            value = Math.Abs(value);
            value &= (1 << (bits - 1)) - 1;

            // Pseudo-read
            if (signed)
            {
                if (value == 0)
                    return 1 << (maxBits - 1);

                value = -value;
            }

            return value;
        }

        static uint GetUnsignedPartialBitValue(uint value, int bits)
        {
            var bitMask = (1 << bits) - 1;
            return (uint)((int)value & bitMask);
        }

        static IEnumerable<long> Range(long min, long max, long maxIterations)
        {
            if (max - min <= maxIterations)
            {
                for (var x = min; x < max; x++)
                {
                    yield return x;
                }
            }
            else
            {
                var diff = max - min;
                var stepMin = (int)(diff / maxIterations) - 10;
                var stepMax = (int)(diff / maxIterations) + 10;
                int step;
                var rnd = new Random();

                for (var x = min; x < max; x += (long)step)
                {
                    yield return x;
                    step = rnd.Next(stepMin, stepMax);
                }

                yield return min;
                yield return max;
                yield return 0;
                yield return 1;
            }
        }

        static IEnumerable<long> TRange<T>()
        {
            var minValue = GetFieldValue<T>("MinValue");
            var maxValue = GetFieldValue<T>("MaxValue");
            return Range(minValue, maxValue, _maxRangeIterations);
        }

        #region Unit tests

        [Test]
        public void ForeignCharacterStringIO()
        {
            const string originalStr = "ěščřžýáíé";

            var bs = new BitStream();
            bs.Write(originalStr);

            bs.PositionBits = 0;
            var outStr = bs.ReadString();

            Assert.AreEqual(originalStr, outStr);
        }

        [Test]
        public void BitIO()
        {
            var bits = new int[] { 1, 0, 1, 0, 0, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 1, 1, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0 };
            var bs = new BitStream(bits.Length * 2);

            for (var j = 0; j < 10; j++)
            {
                for (var i = 0; i < bits.Length; i++)
                {
                    bs.WriteBit(bits[i]);
                }
            }

            bs.PositionBits = 0;

            for (var j = 0; j < 10; j++)
            {
                for (var i = 0; i < bits.Length; i++)
                {
                    Assert.AreEqual(bits[i], bs.ReadBit());
                }
            }
        }

        [Test]
        public void BitStreamIO()
        {
            var a = new int[] { -1, 0, 1, int.MinValue, int.MaxValue };
            var b = new float[] { -1, 0, 1, float.MinValue, float.MaxValue };
            var c = new long[] { -1, 0, 1, long.MinValue, long.MaxValue };
            var d = new bool[] { true, false, false, true, true, false };
            var e = new byte[] { 0, 1, 2, 3, 100, 200, 225, 254, 255 };

            var src = new BitStream(1024);
            src.Write(a, 0, a.Length);
            src.Write(b, 0, b.Length);
            src.Write(c, 0, c.Length);
            src.Write(d, 0, d.Length);
            src.Write(e, 0, e.Length);

            var dest = new BitStream(1024);
            dest.Write(src);

            dest.PositionBits = 0;

            for (var i = 0; i < a.Length; i++)
            {
                Assert.AreEqual(dest.ReadInt(), a[i]);
            }

            for (var i = 0; i < b.Length; i++)
            {
                Assert.AreEqual(dest.ReadFloat(), b[i]);
            }

            for (var i = 0; i < c.Length; i++)
            {
                Assert.AreEqual(dest.ReadLong(), c[i]);
            }

            for (var i = 0; i < d.Length; i++)
            {
                Assert.AreEqual(dest.ReadBool(), d[i]);
            }

            for (var i = 0; i < e.Length; i++)
            {
                Assert.AreEqual(dest.ReadByte(), e[i]);
            }
        }

        [Test]
        public void ByteBitIO()
        {
            const int numBits = 8;
            const int iterations = 5000;

            var rnd = new Random();
            var values = new List<byte> { byte.MinValue, 1, byte.MaxValue };
            for (var i = 0; i < iterations; i++)
            {
                values.Add((byte)rnd.Next(byte.MinValue, byte.MaxValue));
            }

            var bs = new BitStream(values.Count * (numBits / 8) * 2);

            for (var i = 0; i < values.Count; i++)
            {
                var bits = GetBitsAmountForBitTest(i, numBits);
                bs.Write(values[i], bits);
            }

            bs.PositionBits = 0;

            for (var i = 0; i < values.Count; i++)
            {
                var bits = GetBitsAmountForBitTest(i, numBits);
                var original = GetUnsignedPartialBitValue(values[i], bits);
                AreBitsEqual(original, bs.ReadByte(bits), bits);
            }
        }

        [Test]
        public void ByteIO()
        {
            BatchIOTester(from value in TRange<byte>()
                select (byte)value, x => x.ReadByte(), (x, v) => x.Write(v));
        }

        [Test]
        public void ByteMinMaxTest()
        {
            const byte tMin = byte.MinValue;
            const byte tMax = byte.MaxValue;
            const int bits = sizeof(byte) * 8;

            var bs = new BitStream((bits / 8) * bits * 4);

            for (var i = 1; i <= bits; i++)
            {
                bs.Write(tMin, i);
                bs.Write(tMax, i);
            }

            bs.PositionBits = 0;

            for (var i = 1; i <= bits; i++)
            {
                int min = bs.ReadByte(i);
                int max = bs.ReadByte(i);

                if (i == bits)
                {
                    Assert.AreEqual(min, tMin);
                    Assert.AreEqual(max, tMax);
                }
            }
        }

        [Test]
        public void DoubleIO()
        {
            const int numBits = 64;
            const int iterations = 5000;

            var rnd = new Random();

            // Structuring the loop like this allows us to run tons of numbers, but without having
            // to wait so goddamn long for any results
            for (var loop = 0; loop < 10; loop++)
            {
                var values = new List<double> { double.MinValue, 1, double.MaxValue };
                for (var i = 0; i < iterations; i++)
                {
                    values.Add(rnd.NextDouble());
                }

                var bs = new BitStream(iterations * numBits * 2);

                for (var i = 0; i < values.Count; i++)
                {
                    bs.Write(values[i]);
                }

                bs.PositionBits = 0;

                for (var i = 0; i < values.Count; i++)
                {
                    Assert.AreEqual(values[i], bs.ReadDouble());
                }
            }
        }

        [Test]
        public void FloatIO()
        {
            const int numBits = 32;
            const int iterations = 5000;

            var rnd = new Random();

            // Structuring the loop like this allows us to run tons of numbers, but without having
            // to wait so goddamn long for any results
            for (var loop = 0; loop < 10; loop++)
            {
                var values = new List<float> { float.MinValue, 1, float.MaxValue };
                for (var i = 0; i < iterations; i++)
                {
                    values.Add((float)rnd.NextDouble());
                }

                var bs = new BitStream(iterations * numBits * 2);

                for (var i = 0; i < values.Count; i++)
                {
                    bs.Write(values[i]);
                }

                bs.PositionBits = 0;

                for (var i = 0; i < values.Count; i++)
                {
                    Assert.AreEqual(values[i], bs.ReadFloat());
                }
            }
        }

        [Test]
        public void GetBufferTest()
        {
            var bs = new BitStream(16);
            bs.Write((byte)20);
            bs.Write((byte)8);
            bs.Write(true);

            bs.PositionBits = 0;
            var buff = bs.GetBuffer();

            Assert.AreEqual(20, buff[0]);
            Assert.AreEqual(8, buff[1]);
            Assert.AreEqual(128, buff[2] & (1 << 7));
        }

        [Test]
        public void IntBitIO()
        {
            const int numBits = 32;
            const int iterations = 10000;

            var rnd = new Random();

            for (var loop = 0; loop < 10; loop++)
            {
                var bs = new BitStream(iterations * numBits * 2);

                var values = new List<int> { int.MaxValue, 1, int.MinValue, 0, -1635136632 };
                for (var i = 0; i < iterations; i++)
                {
                    values.Add(rnd.Next(int.MinValue, int.MaxValue));
                }

                for (var i = 0; i < values.Count; i++)
                {
                    var bits = GetBitsAmountForBitTest(i, numBits);
                    Assert.LessOrEqual(bits, numBits);
                    Assert.GreaterOrEqual(bits, 1);
                    bs.Write(values[i], bits);
                }

                bs.PositionBits = 0;

                for (var i = 0; i < values.Count; i++)
                {
                    var bits = GetBitsAmountForBitTest(i, numBits);
                    var original = GetSignedPartialBitValue(values[i], bits, numBits);
                    AreBitsEqual(original, bs.ReadInt(bits), numBits);
                }
            }
        }

        [Test]
        public void IntIO()
        {
            BatchIOTester(from value in TRange<int>()
                select (int)value, x => x.ReadInt(), (x, v) => x.Write(v));
        }

        [Test]
        public void IntMinMaxTest()
        {
            const int tMin = int.MinValue;
            const int tMax = int.MaxValue;
            const int bits = sizeof(int) * 8;

            var bs = new BitStream((bits / 8) * bits * 4);

            for (var i = 1; i <= bits; i++)
            {
                bs.Write(tMin, i);
                bs.Write(tMax, i);
            }

            bs.PositionBits = 0;

            for (var i = 1; i <= bits; i++)
            {
                var min = bs.ReadInt(i);
                var max = bs.ReadInt(i);

                if (i == bits)
                {
                    Assert.AreEqual(min, tMin);
                    Assert.AreEqual(max, tMax);
                }
            }
        }

        [Test]
        public void LengthBitsTest()
        {
            var bs = new BitStream(1024);

            var expectedBits = 0;
            for (var i = 0; i < 100; i++)
            {
                Assert.AreEqual(expectedBits, bs.LengthBits);
                var bits = 1 + (i % 31);
                bs.Write(i, bits);
                expectedBits += bits;
            }
        }

        [Test]
        public void LengthTest()
        {
            var bs = new BitStream(1024);

            var expectedBits = 0;
            for (var i = 0; i < 100; i++)
            {
                Assert.AreEqual(Math.Ceiling(expectedBits / 8f), bs.Length);
                var bits = 1 + (i % 31);
                bs.Write(i, bits);
                expectedBits += bits;
            }
        }

        [Test]
        public void LongIO()
        {
            const int numBits = 64;
            const int iterations = 5000;

            var rnd = new Random();

            // Structuring the loop like this allows us to run tons of numbers, but without having
            // to wait so goddamn long for any results
            for (var loop = 0; loop < 10; loop++)
            {
                var values = new List<long> { long.MinValue, 1, long.MaxValue };
                for (long i = 0; i < iterations; i++)
                {
                    values.Add((long)rnd.NextDouble());
                }

                var bs = new BitStream(iterations * numBits * 2);

                for (var i = 0; i < values.Count; i++)
                {
                    bs.Write(values[i]);
                }

                bs.PositionBits = 0;

                for (var i = 0; i < values.Count; i++)
                {
                    Assert.AreEqual(values[i], bs.ReadLong());
                }
            }
        }

        [Test]
        public void LotsOf1sTest()
        {
            var bs = new BitStream();

            for (var i = 2; i < 32; i++)
            {
                bs.Write(uint.MaxValue, i);
            }

            bs.PositionBits = 0;

            for (var i = 2; i < 32; i++)
            {
                Assert.AreEqual((1 << i) - 1, bs.ReadUInt(i));
            }
        }

        [Test]
        public void NegativeOneIOTest()
        {
            var bs = new BitStream(2048);
            for (var i = 2; i <= 32; i++)
            {
                bs.Write(-1, i);
            }

            for (var i = 2; i <= 16; i++)
            {
                bs.Write((short)-1, i);
            }

            for (var i = 2; i <= 8; i++)
            {
                bs.Write((sbyte)-1, i);
            }

            bs.PositionBits = 0;

            for (var i = 2; i <= 32; i++)
            {
                AreBitsEqual(-1, bs.ReadInt(i), i);
            }

            for (var i = 2; i <= 16; i++)
            {
                AreBitsEqual(-1, bs.ReadShort(i), i);
            }

            for (var i = 2; i <= 8; i++)
            {
                AreBitsEqual(-1, bs.ReadSByte(i), i);
            }
        }

        [Test]
        public void NullableBoolIO()
        {
            var bs = new BitStream(256);
            bool? value = true;
            bool? nvalue = null;

            bs.Write(value);
            bs.Write(nvalue);
            bs.Write(value);

            bs.PositionBits = 0;

            Assert.AreEqual(value, bs.ReadNullableBool());
            Assert.AreEqual(nvalue, bs.ReadNullableBool());
            Assert.AreEqual(value, bs.ReadNullableBool());
        }

        [Test]
        public void NullableByteIO()
        {
            var bs = new BitStream(256);
            byte? value = 10;
            byte? nvalue = null;

            bs.Write(value);
            bs.Write(nvalue);
            bs.Write(value);

            bs.PositionBits = 0;

            Assert.AreEqual(value, bs.ReadNullableByte());
            Assert.AreEqual(nvalue, bs.ReadNullableByte());
            Assert.AreEqual(value, bs.ReadNullableByte());
        }

        [Test]
        public void NullableDoubleIO()
        {
            var bs = new BitStream(256);
            double? value = 10;
            double? nvalue = null;

            bs.Write(value);
            bs.Write(nvalue);
            bs.Write(value);

            bs.PositionBits = 0;

            Assert.AreEqual(value, bs.ReadNullableDouble());
            Assert.AreEqual(nvalue, bs.ReadNullableDouble());
            Assert.AreEqual(value, bs.ReadNullableDouble());
        }

        [Test]
        public void NullableFloatIO()
        {
            var bs = new BitStream(256);
            float? value = 10;
            float? nvalue = null;

            bs.Write(value);
            bs.Write(nvalue);
            bs.Write(value);

            bs.PositionBits = 0;

            Assert.AreEqual(value, bs.ReadNullableFloat());
            Assert.AreEqual(nvalue, bs.ReadNullableFloat());
            Assert.AreEqual(value, bs.ReadNullableFloat());
        }

        [Test]
        public void NullableIntIO()
        {
            var bs = new BitStream(256);
            int? value = 10;
            int? nvalue = null;

            bs.Write(value);
            bs.Write(nvalue);
            bs.Write(value);

            bs.PositionBits = 0;

            Assert.AreEqual(value, bs.ReadNullableInt());
            Assert.AreEqual(nvalue, bs.ReadNullableInt());
            Assert.AreEqual(value, bs.ReadNullableInt());
        }

        [Test]
        public void NullableLongIO()
        {
            var bs = new BitStream(256);
            long? value = 10;
            long? nvalue = null;

            bs.Write(value);
            bs.Write(nvalue);
            bs.Write(value);

            bs.PositionBits = 0;

            Assert.AreEqual(value, bs.ReadNullableLong());
            Assert.AreEqual(nvalue, bs.ReadNullableLong());
            Assert.AreEqual(value, bs.ReadNullableLong());
        }

        [Test]
        public void NullableSByteIO()
        {
            var bs = new BitStream(256);
            sbyte? value = 10;
            sbyte? nvalue = null;

            bs.Write(value);
            bs.Write(nvalue);
            bs.Write(value);

            bs.PositionBits = 0;

            Assert.AreEqual(value, bs.ReadNullableSByte());
            Assert.AreEqual(nvalue, bs.ReadNullableSByte());
            Assert.AreEqual(value, bs.ReadNullableSByte());
        }

        [Test]
        public void NullableShortIO()
        {
            var bs = new BitStream(256);
            short? value = 10;
            short? nvalue = null;

            bs.Write(value);
            bs.Write(nvalue);
            bs.Write(value);

            bs.PositionBits = 0;

            Assert.AreEqual(value, bs.ReadNullableShort());
            Assert.AreEqual(nvalue, bs.ReadNullableShort());
            Assert.AreEqual(value, bs.ReadNullableShort());
        }

        [Test]
        public void NullableUIntIO()
        {
            var bs = new BitStream(256);
            uint? value = 10;
            uint? nvalue = null;

            bs.Write(value);
            bs.Write(nvalue);
            bs.Write(value);

            bs.PositionBits = 0;

            Assert.AreEqual(value, bs.ReadNullableUInt());
            Assert.AreEqual(nvalue, bs.ReadNullableUInt());
            Assert.AreEqual(value, bs.ReadNullableUInt());
        }

        [Test]
        public void NullableULongIO()
        {
            var bs = new BitStream(256);
            ulong? value = 10;
            ulong? nvalue = null;

            bs.Write(value);
            bs.Write(nvalue);
            bs.Write(value);

            bs.PositionBits = 0;

            Assert.AreEqual(value, bs.ReadNullableULong());
            Assert.AreEqual(nvalue, bs.ReadNullableULong());
            Assert.AreEqual(value, bs.ReadNullableULong());
        }

        [Test]
        public void NullableUShortIO()
        {
            var bs = new BitStream(256);
            ushort? value = 10;
            ushort? nvalue = null;

            bs.Write(value);
            bs.Write(nvalue);
            bs.Write(value);

            bs.PositionBits = 0;

            Assert.AreEqual(value, bs.ReadNullableUShort());
            Assert.AreEqual(nvalue, bs.ReadNullableUShort());
            Assert.AreEqual(value, bs.ReadNullableUShort());
        }

        [Test]
        public void PerfectLengthStringIO()
        {
            var bs = new BitStream(65536);

            const int count = 1000;

            var strings = new string[count];
            var rnd = new Random();
            for (var i = 0; i < count; i++)
            {
                var chars = new char[rnd.Next(5, 20)];
                for (var j = 0; j < chars.Length; j++)
                {
                    chars[j] = (char)rnd.Next(0, 128);
                }
                strings[i] = new string(chars);
            }

            foreach (var s in strings)
            {
                bs.Write(s, s.Length);
            }

            bs.PositionBits = 0;

            foreach (var s in strings)
            {
                Assert.AreEqual(s, bs.ReadString(s.Length));
            }
        }

        [Test]
        public void ReadLengthChangeModeTest()
        {
            var bs = new BitStream(128);
            Assert.AreEqual(0, bs.LengthBits);

            bs.Write(62);
            bs.Write(true);

            Assert.AreEqual(32 + 1, bs.LengthBits);

            bs.PositionBits = 0;

            Assert.AreEqual(32 + 1, bs.LengthBits);

            bs.PositionBits = 0;

            Assert.AreEqual(32 + 1, bs.LengthBits);
        }

        [Test]
        public void ReadLengthFromBufferTest()
        {
            var data = new byte[5];
            var bs = new BitStream(data);

            Assert.AreEqual(8 * data.Length, bs.LengthBits);

            bs.PositionBits = 0;

            Assert.AreEqual(8 * data.Length, bs.LengthBits);
        }

        [Test]
        public void SByteBitIO()
        {
            const int numBits = 8;
            const int iterations = 5000;

            var rnd = new Random();
            var values = new List<sbyte> { sbyte.MinValue, 1, sbyte.MaxValue };
            for (var i = 0; i < iterations; i++)
            {
                values.Add((sbyte)rnd.Next(sbyte.MinValue, sbyte.MaxValue));
            }

            var bs = new BitStream(values.Count * (numBits / 8) * 2);

            for (var i = 2; i < values.Count; i++)
            {
                var bits = GetBitsAmountForBitTest(i, numBits);
                bs.Write(values[i], bits);
            }

            bs.PositionBits = 0;

            for (var i = 2; i < values.Count; i++)
            {
                var bits = GetBitsAmountForBitTest(i, numBits);
                var original = GetSignedPartialBitValue(values[i], bits, numBits);
                AreBitsEqual(original, bs.ReadSByte(bits), bits);
            }
        }

        [Test]
        public void SByteIO()
        {
            BatchIOTester(from value in TRange<sbyte>()
                select (sbyte)value, x => x.ReadSByte(), (x, v) => x.Write(v));
        }

        [Test]
        public void SByteMinMaxTest()
        {
            const sbyte tMin = sbyte.MinValue;
            const sbyte tMax = sbyte.MaxValue;
            const int bits = sizeof(sbyte) * 8;

            var bs = new BitStream((bits / 8) * bits * 4);

            for (var i = 1; i <= bits; i++)
            {
                bs.Write(tMin, i);
                bs.Write(tMax, i);
            }

            bs.PositionBits = 0;

            for (var i = 1; i <= bits; i++)
            {
                int min = bs.ReadSByte(i);
                int max = bs.ReadSByte(i);

                if (i == bits)
                {
                    Assert.AreEqual(min, tMin);
                    Assert.AreEqual(max, tMax);
                }
            }
        }

        [Test]
        public void SeekFromCurrentTest()
        {
            const int numBits = 32;
            const int iterations = 50000;

            var rnd = new Random();
            var values = new List<int> { int.MinValue, 1, int.MaxValue };
            for (var i = 0; i < iterations; i++)
            {
                values.Add(rnd.Next(int.MinValue, int.MaxValue));
            }

            var bs = new BitStream(iterations * numBits * 2);

            for (var i = 0; i < values.Count; i++)
            {
                var bits = GetBitsAmountForBitTest(i, numBits);
                bs.Write(values[i], bits);
            }

            bs.PositionBits = 0;

            for (var i = 0; i < values.Count; i++)
            {
                var bits = GetBitsAmountForBitTest(i, numBits);

                if (i % 2 == 0)
                {
                    bs.SeekBits(bits, SeekOrigin.Current);
                    continue;
                }

                var original = GetSignedPartialBitValue(values[i], bits, numBits);
                AreBitsEqual(original, bs.ReadInt(bits), bits);
            }
        }

        [Test]
        public void SetLengthWhileReadingTest()
        {
            var data = new byte[5];
            var bs = new BitStream(data);

            Assert.AreEqual(8 * data.Length, bs.LengthBits);

            bs.LengthBits = 32;

            Assert.AreEqual(32, bs.LengthBits);
        }

        [Test]
        public void SetLengthWhileWritingTest()
        {
            var bs = new BitStream(128);
            Assert.AreEqual(0, bs.LengthBits);

            // Populate
            bs.Write(62);
            bs.Write((byte)170);

            Assert.AreEqual(32 + 8, bs.LengthBits);

            // Shift back 8 bits to chop off the last byte written
            bs.LengthBits -= 8;

            // Write again, expecting to overwrite the last byte written
            bs.Write((byte)85);

            // Check our values
            bs.PositionBits = 0;

            Assert.AreEqual(32 + 8, bs.LengthBits);

            Assert.AreEqual(62, bs.ReadInt());
            Assert.AreEqual(85, bs.ReadByte());
        }

        [Test]
        public void ShortBitIO()
        {
            const int numBits = 16;
            const int iterations = 20000;

            var rnd = new Random();
            var values = new List<short> { short.MinValue, 1, short.MaxValue };
            for (short i = 0; i < iterations; i++)
            {
                values.Add((short)rnd.Next(short.MinValue, short.MaxValue));
            }

            var bs = new BitStream(iterations * numBits * 2);

            for (var i = 0; i < values.Count; i++)
            {
                var bits = GetBitsAmountForBitTest(i, numBits);
                Assert.LessOrEqual(bits, numBits);
                Assert.GreaterOrEqual(bits, 1);
                bs.Write(values[i], bits);
            }

            bs.PositionBits = 0;

            for (var i = 0; i < values.Count; i++)
            {
                var bits = GetBitsAmountForBitTest(i, numBits);
                var original = GetSignedPartialBitValue(values[i], bits, numBits);
                AreBitsEqual((short)original, bs.ReadShort(bits), bits);
            }
        }

        [Test]
        public void ShortIO()
        {
            BatchIOTester(from value in TRange<short>()
                select (short)value, x => x.ReadShort(), (x, v) => x.Write(v));
        }

        [Test]
        public void ShortMinMaxTest()
        {
            const short tMin = short.MinValue;
            const short tMax = short.MaxValue;
            const int bits = sizeof(short) * 8;

            var bs = new BitStream((bits / 8) * bits * 4);

            for (var i = 1; i <= bits; i++)
            {
                bs.Write(tMin, i);
                bs.Write(tMax, i);
            }

            bs.PositionBits = 0;

            for (var i = 1; i <= bits; i++)
            {
                int min = bs.ReadShort(i);
                int max = bs.ReadShort(i);

                if (i == bits)
                {
                    Assert.AreEqual(min, tMin);
                    Assert.AreEqual(max, tMax);
                }
            }
        }

        [Test]
        public void SimpleByteBitIO()
        {
            var bs = new BitStream(1024);
            bs.Write((byte)0, 1);
            bs.Write((byte)1, 2);
            bs.Write((byte)2, 3);
            bs.Write((byte)3, 4);
            bs.Write((byte)10, 5);

            bs.PositionBits = 0;

            Assert.AreEqual(bs.ReadByte(1), 0);
            Assert.AreEqual(bs.ReadByte(2), 1);
            Assert.AreEqual(bs.ReadByte(3), 2);
            Assert.AreEqual(bs.ReadByte(4), 3);
            Assert.AreEqual(bs.ReadByte(5), 10);
        }

        [Test]
        public void SimpleIntIO()
        {
            var bs = new BitStream(65536);

            bs.Write(-25);
            bs.Write(-13);
            bs.Write(13);
            bs.Write(25);
            bs.Write(-8);
            bs.Write(8);

            bs.PositionBits = 0;

            Assert.AreEqual(-25, bs.ReadInt());
            Assert.AreEqual(-13, bs.ReadInt());
            Assert.AreEqual(13, bs.ReadInt());
            Assert.AreEqual(25, bs.ReadInt());
            Assert.AreEqual(-8, bs.ReadInt());
            Assert.AreEqual(8, bs.ReadInt());
        }

        [Test]
        public void SimpleLengthBitsTest()
        {
            var bs = new BitStream(1024);

            var expectedBits = 0;
            for (var i = 0; i < 100; i++)
            {
                Assert.AreEqual(expectedBits, bs.LengthBits);
                bs.Write((byte)1);
                expectedBits += 8;
            }
        }

        [Test]
        public void SimpleShortIO()
        {
            var bs = new BitStream(65536);

            bs.Write((short)-25, 15);
            bs.Write((short)-13);
            bs.Write((short)13);
            bs.Write((short)25, 15);
            bs.Write((short)-8, 12);
            bs.Write((short)8, 12);

            bs.PositionBits = 0;

            Assert.AreEqual(-25, bs.ReadShort(15));
            Assert.AreEqual(-13, bs.ReadShort());
            Assert.AreEqual(13, bs.ReadShort());
            Assert.AreEqual(25, bs.ReadShort(15));
            Assert.AreEqual(-8, bs.ReadShort(12));
            Assert.AreEqual(8, bs.ReadShort(12));
        }

        [Test]
        public void SimpleStringIO()
        {
            var bs = new BitStream(65536);

            var strings = new string[]
            { "Hello", "", "abcdefghijklmnopqrstuvwxyz", "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "1234567890!@#$%^&*()" };

            foreach (var s in strings)
            {
                bs.Write(s);
            }

            bs.PositionBits = 0;

            foreach (var s in strings)
            {
                Assert.AreEqual(s, bs.ReadString());
            }
        }

        [Test]
        public void SimpleUIntIO()
        {
            var bs = new BitStream(65536);
            bs.Write(uint.MinValue);
            bs.Write(uint.MaxValue);
            bs.Write((uint)25);
            bs.Write((uint)13);
            bs.Write((uint)8);

            bs.PositionBits = 0;

            Assert.AreEqual(uint.MinValue, bs.ReadUInt());
            Assert.AreEqual(uint.MaxValue, bs.ReadUInt());
            Assert.AreEqual(25, bs.ReadUInt());
            Assert.AreEqual(13, bs.ReadUInt());
            Assert.AreEqual(8, bs.ReadUInt());
        }

        [Test]
        public void StringIO()
        {
            var bs = new BitStream(65536);

            const int count = 1000;

            var strings = new string[count];
            var rnd = new Random();
            for (var i = 0; i < count; i++)
            {
                var chars = new char[rnd.Next(5, 20)];
                for (var j = 0; j < chars.Length; j++)
                {
                    chars[j] = (char)rnd.Next(0, 128);
                }
                strings[i] = new string(chars);
            }

            foreach (var s in strings)
            {
                bs.Write(s);
            }

            bs.PositionBits = 0;

            foreach (var s in strings)
            {
                Assert.AreEqual(s, bs.ReadString());
            }
        }

        [Test]
        public void SuppliedBufferTest()
        {
            var buff = new byte[16];
            var bs = new BitStream(buff);
            bs.Write((byte)20);
            bs.Write((byte)8);
            bs.Write(true);

            bs.PositionBits = 0;

            Assert.AreEqual(20, buff[0]);
            Assert.AreEqual(8, buff[1]);
            Assert.AreEqual(128, buff[2] & (1 << 7));
        }

        [Test]
        public void UIntBitIO()
        {
            const int numBits = 32;
            const int iterations = 50000;

            var rnd = new Random();
            var values = new List<uint> { uint.MinValue, 1, uint.MaxValue };
            for (var i = 0; i < iterations; i++)
            {
                values.Add((uint)rnd.NextDouble());
            }

            var bs = new BitStream(iterations * numBits * 2);

            for (var i = 0; i < values.Count; i++)
            {
                var bits = GetBitsAmountForBitTest(i, numBits);
                bs.Write(values[i], bits);
            }

            bs.PositionBits = 0;

            for (var i = 0; i < values.Count; i++)
            {
                var bits = GetBitsAmountForBitTest(i, numBits);
                var original = GetUnsignedPartialBitValue(values[i], bits);
                AreBitsEqual(original, bs.ReadUInt(bits), bits);
            }
        }

        [Test]
        public void UIntIO()
        {
            BatchIOTester(from value in TRange<uint>()
                select (uint)value, x => x.ReadUInt(), (x, v) => x.Write(v));
        }

        [Test]
        public void ULongIO()
        {
            const int numBits = 64;
            const int iterations = 5000;

            var rnd = new Random();

            // Structuring the loop like this allows us to run tons of numbers, but without having
            // to wait so goddamn long for any results
            for (var loop = 0; loop < 10; loop++)
            {
                var values = new List<ulong> { ulong.MinValue, 1, ulong.MaxValue };
                for (var i = 0; i < iterations; i++)
                {
                    values.Add((ulong)rnd.NextDouble());
                }

                var bs = new BitStream(iterations * numBits * 2);

                for (var i = 0; i < values.Count; i++)
                {
                    bs.Write(values[i]);
                }

                bs.PositionBits = 0;

                for (var i = 0; i < values.Count; i++)
                {
                    Assert.AreEqual(values[i], bs.ReadULong());
                }
            }
        }

        [Test]
        public void UShortBitIO()
        {
            const int numBits = 16;
            const int iterations = 20000;

            var rnd = new Random();
            var values = new List<ushort> { ushort.MinValue, 1, ushort.MaxValue };
            for (ushort i = 0; i < iterations; i++)
            {
                values.Add((ushort)rnd.Next(ushort.MinValue, ushort.MaxValue));
            }

            var bs = new BitStream(iterations * numBits * 2);

            for (var i = 0; i < values.Count; i++)
            {
                var bits = GetBitsAmountForBitTest(i, numBits);
                bs.Write(values[i], bits);
            }

            bs.PositionBits = 0;

            for (var i = 0; i < values.Count; i++)
            {
                var bits = GetBitsAmountForBitTest(i, numBits);
                var original = GetUnsignedPartialBitValue(values[i], bits);
                AreBitsEqual(original, bs.ReadUShort(bits), bits);
            }
        }

        [Test]
        public void UShortIO()
        {
            BatchIOTester(from value in TRange<ushort>()
                select (ushort)value, x => x.ReadUShort(), (x, v) => x.Write(v));
        }

        [Test]
        public void VariableLengthStringIO()
        {
            const int count = 10;

            for (var length = 0; length < 300; length += 7)
            {
                var bs = new BitStream(65536);

                // Create the strings
                var strings = new string[count];
                var rnd = new Random();
                for (var i = 0; i < count; i++)
                {
                    var chars = new char[rnd.Next(200, 500)];
                    for (var j = 0; j < chars.Length; j++)
                    {
                        chars[j] = (char)rnd.Next(32, 128);
                    }
                    strings[i] = new string(chars);
                }

                // Write the strings
                foreach (var s in strings)
                {
                    var expected = s;
                    if (expected.Length > length)
                        expected = expected.Substring(0, length);
                    bs.Write(expected, length);
                }

                bs.PositionBits = 0;

                // Read the strings and compare to the expected string
                foreach (var s in strings)
                {
                    var expected = s;
                    if (expected.Length > length)
                        expected = expected.Substring(0, length);
                    Assert.AreEqual(expected, bs.ReadString(length));
                }
            }
        }

        #endregion
    }
}