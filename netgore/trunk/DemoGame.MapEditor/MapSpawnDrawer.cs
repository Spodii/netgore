using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoGame.Server;
using Microsoft.Xna.Framework.Graphics;
using NetGore.Graphics;

namespace DemoGame.Client
{
    public class MapSpawnDrawer : MapDrawExtensionBase
    {
        static readonly Color _drawColor = new Color(0, 255, 0, 90);

        public IEnumerable<MapSpawnValues> MapSpawnValues { get; set; }

        protected override void StartDrawLayer(MapRenderLayer layer, SpriteBatch spriteBatch, Camera2D camera, bool isDrawing)
        {
        }

        protected override void EndDrawLayer(MapRenderLayer layer, SpriteBatch spriteBatch, Camera2D camera, bool isDrawing)
        {
            if (layer != MapRenderLayer.SpriteForeground)
                return;

            if (MapSpawnValues == null)
                return;

            foreach (var item in MapSpawnValues)
            {
                var rect = item.SpawnArea.ToRectangle(Map);
                XNARectangle.Draw(spriteBatch, rect, _drawColor);
            }
        }
    }
}
