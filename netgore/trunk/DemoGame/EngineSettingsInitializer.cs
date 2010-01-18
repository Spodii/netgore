using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.Features.Shop;

namespace DemoGame
{
    /// <summary>
    /// Provides a clean way to initialize the <see cref="EngineSettings"/>, along with any of the other game
    /// settings that need to be initialized.
    /// </summary>
    public class EngineSettingsInitializer : EngineSettings
    {
        /// <summary>
        /// Initializes the <see cref="EngineSettingsInitializer"/> class.
        /// </summary>
        static EngineSettingsInitializer()
        {
            Initialize(new EngineSettingsInitializer());

            ShopSettings.Initialize(new ShopSettings(6 * 6));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineSettingsInitializer"/> class.
        /// </summary>
        EngineSettingsInitializer() : base(new Vector2(0f, 0.0009f), new Vector2(0.5f, 0.5f), 500)
        {
        }

        /// <summary>
        /// Initializes the <see cref="EngineSettings"/>. This can be called any number of times, but must
        /// be called at least once before the engine components are utilized.
        /// </summary>
        public static void Initialize()
        {
            // This method is empty, but it still initializes the GameSettings. When this method is called,
            // it will force the static GameSettings constructor to be called, which will handle the actual
            // initialization. We just need this method to perform the initial invoke to get things rolling.
        }
    }
}