using System.Linq;
using System.Text;
using NetGore.Cryptography;
using NUnit.Framework;

namespace NetGore.Tests.Cryptography
{
    [TestFixture]
    public class MachineCryptoTests
    {
        #region Unit tests

        [Test]
        public void InvalidTest()
        {
            var c = new MachineCrypto();

            var enc = c.Encode("asdf", null);
            enc[6]++;
            var decRaw = c.ValidatedDecode(enc);

            Assert.IsNull(decRaw);
        }

        [Test]
        public void ValidTest()
        {
            var c = new MachineCrypto();

            var enc = c.Encode("asdf", null);
            var decRaw = c.ValidatedDecode(enc);
            var dec = Encoding.UTF8.GetString(decRaw);

            Assert.AreEqual("asdf", dec);
        }

        #endregion
    }
}