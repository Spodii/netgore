using System.Linq;
using NetGore.Graphics;

namespace NetGore.Features.Emoticons
{
    /// <summary>
    /// A map extension for drawing the emoticons.
    /// </summary>
    /// <typeparam name="TKey">The emoticon key.</typeparam>
    /// <typeparam name="TValue">The emoticon information.</typeparam>
    public class EmoticonMapDrawingExtension<TKey, TValue> : MapDrawingExtension where TValue : EmoticonInfo<TKey>
    {
        readonly EmoticonDisplayManager<TKey, TValue> _displayManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmoticonMapDrawingExtension{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="displayManager">The <see cref="EmoticonDisplayManager{TKey, TValue}"/>.</param>
        public EmoticonMapDrawingExtension(EmoticonDisplayManager<TKey, TValue> displayManager)
        {
            _displayManager = displayManager;
        }

        /// <summary>
        /// The <see cref="EmoticonDisplayManager{TKey, TValue}"/> containing the emoticons to display.
        /// </summary>
        public EmoticonDisplayManager<TKey, TValue> DisplayManager
        {
            get { return _displayManager; }
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing to the map after the given <paramref name="layer"/> is drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="layer">The layer that was just drawn.</param>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the map being drawn.</param>
        protected override void HandleDrawAfterLayer(IDrawableMap map, MapRenderLayer layer, ISpriteBatch spriteBatch, ICamera2D camera)
        {
            if (layer == MapRenderLayer.Chararacter)
                _displayManager.Draw(spriteBatch);
        }
    }
}