using System.Linq;

namespace NetGore.Cryptography
{
    /// <summary>
    /// Interface for a very simple symmetric cryptography provider.
    /// </summary>
    public interface ISimpleCryptoProvider
    {
        /// <summary>
        /// Decodes a byte array of data.
        /// </summary>
        /// <param name="data">The data to decode.</param>
        /// <param name="key">The key that was used to encode the data.</param>
        /// <returns>The decoded data.</returns>
        byte[] Decode(byte[] data, byte[] key);

        /// <summary>
        /// Encodes a byte array of data.
        /// </summary>
        /// <param name="data">The data to encode.</param>
        /// <param name="key">The key to use to encode the data.</param>
        /// <returns>The encoded data.</returns>
        byte[] Encode(byte[] data, byte[] key);
    }
}