using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace InstallationValidator.Tests
{
    public sealed class ConnectToDatabase : TestableBase
    {
        const string _description =
            "Checks that a connection can be made to the MySQL host service. This only checks if the connection can be made to the service.";

        const string _failMessage =
            "Could not connect to the MySQL host service. This indicates that either:\n1. You do not have MySQL installed.\n2. The MySQL service is not running.\n3. A firewall is blocking connecting to the MySQL service.\n\nTo resolve these issues:\n1. Install MySQL (see the NetGore Setup Guide).\n2. The service is not running. You can start the service by first opening the Services window (either type 'services' into the start-menu search in Windows Vista or 7, or navigate to it by opening Explorer, right-clicking Computer, select Manage, then select Services). In the list of services, look for the one with the name 'MySQL' or something similar, right-click, and click Start.\n3. Disable any firewalls you have temporarily and try again.";

        const string _failMessageInvalidLogin =
            "Invalid MySQL account username/password. Please make sure you have entered the correct MySQL username and password into the DbSettings.dat file. Usually, the username will be \"root\" and the password will either be blank, or whatever you entered when setting up the MySQL service.\n\nIf you forgot your username or password, you can reinstall the MySQL service and enter these values again by running MySQLInstanceConfig.exe, which should be located in the \\bin\\ directory you installed MySQL:\n\n{0}";

        const string _testName = "Connect to MySQL";

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectToDatabase"/> class.
        /// </summary>
        public ConnectToDatabase() : base(_testName, _description)
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
            var success = MySqlHelper.TestMySqlCommand(null, new string[] { "exit" }, out errmsg);

            if (!success)
            {
                if (errmsg.Contains("Access denied. Ensure the connection string contains valid account info"))
                {
                    var instanceConfigPath = string.Empty;
                    try
                    {
                        var rootBinDir = Directory.GetParent(MySqlHelper.MySqlPath);
                        instanceConfigPath = rootBinDir.FullName + Path.DirectorySeparatorChar + "MySQLInstanceConfig.exe";
                    }
                    catch (NullReferenceException ex)
                    {
                        Debug.Fail(ex.ToString());
                    }
                    catch (IOException ex)
                    {
                        Debug.Fail(ex.ToString());
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Debug.Fail(ex.ToString());
                    }
                    catch (ArgumentException ex)
                    {
                        Debug.Fail(ex.ToString());
                    }

                    var s = string.Format(_failMessageInvalidLogin, instanceConfigPath);
                    errorMessage = AppendErrorDetails(s, errmsg);
                }
                else
                    errorMessage = AppendErrorDetails(_failMessage, errmsg);

                return false;
            }

            return true;
        }
    }
}