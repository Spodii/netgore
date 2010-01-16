using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Provides informational drawing for the Entities on a Map.
    /// </summary>
    public class MapEntityBoxDrawer : MapDrawingExtension
    {
        /// <summary>
        /// When overridden in the derived class, handles drawing to the map after the given <paramref name="layer"/> is drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="layer">The layer that was just drawn.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to draw to.</param>
        protected override void HandleDrawAfterLayer(IDrawableMap map, MapRenderLayer layer, SpriteBatch spriteBatch)
        {
            if (layer != MapRenderLayer.SpriteForeground)
                return;

            var visibleArea = map.Camera.GetViewArea();
            var visibleNonWalls = map.Spatial.GetMany<Entity>(visibleArea, x => !(x is WallEntityBase));
            foreach (var entity in visibleNonWalls)
            {
                EntityDrawer.Draw(spriteBatch, entity);
            }
        }
    }
}