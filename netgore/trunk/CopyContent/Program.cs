using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace CopyContent
{
    class Program
    {
        /// <summary>
        /// The suffix to give asset files in the destionation.
        /// </summary>
        const string _newAssetSuffix = "";

        /// <summary>
        /// When true, each root directory will be processed in a separate thread.
        /// </summary>
        const bool _useThreading = true;

        /// <summary>
        /// The file suffixes to be considered asset files.
        /// </summary>
        static readonly string[] _assetSuffixes = new string[]
        { ".bmp", ".jpg", ".jpeg", ".dds", ".psd", ".png", ".gif", ".tga", ".hdr", ".ttf", ".mp3", ".wav", ".ogg", ".wma" };

        static readonly List<string> _cleanFolders = new List<string>();
        static readonly List<string> _skipRootFolders = new List<string>();

        static string _destRoot;
        static volatile int _liveThreads = 0;
        static string _srcRoot;

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

        /// <summary>
        /// Callback for calling Copy asynchronously from the thread pool.
        /// </summary>
        /// <param name="po">The passed state values.</param>
        static void AsyncCopy(object po)
        {
            var o = (object[])po;
            Copy((DirectoryInfo)o[0], (string)o[1], false);

            _liveThreads--;
        }

        /// <summary>
        /// Checks if a file can skip being copied.
        /// </summary>
        /// <param name="srcFileInfo">The source file.</param>
        /// <param name="destFile">The destination file path.</param>
        /// <returns>True if to skip the copying; otherwise false.</returns>
        static bool CanSkipFileCopy(FileInfo srcFileInfo, string destFile)
        {
            // Destination file does not exist
            if (!File.Exists(destFile))
                return false;

            var destFileInfo = new FileInfo(destFile);

            // Source file is newer
            if (srcFileInfo.LastWriteTimeUtc > File.GetLastWriteTimeUtc(destFile))
                return false;

            // File size mismatch
            if (srcFileInfo.Length != destFileInfo.Length)
                return false;

            return true;
        }

        /// <summary>
        /// Checks if a folder can be skipped.
        /// </summary>
        /// <param name="dir">The dir to check if can be skipped.</param>
        /// <returns>True if the <paramref name="dir"/> can be skipped; otherwise false.</returns>
        static bool CanSkipFolder(FileSystemInfo dir)
        {
            if (StringComparer.OrdinalIgnoreCase.Equals(dir.Name, ".svn"))
                return true;

            return false;
        }

        /// <summary>
        /// Copies the files from a source to destionation recursively.
        /// </summary>
        /// <param name="srcInfo">The source directory.</param>
        /// <param name="dest">The destination directory.</param>
        /// <param name="isRoot">If true, this is the root directory.</param>
        static void Copy(DirectoryInfo srcInfo, string dest, bool isRoot)
        {
            // Check if we can skip this directory
            if (CanSkipFolder(srcInfo))
                return;

            // Ensure the destination directory exists
            if (!Directory.Exists(dest))
                Directory.CreateDirectory(dest);

            // Get all the child directories in this directory
            foreach (var srcChildDI in srcInfo.GetDirectories())
            {
                // Check if this is an immediate child of the root that we can skip
                if (isRoot)
                {
                    var psrcChildDI = srcChildDI;
                    if (_skipRootFolders.Any(x => StringComparer.OrdinalIgnoreCase.Equals(psrcChildDI.Name, x)))
                        continue;
                }

                // Get the path for the destination directory on the other side
                var destChild = Path.Combine(dest, srcChildDI.Name);

                // Recursive copy the child directory, using threading if this is the first level of recursion
                if (_useThreading && isRoot)
                {
                    // Threaded
                    _liveThreads++;
                    ThreadPool.QueueUserWorkItem(AsyncCopy, new object[] { srcChildDI, destChild });
                }
                else
                {
                    // Not threaded
                    Copy(srcChildDI, destChild, false);
                }
            }

            // Check to delete existing files
            var relativePath = AddPathSeparatorSuffix(dest.Substring(_destRoot.Length));
            if (_cleanFolders.Any(x => x.StartsWith(relativePath, StringComparison.OrdinalIgnoreCase)))
            {
                foreach (var dstFile in Directory.GetFiles(dest))
                {
                    var fileName = Path.GetFileName(dstFile);
                    var srcFilePath = _srcRoot + relativePath + fileName;

                    // Check if the file exists in the un-renamed form (its not an asset file)
                    if (File.Exists(srcFilePath))
                    {
                        // File exists - not asset file
                        continue;
                    }

                    // Check if running any of the files that start with that name, after being run through the
                    // asset file name renamer, matches the dest file name
                    var matchedFiles = Directory.GetFiles(_srcRoot + relativePath, fileName + "*", SearchOption.TopDirectoryOnly);
                    var matchedFilesRenamed = matchedFiles.Select(GetRenamedFileName);

                    if (matchedFilesRenamed.Any(x => StringComparer.OrdinalIgnoreCase.Equals(x, fileName)))
                    {
                        // File exists - asset file
                        continue;
                    }

                    // File does not exist
                    File.Delete(dstFile);
                }
            }

            // Copy the files
            foreach (var srcFileInfo in srcInfo.GetFiles())
            {
                // In the root, skip *.exe, *.pdb, *.manifest files
                if (isRoot)
                {
                    if (StringComparer.OrdinalIgnoreCase.Equals(srcFileInfo.Extension, ".exe") ||
                        StringComparer.OrdinalIgnoreCase.Equals(srcFileInfo.Extension, ".pdb") ||
                        StringComparer.OrdinalIgnoreCase.Equals(srcFileInfo.Extension, ".manifest"))
                        continue;
                }

                // Get the destination file name (so we can alter the name of content files)
                var destFileName = GetRenamedFileName(srcFileInfo.Name);

                // Get the destination file path
                var destFile = Path.Combine(dest, destFileName);

                // Check if we can skip the copy
                if (CanSkipFileCopy(srcFileInfo, destFile))
                    continue;

                // Copy the file
                File.Copy(srcFileInfo.FullName, destFile, true);
            }
        }

        static string GetRenamedFileName(string fileName)
        {
            if (IsAssetFile(fileName))
                return Path.GetFileNameWithoutExtension(fileName) + _newAssetSuffix;
            else
                return fileName;
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

            foreach (var x in a.Select(x => AddPathSeparatorSuffix(RemovePathSeparatorPrefix(x.Trim()))))
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
        static string[] HandleSwitch_ReadValueArray(string value)
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

            foreach (var x in a.Select(x => AddPathSeparatorSuffix(RemovePathSeparatorPrefix(x.Trim()))))
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
            //args = new string[] { @"E:\NetGore\DevContent", @"E:\NetGore\DevContentTest", @"--clean=[Font, Grh\Background, Grh\Character]" };

            // Check for all the args
            if (args == null || args.Length < 2)
            {
                PrintUsage();

                if (args == null || args.Length == 0)
                    Console.ReadLine();

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

            // Start the copying
            Copy(new DirectoryInfo(_srcRoot), _destRoot, true);

            // Ensure we let all threads we spawn die off because they are background threads, so closing
            // before they finish will terminate them prematurely
            while (_liveThreads > 0)
            {
                Thread.Sleep(5);
            }
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

Switches that specify a [] at the end indicate an array is expected. The array should be enclosed in double-quotes, but the individual elements should not. Delimit elements with a comma.

NOTE: This program is intended to only be used by the build process. You shouldn't need to manually run this program.
";

            Console.Write(msg);
        }

        static string RemovePathSeparatorPrefix(string s)
        {
            if (s.Length == 0)
                return s;

            var c = s[0];
            if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar)
                return s.Substring(1);
            else
                return s;
        }
    }
}