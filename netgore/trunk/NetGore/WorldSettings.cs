using System.Linq;
using Microsoft.Xna.Framework;
using NetGore;

// FUTURE: This shouldn't be here! It needs to be in DemoGame

namespace NetGore
{
    /// <summary>
    /// Contains the settings for the world
    /// </summary>
    public static class WorldSettings
    {
        /// <summary>
        /// Amount of velocity added to an entity every millisecond
        /// </summary>
        public static readonly Vector2 Gravity = new Vector2(0f, 0.0009f);

        /// <summary>
        /// Maximum velocity allowed for an entity (specified in positive values)
        /// </summary>
        public static readonly Vector2 MaxVelocity = new Vector2(1f, 1f);
    }
}