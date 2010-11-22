using System.Linq;
using NUnit.Framework;

namespace GoreUpdater.Tests
{
    [TestFixture]
    public class WildcardFileFilterTests
    {
        #region Unit tests

        [Test]
        public void A1()
        {
            var p = new WildcardFileFilter(@"\a?.txt");
            Assert.IsTrue(p.IsMatch(@"\ab.txt"));
        }

        [Test]
        public void A2()
        {
            var p = new WildcardFileFilter(@"\a?.txt");
            Assert.IsTrue(p.IsMatch(@"\a.txt"));
        }

        [Test]
        public void A3()
        {
            var p = new WildcardFileFilter(@"\a?.txt");
            Assert.IsFalse(p.IsMatch(@"\b.txt"));
        }

        [Test]
        public void A4()
        {
            var p = new WildcardFileFilter(@"\a?.txt");
            Assert.IsFalse(p.IsMatch(@"\abb.txt"));
        }

        [Test]
        public void A5()
        {
            var p = new WildcardFileFilter(@"\a?.txt");
            Assert.IsFalse(p.IsMatch(@"\a"));
        }

        [Test]
        public void A6()
        {
            var p = new WildcardFileFilter(@"\a?.txt");
            Assert.IsFalse(p.IsMatch(@"\atxt"));
        }

        [Test]
        public void B1()
        {
            var p = new WildcardFileFilter(@"\a*.txt");
            Assert.IsTrue(p.IsMatch(@"\a.txt"));
            Assert.IsTrue(p.IsMatch(@"\ab.txt"));

            Assert.IsFalse(p.IsMatch(@"\b.txt"));
        }

        [Test]
        public void B2()
        {
            var p = new WildcardFileFilter(@"\a*.txt");
            Assert.IsTrue(p.IsMatch(@"\ab.txt"));

            Assert.IsFalse(p.IsMatch(@"\b.txt"));
        }

        [Test]
        public void B3()
        {
            var p = new WildcardFileFilter(@"\a*.txt");
            Assert.IsFalse(p.IsMatch(@"\b.txt"));
        }

        [Test]
        public void C1()
        {
            var p = new WildcardFileFilter(@"\a*");
            Assert.IsTrue(p.IsMatch(@"\a\b.txt"));
        }

        [Test]
        public void C2()
        {
            var p = new WildcardFileFilter(@"\a*");
            Assert.IsTrue(p.IsMatch(@"\ab.txt"));
        }

        [Test]
        public void C3()
        {
            var p = new WildcardFileFilter(@"\a*");
            Assert.IsTrue(p.IsMatch(@"a.txt"));
        }

        [Test]
        public void C4()
        {
            var p = new WildcardFileFilter(@"\a*");
            Assert.IsTrue(p.IsMatch(@"ab.txt"));
        }

        [Test]
        public void C5()
        {
            var p = new WildcardFileFilter(@"\a*");
            Assert.IsTrue(p.IsMatch(@"a"));
        }

        [Test]
        public void C6()
        {
            var p = new WildcardFileFilter(@"\a*");
            Assert.IsTrue(p.IsMatch(@"ab"));
        }

        [Test]
        public void C7()
        {
            var p = new WildcardFileFilter(@"\a*");
            Assert.IsFalse(p.IsMatch(@"\b.txt"));
        }

        [Test]
        public void C8()
        {
            var p = new WildcardFileFilter(@"\a*");
            Assert.IsFalse(p.IsMatch(@"\b\a.txt"));
        }

        #endregion
    }
}