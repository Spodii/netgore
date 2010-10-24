using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NetGore.Cryptography
{
    /// <summary>
    /// Provides some quick and simple ways to get a hash of data.
    /// </summary>
    public static class Hasher
    {
        public static byte[] GetHash(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            return GetHash(bytes);
        }

        public static byte[] GetHash(byte[] data)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(data);
            }
        }

        public static string GetHashAsBase64String(byte[] data)
        {
            var h = GetHash(data);
            return Convert.ToBase64String(h);
        }

        public static string GetHashAsBase64String(string data)
        {
            var h = GetHash(data);
            return Convert.ToBase64String(h);
        }
    }
}