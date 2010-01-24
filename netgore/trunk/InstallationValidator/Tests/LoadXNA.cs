namespace InstallationValidator.Tests
{
    public sealed class LoadXNA : TestableBase
    {
        const string _testName = "Load XNA";
        const string _description = "Attempts to load the XNA assembly to ensure it is not corrupt.";
        const string _failMessage = "XNA 3.1 is not installed or the assembly is corrupt. A link to the XNA download can be found in the NetGore Setup Guide. If you already have XNA 3.1 installed, try uninstalling and reinstalling it.";
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadXNA"/> class.
        /// </summary>
        public LoadXNA()
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
            var success = AssemblyHelper.TryLoadAssembly(KnownAssembly.Xna) != null;

            if (!success)
                errorMessage = _failMessage;

            return success;
        }
    }
}