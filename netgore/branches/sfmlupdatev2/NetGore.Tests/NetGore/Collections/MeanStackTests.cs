using System.Linq;
using NetGore.Collections;
using NUnit.Framework;

namespace NetGore.Tests.Collections
{
    [TestFixture]
    public class MeanStackTests
    {
        /// <summary>
        /// Ending value for the sum of items to test.
        /// </summary>
        const int _itemEnd = 7;

        /// <summary>
        /// Starting value for the sum of items to test.
        /// </summary>
        const int _itemStart = 1;

        /// <summary>
        /// Number of iterations to perform, where each iteration is the size of the MeanStack.
        /// </summary>
        const int _iterations = 70;

        #region Unit tests

        [Test]
        public void ByteMeanTest()
        {
            for (var i = _itemEnd; i < _iterations; i++)
            {
                var mf = new MeanStack<byte>(i);

                var sum = 0;
                for (var j = _itemStart; j <= _itemEnd; j++)
                {
                    sum += j;
                    mf.Push((byte)j);
                }

                var mean = sum / (_itemEnd - _itemStart + 1);
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void DoubleMeanTest()
        {
            for (var i = _itemEnd; i < _iterations; i++)
            {
                var mf = new MeanStack<double>(i);

                double sum = 0;
                for (var j = _itemStart; j <= _itemEnd; j++)
                {
                    sum += j;
                    mf.Push(j);
                }

                var mean = sum / (_itemEnd - _itemStart + 1);
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void FillTest()
        {
            for (var i = 1; i < 20; i++)
            {
                var mf = new MeanStack<int>(i);
                mf.Fill(i);
                Assert.AreEqual(i, mf.Mean());
            }
        }

        [Test]
        public void FloatMeanTest()
        {
            for (var i = _itemEnd; i < _iterations; i++)
            {
                var mf = new MeanStack<float>(i);

                float sum = 0;
                for (var j = _itemStart; j <= _itemEnd; j++)
                {
                    sum += j;
                    mf.Push(j);
                }

                var mean = sum / (_itemEnd - _itemStart + 1);
                var m = mf.Mean();
                Assert.AreEqual(mean, m);
            }
        }

        [Test]
        public void IntMeanTest()
        {
            for (var i = _itemEnd; i < _iterations; i++)
            {
                var mf = new MeanStack<int>(i);

                var sum = 0;
                for (var j = _itemStart; j <= _itemEnd; j++)
                {
                    sum += j;
                    mf.Push(j);
                }

                var mean = sum / (_itemEnd - _itemStart + 1);
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void LongMeanTest()
        {
            for (var i = _itemEnd; i < _iterations; i++)
            {
                var mf = new MeanStack<long>(i);

                long sum = 0;
                for (long j = _itemStart; j <= _itemEnd; j++)
                {
                    sum += j;
                    mf.Push(j);
                }

                var mean = sum / (_itemEnd - _itemStart + 1);
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void SByteMeanTest()
        {
            for (var i = _itemEnd; i < _iterations; i++)
            {
                var mf = new MeanStack<sbyte>(i);

                var sum = 0;
                for (var j = _itemStart; j <= _itemEnd; j++)
                {
                    sum += j;
                    mf.Push((sbyte)j);
                }

                var mean = sum / (_itemEnd - _itemStart + 1);
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void ShortMeanTest()
        {
            for (var i = _itemEnd; i < _iterations; i++)
            {
                var mf = new MeanStack<short>(i);

                var sum = 0;
                for (var j = _itemStart; j <= _itemEnd; j++)
                {
                    sum += j;
                    mf.Push((short)j);
                }

                var mean = sum / (_itemEnd - _itemStart + 1);
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void UIntMeanTest()
        {
            for (var i = _itemEnd; i < _iterations; i++)
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
            for (var i = _itemEnd; i < _iterations; i++)
            {
                var mf = new MeanStack<ulong>(i);

                ulong sum = 0;
                for (ulong j = _itemStart; j <= _itemEnd; j++)
                {
                    sum += j;
                    mf.Push(j);
                }

                var mean = sum / (_itemEnd - _itemStart + 1);
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        [Test]
        public void UShortMeanTest()
        {
            for (var i = _itemEnd; i < _iterations; i++)
            {
                var mf = new MeanStack<ushort>(i);

                var sum = 0;
                for (var j = _itemStart; j <= _itemEnd; j++)
                {
                    sum += j;
                    mf.Push((ushort)j);
                }

                var mean = sum / (_itemEnd - _itemStart + 1);
                Assert.AreEqual(mean, mf.Mean());
            }
        }

        #endregion
    }
}