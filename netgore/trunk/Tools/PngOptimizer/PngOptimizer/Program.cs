using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace PngOptimizer
{
    class Program
    {
        static readonly object _printSync = new object();

        static bool DirectoryExists(string d)
        {
            try
            {
                return Directory.Exists(d);
            }
            catch (IOException)
            {
                return false;
            }
        }

        static void Main(string[] args)
        {
            var recursive = false;
            var skip = string.Empty;
            var filter = string.Empty;

            var rootDir = args.LastOrDefault(x => !string.IsNullOrEmpty(x) && x.Trim().Length > 0);
            if (!string.IsNullOrEmpty(rootDir))
                rootDir = rootDir.Trim(' ', '"');

            if (args == null || args.Length < 1 || args.Contains("/?") || args.Contains("--help") || string.IsNullOrEmpty(rootDir))
            {
                PrintUsage();
                return;
            }

            if (!DirectoryExists(rootDir))
            {
                Print("Target directory does not exist: {0}", rootDir);
                return;
            }

            args = args.Select(x => x.Trim()).ToArray();

            foreach (var a in args.Where(x => x.StartsWith("-skip=", StringComparison.OrdinalIgnoreCase)))
            {
                var o = "-skip=".Length;
                skip = a.Substring(o, a.Length - o - 1);
                skip = skip.Trim('"', ' ');
            }

            foreach (var a in args.Where(x => x.StartsWith("-filter=", StringComparison.OrdinalIgnoreCase)))
            {
                var o = "-filter=".Length;
                filter = a.Substring(o, a.Length - o - 1);
                filter = filter.Trim('"', ' ');
            }

            if (args.Any(x => StringComparer.OrdinalIgnoreCase.Equals("-r", x)))
                recursive = true;

            SetThreadPriority(ThreadPriority.Lowest);

            Run(rootDir, skip, filter, recursive);
        }

        public static void Print(string s, params object[] args)
        {
            lock (_printSync)
            {
                Console.WriteLine(s, args);
            }
        }

        static void PrintUsage()
        {
            const string msg =
                @"Usage: pngoptimizer [options] ""<target path>""
Options:
    -skip=""filter""      Skips files matching string (RegEx)
    -filter=""filter""    Only files matching string (RegEx)
    -r                  Recursive (search sub-directories)
    /? OR --help        Displays the help (this)

Examples:
    pngoptimizer -r ""C:\My\PNGs""
    pngoptimizer -skip=""\.svn"" -filter=""\.png"" -r ""C:\My\PNGs""";

            Print(msg);
        }

        static void Run(string rootDir, string skip, string filter, bool recursive)
        {
            Regex rSkip = null;
            if (!string.IsNullOrEmpty(skip))
                rSkip = new Regex(skip, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

            Regex rFilter = null;
            if (!string.IsNullOrEmpty(filter))
                rFilter = new Regex(filter, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

            var searchOpt = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            IEnumerable<string> files = Directory.GetFiles(rootDir, "*", searchOpt);
            if (rSkip != null)
                files = files.Where(x => !rSkip.IsMatch(x));
            if (rFilter != null)
                files = files.Where(x => rFilter.IsMatch(x));

            var opt = new Optimizer(files, rootDir.Length);

            opt.Run();
        }

        static void SetThreadPriority(ThreadPriority l)
        {
            try
            {
                Thread.CurrentThread.Priority = l;
            }
            catch (Exception ex)
            {
                Debug.Fail(string.Format("Failed to change thread priority. Exception: {0}", ex));
            }
        }
    }
}