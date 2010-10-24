using System;
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
        public static byte[] MachineKey
        {
            get { return _machineKey; }
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
    }
}