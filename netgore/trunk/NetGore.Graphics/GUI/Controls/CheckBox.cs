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
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        /// <param name="parent">Parent Control of this Control. Cannot be null.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        public CheckBox(Control parent, Vector2 position) : base(parent, position)
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        /// <param name="gui">The <see cref="GUIManagerBase"/> this Control will be part of. Cannot be null.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        public CheckBox(GUIManagerBase gui, Vector2 position) : base(gui, position)
        {
            Initialize();
        }

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
        /// Gets or sets if the checkbox is ticked.
        /// </summary>
        public bool Value
        {
            get { return _value; }
            set
            {
                if (_value == value)
                    return;

                _value = value;

                if (OnChangeValue != null)
                    OnChangeValue(this);
            }
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
        /// The name of this <see cref="Control"/> for when looking up the skin information.
        /// </summary>
        const string _controlSkinName = "CheckBox";

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
        /// Initializes the <see cref="CheckBox"/>.
        /// </summary>
        void Initialize()
        {
            // Set all the event hooks
            // TODO: !! Use the overridden methods instead of these event hooks
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

            // Perform the initial auto-resize
            CheckBox_Resize(this);
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