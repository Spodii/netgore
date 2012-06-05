using System;
using System.Linq;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class DurationParserTests
    {
        #region Unit tests

        [Test]
        public void DayTest()
        {
            Assert.AreEqual(TimeSpan.FromDays(10), DurationParser.Parse("10d"));
            Assert.AreEqual(TimeSpan.FromDays(1), DurationParser.Parse("1day"));
            Assert.AreEqual(TimeSpan.FromDays(50), DurationParser.Parse("50days"));
        }

        [Test]
        public void HourTest()
        {
            Assert.AreEqual(TimeSpan.FromHours(10), DurationParser.Parse("10h"));
            Assert.AreEqual(TimeSpan.FromHours(1), DurationParser.Parse("1hr"));
            Assert.AreEqual(TimeSpan.FromHours(50), DurationParser.Parse("50hour"));
            Assert.AreEqual(TimeSpan.FromHours(898), DurationParser.Parse("898hours"));
            Assert.AreEqual(TimeSpan.FromHours(88), DurationParser.Parse("88hrs"));
        }

        [Test]
        public void MinuteTest()
        {
            Assert.AreEqual(TimeSpan.FromMinutes(10), DurationParser.Parse("10m"));
            Assert.AreEqual(TimeSpan.FromMinutes(1), DurationParser.Parse("1min"));
            Assert.AreEqual(TimeSpan.FromMinutes(50), DurationParser.Parse("50mins"));
            Assert.AreEqual(TimeSpan.FromMinutes(898), DurationParser.Parse("898minute"));
            Assert.AreEqual(TimeSpan.FromMinutes(88), DurationParser.Parse("88minutes"));
        }

        [Test]
        public void MixTest1()
        {
            Assert.AreEqual(TimeSpan.FromDays(10) + TimeSpan.FromHours(5) + TimeSpan.FromSeconds(3),
                DurationParser.Parse("10d5h3s"));
        }

        [Test]
        public void MixTest2()
        {
            Assert.AreEqual(TimeSpan.FromDays(10) + TimeSpan.FromHours(5) + TimeSpan.FromSeconds(3),
                DurationParser.Parse("3s5h10days"));
        }

        [Test]
        public void MixTest3()
        {
            Assert.AreEqual(TimeSpan.FromMinutes(10) + TimeSpan.FromSeconds(3), DurationParser.Parse("10m3s"));
        }

        [Test]
        public void MixTest4()
        {
            Assert.AreEqual(TimeSpan.FromMinutes(59) + TimeSpan.FromSeconds(60), DurationParser.Parse("1hr"));
        }

        [Test]
        public void MonthTest()
        {
            // "about" 365 days in a year, though let some room slide for minor rounding errors
            const int marginOfErrorPerYears = 2;
            var a = DurationParser.Parse("24months").TotalDays;
            var b = TimeSpan.FromDays(365 * 2).TotalDays;
            Assert.IsTrue(Math.Abs(a - b) < (marginOfErrorPerYears * 2));
        }

        [Test]
        public void NegativeMixTest1()
        {
            Assert.AreEqual(TimeSpan.FromDays(10) + TimeSpan.FromHours(5) + TimeSpan.FromSeconds(-3),
                DurationParser.Parse("10d5h-3s"));
        }

        [Test]
        public void NegativeMixTest2()
        {
            Assert.AreEqual(TimeSpan.FromDays(-10) + TimeSpan.FromHours(5) + TimeSpan.FromSeconds(3),
                DurationParser.Parse("3s5h-10days"));
        }

        [Test]
        public void NegativeMixTest3()
        {
            Assert.AreEqual(TimeSpan.FromMinutes(-10) + TimeSpan.FromSeconds(3), DurationParser.Parse("-10m3s"));
        }

        [Test]
        public void OutOfRangeTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => DurationParser.Parse("1000000years"));
        }

        [Test]
        public void SecondTest()
        {
            Assert.AreEqual(TimeSpan.FromSeconds(10), DurationParser.Parse("10s"));
            Assert.AreEqual(TimeSpan.FromSeconds(1), DurationParser.Parse("1sec"));
            Assert.AreEqual(TimeSpan.FromSeconds(50), DurationParser.Parse("50secs"));
            Assert.AreEqual(TimeSpan.FromSeconds(898), DurationParser.Parse("898second"));
            Assert.AreEqual(TimeSpan.FromSeconds(88), DurationParser.Parse("88seconds"));
        }

        [Test]
        public void SpacingTest()
        {
            Assert.AreEqual(TimeSpan.FromDays(10) + TimeSpan.FromHours(5) + TimeSpan.FromSeconds(3),
                DurationParser.Parse("3s 5h 10days"));
            Assert.AreEqual(TimeSpan.FromDays(10) + TimeSpan.FromHours(5) + TimeSpan.FromSeconds(3),
                DurationParser.Parse("3s 5 hr 10   days  "));
        }

        [Test]
        public void WeekTest()
        {
            Assert.AreEqual(TimeSpan.FromDays(10 * 7), DurationParser.Parse("10w"));
            Assert.AreEqual(TimeSpan.FromDays(1 * 7), DurationParser.Parse("1week"));
            Assert.AreEqual(TimeSpan.FromDays(50 * 7), DurationParser.Parse("50wk"));
            Assert.AreEqual(TimeSpan.FromDays(61 * 7), DurationParser.Parse("61weeks"));
        }

        [Test]
        public void YearTest()
        {
            // "about" 365 days in a year, though let some room slide for minor rounding errors
            const int marginOfErrorPerYears = 2;
            var a = DurationParser.Parse("10years").TotalDays;
            var b = TimeSpan.FromDays(365 * 10).TotalDays;
            Assert.IsTrue(Math.Abs(a - b) < (marginOfErrorPerYears * 10));
        }

        #endregion
    }
}