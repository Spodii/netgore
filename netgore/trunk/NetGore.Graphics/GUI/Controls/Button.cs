using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// A standard static button control.
    /// </summary>
    public class Button : TextControl
    {
        /// <summary>
        /// The name of this <see cref="Control"/> for when looking up the skin information.
        /// </summary>
        const string _controlSkinName = "Button";

        ControlBorder _borderOver = null;
        ControlBorder _borderPressed = null;
        ControlBorder _currentBorder;
        Vector2 _textPos = Vector2.Zero;
        Vector2 _textSize = Vector2.Zero;

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        public Button(Control parent, Vector2 position, Vector2 clientSize) : base(parent, position, clientSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        public Button(IGUIManager guiManager, Vector2 position, Vector2 clientSize) : base(guiManager, position, clientSize)
        {
        }

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
        /// Handles when the <see cref="TextControl.Font"/> has changed.
        /// This is called immediately before <see cref="TextControl.OnChangeFont"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.OnChangeFont"/> when possible.
        /// </summary>
        protected override void ChangeFont()
        {
            base.ChangeFont();

            UpdateTextSize();
        }

        /// <summary>
        /// Handles when the <see cref="TextControl.Text"/> has changed.
        /// This is called immediately before <see cref="TextControl.OnChangeText"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.OnChangeText"/> when possible.
        /// </summary>
        protected override void ChangeText()
        {
            base.ChangeText();

            UpdateTextSize();
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
        /// When overridden in the derived class, loads the skinning information for the <see cref="Control"/>
        /// from the given <paramref name="skinManager"/>.
        /// </summary>
        /// <param name="skinManager">The <see cref="ISkinManager"/> to load the skinning information from.</param>
        public override void LoadSkin(ISkinManager skinManager)
        {
            Border = skinManager.GetBorder(_controlSkinName);
            BorderOver = skinManager.GetBorder(_controlSkinName, "MouseOver");
            BorderPressed = skinManager.GetBorder(_controlSkinName, "Pressed");
        }

        /// <summary>
        /// Handles when the <see cref="Control"/> has lost focus.
        /// This is called immediately before <see cref="Control.OnLostFocus"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnLostFocus"/> when possible.
        /// </summary>
        protected override void LostFocus()
        {
            base.LostFocus();

            _currentBorder = Border;
        }

        /// <summary>
        /// Handles when a mouse button has been pressed down on this <see cref="Control"/>.
        /// This is called immediately before <see cref="Control.OnMouseDown"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnMouseDown"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void MouseDown(MouseClickEventArgs e)
        {
            base.MouseDown(e);

            if (e.Button == MouseButtons.Left)
                _currentBorder = _borderPressed;
        }

        /// <summary>
        /// Handles when the mouse has left the area of the <see cref="Control"/>.
        /// This is called immediately before <see cref="Control.OnMouseLeave"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnMouseLeave"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void MouseLeave(MouseEventArgs e)
        {
            base.MouseLeave(e);

            _currentBorder = Border;
        }

        /// <summary>
        /// Handles when the mouse has moved over the <see cref="Control"/>.
        /// This is called immediately before <see cref="Control.OnMouseMove"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnMouseMove"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void MouseMove(MouseEventArgs e)
        {
            base.MouseMove(e);

            if (GUIManager.MouseState.LeftButton == ButtonState.Released)
                _currentBorder = _borderOver;
        }

        /// <summary>
        /// Handles when a mouse button has been raised on the <see cref="Control"/>.
        /// This is called immediately before <see cref="Control.OnMouseUp"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnMouseUp"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void MouseUp(MouseClickEventArgs e)
        {
            base.MouseUp(e);

            if (e.Button == MouseButtons.Left)
                _currentBorder = _borderOver;
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            CanDrag = false;
            CanFocus = false;
            _currentBorder = Border;
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