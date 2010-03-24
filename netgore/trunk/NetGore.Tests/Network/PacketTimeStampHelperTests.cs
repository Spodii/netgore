using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NetGore.Network;
using NUnit.Framework;

namespace NetGore.Tests.Network
{
    [TestFixture]
    public class PacketTimeStampHelperTests
    {
        [Test]
        public void GetTimeStampTests()
        {
            Assert.Greater(PacketTimeStampHelper.GetTimeStamp(100), PacketTimeStampHelper.GetTimeStamp(0));
            Assert.Greater(PacketTimeStampHelper.GetTimeStamp(PacketTimeStampHelper.TimeStampResolution+1), PacketTimeStampHelper.GetTimeStamp(0));
        }

        [Test]
        public void IsTimeStampNewerTests()
        {
            Assert.True(PacketTimeStampHelper.IsTimeStampNewer(1, 0));
            Assert.False(PacketTimeStampHelper.IsTimeStampNewer(0, 1));

            Assert.True(PacketTimeStampHelper.IsTimeStampNewer(5, 0));
            Assert.False(PacketTimeStampHelper.IsTimeStampNewer(0, 5));

            Assert.True(PacketTimeStampHelper.IsTimeStampNewer(100, 0));
            Assert.False(PacketTimeStampHelper.IsTimeStampNewer(0, 100));

            Assert.True(PacketTimeStampHelper.IsTimeStampNewer(251, 250));
            Assert.False(PacketTimeStampHelper.IsTimeStampNewer(250, 251));

            Assert.True(PacketTimeStampHelper.IsTimeStampNewer(0, 250));
            Assert.False(PacketTimeStampHelper.IsTimeStampNewer(250, 0));

            Assert.True(PacketTimeStampHelper.IsTimeStampNewer(0, 255));
            Assert.False(PacketTimeStampHelper.IsTimeStampNewer(255, 0));

            Assert.True(PacketTimeStampHelper.IsTimeStampNewer(5, 220));
            Assert.False(PacketTimeStampHelper.IsTimeStampNewer(200, 5));

            Assert.True(PacketTimeStampHelper.IsTimeStampNewer(0, 0));
            Assert.True(PacketTimeStampHelper.IsTimeStampNewer(250, 250));
            Assert.True(PacketTimeStampHelper.IsTimeStampNewer(255, 255));
        }
    }
}
