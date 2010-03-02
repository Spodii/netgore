using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NetGore.IO;
using NUnit.Framework;

#pragma warning disable 618,612
// ReSharper disable ConditionIsAlwaysTrueOrFalse

namespace NetGore.Tests.IO
{
    [TestFixture]
    public class BitStreamTests
    {
        delegate T BitStreamReadHandler<T>();

        delegate void BitStreamWriteHandler<T>(T value);

        /// <summary>
        /// Maximum number of iterations to use for I/O tests.
        /// </summary>
        const long _maxRangeIterations = 100000;

        static readonly IEnumerable<MethodInfo> _bitStreamReaderHandlerMethods =
            typeof(BitStream).GetMethods().Where(
                x =>
                x.Name.StartsWith("Read") && x.Name != "Read" && !x.Name.StartsWith("ReadBit") && x.GetParameters().Count() == 0);

        static void AreBitsEqual(int a, int b, int bits)
        {
            int mask = (1 << bits) - 1;
            int ba = a & mask;
            int bb = b & mask;
            Assert.AreEqual(ba, bb);
        }

        static void AreBitsEqual(uint a, uint b, int bits)
        {
            uint mask = ((uint)1 << bits) - 1;
            uint ba = a & mask;
            uint bb = b & mask;
            Assert.AreEqual(ba, bb);
        }

        static void BatchIOTester<T>(IEnumerable<T> values)
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 655360);
            var writeHandler = GetWriteHandler<T>(bs);
            var readHandler = GetReadHandler<T>(bs);

            var valueQueue = new Queue<T>(values.Count());

            bs.Mode = BitStreamMode.Write;

            foreach (T value in values)
            {
                writeHandler(value);
                valueQueue.Enqueue(value);
            }

            bs.Mode = BitStreamMode.Read;

            while (valueQueue.Count > 0)
            {
                T expected = valueQueue.Dequeue();
                T value = readHandler();
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
            FieldInfo fieldInfo = typeof(T).GetField(fieldName);
            object value = fieldInfo.GetValue(null);
            return Convert.ToInt64(value);
        }

        static BitStreamReadHandler<T> GetReadHandler<T>(BitStream bitStream)
        {
            MethodInfo methodInfo = null;
            foreach (MethodInfo mi in _bitStreamReaderHandlerMethods)
            {
                if (mi.ReturnType != typeof(T))
                    continue;

                methodInfo = mi;
                break;
            }

            if (methodInfo == null)
                throw new Exception();

            return (BitStreamReadHandler<T>)Delegate.CreateDelegate(typeof(BitStreamReadHandler<T>), bitStream, methodInfo);
        }

        static int GetSignedPartialBitValue(int value, int bits, int maxBits)
        {
            if (bits == 1)
                return (value != 0) ? 1 : 0;

            bool signed = value < 0;

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
            int bitMask = (1 << bits) - 1;
            return (uint)((int)value & bitMask);
        }

        static BitStreamWriteHandler<T> GetWriteHandler<T>(BitStream bitStream)
        {
            MethodInfo methodInfo = typeof(BitStream).GetMethod("Write", new Type[] { typeof(T) });
            return (BitStreamWriteHandler<T>)Delegate.CreateDelegate(typeof(BitStreamWriteHandler<T>), bitStream, methodInfo);
        }

        static IEnumerable<long> Range(long min, long max, long maxIterations)
        {
            if (max - min <= maxIterations)
            {
                for (long x = min; x < max; x++)
                {
                    yield return x;
                }
            }
            else
            {
                long diff = max - min;
                int stepMin = (int)(diff / maxIterations) - 10;
                int stepMax = (int)(diff / maxIterations) + 10;
                int step;
                Random rnd = new Random();

                for (long x = min; x < max; x += (long)step)
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
            long minValue = GetFieldValue<T>("MinValue");
            long maxValue = GetFieldValue<T>("MaxValue");
            return Range(minValue, maxValue, _maxRangeIterations);
        }

        #region Unit tests

        [Test]
        public void BitIO()
        {
            var bits = new int[] { 1, 0, 1, 0, 0, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0, 1, 1, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0 };
            BitStream bs = new BitStream(BitStreamMode.Write, bits.Length * 2);

            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < bits.Length; i++)
                {
                    bs.WriteBit(bits[i]);
                }
            }

            bs.Mode = BitStreamMode.Read;

            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < bits.Length; i++)
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

            BitStream src = new BitStream(BitStreamMode.Write, 1024);
            src.Write(a, 0, a.Length);
            src.Write(b, 0, b.Length);
            src.Write(c, 0, c.Length);
            src.Write(d, 0, d.Length);
            src.Write(e, 0, e.Length);

            BitStream dest = new BitStream(BitStreamMode.Write, 1024);
            dest.Write(src);

            dest.Mode = BitStreamMode.Read;

            for (int i = 0; i < a.Length; i++)
            {
                Assert.AreEqual(dest.ReadInt(), a[i]);
            }

            for (int i = 0; i < b.Length; i++)
            {
                Assert.AreEqual(dest.ReadFloat(), b[i]);
            }

            for (int i = 0; i < c.Length; i++)
            {
                Assert.AreEqual(dest.ReadLong(), c[i]);
            }

            for (int i = 0; i < d.Length; i++)
            {
                Assert.AreEqual(dest.ReadBool(), d[i]);
            }

            for (int i = 0; i < e.Length; i++)
            {
                Assert.AreEqual(dest.ReadByte(), e[i]);
            }
        }

        [Test]
        public void ByteBitIO()
        {
            const int numBits = 8;
            const int iterations = 5000;

            Random rnd = new Random();
            var values = new List<byte> { byte.MinValue, 1, byte.MaxValue };
            for (int i = 0; i < iterations; i++)
            {
                values.Add((byte)rnd.Next(byte.MinValue, byte.MaxValue));
            }

            BitStream bs = new BitStream(BitStreamMode.Write, values.Count * (numBits / 8) * 2);

            for (int i = 0; i < values.Count; i++)
            {
                int bits = GetBitsAmountForBitTest(i, numBits);
                bs.Write(values[i], bits);
            }

            bs.Mode = BitStreamMode.Read;

            for (int i = 0; i < values.Count; i++)
            {
                int bits = GetBitsAmountForBitTest(i, numBits);
                uint original = GetUnsignedPartialBitValue(values[i], bits);
                AreBitsEqual(original, bs.ReadByte(bits), bits);
            }
        }

        [Test]
        public void ByteIO()
        {
            BatchIOTester(from value in TRange<byte>()
                          select (byte)value);
        }

        [Test]
        public void ByteMinMaxTest()
        {
            const byte tMin = byte.MinValue;
            const byte tMax = byte.MaxValue;
            const int bits = sizeof(byte) * 8;

            BitStream bs = new BitStream(BitStreamMode.Write, (bits / 8) * bits * 4);

            for (int i = 1; i <= bits; i++)
            {
                bs.Write(tMin, i);
                bs.Write(tMax, i);
            }

            bs.Mode = BitStreamMode.Read;

            for (int i = 1; i <= bits; i++)
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

            Random rnd = new Random();

            // Structuring the loop like this allows us to run tons of numbers, but without having
            // to wait so goddamn long for any results
            for (int loop = 0; loop < 10; loop++)
            {
                var values = new List<double> { double.MinValue, 1, double.MaxValue };
                for (int i = 0; i < iterations; i++)
                {
                    values.Add(rnd.NextDouble());
                }

                BitStream bs = new BitStream(BitStreamMode.Write, iterations * numBits * 2);

                for (int i = 0; i < values.Count; i++)
                {
                    bs.Write(values[i]);
                }

                bs.Mode = BitStreamMode.Read;

                for (int i = 0; i < values.Count; i++)
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

            Random rnd = new Random();

            // Structuring the loop like this allows us to run tons of numbers, but without having
            // to wait so goddamn long for any results
            for (int loop = 0; loop < 10; loop++)
            {
                var values = new List<float> { float.MinValue, 1, float.MaxValue };
                for (int i = 0; i < iterations; i++)
                {
                    values.Add((float)rnd.NextDouble());
                }

                BitStream bs = new BitStream(BitStreamMode.Write, iterations * numBits * 2);

                for (int i = 0; i < values.Count; i++)
                {
                    bs.Write(values[i]);
                }

                bs.Mode = BitStreamMode.Read;

                for (int i = 0; i < values.Count; i++)
                {
                    Assert.AreEqual(values[i], bs.ReadFloat());
                }
            }
        }

        [Test]
        public void GetBufferTest()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 16);
            bs.Write((byte)20);
            bs.Write((byte)8);
            bs.Write(true);

            bs.Mode = BitStreamMode.Read;
            var buff = bs.GetBuffer();

            Assert.AreEqual(20, buff[0]);
            Assert.AreEqual(8, buff[1]);
            Assert.AreEqual(128, buff[2] & (1 << 7));
        }

        [Test]
        public void HighestWrittenIndexTest()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 1024);

            int expected = -1;
            for (int i = 0; i < 100; i++)
            {
                Assert.AreEqual(expected, bs.HighestWrittenIndex);
                bs.Write((byte)1);
                expected += 1;
            }
        }

        [Test]
        public void IntBitIO()
        {
            const int numBits = 32;
            const int iterations = 10000;

            Random rnd = new Random();

            for (int loop = 0; loop < 10; loop++)
            {
                BitStream bs = new BitStream(BitStreamMode.Write, iterations * numBits * 2);

                var values = new List<int> { int.MaxValue, 1, int.MinValue, 0, -1635136632 };
                for (int i = 0; i < iterations; i++)
                {
                    values.Add(rnd.Next(int.MinValue, int.MaxValue));
                }

                for (int i = 0; i < values.Count; i++)
                {
                    int bits = GetBitsAmountForBitTest(i, numBits);
                    Assert.LessOrEqual(bits, numBits);
                    Assert.GreaterOrEqual(bits, 1);
                    bs.Write(values[i], bits);
                }

                bs.Mode = BitStreamMode.Read;

                for (int i = 0; i < values.Count; i++)
                {
                    int bits = GetBitsAmountForBitTest(i, numBits);
                    int original = GetSignedPartialBitValue(values[i], bits, numBits);
                    AreBitsEqual(original, bs.ReadInt(bits), numBits);
                }
            }
        }

        [Test]
        public void IntIO()
        {
            BatchIOTester(from value in TRange<int>()
                          select (int)value);
        }

        [Test]
        public void IntMinMaxTest()
        {
            const int tMin = int.MinValue;
            const int tMax = int.MaxValue;
            const int bits = sizeof(int) * 8;

            BitStream bs = new BitStream(BitStreamMode.Write, (bits / 8) * bits * 4);

            for (int i = 1; i <= bits; i++)
            {
                bs.Write(tMin, i);
                bs.Write(tMax, i);
            }

            bs.Mode = BitStreamMode.Read;

            for (int i = 1; i <= bits; i++)
            {
                int min = bs.ReadInt(i);
                int max = bs.ReadInt(i);

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
            BitStream bs = new BitStream(BitStreamMode.Write, 1024);

            int expectedBits = 0;
            for (int i = 0; i < 100; i++)
            {
                Assert.AreEqual(expectedBits, bs.LengthBits);
                int bits = 1 + (i % 31);
                bs.Write(i, bits);
                expectedBits += bits;
            }
        }

        [Test]
        public void LengthTest()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 1024);

            int expectedBits = 0;
            for (int i = 0; i < 100; i++)
            {
                Assert.AreEqual(Math.Ceiling(expectedBits / 8f), bs.Length);
                int bits = 1 + (i % 31);
                bs.Write(i, bits);
                expectedBits += bits;
            }
        }

        [Test]
        public void LongIO()
        {
            const int numBits = 64;
            const int iterations = 5000;

            Random rnd = new Random();

            // Structuring the loop like this allows us to run tons of numbers, but without having
            // to wait so goddamn long for any results
            for (int loop = 0; loop < 10; loop++)
            {
                var values = new List<long> { long.MinValue, 1, long.MaxValue };
                for (long i = 0; i < iterations; i++)
                {
                    values.Add((long)rnd.NextDouble());
                }

                BitStream bs = new BitStream(BitStreamMode.Write, iterations * numBits * 2);

                for (int i = 0; i < values.Count; i++)
                {
                    bs.Write(values[i]);
                }

                bs.Mode = BitStreamMode.Read;

                for (int i = 0; i < values.Count; i++)
                {
                    Assert.AreEqual(values[i], bs.ReadLong());
                }
            }
        }

        [Test]
        public void NegativeOneIOTest()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 65536);
            for (int i = 2; i <= 32; i++)
            {
                bs.Write(-1, i);
            }

            for (int i = 2; i <= 16; i++)
            {
                bs.Write((short)-1, i);
            }

            for (int i = 2; i <= 8; i++)
            {
                bs.Write((sbyte)-1, i);
            }

            bs.Mode = BitStreamMode.Read;

            for (int i = 2; i <= 32; i++)
            {
                AreBitsEqual(-1, bs.ReadInt(i), i);
            }

            for (int i = 2; i <= 16; i++)
            {
                AreBitsEqual(-1, bs.ReadShort(i), i);
            }

            for (int i = 2; i <= 8; i++)
            {
                AreBitsEqual(-1, bs.ReadSByte(i), i);
            }
        }

        [Test]
        public void NullableBoolIO()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 256);
            bool? value = true;
            bool? nvalue = null;

            bs.Write(value);
            bs.Write(nvalue);
            bs.Write(value);

            bs.Mode = BitStreamMode.Read;

            Assert.AreEqual(value, bs.ReadNullableBool());
            Assert.AreEqual(nvalue, bs.ReadNullableBool());
            Assert.AreEqual(value, bs.ReadNullableBool());
        }

        [Test]
        public void NullableByteIO()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 256);
            byte? value = 10;
            byte? nvalue = null;

            bs.Write(value);
            bs.Write(nvalue);
            bs.Write(value);

            bs.Mode = BitStreamMode.Read;

            Assert.AreEqual(value, bs.ReadNullableByte());
            Assert.AreEqual(nvalue, bs.ReadNullableByte());
            Assert.AreEqual(value, bs.ReadNullableByte());
        }

        [Test]
        public void NullableDoubleIO()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 256);
            double? value = 10;
            double? nvalue = null;

            bs.Write(value);
            bs.Write(nvalue);
            bs.Write(value);

            bs.Mode = BitStreamMode.Read;

            Assert.AreEqual(value, bs.ReadNullableDouble());
            Assert.AreEqual(nvalue, bs.ReadNullableDouble());
            Assert.AreEqual(value, bs.ReadNullableDouble());
        }

        [Test]
        public void NullableFloatIO()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 256);
            float? value = 10;
            float? nvalue = null;

            bs.Write(value);
            bs.Write(nvalue);
            bs.Write(value);

            bs.Mode = BitStreamMode.Read;

            Assert.AreEqual(value, bs.ReadNullableFloat());
            Assert.AreEqual(nvalue, bs.ReadNullableFloat());
            Assert.AreEqual(value, bs.ReadNullableFloat());
        }

        [Test]
        public void NullableIntIO()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 256);
            int? value = 10;
            int? nvalue = null;

            bs.Write(value);
            bs.Write(nvalue);
            bs.Write(value);

            bs.Mode = BitStreamMode.Read;

            Assert.AreEqual(value, bs.ReadNullableInt());
            Assert.AreEqual(nvalue, bs.ReadNullableInt());
            Assert.AreEqual(value, bs.ReadNullableInt());
        }

        [Test]
        public void NullableLongIO()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 256);
            long? value = 10;
            long? nvalue = null;

            bs.Write(value);
            bs.Write(nvalue);
            bs.Write(value);

            bs.Mode = BitStreamMode.Read;

            Assert.AreEqual(value, bs.ReadNullableLong());
            Assert.AreEqual(nvalue, bs.ReadNullableLong());
            Assert.AreEqual(value, bs.ReadNullableLong());
        }

        [Test]
        public void NullableSByteIO()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 256);
            sbyte? value = 10;
            sbyte? nvalue = null;

            bs.Write(value);
            bs.Write(nvalue);
            bs.Write(value);

            bs.Mode = BitStreamMode.Read;

            Assert.AreEqual(value, bs.ReadNullableSByte());
            Assert.AreEqual(nvalue, bs.ReadNullableSByte());
            Assert.AreEqual(value, bs.ReadNullableSByte());
        }

        [Test]
        public void NullableShortIO()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 256);
            short? value = 10;
            short? nvalue = null;

            bs.Write(value);
            bs.Write(nvalue);
            bs.Write(value);

            bs.Mode = BitStreamMode.Read;

            Assert.AreEqual(value, bs.ReadNullableShort());
            Assert.AreEqual(nvalue, bs.ReadNullableShort());
            Assert.AreEqual(value, bs.ReadNullableShort());
        }

        [Test]
        public void NullableUIntIO()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 256);
            uint? value = 10;
            uint? nvalue = null;

            bs.Write(value);
            bs.Write(nvalue);
            bs.Write(value);

            bs.Mode = BitStreamMode.Read;

            Assert.AreEqual(value, bs.ReadNullableUInt());
            Assert.AreEqual(nvalue, bs.ReadNullableUInt());
            Assert.AreEqual(value, bs.ReadNullableUInt());
        }

        [Test]
        public void NullableULongIO()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 256);
            ulong? value = 10;
            ulong? nvalue = null;

            bs.Write(value);
            bs.Write(nvalue);
            bs.Write(value);

            bs.Mode = BitStreamMode.Read;

            Assert.AreEqual(value, bs.ReadNullableULong());
            Assert.AreEqual(nvalue, bs.ReadNullableULong());
            Assert.AreEqual(value, bs.ReadNullableULong());
        }

        [Test]
        public void NullableUShortIO()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 256);
            ushort? value = 10;
            ushort? nvalue = null;

            bs.Write(value);
            bs.Write(nvalue);
            bs.Write(value);

            bs.Mode = BitStreamMode.Read;

            Assert.AreEqual(value, bs.ReadNullableUShort());
            Assert.AreEqual(nvalue, bs.ReadNullableUShort());
            Assert.AreEqual(value, bs.ReadNullableUShort());
        }

        [Test]
        public void PerfectLengthStringIO()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 65536);

            const int count = 1000;

            var strings = new string[count];
            Random rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                var chars = new char[rnd.Next(5, 20)];
                for (int j = 0; j < chars.Length; j++)
                {
                    chars[j] = (char)rnd.Next(0, 128);
                }
                strings[i] = new string(chars);
            }

            foreach (string s in strings)
            {
                bs.Write(s, s.Length);
            }

            bs.Mode = BitStreamMode.Read;

            foreach (string s in strings)
            {
                Assert.AreEqual(s, bs.ReadString(s.Length));
            }
        }

        [Test]
        public void ReadBoolWhenInWriteModeTest()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 10);

            bs.Write(true);

            try
            {
                bs.ReadBool();
                Assert.Fail("Expected InvalidOperationException.");
            }
            catch (InvalidOperationException)
            {
            }
        }

        [Test]
        public void ReadPastDynamicBufferTest()
        {
            BitStream bs = new BitStream(BitStreamMode.Read, 10) { ReadMode = BitStreamBufferMode.Dynamic };

            for (int i = 0; i < 100; i++)
            {
                bs.ReadInt();
            }
        }

        [Test]
        public void ReadPastStaticBufferTest()
        {
            BitStream bs = new BitStream(BitStreamMode.Read, 10) { ReadMode = BitStreamBufferMode.Static };

            try
            {
                for (int i = 0; i < 100; i++)
                {
                    bs.ReadInt();
                }

                Assert.Fail("Expected OverflowException.");
            }
            catch (OverflowException)
            {
            }
        }

        [Test]
        public void ReadWhenInWriteModeTest()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 10);

            bs.Write(10);

            try
            {
                bs.ReadInt();
                Assert.Fail("Expected InvalidOperationException.");
            }
            catch (InvalidOperationException)
            {
            }
        }

        [Test]
        public void SByteBitIO()
        {
            const int numBits = 8;
            const int iterations = 5000;

            Random rnd = new Random();
            var values = new List<sbyte> { sbyte.MinValue, 1, sbyte.MaxValue };
            for (int i = 0; i < iterations; i++)
            {
                values.Add((sbyte)rnd.Next(sbyte.MinValue, sbyte.MaxValue));
            }

            BitStream bs = new BitStream(BitStreamMode.Write, values.Count * (numBits / 8) * 2);

            for (int i = 2; i < values.Count; i++)
            {
                int bits = GetBitsAmountForBitTest(i, numBits);
                bs.Write(values[i], bits);
            }

            bs.Mode = BitStreamMode.Read;

            for (int i = 2; i < values.Count; i++)
            {
                int bits = GetBitsAmountForBitTest(i, numBits);
                int original = GetSignedPartialBitValue(values[i], bits, numBits);
                AreBitsEqual(original, bs.ReadSByte(bits), bits);
            }
        }

        [Test]
        public void SByteIO()
        {
            BatchIOTester(from value in TRange<sbyte>()
                          select (sbyte)value);
        }

        [Test]
        public void SByteMinMaxTest()
        {
            const sbyte tMin = sbyte.MinValue;
            const sbyte tMax = sbyte.MaxValue;
            const int bits = sizeof(sbyte) * 8;

            BitStream bs = new BitStream(BitStreamMode.Write, (bits / 8) * bits * 4);

            for (int i = 1; i <= bits; i++)
            {
                bs.Write(tMin, i);
                bs.Write(tMax, i);
            }

            bs.Mode = BitStreamMode.Read;

            for (int i = 1; i <= bits; i++)
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

            Random rnd = new Random();
            var values = new List<int> { int.MinValue, 1, int.MaxValue };
            for (int i = 0; i < iterations; i++)
            {
                values.Add(rnd.Next(int.MinValue, int.MaxValue));
            }

            BitStream bs = new BitStream(BitStreamMode.Write, iterations * numBits * 2);

            for (int i = 0; i < values.Count; i++)
            {
                int bits = GetBitsAmountForBitTest(i, numBits);
                bs.Write(values[i], bits);
            }

            bs.Mode = BitStreamMode.Read;

            for (int i = 0; i < values.Count; i++)
            {
                int bits = GetBitsAmountForBitTest(i, numBits);

                if (i % 2 == 0)
                {
                    bs.SeekFromCurrentPosition(BitStreamSeekOrigin.Current, bits);
                    continue;
                }

                int original = GetSignedPartialBitValue(values[i], bits, numBits);
                AreBitsEqual(original, bs.ReadInt(bits), bits);
            }
        }

        [Test]
        public void ShortBitIO()
        {
            const int numBits = 16;
            const int iterations = 20000;

            Random rnd = new Random();
            var values = new List<short> { short.MinValue, 1, short.MaxValue };
            for (short i = 0; i < iterations; i++)
            {
                values.Add((short)rnd.Next(short.MinValue, short.MaxValue));
            }

            BitStream bs = new BitStream(BitStreamMode.Write, iterations * numBits * 2);

            for (int i = 0; i < values.Count; i++)
            {
                int bits = GetBitsAmountForBitTest(i, numBits);
                Assert.LessOrEqual(bits, numBits);
                Assert.GreaterOrEqual(bits, 1);
                bs.Write(values[i], bits);
            }

            bs.Mode = BitStreamMode.Read;

            for (int i = 0; i < values.Count; i++)
            {
                int bits = GetBitsAmountForBitTest(i, numBits);
                int original = GetSignedPartialBitValue(values[i], bits, numBits);
                AreBitsEqual((short)original, bs.ReadShort(bits), bits);
            }
        }

        [Test]
        public void ShortIO()
        {
            BatchIOTester(from value in TRange<short>()
                          select (short)value);
        }

        [Test]
        public void ShortMinMaxTest()
        {
            const short tMin = short.MinValue;
            const short tMax = short.MaxValue;
            const int bits = sizeof(short) * 8;

            BitStream bs = new BitStream(BitStreamMode.Write, (bits / 8) * bits * 4);

            for (int i = 1; i <= bits; i++)
            {
                bs.Write(tMin, i);
                bs.Write(tMax, i);
            }

            bs.Mode = BitStreamMode.Read;

            for (int i = 1; i <= bits; i++)
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
            BitStream bs = new BitStream(BitStreamMode.Write, 1024);
            bs.Write((byte)0, 1);
            bs.Write((byte)1, 2);
            bs.Write((byte)2, 3);
            bs.Write((byte)3, 4);
            bs.Write((byte)10, 5);

            bs.Mode = BitStreamMode.Read;

            Assert.AreEqual(bs.ReadByte(1), 0);
            Assert.AreEqual(bs.ReadByte(2), 1);
            Assert.AreEqual(bs.ReadByte(3), 2);
            Assert.AreEqual(bs.ReadByte(4), 3);
            Assert.AreEqual(bs.ReadByte(5), 10);
        }

        [Test]
        public void SimpleIntIO()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 65536);

            bs.Write(-25);
            bs.Write(-13);
            bs.Write(13);
            bs.Write(25);
            bs.Write(-8);
            bs.Write(8);

            bs.Mode = BitStreamMode.Read;

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
            BitStream bs = new BitStream(BitStreamMode.Write, 1024);

            int expectedBits = 0;
            for (int i = 0; i < 100; i++)
            {
                Assert.AreEqual(expectedBits, bs.LengthBits);
                bs.Write((byte)1, 8);
                expectedBits += 8;
            }
        }

        [Test]
        public void SimpleShortIO()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 65536);

            bs.Write((short)-25, 15);
            bs.Write((short)-13);
            bs.Write((short)13);
            bs.Write((short)25, 15);
            bs.Write((short)-8, 12);
            bs.Write((short)8, 12);

            bs.Mode = BitStreamMode.Read;

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
            BitStream bs = new BitStream(BitStreamMode.Write, 65536);

            var strings = new string[]
            { "Hello", "", "abcdefghijklmnopqrstuvwxyz", "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "1234567890!@#$%^&*()" };

            foreach (string s in strings)
            {
                bs.Write(s);
            }

            bs.Mode = BitStreamMode.Read;

            foreach (string s in strings)
            {
                Assert.AreEqual(s, bs.ReadString());
            }
        }

        [Test]
        public void SimpleUIntIO()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 65536);
            bs.Write(uint.MinValue);
            bs.Write(uint.MaxValue);
            bs.Write((uint)25);
            bs.Write((uint)13);
            bs.Write((uint)8);

            bs.Mode = BitStreamMode.Read;

            Assert.AreEqual(uint.MinValue, bs.ReadUInt());
            Assert.AreEqual(uint.MaxValue, bs.ReadUInt());
            Assert.AreEqual(25, bs.ReadUInt());
            Assert.AreEqual(13, bs.ReadUInt());
            Assert.AreEqual(8, bs.ReadUInt());
        }

        [Test]
        public void StringIO()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 65536);

            const int count = 1000;

            var strings = new string[count];
            Random rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                var chars = new char[rnd.Next(5, 20)];
                for (int j = 0; j < chars.Length; j++)
                {
                    chars[j] = (char)rnd.Next(0, 128);
                }
                strings[i] = new string(chars);
            }

            foreach (string s in strings)
            {
                bs.Write(s);
            }

            bs.Mode = BitStreamMode.Read;

            foreach (string s in strings)
            {
                Assert.AreEqual(s, bs.ReadString());
            }
        }

        [Test]
        public void SuppliedBufferTest()
        {
            var buff = new byte[16];
            BitStream bs = new BitStream(buff) { Mode = BitStreamMode.Write };
            bs.Write((byte)20);
            bs.Write((byte)8);
            bs.Write(true);

            bs.Mode = BitStreamMode.Read;

            Assert.AreEqual(20, buff[0]);
            Assert.AreEqual(8, buff[1]);
            Assert.AreEqual(128, buff[2] & (1 << 7));
        }

        [Test]
        public void UIntBitIO()
        {
            const int numBits = 32;
            const int iterations = 50000;

            Random rnd = new Random();
            var values = new List<uint> { uint.MinValue, 1, uint.MaxValue };
            for (int i = 0; i < iterations; i++)
            {
                values.Add((uint)rnd.NextDouble());
            }

            BitStream bs = new BitStream(BitStreamMode.Write, iterations * numBits * 2);

            for (int i = 0; i < values.Count; i++)
            {
                int bits = GetBitsAmountForBitTest(i, numBits);
                bs.Write(values[i], bits);
            }

            bs.Mode = BitStreamMode.Read;

            for (int i = 0; i < values.Count; i++)
            {
                int bits = GetBitsAmountForBitTest(i, numBits);
                uint original = GetUnsignedPartialBitValue(values[i], bits);
                AreBitsEqual(original, bs.ReadUInt(bits), bits);
            }
        }

        [Test]
        public void UIntIO()
        {
            BatchIOTester(from value in TRange<uint>()
                          select (uint)value);
        }

        [Test]
        public void ULongIO()
        {
            const int numBits = 64;
            const int iterations = 5000;

            Random rnd = new Random();

            // Structuring the loop like this allows us to run tons of numbers, but without having
            // to wait so goddamn long for any results
            for (int loop = 0; loop < 10; loop++)
            {
                var values = new List<ulong> { ulong.MinValue, 1, ulong.MaxValue };
                for (int i = 0; i < iterations; i++)
                {
                    values.Add((ulong)rnd.NextDouble());
                }

                BitStream bs = new BitStream(BitStreamMode.Write, iterations * numBits * 2);

                for (int i = 0; i < values.Count; i++)
                {
                    bs.Write(values[i]);
                }

                bs.Mode = BitStreamMode.Read;

                for (int i = 0; i < values.Count; i++)
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

            Random rnd = new Random();
            var values = new List<ushort> { ushort.MinValue, 1, ushort.MaxValue };
            for (ushort i = 0; i < iterations; i++)
            {
                values.Add((ushort)rnd.Next(ushort.MinValue, ushort.MaxValue));
            }

            BitStream bs = new BitStream(BitStreamMode.Write, iterations * numBits * 2);

            for (int i = 0; i < values.Count; i++)
            {
                int bits = GetBitsAmountForBitTest(i, numBits);
                bs.Write(values[i], bits);
            }

            bs.Mode = BitStreamMode.Read;

            for (int i = 0; i < values.Count; i++)
            {
                int bits = GetBitsAmountForBitTest(i, numBits);
                uint original = GetUnsignedPartialBitValue(values[i], bits);
                AreBitsEqual(original, bs.ReadUShort(bits), bits);
            }
        }

        [Test]
        public void UShortIO()
        {
            BatchIOTester(from value in TRange<ushort>()
                          select (ushort)value);
        }

        [Test]
        public void VariableLengthStringIO()
        {
            const int count = 10;

            for (int length = 0; length < 300; length += 4)
            {
                BitStream bs = new BitStream(BitStreamMode.Write, 65536);

                var strings = new string[count];
                Random rnd = new Random();
                for (int i = 0; i < count; i++)
                {
                    var chars = new char[rnd.Next(200, 500)];
                    for (int j = 0; j < chars.Length; j++)
                    {
                        chars[j] = (char)rnd.Next(0, 128);
                    }
                    strings[i] = new string(chars);
                }

                foreach (string s in strings)
                {
                    string expected = s;
                    if (expected.Length > length)
                        expected = expected.Substring(0, length);
                    bs.Write(expected, length);
                }

                bs.Mode = BitStreamMode.Read;

                foreach (string s in strings)
                {
                    string expected = s;
                    if (expected.Length > length)
                        expected = expected.Substring(0, length);
                    Assert.AreEqual(expected, bs.ReadString(length));
                }
            }
        }

        [Test]
        public void WriteBoolWhenInReadModeTest()
        {
            BitStream bs = new BitStream(BitStreamMode.Read, 10);

            try
            {
                bs.Write(true);
                Assert.Fail("Expected InvalidOperationException.");
            }
            catch (InvalidOperationException)
            {
            }
        }

        [Test]
        public void WritePastDynamicBufferTest()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 10) { WriteMode = BitStreamBufferMode.Dynamic };

            for (int i = 0; i < 100; i++)
            {
                bs.Write(i);
            }
        }

        [Test]
        public void WritePastStaticBufferTest()
        {
            BitStream bs = new BitStream(BitStreamMode.Write, 10) { WriteMode = BitStreamBufferMode.Static };

            try
            {
                for (int i = 0; i < 100; i++)
                {
                    bs.Write(i);
                }

                Assert.Fail("Expected OverflowException.");
            }
            catch (OverflowException)
            {
            }
        }

        [Test]
        public void WriteWhenInReadModeTest()
        {
            BitStream bs = new BitStream(BitStreamMode.Read, 10);

            try
            {
                bs.Write(10);
                Assert.Fail("Expected InvalidOperationException.");
            }
            catch (InvalidOperationException)
            {
            }
        }

        #endregion
    }
}