using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InstallationValidator
{
    public abstract class TestableBase : ITestable
    {
        readonly string _name;
        readonly string _description;

        TestStatus _testStatus = TestStatus.NotTested;
        string _lastRunError = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestableBase"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        protected TestableBase(string name, string description)
        {
            _name = name;
            _description = description;
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

        public static string AppendErrorDetails(string errorMessage, string details)
        {
            if (string.IsNullOrEmpty(details))
                return errorMessage;

            return errorMessage + "\n\nError details:\n" + details;
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

        /// <summary>
        /// Gets the name of the test.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the description of the test.
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Gets the status of the last time the test was run.
        /// </summary>
        public TestStatus LastRunStatus
        {
            get { return _testStatus; }
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
        /// Clears any information about when the test was last run.
        /// </summary>
        public void ClearStatus()
        {
            _testStatus = TestStatus.NotTested;
            _lastRunError = null;
        }
    }
}
