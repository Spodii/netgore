using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NetGore.Collections;
using NUnit.Framework;

namespace NetGore.Collections.Tests
{
    [TestFixture]
    public class TSListTests
    {
        /// <summary>
        /// Dummy method that enumerators over the parameter object.
        /// </summary>
        static void DummyEnumerator(object obj)
        {
            IEnumerable e = (IEnumerable)obj;

            foreach (var item in e)
            {
                Thread.Sleep(1);
            }
        }

        [Test]
        public void ThreadSafeAddTest()
        {
            TSList<int> l = new TSList<int>(Enumerable.Range(1, 1000));

            for (int i = 0; i < 10; i++)
            {
                var t = new Thread(DummyEnumerator);
                t.Start(l);
            }

            for (int i = 0; i < 10; i++)
            {
                l.Add(i);
            }
        }

        [Test]
        public void ThreadSafeInsertTest()
        {
            TSList<int> l = new TSList<int>(Enumerable.Range(1, 1000));

            for (int i = 0; i < 10; i++)
            {
                var t = new Thread(DummyEnumerator);
                t.Start(l);
            }

            for (int i = 0; i < 10; i++)
            {
                l.Insert(0, i);
            }
        }

        [Test]
        public void ThreadSafeRemoveTest()
        {
            TSList<int> l = new TSList<int>(Enumerable.Range(1, 1000));

            for (int i = 0; i < 10; i++)
            {
                var t = new Thread(DummyEnumerator);
                t.Start(l);
            }

            for (int i = 0; i < 10; i++)
            {
                l.Remove(i);
            }
        }

        [Test]
        public void ThreadSafeRemoveAtTest()
        {
            TSList<int> l = new TSList<int>(Enumerable.Range(1, 1000));

            for (int i = 0; i < 10; i++)
            {
                var t = new Thread(DummyEnumerator);
                t.Start(l);
            }

            for (int i = 0; i < 10; i++)
            {
                l.RemoveAt(0);
            }
        }

        [Test]
        public void ThreadSafeToArrayTest()
        {
            TSList<int> l = new TSList<int>(Enumerable.Range(1, 1000));

            for (int i = 0; i < 10; i++)
            {
                var t = new Thread(DummyEnumerator);
                t.Start(l);
            }

            for (int i = 0; i < 10; i++)
            {
                var x = l.ToArray();
            }
        }

        [Test]
        public void ThreadSafeTrimExcessTest()
        {
            TSList<int> l = new TSList<int>(Enumerable.Range(1, 1000));

            for (int i = 0; i < 10; i++)
            {
                var t = new Thread(DummyEnumerator);
                t.Start(l);
            }

            for (int i = 0; i < 10; i++)
            {
                l.TrimExcess();
                l.AddRange(Enumerable.Range(1, 100));
            }
        }

        [Test]
        public void ThreadSafeFindTest()
        {
            TSList<int> l = new TSList<int>(Enumerable.Range(1, 1000));

            for (int i = 0; i < 10; i++)
            {
                var t = new Thread(DummyEnumerator);
                t.Start(l);
            }

            for (int i = 0; i < 10; i++)
            {
                l.Find(y => y == 10);
            }
        }

        [Test]
        public void ThreadSafeIndexOfTest()
        {
            TSList<int> l = new TSList<int>(Enumerable.Range(1, 1000));

            for (int i = 0; i < 10; i++)
            {
                var t = new Thread(DummyEnumerator);
                t.Start(l);
            }

            for (int i = 1; i < 10; i++)
            {
                var x = l.IndexOf(i);
                Assert.AreEqual(i, l[x]);
            }
        }

        [Test]
        public void ThreadSafeIndexerTest()
        {
            TSList<int> l = new TSList<int>(Enumerable.Range(1, 1000));

            for (int i = 0; i < 10; i++)
            {
                var t = new Thread(DummyEnumerator);
                t.Start(l);
            }

            for (int i = 0; i < 10; i++)
            {
                var x = l[i];
                l[i] = x + 1;
                Assert.AreEqual(x + 1, l[i]);
            }
        }
    }
}
