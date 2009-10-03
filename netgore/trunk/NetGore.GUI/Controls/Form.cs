using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;

namespace NetGore.Graphics.GUI
{
    public class Form : TextControl
    {
        public Form(GUIManagerBase gui, FormSettings settings, string text, SpriteFont font, Vector2 position, Vector2 size,
                    Control parent) : base(gui, settings, text, font, position, size, parent)
        {
            // All constructors ultimately lead to this one
            ForeColor = settings.ForeColor;
        }

        public Form(GUIManagerBase gui, FormSettings settings, string text, SpriteFont font, Vector2 position, Vector2 size)
            : this(gui, settings, text, font, position, size, null)
        {
        }

        public Form(GUIManagerBase gui, FormSettings settings, string text, Vector2 position, Vector2 size)
            : this(gui, settings, text, gui.Font, position, size, null)
        {
        }

        public Form(GUIManagerBase gui, string text, Vector2 position, Vector2 size)
            : this(gui, gui.FormSettings, text, gui.Font, position, size)
        {
        }

        public Form(GUIManagerBase gui, string text, Vector2 position, Vector2 size, Control parent)
            : this(gui, gui.FormSettings, text, gui.Font, position, size, parent)
        {
        }

        public Form(GUIManagerBase gui, string text, SpriteFont font, Vector2 position, Vector2 size)
            : this(gui, gui.FormSettings, text, font, position, size)
        {
        }

        public Form(GUIManagerBase gui, string text, SpriteFont font, Vector2 position, Vector2 size, Control parent)
            : this(gui, gui.FormSettings, text, font, position, size, parent)
        {
        }

        protected override void DrawControl(SpriteBatch spriteBatch)
        {
            base.DrawControl(spriteBatch);

            DrawText(spriteBatch, new Vector2(3, 3));
        }
    }
}