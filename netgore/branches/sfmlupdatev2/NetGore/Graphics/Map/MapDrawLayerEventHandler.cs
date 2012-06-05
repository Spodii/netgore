using System;
using System.Linq;

namespace NetGore.Graphics
{
    /// <summary>
    /// <see cref="EventArgs"/> for a draw event for an <see cref="IDrawableMap"/>'s layer being drawn.
    /// </summary>
    public class DrawableMapDrawLayerEventArgs : EventArgs
    {
        readonly ICamera2D _camera;
        readonly MapRenderLayer _layer;
        readonly ISpriteBatch _spriteBatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawableMapDrawLayerEventArgs"/> class.
        /// </summary>
        /// <param name="layer">The layer that the drawing event is related to.</param>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> that was used to do the drawing.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the map being drawn.</param>
        public DrawableMapDrawLayerEventArgs(MapRenderLayer layer, ISpriteBatch spriteBatch, ICamera2D camera)
        {
            _layer = layer;
            _spriteBatch = spriteBatch;
            _camera = camera;
        }

        /// <summary>
        /// Gets the <see cref="ICamera2D"/> that describes the view of the map being drawn.
        /// </summary>
        public ICamera2D Camera
        {
            get { return _camera; }
        }

        /// <summary>
        /// Gets the <see cref="MapRenderLayer"/> that the drawing event is related to.
        /// </summary>
        public MapRenderLayer Layer
        {
            get { return _layer; }
        }

        /// <summary>
        /// Gets the <see cref="ISpriteBatch"/> that was used to do the drawing.
        /// </summary>
        public ISpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }
    }
}