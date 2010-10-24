using System.Linq;

namespace NetGore.Cryptography
{
    /// <summary>
    /// A provider for Xor encryption.
    /// </summary>
    public class XorCryptoProvider : ISimpleCryptoProvider
    {
        static readonly XorCryptoProvider _instance;

        /// <summary>
        /// Initializes the <see cref="XorCryptoProvider"/> class.
        /// </summary>
        static XorCryptoProvider()
        {
            _instance = new XorCryptoProvider();
        }

        /// <summary>
        /// Creates a <see cref="XorCryptoProvider"/> instance.
        /// </summary>
        /// <returns>The <see cref="XorCryptoProvider"/> instance.</returns>
        public static XorCryptoProvider Create()
        {
            return _instance;
        }

        #region ISimpleCryptoProvider Members

        /// <summary>
        /// Decodes a byte array of data.
        /// </summary>
        /// <param name="data">The data to decode.</param>
        /// <param name="key">The key that was used to encode the data.</param>
        /// <returns>The decoded data.</returns>
        public byte[] Decode(byte[] data, byte[] key)
        {
            return Encode(data, key);
        }

        /// <summary>
        /// Encodes a byte array of data.
        /// </summary>
        /// <param name="data">The data to encode.</param>
        /// <param name="key">The key to use to encode the data.</param>
        /// <returns>The encoded data.</returns>
        public byte[] Encode(byte[] data, byte[] key)
        {
            var ret = new byte[data.Length];

            var j = 0;
            for (var i = 0; i < data.Length; i++)
            {
                ret[i] = (byte)(data[i] ^ key[j]);

                j++;
                if (j >= key.Length)
                    j = 0;
            }

            return ret;
        }

        #endregion
    }
}