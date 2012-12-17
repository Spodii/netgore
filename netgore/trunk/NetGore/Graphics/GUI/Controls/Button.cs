using System;
using System.Linq;
using SFML.Graphics;
using SFML.Window;

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
        /// Gets or sets the ControlBorder used for when the button is in a normal state.
        /// </summary>
        public ControlBorder BorderNormal { get; set; }

        /// <summary>
        /// Gets or sets the ControlBorder used for when the mouse is over the control.
        /// </summary>
        public ControlBorder BorderOver { get; set; }

        /// <summary>
        /// Gets or sets the ControlBorder used for when the control is pressed.
        /// </summary>
        public ControlBorder BorderPressed { get; set; }

        /// <summary>
        /// Draw the Button.
        /// </summary>
        /// <param name="spriteBatch"><see cref="ISpriteBatch"/> to draw to.</param>
        protected override void DrawControl(ISpriteBatch spriteBatch)
        {
            base.DrawControl(spriteBatch);

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
            BorderNormal = skinManager.GetBorder(_controlSkinName);
            BorderOver = skinManager.GetBorder(_controlSkinName, "MouseOver");
            BorderPressed = skinManager.GetBorder(_controlSkinName, "Pressed");

            Border = BorderNormal;
        }

        /// <summary>
        /// Handles when the <see cref="TextControl.Font"/> has changed.
        /// This is called immediately before <see cref="TextControl.FontChanged"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.FontChanged"/> when possible.
        /// </summary>
        protected override void OnFontChanged()
        {
            base.OnFontChanged();

            UpdateTextSize();
        }

        /// <summary>
        /// Handles when a mouse button has been pressed down on this <see cref="Control"/>.
        /// This is called immediately before <see cref="Control.OnMouseDown"/>.
        /// Override this method instead of using an event hook on <see cref="Control.MouseDown"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (GUIManager.UnderCursor != this)
                Border = BorderNormal;
            else
            {
                if (e.Button == Mouse.Button.Left)
                    Border = BorderPressed;
                else
                    Border = BorderOver;
            }
        }

        /// <summary>
        /// Handles when the mouse has entered the area of the <see cref="Control"/>.
        /// This is called immediately before <see cref="Control.OnMouseEnter"/>.
        /// Override this method instead of using an event hook on <see cref="Control.MouseEnter"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseEnter(MouseMoveEventArgs e)
        {
            base.OnMouseEnter(e);

            if (GUIManager.UnderCursor == this)
                Border = BorderOver;
            else
                Border = BorderNormal;
        }

        /// <summary>
        /// Handles when the mouse has left the area of the <see cref="Control"/>.
        /// This is called immediately before <see cref="Control.OnMouseLeave"/>.
        /// Override this method instead of using an event hook on <see cref="Control.MouseLeave"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseLeave(MouseMoveEventArgs e)
        {
            base.OnMouseLeave(e);

            Border = BorderNormal;
        }

        /// <summary>
        /// Handles when the mouse has moved over the <see cref="Control"/>.
        /// This is called immediately before <see cref="Control.MouseMoved"/>.
        /// Override this method instead of using an event hook on <see cref="Control.MouseMoved"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseMoved(MouseMoveEventArgs e)
        {
            base.OnMouseMoved(e);

            if (GUIManager.UnderCursor != this)
                Border = BorderNormal;
            else
            {
                if (GUIManager.IsMouseButtonDown(Mouse.Button.Left))
                    Border = BorderPressed;
                else
                    Border = BorderOver;
            }
        }

        /// <summary>
        /// Handles when a mouse button has been raised on the <see cref="Control"/>.
        /// This is called immediately before <see cref="Control.OnMouseUp"/>.
        /// Override this method instead of using an event hook on <see cref="Control.MouseUp"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            if (GUIManager.UnderCursor == this)
                Border = BorderOver;
            else
                Border = BorderNormal;
        }

        /// <summary>
        /// Handles when the <see cref="Control.Size"/> of this <see cref="Control"/> has changed.
        /// This is called immediately before <see cref="Control.Resized"/>.
        /// Override this method instead of using an event hook on <see cref="Control.Resized"/> when possible.
        /// </summary>
        protected override void OnResized()
        {
            base.OnResized();

            UpdateTextPosition();
        }

        /// <summary>
        /// Handles when the <see cref="TextControl.Text"/> has changed.
        /// This is called immediately before <see cref="TextControl.TextChanged"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.TextChanged"/> when possible.
        /// </summary>
        protected override void OnTextChanged()
        {
            base.OnTextChanged();

            UpdateTextSize();
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
            Border = BorderNormal;
        }

        /// <summary>
        /// Updates teh position of the button's text.
        /// </summary>
        void UpdateTextPosition()
        {
            // Center the text on the control
            _textPos = ((Size / 2f) - (_textSize / 2f)).Round();
        }

        /// <summary>
        /// Updates the size and position of the button's text.
        /// </summary>
        void UpdateTextSize()
        {
            if (string.IsNullOrEmpty(Text))
                return;

            // Get the size of the text
            _textSize = Font.MeasureString(Text);

            UpdateTextPosition();
        }
    }
}