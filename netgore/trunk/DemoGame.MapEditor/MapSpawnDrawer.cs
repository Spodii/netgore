using System.Collections.Generic;
using System.Linq;
using DemoGame.Client;
using DemoGame.Server;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;

namespace DemoGame.MapEditor
{
    /// <summary>
    /// Draws the MapSpawnValues on a Map.
    /// </summary>
    public class MapSpawnDrawer : MapDrawExtensionBase
    {
        /// <summary>
        /// The color to draw the spawn rects.
        /// </summary>
        static readonly Color _drawColor = new Color(0, 255, 0, 90);

        /// <summary>
        /// Gets or sets an IEnumerable of the MapSpawnValues to draw.
        /// </summary>
        public IEnumerable<MapSpawnValues> MapSpawns { get; set; }

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

            if (MapSpawns == null)
                return;

            foreach (MapSpawnValues item in MapSpawns)
            {
                Rectangle rect = item.SpawnArea.ToRectangle(Map);
                XNARectangle.Draw(spriteBatch, rect, _drawColor);
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