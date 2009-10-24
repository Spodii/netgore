using System;
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

        static string GetRoot()
        {
            if (_root == null)
                _root = Path.GetFullPath("..\\..\\..\\");

            return _root;
        }

        static void Main()
        {
            string[] _deleteFilePatterns = new string[] { @"\\.resharper.user$", @"\\.suo$", @"\.cachefile$" };
            string[] _deleteFolderPatterns = new string[] { @"\\.bin$", @"\\_resharper", @"\\obj$", @"\\.svn$" };

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

            RecursiveDelete(GetRoot(), WillDeleteFolder, WillDeleteFile);

            Console.ReadLine();
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

        static void DeleteFile(string path)
        {
            Console.WriteLine("F: " + path);
            File.Delete(path);
        }

        static void DeleteFolder(string path)
        {
            Console.WriteLine("D: " + path);
            Directory.Delete(path);
        }

        static bool IsValidRootDir()
        {
            var files = Directory.GetFiles(GetRoot(), "*", SearchOption.TopDirectoryOnly);
            return files.Any(x => x.EndsWith("\\NetGore.sln", StringComparison.InvariantCultureIgnoreCase));
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