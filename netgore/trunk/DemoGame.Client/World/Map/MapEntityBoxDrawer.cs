using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Graphics;

namespace DemoGame.Client
{
    /// <summary>
    /// Provides informational drawing for the Entities on a Map.
    /// </summary>
    public class MapEntityBoxDrawer : MapDrawExtensionBase
    {
        /// <summary>
        /// When overridden in the derived class, handles additional drawing for a MapRenderLayer after the
        /// Map actually renders the layer.
        /// </summary>
        /// <param name="layer">The MapRenderLayer that was drawn.</param>
        /// <param name="spriteBatch">The SpriteBatch the Map used to draw.</param>
        /// <param name="camera">The camera that the Map used to draw.</param>
        /// <param name="isDrawing">If true, the Map actually drew this layer. If false, it is time for this
        /// <paramref name="layer"/> to be drawn, but the Map did not actually draw it.</param>
        protected override void EndDrawLayer(MapRenderLayer layer, SpriteBatch spriteBatch, ICamera2D camera, bool isDrawing)
        {
            if (layer != MapRenderLayer.SpriteForeground)
                return;

            foreach (Entity entity in Map.Entities)
            {
                if (entity is WallEntityBase)
                    continue;

                EntityDrawer.Draw(spriteBatch, entity);
            }
        }

        /// <summary>
        /// When overridden in the derived class, handles additional drawing for a MapRenderLayer before the
        /// Map actually renders the layer.
        /// </summary>
        /// <param name="layer">The MapRenderLayer that is to be drawn.</param>
        /// <param name="spriteBatch">The SpriteBatch the Map used to draw.</param>
        /// <param name="camera">The camera that the Map used to draw.</param>
        /// <param name="isDrawing">If true, the Map actually drew this layer. If false, it is time for this
        /// <paramref name="layer"/> to be drawn, but the Map did not actually draw it.</param>
        protected override void StartDrawLayer(MapRenderLayer layer, SpriteBatch spriteBatch, ICamera2D camera, bool isDrawing)
        {
        }
    }
}