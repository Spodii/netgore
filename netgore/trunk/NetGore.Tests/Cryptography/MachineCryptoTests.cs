using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Cryptography;
using NUnit.Framework;

namespace NetGore.Tests.Cryptography
{
    [TestFixture]
    public class MachineCryptoTests
    {
        [Test]
        public void ValidTest()
        {
            MachineCrypto c = new MachineCrypto();

            var enc = c.Encode("asdf", null);
            var decRaw = c.ValidatedDecode(enc);
            var dec = Encoding.UTF8.GetString(decRaw);

            Assert.AreEqual("asdf", dec);
        }

        [Test]
        public void InvalidTest()
        {
            MachineCrypto c = new MachineCrypto();

            var enc = c.Encode("asdf", null);
            enc[6]++;
            var decRaw = c.ValidatedDecode(enc);

            Assert.IsNull(decRaw);
        }
    }
}
