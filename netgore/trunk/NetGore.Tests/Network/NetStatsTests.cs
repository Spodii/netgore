using System.Linq;
using NetGore.Network;
using NUnit.Framework;

namespace NetGore.Tests.Network
{
    [TestFixture]
    public class NetStatsTests
    {
        #region Unit tests

        [Test]
        public void AddMethodsTest()
        {
            var a = new NetStats();

            Assert.AreEqual(a.Connections, 0);
            a.AddConnections(5);
            Assert.AreEqual(a.Connections, 5);
            a.AddConnections(10);
            Assert.AreEqual(a.Connections, 15);

            Assert.AreEqual(a.Recv, 0);
            a.AddRecv(5);
            Assert.AreEqual(a.Recv, 5);
            a.AddRecv(10);
            Assert.AreEqual(a.Recv, 15);

            Assert.AreEqual(a.Sends, 0);
            a.AddSends(5);
            Assert.AreEqual(a.Sends, 5);
            a.AddSends(10);
            Assert.AreEqual(a.Sends, 15);

            Assert.AreEqual(a.Sent, 0);
            a.AddSent(5);
            Assert.AreEqual(a.Sent, 5);
            a.AddSent(10);
            Assert.AreEqual(a.Sent, 15);
        }

        [Test]
        public void AddNetStatsTest()
        {
            var l = new NetStats();
            var r = new NetStats();

            l.AddConnections(1);
            l.AddRecv(2);
            l.AddSends(3);
            l.AddSent(4);

            r.AddConnections(1);
            r.AddRecv(2);
            r.AddSends(3);
            r.AddSent(4);

            var a = l + r;

            Assert.AreEqual(a.Connections, l.Connections + r.Connections);
            Assert.AreEqual(a.Recv, l.Recv + r.Recv);
            Assert.AreEqual(a.Sends, l.Sends + r.Sends);
            Assert.AreEqual(a.Sent, l.Sent + r.Sent);
        }

        [Test]
        public void CopyValuesFromTest()
        {
            var l = new NetStats();

            l.AddConnections(1);
            l.AddRecv(2);
            l.AddSends(3);
            l.AddSent(4);

            var r = new NetStats();
            r.CopyValuesFrom(l);

            Assert.IsTrue(l.AreValuesEqual(r));
            Assert.AreNotEqual(l, r);
        }

        [Test]
        public void DeepCopyTest()
        {
            var l = new NetStats();

            l.AddConnections(1);
            l.AddRecv(2);
            l.AddSends(3);
            l.AddSent(4);

            var r = l.DeepCopy();

            Assert.IsTrue(l.AreValuesEqual(r));
            Assert.AreNotEqual(l, r);
        }

        [Test]
        public void IncrementMethodsTest()
        {
            var a = new NetStats();

            Assert.AreEqual(a.Connections, 0);
            a.IncrementConnections();
            Assert.AreEqual(a.Connections, 1);
            a.IncrementConnections();
            Assert.AreEqual(a.Connections, 2);

            Assert.AreEqual(a.Receives, 0);
            a.IncrementReceives();
            Assert.AreEqual(a.Receives, 1);
            a.IncrementReceives();
            Assert.AreEqual(a.Receives, 2);

            Assert.AreEqual(a.Sends, 0);
            a.IncrementSends();
            Assert.AreEqual(a.Sends, 1);
            a.IncrementSends();
            Assert.AreEqual(a.Sends, 2);
        }

        [Test]
        public void ResetTest()
        {
            var l = new NetStats();

            l.AddConnections(1);
            l.AddRejectedConnections(7);
            l.AddRecv(2);
            l.AddSends(3);
            l.AddSent(4);

            l.Reset();

            Assert.IsTrue(l.AreValuesEqual(new NetStats()));
        }

        [Test]
        public void SubtractNetStatsTest()
        {
            var l = new NetStats();
            var r = new NetStats();

            l.AddConnections(1);
            l.AddRecv(2);
            l.AddSends(3);
            l.AddSent(4);
            l.AddRejectedConnections(5);

            r.AddConnections(1);
            r.AddRecv(2);
            r.AddSends(3);
            r.AddSent(4);
            r.AddRejectedConnections(5);

            var a = l - r;

            Assert.AreEqual(a.Connections, l.Connections - r.Connections);
            Assert.AreEqual(a.RejectedConnections, l.RejectedConnections - r.RejectedConnections);
            Assert.AreEqual(a.Recv, l.Recv - r.Recv);
            Assert.AreEqual(a.Sends, l.Sends - r.Sends);
            Assert.AreEqual(a.Sent, l.Sent - r.Sent);
        }

        [Test]
        public void ValuesEqualTest()
        {
            var l = new NetStats();
            var r = new NetStats();

            l.AddConnections(1);
            Assert.IsFalse(l.AreValuesEqual(r));
            r.AddConnections(1);
            Assert.IsTrue(l.AreValuesEqual(r));

            l.AddRecv(2);
            Assert.IsFalse(l.AreValuesEqual(r));
            r.AddRecv(2);
            Assert.IsTrue(l.AreValuesEqual(r));

            l.AddSends(3);
            Assert.IsFalse(l.AreValuesEqual(r));
            r.AddSends(3);
            Assert.IsTrue(l.AreValuesEqual(r));

            l.AddSent(4);
            Assert.IsFalse(l.AreValuesEqual(r));
            r.AddSent(4);
            Assert.IsTrue(l.AreValuesEqual(r));
        }

        #endregion
    }
}