using System.Linq;
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
            var enc = MachineCrypto.Encode("asdf");
            enc = (char)(enc[0] + 1) + enc.Substring(1);
            var decRaw = MachineCrypto.ValidatedDecode(enc);

            Assert.IsNull(decRaw);
        }

        [Test]
        public void ValidTest()
        {
            var enc = MachineCrypto.Encode("asdf");
            var dec = MachineCrypto.ValidatedDecode(enc);

            Assert.AreEqual("asdf", dec);
        }

        #endregion
    }
}