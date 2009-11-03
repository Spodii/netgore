using System;
using System.Collections.Generic;
using System.Text;

namespace InstallationValidator
{
    /// <summary>
    /// Interface for an object that runs a test to see if NetGore was installed properly.
    /// </summary>
    interface ITestable
    {
        /// <summary>
        /// Runs a test.
        /// </summary>
        void Test();
    }
}
