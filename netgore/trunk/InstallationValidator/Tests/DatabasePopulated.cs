using System.Text;

namespace InstallationValidator.Tests
{
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
            string[] commands = new string[]
            { "use " + MySqlHelper.ConnectionSettings.Database, "SELECT * FROM " + sb + " WHERE 0=1;", "exit", };

            return MySqlHelper.TestMySqlCommand(testName, null, commands, print);
        }

        #region ITestable Members

        /// <summary>
        /// Runs a test.
        /// </summary>
        public void Test()
        {
            if (!TestInternal(false))
                MySqlHelper.AskToImportDatabase(true);

            TestInternal(true);
        }

        #endregion
    }
}