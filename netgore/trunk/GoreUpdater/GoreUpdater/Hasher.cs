using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace GoreUpdater
{
    /// <summary>
    /// Provides a way of generating hashes of data.
    /// </summary>
    public static class Hasher
    {
        /// <summary>
        /// Pool of objects for hashing.
        /// </summary>
        static readonly Stack<MD5> _hashers = new Stack<MD5>();

        /// <summary>
        /// Synchronization root for the <see cref="_hashers"/> object.
        /// </summary>
        static readonly object _hashersSync = new object();

        /// <summary>
        /// Converts an array of bytes to a string.
        /// </summary>
        /// <param name="bytes">The array of bytes.</param>
        /// <returns>The string representation of the array of bytes.</returns>
        static string BytesToString(byte[] bytes)
        {
            var s = Convert.ToBase64String(bytes);
            return s;
        }

        /// <summary>
        /// Frees an object for generating the hashes that was acquired from <see cref="GetHasher"/>,
        /// adding it back to the pool for later reuse.
        /// </summary>
        /// <param name="hasher">The object to free.</param>
        static void FreeHasher(MD5 hasher)
        {
            lock (_hashersSync)
            {
                _hashers.Push(hasher);
            }
        }

        /// <summary>
        /// Gets the hash for a file.
        /// </summary>
        /// <param name="filePath">The path to the file to get the hash for.</param>
        /// <returns>The hash of the <paramref name="filePath"/>.</returns>
        public static string GetFileHash(string filePath)
        {
            var hasher = GetHasher();

            try
            {
                byte[] hash;
                using (var fs = File.OpenRead(filePath))
                {
                    hash = hasher.ComputeHash(fs);
                }
                return BytesToString(hash);
            }
            finally
            {
                FreeHasher(hasher);
            }
        }

        /// <summary>
        /// Gets the hash for an array of bytes.
        /// </summary>
        /// <param name="data">The array of bytes.</param>
        /// <returns>The hash for the <paramref name="data"/>.</returns>
        public static string GetHash(byte[] data)
        {
            var hasher = GetHasher();

            try
            {
                var hash = hasher.ComputeHash(data);
                return BytesToString(hash);
            }
            finally
            {
                FreeHasher(hasher);
            }
        }

        /// <summary>
        /// Gets the hash for an array of bytes.
        /// </summary>
        /// <param name="data">The array of bytes.</param>
        /// <param name="offset">The offset in the <paramref name="data"/> array to start at.</param>
        /// <param name="count">The number of bytes to use from the <paramref name="offset"/>.</param>
        /// <returns>
        /// The hash for the <paramref name="data"/>.
        /// </returns>
        public static string GetHash(byte[] data, int offset, int count)
        {
            var hasher = GetHasher();

            try
            {
                var hash = hasher.ComputeHash(data, offset, count);
                return BytesToString(hash);
            }
            finally
            {
                FreeHasher(hasher);
            }
        }

        /// <summary>
        /// Gets an object for generating the hashes from the pool.
        /// </summary>
        /// <returns>The object for generating the hashes.</returns>
        static MD5 GetHasher()
        {
            lock (_hashersSync)
            {
                if (_hashers.Count == 0)
                    return MD5.Create();
                else
                    return _hashers.Pop();
            }
        }
    }
}