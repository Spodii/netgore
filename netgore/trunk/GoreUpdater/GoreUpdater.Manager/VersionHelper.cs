using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GoreUpdater.Manager
{
    public static class VersionHelper
    {
        /// <summary>
        /// Gets the root directory for version files.
        /// </summary>
        /// <returns>The root directory for version files.</returns>
        public static string GetVersionRootDir()
        {
            return PathHelper.CombineDifferentPaths(Application.StartupPath, "versions");
        }

        /// <summary>
        /// Gets the path to the files for an update version.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns>The path to the files for a update version.</returns>
        public static string GetVersionPath(int version)
        {
            var path = PathHelper.CombineDifferentPaths(GetVersionRootDir(), PathHelper.GetVersionString(version));
            return path;
        }

        /// <summary>
        /// Gets the path to the file list for a version.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns>The path to the file list for a version.</returns>
        public static string GetVersionFileListPath(int version)
        {
            var path = PathHelper.CombineDifferentPaths(Application.StartupPath, "filelists");
            path = PathHelper.CombineDifferentPaths(path, version + ".txt");
            return path;
        }
    }
}
