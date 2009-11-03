namespace InstallationValidator.Tests
{
    public class CheckForMySqlConnector : ITestable
    {
        #region ITestable Members

        /// <summary>
        /// Runs a test.
        /// </summary>
        /// <returns>True if the test passed; otherwise false.</returns>
        public void Test()
        {
            const string testName = "MySql Connector/.NET Installed";
            const string failInfo = "Please install the MySql .NET connector included in the \\_dependencies\\ folder";

            Tester.Test(testName, AssemblyHelper.IsAssemblyInstalled(KnownAssembly.MySqlData), failInfo);
        }

        #endregion
    }
}