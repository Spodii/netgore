using System;

namespace InstallationValidator.Tests
{
    public sealed class CheckForMySqlConnector : TestableBase
    {
        const string _testName = "MySQL .NET connector installed";
        const string _description = "Checks that the MySQL .NET connector is installed. This is required by NetGore to create a connection to the MySQL database. This file is included in the root NetGore directory in the folder named '_dependencies'.";
        const string _failMessage = "The MySQL .NET connector is not installed. The installation file can be found in the root NetGore directory in the folder named '_dependencies'. If you have already installed this in the past, you may need to install again as you may not have the correct version installed.";

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckForMySqlConnector"/> class.
        /// </summary>
        public CheckForMySqlConnector()
            : base(_testName, _description)
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
            if (!AssemblyHelper.IsAssemblyInstalled(KnownAssembly.MySqlData))
            {
                errorMessage = _failMessage;
                return false;
            }

            return true;
        }
    }
}