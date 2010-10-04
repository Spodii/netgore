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
        /// When overridden in the derived class, handles drawing to the map after the given <paramref name="layer"/> is drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="layer">The layer that was just drawn.</param>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the map being drawn.</param>
        protected virtual void HandleDrawAfterLayer(IDrawableMap map, MapRenderLayer layer, ISpriteBatch spriteBatch,
                                                    ICamera2D camera)
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
        /// When overridden in the derived class, handles drawing to the map before the given <paramref name="layer"/> is drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="layer">The layer that is going to be drawn.</param>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the map being drawn.</param>
        protected virtual void HandleDrawBeforeLayer(IDrawableMap map, MapRenderLayer layer, ISpriteBatch spriteBatch,
                                                     ICamera2D camera)
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
        /// Handles drawing to the map after the given <paramref name="layer"/> is drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="layer">The layer that was just drawn.</param>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the map being drawn.</param>
        void IMapDrawingExtension.DrawAfterLayer(IDrawableMap map, MapRenderLayer layer, ISpriteBatch spriteBatch,
                                                 ICamera2D camera)
        {
            if (!Enabled)
                return;

            if (map == null)
            {
                Debug.Fail("map is null.");
                return;
            }

            if (spriteBatch == null)
            {
                Debug.Fail("spriteBatch is null.");
                return;
            }

            if (camera == null)
            {
                Debug.Fail("camera is null.");
                return;
            }

            Debug.Assert(EnumHelper<MapRenderLayer>.IsDefined(layer), "Invalid layer specified.");
            Debug.Assert(!spriteBatch.IsDisposed, "spriteBatch is disposed.");

            HandleDrawAfterLayer(map, layer, spriteBatch, camera);
        }

        /// <summary>
        /// Handles drawing to the map after any layers are drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the map being drawn.</param>
        void IMapDrawingExtension.DrawAfterMap(IDrawableMap map, ISpriteBatch spriteBatch, ICamera2D camera)
        {
            if (!Enabled)
                return;

            if (map == null)
            {
                Debug.Fail("map is null.");
                return;
            }

            if (spriteBatch == null)
            {
                Debug.Fail("spriteBatch is null.");
                return;
            }

            if (camera == null)
            {
                Debug.Fail("camera is null.");
                return;
            }

            Debug.Assert(!spriteBatch.IsDisposed, "spriteBatch is disposed.");

            HandleDrawAfterMap(map, spriteBatch, camera);
        }

        /// <summary>
        /// Handles drawing to the map before the given <paramref name="layer"/> is drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="layer">The layer that is going to be drawn.</param>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the map being drawn.</param>
        void IMapDrawingExtension.DrawBeforeLayer(IDrawableMap map, MapRenderLayer layer, ISpriteBatch spriteBatch,
                                                  ICamera2D camera)
        {
            if (!Enabled)
                return;

            if (map == null)
            {
                Debug.Fail("map is null.");
                return;
            }

            if (spriteBatch == null)
            {
                Debug.Fail("spriteBatch is null.");
                return;
            }

            if (camera == null)
            {
                Debug.Fail("camera is null.");
                return;
            }

            Debug.Assert(EnumHelper<MapRenderLayer>.IsDefined(layer), "Invalid layer specified.");
            Debug.Assert(!spriteBatch.IsDisposed, "spriteBatch is disposed.");

            HandleDrawBeforeLayer(map, layer, spriteBatch, camera);
        }

        /// <summary>
        /// Handles drawing to the map before any layers are drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the map being drawn.</param>
        void IMapDrawingExtension.DrawBeforeMap(IDrawableMap map, ISpriteBatch spriteBatch, ICamera2D camera)
        {
            if (!Enabled)
                return;

            if (map == null)
            {
                Debug.Fail("map is null.");
                return;
            }

            if (spriteBatch == null)
            {
                Debug.Fail("spriteBatch is null.");
                return;
            }

            if (camera == null)
            {
                Debug.Fail("camera is null.");
                return;
            }

            Debug.Assert(!spriteBatch.IsDisposed, "spriteBatch is disposed.");

            HandleDrawBeforeMap(map, spriteBatch, camera);
        }

        #endregion
    }
}