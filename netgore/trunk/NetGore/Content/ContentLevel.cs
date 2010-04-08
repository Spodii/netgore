using System.Linq;

namespace NetGore.Content
{
    /// <summary>
    /// Represents the level of a content object. Lower content levels are unloaded less frequently.
    /// </summary>
    public enum ContentLevel : byte
    {
        /// <summary>
        /// Intended for content that is to be loaded in memory at all times.
        /// This is the lowest <see cref="ContentLevel"/>.
        /// </summary>
        Global,

        /// <summary>
        /// Intended for content that is for a specific <see cref="GameScreen"/>.
        /// This is lower than a <see cref="Map"/> and greater than <see cref="Global"/>.
        /// </summary>
        GameScreen,

        /// <summary>
        /// Intended for content for a single map.
        /// This is lower than <see cref="Temporary"/> and greater than <see cref="GameScreen"/>.
        /// </summary>
        Map,

        /// <summary>
        /// Intended for content loaded for a short amount of time, such as to build a texture atlas.
        /// This is the greatest <see cref="ContentLevel"/>.
        /// </summary>
        Temporary
    }
}