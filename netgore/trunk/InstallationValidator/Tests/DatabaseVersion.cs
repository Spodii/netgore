using System.Linq;
using System.Text.RegularExpressions;

namespace InstallationValidator.Tests
{
    public sealed class DatabaseVersion : TestableBase
    {
        const string _testName = "Database version";
        const string _description = "Checks to make sure your database is of a supported version. NetGore requires MySQL 5.1 or later. If you do not have MySQL 5.1 or later, you will encounter errors when trying to import the database dump file (db.sql).";
        const string _failMessage = "Your MySQL database version ({0}) is not supported. Please download the latest version of MySQL from the MySQL website at:\n\nhttp://dev.mysql.com/downloads/mysql/\n\nADDITIONAL INFO: If you know you have already installed 5.1 or later, the issue is likely that you have more than one instance of MySQL installed. A common case of this is if you are using WAMP or any similar web server package that includes MySQL. If you do not use the web server package anymore, uninstalling it can fix this issue since it will also uninstall the older version of MySQL. You may need to follow this up by reinstalling the newer version of MySQL, too. If you do not know how to stop and start services, you can either Google it, or ask for assistance in the NetGore forums.";

        /// <summary>
        /// The prefix to give to every <see cref="_supportedVersionStrs"/> regex.
        /// </summary>
        const string _regexPrefix = @"version\(\)[\r\n]*";

        /// <summary>
        /// A collection of Regex strings for the supported versions.
        /// </summary>
        static readonly string[] _supportedVersionStrs = new string[] { 
            // 5.1 and later
            @"5\.[1-9]", 
            // 6.x and later
            @"6\.",
            // Anything beyond 6 (we'll properly test this when those versions actually come out...)
            @"[7-9]\."
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseVersion"/> class.
        /// </summary>
        public DatabaseVersion()
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
            string[] cmds = new string[] { "SELECT version();" ,"exit"};

            string output;
            string error;
            MySqlHelper.MySqlCommand(null, out output, out error, cmds);

            if (!string.IsNullOrEmpty(error))
            {
                errorMessage = "Failed to acquire the MySQL version. Reason: " + error;
                return false;
            }

            var regexes = _supportedVersionStrs.Select(x => new Regex(_regexPrefix + x, RegexOptions.IgnoreCase));
            var success =  regexes.Any(x => x.IsMatch(output));

            if (!success)
            {
                var foundVersion = output.Replace("\r", "").Replace("\n", "").Replace("version()", "");
                errorMessage = string.Format(_failMessage, foundVersion);
            }

            return success;
        }
    }
}