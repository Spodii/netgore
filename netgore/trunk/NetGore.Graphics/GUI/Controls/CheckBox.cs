using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// A control containing a checkbox and a line of text to label it
    /// </summary>
    public class CheckBox : Label
    {
        /// <summary>
        /// The name of this <see cref="Control"/> for when looking up the skin information.
        /// </summary>
        const string _controlSkinName = "CheckBox";

        /// <summary>
        /// Amount of space added between the check box and text
        /// </summary>
        const float _textXAdjust = 2f;

        static readonly object _eventChangeTickedOverSprite = new object();
        static readonly object _eventChangeTickedPressedSprite = new object();
        static readonly object _eventChangeTickedSprite = new object();
        static readonly object _eventChangeUntickedOverSprite = new object();
        static readonly object _eventChangeUntickedPressedSprite = new object();
        static readonly object _eventChangeUntickedSprite = new object();
        static readonly object _eventChangeValue = new object();

        CheckBoxState _state = CheckBoxState.None;

        ISprite _ticked;
        ISprite _tickedOver;
        ISprite _tickedPressed;
        ISprite _unticked;
        ISprite _untickedOver;
        ISprite _untickedPressed;
        bool _value = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        public CheckBox(Control parent, Vector2 position) : base(parent, position)
        {
            HandleAutoResize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        public CheckBox(IGUIManager guiManager, Vector2 position) : base(guiManager, position)
        {
            HandleAutoResize();
        }

        /// <summary>
        /// Notifies listeners when the <see cref="CheckBox.TickedOverSprite"/> changes.
        /// </summary>
        public event ControlEventHandler OnChangeTickedOverSprite
        {
            add { Events.AddHandler(_eventChangeTickedOverSprite, value); }
            remove { Events.RemoveHandler(_eventChangeTickedOverSprite, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="CheckBox.TickedPressedSprite"/> changes.
        /// </summary>
        public event ControlEventHandler OnChangeTickedPressedSprite
        {
            add { Events.AddHandler(_eventChangeTickedPressedSprite, value); }
            remove { Events.RemoveHandler(_eventChangeTickedPressedSprite, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="CheckBox.TickedSprite"/> changes.
        /// </summary>
        public event ControlEventHandler OnChangeTickedSprite
        {
            add { Events.AddHandler(_eventChangeTickedSprite, value); }
            remove { Events.RemoveHandler(_eventChangeTickedSprite, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="CheckBox.UntickedOverSprite"/> changes.
        /// </summary>
        public event ControlEventHandler OnChangeUntickedOverSprite
        {
            add { Events.AddHandler(_eventChangeUntickedOverSprite, value); }
            remove { Events.RemoveHandler(_eventChangeUntickedOverSprite, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="CheckBox.UntickedPressedSprite"/> changes.
        /// </summary>
        public event ControlEventHandler OnChangeUntickedPressedSprite
        {
            add { Events.AddHandler(_eventChangeUntickedPressedSprite, value); }
            remove { Events.RemoveHandler(_eventChangeUntickedPressedSprite, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="CheckBox.UntickedSprite"/> changes.
        /// </summary>
        public event ControlEventHandler OnChangeUntickedSprite
        {
            add { Events.AddHandler(_eventChangeUntickedSprite, value); }
            remove { Events.RemoveHandler(_eventChangeUntickedSprite, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="CheckBox.Value"/> changes.
        /// </summary>
        public event ControlEventHandler OnChangeValue
        {
            add { Events.AddHandler(_eventChangeValue, value); }
            remove { Events.RemoveHandler(_eventChangeValue, value); }
        }

        /// <summary>
        /// Gets or sets the Sprite used for a ticked checkbox with the mouse over it
        /// </summary>
        public ISprite TickedOverSprite
        {
            get { return _ticked; }
            set
            {
                if (_tickedOver == value)
                    return;

                _tickedOver = value;
                InvokeChangeTickedOverSprite();
            }
        }

        /// <summary>
        /// Gets or sets the Sprite used for a ticked checkbox being pressed
        /// </summary>
        public ISprite TickedPressedSprite
        {
            get { return _ticked; }
            set
            {
                if (_ticked == value)
                    return;

                _ticked = value;
                InvokeChangeTickedPressedSprite();
            }
        }

        /// <summary>
        /// Gets or sets the Sprite used for a ticked checkbox
        /// </summary>
        public ISprite TickedSprite
        {
            get { return _ticked; }
            set
            {
                if (_ticked == value)
                    return;

                _ticked = value;
                InvokeChangeTickedSprite();
            }
        }

        /// <summary>
        /// Gets or sets the Sprite used for an unticked checkbox with the mouse over it
        /// </summary>
        public ISprite UntickedOverSprite
        {
            get { return _untickedOver; }
            set
            {
                if (_untickedOver == value)
                    return;

                _untickedOver = value;
                InvokeChangeUntickedOverSprite();
            }
        }

        /// <summary>
        /// Gets or sets the Sprite used for an unticked checkbox being pressed
        /// </summary>
        public ISprite UntickedPressedSprite
        {
            get { return _untickedPressed; }
            set
            {
                if (_untickedPressed == value)
                    return;

                _untickedPressed = value;
                InvokeChangeUntickedPressedSprite();
            }
        }

        /// <summary>
        /// Gets or sets the Sprite used for an unticked checkbox
        /// </summary>
        public ISprite UntickedSprite
        {
            get { return _unticked; }
            set
            {
                if (_unticked == value)
                    return;

                _unticked = value;
                InvokeChangeUntickedSprite();
            }
        }

        /// <summary>
        /// Gets or sets if the <see cref="CheckBox"/> is ticked.
        /// </summary>
        public bool Value
        {
            get { return _value; }
            set
            {
                if (_value == value)
                    return;

                _value = value;
                InvokeChangeValue();
            }
        }

        /// <summary>
        /// Handles when the <see cref="TextControl.Font"/> has changed.
        /// This is called immediately before <see cref="TextControl.OnChangeFont"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.OnChangeFont"/> when possible.
        /// </summary>
        protected override void ChangeFont()
        {
            base.ChangeFont();

            HandleAutoResize();
        }

        /// <summary>
        /// Handles when the <see cref="TextControl.Text"/> has changed.
        /// This is called immediately before <see cref="TextControl.OnChangeText"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.OnChangeText"/> when possible.
        /// </summary>
        protected override void ChangeText()
        {
            base.ChangeText();

            HandleAutoResize();
        }

        /// <summary>
        /// Handles when the <see cref="CheckBox.TickedOverSprite"/> changes.
        /// This is called immediately before <see cref="CheckBox.OnChangeTickedOverSprite"/>.
        /// Override this method instead of using an event hook on <see cref="CheckBox.OnChangeTickedOverSprite"/> when possible.
        /// </summary>
        protected virtual void ChangeTickedOverSprite()
        {
            HandleAutoResize();
        }

        /// <summary>
        /// Handles when the <see cref="CheckBox.TickedPressedSprite"/> changes.
        /// This is called immediately before <see cref="CheckBox.OnChangeTickedPressedSprite"/>.
        /// Override this method instead of using an event hook on <see cref="CheckBox.OnChangeTickedPressedSprite"/> when possible.
        /// </summary>
        protected virtual void ChangeTickedPressedSprite()
        {
            HandleAutoResize();
        }

        /// <summary>
        /// Handles when the <see cref="CheckBox.TickedSprite"/> changes.
        /// This is called immediately before <see cref="CheckBox.OnChangeTickedSprite"/>.
        /// Override this method instead of using an event hook on <see cref="CheckBox.OnChangeTickedSprite"/> when possible.
        /// </summary>
        protected virtual void ChangeTickedSprite()
        {
            HandleAutoResize();
        }

        /// <summary>
        /// Handles when the <see cref="CheckBox.UntickedOverSprite"/> changes.
        /// This is called immediately before <see cref="CheckBox.OnChangeUntickedOverSprite"/>.
        /// Override this method instead of using an event hook on <see cref="CheckBox.OnChangeUntickedOverSprite"/> when possible.
        /// </summary>
        protected virtual void ChangeUntickedOverSprite()
        {
            HandleAutoResize();
        }

        /// <summary>
        /// Handles when the <see cref="CheckBox.UntickedPressedSprite"/> changes.
        /// This is called immediately before <see cref="CheckBox.OnChangeUntickedPressedSprite"/>.
        /// Override this method instead of using an event hook on <see cref="CheckBox.OnChangeUntickedPressedSprite"/> when possible.
        /// </summary>
        protected virtual void ChangeUntickedPressedSprite()
        {
            HandleAutoResize();
        }

        /// <summary>
        /// Handles when the <see cref="CheckBox.UntickedSprite"/> changes.
        /// This is called immediately before <see cref="CheckBox.OnChangeUntickedSprite"/>.
        /// Override this method instead of using an event hook on <see cref="CheckBox.OnChangeUntickedSprite"/> when possible.
        /// </summary>
        protected virtual void ChangeUntickedSprite()
        {
            HandleAutoResize();
        }

        /// <summary>
        /// Handles when the <see cref="CheckBox.Value"/> changes.
        /// This is called immediately before <see cref="CheckBox.OnChangeValue"/>.
        /// Override this method instead of using an event hook on <see cref="CheckBox.OnChangeValue"/> when possible.
        /// </summary>
        protected virtual void ChangeValue()
        {
        }

        /// <summary>
        /// Draws the control
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to</param>
        protected override void DrawControl(SpriteBatch spriteBatch)
        {
            // Draw the border
            Border.Draw(spriteBatch, this);

            // Find the sprite based on the state and value
            ISprite sprite;
            if (Value)
            {
                if (_state == CheckBoxState.Pressed)
                    sprite = _tickedPressed;
                else if (_state == CheckBoxState.Over)
                    sprite = _tickedOver;
                else
                    sprite = _ticked;

                if (sprite == null)
                    sprite = _ticked;
            }
            else
            {
                if (_state == CheckBoxState.Pressed)
                    sprite = _untickedPressed;
                else if (_state == CheckBoxState.Over)
                    sprite = _untickedOver;
                else
                    sprite = _unticked;

                if (sprite == null)
                    sprite = _unticked;
            }

            // Validate the sprite
            if (sprite == null)
                return;

            // Find the text offset
            Vector2 textOffset = new Vector2(sprite.Source.Width + _textXAdjust, 0);

            // Draw the checkbox
            if (sprite.Texture != null)
                spriteBatch.Draw(sprite.Texture, ScreenPosition, sprite.Source, Color.White);

            // Draw the text
            DrawText(spriteBatch, textOffset);
        }

        /// <summary>
        /// Handles control resizing.
        /// </summary>
        void HandleAutoResize()
        {
            // Start with the size of the text
            Vector2 textSize = Font.MeasureString(Text);

            // Add the width of checkbox
            textSize.X += Math.Max(_unticked.Source.Width, _ticked.Source.Width);

            // Set the height to the largest of either the text or checkbox
            textSize.Y = Math.Max(textSize.Y, Math.Max(_unticked.Source.Height, _ticked.Source.Height));

            // Add the space between the checkbox and text
            textSize.X += _textXAdjust;

            // Set the new size
            Size = textSize;
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeChangeTickedOverSprite()
        {
            ChangeTickedOverSprite();
            var handler = Events[_eventChangeTickedOverSprite] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeChangeTickedPressedSprite()
        {
            ChangeTickedPressedSprite();
            var handler = Events[_eventChangeTickedPressedSprite] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeChangeTickedSprite()
        {
            ChangeTickedSprite();
            var handler = Events[_eventChangeTickedSprite] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeChangeUntickedOverSprite()
        {
            ChangeUntickedOverSprite();
            var handler = Events[_eventChangeUntickedOverSprite] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeChangeUntickedPressedSprite()
        {
            ChangeUntickedPressedSprite();
            var handler = Events[_eventChangeUntickedPressedSprite] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeChangeUntickedSprite()
        {
            ChangeUntickedSprite();
            var handler = Events[_eventChangeUntickedSprite] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeChangeValue()
        {
            ChangeValue();
            var handler = Events[_eventChangeValue] as ControlEventHandler;
            if (handler != null)
                handler(this);
        }

        /// <summary>
        /// When overridden in the derived class, loads the skinning information for the <see cref="Control"/>
        /// from the given <paramref name="skinManager"/>.
        /// </summary>
        /// <param name="skinManager">The <see cref="ISkinManager"/> to load the skinning information from.</param>
        public override void LoadSkin(ISkinManager skinManager)
        {
            base.LoadSkin(skinManager);

            _ticked = skinManager.GetControlSprite(_controlSkinName, "Ticked");
            _tickedOver = skinManager.GetControlSprite(_controlSkinName, "TickedMouseOver");
            _tickedPressed = skinManager.GetControlSprite(_controlSkinName, "TickedPressed");

            _unticked = skinManager.GetControlSprite(_controlSkinName, "Unticked");
            _untickedOver = skinManager.GetControlSprite(_controlSkinName, "UntickedMouseOver");
            _untickedPressed = skinManager.GetControlSprite(_controlSkinName, "UntickedPressed");
        }

        /// <summary>
        /// Handles when the <see cref="Control"/> has lost focus.
        /// This is called immediately before <see cref="Control.OnLostFocus"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnLostFocus"/> when possible.
        /// </summary>
        protected override void LostFocus()
        {
            base.LostFocus();

            _state = CheckBoxState.None;
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
            {
                Value = !_value;
                _state = CheckBoxState.Pressed;
            }
        }

        /// <summary>
        /// Handles when the mouse has entered the area of the <see cref="Control"/>.
        /// This is called immediately before <see cref="Control.OnMouseEnter"/>.
        /// Override this method instead of using an event hook on <see cref="Control.OnMouseEnter"/> when possible.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void MouseEnter(MouseEventArgs e)
        {
            base.MouseEnter(e);

            _state = CheckBoxState.Over;
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

            _state = CheckBoxState.None;
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

            _state = CheckBoxState.Over;
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

            _state = CheckBoxState.Over;
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            CanFocus = true;
        }

        enum CheckBoxState
        {
            None,
            Over,
            Pressed
        }
    }
}