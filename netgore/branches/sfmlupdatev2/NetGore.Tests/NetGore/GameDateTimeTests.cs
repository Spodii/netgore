using System;
using System.Linq;
using NetGore.Features.GameTime;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class GameDateTimeTests
    {
        #region Unit tests

        [Test]
        public void AddOperatorTest()
        {
            var a = new GameDateTime(50);
            var b = new GameDateTime(25);

            Assert.AreEqual(new GameDateTime(75), a + b);
            Assert.AreEqual(new GameDateTime(75), b + a);
        }

        [Test]
        public void EqualsOperatorTest()
        {
            var a = new GameDateTime(512);
            var b = new GameDateTime(512);
            var c = new GameDateTime(312);

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
            Assert.IsFalse(a == c);
            Assert.IsFalse(b == c);
        }

        [Test]
        public void EqualsTest()
        {
            var a = new GameDateTime(512);
            var b = new GameDateTime(512);
            var c = new GameDateTime(312);

            Assert.AreEqual(a, b);
            Assert.AreEqual(b, a);
            Assert.AreNotEqual(a, c);
            Assert.AreNotEqual(b, c);

            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(b.Equals(a));
            Assert.IsFalse(a.Equals(c));
            Assert.IsFalse(b.Equals(c));
        }

        [Test]
        public void GreaterThanOperatorTest()
        {
            var less = new GameDateTime(100);
            var more = new GameDateTime(200);

            Assert.IsFalse(less > more);
            Assert.IsTrue(more > less);
        }

        [Test]
        public void GreaterThanOrEqualToOperatorTest()
        {
            var less = new GameDateTime(100);
            var more = new GameDateTime(200);
            var less2 = new GameDateTime(100);

            Assert.IsFalse(less >= more);
            Assert.IsTrue(more >= less);
            Assert.IsTrue(less >= less2);
            Assert.IsTrue(less2 >= less);
        }

        [Test]
        public void LessThanOperatorTest()
        {
            var less = new GameDateTime(100);
            var more = new GameDateTime(200);

            Assert.IsTrue(less < more);
            Assert.IsFalse(more < less);
        }

        [Test]
        public void LessThanOrEqualToOperatorTest()
        {
            var less = new GameDateTime(100);
            var more = new GameDateTime(200);
            var less2 = new GameDateTime(100);

            Assert.IsTrue(less < more);
            Assert.IsFalse(more < less);
            Assert.IsTrue(less <= less2);
            Assert.IsTrue(less2 <= less);
        }

        [Test]
        public void NotEqualsOperatorTest()
        {
            var a = new GameDateTime(512);
            var b = new GameDateTime(512);
            var c = new GameDateTime(312);

            Assert.IsFalse(a != b);
            Assert.IsFalse(b != a);
            Assert.IsTrue(a != c);
            Assert.IsTrue(b != c);
        }

        [Test]
        public void SubtractOperatorTest()
        {
            var a = new GameDateTime(50);
            var b = new GameDateTime(25);

            Assert.AreEqual(new GameDateTime(25), a - b);
        }

        [Test]
        public void TimeOffsetTest()
        {
            var clientTime = DateTime.Now;

            var serverTime = new DateTime(clientTime.Year, clientTime.Month, clientTime.Day,
                clientTime.Hour - (clientTime.Hour <= 1 ? 0 : 1), clientTime.Minute, clientTime.Second);
            GameDateTime.SetServerTimeOffset(serverTime);

            Assert.AreEqual((clientTime.Hour <= 1 ? 0 : 1), Math.Round(GameDateTime.ServerTimeOffset.TotalHours));
        }

        [Test]
        public void ToFromMinutesTest()
        {
            const int minutes = 123123;
            var gt = new GameDateTime(minutes);
            Assert.AreEqual(minutes, (int)gt.TotalRealMinutes);
            Assert.AreEqual(gt, new GameDateTime((int)gt.TotalRealMinutes));
        }

        #endregion
    }
}