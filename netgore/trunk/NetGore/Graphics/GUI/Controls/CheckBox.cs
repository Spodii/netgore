using System;
using System.Linq;
using SFML.Graphics;
using SFML.Window;

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

        static readonly object _eventTickedOverSpriteChanged = new object();
        static readonly object _eventTickedPressedSpriteChanged = new object();
        static readonly object _eventTickedSpriteChanged = new object();
        static readonly object _eventUntickedOverSpriteChanged = new object();
        static readonly object _eventUntickedPressedSpriteChanged = new object();
        static readonly object _eventUntickedSpriteChanged = new object();
        static readonly object _eventValueChanged = new object();

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
        public event TypedEventHandler<Control> TickedOverSpriteChanged
        {
            add { Events.AddHandler(_eventTickedOverSpriteChanged, value); }
            remove { Events.RemoveHandler(_eventTickedOverSpriteChanged, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="CheckBox.TickedPressedSprite"/> changes.
        /// </summary>
        public event TypedEventHandler<Control> TickedPressedSpriteChanged
        {
            add { Events.AddHandler(_eventTickedPressedSpriteChanged, value); }
            remove { Events.RemoveHandler(_eventTickedPressedSpriteChanged, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="CheckBox.TickedSprite"/> changes.
        /// </summary>
        public event TypedEventHandler<Control> TickedSpriteChanged
        {
            add { Events.AddHandler(_eventTickedSpriteChanged, value); }
            remove { Events.RemoveHandler(_eventTickedSpriteChanged, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="CheckBox.UntickedOverSprite"/> changes.
        /// </summary>
        public event TypedEventHandler<Control> UntickedOverSpriteChanged
        {
            add { Events.AddHandler(_eventUntickedOverSpriteChanged, value); }
            remove { Events.RemoveHandler(_eventUntickedOverSpriteChanged, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="CheckBox.UntickedPressedSprite"/> changes.
        /// </summary>
        public event TypedEventHandler<Control> UntickedPressedSpriteChanged
        {
            add { Events.AddHandler(_eventUntickedPressedSpriteChanged, value); }
            remove { Events.RemoveHandler(_eventUntickedPressedSpriteChanged, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="CheckBox.UntickedSprite"/> changes.
        /// </summary>
        public event TypedEventHandler<Control> UntickedSpriteChanged
        {
            add { Events.AddHandler(_eventUntickedSpriteChanged, value); }
            remove { Events.RemoveHandler(_eventUntickedSpriteChanged, value); }
        }

        /// <summary>
        /// Notifies listeners when the <see cref="CheckBox.Value"/> changes.
        /// </summary>
        public event TypedEventHandler<Control> ValueChanged
        {
            add { Events.AddHandler(_eventValueChanged, value); }
            remove { Events.RemoveHandler(_eventValueChanged, value); }
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
                InvokeTickedOverSpriteChanged();
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
                InvokeTickedPressedSpriteChanged();
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
                InvokeTickedSpriteChanged();
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
                InvokeUntickedOverSpriteChanged();
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
                InvokeUntickedPressedSpriteChanged();
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
                InvokeUntickedSpriteChanged();
            }
        }

        /// <summary>
        /// Gets or sets if the <see cref="CheckBox"/> is ticked.
        /// </summary>
        [SyncValue]
        public bool Value
        {
            get { return _value; }
            set
            {
                if (_value == value)
                    return;

                _value = value;
                InvokeValueChanged();
            }
        }

        /// <summary>
        /// Draws the control.
        /// </summary>
        /// <param name="spriteBatch"><see cref="ISpriteBatch"/> to draw to.</param>
        protected override void DrawControl(ISpriteBatch spriteBatch)
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
            var textOffset = new Vector2(sprite.Source.Width + _textXAdjust, 0);

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
            var textSize = Font.MeasureString(Text);

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
        void InvokeTickedOverSpriteChanged()
        {
            OnTickedOverSpriteChanged();
            var handler = Events[_eventTickedOverSpriteChanged] as TypedEventHandler<Control>;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeTickedPressedSpriteChanged()
        {
            OnTickedPressedSpriteChanged();
            var handler = Events[_eventTickedPressedSpriteChanged] as TypedEventHandler<Control>;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeTickedSpriteChanged()
        {
            OnTickedSpriteChanged();
            var handler = Events[_eventTickedSpriteChanged] as TypedEventHandler<Control>;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeUntickedOverSpriteChanged()
        {
            OnUntickedOverSpriteChanged();
            var handler = Events[_eventUntickedOverSpriteChanged] as TypedEventHandler<Control>;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeUntickedPressedSpriteChanged()
        {
            OnUntickedPressedSpriteChanged();
            var handler = Events[_eventUntickedPressedSpriteChanged] as TypedEventHandler<Control>;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeUntickedSpriteChanged()
        {
            OnUntickedSpriteChanged();
            var handler = Events[_eventUntickedSpriteChanged] as TypedEventHandler<Control>;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Invokes the corresponding virtual method and event for the given event. Use this instead of invoking
        /// the virtual method and event directly to ensure that the event is invoked correctly.
        /// </summary>
        void InvokeValueChanged()
        {
            OnValueChanged();
            var handler = Events[_eventValueChanged] as TypedEventHandler<Control>;
            if (handler != null)
                handler(this, EventArgs.Empty);
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
        /// Handles when the <see cref="TextControl.Font"/> has changed.
        /// This is called immediately before <see cref="TextControl.FontChanged"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.FontChanged"/> when possible.
        /// </summary>
        protected override void OnFontChanged()
        {
            base.OnFontChanged();

            HandleAutoResize();
        }

        /// <summary>
        /// Handles when the <see cref="Control"/> has lost focus.
        /// This is called immediately before <see cref="Control.OnLostFocus"/>.
        /// Override this method instead of using an event hook on <see cref="Control.LostFocus"/> when possible.
        /// </summary>
        protected override void OnLostFocus()
        {
            base.OnLostFocus();

            _state = CheckBoxState.None;
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

            if (e.Button == Mouse.Button.Left)
            {
                Value = !_value;
                _state = CheckBoxState.Pressed;
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

            _state = CheckBoxState.Over;
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

            _state = CheckBoxState.None;
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

            _state = CheckBoxState.Over;
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

            _state = CheckBoxState.Over;
        }

        /// <summary>
        /// Handles when the <see cref="TextControl.Text"/> has changed.
        /// This is called immediately before <see cref="TextControl.TextChanged"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.TextChanged"/> when possible.
        /// </summary>
        protected override void OnTextChanged()
        {
            base.OnTextChanged();

            HandleAutoResize();
        }

        /// <summary>
        /// Handles when the <see cref="CheckBox.TickedOverSprite"/> changes.
        /// This is called immediately before <see cref="CheckBox.TickedOverSpriteChanged"/>.
        /// Override this method instead of using an event hook on <see cref="CheckBox.TickedOverSpriteChanged"/> when possible.
        /// </summary>
        protected virtual void OnTickedOverSpriteChanged()
        {
            HandleAutoResize();
        }

        /// <summary>
        /// Handles when the <see cref="CheckBox.TickedPressedSprite"/> changes.
        /// This is called immediately before <see cref="CheckBox.TickedPressedSpriteChanged"/>.
        /// Override this method instead of using an event hook on <see cref="CheckBox.TickedPressedSpriteChanged"/> when possible.
        /// </summary>
        protected virtual void OnTickedPressedSpriteChanged()
        {
            HandleAutoResize();
        }

        /// <summary>
        /// Handles when the <see cref="CheckBox.TickedSprite"/> changes.
        /// This is called immediately before <see cref="CheckBox.TickedSpriteChanged"/>.
        /// Override this method instead of using an event hook on <see cref="CheckBox.TickedSpriteChanged"/> when possible.
        /// </summary>
        protected virtual void OnTickedSpriteChanged()
        {
            HandleAutoResize();
        }

        /// <summary>
        /// Handles when the <see cref="CheckBox.UntickedOverSprite"/> changes.
        /// This is called immediately before <see cref="CheckBox.UntickedOverSpriteChanged"/>.
        /// Override this method instead of using an event hook on <see cref="CheckBox.UntickedOverSpriteChanged"/> when possible.
        /// </summary>
        protected virtual void OnUntickedOverSpriteChanged()
        {
            HandleAutoResize();
        }

        /// <summary>
        /// Handles when the <see cref="CheckBox.UntickedPressedSprite"/> changes.
        /// This is called immediately before <see cref="CheckBox.UntickedPressedSpriteChanged"/>.
        /// Override this method instead of using an event hook on <see cref="CheckBox.UntickedPressedSpriteChanged"/> when possible.
        /// </summary>
        protected virtual void OnUntickedPressedSpriteChanged()
        {
            HandleAutoResize();
        }

        /// <summary>
        /// Handles when the <see cref="CheckBox.UntickedSprite"/> changes.
        /// This is called immediately before <see cref="CheckBox.UntickedSpriteChanged"/>.
        /// Override this method instead of using an event hook on <see cref="CheckBox.UntickedSpriteChanged"/> when possible.
        /// </summary>
        protected virtual void OnUntickedSpriteChanged()
        {
            HandleAutoResize();
        }

        /// <summary>
        /// Handles when the <see cref="CheckBox.Value"/> changes.
        /// This is called immediately before <see cref="CheckBox.ValueChanged"/>.
        /// Override this method instead of using an event hook on <see cref="CheckBox.ValueChanged"/> when possible.
        /// </summary>
        protected virtual void OnValueChanged()
        {
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