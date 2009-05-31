using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace NetGore.Collections.Tests
{
    [TestFixture]
    public class SafeEnumeratorTests
    {
        /// <summary>
        /// Tests simple enumeration over an unmodified collection.
        /// </summary>
        /// <param name="source">Souce to test.</param>
        static void EnumerateTest(IEnumerable<int> source)
        {
            var se = new SafeEnumerator<int>(source);

            for (int i = 0; i < 3; i++)
            {
                int sum = 0;
                foreach (int item in se)
                {
                    sum += item;
                }
                Assert.AreEqual(source.Sum(), sum);
            }
        }

        /// <summary>
        /// Tests enumeration over a collection being modified between enumerations.
        /// </summary>
        /// <param name="source">Source to test.</param>
        static void ModifyEnumerateTest(ICollection<int> source)
        {
            var se = new SafeEnumerator<int>(source);

            for (int i = 0; i < 3; i++)
            {
                foreach (int item in Enumerable.Range(i * 10, i * 10 + 5))
                {
                    source.Add(item);
                }

                int sum = 0;
                foreach (int item in se)
                {
                    sum += item;
                }
                Assert.AreEqual(source.Sum(), sum);
            }
        }

        /// <summary>
        /// Tests enumeration over a collection, clearing it, then enumerating over it again.
        /// </summary>
        /// <param name="source">Source to test.</param>
        static void ClearEnumerateTest(ICollection<int> source)
        {
            var se = new SafeEnumerator<int>(source);

            for (int i = 0; i < 3; i++)
            {
                int sum = 0;
                foreach (int item in se)
                {
                    sum += item;
                }
                Assert.AreEqual(source.Sum(), sum);
                source.Clear();
            }
        }

        /// <summary>
        /// Tests enumerating over an IEnumerable that is already being enumerated over.
        /// </summary>
        /// <param name="source">Source to test.</param>
        static void DoubleEnumerateTest(IEnumerable<int> source)
        {
            var se = new SafeEnumerator<int>(source);

            int sum = 0;
#pragma warning disable 168
            foreach (int i1 in se)
            {
                foreach (int i2 in se)
                {
                    sum++;
                }
            }
#pragma warning restore 168
            Assert.AreEqual(se.Count() * se.Count(), sum);
        }

        [Test]
        public void DoubleEnumerateCollectionTest()
        {
            DoubleEnumerateTest(Enumerable.Range(0, 100).ToList());
        }

        [Test]
        public void DoubleEnumerateDArrayTest()
        {
            DoubleEnumerateTest(new DArray<int>(Enumerable.Range(0, 100), false));
        }

        [Test]
        public void DoubleEnumerateEnumerableTest()
        {
            DoubleEnumerateTest(Enumerable.Range(0, 100));
        }

        [Test]
        public void DoubleEnumerateReadonlyCollectionTest()
        {
            DoubleEnumerateTest(Enumerable.Range(0, 100).ToList().AsReadOnly());
        }

        [Test]
        public void DoubleEnumerateTSListTest()
        {
            DoubleEnumerateTest(new TSList<int>(Enumerable.Range(0, 100)));
        }

        [Test]
        public void EnumerateClearCollectionTest()
        {
            ClearEnumerateTest(Enumerable.Range(0, 100).ToList());
        }

        [Test]
        public void EnumerateClearDArrayTest()
        {
            ClearEnumerateTest(new DArray<int>(Enumerable.Range(0, 100), false));
        }

        [Test]
        public void EnumerateClearTSListTest()
        {
            ClearEnumerateTest(new TSList<int>(Enumerable.Range(0, 100)));
        }

        [Test]
        public void EnumerateCollectionTest()
        {
            EnumerateTest(Enumerable.Range(0, 100).ToList());
        }

        [Test]
        public void EnumerateDArrayTest()
        {
            EnumerateTest(new DArray<int>(Enumerable.Range(0, 100), false));
        }

        [Test]
        public void EnumerateEnumerableTest()
        {
            EnumerateTest(Enumerable.Range(0, 100));
        }

        [Test]
        public void EnumerateModifiedCollectionTest()
        {
            ModifyEnumerateTest(Enumerable.Range(0, 100).ToList());
        }

        [Test]
        public void EnumerateModifiedDArrayTest()
        {
            ModifyEnumerateTest(new DArray<int>(Enumerable.Range(0, 100), false));
        }

        [Test]
        public void EnumerateModifiedTSListTest()
        {
            ModifyEnumerateTest(new TSList<int>(Enumerable.Range(0, 100)));
        }

        [Test]
        public void EnumerateReadonlyCollectionTest()
        {
            EnumerateTest(Enumerable.Range(0, 100).ToList().AsReadOnly());
        }

        [Test]
        public void EnumerateTSListTest()
        {
            EnumerateTest(new TSList<int>(Enumerable.Range(0, 100)));
        }
    }
}