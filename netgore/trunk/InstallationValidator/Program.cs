using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using NetGore;

namespace InstallationValidator
{
    class Program
    {
        const string _dbSettingsFile = "DemoGame.ServerObjs\\DbSettings.xml";

        const string _mysqlDataAssembly =
            "MySql.Data, Version=6.1.0.0, Culture=neutral, PublicKeyToken=c5687fc88969c44d, processorArchitecture=MSIL";

        const string _xnaAssembly =
            "Microsoft.Xna.Framework, Version=3.1.0.0, Culture=neutral, PublicKeyToken=6d5c3888ef60e27d, processorArchitecture=x86";

        static readonly string _mysqlPath = FindMySqlFile();
        static DBConnectionSettings _connectionSettings;
        static bool _hasErrors = false;

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
            var filePath = FindFile(file, "C:\\Program Files\\MySql") ?? FindFile(file, "C:\\Program Files (x86)\\MySql");
            return filePath;
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
                Write("One or more errors were found. Resolve them for NetGore to work properly.", ConsoleColor.Red);
            else
                Write("Congratulations, no errors!", ConsoleColor.Green);

            Console.ReadLine();
        }

        static bool MySqlCommand(string command, out string output, out string error, params string[] cmds)
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
            try
            {
                p.Start();
            }
            catch (Win32Exception)
            {
                output = string.Empty;
                error = string.Empty;
                return false;
            }
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

            return true;
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

            string username = _connectionSettings.SqlUser;
            string password = _connectionSettings.SqlPass;
            string host = _connectionSettings.SqlHost;

            string output;
            string error;
            if (
                !MySqlCommand(string.Format("--user={0} --password={1} --host=a{2}", username, password, host), out output,
                              out error, "exit") || error.ToLower().Contains("access denied"))
            {
                const string failInfo = "Acess denied for user:`{0}` password:`{1}`";
                Test(testName, false, string.Format(failInfo, username, password));
                return;
            }
            else if (error.ToLower().Contains("error 2005"))
            {
                const string failInfo =
                    "Unknown service host `{0}`. Make sure the host is correct and that the MySql server service has been started.";
                Test(testName, false, string.Format(failInfo, host));
                return;
            }

            Test(testName, true, null);
        }

        static void TestDatabaseFileExists()
        {
            const string testName = "Database connection settings file exists";
            const string failInfo = "Failed to find the database settings file at " + _dbSettingsFile;

            Test(testName, File.Exists(_dbSettingsFile), failInfo);
        }

        static void TestLoadDbConnectionSettings()
        {
            const string testName = "Load database connection settings";
            const string failInfo = "Failed to load the database connection settings file at " + _dbSettingsFile + ".\n";

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

        static void TestMySqlPath()
        {
            const string testName = "Locate mysql.exe";
            const string failInfo = "Failed to find mysql.exe";

            Test(testName, _mysqlPath != null, failInfo);
        }

        static void TestSelectTable()
        {
            const string testName = "Database populated";
            const string failInfo =
                "Database `{0}` is not populated. Make sure you import the db.sql file. See http://netgore.com/wiki/setup-guide.html for more information.";

            string username = _connectionSettings.SqlUser;
            string password = _connectionSettings.SqlPass;
            string host = _connectionSettings.SqlHost;
            string db = _connectionSettings.SqlDatabase;

            string output;
            string error;
            if (
                !MySqlCommand(string.Format("--user={0} --password={1} --host={2}", username, password, host), out output,
                              out error, string.Format("use {0}", db), "SELECT * FROM character;", "SELECT * FROM alliance;",
                              "SELECT * FROM account;", "SELECT * FROM item;", "SELECT * FROM map;", "SELECT * FROM shop;",
                              "SELECT * FROM character_template;", "exit") || error.ToLower().Contains("error 1146"))
            {
                Test(testName, false, string.Format(failInfo, db));
                return;
            }

            Test(testName, true, null);
        }

        static void TestUseDatabase()
        {
            const string testName = "Database exists";
            const string failInfo = "Database `{0}` does not exist or could not be used";

            string username = _connectionSettings.SqlUser;
            string password = _connectionSettings.SqlPass;
            string host = _connectionSettings.SqlHost;
            string db = _connectionSettings.SqlDatabase;

            string output;
            string error;
            if (
                !MySqlCommand(string.Format("--user={0} --password={1} --host={2}", username, password, host), out output,
                              out error, string.Format("use {0}", db), "exit") || error.ToLower().Contains("unknown database"))
            {
                Test(testName, false, string.Format(failInfo, db));
                return;
            }

            Test(testName, true, null);
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