using System;
using NUnit.Framework;

namespace NetGore.IO.Tests
{
    [TestFixture]
    public class BitOpsTests
    {
        [Test]
        public void CountBitsTest()
        {
            const string errDetails = "Value: {0}    Type: {1}";
            const int testIterations = 1000;

            Random rnd = new Random();

            for (int test = 0; test < testIterations; test++)
            {
                byte value = 0;
                const int numBits = sizeof(byte) * 8;

                int bitsSet = 0;
                for (int i = 0; i < numBits; i++)
                {
                    if (rnd.Next(0, 2) == 0)
                    {
                        value |= (byte)(1 << i);
                        bitsSet++;
                    }
                }

                string details = string.Format(errDetails, value, value.GetType());
                Assert.AreEqual(BitOps.CountBits(value), bitsSet, details);
                for (int i = 0; i <= numBits; i++)
                {
                    if (i != bitsSet)
                        Assert.AreNotEqual(BitOps.CountBits(value), i, details);
                }
            }

            for (int test = 0; test < testIterations; test++)
            {
                sbyte value = 0;
                const int numBits = sizeof(sbyte) * 8;

                int bitsSet = 0;
                for (int i = 0; i < numBits; i++)
                {
                    if (rnd.Next(0, 2) == 0)
                    {
                        value |= (sbyte)(1 << i);
                        bitsSet++;
                    }
                }

                string details = string.Format(errDetails, value, value.GetType());
                Assert.AreEqual(BitOps.CountBits(value), bitsSet, details);
                for (int i = 0; i <= numBits; i++)
                {
                    if (i != bitsSet)
                        Assert.AreNotEqual(BitOps.CountBits(value), i, details);
                }
            }

            for (int test = 0; test < testIterations; test++)
            {
                ushort value = 0;
                const int numBits = sizeof(ushort) * 8;

                int bitsSet = 0;
                for (int i = 0; i < numBits; i++)
                {
                    if (rnd.Next(0, 2) == 0)
                    {
                        value |= (ushort)(1 << i);
                        bitsSet++;
                    }
                }

                string details = string.Format(errDetails, value, value.GetType());
                Assert.AreEqual(BitOps.CountBits(value), bitsSet, details);
                for (int i = 0; i <= numBits; i++)
                {
                    if (i != bitsSet)
                        Assert.AreNotEqual(BitOps.CountBits(value), i, details);
                }
            }

            for (int test = 0; test < testIterations; test++)
            {
                short value = 0;
                const int numBits = sizeof(short) * 8;

                int bitsSet = 0;
                for (int i = 0; i < numBits; i++)
                {
                    if (rnd.Next(0, 2) == 0)
                    {
                        value |= (short)(1 << i);
                        bitsSet++;
                    }
                }

                string details = string.Format(errDetails, value, value.GetType());
                Assert.AreEqual(BitOps.CountBits(value), bitsSet, details);
                for (int i = 0; i <= numBits; i++)
                {
                    if (i != bitsSet)
                        Assert.AreNotEqual(BitOps.CountBits(value), i, details);
                }
            }

            for (int test = 0; test < testIterations; test++)
            {
                uint value = 0;
                const int numBits = sizeof(uint) * 8;

                int bitsSet = 0;
                for (int i = 0; i < numBits; i++)
                {
                    if (rnd.Next(0, 2) == 0)
                    {
                        value |= ((uint)1 << i);
                        bitsSet++;
                    }
                }

                string details = string.Format(errDetails, value, value.GetType());
                Assert.AreEqual(BitOps.CountBits(value), bitsSet, details);
                for (int i = 0; i <= numBits; i++)
                {
                    if (i != bitsSet)
                        Assert.AreNotEqual(BitOps.CountBits(value), i, details);
                }
            }

            for (int test = 0; test < testIterations; test++)
            {
                int value = 0;
                const int numBits = sizeof(int) * 8;

                int bitsSet = 0;
                for (int i = 0; i < numBits; i++)
                {
                    if (rnd.Next(0, 2) == 0)
                    {
                        value |= (1 << i);
                        bitsSet++;
                    }
                }

                string details = string.Format(errDetails, value, value.GetType());
                Assert.AreEqual(BitOps.CountBits(value), bitsSet, details);
                for (int i = 0; i <= numBits; i++)
                {
                    if (i != bitsSet)
                        Assert.AreNotEqual(BitOps.CountBits(value), i, details);
                }
            }

            for (int test = 0; test < testIterations; test++)
            {
                ulong value = 0;
                const int numBits = sizeof(ulong) * 8;

                int bitsSet = 0;
                for (int i = 0; i < numBits; i++)
                {
                    if (rnd.Next(0, 2) == 0)
                    {
                        value |= ((ulong)1 << i);
                        bitsSet++;
                    }
                }

                string details = string.Format(errDetails, value, value.GetType());
                Assert.AreEqual(BitOps.CountBits(value), bitsSet, details);
                for (int i = 0; i <= numBits; i++)
                {
                    if (i != bitsSet)
                        Assert.AreNotEqual(BitOps.CountBits(value), i, details);
                }
            }

            for (int test = 0; test < testIterations; test++)
            {
                long value = 0;
                const int numBits = sizeof(long) * 8;

                int bitsSet = 0;
                for (int i = 0; i < numBits; i++)
                {
                    if (rnd.Next(0, 2) == 0)
                    {
                        value |= ((long)1 << i);
                        bitsSet++;
                    }
                }

                string details = string.Format(errDetails, value, value.GetType());
                Assert.AreEqual(BitOps.CountBits(value), bitsSet, details);
                for (int i = 0; i <= numBits; i++)
                {
                    if (i != bitsSet)
                        Assert.AreNotEqual(BitOps.CountBits(value), i, details);
                }
            }
        }

        [Test]
        public void DoubleToLongTest()
        {
            // NOTE: Do not worry that this test fails until we actually need to write a Double to the BitStream.
            Assert.Fail("Unsupported test.");

            const long iterations = 10000000;

            const long step = (long.MaxValue / iterations) / 2;
            for (long l = long.MinValue; l < long.MaxValue - step; l += step)
            {
                double d = BitOps.LongToDouble(l);
                long l2 = BitOps.DoubleToLong(d);

                if (l == l2)
                    continue;

                const string details = "Original bits: {0}   New bits: {1}";
                string lBits = Convert.ToString(l, 2);
                string l2Bits = Convert.ToString(l2, 2);
                Assert.AreEqual(l, l2, string.Format(details, lBits, l2Bits));
            }
        }

        [Test]
        public void PowerOf2Test()
        {
            const string errDetails = "Iterator: {0}    Value: {1}    Type: {2}";

            for (int i = 1; i < 7; i++)
            {
                sbyte value = (sbyte)Math.Pow(2, i);
                Assert.IsTrue(BitOps.IsPowerOf2(value), string.Format(errDetails, i, value, value.GetType()));
            }

            for (int i = 1; i < 8; i++)
            {
                byte value = (byte)Math.Pow(2, i);
                Assert.IsTrue(BitOps.IsPowerOf2(value), string.Format(errDetails, i, value, value.GetType()));
            }

            for (int i = 1; i < 15; i++)
            {
                short value = (short)Math.Pow(2, i);
                Assert.IsTrue(BitOps.IsPowerOf2(value), string.Format(errDetails, i, value, value.GetType()));
            }

            for (int i = 1; i < 16; i++)
            {
                ushort value = (ushort)Math.Pow(2, i);
                Assert.IsTrue(BitOps.IsPowerOf2(value), string.Format(errDetails, i, value, value.GetType()));
            }

            for (int i = 1; i < 31; i++)
            {
                int value = (int)Math.Pow(2, i);
                Assert.IsTrue(BitOps.IsPowerOf2(value), string.Format(errDetails, i, value, value.GetType()));
            }

            for (int i = 1; i < 32; i++)
            {
                uint value = (uint)Math.Pow(2, i);
                Assert.IsTrue(BitOps.IsPowerOf2(value), string.Format(errDetails, i, value, value.GetType()));
            }

            for (int i = 1; i < 63; i++)
            {
                long value = (long)Math.Pow(2, i);
                Assert.IsTrue(BitOps.IsPowerOf2(value), string.Format(errDetails, i, value, value.GetType()));
            }

            for (int i = 1; i < 64; i++)
            {
                ulong value = (ulong)Math.Pow(2, i);
                Assert.IsTrue(BitOps.IsPowerOf2(value), string.Format(errDetails, i, value, value.GetType()));
            }
        }
    }
}