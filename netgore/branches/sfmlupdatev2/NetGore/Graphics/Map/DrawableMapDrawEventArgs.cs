using System;
using System.Linq;

namespace NetGore.Graphics
{
    /// <summary>
    /// <see cref="EventArgs"/> for a draw event for an <see cref="IDrawableMap"/>.
    /// </summary>
    public class DrawableMapDrawEventArgs : EventArgs
    {
        readonly ICamera2D _camera;
        readonly ISpriteBatch _spriteBatch;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawableMapDrawEventArgs"/> class.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> that was used to do the drawing.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the map being drawn.</param>
        public DrawableMapDrawEventArgs(ISpriteBatch spriteBatch, ICamera2D camera)
        {
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
        /// Gets the <see cref="ISpriteBatch"/> that was used to do the drawing.
        /// </summary>
        public ISpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }
    }
}