using System;
using System.Text;

namespace NetGore.Cryptography
{
    /// <summary>
    /// Extension methods for the <see cref="ISimpleCryptoProvider"/>.
    /// </summary>
    public static class CryptoProviderExtensions
    {
        public static byte[] Encode(this ISimpleCryptoProvider c, string data, byte[] key)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            return c.Encode(bytes, key);
        }

        public static byte[] Decode(this ISimpleCryptoProvider c, string data, byte[] key)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            return c.Decode(bytes, key);
        }

        public static string EncodeToBase64(this ISimpleCryptoProvider c, string data, byte[] key)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            return c.EncodeToBase64(bytes, key);
        }

        public static string EncodeToBase64(this ISimpleCryptoProvider c, byte[] data, byte[] key)
        {
            var encBytes = c.Encode(data, key);
            return Convert.ToBase64String(encBytes);
        }

        public static byte[] DecodeFromBase64(this ISimpleCryptoProvider c, string data, byte[] key)
        {
            var bytes = Convert.FromBase64String(data);
            var decBytes = c.Decode(bytes, key);
            return decBytes;
        }

        public static string DecodeStringFromBase64(this ISimpleCryptoProvider c, string data, byte[] key)
        {
            var decBytes = c.DecodeFromBase64(data, key);
            return Encoding.UTF8.GetString(decBytes);
        }
    }
}