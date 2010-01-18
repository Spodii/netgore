using System.Linq;
using Microsoft.Xna.Framework;
using NetGore.Features.Shops;
using NetGore.Features.StatusEffects;
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
            EngineSettings.Initialize(new EngineSettings(new Vector2(0f, 0.0009f), new Vector2(0.5f, 0.5f)));
            ShopSettings.Initialize(new ShopSettings(6 * 6));
            StatusEffectsSettings.Initialize(new StatusEffectsSettings(500));
        }
    }
}