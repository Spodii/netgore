using System;
using System.Linq;
using NetGore.Db;

namespace InstallationValidator.Tests
{
    public sealed class LoadDbConnectionSettings : TestableBase
    {
        const string _description =
            "Attempts to load the database connection settings file. The file may exist, but it may be formatted in an invalid way or be missing data, resulting in the engine not being able to read it. It is vital that the engine can load the database connection settings correctly, otherwise it won't be able to know how to connect to the database.";

        const string _failMessage = "Failed to load the database connection settings file from path:\n {0}";
        const string _testName = "Load database settings";

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadDbConnectionSettings"/> class.
        /// </summary>
        public LoadDbConnectionSettings() : base(_testName, _description)
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
            try
            {
                MySqlHelper.ConnectionSettings = new DbConnectionSettings(MySqlHelper.DbSettingsFile, true);
            }
            catch (Exception ex)
            {
                errorMessage = AppendErrorDetails(string.Format(_failMessage, MySqlHelper.DbSettingsFile), ex.ToString());
                return false;
            }

            return true;
        }
    }
}