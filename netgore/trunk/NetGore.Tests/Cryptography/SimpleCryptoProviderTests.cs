using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Cryptography;
using NUnit.Framework;

namespace NetGore.Tests.Cryptography
{
    [TestFixture]
    public class SimpleCryptoProviderTests
    {
        static readonly IEnumerable<ISimpleCryptoProvider> _cryptoProviders;

        /// <summary>
        /// Initializes the <see cref="SimpleCryptoProviderTests"/> class.
        /// </summary>
        static SimpleCryptoProviderTests()
        {
            // Add all your ISimpleCryptoProvider implementations to test in here

            _cryptoProviders = new ISimpleCryptoProvider[] { XorCryptoProvider.Create(), MachineCrypto.Create() };
        }

        static bool AreContentsEqual<T>(IList<T> a, IList<T> b) where T : IEquatable<T>
        {
            if (a.Count != b.Count)
                return false;

            for (var i = 0; i < a.Count; i++)
            {
                if (!a[i].Equals(b[i]))
                    return false;
            }

            return true;
        }

        #region Unit tests

        [Test]
        public void EncodeDecodeBytesTest01()
        {
            var expected = new byte[] { 1, 35, 1, 7, 4, 4, 45, 6, 43, 5, 23, 12, 123, 4, 125, 34, 56, 76, 97, 1 };

            foreach (var c in _cryptoProviders)
            {
                var encBytes = c.Encode(expected, CryptoHelper.MachineKey);
                Assert.IsFalse(AreContentsEqual(expected, encBytes));

                var decBytes = c.Decode(encBytes, CryptoHelper.MachineKey);
                Assert.IsTrue(AreContentsEqual(expected, decBytes));
            }
        }

        [Test]
        public void EncodeDecodeBytesTest02()
        {
            var expected = new byte[] { 5 };

            foreach (var c in _cryptoProviders)
            {
                var encBytes = c.Encode(expected, CryptoHelper.MachineKey);

                var decBytes = c.Decode(encBytes, CryptoHelper.MachineKey);
                Assert.IsTrue(AreContentsEqual(expected, decBytes));
            }
        }

        [Test]
        public void EncodeDecodeStringTest01()
        {
            const string expected = "asldfj123?><kafoiurklsaf";

            foreach (var c in _cryptoProviders)
            {
                var encBytes = c.Encode(expected, CryptoHelper.MachineKey);
                var decBytes = c.Decode(encBytes, CryptoHelper.MachineKey);
                var str = Encoding.UTF8.GetString(decBytes);
                Assert.AreEqual(expected, str);
            }
        }

        [Test]
        public void EncodeDecodeStringTest02()
        {
            const string expected = "z";

            foreach (var c in _cryptoProviders)
            {
                var encBytes = c.Encode(expected, CryptoHelper.MachineKey);
                var decBytes = c.Decode(encBytes, CryptoHelper.MachineKey);
                var str = Encoding.UTF8.GetString(decBytes);
                Assert.AreEqual(expected, str);
            }
        }

        #endregion
    }
}