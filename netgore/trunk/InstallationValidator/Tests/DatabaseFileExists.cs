using System.IO;

namespace InstallationValidator.Tests
{
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
}