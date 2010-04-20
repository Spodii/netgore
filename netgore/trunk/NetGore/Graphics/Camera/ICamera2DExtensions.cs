using System;
using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Extension methods for the <see cref="ICamera2D"/> interface.
    /// </summary>
    public static class ICamera2DExtensions
    {
        /// <summary>
        /// Gets the zoom level for a <see cref="ICamera2D"/> needed to completely fill the camera's view with an
        /// item of the given size while fitting the whole item into the screen.
        /// </summary>
        /// <param name="camera">The <see cref="ICamera2D"/>.</param>
        /// <param name="itemSize">The size of the item to fill the camera's view area.</param>
        /// <returns>The zoom level for a <see cref="ICamera2D"/> needed to completely fill the camera's view
        /// with an item of the given <paramref name="itemSize"/>.</returns>
        public static float GetFillScreenZoomLevel(this ICamera2D camera, Vector2 itemSize)
        {
            var zoom = (camera.Size * camera.Scale) / itemSize;
            return Math.Min(zoom.X, zoom.Y);
        }
    }
}