using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore.Collections;
using NUnit.Framework;

namespace NetGore.Collections.Tests
{
    [TestFixture]
    public class MeanStackTests
    {
        /// <summary>
        /// Number of iterations to perform, where each iteration is the size of the MeanStack.
        /// </summary>
        const int _iterations = 70;

        /// <summary>
        /// Ending value for the sum of items to test.
        /// </summary>
        const int _itemEnd = 7;

        /// <summary>
        /// Starting value for the sum of items to test.
        /// </summary>
        const int _itemStart = 1;

        [Test]
        public void ByteMeanTest()
        {
            for (int i = _itemEnd; i < _iterations; i++)
            {
                var mf = new MeanStack<byte>(i);

                int sum = 0;
                for (int j = _itemStart; j <= _itemEnd; j++)
                {
                    sum += j;
                    mf.Push((byte)j);
                }

                int mean = sum / (_itemEnd - _itemStart + 1);
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void DoubleMeanTest()
        {
            for (int i = _itemEnd; i < _iterations; i++)
            {
                var mf = new MeanStack<double>(i);

                double sum = 0;
                for (int j = _itemStart; j <= _itemEnd; j++)
                {
                    sum += j;
                    mf.Push(j);
                }

                double mean = sum / (_itemEnd - _itemStart + 1);
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void FillTest()
        {
            for (int i = 1; i < 20; i++)
            {
                var mf = new MeanStack<int>(i);
                mf.Fill(i);
                Assert.AreEqual(i, mf.Mean());
            }
        }

        [Test]
        public void FloatMeanTest()
        {
            for (int i = _itemEnd; i < _iterations; i++)
            {
                var mf = new MeanStack<float>(i);

                float sum = 0;
                for (int j = _itemStart; j <= _itemEnd; j++)
                {
                    sum += j;
                    mf.Push(j);
                }

                float mean = sum / (_itemEnd - _itemStart + 1);
                float m = mf.Mean();
                Assert.AreEqual(mean, m);
            }
        }

        [Test]
        public void IntMeanTest()
        {
            for (int i = _itemEnd; i < _iterations; i++)
            {
                var mf = new MeanStack<int>(i);

                int sum = 0;
                for (int j = _itemStart; j <= _itemEnd; j++)
                {
                    sum += j;
                    mf.Push(j);
                }

                int mean = sum / (_itemEnd - _itemStart + 1);
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void LongMeanTest()
        {
            for (int i = _itemEnd; i < _iterations; i++)
            {
                var mf = new MeanStack<long>(i);

                long sum = 0;
                for (long j = _itemStart; j <= _itemEnd; j++)
                {
                    sum += j;
                    mf.Push(j);
                }

                long mean = sum / (_itemEnd - _itemStart + 1);
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void SByteMeanTest()
        {
            for (int i = _itemEnd; i < _iterations; i++)
            {
                var mf = new MeanStack<sbyte>(i);

                int sum = 0;
                for (int j = _itemStart; j <= _itemEnd; j++)
                {
                    sum += j;
                    mf.Push((sbyte)j);
                }

                int mean = sum / (_itemEnd - _itemStart + 1);
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void ShortMeanTest()
        {
            for (int i = _itemEnd; i < _iterations; i++)
            {
                var mf = new MeanStack<short>(i);

                int sum = 0;
                for (int j = _itemStart; j <= _itemEnd; j++)
                {
                    sum += j;
                    mf.Push((short)j);
                }

                int mean = sum / (_itemEnd - _itemStart + 1);
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void UIntMeanTest()
        {
            for (int i = _itemEnd; i < _iterations; i++)
            {
                var mf = new MeanStack<uint>(i);

                uint sum = 0;
                for (uint j = _itemStart; j <= _itemEnd; j++)
                {
                    sum += j;
                    mf.Push(j);
                }

                long mean = sum / (_itemEnd - _itemStart + 1);
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void ULongMeanTest()
        {
            for (int i = _itemEnd; i < _iterations; i++)
            {
                var mf = new MeanStack<ulong>(i);

                ulong sum = 0;
                for (ulong j = _itemStart; j <= _itemEnd; j++)
                {
                    sum += j;
                    mf.Push(j);
                }

                ulong mean = sum / (_itemEnd - _itemStart + 1);
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void UShortMeanTest()
        {
            for (int i = _itemEnd; i < _iterations; i++)
            {
                var mf = new MeanStack<ushort>(i);

                int sum = 0;
                for (int j = _itemStart; j <= _itemEnd; j++)
                {
                    sum += j;
                    mf.Push((ushort)j);
                }

                int mean = sum / (_itemEnd - _itemStart + 1);
                Assert.AreEqual(mean, mf.Mean());
            }
        }
    }
}