using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "TrackFree")]
        static void AddTestSub(bool trackFree)
        {
            var o1 = new object();
            var o2 = new object();
            var d = new DArray<object>(trackFree) { o1, o2 };

            Assert.IsTrue(d.Contains(o1), "TrackFree = " + trackFree);
            Assert.IsTrue(d.Contains(o2), "TrackFree = " + trackFree);
            Assert.AreEqual(2, d.Length, "TrackFree = " + trackFree);
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "TrackFree")]
        static void AddValueTypeTestSub(bool trackFree)
        {
            const int o1 = new int();
            const int o2 = new int();
            var d = new DArray<object>(trackFree) { o1, o2 };

            Assert.IsTrue(d.Contains(o1), "TrackFree = " + trackFree);
            Assert.IsTrue(d.Contains(o2), "TrackFree = " + trackFree);
            Assert.AreEqual(2, d.Length, "TrackFree = " + trackFree);
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly",
            MessageId = "ArgumentOutOfRangeException")]
        static void CanGetAndIndexRangeTestSub(bool trackFree)
        {
            var d = new DArray<object>(trackFree);
            d[0] = new object();

            var o = d[0];

            Assert.IsFalse(d.CanGet(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => o = d[-1],
                "Failed to generate ArgumentOutOfRangeException for d[-1].");

            Assert.IsFalse(d.CanGet(1));
            Assert.Throws<ArgumentOutOfRangeException>(() => o = d[-1], "Failed to generate ArgumentOutOfRangeException for d[1].");
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IndexOutOfRangeException"
            )]
        static void ClearTestSub(bool trackFree)
        {
            const int size = 50;

            var d = new DArray<object>(trackFree);
            for (var i = 0; i < size; i++)
            {
                d[i] = new object();
            }

            d.Clear();

            Assert.AreEqual(0, d.Length);
            Assert.AreEqual(0, d.Count);

            object o;
            Assert.Throws<ArgumentOutOfRangeException>(() => o = d[0], "Failed to generate IndexOutOfRangeException for d[0].");
            Assert.IsFalse(d.CanGet(0));
        }

        static void ContainsTestSub(bool trackFree)
        {
            const int size = 10;

            var d = new DArray<object>(trackFree);
            for (var i = 0; i < size; i++)
            {
                d[i] = new object();
            }

            for (var i = 0; i < size; i++)
            {
                Assert.IsTrue(d.Contains(d[i]));
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "TrackFree")]
        static void CountTestSub(bool trackFree)
        {
            var expectedCount = 0;

            var d = new DArray<object>(trackFree);

            for (var i = 0; i < 1000; i++)
            {
                if ((i % 3) == 0)
                {
                    d[i] = new object();
                    expectedCount++;
                    Assert.AreEqual(expectedCount, d.Count, "TrackFree = " + trackFree);
                }
            }

            for (var i = 0; i < d.Length; i++)
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

            for (var i = 0; i < 50; i++)
            {
                d.Add(new object());
                expectedCount++;
                Assert.AreEqual(expectedCount, d.Count, "TrackFree = " + trackFree);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "objs")]
        static void EnumerateTestSub(bool trackFree)
        {
            const int size = 100;

            var objs = new object[size];
            for (var i = 0; i < size; i++)
            {
                if ((i % 2) == 0)
                    objs[i] = new object();
            }

            var d = new DArray<object>(size * 2, trackFree);
            for (var i = 0; i < size; i++)
            {
                if (objs[i] != null)
                    d[i] = objs[i];
            }

            foreach (var obj in d)
            {
                var i = d.IndexOf(obj);

                Assert.IsNotNull(obj);
                Assert.AreSame(objs[i], obj);
                Assert.AreSame(objs[i], d[i]);

                objs[i] = null;
            }

            var remainingObjs = objs.Count(obj => obj != null);
            Assert.AreEqual(0, remainingObjs,
                "One or more items failed to be enumerated since all enumerated " + "items should have been removed from objs[].");
        }

        static void EnumerateValueTypeTestSub(bool trackFree)
        {
            const int size = 100;

            var objs = new int[size];
            for (var i = 0; i < size; i++)
            {
                objs[i] = i * 4;
            }

            var d = new DArray<int>(size * 2, trackFree);
            for (var i = 0; i < size; i++)
            {
                d[i] = objs[i];
            }

            foreach (var obj in d)
            {
                var i = d.IndexOf(obj);

                Assert.AreEqual(objs[i], obj);
                Assert.AreEqual(objs[i], d[i]);

                objs[i] = -1;
            }

            var remainingObjs = objs.Where(obj => obj != -1).Count();
            Assert.AreEqual(0, remainingObjs,
                "One or more items failed to be enumerated since all enumerated " + "items should be equal to -1.");
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly",
            MessageId = "InvalidOperationException")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "obj")]
        static void EnumerateVersionTestSub(bool trackFree)
        {
            var d = new DArray<object>(50, trackFree) { new object(), new object() };

            try
            {
                foreach (var obj in d)
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
                foreach (var obj in d)
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
                foreach (var obj in d)
                {
                    d[0] = new object();
                }
                Assert.Fail("Failed to generate InvalidOperationException.");
            }
            catch (InvalidOperationException)
            {
            }
        }

        static void GetSetTestSub(bool trackFree)
        {
            const int size = 1000;

            var objs = new object[size];
            for (var i = 0; i < size; i++)
            {
                objs[i] = new object();
            }

            var d = new DArray<object>(trackFree);

            for (var i = 0; i < 1000; i++)
            {
                d[i] = objs[i];
            }

            for (var i = 0; i < 1000; i++)
            {
                Assert.AreSame(objs[i], d[i]);
            }
        }

        static void IndexOfTestSub(bool trackFree)
        {
            const int size = 50;

            var d = new DArray<object>(trackFree);
            for (var i = 0; i < size; i++)
            {
                d[i] = new object();
            }

            for (var i = 0; i < size; i++)
            {
                Assert.AreEqual(i, d.IndexOf(d[i]));
            }
        }

        static void IndexOfValueTypeTestSub(bool trackFree)
        {
            const int size = 50;

            var d = new DArray<int>(trackFree);
            for (var i = 0; i < size; i++)
            {
                d[i] = i * 4;
            }

            for (var i = 0; i < size; i++)
            {
                Assert.AreEqual(i, d.IndexOf(d[i]));
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "TrackFree")]
        static void LengthTestSub(bool trackFree)
        {
            var d = new DArray<object>(trackFree);
            for (var i = 0; i < 1000; i++)
            {
                Assert.AreEqual(i, d.Length, "TrackFree = " + trackFree);
                d[i] = new object();
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "TrackFree")]
        static void RemoveInsertTestSub(bool trackFree)
        {
            var d = new DArray<object>(trackFree);

            for (var i = 0; i < 10; i++)
            {
                d[i] = new object();
            }

            d.RemoveAt(0);
            d.RemoveAt(5);
            d.RemoveAt(6);
            d.RemoveAt(9);

            var usedIndices = new List<int>();
            for (var i = 0; i < 7; i++)
            {
                usedIndices.Add(d.Insert(new object()));
            }

            var expected = new int[] { 0, 5, 6, 9, 10, 11, 12 };

            Assert.AreEqual(usedIndices.Count(), expected.Length);

            foreach (var i in usedIndices)
            {
                Assert.IsTrue(expected.Contains(i), "TrackFree = " + trackFree);
            }
        }

        static void RemoveTestSub(bool trackFree)
        {
            var d = new DArray<object>(trackFree);

            for (var i = 0; i < 10; i++)
            {
                d[i] = new object();
            }

            var o = d[5];
            Assert.IsTrue(d.Remove(o));
            Assert.IsFalse(d.Contains(o));
        }

        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "TrackFree")]
        static void TrimTestSub(bool trackFree)
        {
            const int size = 1000;

            var objs = new object[size];
            for (var i = 0; i < size / 2; i++)
            {
                if ((i % 3) == 0)
                    objs[i] = i.ToString();
            }

            var d = new DArray<object>(size, trackFree);
            for (var i = 0; i < size; i++)
            {
                if (objs[i] != null)
                    d[i] = objs[i];
            }

            d.Trim();

            // Make sure our data has not changed
            for (var i = 0; i < size; i++)
            {
                if (objs[i] != null)
                    Assert.AreSame(objs[i], d[i], "TrackFree = " + trackFree);
            }

            // Make sure the null slots are still null
            for (var i = 0; i < d.Length; i++)
            {
                Assert.AreSame(objs[i], d[i], "TrackFree = " + trackFree);
            }

            // Make sure that inserts first fill up the gaps, THEN expand
            var startLen = d.Length;
            var gaps = startLen - d.Count;
            for (var i = 0; i < gaps; i++)
            {
                d.Insert(new object());
            }

            Assert.AreEqual(startLen, d.Length, "TrackFree = " + trackFree);

            // Make sure we start expanding now
            for (var i = 0; i < 10; i++)
            {
                var before = d.Length;
                d.Insert(new object());
                Assert.AreEqual(before + 1, d.Length, "TrackFree = " + trackFree);
            }
        }

        #region Unit tests

        [Test]
        public void AddTest()
        {
            AddTestSub(true);
            AddTestSub(false);
        }

        [Test]
        public void AddValueTypeTest()
        {
            AddValueTypeTestSub(true);
            AddValueTypeTestSub(false);
        }

        [Test]
        public void CanGetAndIndexRangeTest()
        {
            CanGetAndIndexRangeTestSub(true);
            CanGetAndIndexRangeTestSub(false);
        }

        [Test]
        public void ClearTest()
        {
            ClearTestSub(true);
            ClearTestSub(false);
        }

        [Test]
        public void ContainsTest()
        {
            ContainsTestSub(true);
            ContainsTestSub(false);
        }

        [Test]
        public void CountTest()
        {
            CountTestSub(true);
            CountTestSub(false);
        }

        [Test]
        public void EnumerateTest()
        {
            EnumerateTestSub(true);
            EnumerateTestSub(false);
        }

        [Test]
        public void EnumerateValueTypeTest()
        {
            EnumerateValueTypeTestSub(true);
            EnumerateValueTypeTestSub(false);
        }

        [Test]
        public void EnumerateVersionTest()
        {
            EnumerateVersionTestSub(true);
            EnumerateVersionTestSub(false);
        }

        [Test]
        public void GetSetTest()
        {
            GetSetTestSub(true);
            GetSetTestSub(false);
        }

        [Test]
        public void IndexOfTest()
        {
            IndexOfTestSub(true);
            IndexOfTestSub(false);
        }

        [Test]
        public void IndexOfValueTypeTest()
        {
            IndexOfValueTypeTestSub(true);
            IndexOfValueTypeTestSub(false);
        }

        [Test]
        public void LengthTest()
        {
            LengthTestSub(true);
            LengthTestSub(false);
        }

        [Test]
        public void RemoveInsertTest()
        {
            RemoveInsertTestSub(true);
            RemoveInsertTestSub(false);
        }

        [Test]
        public void RemoveTest()
        {
            RemoveTestSub(true);
            RemoveTestSub(false);
        }

        [Test]
        public void TrimTest()
        {
            TrimTestSub(true);
            TrimTestSub(false);
        }

        #endregion
    }
}