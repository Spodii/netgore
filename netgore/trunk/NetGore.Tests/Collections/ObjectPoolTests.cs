using System.Collections.Generic;
using System.Linq;
using NetGore;
using NUnit.Framework;

// TODO: More ObjectPool tests

namespace NetGore.Collections.Tests
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

        public void SetPoolData(ObjectPool<TestPoolItem> objectPool, PoolData<TestPoolItem> poolData)
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
    }
}