using System.Linq;

namespace InstallationValidator
{
    /// <summary>
    /// Interface for an object that runs a test to see if NetGore was installed properly.
    /// </summary>
    public interface ITestable
    {
        /// <summary>
        /// Gets the description of the test.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets if this test performs something that is vital to any other tests. If true, execution of tests will break
        /// immediately after this test fails. If false, tests will continue to be executed if this test fails.
        /// </summary>
        bool IsVital { get; }

        /// <summary>
        /// Gets the error message from the last run. If the test has not yet been run, or it was successful, this
        /// will return null.
        /// </summary>
        string LastRunError { get; }

        /// <summary>
        /// Gets the status of the last time the test was run.
        /// </summary>
        TestStatus LastRunStatus { get; }

        /// <summary>
        /// Gets the name of the test.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Clears any information about when the test was last run.
        /// </summary>
        void ClearStatus();

        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="errorMessage">When the method returns false, contains an error message as to why
        /// the test failed. Otherwise, contains an empty string.</param>
        /// <returns>True if the test passed; false if the test failed.</returns>
        bool Test(out string errorMessage);
    }
}