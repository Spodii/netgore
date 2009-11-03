
namespace InstallationValidator.Tests
{
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