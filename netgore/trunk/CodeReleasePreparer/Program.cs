using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeReleasePreparer
{
    class Program
    {
        static readonly string _mysqldumpPath = FindMySqlFile("mysqldump.exe");
        static readonly string _mysqlPath = FindMySqlFile("mysql.exe");
        static RegexCollection _fileRegexes;
        static RegexCollection _folderRegexes;

        static string _root = null;

        static void DeleteFile(string path)
        {
            if (path.EndsWith("dbdump.bat"))
                return;

            Console.WriteLine("Delete: " + path);

            var attributes = File.GetAttributes(path);
            if ((attributes & FileAttributes.ReadOnly) != 0)
                File.SetAttributes(path, FileAttributes.Normal);

            try
            {
                File.Delete(path);
            }
            catch (UnauthorizedAccessException)
            {
            }
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

        static string FindFile(string fileName, string root)
        {
            if (!Directory.Exists(root))
                return null;

            foreach (var file in Directory.GetFiles(root, "*", SearchOption.AllDirectories))
            {
                if (Path.GetFileName(file).ToLower().EndsWith(fileName))
                    return file;
            }

            return null;
        }

        static string FindMySqlFile(string fileName)
        {
            var filePath =
                FindFile(fileName, Environment.GetEnvironmentVariable("ProgramFiles(x86)") + Path.DirectorySeparatorChar + "MySql") ??
                FindFile(fileName, Environment.GetEnvironmentVariable("ProgramFiles") + Path.DirectorySeparatorChar + "MySql");

            return filePath;
        }

        static string GetRoot()
        {
            if (_root == null)
                _root = Path.GetFullPath("..\\..\\..\\");

            return _root;
        }

        /// <summary>
        /// Checks if the one who is running this program is Spodi.
        /// </summary>
        /// <returns>True if the one who is running this program is probably Spodi; false if it is AN IMPOSTER!!!</returns>
        static bool IsSpodi()
        {
            var drives = DriveInfo.GetDrives();

            // Spodi loves his iRAM! How dare he leave home without it!?
            if (!drives.Any(x => x.Name == "E:\\" && x.VolumeLabel == "iRAM"))
                return false;

            // Dual-core ftw!
            if (Environment.ProcessorCount != 2)
                return false;

            // My PC named its self after me
            if (Environment.MachineName != "SPODI-PC")
                return false;

            // ...or did I name my self after my PC? :o
            if (Environment.UserName != "Spodi")
                return false;

            return true;
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

            // Hmm, spend my time programming, or making ASCII art...
            Console.WriteLine(@"             __          __     _____  _   _ _____ _   _  _____");
            Console.WriteLine(@"             \ \        / /\   |  __ \| \ | |_   _| \ | |/ ____|");
            Console.WriteLine(@"              \ \  /\  / /  \  | |__) |  \| | | | |  \| | |  __ ");
            Console.WriteLine(@"               \ \/  \/ / /\ \ |  _  /| . ` | | | | . ` | | |_ |");
            Console.WriteLine(@"                \  /\  / ____ \| | \ \| |\  |_| |_| |\  | |__| |");
            Console.WriteLine(@"                 \/  \/_/    \_\_|  \_\_| \_|_____|_| \_|\_____|");
            Console.WriteLine();
            Console.WriteLine("                          DO NOT RUN THIS PROGRAM!");
            Console.WriteLine();
            Console.WriteLine(
                "This program is intended to be run ONLY by Spodi for setting up official releases. Running this program WILL alter your database contents and DELETE many files!");
            Console.WriteLine();

            if (!IsSpodi())
            {
                Console.WriteLine(@"                                _.--""""""""""--._");
                Console.WriteLine(@"                              .'             '.");
                Console.WriteLine(@"                             /                 \");
                Console.WriteLine(@"                            ;                   ;");
                Console.WriteLine(@"                            |                   |");
                Console.WriteLine(@"                            |                   |");
                Console.WriteLine(@"                            ;                   ;");
                Console.WriteLine(@"                             \ (`'--,   ,--'`) /");
                Console.WriteLine(@"                              ) )(')/ _ \(')( (");
                Console.WriteLine(@"                             (_ `""""` / \ `""""` _)");
                Console.WriteLine(@"                              \`'-, /   \ ,-'`/");
                Console.WriteLine(@"                               `\ / `'`'` \ /`");
                Console.WriteLine(@"                                | _. ; ; ._ |");
                Console.WriteLine(@"                                |\ '-'-'-' /|");
                Console.WriteLine(@"                                | | _ _ _ | |");
                Console.WriteLine(@"                                 \ '.;_;.' /");
                Console.WriteLine(@"                                  \       /");
                Console.WriteLine(@"                                   ',___,'");
                Console.WriteLine(@"                                    q___p");
                Console.WriteLine(@"                                    q___p");
                Console.WriteLine(@"                                    q___p");
                Console.WriteLine(@"");
                Console.WriteLine("You are not Spodi! Press any key to terminate program...");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Root clean directory: " + GetRoot());

            // Check for the mysql files
            if (string.IsNullOrEmpty(_mysqlPath))
            {
                Console.WriteLine("Failed to find mysql.exe");
                return;
            }

            if (string.IsNullOrEmpty(_mysqldumpPath))
            {
                Console.WriteLine("Failed to find mysqldump.exe");
                return;
            }

            // Validate run dir
            if (!IsValidRootDir())
            {
                Console.WriteLine("This program may only be run from the project's default build path!");
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                return;
            }

            // Clean out the items table in the database
            Console.WriteLine("Cleaning out `item` table...");
            string sout;
            string serr;
            MySqlCommand("--user=root --password= --host=localhost", out sout, out serr,
                         new string[] { "USE demogame;", "DELETE FROM item;", "exit" });

            if (!string.IsNullOrEmpty(sout) || !string.IsNullOrEmpty(serr))
            {
                Console.WriteLine("Error deleting entries in `item` table.");
                if (!string.IsNullOrEmpty(sout))
                    Console.WriteLine("out: " + sout);
                if (!string.IsNullOrEmpty(serr))
                    Console.WriteLine("err: " + serr);
                Console.ReadLine();
                return;
            }

            // Dump database file
            Console.WriteLine("Dumping database to file...");
            const string dbfile = "db.sql";
            if (File.Exists(dbfile))
                File.Delete(dbfile);

            RunBatchFile(false, "mysqldump demogame --user=root --all-tables --routines --create-options > db.sql");

            if (!File.Exists(dbfile) || new FileInfo(dbfile).Length < 1000)
            {
                Console.WriteLine("Failed to dump database to db.sql.");
                Console.ReadLine();
            }

            // Move dump file
            Console.WriteLine("Moving dump file to trunk root...");
            const string dbfileRooted = @"..\..\..\" + dbfile;
            if (File.Exists(dbfileRooted))
                File.Delete(dbfileRooted);

            File.Move(dbfile, dbfileRooted);

            // Delete crap
            RecursiveDelete(GetRoot(), WillDeleteFolder, WillDeleteFile);

            // Create self-destroying batch file that will delete this program's binaries

            string programPath = string.Format("{0}CodeReleasePreparer{1}", GetRoot(), Path.DirectorySeparatorChar);
            RunBatchFile(true, "CHOICE /c 1 /d 1 /t 2 > nul", "RMDIR /S /Q \"" + programPath + "bin\"",
                         "RMDIR /S /Q \"" + programPath + "obj\"", "DEL %0");

            Console.WriteLine("Done");
        }

        /// <summary>
        /// Runs the mysql.exe process.
        /// </summary>
        /// <param name="command">The parameters to use when running the mysql.exe process.</param>
        /// <param name="output">A string of the text mysql.exe sent to the Standard Out stream.</param>
        /// <param name="error">A string of the text mysql.exe sent ot the Standard Error stream.</param>
        /// <param name="cmds">The additional commands to input when running the process.</param>
        static void MySqlCommand(string command, out string output, out string error, params string[] cmds)
        {
            ProcessStartInfo psi = new ProcessStartInfo(_mysqlPath, command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            Process p = new Process { StartInfo = psi };
            p.Start();

            if (cmds != null)
            {
                for (int i = 0; i < cmds.Length; i++)
                {
                    p.StandardInput.WriteLine(cmds[i]);
                }
            }
            p.WaitForExit();

            output = p.StandardOutput.ReadToEnd();
            error = p.StandardError.ReadToEnd();
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

        static void RunBatchFile(bool async, params string[] lines)
        {
            var filePath = Path.GetTempFileName() + ".bat";
            File.WriteAllLines(filePath, lines);
            ProcessStartInfo psi = new ProcessStartInfo(filePath)
            { CreateNoWindow = true, UseShellExecute = true, WindowStyle = ProcessWindowStyle.Hidden };

            var p = Process.Start(psi);

            if (p == null)
            {
                Console.WriteLine("Failed to run batch file!");
                Console.WriteLine("Batch file content:");
                if (lines == null || lines.Length == 0)
                    Console.WriteLine("(empty)");
                else
                {
                    for (int i = 0; i < lines.Length; i++)
                        Console.WriteLine(lines[i]);
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
                return;
            }

            if (!async)
                p.WaitForExit();
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