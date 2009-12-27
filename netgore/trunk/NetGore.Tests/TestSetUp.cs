using System.Linq;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace NetGore.Tests
{
    /// <summary>
    /// Provides calls to initialization methods that are run before any tests.
    /// </summary>
    [SetUpFixture]
    public class TestSetUp
    {
        /// <summary>
        /// Initializes the <see cref="EngineSettings"/> for tests that require it.
        /// </summary>
        [SetUp]
        public void SetUpEngineSettings()
        {
            EngineSettings.Initialize(new EngineSettings(new Vector2(0f, 0.0009f), new Vector2(1f, 1f)));
        }
    }
}