namespace InstallationValidator.Tests
{
    public sealed class ConnectToDatabase : TestableBase
    {
        const string _testName = "Connect to MySQL";
        const string _description = "Checks that a connection can be made to the MySQL host service. This only checks if the connection can be made to the service.";
        const string _failMessage = "Could not connect to the MySQL host service. This indicates that either:\n1. You do not have MySQL installed.\n2. The MySQL service is not running.\n3. A firewall is blocking connecting to the MySQL service.\n\nTo resolve these issues:\n1. Install MySQL (see the NetGore Setup Guide).\n2. The service is not running. You can start the service by first opening the Services window (either type 'services' into the start-menu search in Windows Vista or 7, or navigate to it by opening Explorer, right-clicking Computer, select Manage, then select Services). In the list of services, look for the one with the name 'MySQL' or something similar, right-click, and click Start.\n3. Disable any firewalls you have temporarily and try again.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectToDatabase"/> class.
        /// </summary>
        public ConnectToDatabase()
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
            string errmsg;
            bool success = MySqlHelper.TestMySqlCommand(null, new string[] { "exit" }, out errmsg);

            if (!success)
            {
                errorMessage = AppendErrorDetails(_failMessage, errmsg);
                return false;
            }

            return true;
        }
    }
}