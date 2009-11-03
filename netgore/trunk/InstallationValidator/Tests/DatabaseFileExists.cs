using System.IO;

namespace InstallationValidator.Tests
{
    public class DatabaseFileExists : ITestable
    {
        #region ITestable Members

        /// <summary>
        /// Runs a test.
        /// </summary>
        public void Test()
        {
            const string testName = "Database connection settings file exists";
            string failInfo = "Failed to find the database settings file at " + MySqlHelper.DbSettingsFile;

            Tester.Test(testName, File.Exists(MySqlHelper.DbSettingsFile), failInfo);
        }

        #endregion
    }
}