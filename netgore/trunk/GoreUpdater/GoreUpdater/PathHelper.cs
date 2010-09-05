using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;

namespace GoreUpdater
{
    /// <summary>
    /// Helper methods for working with paths.
    /// </summary>
    public static class PathHelper
    {
        /// <summary>
        /// The minimum version string length. Versions with less digits than this value will be prefixed with 0's.
        /// </summary>
        public const int MinVersionStringLength = 6;

        /// <summary>
        /// The name of the remote file that holds the hash of the <see cref="VersionFileList"/> in a version folder. This file
        /// only exists when the synchronization is completed, and contains the hash of the <see cref="VersionFileList"/> for
        /// the files uploaded.
        /// </summary>
        public const string RemoteFileListHashFileName = "sync_complete";

        /// <summary>
        /// The length of the <see cref="Path.DirectorySeparatorChar"/> as a string.
        /// </summary>
        static readonly int _pathSeperatorCharLength = Path.DirectorySeparatorChar.ToString().Length;

        /// <summary>
        /// The possible path separators.
        /// </summary>
        static readonly string[] _pathSeps = new string[]
        { Path.DirectorySeparatorChar.ToString(), Path.AltDirectorySeparatorChar.ToString() };

        /// <summary>
        /// Combines two paths and forces them to be in different directories. That is, the second path will always
        /// be either a file or sub-directory of the first path.
        /// </summary>
        /// <param name="l">The left-side path (first path).</param>
        /// <param name="r">The right-side path (second path).</param>
        /// <returns>The <paramref name="l"/> and <paramref name="r"/> paths combined.</returns>
        public static string CombineDifferentPaths(string l, string r)
        {
            string ret;

            if (_pathSeps.Any(l.EndsWith))
            {
                if (_pathSeps.Any(r.StartsWith))
                    ret = l + r.Substring(_pathSeperatorCharLength);
                else
                    ret = l + r;
            }
            else
            {
                if (_pathSeps.Any(r.StartsWith))
                    ret = l + r;
                else
                    ret = l + Path.DirectorySeparatorChar + r;
            }

            if (log.IsDebugEnabled)
                log.DebugFormat("CombineDifferentPaths(l: {0}, r: {1}) -> {2}", l, r, ret);

            return ret;
        }

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Combines two paths and forces them to be in different directories. That is, the second path will always
        /// be either a file or sub-directory of the first path.
        /// </summary>
        /// <param name="l">The left-side path (first path).</param>
        /// <param name="r">The right-side path (second path).</param>
        /// <param name="separator">The string to use to separate the paths of the <paramref name="l"/> and <paramref name="r"/>.</param>
        /// <returns>
        /// The <paramref name="l"/> and <paramref name="r"/> paths combined.
        /// </returns>
        public static string CombineDifferentPaths(string l, string r, string separator)
        {
            string ret;

            if (l.EndsWith(separator))
            {
                if (r.StartsWith(separator))
                    ret = l + r.Substring(separator.Length);
                else
                    ret = l + r;
            }
            else
            {
                if (r.StartsWith(separator))
                    ret = l + r;
                else
                    ret = l + separator + r;
            }

            if (log.IsDebugEnabled)
                log.DebugFormat("CombineDifferentPaths(l: {0}, r: {1}, separator: {3}) -> {2}", l, r, ret, separator);

            return ret;
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
            string ret = path;

            if (removeChars != null)
            {
                while (removeChars.Any(ret.EndsWith))
                {
                    ret = path.Substring(0, ret.Length - 1);
                }
            }

            if (!ret.EndsWith(endingChar))
                ret += endingChar;

            if (log.IsDebugEnabled)
                log.DebugFormat("ForceEndWithChar(path: {0}, endingChar: {1}, removeChars: ({2})) -> {3}", path, endingChar, removeChars == null ? "[NULL]" : removeChars.Aggregate((x,y)=>x+","+y), ret);

            return ret;
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

        /// <summary>
        /// Gets the string to use for a version number.
        /// </summary>
        /// <param name="version">The version number.</param>
        /// <returns>The string to use for the version number.</returns>
        public static string GetVersionString(int version)
        {
            var vstr = version.ToString();
            var prefixLen = MinVersionStringLength - vstr.Length;
            if (prefixLen > 0)
                vstr = new string('0', prefixLen) + vstr;

            return vstr;
        }

        /// <summary>
        /// Safely deletes a temporary file.
        /// </summary>
        /// <param name="filePath">The temporary file path.</param>
        public static void SafeDeleteTempFile(string filePath)
        {
            // Try up to 10 times to delete the file. After that, screw it.
            for (var i = 0; i < 10; i++)
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
    }
}