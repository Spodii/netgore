using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GoreUpdater
{
    /// <summary>
    /// Helper methods for the <see cref="IOfflineFileReplacer"/>.
    /// </summary>
    public static class OfflineFileReplacerHelper
    {
        /// <summary>
        /// Tries to execute an <see cref="IOfflineFileReplacer"/> file.
        /// </summary>
        /// <param name="filePath">The path to the file to execute.</param>
        /// <returns>True if the <paramref name="filePath"/> exists and was successfully executed; otherwise false.</returns>
        public static bool TryExecute(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            if (!File.Exists(filePath))
                return false;

            var psi = new ProcessStartInfo(filePath);
            var p = new Process { StartInfo = psi };
            return p.Start();
        }
    }
}