using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Handler for Map drawing events.
    /// </summary>
    /// <param name="map">Map that the drawing is taking place on.</param>
    /// <param name="layer">The layer that the drawing event is related to.</param>
    /// <param name="spriteBatch">The SpriteBatch that was used to do the drawing.</param>
    public delegate void MapDrawEventHandler(IDrawableMap map, MapRenderLayer layer, SpriteBatch spriteBatch);
}