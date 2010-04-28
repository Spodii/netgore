using System.Linq;

namespace NetGore.Graphics
{
    /// <summary>
    /// Describes how a BackgroundLayer is drawn on an axis.
    /// </summary>
    public enum BackgroundLayerLayout : byte
    {
        /// <summary>
        /// Displays the image as-is without any alterations.
        /// </summary>
        None,

        /// <summary>
        /// Tiles the image repeatidly over the whole map.
        /// </summary>
        Tiled,

        /// <summary>
        /// Stretches the image to span across the whole map over the axis.
        /// </summary>
        Stretched
    }
}