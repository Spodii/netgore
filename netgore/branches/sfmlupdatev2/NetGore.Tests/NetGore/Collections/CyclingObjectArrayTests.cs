using System.Linq;
using NetGore.Collections;
using NUnit.Framework;

namespace NetGore.Tests.NetGore.Collections
{
    [TestFixture]
    public class CyclingObjectArrayTests
    {
        #region Unit tests

        [Test]
        public void NextFreeKeyTest()
        {
            var c = CyclingObjectArray.CreateUsingByteKey<string>();

            for (var i = 5; i < 10; i++)
            {
                c[(byte)i] = i.ToString();
            }

            for (var i = 0; i < 20; i++)
            {
                var key = c.Add(i.ToString());
                Assert.IsTrue(key < 5 || key >= 10, "key: " + key);
                Assert.AreEqual(c[key], i.ToString());
            }
        }

        [Test]
        public void NextKeyFreeTest()
        {
            var c = CyclingObjectArray.CreateUsingByteKey<string>();
            var i = c.NextFreeKey();
            var j = c.NextFreeKey();
            Assert.AreNotEqual(i, j);
        }

        [Test]
        public void NextKeyTest()
        {
            var c = CyclingObjectArray.CreateUsingByteKey<string>();
            var i = c.NextFreeKey();
            var j = c.NextFreeKey();
            Assert.AreEqual(i, j - 1);
        }

        [Test]
        public void RotateTest()
        {
            var c = CyclingObjectArray.CreateUsingByteKey<string>();
            int start = c.NextFreeKey();
            var expected = start + 1;
            int curr;

            while ((curr = c.NextFreeKey()) != start)
            {
                Assert.AreEqual(expected, curr);
                expected++;
                if (expected > c.MaxIndex)
                    expected = c.MinIndex;
            }
        }

        [Test]
        public void SkipUsedTest()
        {
            var c = CyclingObjectArray.CreateUsingByteKey<string>();

            for (var i = 0; i < 10; i++)
            {
                c.Add(i.ToString());
            }

            var usedKeys = c.Keys.ToImmutable();

            for (var i = c.MinIndex; i < c.MaxIndex; i++)
            {
                var key = c.NextFreeKey();
                Assert.IsFalse(c.IsSet(key));
                Assert.IsNull(c[key]);
            }

            foreach (var k in usedKeys)
            {
                Assert.IsTrue(c.IsSet(k));
                Assert.IsNotNull(c[k]);
            }
        }

        #endregion
    }
}