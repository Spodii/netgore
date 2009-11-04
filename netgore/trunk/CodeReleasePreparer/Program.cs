using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using InstallationValidator;

// ReSharper disable ConditionIsAlwaysTrueOrFalse
#pragma warning disable 162

namespace CodeReleasePreparer
{
    class Program
    {
        /// <summary>
        /// If true, ONLY the database schema part will be built. Otherwise, a complete clean will be done.
        /// </summary>
        const bool _buildSchemaOnly = false;

        static readonly string _mysqldumpPath = MySqlHelper.FindMySqlFile("mysqldump.exe");
        static readonly string _mysqlPath = MySqlHelper.FindMySqlFile("mysql.exe");
        static RegexCollection _fileRegexes;
        static RegexCollection _folderRegexes;

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
            var files = Directory.GetFiles(Paths.Root, "*", SearchOption.TopDirectoryOnly);
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

            Console.WriteLine("Root clean directory: " + Paths.Root);

            // Check for the mysql files
            if (string.IsNullOrEmpty(_mysqlPath))
            {
                Console.WriteLine("Failed to find mysql.exe");
                Console.WriteLine("Press any key to exit...");
                return;
            }

            if (string.IsNullOrEmpty(_mysqldumpPath))
            {
                Console.WriteLine("Failed to find mysqldump.exe");
                Console.WriteLine("Press any key to exit...");
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

            // Save the schema file
            Console.WriteLine("Updating the database schema file...");
            string schemaFile = Paths.Root + MySqlHelper.DbSchemaFile;
            if (File.Exists(schemaFile))
                File.Delete(schemaFile);

            SchemaSaver.Save();

            if (!File.Exists(schemaFile))
            {
                Console.WriteLine("Failed to create database schema file! Path: {0}", schemaFile);
                Console.WriteLine("Press any key to exit...");
                Console.ReadLine();
                return;
            }

            if (_buildSchemaOnly)
            {
                Console.WriteLine("_buildSchemaOnly is set - will not progress any farther.");
                Console.WriteLine("Done!");
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
            string dbfileRooted = string.Format("..{0}..{0}..{0}", Path.DirectorySeparatorChar) + dbfile;
            if (File.Exists(dbfileRooted))
                File.Delete(dbfileRooted);

            File.Move(dbfile, dbfileRooted);

            // Remove version control references
            Console.WriteLine("Deleting version control references...");
            VersionControlCleaner.Run(Paths.Root);

            // Delete crap
            Console.WriteLine("Deleting unneeded files/folders...");
            Deleter.RecursiveDelete(Paths.Root, WillDeleteFolder, WillDeleteFile);

            // Create self-destroying batch file that will delete this program's binaries
            Console.WriteLine("Creating self-destruct batch file...");
            string programPath = string.Format("{0}CodeReleasePreparer{1}", Paths.Root, Path.DirectorySeparatorChar);
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
                    {
                        Console.WriteLine(lines[i]);
                    }
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
}