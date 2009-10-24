using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeReleasePreparer
{
    class Program
    {
        static RegexCollection _fileRegexes;
        static RegexCollection _folderRegexes;

        static string _root = null;

        static void AsyncRunBatchFile(params string[] lines)
        {
            var filePath = Path.GetTempFileName() + ".bat";
            File.WriteAllLines(filePath, lines);
            ProcessStartInfo psi = new ProcessStartInfo(filePath) { CreateNoWindow = true, UseShellExecute = true, WindowStyle = ProcessWindowStyle.Hidden };

            Process.Start(psi);
        }

        static void DeleteFile(string path)
        {
            Console.WriteLine("Delete: " + path);

            var attributes = File.GetAttributes(path);
            if ((attributes & FileAttributes.ReadOnly) != 0)
                File.SetAttributes(path, FileAttributes.Normal);

            File.Delete(path);
        }

        static void DeleteFolder(string path)
        {
            if (path.EndsWith(string.Format(@"CodeReleasePreparer{0}bin", Path.DirectorySeparatorChar),
                              StringComparison.InvariantCultureIgnoreCase))
                return;

            Console.WriteLine("Delete: " + path);

            foreach (var file in Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly))
            {
                DeleteFile(file);
            }

            foreach (var subDirectory in Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly))
            {
                DeleteFolder(subDirectory);
            }

            Directory.Delete(path);
        }

        static string GetRoot()
        {
            if (_root == null)
                _root = Path.GetFullPath("..\\..\\..\\");

            return _root;
        }

        static bool IsValidRootDir()
        {
            var files = Directory.GetFiles(GetRoot(), "*", SearchOption.TopDirectoryOnly);
            return files.Any(x => x.EndsWith("\\NetGore.sln", StringComparison.InvariantCultureIgnoreCase));
        }

        static void Main()
        {
            string[] _deleteFilePatterns = new string[] { @"\.resharper\.user$", @"\.suo$", @"\.cachefile$", @"\.vshost\.exe" };
            string[] _deleteFolderPatterns = new string[] { @"\\.bin$", @"\\bin$", @"\\_resharper", @"\\obj$", @"\\.svn$" };

            _fileRegexes = new RegexCollection(_deleteFilePatterns);
            _folderRegexes = new RegexCollection(_deleteFolderPatterns);

            Console.WriteLine("Root clean directory: " + GetRoot());

            // Validate run dir
            if (!IsValidRootDir())
            {
                Console.WriteLine("This program may only be run from the project's default build path!");
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                return;
            }

            // Delete crap
            RecursiveDelete(GetRoot(), WillDeleteFolder, WillDeleteFile);

            // Create self-destroying batch file that will delete this program's binaries
            string programPath = string.Format("{0}CodeReleasePreparer{1}", GetRoot(), Path.DirectorySeparatorChar);
            AsyncRunBatchFile("CHOICE /c 1 /d 1 /t 2 > nul", "RMDIR /S /Q \"" + programPath + "bin\"",
                              "RMDIR /S /Q \"" + programPath + "obj\"", "DEL %0");

            Console.WriteLine("Done");
        }

        static void RecursiveDelete(string path, Func<string, bool> folderFilter, Func<string, bool> fileFilter)
        {
            var files = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
            foreach (var f in files)
            {
                if (fileFilter(f))
                    DeleteFile(f);
            }

            var folders = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
            foreach (var f in folders)
            {
                if (folderFilter(f))
                    DeleteFolder(f);
                else
                    RecursiveDelete(f, folderFilter, fileFilter);
            }
        }

        static bool WillDeleteFile(string fileName)
        {
            return _fileRegexes.Matches(fileName);
        }

        static bool WillDeleteFolder(string folderName)
        {
            return _folderRegexes.Matches(folderName);
        }
    }

    class RegexCollection
    {
        const RegexOptions _options =
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase;

        readonly Regex[] _regexes;

        public RegexCollection(string[] patterns)
        {
            if (patterns == null)
                throw new ArgumentNullException("patterns");

            _regexes = new Regex[patterns.Length];

            for (int i = 0; i < patterns.Length; i++)
            {
                _regexes[i] = new Regex(patterns[i], _options);
            }
        }

        public bool Matches(string s)
        {
            return _regexes.Any(x => x.IsMatch(s));
        }
    }
}