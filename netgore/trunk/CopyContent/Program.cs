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
        const string _newAssetSuffix = ".xnb";

        /// <summary>
        /// The file suffixes to be considered asset files.
        /// </summary>
        static readonly string[] _assetSuffixes = new string[]
        { ".bmp", ".jpg", ".jpeg", ".dds", ".psd", ".png", ".gif", ".tga", ".hdr", ".ttf", ".mp3", ".wav", ".ogg", ".wma" };

        static int _liveThreads = 0;
        static string[] _skipRootFolders;

        /// <summary>
        /// Callback for calling Copy asynchronously from the thread pool.
        /// </summary>
        /// <param name="po">The passed state values.</param>
        static void AsyncCopy(object po)
        {
            var o = (object[])po;
            Copy((DirectoryInfo)o[0], (string)o[1], false);

            Interlocked.Decrement(ref _liveThreads);
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
                if (isRoot)
                {
                    // Threaded
                    Interlocked.Increment(ref _liveThreads);
                    ThreadPool.QueueUserWorkItem(AsyncCopy, new object[] { srcChildDI, destChild });
                }
                else
                {
                    // Not threaded
                    Copy(srcChildDI, destChild, false);
                }
            }

            // Copy the files
            foreach (var srcFileInfo in srcInfo.GetFiles())
            {
                // Get the destination file name (so we can alter the name of content files)
                string destFileName;
                if (IsAssetFile(srcFileInfo.Name))
                    destFileName = Path.GetFileNameWithoutExtension(srcFileInfo.Name) + _newAssetSuffix;
                else
                    destFileName = srcFileInfo.Name;

                // Get the destination file path
                string destFile = Path.Combine(dest, destFileName);

                // Check if we can skip the copy
                if (CanSkipFileCopy(srcFileInfo, destFile))
                    continue;

                // Copy the file
                File.Copy(srcFileInfo.FullName, destFile, true);
            }
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
            //args = new string[] { @"E:\NetGore\DemoGame.Client\Content", @"E:\NetGore\DemoGame.Client\bin\Content" };

            // Check for all the args
            if (args == null || args.Length < 2)
            {
                PrintUsage();
                return;
            }

            var rootSrc = args[0];
            var rootDest = args[1];

            // Validate the params
            if (string.IsNullOrEmpty(rootSrc) || string.IsNullOrEmpty(rootDest))
            {
                PrintUsage();
                return;
            }

            // Check for a valid source
            if (!Directory.Exists(rootSrc))
            {
                Console.WriteLine("Invalid source directory.");
                return;
            }

            // Get the root folders to skip
            List<string> toSkip = new List<string>();
            for (int i = 2; i < args.Length; i++)
            {
                toSkip.Add(args[i]);
            }

            _skipRootFolders = toSkip.ToArray();

            // Start the copying
            Copy(new DirectoryInfo(rootSrc), rootDest, true);

            // Ensure we let all threads we spawn die off because they are background threads, so closing
            // before they finish will terminate them prematurely
            while (_liveThreads > 0)
            {
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// Prints the usage info text.
        /// </summary>
        static void PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("copycontent <source_folder> <dest_folder> [root_folders[]]");
            Console.WriteLine(@"ex: copycontent C:\Content C:\bin\Content Data Engine Maps Skeletons");
            Console.WriteLine("If no root folders are specified, all root folders will be used.");
        }
    }
}