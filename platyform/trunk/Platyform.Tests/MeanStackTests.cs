using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace Platyform.Tests
{
    [TestFixture]
    public class MeanStackTests
    {
        [Test]
        public void ByteMeanTest()
        {
            for (int i = 1; i < 50; i++)
            {
                var mf = new MeanStack<byte>(i);

                int sum = 0;
                for (int j = 1; j <= i; j++)
                {
                    sum += j;
                    mf.Push((byte)j);
                }

                int mean = sum / i;
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void DoubleMeanTest()
        {
            for (int i = 1; i < 50; i++)
            {
                var mf = new MeanStack<double>(i);

                double sum = 0;
                for (int j = 1; j <= i; j++)
                {
                    sum += j;
                    mf.Push(j);
                }

                double mean = sum / i;
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
            for (int i = 1; i < 50; i++)
            {
                var mf = new MeanStack<float>(i);

                float sum = 0;
                for (int j = 1; j <= i; j++)
                {
                    sum += j;
                    mf.Push(j);
                }

                float mean = sum / i;
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void IntMeanTest()
        {
            for (int i = 1; i < 50; i++)
            {
                var mf = new MeanStack<int>(i);

                int sum = 0;
                for (int j = 1; j <= i; j++)
                {
                    sum += j;
                    mf.Push(j);
                }

                int mean = sum / i;
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void LongMeanTest()
        {
            for (int i = 1; i < 50; i++)
            {
                var mf = new MeanStack<long>(i);

                long sum = 0;
                for (long j = 1; j <= i; j++)
                {
                    sum += j;
                    mf.Push(j);
                }

                long mean = sum / i;
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void SByteMeanTest()
        {
            for (int i = 1; i < 50; i++)
            {
                var mf = new MeanStack<sbyte>(i);

                int sum = 0;
                for (int j = 1; j <= i; j++)
                {
                    sum += j;
                    mf.Push((sbyte)j);
                }

                int mean = sum / i;
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void ShortMeanTest()
        {
            for (int i = 1; i < 50; i++)
            {
                var mf = new MeanStack<short>(i);

                int sum = 0;
                for (int j = 1; j <= i; j++)
                {
                    sum += j;
                    mf.Push((short)j);
                }

                int mean = sum / i;
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void UIntMeanTest()
        {
            for (int i = 1; i < 50; i++)
            {
                var mf = new MeanStack<uint>(i);

                uint sum = 0;
                for (uint j = 1; j <= i; j++)
                {
                    sum += j;
                    mf.Push(j);
                }

                long mean = sum / i;
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void ULongMeanTest()
        {
            for (int i = 1; i < 50; i++)
            {
                var mf = new MeanStack<ulong>(i);

                ulong sum = 0;
                for (ulong j = 1; j <= (ulong)i; j++)
                {
                    sum += j;
                    mf.Push(j);
                }

                ulong mean = sum / (ulong)i;
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void UShortMeanTest()
        {
            for (int i = 1; i < 50; i++)
            {
                var mf = new MeanStack<ushort>(i);

                int sum = 0;
                for (int j = 1; j <= i; j++)
                {
                    sum += j;
                    mf.Push((ushort)j);
                }

                int mean = sum / i;
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void Vector2MeanTest()
        {
            for (int i = 1; i < 50; i++)
            {
                var mf = new MeanStack<Vector2>(i);

                Vector2 sum = Vector2.Zero;
                for (int j = 1; j <= i; j++)
                {
                    Vector2 value = new Vector2(j, j * 2);
                    sum += value;
                    mf.Push(value);
                }

                Vector2 mean = sum / i;
                Assert.AreEqual(mean.X, mf.Mean().X);
                Assert.AreEqual(mean.Y, mf.Mean().Y);
            }
        }
    }
}