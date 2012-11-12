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
        /// When overridden in the derived class, handles drawing to the map after the given <see cref="MapRenderLayer"/> is drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="e">The <see cref="NetGore.Graphics.DrawableMapDrawLayerEventArgs"/> instance containing the event data.</param>
        protected override void HandleDrawAfterLayer(IDrawableMap map, DrawableMapDrawLayerEventArgs e)
        {
            // Draw emoticons after dynamic layer finishes
            if (e.Layer == MapRenderLayer.Dynamic)
                _displayManager.Draw(e.SpriteBatch);
        }
    }
}