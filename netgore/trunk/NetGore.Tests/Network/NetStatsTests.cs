using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Network;
using NUnit.Framework;

namespace NetGore.Tests.Network
{
    [TestFixture]
    public class NetStatsTests
    {
        [Test]
        public void ResetTest()
        {
            var l = new NetStats();

            l.AddConnections(1);
            l.AddTCPRecv(2);
            l.AddTCPSends(3);
            l.AddTCPSent(4);
            l.AddUDPRecv(5);
            l.AddUDPSends(6);
            l.AddUDPSent(7);

            l.Reset();

            Assert.IsTrue(l.AreValuesEqual(new NetStats()));
        }

        [Test]
        public void DeepCopyTest()
        {
            var l = new NetStats();

            l.AddConnections(1);
            l.AddTCPRecv(2);
            l.AddTCPSends(3);
            l.AddTCPSent(4);
            l.AddUDPRecv(5);
            l.AddUDPSends(6);
            l.AddUDPSent(7);

            var r = l.DeepCopy();

            Assert.IsTrue(l.AreValuesEqual(r));
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

            l.AddTCPRecv(2);
            Assert.IsFalse(l.AreValuesEqual(r));
            r.AddTCPRecv(2);
            Assert.IsTrue(l.AreValuesEqual(r));

            l.AddTCPSends(3);
            Assert.IsFalse(l.AreValuesEqual(r));
            r.AddTCPSends(3);
            Assert.IsTrue(l.AreValuesEqual(r));

            l.AddTCPSent(4);
            Assert.IsFalse(l.AreValuesEqual(r));
            r.AddTCPSent(4);
            Assert.IsTrue(l.AreValuesEqual(r));

            l.AddUDPRecv(5);
            Assert.IsFalse(l.AreValuesEqual(r));
            r.AddUDPRecv(5);
            Assert.IsTrue(l.AreValuesEqual(r));

            l.AddUDPSends(6);
            Assert.IsFalse(l.AreValuesEqual(r));
            r.AddUDPSends(6);
            Assert.IsTrue(l.AreValuesEqual(r));

            l.AddUDPSent(7);
            Assert.IsFalse(l.AreValuesEqual(r));
            r.AddUDPSent(7);
            Assert.IsTrue(l.AreValuesEqual(r));
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

            Assert.AreEqual(a.TCPReceives, 0);
            a.IncrementTCPReceives();
            Assert.AreEqual(a.TCPReceives, 1);
            a.IncrementTCPReceives();
            Assert.AreEqual(a.TCPReceives, 2);

            Assert.AreEqual(a.TCPSends, 0);
            a.IncrementTCPSends();
            Assert.AreEqual(a.TCPSends, 1);
            a.IncrementTCPSends();
            Assert.AreEqual(a.TCPSends, 2);

            Assert.AreEqual(a.UDPReceives, 0);
            a.IncrementUDPReceives();
            Assert.AreEqual(a.UDPReceives, 1);
            a.IncrementUDPReceives();
            Assert.AreEqual(a.UDPReceives, 2);

            Assert.AreEqual(a.UDPSends, 0);
            a.IncrementUDPSends();
            Assert.AreEqual(a.UDPSends, 1);
            a.IncrementUDPSends();
            Assert.AreEqual(a.UDPSends, 2);
        }

        [Test]
        public void AddMethodsTest()
        {
            var a = new NetStats();

            Assert.AreEqual(a.Connections, 0);
            a.AddConnections(5);
            Assert.AreEqual(a.Connections, 5);
            a.AddConnections(10);
            Assert.AreEqual(a.Connections, 15);

            Assert.AreEqual(a.TCPRecv, 0);
            a.AddTCPRecv(5);
            Assert.AreEqual(a.TCPRecv, 5);
            a.AddTCPRecv(10);
            Assert.AreEqual(a.TCPRecv, 15);

            Assert.AreEqual(a.TCPSends, 0);
            a.AddTCPSends(5);
            Assert.AreEqual(a.TCPSends, 5);
            a.AddTCPSends(10);
            Assert.AreEqual(a.TCPSends, 15);

            Assert.AreEqual(a.TCPSent, 0);
            a.AddTCPSent(5);
            Assert.AreEqual(a.TCPSent, 5);
            a.AddTCPSent(10);
            Assert.AreEqual(a.TCPSent, 15);

            Assert.AreEqual(a.UDPRecv, 0);
            a.AddUDPRecv(5);
            Assert.AreEqual(a.UDPRecv, 5);
            a.AddUDPRecv(10);
            Assert.AreEqual(a.UDPRecv, 15);

            Assert.AreEqual(a.UDPSends, 0);
            a.AddUDPSends(5);
            Assert.AreEqual(a.UDPSends, 5);
            a.AddUDPSends(10);
            Assert.AreEqual(a.UDPSends, 15);

            Assert.AreEqual(a.UDPSent, 0);
            a.AddUDPSent(5);
            Assert.AreEqual(a.UDPSent, 5);
            a.AddUDPSent(10);
            Assert.AreEqual(a.UDPSent, 15);
        }

        [Test]
        public void AddNetStatsTest()
        {
            var l = new NetStats();
            var r = new NetStats();

            l.AddConnections(1);
            l.AddTCPRecv(2);
            l.AddTCPSends(3);
            l.AddTCPSent(4);
            l.AddUDPRecv(5);
            l.AddUDPSends(6);
            l.AddUDPSent(7);

            r.AddConnections(1);
            r.AddTCPRecv(2);
            r.AddTCPSends(3);
            r.AddTCPSent(4);
            r.AddUDPRecv(5);
            r.AddUDPSends(6);
            r.AddUDPSent(7);

            var a = l + r;

            Assert.AreEqual(a.Connections, l.Connections + r.Connections);
            Assert.AreEqual(a.TCPRecv, l.TCPRecv + r.TCPRecv);
            Assert.AreEqual(a.TCPSends, l.TCPSends + r.TCPSends);
            Assert.AreEqual(a.TCPSent, l.TCPSent + r.TCPSent);
            Assert.AreEqual(a.UDPRecv, l.UDPRecv + r.UDPRecv);
            Assert.AreEqual(a.UDPSends, l.UDPSends + r.UDPSends);
            Assert.AreEqual(a.UDPSent, l.UDPSent + r.UDPSent);
        }


        [Test]
        public void SubtractNetStatsTest()
        {
            var l = new NetStats();
            var r = new NetStats();

            l.AddConnections(1);
            l.AddTCPRecv(2);
            l.AddTCPSends(3);
            l.AddTCPSent(4);
            l.AddUDPRecv(5);
            l.AddUDPSends(6);
            l.AddUDPSent(7);

            r.AddConnections(1);
            r.AddTCPRecv(2);
            r.AddTCPSends(3);
            r.AddTCPSent(4);
            r.AddUDPRecv(5);
            r.AddUDPSends(6);
            r.AddUDPSent(7);

            var a = l - r;

            Assert.AreEqual(a.Connections, l.Connections - r.Connections);
            Assert.AreEqual(a.TCPRecv, l.TCPRecv - r.TCPRecv);
            Assert.AreEqual(a.TCPSends, l.TCPSends - r.TCPSends);
            Assert.AreEqual(a.TCPSent, l.TCPSent - r.TCPSent);
            Assert.AreEqual(a.UDPRecv, l.UDPRecv - r.UDPRecv);
            Assert.AreEqual(a.UDPSends, l.UDPSends - r.UDPSends);
            Assert.AreEqual(a.UDPSent, l.UDPSent - r.UDPSent);
        }
    }
}
