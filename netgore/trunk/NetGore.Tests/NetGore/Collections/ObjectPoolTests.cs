using System.Collections.Generic;
using System.Linq;
using NetGore.Collections;
using NUnit.Framework;

namespace NetGore.Tests.Collections
{
    class TestPoolItem : IPoolable<TestPoolItem>
    {
        PoolData<TestPoolItem> _poolData;

        #region IPoolable<TestPoolItem> Members

        public PoolData<TestPoolItem> PoolData
        {
            get { return _poolData; }
        }

        public void Activate()
        {
        }

        public void Deactivate()
        {
        }

        public void SetPoolData(IObjectPool<TestPoolItem> objectPool, PoolData<TestPoolItem> poolData)
        {
            _poolData = poolData;
        }

        #endregion
    }

    class TestPool : ObjectPool<TestPoolItem>
    {
    }

    [TestFixture]
    public class ObjectPoolTests
    {
        [Test]
        public void CountTest()
        {
            TestPool pool = new TestPool();

            var pooled = new Stack<TestPoolItem>();

            for (int i = 1; i < 20; i++)
            {
                pooled.Push(pool.Create());
                Assert.AreEqual(i, pool.Count);
            }

            while (pooled.Count > 0)
            {
                int start = pool.Count;
                pool.Destroy(pooled.Pop());
                Assert.AreEqual(start - 1, pool.Count);
            }

            Assert.AreEqual(0, pool.Count);
        }

        static void CreateSpeedTest<T>(IObjectPool<T> pool) where T : IPoolable<T>, new()
        {
            for (int i = 0; i < 500000; i++)
            {
                pool.Create();
            }
        }

        [Test]
        public void CreateSpeedTestNotThreadSafe()
        {
            CreateSpeedTest(new ObjectPool<TestPoolItem>());
        }

        [Test]
        public void CreateSpeedTestThreadSafe()
        {
            CreateSpeedTest(new ThreadSafeObjectPool<TestPoolItem>());
        }

        static void Destroy2SpeedTest<T>(IObjectPool<T> pool) where T : IPoolable<T>, new()
        {
            Stack<T> stack = new Stack<T>();

            for (int i = 0; i < 500000; i++)
            {
                var c = pool.Create();
                stack.Push(c);
            }

            while (stack.Count > 0)
            {
                pool.Destroy(stack.Pop());
            }
        }

        [Test]
        public void Destroy2SpeedTestNotThreadSafe()
        {
            Destroy2SpeedTest(new ObjectPool<TestPoolItem>());
        }

        [Test]
        public void Destroy2SpeedTestThreadSafe()
        {
            Destroy2SpeedTest(new ThreadSafeObjectPool<TestPoolItem>());
        }

        static void DestroySpeedTest<T>(IObjectPool<T> pool) where T : IPoolable<T>, new()
        {
            for (int i = 0; i < 500000; i++)
            {
                var c = pool.Create();
                pool.Destroy(c);
            }
        }

        [Test]
        public void DestroySpeedTestNotThreadSafe()
        {
            DestroySpeedTest(new ObjectPool<TestPoolItem>());
        }

        [Test]
        public void DestroySpeedTestThreadSafe()
        {
            DestroySpeedTest(new ThreadSafeObjectPool<TestPoolItem>());
        }
    }
}