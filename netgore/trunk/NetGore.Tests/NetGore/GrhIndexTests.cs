using System.Linq;
using NUnit.Framework;

namespace NetGore.Tests
{
    [TestFixture]
    public class GrhIndexTests
    {
        #region Unit tests

        [Test]
        public void CreateTest()
        {
            new GrhIndex(5);
            new GrhIndex(GrhIndex.MinValue);
            new GrhIndex(GrhIndex.MaxValue);
        }

        [Test]
        public void EqualsTest()
        {
            // ReSharper disable EqualExpressionComparison
            Assert.AreEqual(new GrhIndex(5), new GrhIndex(5));
            Assert.IsTrue(new GrhIndex(5) == new GrhIndex(5));
            Assert.IsTrue(new GrhIndex(5).Equals(new GrhIndex(5)));
            // ReSharper restore EqualExpressionComparison
        }

        [Test]
        public void IsInvalidTest()
        {
            Assert.IsTrue(GrhIndex.Invalid.IsInvalid);
            Assert.IsTrue(new GrhIndex().IsInvalid);
        }

        [Test]
        public void NotEqualTest()
        {
            Assert.AreNotEqual(new GrhIndex(5), new GrhIndex(6));
            Assert.IsTrue(new GrhIndex(5) != new GrhIndex(6));
            Assert.IsFalse(new GrhIndex(5).Equals(new GrhIndex(6)));
        }

        #endregion
    }
}