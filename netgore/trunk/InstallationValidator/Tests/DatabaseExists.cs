namespace InstallationValidator.Tests
{
    public class DatabaseExists : ITestable
    {
        static bool TestInternal(bool print)
        {
            const string testName = "Database exists";

            string[] commands = new string[] { "use " + MySqlHelper.ConnectionSettings.Database, "exit" };
            return MySqlHelper.TestMySqlCommand(testName, null, commands, print);
        }

        #region ITestable Members

        /// <summary>
        /// Runs a test.
        /// </summary>
        public void Test()
        {
            if (!TestInternal(false))
                MySqlHelper.AskToImportDatabase(false);

            TestInternal(true);
        }

        #endregion
    }
}