using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemoGame.Server;
using NetGore;
using NetGore.Graphics;
using SFML.Graphics;

namespace DemoGame.Editor
{
    /// <summary>
    /// Assists in drawing different types of Entities.
    /// Like EntityDrawer in the client, but for types not available from the client.
    /// </summary>
    public static class EditorEntityDrawer
    {
        public static void DrawSpawn(MapSpawnValues spawn, ISpriteBatch spriteBatch, IDrawableMap map, Font font)
        {
            var spawnArea = spawn.SpawnArea;

            // Only draw if it does not cover the whole map
            if (!spawnArea.X.HasValue && !spawnArea.Y.HasValue && !spawnArea.Width.HasValue && !spawnArea.Height.HasValue)
                return;

            // Draw spawn area
            Vector2 cameraOffset = map.Camera.Min;
            Rectangle rect = spawnArea.ToRectangle(map);
            rect.X -= (int)cameraOffset.X;
            rect.Y -= (int)cameraOffset.Y;
            RenderRectangle.Draw(spriteBatch, rect, new Color(0, 255, 0, 75), new Color(0, 0, 0, 125), 2);

            // Draw name
            CharacterTemplate charTemp = CharacterTemplateManager.Instance[spawn.CharacterTemplateID];
            if (charTemp != null)
            {
                string text = string.Format("{0}x {1} [{2}]", spawn.SpawnAmount, charTemp.TemplateTable.Name, spawn.CharacterTemplateID);

                Vector2 textPos = new Vector2(rect.X, rect.Y);
                textPos -= new Vector2(0, font.MeasureString(text).Y);
                textPos = textPos.Round();

                spriteBatch.DrawStringShaded(font, text, textPos, Color.White, Color.Black);
            }
        }
    }
}
