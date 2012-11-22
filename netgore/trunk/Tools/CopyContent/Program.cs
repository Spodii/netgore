using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CopyContent
{
    class Program
    {
        /// <summary>
        /// The file suffixes to be considered asset files.
        /// </summary>
        static readonly string[] _assetSuffixes = new string[]
        { ".bmp", ".jpg", ".jpeg", ".dds", ".psd", ".png", ".gif", ".tga", ".hdr", ".ttf", ".mp3", ".wav", ".ogg", ".wma" };

        static readonly List<string> _cleanFolders = new List<string>();
        static readonly List<string> _skipRootFolders = new List<string>();

        static readonly char[] _dirSepChars = new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

        static string _destRoot;
        static string _srcRoot;
        static bool _silent;

        static string AddPathSeparatorSuffix(string s)
        {
            if (s.Length == 0)
                return Path.DirectorySeparatorChar.ToString();

            var c = s[s.Length - 1];
            if (c != Path.DirectorySeparatorChar && c != Path.AltDirectorySeparatorChar)
                return s + Path.DirectorySeparatorChar;
            else
                return s;
        }

        static string GetRenamedFileName(string fileName)
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
        static bool IsAssetFile(string fileName)
        {
            return _assetSuffixes.Any(x => fileName.EndsWith(x, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">The args.</param>
        static void Main(string[] args)
        {
            /*
            args = new string[] 
            {
                @"C:\Programming\NetGore\DevContent", 
                @"C:\Programming\NetGore\DevContentTest", 
                @"--clean=[Grh]", 
                @"--skip=[Font,Data,Engine,Fx,Languages,Maps,Music,Skeletons,Sounds]"
            };
            */

            // Check for all the args
            if (args == null || args.Length < 2)
            {
                PrintUsage();
                return;
            }

            _srcRoot = args[0];
            _destRoot = args[1];

            // Validate the params
            if (string.IsNullOrEmpty(_srcRoot) || string.IsNullOrEmpty(_destRoot))
            {
                PrintUsage();
                return;
            }

            _srcRoot = AddPathSeparatorSuffix(_srcRoot);
            _destRoot = AddPathSeparatorSuffix(_destRoot);

            // Check for a valid source
            if (!Directory.Exists(_srcRoot))
            {
                Console.WriteLine("Invalid source directory.");
                return;
            }

            // Process the switches
            if (!HandleSwitches(args, 2))
                return;

            // Go through the individual content directories
            Parallel.ForEach(Directory.GetDirectories(_srcRoot, "*", SearchOption.TopDirectoryOnly), absSrcContentDir =>
            {
                string relContentDirNoSep = AbsToRel(_srcRoot, absSrcContentDir);
                if (_skipRootFolders.Contains(relContentDirNoSep, StringComparer.OrdinalIgnoreCase))
                    return;

                string relContentDir = relContentDirNoSep + Path.DirectorySeparatorChar;

                string absDstContentDir = Path.Combine(_destRoot, relContentDir);
                if (!Directory.Exists(absDstContentDir))
                    Directory.CreateDirectory(absDstContentDir);

                var srcDirs = GetAbsAndRelDirectories(absSrcContentDir);
                var dstDirs = GetAbsAndRelDirectories(absDstContentDir);

                bool shouldClean = _cleanFolders.Contains(relContentDirNoSep, StringComparer.OrdinalIgnoreCase);

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

                var srcFiles = GetAbsAndRelFiles(absSrcContentDir);
                var dstFiles = GetAbsAndRelFiles(absDstContentDir);

                var srcFilesRenamed = new HashSet<string>(srcFiles.Keys.Select(GetRenamedFileName));

                // Delete files in destination that are not in source
                if (shouldClean)
                {
                    foreach (var kvp in dstFiles.ToArray())
                    {
                        string relFile = kvp.Key;
                        if (!srcFilesRenamed.Contains(relFile))
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
                        log("Creating directory: " + kvp.Key);
                        Directory.CreateDirectory(dir);
                    }
                }

                // Copy files from source to destination
                foreach (var kvp in srcFiles)
                {
                    string relPath = kvp.Key;
                    string srcFilePath = kvp.Value;
                    string dstFilePath = GetRenamedFileName(Path.Combine(absDstContentDir, relPath.TrimStart(_dirSepChars)));

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

            log("Done");
        }

        static void log(string txt)
        {
            if (_silent)
                return;

            Console.WriteLine(txt);
        }

        static bool ShouldCopyFile(string src, string dst, out FileInfo srcInfo, out FileInfo dstInfo)
        {
            srcInfo = new FileInfo(src);
            dstInfo = new FileInfo(dst);

            if (srcInfo.Length != dstInfo.Length ||
                srcInfo.LastWriteTimeUtc != dstInfo.LastWriteTimeUtc ||
                srcInfo.CreationTimeUtc != dstInfo.CreationTimeUtc)
            {
                return true;
            }

            return false;
        }

        static string AbsToRel(string root, string path)
        {
            if (!root.EndsWith("\\") && !root.EndsWith("/"))
                throw new Exception("Root needs to end with path separator");

            return path.Substring(root.Length);
        }

        static Dictionary<string, string> GetAbsAndRelDirectories(string root)
        {
            root = AddPathSeparatorSuffix(root);

            return Directory.GetDirectories(root, "*", SearchOption.AllDirectories)
                .ToDictionary(x => x.Substring(root.Length), x => x);
        }

        static Dictionary<string, string> GetAbsAndRelFiles(string root)
        {
            root = AddPathSeparatorSuffix(root);

            return Directory.GetFiles(root, "*", SearchOption.AllDirectories)
                .ToDictionary(x => x.Substring(root.Length), x => x);
        }

        /// <summary>
        /// Prints the usage info text.
        /// </summary>
        static void PrintUsage()
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
  --silent:         Don't display messages

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
        static string HandleSwitch_Clean(string value)
        {
            var a = HandleSwitch_ReadValueArray(value);

            if (a == null)
                return "Expected an array for the switch's value";

            foreach (var x in a.Select(x => x.Trim().Trim(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)))
            {
                if (!_cleanFolders.Contains(x, StringComparer.OrdinalIgnoreCase))
                    _cleanFolders.Add(x);
            }

            return null;
        }

        /// <summary>
        /// Reads a switch value array.
        /// </summary>
        /// <param name="value">The value of the switch.</param>
        /// <returns>The values of the <paramref name="value"/>, or null if the <paramref name="value"/> was not a
        /// valid array.</returns>
        static IEnumerable<string> HandleSwitch_ReadValueArray(string value)
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
        static string HandleSwitch_Skip(string value)
        {
            var a = HandleSwitch_ReadValueArray(value);

            if (a == null)
                return "Expected an array for the switch's value";

            foreach (var x in a.Select(x => x.Trim().Trim(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)))
            {
                if (!_skipRootFolders.Contains(x, StringComparer.OrdinalIgnoreCase))
                    _skipRootFolders.Add(x);
            }

            return null;
        }

        /// <summary>
        /// Handles processing the switches.
        /// </summary>
        /// <param name="switches">The array of switches to process.</param>
        /// <param name="offset">The offset in the <paramref name="switches"/> array to start at.</param>
        /// <returns>True if the switches were successfully handled; false if there were one or more errors.</returns>
        static bool HandleSwitches(string[] switches, int offset)
        {
            for (var i = offset; i < switches.Length; i++)
            {
                if (string.IsNullOrEmpty(switches[i].Trim()))
                    continue;

                // Split the key and value
                var s = switches[i].Split(new char[] { '=' }, 2);

                // Check for a valid key
                if (string.IsNullOrEmpty(s[0]))
                {
                    Console.WriteLine("Invalid arguments specified near `{0}`.", switches[i]);
                    return false;
                }

                var key = s[0].ToLowerInvariant().Trim();
                if (!key.StartsWith("--"))
                {
                    Console.WriteLine("Invalid arguments specified near `{0}`.", switches[i]);
                    return false;
                }

                // Remove the prefixed --'s
                key = key.Substring(2);

                // Get the value (or set to null if none given)
                var value = s.Length > 1 ? s[1] : null;

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