using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace NetGore.Cryptography
{
    /// <summary>
    /// Provides helper methods and properties for cryptography.
    /// </summary>
    public static class CryptoHelper
    {
        static readonly byte[] _machineKey;

        /// <summary>
        /// Initializes the <see cref="CryptoHelper"/> class.
        /// </summary>
        static CryptoHelper()
        {
            _machineKey = GenerateMachineKey();
        }

        /// <summary>
        /// A key generated from information about the machine this application is running on.
        /// Can end up changing when anything from hardware to the operating system and even some
        /// OS-level configuration changes, so do not rely on it to be consistent over time.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public static byte[] MachineKey
        {
            get { return _machineKey; }
        }

        /// <summary>
        /// Converts a byte array to a string.
        /// This should be used by all cryptographic implementations performing this operating to ensure consistent behavior.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <returns>The string.</returns>
        public static string BytesToString(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        /// <summary>
        /// Converts a byte array to a base-16 string.
        /// This should be used by all cryptographic implementations performing this operating to ensure consistent behavior.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <returns>The string.</returns>
        public static string BytesToString16(byte[] data)
        {
            var ret = BitConverter.ToString(data);
            ret = ret.Replace("-", string.Empty);
            return ret;
        }

        /// <summary>
        /// Converts a byte array to a base-64 string.
        /// This should be used by all cryptographic implementations performing this operating to ensure consistent behavior.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <returns>The string.</returns>
        public static string BytesToString64(byte[] data)
        {
            return Convert.ToBase64String(data);
        }

        static byte[] GenerateMachineKey()
        {
            var sb = new StringBuilder();

            // Do not add any information here that could potentially compromise the user's privacy. This is a relatively weak key
            // used for encrypting even weaker data, so reversing this information is unlikely but not impossible.

            sb.Append(Environment.OSVersion.Platform);
            sb.Append(Environment.OSVersion.Version.Major);
            sb.Append(Environment.ProcessorCount);
            sb.Append(Environment.MachineName);

            return Hasher.GetHash(sb.ToString());
        }

        /// <summary>
        /// Converts a base-64 string to a byte array.
        /// This should be used by all cryptographic implementations performing this operating to ensure consistent behavior.
        /// </summary>
        /// <param name="data">The base-64 string.</param>
        /// <returns>The byte array.</returns>
        public static byte[] String64ToBytes(string data)
        {
            return Convert.FromBase64String(data);
        }

        /// <summary>
        /// Converts a string to a byte array.
        /// This should be used by all cryptographic implementations performing this operating to ensure consistent behavior.
        /// </summary>
        /// <param name="data">The string.</param>
        /// <returns>The byte array.</returns>
        public static byte[] StringToBytes(string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }
    }
}