using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DemoGame.Editor.Properties;
using NetGore.Editor;
using NetGore.Graphics;

namespace DemoGame.Editor
{
    sealed class xxAddLightCursor : EditorCursor<EditMapForm>
    {
        static readonly ISprite _lightSprite = SystemSprites.Lightblub;

        /// <summary>
        /// Gets the cursor's <see cref="System.Drawing.Image"/>.
        /// </summary>
        public override Image CursorImage
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the <see cref="ISprite"/> for the lightbulbs used by this cursor to represent lights.
        /// </summary>
        public static ISprite LightSprite
        {
            get { return _lightSprite; }
        }

        /// <summary>
        /// Property to access the MSC. Provided purely for the means of shortening the
        /// code
        /// </summary>
        MapScreenControl MSC
        {
            get { return Container.MapScreenControl; }
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
        /// When overridden in the derived class, allows for handling when the cursor becomes the active cursor.
        /// </summary>
        public override void Activate()
        {
            base.Activate();

            ClearState();
        }

        /// <summary>
        /// Completely clears the state of the cursor.
        /// </summary>
        void ClearState()
        {
        }

        /// <summary>
        /// When overridden in the derived class, allows for handling when the cursor is no longer the active cursor.
        /// </summary>
        public override void Deactivate()
        {
            base.Deactivate();

            ClearState();
        }

        /// <summary>
        /// When overridden in the derived class, handles drawing the interface for the cursor, which is
        /// displayed over everything else. This can include the name of entities, selection boxes, etc.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        public override void DrawInterface(ISpriteBatch spriteBatch)
        {
            var cursorPos = MSC.CursorPos;
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

            var light = new Light { Tag = MSC.Map, Position = MSC.CursorPos };
            MSC.DrawingManager.LightManager.Add(light);
            MSC.Map.AddLight(light);
        }
    }
}