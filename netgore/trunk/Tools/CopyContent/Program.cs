using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CopyContent
{
    internal class Program
    {
        /// <summary>
        /// The file suffixes to be considered asset files. Asset files get their file extension removed.
        /// Very roughly sorted by how commonly they are used.
        /// </summary>
        private static readonly string[] _assetSuffixes = new[] 
        {
            ".png", ".bmp", ".wav", ".ogg", ".wma", ".mp3", ".jpg", ".jpeg", ".gif", ".tga", ".hdr", ".ttf", ".dds", ".psd"
        };

        /// <summary>
        /// File names/endings that will never be copied to, and will get deleted from the destination.
        /// </summary>
        private static readonly string[] _neverIncludeFiles = new[] { "Thumbs.db" };

        private static readonly List<string> _cleanFolders = new List<string>();
        private static readonly List<string> _skipRootFolders = new List<string>();
        private static bool _silent;

        private static readonly char[] _dirSepChars = new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

        /// <summary>
        /// Ensures the specified input contains the directory specified char at the end.
        /// </summary>
        private static string AddPathSeparatorSuffix(string s)
        {
            if (s.Length == 0)
                return Path.DirectorySeparatorChar.ToString();

            char c = s[s.Length - 1];
            if (c != Path.DirectorySeparatorChar && c != Path.AltDirectorySeparatorChar)
                return s + Path.DirectorySeparatorChar;
            else
                return s;
        }

        /// <summary>
        /// Renames a file (or file path) to the correct content name.
        /// (Simply just removes the file extension)
        /// </summary>
        private static string GetRenamedFileName(string fileName)
        {
            if (!IsAssetFile(fileName))
                return fileName;

            int sepIndex = fileName.LastIndexOf('.');
            string ret = fileName.Substring(0, sepIndex);
            return ret;
        }

        /// <summary>
        /// Gets if a file is an asset file.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>True if an asset file; otherwise false.</returns>
        private static bool IsAssetFile(string fileName)
        {
            return _assetSuffixes.Any(x => fileName.EndsWith(x, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">The args.</param>
        private static void Main(string[] args)
        {
            Stopwatch watch = Stopwatch.StartNew();

            /*
            args = new string[] 
            {
                @"C:\Programming\NetGore\DevContent", 
                @"C:\Programming\NetGore\DevContentTest", 
                @"--clean=[Maps]", 
                @"--skip=[Font,Data,Engine,Fx,Languages,Music,Skeletons,Sounds]"
            };
            */

            // Check for all the args
            if (args == null || args.Length < 2)
            {
                PrintUsage();
                return;
            }

            string srcAbsRoot = args[0];
            string destAbsRoot = args[1];

            // Validate the params
            if (string.IsNullOrEmpty(srcAbsRoot) || string.IsNullOrEmpty(destAbsRoot))
            {
                PrintUsage();
                return;
            }

            srcAbsRoot = AddPathSeparatorSuffix(srcAbsRoot);
            destAbsRoot = AddPathSeparatorSuffix(destAbsRoot);

            // Check for a valid source
            if (!Directory.Exists(srcAbsRoot))
            {
                Console.WriteLine("Invalid source directory.");
                return;
            }

            // Process the switches
            if (!HandleSwitches(args, 2))
                return;

            // Go through the individual content directories
            string[] absSrcContentDirs = Directory.GetDirectories(srcAbsRoot, "*", SearchOption.TopDirectoryOnly);
            Parallel.ForEach(absSrcContentDirs, absSrcContentDir =>
            {
                string relContentDirNoSep = AbsToRel(srcAbsRoot, absSrcContentDir);
                if (_skipRootFolders.Contains(relContentDirNoSep, StringComparer.OrdinalIgnoreCase))
                    return;

                string relContentDir = relContentDirNoSep + Path.DirectorySeparatorChar;

                string absDstContentDir = Path.Combine(destAbsRoot, relContentDir);
                if (!Directory.Exists(absDstContentDir))
                    Directory.CreateDirectory(absDstContentDir);

                bool shouldClean = _cleanFolders.Contains(relContentDirNoSep, StringComparer.OrdinalIgnoreCase);

                Dictionary<string, string> srcDirs = GetAbsAndRelDirectories(absSrcContentDir);
                Dictionary<string, string> dstDirs = GetAbsAndRelDirectories(absDstContentDir);

                // Delete folders in destination that are not in source
                if (shouldClean)
                {
                    foreach (var kvp in dstDirs.ToArray())
                    {
                        string relDir = kvp.Key;
                        if (!srcDirs.ContainsKey(relDir))
                        {
                            if (Directory.Exists(kvp.Value))
                            {
                                log("Deleting directory: " + relDir);
                                Directory.Delete(kvp.Value, true);
                            }

                            dstDirs.Remove(relDir);
                        }
                    }
                }
                
                Dictionary<string, string> srcFiles = GetAbsAndRelFiles(absSrcContentDir);
                Dictionary<string, string> dstFiles = GetAbsAndRelFiles(absDstContentDir);

                var srcFilesRenamed = new HashSet<string>(srcFiles.Keys.Select(GetRenamedFileName));

                // Delete files in destination that are not in source, or match a non-inclusion pattern
                if (shouldClean)
                {
                    foreach (var kvp in dstFiles.ToArray())
                    {
                        string relFile = kvp.Key;
                        if (!srcFilesRenamed.Contains(relFile) || _neverIncludeFiles.Any(x => kvp.Value.EndsWith(x, StringComparison.OrdinalIgnoreCase)))
                        {
                            if (File.Exists(kvp.Value))
                            {
                                log("Deleting file: " + relFile);
                                File.Delete(kvp.Value);
                            }

                            dstFiles.Remove(relFile);
                        }
                    }
                }

                // Create directories not in destination that are in source
                foreach (var kvp in srcDirs)
                {
                    if (!dstDirs.ContainsKey(kvp.Key))
                    {
                        string dir = Path.Combine(absDstContentDir, kvp.Key.TrimStart(_dirSepChars));
                        if (!Directory.Exists(dir))
                        {
                            log("Creating directory: " + kvp.Key);
                            Directory.CreateDirectory(dir);
                        }
                    }
                }

                // Copy files from source to destination
                foreach (var kvp in srcFiles)
                {
                    string srcFilePath = kvp.Value;

                    if (_neverIncludeFiles.Any(x => srcFilePath.EndsWith(x, StringComparison.OrdinalIgnoreCase)))
                        continue;

                    string relPath = kvp.Key;
                    string dstFilePath = GetRenamedFileName(Path.Combine(absDstContentDir, relPath.TrimStart( _dirSepChars)));

                    FileInfo srcInfo = null, dstInfo = null;
                    if (!File.Exists(dstFilePath) || ShouldCopyFile(srcFilePath, dstFilePath, out srcInfo, out dstInfo))
                    {
                        log("Copying file: " + relPath);
                        File.Copy(srcFilePath, dstFilePath, true);
                       
                        if (srcInfo == null)
                            srcInfo = new FileInfo(srcFilePath);

                        if (dstInfo == null)
                            dstInfo = new FileInfo(dstFilePath);

                        dstInfo.LastWriteTimeUtc = srcInfo.LastWriteTimeUtc;
                        dstInfo.CreationTimeUtc = srcInfo.CreationTimeUtc;
                    }
                }
            });

            log(string.Format("CopyContent complete in {0}ms", watch.ElapsedMilliseconds));
        }

        /// <summary>
        /// Logs the status.
        /// </summary>
        private static void log(string txt)
        {
            if (_silent)
                return;

            Console.WriteLine(txt);
        }

        /// <summary>
        /// Checks if the existing source file should be copied to the destination.
        /// Both files must exist.
        /// </summary>
        private static bool ShouldCopyFile(string src, string dst, out FileInfo srcInfo, out FileInfo dstInfo)
        {
            // If the file size, last write time, or creation time differ then replace the destionation
            // We update the last write and creation time on the destination after doing the copy
            srcInfo = new FileInfo(src);
            dstInfo = new FileInfo(dst);

            return
                srcInfo.Length != dstInfo.Length ||
                srcInfo.LastWriteTimeUtc != dstInfo.LastWriteTimeUtc ||
                srcInfo.CreationTimeUtc != dstInfo.CreationTimeUtc;
        }

        /// <summary>
        /// Converts an absolute path to a relative path for the root.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string AbsToRel(string root, string path)
        {
            if (!root.EndsWith("\\") && !root.EndsWith("/"))
                throw new Exception("Root needs to end with path separator");

            return path.Substring(root.Length);
        }

        /// <summary>
        /// Gets the directories under the given root, and returns the relative (key) and absolute (value) paths.
        /// </summary>
        private static Dictionary<string, string> GetAbsAndRelDirectories(string root)
        {
            root = AddPathSeparatorSuffix(root);

            return Directory.GetDirectories(root, "*", SearchOption.AllDirectories)
                .ToDictionary(x => x.Substring(root.Length), x => x);
        }

        /// <summary>
        /// Gets the files under the given root, and returns the relative (key) and absolute (value) paths.
        /// </summary>
        private static Dictionary<string, string> GetAbsAndRelFiles(string root)
        {
            root = AddPathSeparatorSuffix(root);

            return Directory.GetFiles(root, "*", SearchOption.AllDirectories)
                .ToDictionary(x => x.Substring(root.Length), x => x);
        }

        /// <summary>
        /// Prints the usage info text.
        /// </summary>
        private static void PrintUsage()
        {
            const string msg =
                @"
Usage:
  copycontent <source_folder> <dest_folder> [switches]

Examples:
  copycontent ""C:\Content\"" ""C:\bin\Content\""
  copycontent ... --skip=""[Data, Engine, Maps, Skeletons]""
  copycontent ... --clean=""[Grh, Data\Temp\]""

Arguments:
  source_folder:    The folder to copy files from
  dest_folder:      The folder to copy files to

Switches:
  --skip[]:         Specifies root folder names to skip copying
  --clean[]:        Specifies paths to delete files from the dest that are not in the source
  --silent:         Don't display status messages

Switches that specify a [] at the end indicate an array is expected. The array should be enclosed in double-quotes, but the individual elements should not. Delimit elements with a comma.

NOTE: This program is intended to only be used by the build process. You shouldn't need to manually run this program.
";

            Console.Write(msg);
        }

        /// <summary>
        /// Handles the --clean switch.
        /// </summary>
        /// <param name="value">The value passed to the switch.</param>
        /// <returns>The error message if there was an error processing the <paramref name="value"/>, or null if no errors.</returns>
        private static string HandleSwitch_Clean(string value)
        {
            IEnumerable<string> a = HandleSwitch_ReadValueArray(value);

            if (a == null)
                return "Expected an array for the switch's value";

            a = a.Select(x => x.Trim().Trim(_dirSepChars)); // Remove whicespaces and separators

            _cleanFolders.AddRange(a.Distinct(StringComparer.OrdinalIgnoreCase));

            return null;
        }

        /// <summary>
        /// Reads a switch value array.
        /// </summary>
        /// <param name="value">The value of the switch.</param>
        /// <returns>The values of the <paramref name="value"/>, or null if the <paramref name="value"/> was not a
        /// valid array.</returns>
        private static IEnumerable<string> HandleSwitch_ReadValueArray(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            if (!value.StartsWith("[") || !value.EndsWith("]"))
                return null;

            value = value.Substring(1, value.Length - 2);
            return value.Split(',');
        }

        /// <summary>
        /// Handles the --skip switch.
        /// </summary>
        /// <param name="value">The value passed to the switch.</param>
        /// <returns>The error message if there was an error processing the <paramref name="value"/>, or null if no errors.</returns>
        private static string HandleSwitch_Skip(string value)
        {
            IEnumerable<string> a = HandleSwitch_ReadValueArray(value);

            if (a == null)
                return "Expected an array for the switch's value";

            a = a.Select(x => x.Trim().Trim(_dirSepChars)); // Remove whicespaces and separators

            _skipRootFolders.AddRange(a.Distinct(StringComparer.OrdinalIgnoreCase));

            return null;
        }

        /// <summary>
        /// Handles processing the switches.
        /// </summary>
        /// <param name="switches">The array of switches to process.</param>
        /// <param name="offset">The offset in the <paramref name="switches"/> array to start at.</param>
        /// <returns>True if the switches were successfully handled; false if there were one or more errors.</returns>
        private static bool HandleSwitches(string[] switches, int offset)
        {
            for (int i = offset; i < switches.Length; i++)
            {
                if (string.IsNullOrEmpty(switches[i].Trim()))
                    continue;

                // Split the key and value
                string[] s = switches[i].Split(new[] {'='}, 2);

                // Check for a valid key
                if (string.IsNullOrEmpty(s[0]))
                {
                    Console.WriteLine("Invalid arguments specified near `{0}`.", switches[i]);
                    return false;
                }

                string key = s[0].ToLowerInvariant().Trim();
                if (!key.StartsWith("--"))
                {
                    Console.WriteLine("Invalid arguments specified near `{0}`.", switches[i]);
                    return false;
                }

                // Remove the prefixed --'s
                key = key.Substring(2);

                // Get the value (or set to null if none given)
                string value = s.Length > 1 ? s[1] : null;

                // Handle the switch by calling their method, which returns a string if there was an error
                string err;
                switch (key)
                {
                    case "skip":
                        err = HandleSwitch_Skip(value);
                        break;

                    case "clean":
                        err = HandleSwitch_Clean(value);
                        break;

                    case "silent":
                        _silent = true;
                        err = null;
                        break;

                    default:
                        Console.WriteLine("Unknown switch `{0}` near `{1}`.", key, switches[i]);
                        return false;
                }

                // Display error handling a switch
                if (!string.IsNullOrEmpty(err))
                {
                    Console.WriteLine("Failed to handle switch `{0}` near `{1}`: {2}", key, switches[i], err);
                    return false;
                }
            }

            return true;
        }
    }
}