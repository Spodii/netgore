using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    public class TextBox : TextControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextBox"/> class.
        /// </summary>
        /// <param name="gui">GUIManager used by this Control.</param>
        /// <param name="settings">Settings for this TextControl.</param>
        /// <param name="font">SpriteFont used to write the text.</param>
        /// <param name="position">Position of the Control relative to its parent.</param>
        /// <param name="size">Size of the Control.</param>
        /// <param name="parent">Control that this Control belongs to.</param>
        public TextBox(GUIManagerBase gui, TextControlSettings settings, SpriteFont font, Vector2 position, Vector2 size,
                       Control parent)
            : base(gui, settings, "asdfasdfasdf", font, position, size, parent)
        {
            IsVisible = true;
            CanDrag = false;
            CanFocus = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBox"/> class.
        /// </summary>
        /// <param name="gui">GUIManager used by this Control.</param>
        /// <param name="font">SpriteFont used to write the text.</param>
        /// <param name="position">Position of the Control relative to its parent.</param>
        /// <param name="size">Size of the Control.</param>
        /// <param name="parent">Control that this Control belongs to.</param>
        public TextBox(GUIManagerBase gui, SpriteFont font, Vector2 position, Vector2 size,
                       Control parent)
            : this(gui, gui.TextBoxSettings, font, position, size, parent)
        {
        }

        public void Append(StyledText text)
        {
        }

        protected override void DrawControl(SpriteBatch spriteBatch)
        {
            base.DrawControl(spriteBatch);
        }
    }
}