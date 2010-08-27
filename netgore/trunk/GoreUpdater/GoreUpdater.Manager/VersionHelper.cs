using System.Linq;
using System.Windows.Forms;

namespace GoreUpdater.Manager
{
    public static class VersionHelper
    {
        /// <summary>
        /// Gets the path to the file list for a version.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns>The path to the file list for a version.</returns>
        public static string GetVersionFileListPath(int version)
        {
            var versionStr = PathHelper.GetVersionString(version);
            var path = PathHelper.CombineDifferentPaths(Application.StartupPath, "filelists");
            path = PathHelper.CombineDifferentPaths(path, versionStr + ".txt");
            return path;
        }

        /// <summary>
        /// Gets the path to the hash of the file list for a version.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns>The path to the hash of the file list for a version.</returns>
        public static string GetVersionFileListHashPath(int version)
        {
            var versionStr = PathHelper.GetVersionString(version);
            var path = PathHelper.CombineDifferentPaths(Application.StartupPath, "filelists");
            path = PathHelper.CombineDifferentPaths(path, versionStr + ".hash");
            return path;
        }

        /// <summary>
        /// Gets the local path to the files for an update version.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns>The path to the files for a update version.</returns>
        public static string GetVersionPath(int version)
        {
            var path = PathHelper.CombineDifferentPaths(GetVersionRootDir(), PathHelper.GetVersionString(version));
            return path;
        }

        /// <summary>
        /// Gets the local path to a file for an update version.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="file">The relative file.</param>
        /// <returns>
        /// The local path to the <paramref name="file"/> for the <paramref name="version"/>.
        /// </returns>
        public static string GetVersionFile(int version, string file)
        {
            var path = GetVersionPath(version);
            path = PathHelper.CombineDifferentPaths(path, file);
            return path;
        }

        /// <summary>
        /// Gets the root directory for version files.
        /// </summary>
        /// <returns>The root directory for version files.</returns>
        public static string GetVersionRootDir()
        {
            return PathHelper.CombineDifferentPaths(Application.StartupPath, "versions");
        }
    }
}