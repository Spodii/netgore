using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;

namespace NetGore.Graphics.GUI
{
    public class Label : TextControl
    {
        bool _autoResize = false;
        ControlEventHandler _autoResizeHandler = null;

        /// <summary>
        /// Gets or sets if the label will automatically resize when the text or font changes
        /// </summary>
        public bool AutoResize
        {
            get { return _autoResize; }
            set
            {
                // If value is not different, abort
                if (_autoResize == value)
                    return;

                // Set the new value
                _autoResize = value;

                if (_autoResize)
                {
                    // Enable auto-resizing
                    if (_autoResizeHandler == null)
                        _autoResizeHandler = ResizeHandler;
                    OnChangeText += _autoResizeHandler;
                    OnChangeFont += _autoResizeHandler;
                }
                else
                {
                    // Disable auto-resizing
                    if (_autoResizeHandler != null)
                    {
                        OnChangeText -= _autoResizeHandler;
                        OnChangeFont -= _autoResizeHandler;
                    }
                }
            }
        }

        public Label(GUIManagerBase gui, LabelSettings settings, string text, SpriteFont font, Vector2 position, Vector2 size,
                     Control parent) : base(gui, settings, text, font, position, size, parent)
        {
            CanDrag = false;
            CanFocus = false;
            AutoResize = true;
        }

        public Label(GUIManagerBase gui, string text, SpriteFont font, Vector2 position, Vector2 size, Control parent)
            : this(gui, gui.LabelSettings, text, font, position, size, parent)
        {
        }

        public Label(GUIManagerBase gui, string text, SpriteFont font, Vector2 position, Control parent)
            : this(gui, text, font, position, font.MeasureString(text), parent)
        {
        }

        public Label(GUIManagerBase gui, LabelSettings settings, string text, SpriteFont font, Vector2 position, Control parent)
            : this(gui, settings, text, font, position, font.MeasureString(text), parent)
        {
        }

        public Label(string text, SpriteFont font, Vector2 position, Control parent)
            : this(parent.GUIManager, text, font, position, font.MeasureString(text), parent)
        {
        }

        public Label(LabelSettings settings, string text, SpriteFont font, Vector2 position, Control parent)
            : this(parent.GUIManager, settings, text, font, position, font.MeasureString(text), parent)
        {
        }

        public Label(string text, Vector2 position, Control parent) : this(text, parent.GUIManager.Font, position, parent)
        {
        }

        public Label(LabelSettings settings, string text, Vector2 position, Control parent)
            : this(settings, text, parent.GUIManager.Font, position, parent)
        {
        }

        protected override void DrawControl(SpriteBatch spriteBatch)
        {
            base.DrawControl(spriteBatch);
            DrawText(spriteBatch, Vector2.Zero);
        }

        void ResizeHandler(Control sender)
        {
            Size = Font.MeasureString(Text);
        }
    }
}