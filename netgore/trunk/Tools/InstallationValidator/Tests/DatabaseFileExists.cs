using System.IO;
using System.Linq;

namespace InstallationValidator.Tests
{
    public sealed class DatabaseFileExists : TestableBase
    {
        const string _description =
            "Checks that the database connection settings file exists. This file is vital to determining how to make the connection to the database.";

        const string _failMessage =
            "The database settings file does not exist at the path expected path:\n{0}\n\nThe file should be located at this path by default. If you moved it, you should probably move it back.";

        const string _testName = "Database settings file";

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseFileExists"/> class.
        /// </summary>
        public DatabaseFileExists() : base(_testName, _description)
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
            var success = File.Exists(MySqlHelper.DbSettingsFile);

            if (!success)
                errorMessage = string.Format(_failMessage, MySqlHelper.DbSettingsFile);

            return success;
        }
    }
}