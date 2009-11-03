
namespace InstallationValidator.Tests
{
    public class LoadXNA : ITestable
    {
        /// <summary>
        /// Runs a test.
        /// </summary>
        public void Test()
        {
            const string testName = "Load XNA";
            const string failInfo = "Failed to load the XNA assembly";

            Tester.Test(testName, AssemblyHelper.TryLoadAssembly(KnownAssembly.Xna) != null, failInfo);
        }
    }
}