using System.Collections.Generic;
using System.Linq;
using DemoGame.Server;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;

namespace DemoGame.MapEditor
{
    /// <summary>
    /// Draws the MapSpawnValues on a Map.
    /// </summary>
    public class MapSpawnDrawer : MapDrawingExtension
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
        /// When overridden in the derived class, handles drawing to the map after the given <paramref name="layer"/> is drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="layer">The layer that was just drawn.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> to draw to.</param>
        protected override void HandleDrawAfterLayer(IDrawableMap map, MapRenderLayer layer, SpriteBatch spriteBatch)
        {
            if (layer != MapRenderLayer.SpriteForeground)
                return;

            if (MapSpawns == null)
                return;

            foreach (MapSpawnValues item in MapSpawns)
            {
                Rectangle rect = item.SpawnArea.ToRectangle(map);
                if (map.Camera.InView(rect))
                    XNARectangle.Draw(spriteBatch, rect, _drawColor);
            }
        }
    }
}