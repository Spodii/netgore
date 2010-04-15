using System.Linq;
using System.Windows.Forms;
using DemoGame.MapEditor.Properties;
using NetGore.EditorTools;
using NetGore.Graphics;
using SFML.Graphics;
using Image=System.Drawing.Image;

namespace DemoGame.MapEditor
{
    sealed class AddLightCursor : EditorCursor<ScreenForm>
    {
        static readonly ISprite _lightSprite = SystemSprites.Lightblub;

        /// <summary>
        /// Gets the cursor's <see cref="System.Drawing.Image"/>.
        /// </summary>
        public override Image CursorImage
        {
            get { return Resources.cursor_lightsadd; }
        }

        /// <summary>
        /// Gets the <see cref="ISprite"/> for the lightbulbs used by this cursor to represent lights.
        /// </summary>
        public static ISprite LightSprite
        {
            get { return _lightSprite; }
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
            _lightSprite.Draw(spriteBatch, cursorPos);
        }

        /// <summary>
        /// When overridden in the derived class, handles when a mouse button has been pressed.
        /// </summary>
        /// <param name="e">Mouse events.</param>
        public override void MouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            var light = new Light { Tag = Container.Map, Position = Container.CursorPos };
            Container.DrawingManager.LightManager.Add(light);
            Container.Map.AddLight(light);
        }
    }
}