namespace InstallationValidator.Tests
{
    public class ConnectToDatabase : ITestable
    {
        #region ITestable Members

        /// <summary>
        /// Runs a test.
        /// </summary>
        public void Test()
        {
            const string testName = "Connect to database";
            MySqlHelper.TestMySqlCommand(testName, null, new string[] { "exit" });
        }

        #endregion
    }
}