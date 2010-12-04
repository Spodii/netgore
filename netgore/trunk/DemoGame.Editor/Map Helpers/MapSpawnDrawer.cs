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
        /// When overridden in the derived class, handles drawing to the map after the given <see cref="MapRenderLayer"/> is drawn.
        /// </summary>
        /// <param name="map">The map the drawing is taking place on.</param>
        /// <param name="e">The <see cref="NetGore.Graphics.DrawableMapDrawLayerEventArgs"/> instance containing the event data.</param>
        protected override void HandleDrawAfterLayer(IDrawableMap map, DrawableMapDrawLayerEventArgs e)
        {
            if (e.Layer != MapRenderLayer.SpriteForeground)
                return;

            if (MapSpawns == null)
                return;

            foreach (var item in MapSpawns)
            {
                var rect = item.SpawnArea.ToRectangle(map);
                if (e.Camera.InView(rect))
                    RenderRectangle.Draw(e.SpriteBatch, rect, _drawColor);
            }
        }
    }
}