namespace InstallationValidator.Tests
{
    public class LocateMySqlExe : ITestable
    {
        #region ITestable Members

        /// <summary>
        /// Runs a test.
        /// </summary>
        public void Test()
        {
            const string testName = "Locate mysql.exe";
            const string failInfo = "Failed to find mysql.exe";

            Tester.Test(testName, MySqlHelper.MySqlPath != null, failInfo);
        }

        #endregion
    }
}