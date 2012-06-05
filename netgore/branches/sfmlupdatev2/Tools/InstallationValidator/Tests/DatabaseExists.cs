using System.Linq;

namespace InstallationValidator.Tests
{
    public sealed class DatabaseExists : TestableBase
    {
        const string _description =
            "Checks if the database for the game exists. This will only check that the actual database exists, not if it contains the needed data.";

        const string _failMessage =
            "The database does not exist. You have either used a different name by default and did not update the database settings file, or you have not imported the database.";

        const string _testName = "Game database exists";

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseExists"/> class.
        /// </summary>
        public DatabaseExists() : base(_testName, _description)
        {
        }

        /// <summary>
        /// When overridden in the derived class, runs the test.
        /// </summary>
        /// <param name="errorMessage">When the method returns false, contains an error message as to why
        /// the test failed. Otherwise, contains an empty string.</param>
        /// <returns>
        /// True if the test passed; false if the test failed.
        /// </returns>
        protected override bool RunTest(ref string errorMessage)
        {
            string errmsg;

            if (!TestInternal(out errmsg))
                MySqlHelper.AskToImportDatabase(false);

            var success = TestInternal(out errmsg);

            if (!success)
                errorMessage = AppendErrorDetails(_failMessage, errmsg);

            return success;
        }

        static bool TestInternal(out string errmsg)
        {
            var commands = new string[] { "use " + MySqlHelper.ConnectionSettings.Database, "exit" };
            return MySqlHelper.TestMySqlCommand(null, commands, out errmsg);
        }
    }
}