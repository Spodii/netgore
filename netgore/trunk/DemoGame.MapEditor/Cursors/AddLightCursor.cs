using System.Drawing;
using System.Linq;
using DemoGame.MapEditor.Properties;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.EditorTools;
using NetGore.Graphics;
using Rectangle=Microsoft.Xna.Framework.Rectangle;

namespace DemoGame.MapEditor
{
    sealed class AddLightCursor : EditorCursor<ScreenForm>
    {
        static Grh _lightSprite;

        /// <summary>
        /// Gets the cursor's <see cref="Image"/>.
        /// </summary>
        public override Image CursorImage
        {
            get { return Resources.cursor_lightsadd; }
        }

        /// <summary>
        /// Gets the sprite to use for drawing lightbulbs.
        /// </summary>
        public static Grh LightSprite
        {
            get
            {
                if (_lightSprite == null)
                    _lightSprite = new Grh(GrhInfo.GetData("System", "Lightbulb"));

                return _lightSprite;
            }
        }

        /// <summary>
        /// When overridden in the derived class, gets the name of the cursor.
        /// </summary>
        public override string Name
        {
            get { return "Add Light"; }
        }

        /// <summary>
        /// Gets the priority of the cursor on the toolbar. Lower values appear first.
        /// </summary>
        public override int ToolbarPriority
        {
            get { return 45; }
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing the interface for the cursor, which is
        /// displayed over everything else. This can include the name of entities, selection boxes, etc.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        public override void DrawInterface(ISpriteBatch spriteBatch)
        {
            Vector2 cursorPos = Container.CursorPos;
            LightSprite.Draw(spriteBatch, cursorPos);
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been pressed.
        /// </summary>
        /// <param name="e">Mouse events.</param>
        public override void MouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            var light = new Light { Tag = Container.Map };
            light.Position = Container.CursorPos;
            Container.DrawingManager.LightManager.Add(light);
        }

        static Rectangle GetCursorSelectionArea(Vector2 cursorPos)
        {
            return LightCursor.GetCursorSelectionArea(cursorPos);
        }

        static Rectangle GetLightSelectionArea(ISpatial light)
        {
            return LightCursor.GetLightSelectionArea(light);
        }
    }
}