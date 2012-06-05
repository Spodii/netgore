using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace InstallationValidator.Tests
{
    public sealed class DatabaseVersion : TestableBase
    {
        const string _description =
            "Checks to make sure your database is of a supported version." + " NetGore requires " + _minVersionName + " or later." +
            " If you do not have " + _minVersionName +
            " or later, you will encounter errors when trying to import the database dump file (db.sql).";

        const string _failMessage =
            "Your MySQL database version {0}. Please download the latest version of MySQL" +
            " from the MySQL website at:\n\nhttp://dev.mysql.com/downloads/mysql/" +
            "\n\nADDITIONAL INFO: If you know you have already installed " + _minVersionName +
            " or later, the issue is likely that you have more than one instance of MySQL installed." +
            " A common case of this is if you are using WAMP or any similar web server package that includes MySQL." +
            " If you do not use the web server package anymore, uninstalling it can fix this issue since it will" +
            " also uninstall the older version of MySQL. You may need to follow this up by reinstalling the newer version of MySQL, too." +
            " If you do not know how to stop and start services, you can either Google it, or ask for assistance in the NetGore forums.";

        const string _minVersionName = "MySQL 5.1.38";

        const string _testName = "Database version";

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseVersion"/> class.
        /// </summary>
        public DatabaseVersion() : base(_testName, _description)
        {
        }

        static int IsValidVersion(int major, int minor, int revision)
        {
            if (major > 5)
                return 0;

            if (minor > 1)
                return 0;

            if (revision >= 38)
                return 0;

            return 1;
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
            var cmds = new string[] { "SELECT version();", "exit" };

            string output;
            string error;
            MySqlHelper.MySqlCommand(null, out output, out error, cmds);

            if (!string.IsNullOrEmpty(error))
            {
                errorMessage = "Failed to acquire the MySQL version. Reason: " + error;
                return false;
            }

            var reg = new Regex(@"(?<Major>\d+)\.(?<Minor>\d+)\.(?<Revision>\d+)",
                RegexOptions.CultureInvariant);

            var success = -1;

            var m = reg.Match(output);
            if (m.Success)
            {
                int major;
                int minor;
                int revision;
                if (int.TryParse(m.Groups["Major"].Value, out major) && int.TryParse(m.Groups["Minor"].Value, out minor) &&
                    int.TryParse(m.Groups["Revision"].Value, out revision))
                    success = IsValidVersion(major, minor, revision);
            }

            var foundVersion = string.IsNullOrEmpty(output)
                                   ? "[UNKNOWN]" : output.Replace("\r", "").Replace("\n", "").Replace("version()", "");
            switch (success)
            {
                case 0:
                    return true;

                case 1:
                    errorMessage = string.Format("({0}) is not supported and should be updated.", foundVersion);
                    return false;

                case 2:
                    errorMessage = string.Format("({0}) was unrecognized.", foundVersion);
                    return false;

                default:
                    errorMessage = string.Format(_failMessage, foundVersion);
                    return false;
            }
        }
    }
}