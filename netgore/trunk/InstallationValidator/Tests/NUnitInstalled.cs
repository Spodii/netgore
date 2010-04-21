using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace InstallationValidator.Tests
{
    public sealed class NUnitInstalled : TestableBase
    {
        const string _testName = "NUnit installed";
        const string _description = "Checks if NUnit version 2.5 or later is installed. This is not required to use NetGore, but is required to run the unit tests in NetGore, and is highly recommended since unit tests can help significantly with ensuring things are working properly.";
        const string _failMessage = "Could not find NUnit version 2.5 or later. You either do not have NUnit installed, or you have a version earlier than 2.5 installed. To resolve this, you can do one of the following:\n1. Go to http://nunit.com/ to download and install the latest version of NUnit.\n2. Ignore this error and don't install NUnit. To build NetGore, when you open the solution, you will need to right-click the NetGore.Tests project in the Solution Explorer window, and click Unload Project to prevent it from building.";

        static readonly Regex _versionRegex = new Regex(@"Version=(?<Major>\d+)\.(?<Minor>\d+)\.\d+", RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.CultureInvariant);

        /// <summary>
        /// Initializes a new instance of the <see cref="NUnitInstalled"/> class.
        /// </summary>
        public NUnitInstalled() : base(_testName, _description, false)
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
            errorMessage = _failMessage;

            Assembly a;
            try
            {
                a = Assembly.Load("nunit.framework");
                var m = _versionRegex.Match(a.ToString());

                if (!m.Success)
                    return false;

                var major = int.Parse(m.Groups["Major"].Value);
                var minor = int.Parse(m.Groups["Minor"].Value);

                // Greater than version 2
                if (major > 2)
                    return true;

                // Version 2.5 or greater
                if (major == 2 && minor >= 5)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                errorMessage = AppendErrorDetails(_failMessage, ex.ToString());
                return false;
            }
        }
    }
}