using System;
using NetGore;

namespace InstallationValidator.Tests
{
    public class LoadDbConnectionSettings : ITestable
    {
        #region ITestable Members

        /// <summary>
        /// Runs a test.
        /// </summary>
        public void Test()
        {
            const string testName = "Load database connection settings";
            string failInfo = "Failed to load the database connection settings file at " + MySqlHelper.DbSettingsFile;

            try
            {
                MySqlHelper.ConnectionSettings = new DBConnectionSettings(MySqlHelper.DbSettingsFile);
            }
            catch (Exception ex)
            {
                Tester.Test(testName, false, failInfo + ex);
                return;
            }

            Tester.Test(testName, true, failInfo);
        }

        #endregion
    }
}