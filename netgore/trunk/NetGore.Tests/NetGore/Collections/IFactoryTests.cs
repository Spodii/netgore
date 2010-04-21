using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NetGore.Collections;
using NUnit.Framework;

namespace NetGore.Tests.NetGore.Collections
{
    [TestFixture]
    public class IFactoryTests
    {
        static IEnumerable<ICache<TKey, TValue>> CreateFactories<TKey, TValue>(Func<TKey, TValue> valueCreator,
                                                                               IEqualityComparer<TKey> equalityComparer = null)
            where TValue : class
        {
            yield return new HashCache<TKey, TValue>(valueCreator, equalityComparer);
            yield return new ThreadSafeHashCache<TKey, TValue>(valueCreator, equalityComparer);
        }

        #region Unit tests

        [Test]
        public void CacheIsBeingUsedTest()
        {
            var createCount = 0;

            Func<int, string> creator = delegate(int key)
            {
                // ReSharper disable AccessToModifiedClosure
                createCount++;
                // ReSharper restore AccessToModifiedClosure

                return key.ToString();
            };

            var factories = CreateFactories(creator).ToArray();

            foreach (var f in factories)
            {
                createCount = 0;

                Assert.AreEqual(0, createCount);

                Assert.AreEqual("1", f[1]);
                Assert.AreEqual(1, createCount);

                Assert.AreEqual("1", f[1]);
                Assert.AreEqual(1, createCount);

                Assert.AreEqual("2", f[2]);
                Assert.AreEqual(2, createCount);

                Assert.AreEqual("2", f[2]);
                Assert.AreEqual(2, createCount);

                Assert.AreEqual("1", f[1]);
                Assert.AreEqual(2, createCount);
            }
        }

        [Test]
        public void ClearTest()
        {
            var createCount = 0;

            Func<int, string> creator = delegate(int key)
            {
                // ReSharper disable AccessToModifiedClosure
                createCount++;
                // ReSharper restore AccessToModifiedClosure

                return key.ToString();
            };

            var factories = CreateFactories(creator).ToArray();

            foreach (var f in factories)
            {
                createCount = 0;

                Assert.AreEqual(0, createCount);

                Assert.AreEqual("1", f[1]);
                Assert.AreEqual(1, createCount);

                Assert.AreEqual("1", f[1]);
                Assert.AreEqual(1, createCount);

                Assert.AreEqual("2", f[2]);
                Assert.AreEqual(2, createCount);

                Assert.AreEqual("2", f[2]);
                Assert.AreEqual(2, createCount);

                Assert.AreEqual("1", f[1]);
                Assert.AreEqual(2, createCount);

                f.Clear();
                createCount = 0;

                Assert.AreEqual(0, createCount);

                Assert.AreEqual("1", f[1]);
                Assert.AreEqual(1, createCount);

                Assert.AreEqual("1", f[1]);
                Assert.AreEqual(1, createCount);

                Assert.AreEqual("2", f[2]);
                Assert.AreEqual(2, createCount);

                Assert.AreEqual("2", f[2]);
                Assert.AreEqual(2, createCount);

                Assert.AreEqual("1", f[1]);
                Assert.AreEqual(2, createCount);
            }
        }

        [Test]
        public void ContainsKeyTest()
        {
            var factories = CreateFactories<int, string>(x => x.ToString()).ToArray();

            foreach (var f in factories)
            {
                Assert.IsFalse(f.ContainsKey(1));
                Assert.IsFalse(f.ContainsKey(1));

                Assert.AreEqual("1", f[1]);
                Assert.IsTrue(f.ContainsKey(1));
                Assert.IsFalse(f.ContainsKey(2));

                Assert.AreEqual("2", f[2]);
                Assert.IsTrue(f.ContainsKey(2));

                Assert.IsFalse(f.ContainsKey(3));
                Assert.IsFalse(f.ContainsKey(3));
                Assert.IsFalse(f.ContainsKey(4));
                Assert.IsFalse(f.ContainsKey(4));
            }
        }

        [Test]
        public void CreateTest()
        {
            CreateFactories<int, string>(x => x.ToString()).ToArray();
        }

        [Test]
        public void GetCachedValuesTest()
        {
            var factories = CreateFactories<int, string>(x => x.ToString()).ToArray();

            foreach (var f in factories)
            {
                var a = f.GetCachedValues();
                Assert.AreEqual(0, a.Count());

                Assert.AreEqual("1", f[1]);

                var b = f.GetCachedValues();
                Assert.AreEqual(1, b.Count());
                Assert.AreNotSame(a, b);

                Assert.AreEqual("2", f[2]);
                Assert.AreEqual("3", f[3]);
                Assert.AreEqual("4", f[4]);
                Assert.AreEqual("5", f[5]);

                var c = f.GetCachedValues();
                Assert.AreEqual(0, a.Count());
                Assert.AreEqual(1, b.Count());
                Assert.AreEqual(5, c.Count());

                Assert.AreNotSame(a, b);
                Assert.AreNotSame(a, c);
                Assert.AreNotSame(b, c);
            }
        }

        [Test]
        public void GetKeyTest()
        {
            var factories = CreateFactories<int, string>(x => x.ToString()).ToArray();

            foreach (var f in factories)
            {
                Assert.AreEqual("1", f[1]);
                Assert.AreEqual("100", f[100]);
            }
        }

        [Test]
        public void PrepareKeysTest()
        {
            var factories = CreateFactories<int, string>(x => x.ToString()).ToArray();

            foreach (var f in factories)
            {
                Assert.IsFalse(f.ContainsKey(1));
                Assert.IsFalse(f.ContainsKey(2));
                Assert.IsFalse(f.ContainsKey(3));
                Assert.IsFalse(f.ContainsKey(4));
                Assert.IsFalse(f.ContainsKey(5));
                Assert.IsFalse(f.ContainsKey(6));

                Assert.AreEqual("1", f[1]);
                Assert.IsTrue(f.ContainsKey(1));

                f.PrepareKeys(Enumerable.Range(1, 6));

                Assert.IsTrue(f.ContainsKey(1));
                Assert.IsTrue(f.ContainsKey(2));
                Assert.IsTrue(f.ContainsKey(3));
                Assert.IsTrue(f.ContainsKey(4));
                Assert.IsTrue(f.ContainsKey(5));
                Assert.IsTrue(f.ContainsKey(6));

                f.PrepareKeys(Enumerable.Range(1, 6));

                Assert.IsTrue(f.ContainsKey(1));
                Assert.IsTrue(f.ContainsKey(2));
                Assert.IsTrue(f.ContainsKey(3));
                Assert.IsTrue(f.ContainsKey(4));
                Assert.IsTrue(f.ContainsKey(5));
                Assert.IsTrue(f.ContainsKey(6));
            }
        }

        [Test]
        public void ThreadSafeGetKeyTest()
        {
            const int numKeysAccessed = 100;

            // ReSharper disable AccessToModifiedClosure
            var createCount = 0;

            Func<int, string> creator = delegate(int key)
            {
                createCount++;

                Thread.Sleep(5);
                return key.ToString();
            };

            var factories = CreateFactories(creator).Where(x => x.IsThreadSafe).ToArray();

            foreach (var f in factories)
            {
                createCount = 0;

                // Some threads increment and some decrement so there will definitely be overlap
                ThreadStart threadWorkloadA = delegate
                {
                    for (var j = 0; j < numKeysAccessed; j++)
                    {
                        Assert.AreEqual(j.ToString(), f[j]);
                    }
                };

                ThreadStart threadWorkloadB = delegate
                {
                    for (var j = numKeysAccessed - 1; j >= 0; j--)
                    {
                        Assert.AreEqual(j.ToString(), f[j]);
                    }
                };

                var threads = new List<Thread>();

                // Create the threads to perform the work
                for (var i = 0; i < 5; i++)
                {
                    var t = new Thread(threadWorkloadA);
                    threads.Add(t);
                    t.Start();
                }

                for (var i = 0; i < 5; i++)
                {
                    var t = new Thread(threadWorkloadB);
                    threads.Add(t);
                    t.Start();
                }

                // Wait for all threads to finish
                foreach (var t in threads)
                {
                    t.Join();
                }

                // Make sure the objects were only created the needed number of times
                Assert.AreEqual(numKeysAccessed, createCount);
            }
            // ReSharper restore AccessToModifiedClosure
        }

        #endregion
    }
}