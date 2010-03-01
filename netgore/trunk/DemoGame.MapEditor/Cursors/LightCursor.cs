using System.Drawing;
using System.Linq;
using DemoGame.MapEditor.Properties;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.EditorTools;
using NetGore.Graphics;
using Rectangle=Microsoft.Xna.Framework.Rectangle;
using Color=Microsoft.Xna.Framework.Graphics.Color;

namespace DemoGame.MapEditor
{
    sealed class LightCursor : EditorCursor<ScreenForm>
    {
        /// <summary>
        /// Gets the cursor's <see cref="Image"/>.
        /// </summary>
        public override Image CursorImage
        {
            get { return Resources.cursor_lights; }
        }

        /// <summary>
        /// When overridden in the derived class, gets the name of the cursor.
        /// </summary>
        public override string Name
        {
            get { return "Select Light"; }
        }

        /// <summary>
        /// Gets the priority of the cursor on the toolbar. Lower values appear first.
        /// </summary>
        public override int ToolbarPriority
        {
            get { return 40; }
        }

        public static Rectangle GetLightSelectionArea(ISpatial light)
        {
            return light.ToRectangle(16);
        }

        public static Rectangle GetCursorSelectionArea(Vector2 cursorPos)
        {
            return new Rectangle((int)cursorPos.X, (int)cursorPos.Y, 4, 4);
        }

        static readonly Color _closestLightRectColor = new Color(0, 255, 0);

        /// <summary>
        /// When overridden in the derived class, handles drawing the interface for the cursor, which is
        /// displayed over everything else. This can include the name of entities, selection boxes, etc.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        public override void DrawInterface(ISpriteBatch spriteBatch)
        {
            if (Container.DrawingManager.LightManager.IsEmpty())
                return;

            var cursorRect = GetCursorSelectionArea(Container.CursorPos);

            // Find the closest light
            var closestLight = Container.DrawingManager.LightManager.MinElement(x => x.GetDistance(cursorRect));
            if (closestLight == null)
                return;

            // Draw a rectangle around the closest light
            XNARectangle.Draw(spriteBatch, GetLightSelectionArea(closestLight), _closestLightRectColor, Color.Black);
        }
    }
}