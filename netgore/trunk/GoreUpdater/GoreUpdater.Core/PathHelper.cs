using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace GoreUpdater
{
    /// <summary>
    /// Helper methods for working with paths.
    /// </summary>
    public static class PathHelper
    {
        /// <summary>
        /// Safely deletes a temporary file.
        /// </summary>
        /// <param name="filePath">The temporary file path.</param>
        public static void SafeDeleteTempFile(string filePath)
        {
            // Try up to 10 times to delete the file. After that, screw it.
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    // Make sure the file exists
                    if (!File.Exists(filePath))
                        break;

                    // Try to delete
                    File.Delete(filePath);

                    // Break early if deletion successful
                    break;
                }
                catch (IOException ex)
                {
                    Debug.Fail(ex.ToString());

                    // Give a small timout before trying to delete again
                    Thread.Sleep(25);
                }
            }
        }

        /// <summary>
        /// The minimum version string length. Versions with less digits than this value will be prefixed with 0's.
        /// </summary>
        public const int MinVersionStringLength = 6;

        /// <summary>
        /// The possible path separators.
        /// </summary>
        static readonly string[] _pathSeps = new string[]
        { Path.DirectorySeparatorChar.ToString(), Path.AltDirectorySeparatorChar.ToString() };

        /// <summary>
        /// Gets the string to use for a version number.
        /// </summary>
        /// <param name="version">The version number.</param>
        /// <returns>The string to use for the version number.</returns>
        public static string GetVersionString(int version)
        {
            var vstr = version.ToString();
            int prefixLen = MinVersionStringLength - vstr.Length;
            if (prefixLen > 0)
                vstr = new string('0', prefixLen) + vstr;

            return vstr;
        }

        /// <summary>
        /// Combines two paths and forces them to be in different directories. That is, the second path will always
        /// be either a file or sub-directory of the first path.
        /// </summary>
        /// <param name="l">The left-side path (first path).</param>
        /// <param name="r">The right-side path (second path).</param>
        /// <returns>The <paramref name="l"/> and <paramref name="r"/> paths combined.</returns>
        public static string CombineDifferentPaths(string l, string r)
        {
            if (_pathSeps.Any(l.EndsWith))
            {
                if (_pathSeps.Any(r.StartsWith))
                    return l + r.Substring(1);
                else
                    return l + r;
            }
            else
            {
                if (_pathSeps.Any(r.StartsWith))
                    return l + r;
                else
                    return l + Path.DirectorySeparatorChar + r;
            }
        }

        /// <summary>
        /// Forces a path to end with a specific character.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="endingChar">The character the <paramref name="path"/> needs to end with.</param>
        /// <param name="removeChars">If any of these characters are at the end of the <paramref name="path"/> already,
        /// they will be removed.</param>
        /// <returns>The new path.</returns>
        public static string ForceEndWithChar(string path, string endingChar, params string[] removeChars)
        {
            if (removeChars != null)
            {
                while (removeChars.Any(path.EndsWith))
                {
                    path = path.Substring(0, path.Length - 1);
                }
            }

            if (!path.EndsWith(endingChar))
                path += endingChar;

            return path;
        }

        /// <summary>
        /// Forces a path to end with a specific character.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="endingChar">The character the <paramref name="path"/> needs to end with.</param>
        /// <param name="removeChars">If any of these characters are at the end of the <paramref name="path"/> already,
        /// they will be removed.</param>
        /// <returns>The new path.</returns>
        public static string ForceEndWithChar(string path, char endingChar, params char[] removeChars)
        {
            string[] r = null;
            if (removeChars != null)
            {
                r = new string[removeChars.Length];
                for (var i = 0; i < r.Length; i++)
                {
                    r[i] = removeChars[i].ToString();
                }
            }

            return ForceEndWithChar(path, endingChar.ToString(), r);
        }
    }
}