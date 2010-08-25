using System.Linq;
using NUnit.Framework;

namespace GoreUpdater.Tests
{
    [TestFixture]
    public class PathHelperTests
    {
        [Test]
        public void ForceEndWithCharTest1()
        {
            Assert.AreEqual(@"C:\test\a\", PathHelper.ForceEndWithChar(@"C:\test\a\", '\\'));
            Assert.AreEqual(@"C:\test\a\", PathHelper.ForceEndWithChar(@"C:\test\a\", "\\"));
        }

        [Test]
        public void ForceEndWithCharTest2()
        {
            Assert.AreEqual(@"C:\test\a\", PathHelper.ForceEndWithChar(@"C:\test\a", '\\'));
            Assert.AreEqual(@"C:\test\a\", PathHelper.ForceEndWithChar(@"C:\test\a", "\\"));
        }

        [Test]
        public void ForceEndWithCharTest3()
        {
            Assert.AreEqual(@"C:\test\a\", PathHelper.ForceEndWithChar(@"C:\test\a/", '\\', '/'));
            Assert.AreEqual(@"C:\test\a\", PathHelper.ForceEndWithChar(@"C:\test\a/", "\\", "/"));
        }

        [Test]
        public void CombineDifferentPathsTest1()
        {
            var p = PathHelper.CombineDifferentPaths(@"C:\test\a", @"test\sadf.png");
            Assert.AreEqual(@"C:\test\a\test\sadf.png", p);
        }

        [Test]
        public void CombineDifferentPathsTest2()
        {
            var p = PathHelper.CombineDifferentPaths(@"C:\test\a\", @"test\sadf.png");
            Assert.AreEqual(@"C:\test\a\test\sadf.png", p);
        }

        [Test]
        public void CombineDifferentPathsTest3()
        {
            var p = PathHelper.CombineDifferentPaths(@"C:\test\a", @"\test\sadf.png");
            Assert.AreEqual(@"C:\test\a\test\sadf.png", p);
        }

        [Test]
        public void CombineDifferentPathsTest4()
        {
            var p = PathHelper.CombineDifferentPaths(@"C:\test\a\", @"\test\sadf.png");
            Assert.AreEqual(@"C:\test\a\test\sadf.png", p);
        }

        #region Unit tests

        [Test]
        public void GetVersionStringTest1()
        {
            Assert.AreEqual("000001", PathHelper.GetVersionString(1));
        }

        [Test]
        public void GetVersionStringTest2()
        {
            Assert.AreEqual("000011", PathHelper.GetVersionString(11));
        }

        [Test]
        public void GetVersionStringTest3()
        {
            Assert.AreEqual("000111", PathHelper.GetVersionString(111));
        }

        [Test]
        public void GetVersionStringTest4()
        {
            Assert.AreEqual("001111", PathHelper.GetVersionString(1111));
        }

        [Test]
        public void GetVersionStringTest5()
        {
            Assert.AreEqual("011111", PathHelper.GetVersionString(11111));
        }

        [Test]
        public void GetVersionStringTest6()
        {
            Assert.AreEqual("111111", PathHelper.GetVersionString(111111));
        }

        [Test]
        public void GetVersionStringTest7()
        {
            Assert.AreEqual("1111111", PathHelper.GetVersionString(1111111));
        }

        #endregion
    }
}