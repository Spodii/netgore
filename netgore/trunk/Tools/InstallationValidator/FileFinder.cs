using System.IO;
using System.Linq;

namespace InstallationValidator
{
    /// <summary>
    /// Helper class for finding files.
    /// </summary>
    public static class FileFinder
    {
        /// <summary>
        /// Searches for a file in all the directories of a given root directory.
        /// </summary>
        /// <param name="fileName">The name of the file to look for.</param>
        /// <param name="root">The root directory.</param>
        /// <returns>The path of the found file, or null if the file was not found.</returns>
        public static string Find(string fileName, string root)
        {
            if (string.IsNullOrEmpty(root))
                return null;

            if (!Directory.Exists(root))
                return null;

            foreach (var file in Directory.GetFiles(root, "*", SearchOption.AllDirectories).Reverse())
            {
                if (Path.GetFileName(file).ToLower().EndsWith(fileName))
                    return file;
            }

            return null;
        }
    }
}