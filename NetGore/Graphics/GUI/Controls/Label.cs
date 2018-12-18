using System;
using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics.GUI
{
    public class Label : TextControl
    {
        /// <summary>
        /// The name of this <see cref="Control"/> for when looking up the skin information.
        /// </summary>
        const string _controlSkinName = "Label";

        bool _autoresize;

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        public Label(Control parent, Vector2 position) : base(parent, position, Vector2.One)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager this <see cref="Control"/> will be managed by.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <exception cref="ArgumentNullException"><paramref name="guiManager"/> is null.</exception>
        public Label(IGUIManager guiManager, Vector2 position) : base(guiManager, position, Vector2.One)
        {
        }

        /// <summary>
        /// Gets or sets if the <see cref="Label"/> will automatically resize when the text or font changes.
        /// </summary>
        [SyncValue]
        public bool AutoResize
        {
            get { return _autoresize; }
            set
            {
                if (_autoresize == value)
                    return;

                _autoresize = value;

                ResizeToFitText();
            }
        }

        /// <summary>
        /// Draws the <see cref="Control"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        protected override void DrawControl(ISpriteBatch spriteBatch)
        {
            base.DrawControl(spriteBatch);

            DrawText(spriteBatch, new Vector2(Border.LeftWidth, Border.TopHeight));
        }

        /// <summary>
        /// When overridden in the derived class, loads the skinning information for the <see cref="Control"/>
        /// from the given <paramref name="skinManager"/>.
        /// </summary>
        /// <param name="skinManager">The <see cref="ISkinManager"/> to load the skinning information from.</param>
        public override void LoadSkin(ISkinManager skinManager)
        {
            Border = skinManager.GetBorder(_controlSkinName);
        }

        /// <summary>
        /// Handles when the <see cref="Control.Border"/> has changed.
        /// This is called immediately before <see cref="Control.BorderChanged"/>.
        /// Override this method instead of using an event hook on <see cref="Control.BorderChanged"/> when possible.
        /// </summary>
        protected override void OnBorderChanged()
        {
            base.OnBorderChanged();

            ResizeToFitText();
        }

        /// <summary>
        /// Handles when the <see cref="TextControl.Font"/> has changed.
        /// This is called immediately before <see cref="TextControl.FontChanged"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.FontChanged"/> when possible.
        /// </summary>
        protected override void OnFontChanged()
        {
            base.OnFontChanged();

            ResizeToFitText();
        }

        /// <summary>
        /// Handles when the <see cref="TextControl.Text"/> has changed.
        /// This is called immediately before <see cref="TextControl.TextChanged"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.TextChanged"/> when possible.
        /// </summary>
        protected override void OnTextChanged()
        {
            base.OnTextChanged();

            ResizeToFitText();
        }

        /// <summary>
        /// Resizes the <see cref="Label"/> so it is large enough to fit the <see cref="TextControl.Text"/>.
        /// </summary>
        void ResizeToFitText()
        {
            if (!AutoResize)
                return;

            if (Font != null && !Font.IsDisposed)
            {
                // Valid font
                var textToMeasure = string.IsNullOrEmpty(Text) ? "W" : Text;
                ClientSize = Font.MeasureString(textToMeasure);
            }
            else
            {
                // Invalid font
                ClientSize = Vector2.One;
            }
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
            AutoResize = true;
        }
    }
}