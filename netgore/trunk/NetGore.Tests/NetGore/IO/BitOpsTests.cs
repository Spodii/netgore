using System;
using System.Linq;
using NUnit.Framework;

namespace NetGore.Tests.IO
{
    [TestFixture]
    public class BitOpsTests
    {
        #region Unit tests

        [Test]
        public void BitsRequiredTest()
        {
            for (uint i = 0; i < 1200; i++)
            {
                int req = BitOps.RequiredBits(i);
                Assert.Greater(1 << req, i, "i: " + i);
            }
        }

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

        #endregion
    }
}