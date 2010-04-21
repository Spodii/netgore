using System.Linq;

namespace InstallationValidator.Tests
{
    public sealed class LocateMySqlExe : TestableBase
    {
        const string _description =
            "Finds the location of the mysql.exe application, which is what the Installation Validator uses to run the MySql tests without having to use the MySql .NET connector.";

        const string _failMessage =
            "The mysql.exe file could not be located. When the Installation Validator started, a prompt should have come up asking you the location of this file. Somehow, you seem to have bypassed this prompt, or the file has been moved/renamed since this program started. Please restart this program to select the location of mysql.exe again.";

        const string _testName = "Locate mysql.exe";

        /// <summary>
        /// Initializes a new instance of the <see cref="LocateMySqlExe"/> class.
        /// </summary>
        public LocateMySqlExe() : base(_testName, _description)
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
            var success = MySqlHelper.MySqlPath != null;

            if (!success)
                errorMessage = _failMessage;

            return success;
        }
    }
}