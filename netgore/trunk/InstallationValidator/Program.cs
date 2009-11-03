using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using NetGore;

namespace InstallationValidator
{
    class Program
    {
        const string _dbSqlFile = "db.sql";

        const string _mysqlDataAssembly =
            "MySql.Data, Version=6.1.0.0, Culture=neutral, PublicKeyToken=c5687fc88969c44d, processorArchitecture=MSIL";

        const string _xnaAssembly =
            "Microsoft.Xna.Framework, Version=3.1.0.0, Culture=neutral, PublicKeyToken=6d5c3888ef60e27d, processorArchitecture=x86";

        static readonly string _dbSettingsFile = string.Format("DemoGame.ServerObjs{0}DbSettings.xml", Path.DirectorySeparatorChar);

        static readonly string[] _dbTables = new string[]
        {
            "account", "alliance", "alliance_attackable", "alliance_hostile", "character", "character_equipped",
            "character_inventory", "character_status_effect", "character_template", "character_template_equipped",
            "character_template_inventory", "game_constant", "item", "item_template", "map", "map_spawn", "server_setting",
            "server_time", "shop", "shop_item"
        };

        static DBConnectionSettings _connectionSettings;
        static bool _hasErrors = false;
        static string _mysqlPath = FindMySqlFile();

        /// <summary>
        /// Allow the user to create the database and import the values.
        /// </summary>
        static void AskToImportDatabase(bool dbExists)
        {
            string s;
            if (!dbExists)
                s = "Database `{0}` does not exist. Do you wish to create it (Y/N)?";
            else
                s = "Database `{0}` is not populated. Do you wish to populate it (Y/N)?";

            Write(string.Format("\n" + s + "\n", _connectionSettings.SqlDatabase), ConsoleColor.Green);
            Write("Additional information:\n", ConsoleColor.Yellow);
            Write("1. You MUST create the database and import the data at some point.\n", ConsoleColor.White);
            Write("2. You can always re-run this program and create the database later.\n", ConsoleColor.White);
            Write("3. Expert users might want to create the database manually.\n", ConsoleColor.White);
            Write("4. Since the database is empty, no data will be lost, so 'Y' is recommended.\n", ConsoleColor.White);

            // Ask if they want to import the database
            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.N)
                    break;
                if (key.Key == ConsoleKey.Y)
                {
                    ImportDatabaseContents();
                    break;
                }
            }
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

        static string FindMySqlFile()
        {
            const string file = "mysql.exe";

            var filePath =
                FindFile(file, Environment.GetEnvironmentVariable("ProgramFiles(x86)") + Path.DirectorySeparatorChar + "MySql") ??
                FindFile(file, Environment.GetEnvironmentVariable("ProgramFiles") + Path.DirectorySeparatorChar + "MySql");

            return filePath;
        }

        static string GetMySqlCommandErrorStr(string mysqlOut, string mysqlError, string errmsg, params string[] p)
        {
            const string div1 = "=====";
            const string div2 = "===";

            string err;
            if (p != null && p.Length > 0)
                err = string.Format(errmsg, p);
            else
                err = errmsg;

            string errDetails = string.Empty;

            if (mysqlOut != null)
            {
                mysqlOut = mysqlOut.Trim();
                if (mysqlOut.Length > 0)
                    errDetails += string.Format("{0}{1} mysql.exe output {1}{0}{2}", "\n", div2, mysqlOut);
            }

            if (mysqlError != null)
            {
                mysqlError = mysqlError.Trim();
                if (mysqlError.Length > 0)
                    errDetails += string.Format("{0}{1} mysql.exe error output {1}{0}{2}", "\n", div2, mysqlError);
            }

            if (errDetails.Length > 0)
            {
                err += string.Format("{0}{0}{1} ERROR DETAILS {1}{0}", "\n", div1);
                err += errDetails;
            }

            return err + "\n";
        }

        static void ImportDatabaseContents()
        {
            string dbFile = Path.GetFullPath(_dbSqlFile);
            bool fileExists = File.Exists(dbFile);

            Test("db.sql file exists", fileExists,
                 "Failed to find the db.sql file. Make sure you didn't move/delete it, or run this program from a different directory than the default.");

            if (!fileExists)
                return;

            const string testName = "Create database and import data";

            string[] commands = new string[]
            {
                "DROP DATABASE IF EXISTS " + _connectionSettings.SqlDatabase + ";",
                "CREATE DATABASE " + _connectionSettings.SqlDatabase + ";", 
                "USE " + _connectionSettings.SqlDatabase + ";",
                "SOURCE " + dbFile, 
                "exit"
            };

            TestMySqlCommand(testName, null, commands);
        }

        static bool IsAssemblyInstalled(string assemblyName)
        {
            try
            {
                var asm = Assembly.ReflectionOnlyLoad(assemblyName);
                return asm != null;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }

        static void Main()
        {
            if (string.IsNullOrEmpty(_mysqlPath) || !File.Exists(_mysqlPath))
            {
                Console.WriteLine("Failed to automatically find the path to mysql.exe.");
                Console.WriteLine(
                    "Please enter the path to mysql.exe, which should be located in your MySQL installation directory under the sub-directory \\bin\\.");
                Console.WriteLine(@"Example: C:\Program Files\MySQL\MySQL Server 5.1\bin\mysql.exe");
                while (true)
                {
                    Console.WriteLine();
                    _mysqlPath = (Console.ReadLine() ?? string.Empty).Trim();
                    if (_mysqlPath != null)
                    {
                        if (_mysqlPath.Length > 2 && File.Exists(_mysqlPath))
                            break;
                        if (_mysqlPath.Equals("exit", StringComparison.OrdinalIgnoreCase))
                            return;
                    }
                    Console.WriteLine("Invalid file path. Please enter the path to mysql.exe ('exit' to abort).");
                }
            }

            TestHandler[] tests = new TestHandler[]
            {
                TestMySqlPath, TestCheckForXNA, TestLoadXNA, TestCheckForMySqlConnector, TestLoadMySqlData, TestDatabaseFileExists,
                TestLoadDbConnectionSettings, TestConnectToDatabase, TestUseDatabase, TestSelectTable
            };

            foreach (var test in tests)
            {
                test();
            }

            Console.WriteLine();

            if (_hasErrors)
            {
                Write("One or more errors were found. Resolve them for NetGore to work properly.\n", ConsoleColor.Red);
                Write("If you need help resolving these errors, please ask on the NetGore forums at www.netgore.com.",
                      ConsoleColor.Yellow);
            }
            else
                Write("Congratulations, no errors! :)", ConsoleColor.Green);

            Console.ReadLine();
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

        static void Test(string testName, bool passed, string failMessage)
        {
            if (!passed)
                _hasErrors = true;

            const ConsoleColor normalColor = ConsoleColor.White;
            const ConsoleColor passColor = ConsoleColor.Green;
            const ConsoleColor failColor = ConsoleColor.Red;
            const ConsoleColor testColor = ConsoleColor.Yellow;
            const ConsoleColor msgColor = ConsoleColor.Red;

            if (Console.CursorLeft > 0)
                Write("\n", normalColor);

            Write("[", normalColor);
            Write(passed ? "PASS" : "FAIL", passed ? passColor : failColor);
            Write("] ", normalColor);

            Write(testName, testColor);

            if (!passed && !string.IsNullOrEmpty(failMessage))
            {
                Write(" - ", normalColor);
                Write(failMessage, msgColor);
            }

            Console.WriteLine();
        }

        static void TestCheckForMySqlConnector()
        {
            const string testName = "MySql Connector/.NET Installed";
            const string failInfo = "Please install the MySql .NET connector included in the \\_dependencies\\ folder";

            Test(testName, IsAssemblyInstalled(_mysqlDataAssembly), failInfo);
        }

        static void TestCheckForXNA()
        {
            const string testName = "XNA 3.1 installed";
            const string failInfo = "Please install XNA 3.1";

            Test(testName, IsAssemblyInstalled(_xnaAssembly), failInfo);
        }

        static void TestConnectToDatabase()
        {
            const string testName = "Connect to database";
            TestMySqlCommand(testName, null, new string[] { "exit" });
        }

        static void TestDatabaseFileExists()
        {
            const string testName = "Database connection settings file exists";
            string failInfo = "Failed to find the database settings file at " + _dbSettingsFile;

            Test(testName, File.Exists(_dbSettingsFile), failInfo);
        }

        static void TestLoadDbConnectionSettings()
        {
            const string testName = "Load database connection settings";
            string failInfo = "Failed to load the database connection settings file at " + _dbSettingsFile;

            try
            {
                _connectionSettings = new DBConnectionSettings(_dbSettingsFile);
            }
            catch (Exception ex)
            {
                Test(testName, false, failInfo + ex);
                return;
            }

            Test(testName, true, failInfo);
        }

        static void TestLoadMySqlData()
        {
            const string testName = "Load MySql Connector";
            const string failInfo = "Failed to load the MySql connector assembly";

            Test(testName, TryLoadAssembly(_mysqlDataAssembly) != null, failInfo);
        }

        static void TestLoadXNA()
        {
            const string testName = "Load XNA";
            const string failInfo = "Failed to load the XNA assembly";

            Test(testName, TryLoadAssembly(_xnaAssembly) != null, failInfo);
        }

        /// <summary>
        /// Provides a wrapper for a test that does a test on the MySql.exe process. This will call Test()
        /// and perform internal checks for known errors. All expected errors should be defined in here.
        /// </summary>
        /// <param name="testName">The name of the test.</param>
        /// <param name="command">The primary command (command line command). Leave null or empty to use
        /// the default connection string.</param>
        /// <param name="cmds">The additional commands to execute.</param>
        /// <returns>True if the test was successful; false if the test failed.</returns>
        static bool TestMySqlCommand(string testName, string command, string[] cmds)
        {
            return TestMySqlCommand(testName, command, cmds, true);
        }

        /// <summary>
        /// Provides a wrapper for a test that does a test on the MySql.exe process. This will call Test()
        /// and perform internal checks for known errors. All expected errors should be defined in here.
        /// </summary>
        /// <param name="testName">The name of the test.</param>
        /// <param name="command">The primary command (command line command). Leave null or empty to use
        /// the default connection string.</param>
        /// <param name="cmds">The additional commands to execute.</param>
        /// <param name="print">If true, the results will be printed through <see cref="Test"/>. If false, the
        /// results will not be displayed. Set this to false if you want to run a test without having the
        /// results be displayed.</param>
        /// <returns>True if the test was successful; false if the test failed.</returns>
        static bool TestMySqlCommand(string testName, string command, string[] cmds, bool print)
        {
            if (testName == null)
                testName = "<Unnamed Test>";

            // Check for a valid application path
            if (string.IsNullOrEmpty(_mysqlPath) || !File.Exists(_mysqlPath))
            {
                string errmsg = "Unknown or invalid path to mysql.exe. Cannot run command.";
                if (!string.IsNullOrEmpty(_mysqlPath))
                    errmsg += " Supplied path: " + _mysqlPath;

                if (print)
                    Test(testName, false, errmsg);
                return false;
            }

            // Build the default command string
            if (string.IsNullOrEmpty(command))
            {
                string username = _connectionSettings.SqlUser;
                string password = _connectionSettings.SqlPass;
                string host = _connectionSettings.SqlHost;
                command = string.Format("--user={0} --password={1} --host={2}", username, password, host);
            }

            // Run the mysql.exe process
            string output;
            string error;

            try
            {
                MySqlCommand(command, out output, out error, cmds);
            }
            catch (Exception ex)
            {
                // Display any exceptions found from trying to run the process
                // Generally this means that the process itself crashed or completely failed to run
                const string failInfo = "Failed to launch mysql.exe. Internal error: {0}";

                if (print)
                    Test(testName, false, string.Format(failInfo, ex));
                return false;
            }

            // Check for a variety of failure messages from the mysql.exe process
            if (error != null)
            {
                string errorLower = error.ToLower();
                if (errorLower.Contains("access denied"))
                {
                    const string failInfo = "Access denied. Ensure the connection string contains valid account info: `{0}`";
                    string err = GetMySqlCommandErrorStr(output, error, failInfo, command);
                    if (print)
                        Test(testName, false, err);
                    return false;
                }
                else if (errorLower.Contains("error 2005"))
                {
                    const string failInfo =
                        "Unknown service host `{0}`. Make sure the host used in the connection string is correct" +
                        " and that the MySql server service has been started: `{0}`";
                    string err = GetMySqlCommandErrorStr(output, error, failInfo, command);
                    if (print)
                        Test(testName, false, err);
                    return false;
                }
                else if (errorLower.Contains("error 1146"))
                {
                    const string failInfo =
                        "The target database is not populated. Make sure you import the db.sql file." +
                        " See http://netgore.com/wiki/setup-guide.html for more information.";
                    string err = GetMySqlCommandErrorStr(output, error, failInfo);
                    if (print)
                        Test(testName, false, err);
                    return false;
                }
                else if (errorLower.Contains("error 1049"))
                {
                    const string failInfo =
                        "Specified database does not exist. Make sure you create the MySql database" +
                        " and import the db.sql file. If you use a database name other than" +
                        " `demogame`, make sure you updated the database settings file located at `{0}`." +
                        " See http://netgore.com/wiki/setup-guide.html for more information.";
                    string err = GetMySqlCommandErrorStr(output, error, failInfo, _dbSettingsFile);
                    if (print)
                        Test(testName, false, err);
                    return false;
                }
                else if (errorLower.Contains("failed to open file"))
                {
                    const string failInfo =
                        "Failed to open the db.sql file. Please make sure it is in the default location." +
                        " If you continue to get this message, you may have to manually import the database's contents";
                    string err = GetMySqlCommandErrorStr(output, error, failInfo, _dbSettingsFile);
                    if (print)
                        Test(testName, false, err);
                    return false;
                }
            }

            // Successful
            if (print)
                Test(testName, true, null);
            return true;
        }

        static void TestMySqlPath()
        {
            const string testName = "Locate mysql.exe";
            const string failInfo = "Failed to find mysql.exe";

            Test(testName, _mysqlPath != null, failInfo);
        }

        static void TestSelectTable()
        {
            if (!TestSelectTableInternal(false))
                AskToImportDatabase(true);

            TestSelectTableInternal(true);
        }

        static bool TestSelectTableInternal(bool print)
        {
            const string testName = "Database populated";

            StringBuilder sb = new StringBuilder();
            foreach (var tableName in _dbTables)
            {
                sb.Append("`" + tableName + "`,");
            }
            sb.Length--; // Remove trailing comma

            // In the SELECT below, if any of the tables listed don't exist, it will produce the error
            string[] commands = new string[]
            { "use " + _connectionSettings.SqlDatabase, "SELECT * FROM " + sb + " WHERE 0=1;", "exit", };

            return TestMySqlCommand(testName, null, commands, print);
        }

        static void TestUseDatabase()
        {
            if (!TestUseDatabaseInternal(false))
                AskToImportDatabase(false);

            TestUseDatabaseInternal(true);
        }

        static bool TestUseDatabaseInternal(bool print)
        {
            const string testName = "Database exists";

            string[] commands = new string[] { "use " + _connectionSettings.SqlDatabase, "exit" };
            return TestMySqlCommand(testName, null, commands, print);
        }

        static Assembly TryLoadAssembly(string assemblyName)
        {
            try
            {
                return Assembly.ReflectionOnlyLoad(assemblyName);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        static void Write(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(msg);
        }

        delegate void TestHandler();
    }
}