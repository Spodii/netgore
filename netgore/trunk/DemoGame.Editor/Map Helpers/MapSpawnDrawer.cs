using System.Collections.Generic;
using System.Linq;
using DemoGame.Server;
using NetGore.Graphics;
using SFML.Graphics;

namespace DemoGame.Editor
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
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="camera">The <see cref="ICamera2D"/> that describes the view of the map being drawn.</param>
        protected override void HandleDrawAfterLayer(IDrawableMap map, MapRenderLayer layer, ISpriteBatch spriteBatch,
                                                     ICamera2D camera)
        {
            if (layer != MapRenderLayer.SpriteForeground)
                return;

            if (MapSpawns == null)
                return;

            foreach (var item in MapSpawns)
            {
                var rect = item.SpawnArea.ToRectangle(map);
                if (camera.InView(rect))
                    RenderRectangle.Draw(spriteBatch, rect, _drawColor);
            }
        }
    }
}