using System.Linq;
using NetGore.Network;
using NUnit.Framework;

namespace NetGore.Tests.Network
{
    [TestFixture]
    public class IPAddressHelperTests
    {
        #region Unit tests

        [Test]
        public void ByteArrayToStringTest()
        {
            var b = new byte[] { 123, 232, 112, 12 };
            var s = IPAddressHelper.ToIPv4Address(b, 0);
            Assert.AreEqual("123.232.112.12", s);
        }

        [Test]
        public void StringToUIntTest()
        {
            var u = IPAddressHelper.IPv4AddressToUInt("123.232.112.12");
            var s = IPAddressHelper.ToIPv4Address(u);
            Assert.AreEqual("123.232.112.12", s);
        }

        [Test]
        public void ToFromBytesTest()
        {
            var b = new byte[] { 123, 232, 112, 12 };
            var u = IPAddressHelper.IPv4AddressToUInt(b, 0);
            var s = IPAddressHelper.ToIPv4Address(u);
            var u2 = IPAddressHelper.IPv4AddressToUInt(s);
            Assert.AreEqual("123.232.112.12", s);
            Assert.AreEqual(u, u2);
        }

        [Test]
        public void ToFromUIntTest()
        {
            var u = IPAddressHelper.IPv4AddressToUInt("123.232.112.12");
            var s = IPAddressHelper.ToIPv4Address(u);
            var u2 = IPAddressHelper.IPv4AddressToUInt(s);
            Assert.AreEqual("123.232.112.12", s);
            Assert.AreEqual(u, u2);
        }

        [Test]
        public void UIntToStringTest()
        {
            var b = new byte[] { 123, 232, 112, 12 };
            var u = IPAddressHelper.IPv4AddressToUInt(b, 0);
            var s = IPAddressHelper.ToIPv4Address(u);
            Assert.AreEqual("123.232.112.12", s);
        }

        #endregion
    }
}