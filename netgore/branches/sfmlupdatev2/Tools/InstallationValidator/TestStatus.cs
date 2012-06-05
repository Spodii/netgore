using System.Linq;

namespace InstallationValidator
{
    /// <summary>
    /// Contains the different statuses possible for a test.
    /// </summary>
    public enum TestStatus : byte
    {
        /// <summary>
        /// The test has yet to be run.
        /// </summary>
        NotTested,

        /// <summary>
        /// The test passed.
        /// </summary>
        Passed,

        /// <summary>
        /// The test failed.
        /// </summary>
        Failed
    }
}