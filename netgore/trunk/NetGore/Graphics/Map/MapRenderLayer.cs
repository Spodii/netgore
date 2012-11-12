using System.Linq;

namespace NetGore.Graphics
{
    /// <summary>
    /// Enum consisting of the layer at which map entities are rendered
    /// </summary>
    public enum MapRenderLayer : byte
    {
        /// <summary>
        /// Back-most layer for the Map. Contains all of the background images, which are always behind every other layer.
        /// </summary>
        Background,

        /// <summary>
        /// Back-most layer for Map sprites. These will always appear behind the Dynamic layer.
        /// </summary>
        SpriteBackground,

        /// <summary>
        /// Sprites on this layer draw in an order that depends on the Y axis and the height of the sprite. Formally known as the
        /// Character and Item layers.
        /// </summary>
        Dynamic,

        /// <summary>
        /// Front-most layer for Map sprites. These will always appear in front of the Dynamic layer.
        /// </summary>
        SpriteForeground
    }
}