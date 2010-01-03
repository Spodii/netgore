using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Provides drawing of the WallEntities on the Map.
    /// </summary>
    public class MapWallDrawer : MapDrawExtensionBase
    {
        /// <summary>
        /// When overridden in the derived class, handles additional drawing for a MapRenderLayer after the
        /// Map actually renders the layer.
        /// </summary>
        /// <param name="layer">The MapRenderLayer that was drawn.</param>
        /// <param name="spriteBatch">The SpriteBatch the Map used to draw.</param>
        /// <param name="camera">The camera that the Map used to draw.</param>
        protected override void EndDrawLayer(MapRenderLayer layer, SpriteBatch spriteBatch, ICamera2D camera)
        {
            if (layer != MapRenderLayer.SpriteForeground)
                return;

            foreach (WallEntityBase wall in Map.Entities.OfType<WallEntityBase>())
            {
                if (camera.InView(wall))
                    EntityDrawer.Draw(spriteBatch, wall);
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles additional drawing for a MapRenderLayer before the
        /// Map actually renders the layer.
        /// </summary>
        /// <param name="layer">The MapRenderLayer that is to be drawn.</param>
        /// <param name="spriteBatch">The SpriteBatch the Map used to draw.</param>
        /// <param name="camera">The camera that the Map used to draw.</param>
        protected override void StartDrawLayer(MapRenderLayer layer, SpriteBatch spriteBatch, ICamera2D camera)
        {
        }
    }
}