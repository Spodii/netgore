namespace InstallationValidator.Tests
{
    public class LoadMySqlData : ITestable
    {
        #region ITestable Members

        /// <summary>
        /// Runs a test.
        /// </summary>
        public void Test()
        {
            const string testName = "Load MySql Connector";
            const string failInfo = "Failed to load the MySql connector assembly";

            Tester.Test(testName, AssemblyHelper.TryLoadAssembly(KnownAssembly.MySqlData) != null, failInfo);
        }

        #endregion
    }
}