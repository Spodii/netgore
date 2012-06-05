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
        /// Handles drawing to the map after the given <see cref="MapRenderLayer"/> is drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="e">The <see cref="NetGore.Graphics.DrawableMapDrawLayerEventArgs"/> containing the drawing event arguments.</param>
        void DrawAfterLayer(IDrawableMap map, DrawableMapDrawLayerEventArgs e);

        /// <summary>
        /// Handles drawing to the map after any layers are drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="e">The <see cref="DrawableMapDrawEventArgs"/> containing the drawing event arguments.</param>
        void DrawAfterMap(IDrawableMap map, DrawableMapDrawEventArgs e);

        /// <summary>
        /// Handles drawing to the map before the given <see cref="MapRenderLayer"/> is drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="e">The <see cref="NetGore.Graphics.DrawableMapDrawLayerEventArgs"/> containing the drawing event arguments.</param>
        void DrawBeforeLayer(IDrawableMap map, DrawableMapDrawLayerEventArgs e);

        /// <summary>
        /// Handles drawing to the map before any layers are drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="e">The <see cref="DrawableMapDrawEventArgs"/> containing the drawing event arguments.</param>
        void DrawBeforeMap(IDrawableMap map, DrawableMapDrawEventArgs e);
    }
}