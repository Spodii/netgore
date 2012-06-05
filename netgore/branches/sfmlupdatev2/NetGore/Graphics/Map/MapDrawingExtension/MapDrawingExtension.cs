using System.Diagnostics;
using System.Linq;

namespace NetGore.Graphics
{
    /// <summary>
    /// Base class for implementing a <see cref="IMapDrawingExtension"/>.
    /// </summary>
    public abstract class MapDrawingExtension : IMapDrawingExtension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapDrawingExtension"/> class.
        /// </summary>
        protected MapDrawingExtension()
        {
            Enabled = true;
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing to the map after the given <see cref="MapRenderLayer"/> is drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="e">The <see cref="NetGore.Graphics.DrawableMapDrawLayerEventArgs"/> instance containing the event data.</param>
        protected virtual void HandleDrawAfterLayer(IDrawableMap map, DrawableMapDrawLayerEventArgs e)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing to the map after all of the map drawing finishes.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the map being drawn.</param>
        protected virtual void HandleDrawAfterMap(IDrawableMap map, ISpriteBatch spriteBatch, ICamera2D camera)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing to the map before the given <see cref="MapRenderLayer"/> is drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="e">The <see cref="NetGore.Graphics.DrawableMapDrawLayerEventArgs"/> instance containing the event data.</param>
        protected virtual void HandleDrawBeforeLayer(IDrawableMap map, DrawableMapDrawLayerEventArgs e)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing to the map before any of the map drawing starts.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the map being drawn.</param>
        protected virtual void HandleDrawBeforeMap(IDrawableMap map, ISpriteBatch spriteBatch, ICamera2D camera)
        {
        }

        #region IMapDrawingExtension Members

        /// <summary>
        /// Gets or sets if this <see cref="IMapDrawingExtension"/> will perform the drawing.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Handles drawing to the map after the given <see cref="MapRenderLayer"/> is drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="e">The <see cref="NetGore.Graphics.DrawableMapDrawLayerEventArgs"/> containing the drawing event arguments.</param>
        void IMapDrawingExtension.DrawAfterLayer(IDrawableMap map, DrawableMapDrawLayerEventArgs e)
        {
            if (!Enabled)
                return;

            if (map == null)
            {
                Debug.Fail("map is null.");
                return;
            }

            if (e.SpriteBatch == null)
            {
                Debug.Fail("spriteBatch is null.");
                return;
            }

            if (e.Camera == null)
            {
                Debug.Fail("camera is null.");
                return;
            }

            Debug.Assert(EnumHelper<MapRenderLayer>.IsDefined(e.Layer), "Invalid layer specified.");
            Debug.Assert(!e.SpriteBatch.IsDisposed, "spriteBatch is disposed.");

            HandleDrawAfterLayer(map, e);
        }

        /// <summary>
        /// Handles drawing to the map after any layers are drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="e">The <see cref="DrawableMapDrawEventArgs"/> containing the drawing event arguments.</param>
        void IMapDrawingExtension.DrawAfterMap(IDrawableMap map, DrawableMapDrawEventArgs e)
        {
            if (!Enabled)
                return;

            if (map == null)
            {
                Debug.Fail("map is null.");
                return;
            }

            if (e.SpriteBatch == null)
            {
                Debug.Fail("spriteBatch is null.");
                return;
            }

            if (e.Camera == null)
            {
                Debug.Fail("camera is null.");
                return;
            }

            Debug.Assert(!e.SpriteBatch.IsDisposed, "spriteBatch is disposed.");

            HandleDrawAfterMap(map, e.SpriteBatch, e.Camera);
        }

        /// <summary>
        /// Handles drawing to the map before the given <see cref="MapRenderLayer"/> is drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="e">The <see cref="NetGore.Graphics.DrawableMapDrawLayerEventArgs"/> containing the drawing event arguments.</param>
        void IMapDrawingExtension.DrawBeforeLayer(IDrawableMap map, DrawableMapDrawLayerEventArgs e)
        {
            if (!Enabled)
                return;

            if (map == null)
            {
                Debug.Fail("map is null.");
                return;
            }

            if (e.SpriteBatch == null)
            {
                Debug.Fail("spriteBatch is null.");
                return;
            }

            if (e.Camera == null)
            {
                Debug.Fail("camera is null.");
                return;
            }

            Debug.Assert(EnumHelper<MapRenderLayer>.IsDefined(e.Layer), "Invalid layer specified.");
            Debug.Assert(!e.SpriteBatch.IsDisposed, "spriteBatch is disposed.");

            HandleDrawBeforeLayer(map, e);
        }

        /// <summary>
        /// Handles drawing to the map before any layers are drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="e">The <see cref="DrawableMapDrawEventArgs"/> containing the drawing event arguments.</param>
        void IMapDrawingExtension.DrawBeforeMap(IDrawableMap map, DrawableMapDrawEventArgs e)
        {
            if (!Enabled)
                return;

            if (map == null)
            {
                Debug.Fail("map is null.");
                return;
            }

            if (e.SpriteBatch == null)
            {
                Debug.Fail("spriteBatch is null.");
                return;
            }

            if (e.Camera == null)
            {
                Debug.Fail("camera is null.");
                return;
            }

            Debug.Assert(!e.SpriteBatch.IsDisposed, "spriteBatch is disposed.");

            HandleDrawBeforeMap(map, e.SpriteBatch, e.Camera);
        }

        #endregion
    }
}