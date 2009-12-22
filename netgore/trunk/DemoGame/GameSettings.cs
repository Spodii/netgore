using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame
{
    public class GameSettings : EngineSettings
    {
        static readonly GameSettings _instance;

        /// <summary>
        /// Gets the <see cref="GameSettings"/> instance.
        /// </summary>
        new public static GameSettings Instance { get { return _instance; } }

        /// <summary>
        /// Initializes the <see cref="GameSettings"/> class.
        /// </summary>
        static GameSettings()
        {
            _instance = new GameSettings();
            Initialize(_instance);
        }

        /// <summary>
        /// Initializes the <see cref="GameSettings"/> instance. This can be called any number of times, but must
        /// be called at least once before the engine components are utilized.
        /// </summary>
        public static void Initialize()
        {
            // This method is empty, but it still initializes the GameSettings. When this method is called,
            // it will force the static GameSettings constructor to be called, which will handle the actual
            // initialization. We just need this method to perform the initial invoke to get things rolling.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameSettings"/> class.
        /// </summary>
        GameSettings() : base(new Vector2(0f, 0.0009f), new Vector2(1f, 1f))
        {
        }
    }
}
