using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace NetGore.Cryptography
{
    /// <summary>
    /// Provides some quick and simple ways to get a hash of data.
    /// </summary>
    public static class Hasher
    {
        public static byte[] GetFileHash(string filePath)
        {
            using (var fs = File.OpenRead(filePath))
            {
                using (var md5 = MD5.Create())
                {
                    return md5.ComputeHash(fs);
                }
            }
        }

        public static byte[] GetHash(string data)
        {
            var bytes = CryptoHelper.StringToBytes(data);
            return GetHash(bytes);
        }

        public static byte[] GetHash(byte[] data)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(data);
            }
        }

        public static string GetHashAsBase16String(byte[] data)
        {
            var h = GetHash(data);
            return CryptoHelper.BytesToString16(h);
        }

        public static string GetHashAsBase16String(string data)
        {
            var h = GetHash(data);
            return CryptoHelper.BytesToString16(h);
        }

        public static string GetHashAsBase64String(byte[] data)
        {
            var h = GetHash(data);
            return CryptoHelper.BytesToString64(h);
        }

        public static string GetHashAsBase64String(string data)
        {
            var h = GetHash(data);
            return CryptoHelper.BytesToString64(h);
        }
    }
}