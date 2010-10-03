using System.Linq;

namespace NetGore.Graphics
{
    /// <summary>
    /// Interface for an object that extends the drawing of an <see cref="IDrawableMap"/>.
    /// </summary>
    public interface IMapDrawingExtension
    {
        /// <summary>
        /// Gets or sets if this <see cref="IMapDrawingExtension"/> will perform the drawing.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Handles drawing to the map after the given <paramref name="layer"/> is drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="layer">The layer that was just drawn.</param>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the map being drawn.</param>
        void DrawAfterLayer(IDrawableMap map, MapRenderLayer layer, ISpriteBatch spriteBatch, ICamera2D camera);

        /// <summary>
        /// Handles drawing to the map before the given <paramref name="layer"/> is drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="layer">The layer that is going to be drawn.</param>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the map being drawn.</param>
        void DrawBeforeLayer(IDrawableMap map, MapRenderLayer layer, ISpriteBatch spriteBatch, ICamera2D camera);

        /// <summary>
        /// Handles drawing to the map before any layers are drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the map being drawn.</param>
        void DrawBeforeMap(IDrawableMap map, ISpriteBatch spriteBatch, ICamera2D camera);

        /// <summary>
        /// Handles drawing to the map after any layers are drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the map being drawn.</param>
        void DrawAfterMap(IDrawableMap map, ISpriteBatch spriteBatch, ICamera2D camera);
    }
}