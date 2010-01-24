namespace InstallationValidator.Tests
{
    public sealed class LoadMySqlData : TestableBase
    {
        const string _testName = "Load MySql Connector";
        const string _description = "Checks to make sure the MySql .NET connector assembly can be loaded. Usually this test will fail only if the needed version of the MySql .NET connector is not installed, though it is also possible that the assembly is somehow corrupt.";
        const string _failMessage = "The MySQL .NET connector failed to load. The installation file can be found in the root NetGore directory in the folder named '_dependencies'. If you have already installed this in the past, you may need to install again as you may not have the correct version installed. If you do have the correct version installed, try uninstalling then reinstalling in case the assembly somehow became corrupt.";
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadMySqlData"/> class.
        /// </summary>
        public LoadMySqlData()
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
            var success = AssemblyHelper.TryLoadAssembly(KnownAssembly.MySqlData) != null;

            if (!success)
                errorMessage = _failMessage;

            return success;
        }
    }
}