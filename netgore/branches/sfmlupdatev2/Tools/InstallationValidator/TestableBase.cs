using System;
using System.Linq;

namespace InstallationValidator
{
    /// <summary>
    /// Base class for a <see cref="ITestable"/>.
    /// </summary>
    public abstract class TestableBase : ITestable
    {
        readonly string _description;
        readonly bool _isVital;
        readonly string _name;

        string _lastRunError = null;
        TestStatus _testStatus = TestStatus.NotTested;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestableBase"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="isVital">True if the tests will stop executing if this test fails; otherwise false.</param>
        protected TestableBase(string name, string description, bool isVital = true)
        {
            _name = name;
            _description = description;
            _isVital = isVital;
        }

        /// <summary>
        /// Appends the error details to an error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="details">The error details.</param>
        /// <returns>The formatted error message with appended error details.</returns>
        public static string AppendErrorDetails(string errorMessage, string details)
        {
            if (string.IsNullOrEmpty(details))
                return errorMessage;

            return errorMessage + "\n\nError details:\n" + details;
        }

        /// <summary>
        /// When overridden in the derived class, runs the test.
        /// </summary>
        /// <param name="errorMessage">When the method returns false, contains an error message as to why
        /// the test failed. Otherwise, contains an empty string.</param>
        /// <returns>
        /// True if the test passed; false if the test failed.
        /// </returns>
        protected abstract bool RunTest(ref string errorMessage);

        #region ITestable Members

        /// <summary>
        /// Gets the description of the test.
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Gets if this test performs something that is vital to any other tests. If true, execution of tests will break
        /// immediately after this test fails. If false, tests will continue to be executed if this test fails.
        /// </summary>
        public bool IsVital
        {
            get { return _isVital; }
        }

        /// <summary>
        /// Gets the error message from the last run. If the test has not yet been run, or it was successful, this
        /// will return null.
        /// </summary>
        public string LastRunError
        {
            get { return _lastRunError; }
        }

        /// <summary>
        /// Gets the status of the last time the test was run.
        /// </summary>
        public TestStatus LastRunStatus
        {
            get { return _testStatus; }
        }

        /// <summary>
        /// Gets the name of the test.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Clears any information about when the test was last run.
        /// </summary>
        public void ClearStatus()
        {
            _testStatus = TestStatus.NotTested;
            _lastRunError = null;
        }

        /// <summary>
        /// Runs the test.
        /// </summary>
        /// <param name="errorMessage">When the method returns false, contains an error message as to why
        /// the test failed. Otherwise, contains an empty string.</param>
        /// <returns>True if the test passed; false if the test failed.</returns>
        public bool Test(out string errorMessage)
        {
            errorMessage = string.Empty;

            bool passed;
            try
            {
                passed = RunTest(ref errorMessage);
            }
            catch (Exception ex)
            {
                passed = false;
                errorMessage += "\n\nThe test threw the following unhandled exception:\n" + ex;
            }

            if (passed)
            {
                _lastRunError = null;
                _testStatus = TestStatus.Passed;
            }
            else
            {
                _lastRunError = errorMessage;
                _testStatus = TestStatus.Failed;
            }

            return passed;
        }

        #endregion
    }
}