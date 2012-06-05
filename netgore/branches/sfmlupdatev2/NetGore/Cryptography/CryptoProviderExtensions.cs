using System.Linq;

namespace NetGore.Cryptography
{
    /// <summary>
    /// Extension methods for the <see cref="ISimpleCryptoProvider"/>.
    /// </summary>
    public static class CryptoProviderExtensions
    {
        public static byte[] Decode(this ISimpleCryptoProvider c, string data, byte[] key)
        {
            var bytes = CryptoHelper.StringToBytes(data);
            return c.Decode(bytes, key);
        }

        public static byte[] DecodeFromBase64(this ISimpleCryptoProvider c, string data, byte[] key)
        {
            var bytes = CryptoHelper.String64ToBytes(data);
            var decBytes = c.Decode(bytes, key);
            return decBytes;
        }

        public static string DecodeStringFromBase64(this ISimpleCryptoProvider c, string data, byte[] key)
        {
            var decBytes = c.DecodeFromBase64(data, key);
            return CryptoHelper.BytesToString(decBytes);
        }

        public static byte[] Encode(this ISimpleCryptoProvider c, string data, byte[] key)
        {
            var bytes = CryptoHelper.StringToBytes(data);
            return c.Encode(bytes, key);
        }

        public static string EncodeToBase64(this ISimpleCryptoProvider c, string data, byte[] key)
        {
            var bytes = CryptoHelper.StringToBytes(data);
            return c.EncodeToBase64(bytes, key);
        }

        public static string EncodeToBase64(this ISimpleCryptoProvider c, byte[] data, byte[] key)
        {
            var encBytes = c.Encode(data, key);
            return CryptoHelper.BytesToString64(encBytes);
        }
    }
}