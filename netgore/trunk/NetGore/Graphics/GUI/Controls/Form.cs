using System;
using System.ComponentModel;
using System.Linq;
using NetGore.IO;
using SFML.Graphics;
using SFML.Window;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// A standard sub-window that often draggable and is used to contain child controls.
    /// </summary>
    public class Form : TextControl
    {
        /// <summary>
        /// The name of this <see cref="Control"/> for when looking up the skin information.
        /// </summary>
        const string _controlSkinName = "Form";

        Control _closeButton;
        bool _isCloseButtonVisible = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        public Form(Control parent, Vector2 position, Vector2 clientSize) : base(parent, position, clientSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Form"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        public Form(IGUIManager guiManager, Vector2 position, Vector2 clientSize) : base(guiManager, position, clientSize)
        {
        }

        /// <summary>
        /// Gets the <see cref="Control"/> for showing the close button on this <see cref="Form"/>.
        /// </summary>
        public Control CloseButton
        {
            get { return _closeButton; }
        }

        /// <summary>
        /// Gets or sets if the Close button on the form is visible. Default is true.
        /// </summary>
        [DefaultValue(true)]
        public bool IsCloseButtonVisible
        {
            get { return _isCloseButtonVisible; }
            set
            {
                if (_isCloseButtonVisible == value)
                    return;

                _isCloseButtonVisible = value;

                if (_closeButton != null)
                    _closeButton.IsVisible = _isCloseButtonVisible;
            }
        }

        /// <summary>
        /// Handles when the Close button on the form is clicked.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SFML.Window.MouseButtonEventArgs"/> instance containing the event data.</param>
        protected virtual void CloseButtonClicked(object sender, MouseButtonEventArgs e)
        {
            IsVisible = false;
        }

        /// <summary>
        /// Creates the <see cref="Control"/> for showing the close button for this <see cref="Form"/>.
        /// </summary>
        /// <param name="spriteName">The default name of the sprite to display.</param>
        /// <returns>The <see cref="Control"/> for the close button.</returns>
        protected virtual Control CreateCloseButton(string spriteName)
        {
            return new FormButton(this, spriteName);         
        }

        /// <summary>
        /// Draws the Control.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        protected override void DrawControl(ISpriteBatch spriteBatch)
        {
            base.DrawControl(spriteBatch);

            DrawText(spriteBatch, new Vector2(5, 3));
        }

        /// <summary>
        /// Gets the position to use for a toolbar button.
        /// </summary>
        /// <param name="button">The button.</param>
        /// <param name="hOffset">The amount to offset the position on the horizontal axis.</param>
        /// <returns>The position to give the <paramref name="button"/>.</returns>
        Vector2 GetToolbarButtonPosition(Control button, float hOffset)
        {
            return new Vector2(ClientSize.X - button.Size.X - hOffset, -(Border.TopHeight / 2f) - (button.Size.Y / 2f));
        }

        /// <summary>
        /// When overridden in the derived class, loads the skinning information for the <see cref="Control"/>
        /// from the given <paramref name="skinManager"/>.
        /// </summary>
        /// <param name="skinManager">The <see cref="ISkinManager"/> to load the skinning information from.</param>
        public override void LoadSkin(ISkinManager skinManager)
        {
            // Create the toolbar buttons for the form, if needed
            if (_closeButton == null)
            {
                _closeButton = CreateCloseButton("Close");
                _closeButton.IsVisible = IsCloseButtonVisible;
                _closeButton.Clicked -= CloseButtonClicked;
                _closeButton.Clicked += CloseButtonClicked;
            }

            // Load the border
            Border = skinManager.GetBorder(_controlSkinName);

            // Update the toolbar button positions
            UpdateToolbarButtonPositions();
        }

        /// <summary>
        /// Handles when the <see cref="Control.Size"/> of this <see cref="Control"/> has changed.
        /// This is called immediately before <see cref="Control.Resized"/>.
        /// Override this method instead of using an event hook on <see cref="Control.Resized"/> when possible.
        /// </summary>
        protected override void OnResized()
        {
            base.OnResized();

            UpdateToolbarButtonPositions();
        }

        /// <summary>
        /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
        /// base class's method to ensure that changes to settings are hierchical.
        /// </summary>
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            CanDrag = true;
            CanFocus = true;
            ForeColor = Color.Yellow;
            IsCloseButtonVisible = true;
        }

        /// <summary>
        /// Updates the positions of the buttons in the toolbar.
        /// </summary>
        void UpdateToolbarButtonPositions()
        {
            // If the _closeButton is null, the buttons have not been created yet
            if (_closeButton == null)
                return;

            _closeButton.Position = GetToolbarButtonPosition(_closeButton, 0f);
        }

        /// <summary>
        /// A button that is part of a <see cref="Form"/>'s toolbar.
        /// </summary>
        sealed class FormButton : PictureBox
        {
            readonly string _spriteName;

            ISprite _sprite;
            ISprite _spriteMouseOver;
            ISprite _spritePressed;

            /// <summary>
            /// Initializes a new instance of the <see cref="FormButton"/> class.
            /// </summary>
            /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
            /// <param name="spriteName">The name of the button sprite to load.</param>
            /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
            /// <exception cref="ArgumentNullException"><paramref name="spriteName"/> is null or empty.</exception>
            public FormButton(Control parent, string spriteName) : base(parent, Vector2.Zero, Vector2.One)
            {
                if (string.IsNullOrEmpty(spriteName))
                    throw new ArgumentNullException("spriteName");

                _spriteName = spriteName;

                LoadSkin(GUIManager.SkinManager);
            }

            /// <summary>
            /// When overridden in the derived class, loads the skinning information for the <see cref="Control"/>
            /// from the given <paramref name="skinManager"/>.
            /// </summary>
            /// <param name="skinManager">The <see cref="ISkinManager"/> to load the skinning information from.</param>
            public override void LoadSkin(ISkinManager skinManager)
            {
                if (_spriteName == null)
                    return;

                base.LoadSkin(skinManager);

                const string buttonsCategory = _controlSkinName + SpriteCategorization.Delimiter + "Buttons";

                // Get the sprites
                _sprite = skinManager.GetControlSprite(buttonsCategory, _spriteName);
                _spriteMouseOver = skinManager.GetControlSprite(buttonsCategory, _spriteName + "MouseOver");
                _spritePressed = skinManager.GetControlSprite(buttonsCategory, _spriteName + "Pressed");

                // Set the control's size to the largest of the sprites
                var newSize = Vector2.One;

                if (_sprite != null)
                    newSize = newSize.Max(_sprite.Size);

                if (_spriteMouseOver != null)
                    newSize = newSize.Max(_spriteMouseOver.Size);

                if (_spritePressed != null)
                    newSize = newSize.Max(_spritePressed.Size);

                ClientSize = newSize;
            }

            /// <summary>
            /// Sets the default values for the <see cref="Control"/>. This should always begin with a call to the
            /// base class's method to ensure that changes to settings are hierchical.
            /// </summary>
            protected override void SetDefaultValues()
            {
                base.SetDefaultValues();

                StretchSprite = false;
                IsVisible = true;
                IsBoundToParentArea = false;
                IncludeInResizeToChildren = false;
            }

            /// <summary>
            /// Updates the <see cref="Control"/>. This is called for every <see cref="Control"/>, even if it is disabled or
            /// not visible.
            /// </summary>
            /// <param name="currentTime">The current time in milliseconds.</param>
            protected override void UpdateControl(TickCount currentTime)
            {
                base.UpdateControl(currentTime);

                if (GUIManager.UnderCursor == this)
                {
                    // Cursor is over
                    if (GUIManager.IsMouseButtonDown(Mouse.Button.Left))
                    {
                        // Left button is down
                        Sprite = _spritePressed;
                    }
                    else
                    {
                        // Left button is not down
                        Sprite = _spriteMouseOver;
                    }
                }
                else
                {
                    // Cursor is not over
                    Sprite = _sprite;
                }
            }
        }
    }
}