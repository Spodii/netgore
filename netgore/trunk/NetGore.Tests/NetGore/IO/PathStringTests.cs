using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NetGore.Tests.NetGore.IO
{
    [TestFixture]
    public class PathStringTests
    {
        [Test]
        public void JoinATest()
        {
            PathString s = new PathString(@"C:\One");
            var s2 = s.Join(@"Two");
            Assert.AreEqual(new PathString(@"C:\One\Two"), s2);
        }

        [Test]
        public void EqualsTest()
        {
            Assert.AreEqual(new PathString(@"C:\One\Two"), new PathString(@"C:\One\Two"));
        }

        [Test]
        public void BackTest()
        {
            Assert.AreEqual(new PathString(@"C:\One"), new PathString(@"C:\One\Two").Back());
        }

        [Test]
        public void BackRootTest()
        {
            Assert.AreEqual(new PathString(@"C:\"), new PathString(@"C:\One").Back());
        }

        [Test]
        public void ToStringEqualsStringTest()
        {
            Assert.AreEqual(@"C:\One\Two", new PathString(@"C:\One\Two").ToString());
            Assert.AreEqual(@"C:\One\Two", new PathString(@"C:\One\Two\").ToString());
        }

        [Test]
        public void JoinBTest()
        {
            PathString s = new PathString(@"C:\One\");
            var s2 = s.Join(@"Two");
            Assert.AreEqual(new PathString(@"C:\One\Two"), s2);
        }

        [Test]
        public void JoinCTest()
        {
            PathString s = new PathString(@"C:\One");
            var s2 = s.Join(@"\Two");
            Assert.AreEqual(new PathString(@"C:\One\Two"), s2);
        }

        [Test]
        public void JoinDTest()
        {
            PathString s = new PathString(@"C:\One\");
            var s2 = s.Join(@"\Two");
            Assert.AreEqual(new PathString(@"C:\One\Two"), s2);
        }

        [Test]
        public void JoinETest()
        {
            PathString s = new PathString(@"C:\One\");
            var s2 = s.Join(@"\Two\");
            Assert.AreEqual(new PathString(@"C:\One\Two"), s2);
        }

        [Test]
        public void JoinFTest()
        {
            PathString s = new PathString(@"C:\One\");
            var s2 = s.Join(@"Two\");
            Assert.AreEqual(new PathString(@"C:\One\Two"), s2);
        }

        [Test]
        public void JoinGTest()
        {
            PathString s = new PathString(@"C:\One");
            var s2 = s.Join(@"Two\");
            Assert.AreEqual(new PathString(@"C:\One\Two"), s2);
        }

        [Test]
        public void JoinHTest()
        {
            PathString s = new PathString(@"C:\One");
            var s2 = s.Join(@"\Two\");
            Assert.AreEqual(new PathString(@"C:\One\Two"), s2);
        }
    }
}
