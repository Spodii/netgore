using System.Linq;

namespace InstallationValidator
{
    public enum TestStatus
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