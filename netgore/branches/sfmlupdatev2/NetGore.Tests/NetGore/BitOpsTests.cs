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
                var req = BitOps.RequiredBits(i);
                Assert.Greater(1 << req, i, "i: " + i);
            }
        }

        [Test]
        public void CountBitsTest()
        {
            const string errDetails = "Value: {0}    Type: {1}";
            const int testIterations = 1000;

            var rnd = new Random();

            for (var test = 0; test < testIterations; test++)
            {
                byte value = 0;
                const int numBits = sizeof(byte) * 8;

                var bitsSet = 0;
                for (var i = 0; i < numBits; i++)
                {
                    if (rnd.Next(0, 2) == 0)
                    {
                        value |= (byte)(1 << i);
                        bitsSet++;
                    }
                }

                var details = string.Format(errDetails, value, value.GetType());
                Assert.AreEqual(BitOps.CountBits(value), bitsSet, details);
                for (var i = 0; i <= numBits; i++)
                {
                    if (i != bitsSet)
                        Assert.AreNotEqual(BitOps.CountBits(value), i, details);
                }
            }

            for (var test = 0; test < testIterations; test++)
            {
                sbyte value = 0;
                const int numBits = sizeof(sbyte) * 8;

                var bitsSet = 0;
                for (var i = 0; i < numBits; i++)
                {
                    if (rnd.Next(0, 2) == 0)
                    {
                        value |= (sbyte)(1 << i);
                        bitsSet++;
                    }
                }

                var details = string.Format(errDetails, value, value.GetType());
                Assert.AreEqual(BitOps.CountBits(value), bitsSet, details);
                for (var i = 0; i <= numBits; i++)
                {
                    if (i != bitsSet)
                        Assert.AreNotEqual(BitOps.CountBits(value), i, details);
                }
            }

            for (var test = 0; test < testIterations; test++)
            {
                ushort value = 0;
                const int numBits = sizeof(ushort) * 8;

                var bitsSet = 0;
                for (var i = 0; i < numBits; i++)
                {
                    if (rnd.Next(0, 2) == 0)
                    {
                        value |= (ushort)(1 << i);
                        bitsSet++;
                    }
                }

                var details = string.Format(errDetails, value, value.GetType());
                Assert.AreEqual(BitOps.CountBits(value), bitsSet, details);
                for (var i = 0; i <= numBits; i++)
                {
                    if (i != bitsSet)
                        Assert.AreNotEqual(BitOps.CountBits(value), i, details);
                }
            }

            for (var test = 0; test < testIterations; test++)
            {
                short value = 0;
                const int numBits = sizeof(short) * 8;

                var bitsSet = 0;
                for (var i = 0; i < numBits; i++)
                {
                    if (rnd.Next(0, 2) == 0)
                    {
                        value |= (short)(1 << i);
                        bitsSet++;
                    }
                }

                var details = string.Format(errDetails, value, value.GetType());
                Assert.AreEqual(BitOps.CountBits(value), bitsSet, details);
                for (var i = 0; i <= numBits; i++)
                {
                    if (i != bitsSet)
                        Assert.AreNotEqual(BitOps.CountBits(value), i, details);
                }
            }

            for (var test = 0; test < testIterations; test++)
            {
                uint value = 0;
                const int numBits = sizeof(uint) * 8;

                var bitsSet = 0;
                for (var i = 0; i < numBits; i++)
                {
                    if (rnd.Next(0, 2) == 0)
                    {
                        value |= ((uint)1 << i);
                        bitsSet++;
                    }
                }

                var details = string.Format(errDetails, value, value.GetType());
                Assert.AreEqual(BitOps.CountBits(value), bitsSet, details);
                for (var i = 0; i <= numBits; i++)
                {
                    if (i != bitsSet)
                        Assert.AreNotEqual(BitOps.CountBits(value), i, details);
                }
            }

            for (var test = 0; test < testIterations; test++)
            {
                var value = 0;
                const int numBits = sizeof(int) * 8;

                var bitsSet = 0;
                for (var i = 0; i < numBits; i++)
                {
                    if (rnd.Next(0, 2) == 0)
                    {
                        value |= (1 << i);
                        bitsSet++;
                    }
                }

                var details = string.Format(errDetails, value, value.GetType());
                Assert.AreEqual(BitOps.CountBits(value), bitsSet, details);
                for (var i = 0; i <= numBits; i++)
                {
                    if (i != bitsSet)
                        Assert.AreNotEqual(BitOps.CountBits(value), i, details);
                }
            }

            for (var test = 0; test < testIterations; test++)
            {
                ulong value = 0;
                const int numBits = sizeof(ulong) * 8;

                var bitsSet = 0;
                for (var i = 0; i < numBits; i++)
                {
                    if (rnd.Next(0, 2) == 0)
                    {
                        value |= ((ulong)1 << i);
                        bitsSet++;
                    }
                }

                var details = string.Format(errDetails, value, value.GetType());
                Assert.AreEqual(BitOps.CountBits(value), bitsSet, details);
                for (var i = 0; i <= numBits; i++)
                {
                    if (i != bitsSet)
                        Assert.AreNotEqual(BitOps.CountBits(value), i, details);
                }
            }

            for (var test = 0; test < testIterations; test++)
            {
                long value = 0;
                const int numBits = sizeof(long) * 8;

                var bitsSet = 0;
                for (var i = 0; i < numBits; i++)
                {
                    if (rnd.Next(0, 2) == 0)
                    {
                        value |= ((long)1 << i);
                        bitsSet++;
                    }
                }

                var details = string.Format(errDetails, value, value.GetType());
                Assert.AreEqual(BitOps.CountBits(value), bitsSet, details);
                for (var i = 0; i <= numBits; i++)
                {
                    if (i != bitsSet)
                        Assert.AreNotEqual(BitOps.CountBits(value), i, details);
                }
            }
        }

        [Test]
        public void NextPowerOf2Test()
        {
            Assert.AreEqual(16, BitOps.NextPowerOf2(16));
            Assert.AreEqual(16, BitOps.NextPowerOf2(15));
            Assert.AreEqual(2, BitOps.NextPowerOf2(2));
            Assert.AreEqual(4, BitOps.NextPowerOf2(3));
            Assert.AreEqual(128, BitOps.NextPowerOf2(65));

            Assert.AreEqual(16, BitOps.NextPowerOf2((uint)16));
            Assert.AreEqual(16, BitOps.NextPowerOf2((uint)15));
            Assert.AreEqual(2, BitOps.NextPowerOf2((uint)2));
            Assert.AreEqual(4, BitOps.NextPowerOf2((uint)3));
            Assert.AreEqual(128, BitOps.NextPowerOf2((uint)65));
        }

        [Test]
        public void PowerOf2Test()
        {
            const string errDetails = "Iterator: {0}    Value: {1}    Type: {2}";

            for (var i = 1; i < 7; i++)
            {
                var value = (sbyte)Math.Pow(2, i);
                Assert.IsTrue(BitOps.IsPowerOf2(value), string.Format(errDetails, i, value, value.GetType()));
            }

            for (var i = 1; i < 8; i++)
            {
                var value = (byte)Math.Pow(2, i);
                Assert.IsTrue(BitOps.IsPowerOf2(value), string.Format(errDetails, i, value, value.GetType()));
            }

            for (var i = 1; i < 15; i++)
            {
                var value = (short)Math.Pow(2, i);
                Assert.IsTrue(BitOps.IsPowerOf2(value), string.Format(errDetails, i, value, value.GetType()));
            }

            for (var i = 1; i < 16; i++)
            {
                var value = (ushort)Math.Pow(2, i);
                Assert.IsTrue(BitOps.IsPowerOf2(value), string.Format(errDetails, i, value, value.GetType()));
            }

            for (var i = 1; i < 31; i++)
            {
                var value = (int)Math.Pow(2, i);
                Assert.IsTrue(BitOps.IsPowerOf2(value), string.Format(errDetails, i, value, value.GetType()));
            }

            for (var i = 1; i < 32; i++)
            {
                var value = (uint)Math.Pow(2, i);
                Assert.IsTrue(BitOps.IsPowerOf2(value), string.Format(errDetails, i, value, value.GetType()));
            }

            for (var i = 1; i < 63; i++)
            {
                var value = (long)Math.Pow(2, i);
                Assert.IsTrue(BitOps.IsPowerOf2(value), string.Format(errDetails, i, value, value.GetType()));
            }

            for (var i = 1; i < 64; i++)
            {
                var value = (ulong)Math.Pow(2, i);
                Assert.IsTrue(BitOps.IsPowerOf2(value), string.Format(errDetails, i, value, value.GetType()));
            }
        }

        #endregion
    }
}