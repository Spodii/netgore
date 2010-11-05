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
        static void Cmd(string app, string args, params string[] p)
        {
            if (p != null && p.Length > 0)
                args = string.Format(args, p);

            var psi = new ProcessStartInfo(app, args) { CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden };

            var proc = Process.Start(psi);
            proc.Start();
            proc.WaitForExit();
        }

        static void Main(string[] args)
        {
            var recursive = false;
            var rootDir = Path.GetPathRoot(".");
            var skip = string.Empty;
            var filter = string.Empty;

            if (args == null || args.Length < 1 || args.Contains("/?") || args.Contains("--help"))
            {
                PrintUsage();
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

            foreach (var a in args.Where(x => x.StartsWith("-source=", StringComparison.OrdinalIgnoreCase)))
            {
                var o = "-source=".Length;
                rootDir = a.Substring(o, a.Length - o);
                rootDir = rootDir.Trim('"', ' ');
            }

            Run(rootDir, skip, filter, recursive);
        }

        static void PrintUsage()
        {
            const string msg =
                @"Usage: pngoptimizer [options]
Options:
    -skip=\""filter string\""       Skips files matching string (ReGex)
    -filter=\""filter string\""     Only files matching string (ReGex)
    -source=\""source root\""       The source directory
    -r                              Use recursion
    /? OR --help                    Displays help
";

            Console.WriteLine(msg);
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
            IEnumerable<string> allFilesX = Directory.GetFiles(rootDir, "*", searchOpt);
            if (rSkip != null)
                allFilesX = allFilesX.Where(x => !rSkip.IsMatch(x));
            if (rFilter != null)
                allFilesX = allFilesX.Where(x => rFilter.IsMatch(x));

            var files = allFilesX.ToArray();

            var current = 0;
            var max = files.Length;
            var rDirLen = rootDir.Length;

            foreach (var f in files)
            {
                ++current;
                Console.WriteLine("[{0}%] {1}", Math.Round((current / (float)max) * 100f, 0), f.Substring(rDirLen));

                var fNew = f + ".tmp";
                var fNewInc = 0;
                while (File.Exists(fNew))
                {
                    fNew = f + ".tmp" + ++fNewInc;
                }

                try
                {
                    Cmd("pngcrush.exe", "-brute \"{0}\" \"{1}\"", f, fNew);

                    WaitForFile(f);
                    WaitForFile(fNew);

                    if (!File.Exists(fNew))
                    {
                        Console.WriteLine("\tFile skipped (probably not a valid PNG...)");
                        continue;
                    }

                    File.Copy(fNew, f, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    WaitForFile(fNew);

                    if (File.Exists(fNew))
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            try
                            {
                                File.Delete(fNew);
                            }
                            catch (IOException ex)
                            {
                                if (i == 99)
                                    Console.WriteLine("Failed to delete file `{0}` after `{1}` attempts. Exception: {2}", fNew, i+1, ex);
                                else
                                    Thread.Sleep(10);
                            }
                        }
                    }
                }
            }
        }

        static void WaitForFile(string fullPath)
        {
            if (!File.Exists(fullPath))
                return;

            while (true)
            {
                try
                {
                    using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 100))
                    {
                        fs.ReadByte();
                        break;
                    }
                }
                catch (Exception)
                {
                    Thread.Sleep(50);
                }
            }
        }
    }
}