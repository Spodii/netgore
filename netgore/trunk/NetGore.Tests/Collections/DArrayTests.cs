using System;
using System.Collections.Generic;
using System.Linq;
using NetGore;
using NetGore.Collections;
using NUnit.Framework;

// ReSharper disable RedundantAssignment
#pragma warning disable 219
#pragma warning disable 168

namespace NetGore.Tests.Collections
{
    [TestFixture]
    public class DArrayTests
    {
        [Test]
        public void AddTest()
        {
            AddTestSub(true);
            AddTestSub(false);
        }

        static void AddTestSub(bool trackFree)
        {
            object o1 = new object();
            object o2 = new object();
            var d = new DArray<object>(trackFree) { o1, o2 };

            Assert.IsTrue(d.Contains(o1), "TrackFree = " + trackFree);
            Assert.IsTrue(d.Contains(o2), "TrackFree = " + trackFree);
            Assert.AreEqual(2, d.Length, "TrackFree = " + trackFree);
        }

        [Test]
        public void AddValueTypeTest()
        {
            AddValueTypeTestSub(true);
            AddValueTypeTestSub(false);
        }

        static void AddValueTypeTestSub(bool trackFree)
        {
            const int o1 = new int();
            const int o2 = new int();
            var d = new DArray<object>(trackFree) { o1, o2 };

            Assert.IsTrue(d.Contains(o1), "TrackFree = " + trackFree);
            Assert.IsTrue(d.Contains(o2), "TrackFree = " + trackFree);
            Assert.AreEqual(2, d.Length, "TrackFree = " + trackFree);
        }

        [Test]
        public void CanGetAndIndexRangeTest()
        {
            CanGetAndIndexRangeTestSub(true);
            CanGetAndIndexRangeTestSub(false);
        }

        static void CanGetAndIndexRangeTestSub(bool trackFree)
        {
            var d = new DArray<object>(trackFree);
            d[0] = new object();

            object o = d[0];

            try
            {
                o = d[-1];
                Assert.Fail("Failed to generate IndexOutOfRangeException for d[-1].");
            }
            catch (IndexOutOfRangeException)
            {
                Assert.IsFalse(d.CanGet(-1));
            }

            try
            {
                o = d[1];
                Assert.Fail("Failed to generate IndexOutOfRangeException for d[1].");
            }
            catch (IndexOutOfRangeException)
            {
                Assert.IsFalse(d.CanGet(1));
            }
        }

        [Test]
        public void ClearTest()
        {
            ClearTestSub(true);
            ClearTestSub(false);
        }

        static void ClearTestSub(bool trackFree)
        {
            const int size = 50;

            var d = new DArray<object>(trackFree);
            for (int i = 0; i < size; i++)
            {
                d[i] = new object();
            }

            d.Clear();

            Assert.AreEqual(0, d.Length);
            Assert.AreEqual(0, d.Count);

            try
            {
                object o = d[0];
                Assert.Fail("Failed to generate IndexOutOfRangeException for d[-1].");
            }
            catch (IndexOutOfRangeException)
            {
                Assert.IsFalse(d.CanGet(0));
            }
        }

        [Test]
        public void ContainsTest()
        {
            ContainsTestSub(true);
            ContainsTestSub(false);
        }

        static void ContainsTestSub(bool trackFree)
        {
            const int size = 10;

            var d = new DArray<object>(trackFree);
            for (int i = 0; i < size; i++)
            {
                d[i] = new object();
            }

            for (int i = 0; i < size; i++)
            {
                Assert.IsTrue(d.Contains(d[i]));
            }
        }

        [Test]
        public void CountTest()
        {
            CountTestSub(true);
            CountTestSub(false);
        }

        static void CountTestSub(bool trackFree)
        {
            int expectedCount = 0;

            var d = new DArray<object>(trackFree);

            for (int i = 0; i < 1000; i++)
            {
                if ((i % 3) == 0)
                {
                    d[i] = new object();
                    expectedCount++;
                    Assert.AreEqual(expectedCount, d.Count, "TrackFree = " + trackFree);
                }
            }

            for (int i = 0; i < d.Length; i++)
            {
                if ((i % 2) == 0 && d[i] != null)
                {
                    Assert.IsNotNull(d[i]);
                    d.RemoveAt(i);
                    expectedCount--;
                    Assert.IsNull(d[i]);
                    Assert.AreEqual(expectedCount, d.Count, "TrackFree = " + trackFree);
                }
            }

            for (int i = 0; i < 50; i++)
            {
                d.Add(new object());
                expectedCount++;
                Assert.AreEqual(expectedCount, d.Count, "TrackFree = " + trackFree);
            }
        }

        [Test]
        public void EnumerateTest()
        {
            EnumerateTestSub(true);
            EnumerateTestSub(false);
        }

        static void EnumerateTestSub(bool trackFree)
        {
            const int size = 100;

            var objs = new object[size];
            for (int i = 0; i < size; i++)
            {
                if ((i % 2) == 0)
                    objs[i] = new object();
            }

            var d = new DArray<object>(size * 2, trackFree);
            for (int i = 0; i < size; i++)
            {
                if (objs[i] != null)
                    d[i] = objs[i];
            }

            foreach (object obj in d)
            {
                int i = d.IndexOf(obj);

                Assert.IsNotNull(obj);
                Assert.AreSame(objs[i], obj);
                Assert.AreSame(objs[i], d[i]);

                objs[i] = null;
            }

            int remainingObjs = objs.Where(obj => obj != null).Count();
            Assert.AreEqual(0, remainingObjs,
                            "One or more items failed to be enumerated since all enumerated " +
                            "items should have been removed from objs[].");
        }

        [Test]
        public void EnumerateValueTypeTest()
        {
            EnumerateValueTypeTestSub(true);
            EnumerateValueTypeTestSub(false);
        }

        static void EnumerateValueTypeTestSub(bool trackFree)
        {
            const int size = 100;

            var objs = new int[size];
            for (int i = 0; i < size; i++)
            {
                objs[i] = i * 4;
            }

            var d = new DArray<int>(size * 2, trackFree);
            for (int i = 0; i < size; i++)
            {
                d[i] = objs[i];
            }

            foreach (int obj in d)
            {
                int i = d.IndexOf(obj);

                Assert.AreEqual(objs[i], obj);
                Assert.AreEqual(objs[i], d[i]);

                objs[i] = -1;
            }

            int remainingObjs = objs.Where(obj => obj != -1).Count();
            Assert.AreEqual(0, remainingObjs,
                            "One or more items failed to be enumerated since all enumerated " + "items should be equal to -1.");
        }

        [Test]
        public void EnumerateVersionTest()
        {
            EnumerateVersionTestSub(true);
            EnumerateVersionTestSub(false);
        }

        static void EnumerateVersionTestSub(bool trackFree)
        {
            var d = new DArray<object>(50, trackFree) { new object(), new object() };

            try
            {
                foreach (object obj in d)
                {
                    d[10] = new object();
                }
                Assert.Fail("Failed to generate InvalidOperationException.");
            }
            catch (InvalidOperationException)
            {
            }

            try
            {
                foreach (object obj in d)
                {
                    d.RemoveAt(0);
                }
                Assert.Fail("Failed to generate InvalidOperationException.");
            }
            catch (InvalidOperationException)
            {
            }

            try
            {
                foreach (object obj in d)
                {
                    d[0] = new object();
                }
                Assert.Fail("Failed to generate InvalidOperationException.");
            }
            catch (InvalidOperationException)
            {
            }
        }

        [Test]
        public void GetSetTest()
        {
            GetSetTestSub(true);
            GetSetTestSub(false);
        }

        static void GetSetTestSub(bool trackFree)
        {
            const int size = 1000;

            var objs = new object[size];
            for (int i = 0; i < size; i++)
            {
                objs[i] = new object();
            }

            var d = new DArray<object>(trackFree);

            for (int i = 0; i < 1000; i++)
            {
                d[i] = objs[i];
            }

            for (int i = 0; i < 1000; i++)
            {
                Assert.AreSame(objs[i], d[i]);
            }
        }

        [Test]
        public void IndexOfTest()
        {
            IndexOfTestSub(true);
            IndexOfTestSub(false);
        }

        static void IndexOfTestSub(bool trackFree)
        {
            const int size = 50;

            var d = new DArray<object>(trackFree);
            for (int i = 0; i < size; i++)
            {
                d[i] = new object();
            }

            for (int i = 0; i < size; i++)
            {
                Assert.AreEqual(i, d.IndexOf(d[i]));
            }
        }

        [Test]
        public void IndexOfValueTypeTest()
        {
            IndexOfValueTypeTestSub(true);
            IndexOfValueTypeTestSub(false);
        }

        static void IndexOfValueTypeTestSub(bool trackFree)
        {
            const int size = 50;

            var d = new DArray<int>(trackFree);
            for (int i = 0; i < size; i++)
            {
                d[i] = i * 4;
            }

            for (int i = 0; i < size; i++)
            {
                Assert.AreEqual(i, d.IndexOf(d[i]));
            }
        }

        [Test]
        public void LengthTest()
        {
            LengthTestSub(true);
            LengthTestSub(false);
        }

        static void LengthTestSub(bool trackFree)
        {
            var d = new DArray<object>(trackFree);
            for (int i = 0; i < 1000; i++)
            {
                Assert.AreEqual(i, d.Length, "TrackFree = " + trackFree);
                d[i] = new object();
            }
        }

        [Test]
        public void RemoveInsertTest()
        {
            RemoveInsertTestSub(true);
            RemoveInsertTestSub(false);
        }

        static void RemoveInsertTestSub(bool trackFree)
        {
            var d = new DArray<object>(trackFree);

            for (int i = 0; i < 10; i++)
            {
                d[i] = new object();
            }

            d.RemoveAt(0);
            d.RemoveAt(5);
            d.RemoveAt(6);
            d.RemoveAt(9);

            var usedIndices = new List<int>();
            for (int i = 0; i < 7; i++)
            {
                usedIndices.Add(d.Insert(new object()));
            }

            var expected = new int[] { 0, 5, 6, 9, 10, 11, 12 };

            Assert.AreEqual(usedIndices.Count(), expected.Length);

            foreach (int i in usedIndices)
            {
                Assert.IsTrue(expected.Contains(i), "TrackFree = " + trackFree);
            }
        }

        [Test]
        public void RemoveTest()
        {
            RemoveTestSub(true);
            RemoveTestSub(false);
        }

        static void RemoveTestSub(bool trackFree)
        {
            var d = new DArray<object>(trackFree);

            for (int i = 0; i < 10; i++)
            {
                d[i] = new object();
            }

            object o = d[5];
            Assert.IsTrue(d.Remove(o));
            Assert.IsFalse(d.Contains(o));
        }

        [Test]
        public void TrimTest()
        {
            TrimTestSub(true);
            TrimTestSub(false);
        }

        static void TrimTestSub(bool trackFree)
        {
            const int size = 1000;

            var objs = new object[size];
            for (int i = 0; i < size / 2; i++)
            {
                if ((i % 3) == 0)
                    objs[i] = i.ToString();
            }

            var d = new DArray<object>(size, trackFree);
            for (int i = 0; i < size; i++)
            {
                if (objs[i] != null)
                    d[i] = objs[i];
            }

            d.Trim();

            // Make sure our data has not changed
            for (int i = 0; i < size; i++)
            {
                if (objs[i] != null)
                    Assert.AreSame(objs[i], d[i], "TrackFree = " + trackFree);
            }

            // Make sure the null slots are still null
            for (int i = 0; i < d.Length; i++)
            {
                Assert.AreSame(objs[i], d[i], "TrackFree = " + trackFree);
            }

            // Make sure that inserts first fill up the gaps, THEN expand
            int startLen = d.Length;
            int gaps = startLen - d.Count;
            for (int i = 0; i < gaps; i++)
            {
                d.Insert(new object());
            }

            Assert.AreEqual(startLen, d.Length, "TrackFree = " + trackFree);

            // Make sure we start expanding now
            for (int i = 0; i < 10; i++)
            {
                int before = d.Length;
                d.Insert(new object());
                Assert.AreEqual(before + 1, d.Length, "TrackFree = " + trackFree);
            }
        }
    }
}