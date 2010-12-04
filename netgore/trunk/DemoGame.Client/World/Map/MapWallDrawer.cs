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
        /// When overridden in the derived class, handles drawing to the map after the given <see cref="MapRenderLayer"/> is drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="e">The <see cref="NetGore.Graphics.DrawableMapDrawLayerEventArgs"/> instance containing the event data.</param>
        protected override void HandleDrawAfterLayer(IDrawableMap map, DrawableMapDrawLayerEventArgs e)
        {
            if (e.Layer != MapRenderLayer.SpriteForeground)
                return;

            var visibleArea = e.Camera.GetViewArea();
            var visibleWalls = map.Spatial.GetMany<WallEntityBase>(visibleArea);
            foreach (var wall in visibleWalls)
            {
                EntityDrawer.Draw(e.SpriteBatch, e.Camera, wall);
            }
        }
    }
}