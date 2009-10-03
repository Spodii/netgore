using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetGore;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// A button control
    /// </summary>
    public class Button : TextControl
    {
        ControlBorder _borderOver = null;
        ControlBorder _borderPressed = null;
        ControlBorder _currentBorder;
        Vector2 _textPos = Vector2.Zero;
        Vector2 _textSize = Vector2.Zero;

        /// <summary>
        /// Gets or sets the ControlBorder used for when the mouse is over the control
        /// </summary>
        public ControlBorder BorderOver
        {
            get { return _borderOver; }
            set { _borderOver = value; }
        }

        /// <summary>
        /// Gets or sets the ControlBorder used for when the control is pressed
        /// </summary>
        public ControlBorder BorderPressed
        {
            get { return _borderPressed; }
            set { _borderPressed = value; }
        }

        /// <summary>
        /// Button constructor
        /// </summary>
        /// <param name="gui">GUIManager used by this Control</param>
        /// <param name="settings">Button settings</param>
        /// <param name="text">Text to display</param>
        /// <param name="font">SpriteFont used to write the text</param>
        /// <param name="position">Position of the Control relative to its parent</param>
        /// <param name="size">Size of the Control</param>
        /// <param name="parent">Control that this Control belongs to</param>
        public Button(GUIManagerBase gui, ButtonSettings settings, string text, SpriteFont font, Vector2 position, Vector2 size,
                      Control parent) : base(gui, settings, text, font, position, size, parent)
        {
            // Set the default behavior
            CanDrag = false;
            CanFocus = false;

            // Set the default borders
            _borderOver = GUIManager.ButtonSettings.MouseOver;
            _borderPressed = GUIManager.ButtonSettings.Pressed;
            _currentBorder = Border;

            // Set the event hooks
            OnMouseDown += Button_OnMouseDown;
            OnMouseUp += Button_OnMouseUp;
            OnLostFocus += Button_OnLostFocus;
            OnMouseMove += Button_OnMouseMove;
            OnMouseLeave += Button_OnMouseLeave;
            OnChangeText += Button_OnChangeText;
            OnChangeFont += Button_OnChangeFont;

            // Perform the initial text update
            UpdateTextSize();
        }

        /// <summary>
        /// Button constructor
        /// </summary>
        /// <param name="gui">GUIManager used by this Control</param>
        /// <param name="settings">Button settings</param>
        /// <param name="text">Text to display</param>
        /// <param name="font">SpriteFont used to write the text</param>
        /// <param name="position">Position of the Control relative to its parent</param>
        /// <param name="size">Size of the Control</param>
        public Button(GUIManagerBase gui, ButtonSettings settings, string text, SpriteFont font, Vector2 position, Vector2 size)
            : this(gui, settings, text, font, position, size, null)
        {
        }

        /// <summary>
        /// Button constructor
        /// </summary>
        /// <param name="gui">GUIManager used by this Control</param>
        /// <param name="settings">Button settings</param>
        /// <param name="text">Text to display</param>
        /// <param name="position">Position of the Control relative to its parent</param>
        /// <param name="size">Size of the Control</param>
        public Button(GUIManagerBase gui, ButtonSettings settings, string text, Vector2 position, Vector2 size)
            : this(gui, settings, text, gui.Font, position, size)
        {
        }

        /// <summary>
        /// Button constructor
        /// </summary>
        /// <param name="gui">GUIManager used by this Control</param>
        /// <param name="text">Text to display</param>
        /// <param name="position">Position of the Control relative to its parent</param>
        /// <param name="size">Size of the Control</param>
        public Button(GUIManagerBase gui, string text, Vector2 position, Vector2 size)
            : this(gui, gui.ButtonSettings, text, gui.Font, position, size)
        {
        }

        /// <summary>
        /// Button constructor
        /// </summary>
        /// <param name="text">Text to display</param>
        /// <param name="position">Position of the Control relative to its parent</param>
        /// <param name="size">Size of the Control</param>
        /// <param name="parent">Control that this Control belongs to</param>
        public Button(string text, Vector2 position, Vector2 size, Control parent)
            : this(parent.GUIManager, parent.GUIManager.ButtonSettings, text, parent.GUIManager.Font, position, size, parent)
        {
        }

        /// <summary>
        /// Button constructor
        /// </summary>
        /// <param name="settings">Button settings</param>
        /// <param name="text">Text to display</param>
        /// <param name="position">Position of the Control relative to its parent</param>
        /// <param name="size">Size of the Control</param>
        /// <param name="parent">Control that this Control belongs to</param>
        public Button(ButtonSettings settings, string text, Vector2 position, Vector2 size, Control parent)
            : this(parent.GUIManager, settings, text, parent.GUIManager.Font, position, size, parent)
        {
        }

        void Button_OnChangeFont(Control sender)
        {
            UpdateTextSize();
        }

        void Button_OnChangeText(Control sender)
        {
            UpdateTextSize();
        }

        void Button_OnLostFocus(Control sender)
        {
            _currentBorder = Border;
        }

        void Button_OnMouseDown(object sender, MouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _currentBorder = _borderPressed;
        }

        void Button_OnMouseLeave(object sender, MouseEventArgs e)
        {
            _currentBorder = Border;
        }

        void Button_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (GUIManager.MouseState.LeftButton == ButtonState.Released)
                _currentBorder = _borderOver;
        }

        void Button_OnMouseUp(object sender, MouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _currentBorder = _borderOver;
        }

        /// <summary>
        /// Draw the Button
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to</param>
        protected override void DrawControl(SpriteBatch spriteBatch)
        {
            // Find the border, and try using the default if null in case one of the
            // mouse over or pressed borders were not set
            ControlBorder border = _currentBorder ?? Border;

            // Draw the border
            if (border != null)
                border.Draw(spriteBatch, this);

            // Draw the text
            DrawText(spriteBatch, _textPos);
        }

        /// <summary>
        /// Updates the size and position of the button text
        /// </summary>
        void UpdateTextSize()
        {
            if (string.IsNullOrEmpty(Text))
                return;

            // Get the size of the text
            _textSize = Font.MeasureString(Text);

            // Center the text on the control
            _textPos = Size / 2 - _textSize / 2;

            // Make sure we have rounded values or else things get icky
            _textPos.X = (float)Math.Round(_textPos.X);
            _textPos.Y = (float)Math.Round(_textPos.Y);
        }
    }
}