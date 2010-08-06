using System.Diagnostics;
using System.Linq;
using NetGore.Graphics;
using NetGore.World;

namespace DemoGame.Client
{
    /// <summary>
    /// Provides drawing of the WallEntities on the Map.
    /// </summary>
    public class MapWallDrawer : MapDrawingExtension
    {
        /// <summary>
        /// When overridden in the derived class, handles drawing to the map after the given <paramref name="layer"/> is drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="layer">The layer that was just drawn.</param>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        protected override void HandleDrawAfterLayer(IDrawableMap map, MapRenderLayer layer, ISpriteBatch spriteBatch)
        {
            if (layer != MapRenderLayer.SpriteForeground)
                return;

            if (map.Camera == null)
            {
                Debug.Fail("Expected the map's Camera to not be null.");
                return;
            }

            var visibleArea = map.Camera.GetViewArea();
            var visibleWalls = map.Spatial.GetMany<WallEntityBase>(visibleArea);
            foreach (var wall in visibleWalls)
            {
                EntityDrawer.Draw(spriteBatch, map.Camera, wall);
            }
        }
    }
}