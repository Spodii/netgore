using System.Linq;

namespace NetGore.Graphics
{
    /// <summary>
    /// Handler for Map drawing events.
    /// </summary>
    /// <param name="map">Map that the drawing is taking place on.</param>
    /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> that was used to do the drawing.</param>
    /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the map being drawn.</param>
    public delegate void MapDrawEventHandler(IDrawableMap map, ISpriteBatch spriteBatch, ICamera2D camera);
}