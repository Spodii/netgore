using System.Linq;
using NetGore;

namespace DemoGame.Client
{
    /// <summary>
    /// Enum consisting of the layer at which map entities are rendered
    /// </summary>
    public enum MapRenderLayer
    {
        /// <summary>
        /// Back-most layer for the Map. Contains all of the background images, which are always behind every other layer.
        /// </summary>
        Background,

        /// <summary>
        /// Back-most layer for Map sprites. These will appear behind the Character and Item.
        /// </summary>
        SpriteBackground,

        /// <summary>
        /// Character layer.
        /// </summary>
        Chararacter,

        /// <summary>
        /// Item layer.
        /// </summary>
        Item,

        /// <summary>
        /// Front-most layer for Map sprites. These will appear in front of the Character and Item.
        /// </summary>
        SpriteForeground
    }
}