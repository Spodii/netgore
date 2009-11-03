using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NetGore;

namespace InstallationValidator.Tests
{
    public class CheckForXNA : ITestable
    {
        /// <summary>
        /// Runs a test.
        /// </summary>
        public void Test()
        {
            const string testName = "XNA 3.1 installed";
            const string failInfo = "Please install XNA 3.1";

            Tester.Test(testName, AssemblyHelper.IsAssemblyInstalled(KnownAssembly.Xna), failInfo);
        }
    }

    public class ConnectToDatabase : ITestable
    {
        /// <summary>
        /// Runs a test.
        /// </summary>
        public void Test()
        {
            const string testName = "Connect to database";
            MySqlHelper.TestMySqlCommand(testName, null, new string[] { "exit" });
        }
    }

    public class DatabaseFileExists : ITestable
    {
        /// <summary>
        /// Runs a test.
        /// </summary>
        public void Test()
        {
            const string testName = "Database connection settings file exists";
            string failInfo = "Failed to find the database settings file at " + MySqlHelper.DBSettingsFile;

            Tester.Test(testName, File.Exists(MySqlHelper.DBSettingsFile), failInfo);
        }
    }

    public class LoadDbConnectionSettings : ITestable
    {
        /// <summary>
        /// Runs a test.
        /// </summary>
        public void Test()
        {
            const string testName = "Load database connection settings";
            string failInfo = "Failed to load the database connection settings file at " + MySqlHelper.DBSettingsFile;

            try
            {
                MySqlHelper.ConnectionSettings = new DBConnectionSettings(MySqlHelper.DBSettingsFile);
            }
            catch (Exception ex)
            {
                Tester.Test(testName, false, failInfo + ex);
                return;
            }

            Tester.Test(testName, true, failInfo);
        }
    }

    public class LoadMySqlData : ITestable
    {
        /// <summary>
        /// Runs a test.
        /// </summary>
        public void Test()
        {
            const string testName = "Load MySql Connector";
            const string failInfo = "Failed to load the MySql connector assembly";

            Tester.Test(testName, AssemblyHelper.TryLoadAssembly(KnownAssembly.MySqlData) != null, failInfo);
        }
    }

    public class LoadXNA : ITestable
    {
        /// <summary>
        /// Runs a test.
        /// </summary>
        public void Test()
        {
            const string testName = "Load XNA";
            const string failInfo = "Failed to load the XNA assembly";

            Tester.Test(testName, AssemblyHelper.TryLoadAssembly(KnownAssembly.Xna) != null, failInfo);
        }
    }

    public class LocateMySqlExe : ITestable
    {
        /// <summary>
        /// Runs a test.
        /// </summary>
        public void Test()
        {
            const string testName = "Locate mysql.exe";
            const string failInfo = "Failed to find mysql.exe";

            Tester.Test(testName, MySqlHelper.MySqlPath != null, failInfo);
        }
    }

    public class DatabasePopulated : ITestable
    {
        static bool TestInternal(bool print)
        {
            const string testName = "Database populated";

            StringBuilder sb = new StringBuilder();
            foreach (var tableName in MySqlHelper.DBTables)
            {
                sb.Append("`" + tableName + "`,");
            }
            sb.Length--; // Remove trailing comma

            // In the SELECT below, if any of the tables listed don't exist, it will produce the error
            string[] commands = new string[] { "use " + MySqlHelper.ConnectionSettings.SqlDatabase, "SELECT * FROM " + sb + " WHERE 0=1;", "exit", };

            return MySqlHelper.TestMySqlCommand(testName, null, commands, print);
        }

        /// <summary>
        /// Runs a test.
        /// </summary>
        public void Test()
        {
            if (!TestInternal(false))
                MySqlHelper.AskToImportDatabase(true);

            TestInternal(true);
        }
    }

    public class DatabaseExists : ITestable
    {
        static bool TestInternal(bool print)
        {
            const string testName = "Database exists";

            string[] commands = new string[] { "use " + MySqlHelper.ConnectionSettings.SqlDatabase, "exit" };
            return MySqlHelper.TestMySqlCommand(testName, null, commands, print);
        }

        /// <summary>
        /// Runs a test.
        /// </summary>
        public void Test()
        {
            if (!TestInternal(false))
                MySqlHelper.AskToImportDatabase(false);

            TestInternal(true);
        }
    }
}
