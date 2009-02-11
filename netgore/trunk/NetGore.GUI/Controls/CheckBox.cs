using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// Amount of space added between the check box and text
        /// </summary>
        const float _textXAdjust = 2f;

        CheckBoxState _state = CheckBoxState.None;

        ISprite _ticked;
        ISprite _tickedOver;
        ISprite _tickedPressed;
        ISprite _unticked;
        ISprite _untickedOver;
        ISprite _untickedPressed;
        bool _value = false;

        /// <summary>
        /// Notifies when the TickedOverSprite changes
        /// </summary>
        public event ControlEventHandler OnChangeTickedOverSprite;

        /// <summary>
        /// Notifies when the TickedPressedSprite changes
        /// </summary>
        public event ControlEventHandler OnChangeTickedPressedSprite;

        /// <summary>
        /// Notifies when the TickedSprite changes
        /// </summary>
        public event ControlEventHandler OnChangeTickedSprite;

        /// <summary>
        /// Notifies when the UntickedOverSprite changes
        /// </summary>
        public event ControlEventHandler OnChangeUntickedOverSprite;

        /// <summary>
        /// Notifies when the UntickedPressedSprite changes
        /// </summary>
        public event ControlEventHandler OnChangeUntickedPressedSprite;

        /// <summary>
        /// Notifies when the UntickedSprite changes
        /// </summary>
        public event ControlEventHandler OnChangeUntickedSprite;

        /// <summary>
        /// Notifies when the checkbox value changes
        /// </summary>
        public event ControlEventHandler OnChangeValue;

        /// <summary>
        /// Gets or sets the Sprite used for a ticked checkbox with the mouse over it
        /// </summary>
        public ISprite TickedOverSprite
        {
            get { return _ticked; }
            set
            {
                if (_tickedOver != value)
                {
                    _tickedOver = value;
                    if (OnChangeTickedOverSprite != null)
                        OnChangeTickedOverSprite(this);
                }
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
                if (_ticked != value)
                {
                    _ticked = value;
                    if (OnChangeTickedPressedSprite != null)
                        OnChangeTickedPressedSprite(this);
                }
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
                if (_ticked != value)
                {
                    _ticked = value;
                    if (OnChangeTickedSprite != null)
                        OnChangeTickedSprite(this);
                }
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
                if (_untickedOver != value)
                {
                    _untickedOver = value;
                    if (OnChangeUntickedOverSprite != null)
                        OnChangeUntickedOverSprite(this);
                }
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
                if (_untickedPressed != value)
                {
                    _untickedPressed = value;
                    if (OnChangeUntickedPressedSprite != null)
                        OnChangeUntickedPressedSprite(this);
                }
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
                if (_unticked != value)
                {
                    _unticked = value;
                    if (OnChangeUntickedSprite != null)
                        OnChangeUntickedSprite(this);
                }
            }
        }

        /// <summary>
        /// Gets or sets if the checkbox is ticked (true for ticked, false for unticked)
        /// </summary>
        public bool Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    if (OnChangeValue != null)
                        OnChangeValue(this);
                }
            }
        }

        /// <summary>
        /// CheckBox constructor
        /// </summary>
        /// <param name="text">Text to display</param>
        /// <param name="position">Position of the CheckBox relative to its parent</param>
        /// <param name="parent">Parent Control for this CheckBox</param>
        public CheckBox(string text, Vector2 position, Control parent) : base(text, position, parent)
        {
            Initialize(GUIManager.CheckBoxSettings);
        }

        /// <summary>
        /// CheckBox constructor
        /// </summary>
        /// <param name="settings">CheckBox settings</param>
        /// <param name="text">Text to display</param>
        /// <param name="position">Position of the CheckBox relative to its parent</param>
        /// <param name="parent">Parent Control for this CheckBox</param>
        public CheckBox(CheckBoxSettings settings, string text, Vector2 position, Control parent)
            : base(settings, text, position, parent)
        {
            Initialize(settings);
        }

        /// <summary>
        /// CheckBox constructor
        /// </summary>
        /// <param name="text">Text to display</param>
        /// <param name="font">Font used to write the text</param>
        /// <param name="position">Position of the CheckBox relative to its parent</param>
        /// <param name="parent">Parent Control for this CheckBox</param>
        public CheckBox(string text, SpriteFont font, Vector2 position, Control parent) : base(text, font, position, parent)
        {
            Initialize(GUIManager.CheckBoxSettings);
        }

        /// <summary>
        /// CheckBox constructor
        /// </summary>
        /// <param name="settings">CheckBox settings</param>
        /// <param name="text">Text to display</param>
        /// <param name="font">Font used to write the text</param>
        /// <param name="position">Position of the CheckBox relative to its parent</param>
        /// <param name="parent">Parent Control for this CheckBox</param>
        public CheckBox(CheckBoxSettings settings, string text, SpriteFont font, Vector2 position, Control parent)
            : base(settings, text, font, position, parent)
        {
            Initialize(settings);
        }

        /// <summary>
        /// CheckBox constructor
        /// </summary>
        /// <param name="gui">GUIManager this CheckBox is handled by</param>
        /// <param name="text">Text to display</param>
        /// <param name="font">Font used to write the text</param>
        /// <param name="position">Position of the CheckBox relative to its parent</param>
        /// <param name="parent">Parent Control for this CheckBox</param>
        public CheckBox(GUIManagerBase gui, string text, SpriteFont font, Vector2 position, Control parent)
            : base(gui, text, font, position, parent)
        {
            Initialize(GUIManager.CheckBoxSettings);
        }

        /// <summary>
        /// CheckBox constructor
        /// </summary>
        /// <param name="gui">GUIManager this CheckBox is handled by</param>
        /// <param name="settings">CheckBox settings</param>
        /// <param name="text">Text to display</param>
        /// <param name="font">Font used to write the text</param>
        /// <param name="position">Position of the CheckBox relative to its parent</param>
        /// <param name="parent">Parent Control for this CheckBox</param>
        public CheckBox(GUIManagerBase gui, CheckBoxSettings settings, string text, SpriteFont font, Vector2 position,
                        Control parent) : base(gui, settings, text, font, position, parent)
        {
            Initialize(settings);
        }

        void CheckBox_OnLostFocus(Control sender)
        {
            _state = CheckBoxState.None;
        }

        /// <summary>
        /// Handles mouse clicks
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event args</param>
        void CheckBox_OnMouseDown(object sender, MouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Value = !_value;
                _state = CheckBoxState.Pressed;
            }
        }

        void CheckBox_OnMouseEnter(object sender, MouseEventArgs e)
        {
            _state = CheckBoxState.Over;
        }

        void CheckBox_OnMouseLeave(object sender, MouseEventArgs e)
        {
            _state = CheckBoxState.None;
        }

        void CheckBox_OnMouseMove(object sender, MouseEventArgs e)
        {
            _state = CheckBoxState.Over;
        }

        void CheckBox_OnMouseUp(object sender, MouseClickEventArgs e)
        {
            _state = CheckBoxState.Over;
        }

        /// <summary>
        /// Handles control resizing.
        /// </summary>
        /// <param name="sender">Sender Control.</param>
        void CheckBox_Resize(Control sender)
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
        /// Draws the control
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw to</param>
        protected override void DrawControl(SpriteBatch spriteBatch)
        {
            // Draw the border
            if (Border != null)
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
        /// Initializes the CheckBox
        /// </summary>
        /// <param name="settings">CheckBox settings</param>
        void Initialize(CheckBoxSettings settings)
        {
            // Set the default Sprites
            _ticked = settings.Ticked;
            _tickedOver = settings.TickedMouseOver;
            _tickedPressed = settings.TickedPressed;
            _unticked = settings.Unticked;
            _untickedOver = settings.UntickedMouseOver;
            _untickedPressed = settings.UntickedPressed;

            // Set all the event hooks
            OnChangeText += CheckBox_Resize;
            OnChangeFont += CheckBox_Resize;
            OnChangeTickedSprite += CheckBox_Resize;
            OnChangeTickedOverSprite += CheckBox_Resize;
            OnChangeTickedPressedSprite += CheckBox_Resize;
            OnChangeUntickedSprite += CheckBox_Resize;
            OnChangeUntickedOverSprite += CheckBox_Resize;
            OnChangeUntickedPressedSprite += CheckBox_Resize;

            OnMouseDown += CheckBox_OnMouseDown;
            OnMouseEnter += CheckBox_OnMouseEnter;
            OnMouseLeave += CheckBox_OnMouseLeave;
            OnMouseUp += CheckBox_OnMouseUp;
            OnMouseMove += CheckBox_OnMouseMove;
            OnLostFocus += CheckBox_OnLostFocus;

            // Allow the CheckBox to get focus
            CanFocus = true;

            // Perform the initial auto-resize
            CheckBox_Resize(this);
        }

        enum CheckBoxState
        {
            None,
            Over,
            Pressed
        }
    }
}