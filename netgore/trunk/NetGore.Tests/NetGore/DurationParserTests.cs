using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NetGore.Tests.NetGore
{
    [TestFixture]
    public class DurationParserTests
    {
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
        public void MinuteTest()
        {
            Assert.AreEqual(TimeSpan.FromMinutes(10), DurationParser.Parse("10m"));
            Assert.AreEqual(TimeSpan.FromMinutes(1), DurationParser.Parse("1min"));
            Assert.AreEqual(TimeSpan.FromMinutes(50), DurationParser.Parse("50mins"));
            Assert.AreEqual(TimeSpan.FromMinutes(898), DurationParser.Parse("898minute"));
            Assert.AreEqual(TimeSpan.FromMinutes(88), DurationParser.Parse("88minutes"));
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
        public void DayTest()
        {
            Assert.AreEqual(TimeSpan.FromDays(10), DurationParser.Parse("10d"));
            Assert.AreEqual(TimeSpan.FromDays(1), DurationParser.Parse("1day"));
            Assert.AreEqual(TimeSpan.FromDays(50), DurationParser.Parse("50days"));
        }

        [Test]
        public void MixTest1()
        {
            Assert.AreEqual(TimeSpan.FromDays(10) + TimeSpan.FromHours(5) + TimeSpan.FromSeconds(3), DurationParser.Parse("10d5h3s"));
        }

        [Test]
        public void MixTest2()
        {
            Assert.AreEqual(TimeSpan.FromDays(10) + TimeSpan.FromHours(5) + TimeSpan.FromSeconds(3), DurationParser.Parse("2s5h10days"));
        }

        [Test]
        public void MixTest3()
        {
            Assert.AreEqual(TimeSpan.FromMinutes(10) + TimeSpan.FromSeconds(3), DurationParser.Parse("10m3s"));
        }

        [Test]
        public void MixTest5()
        {
            Assert.AreEqual(TimeSpan.FromMinutes(59) + TimeSpan.FromSeconds(60), DurationParser.Parse("1hr"));
        }
    }
}
