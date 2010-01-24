using System;

namespace InstallationValidator.Tests
{
    public sealed class CheckForXNA : TestableBase
    {
        const string _testName = "XNA 3.1 installed";
        const string _description = "Checks that the required version of XNA is installed. This is required by all parts of NetGore, and the engine will not be able to function without it.";
        const string _failMessage = "XNA 3.1 is not installed. A link to the XNA download can be found in the NetGore Setup Guide.";

        /// <summary>
        /// Initializes a new instance of the <see cref="TestableBase"/> class.
        /// </summary>
        public CheckForXNA()
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
            var success =AssemblyHelper.IsAssemblyInstalled(KnownAssembly.Xna);

            if (!success)
                errorMessage = _failMessage;

            return success;
        }
    }
}