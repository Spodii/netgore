using System.Linq;
using NetGore.Graphics;
using NetGore.World;

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
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the map being drawn.</param>
        protected override void HandleDrawAfterLayer(IDrawableMap map, MapRenderLayer layer, ISpriteBatch spriteBatch,
                                                     ICamera2D camera)
        {
            if (layer != MapRenderLayer.SpriteForeground)
                return;

            // Get the visible area
            var visibleArea = camera.GetViewArea();

            // Get and draw all entities except walls (they are drawn differently) and entities that can require drawing even when they
            // are not in view (e.g. TeleportEntity)
            var visibleEntities = map.Spatial.GetMany<Entity>(visibleArea, x => !(x is WallEntityBase || x is TeleportEntityBase));

            foreach (var entity in visibleEntities)
            {
                EntityDrawer.Draw(spriteBatch, camera, entity);
            }

            // Get and draw the TeleportEntities in view
            var visibleTeleportEntities = map.Spatial.GetMany<TeleportEntityBase>();

            foreach (var te in visibleTeleportEntities)
            {
                // If the source is in view, or if the destination is on the same map and in view, then draw
                if (camera.InView(te) || (te.DestinationMap == map.ID && camera.InView(te.Destination, te.Size)))
                    EntityDrawer.Draw(spriteBatch, camera, te);
            }
        }
    }
}