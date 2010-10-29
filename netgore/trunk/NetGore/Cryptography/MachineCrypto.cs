using System;
using System.Linq;

namespace NetGore.Cryptography
{
    /// <summary>
    /// Provides very simple cryptography for encrypting data based on information about the user's system. Because system
    /// information can change over time, data encrypted by this may not always be able to be decrypted. For this reason,
    /// extra methods are provided to encrypt and decrypt data while including an integrity check to see if the encrypted
    /// data is the same as the decrypted.
    /// </summary>
    public class MachineCrypto : ISimpleCryptoProvider
    {
        /// <summary>
        /// The number of bytes in the header.
        /// </summary>
        const int _headerBytes = 8;

        static readonly ISimpleCryptoProvider _crypt;
        static readonly MachineCrypto _instance;

        /// <summary>
        /// Initializes the <see cref="MachineCrypto"/> class.
        /// </summary>
        static MachineCrypto()
        {
            _crypt = XorCryptoProvider.Create();
            _instance = new MachineCrypto();
        }

        /// <summary>
        /// Gets the key to use in the <see cref="MachineCrypto"/>.
        /// </summary>
        static byte[] Key
        {
            get { return CryptoHelper.MachineKey; }
        }

        /// <summary>
        /// Creates a <see cref="MachineCrypto"/> instance.
        /// </summary>
        /// <returns>The <see cref="MachineCrypto"/> instance.</returns>
        public static MachineCrypto Create()
        {
            return _instance;
        }

        public static byte[] Encode(byte[] data)
        {
            return Create().Encode(data, null);
        }

        public static string Encode(string data)
        {
            return Create().EncodeToBase64(data, null);
        }

        /// <summary>
        /// Decodes a byte array of data and validates that the key used to decode it was the same as the key used to
        /// encode it.
        /// </summary>
        /// <param name="data">The data to decode.</param>
        /// <returns>The decoded data, or null if the key used to encode it differs from the key used to decode it.</returns>
        public static byte[] ValidatedDecode(byte[] data)
        {
            if (data == null || data.Length <= 0)
                return null;

            // Pull the header from the data
            var expectedHeader = new byte[_headerBytes];
            Buffer.BlockCopy(data, 0, expectedHeader, 0, _headerBytes);

            // Pull out the actual data
            var encData = new byte[data.Length - _headerBytes];
            Buffer.BlockCopy(data, _headerBytes, encData, 0, encData.Length);

            // Decrypt the actual data
            var decData = _crypt.Decode(encData, Key);

            // Calculate the hash of the decoded data
            var actualHeader = Hasher.GetHash(decData);

            // Check that the actual header equals the expected header
            for (var i = 0; i < _headerBytes; i++)
            {
                if (expectedHeader[i] != actualHeader[i])
                    return null;
            }

            return decData;
        }

        public static string ValidatedDecode(string data)
        {
            if (string.IsNullOrEmpty(data))
                return null;

            var bytes = CryptoHelper.String64ToBytes(data);
            var decBytes = ValidatedDecode(bytes);
            if (decBytes == null)
                return null;

            var str = CryptoHelper.BytesToString(decBytes);
            return str;
        }

        #region ISimpleCryptoProvider Members

        /// <summary>
        /// Decodes a byte array of data.
        /// </summary>
        /// <param name="data">The data to decode.</param>
        /// <param name="unusedKey">Unused by the <see cref="MachineCrypto"/>.</param>
        /// <returns>The decoded data.</returns>
        public byte[] Decode(byte[] data, byte[] unusedKey)
        {
            // Pull the header from the data
            var expectedHeader = new byte[_headerBytes];
            Buffer.BlockCopy(data, 0, expectedHeader, 0, _headerBytes);

            // Pull out the actual data
            var encData = new byte[data.Length - _headerBytes];
            Buffer.BlockCopy(data, _headerBytes, encData, 0, encData.Length);

            // Decrypt the actual data
            var decData = _crypt.Decode(encData, Key);

            return decData;
        }

        /// <summary>
        /// Encodes a byte array of data.
        /// </summary>
        /// <param name="data">The data to encode.</param>
        /// <param name="unusedKey">Unused by the <see cref="MachineCrypto"/>.</param>
        /// <returns>The encoded data.</returns>
        public byte[] Encode(byte[] data, byte[] unusedKey)
        {
            // Calculate the header (hash of the decrypted data)
            var header = Hasher.GetHash(data);

            // Encrypt the data
            var encData = _crypt.Encode(data, Key);

            // Join the header and encrypted data
            var joinedData = new byte[encData.Length + _headerBytes];
            Buffer.BlockCopy(header, 0, joinedData, 0, Math.Min(header.Length, _headerBytes));
            encData.CopyTo(joinedData, _headerBytes);

            // Return the joined data
            return joinedData;
        }

        #endregion
    }
}