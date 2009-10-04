using System.Linq;
using NetGore;
using NUnit.Framework;

namespace NetGore.Network.Tests
{
    [TestFixture]
    public class IPAddressHelperTests
    {
        [Test]
        public void ByteArrayToStringTest()
        {
            var b = new byte[] { 123, 232, 112, 12 };
            string s = IPAddressHelper.ToIPv4Address(b, 0);
            Assert.AreEqual("123.232.112.12", s);
        }

        [Test]
        public void StringToUIntTest()
        {
            uint u = IPAddressHelper.IPv4AddressToUInt("123.232.112.12");
            string s = IPAddressHelper.ToIPv4Address(u);
            Assert.AreEqual("123.232.112.12", s);
        }

        [Test]
        public void ToFromBytesTest()
        {
            var b = new byte[] { 123, 232, 112, 12 };
            uint u = IPAddressHelper.IPv4AddressToUInt(b, 0);
            string s = IPAddressHelper.ToIPv4Address(u);
            uint u2 = IPAddressHelper.IPv4AddressToUInt(s);
            Assert.AreEqual("123.232.112.12", s);
            Assert.AreEqual(u, u2);
        }

        [Test]
        public void ToFromUIntTest()
        {
            uint u = IPAddressHelper.IPv4AddressToUInt("123.232.112.12");
            string s = IPAddressHelper.ToIPv4Address(u);
            uint u2 = IPAddressHelper.IPv4AddressToUInt(s);
            Assert.AreEqual("123.232.112.12", s);
            Assert.AreEqual(u, u2);
        }

        [Test]
        public void UIntToStringTest()
        {
            var b = new byte[] { 123, 232, 112, 12 };
            uint u = IPAddressHelper.IPv4AddressToUInt(b, 0);
            string s = IPAddressHelper.ToIPv4Address(u);
            Assert.AreEqual("123.232.112.12", s);
        }
    }
}