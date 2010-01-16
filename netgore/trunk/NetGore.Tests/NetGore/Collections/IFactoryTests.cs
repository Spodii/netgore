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
        [Test]
        public void CacheIsBeingUsedTest()
        {
            int createCount = 0;

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
            int createCount = 0;

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

        static IEnumerable<ICache<TKey, TValue>> CreateFactories<TKey, TValue>(Func<TKey, TValue> valueCreator)
            where TValue : class
        {
            return CreateFactories(valueCreator, null);
        }

        static IEnumerable<ICache<TKey, TValue>> CreateFactories<TKey, TValue>(Func<TKey, TValue> valueCreator,
                                                                               IEqualityComparer<TKey> equalityComparer)
            where TValue : class
        {
            yield return new HashCache<TKey, TValue>(valueCreator, equalityComparer);
            yield return new ThreadSafeHashCache<TKey, TValue>(valueCreator, equalityComparer);
        }

        [Test]
        public void CreateTest()
        {
            CreateFactories<int, string>(x => x.ToString()).ToArray();
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
        public void ThreadSafeGetKeyTest()
        {
            const int numKeysAccessed = 100;

            // ReSharper disable AccessToModifiedClosure
            int createCount = 0;

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
                                                  for (int j = 0; j < numKeysAccessed; j++)
                                                  {
                                                      Assert.AreEqual(j.ToString(), f[j]);
                                                  }
                                              };

                ThreadStart threadWorkloadB = delegate
                                              {
                                                  for (int j = numKeysAccessed - 1; j >= 0; j--)
                                                  {
                                                      Assert.AreEqual(j.ToString(), f[j]);
                                                  }
                                              };

                List<Thread> threads = new List<Thread>();

                // Create the threads to perform the work
                for (int i = 0; i < 5; i++)
                {
                    Thread t = new Thread(threadWorkloadA);
                    threads.Add(t);
                    t.Start();
                }

                for (int i = 0; i < 5; i++)
                {
                    Thread t = new Thread(threadWorkloadB);
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
    }
}