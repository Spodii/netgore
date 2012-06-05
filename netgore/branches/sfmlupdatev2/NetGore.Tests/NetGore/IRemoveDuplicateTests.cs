using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class IRemoveDuplicateTests
    {
        #region Unit tests

        [Test]
        public void RemoveAllTest()
        {
            var original = new int[] { 1, 1, 1, 1, 1, 1 };
            var l = new List<int>(original);

            Assert.AreEqual(original.Length, l.Count());
            l.RemoveDuplicates((x, y) => x == y);

            Assert.AreEqual(1, l.Count());
        }

        [Test]
        public void RemoveFromEndTest()
        {
            var original = new int[] { 1, 2, 3, 4, 4 };
            var l = new List<int>(original);

            Assert.AreEqual(original.Length, l.Count());
            l.RemoveDuplicates((x, y) => x == y);

            Assert.AreEqual(1, l[0]);
            Assert.AreEqual(2, l[1]);
            Assert.AreEqual(3, l[2]);
            Assert.AreEqual(4, l[3]);
        }

        [Test]
        public void RemoveFromMiddleTest()
        {
            var original = new int[] { 1, 2, 3, 3, 4 };
            var l = new List<int>(original);

            Assert.AreEqual(original.Length, l.Count());
            l.RemoveDuplicates((x, y) => x == y);

            Assert.AreEqual(1, l[0]);
            Assert.AreEqual(2, l[1]);
            Assert.AreEqual(3, l[2]);
            Assert.AreEqual(4, l[3]);
        }

        [Test]
        public void RemoveFromStartTest()
        {
            var original = new int[] { 1, 1, 2, 3, 4 };
            var l = new List<int>(original);

            Assert.AreEqual(original.Length, l.Count());
            l.RemoveDuplicates((x, y) => x == y);

            Assert.AreEqual(1, l[0]);
            Assert.AreEqual(2, l[1]);
            Assert.AreEqual(3, l[2]);
            Assert.AreEqual(4, l[3]);
        }

        [Test]
        public void RemoveNoneTest()
        {
            var original = new int[] { 1, 2, 3, 4, 5, 6 };
            var l = new List<int>(original);

            Assert.AreEqual(original.Length, l.Count());
            l.RemoveDuplicates((x, y) => x == y);

            Assert.AreEqual(original.Length, l.Count());
        }

        [Test]
        public void RemoveRandomTest()
        {
            var original = new int[] { 4, 4, 4, 3, 2, 1, 1, 2, 3, 3, 4, 4, 1, 1, 2, 34, 4, 1, 1, 1, 2, 3, 4, 5, 1 };
            var l = new List<int>(original);

            Assert.AreEqual(original.Length, l.Count());
            l.RemoveDuplicates((x, y) => x == y);

            Assert.AreEqual(l[0], 4);
            Assert.AreEqual(l[1], 3);
            Assert.AreEqual(l[2], 2);
            Assert.AreEqual(l[3], 1);
            Assert.AreEqual(l[4], 34);
            Assert.AreEqual(l[5], 5);
        }

        #endregion
    }
}