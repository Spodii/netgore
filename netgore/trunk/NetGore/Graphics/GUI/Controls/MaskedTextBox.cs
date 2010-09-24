using System;
using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// A <see cref="TextBox"/> that hides the entered text, replacing each character with a single constant character instead.
    /// Often seen for input fields for passwords.
    /// </summary>
    public class MaskedTextBox : TextBox
    {
        /// <summary>
        /// The length of the mask char. When 0, it needs to be recalculated.
        /// </summary>
        byte _charLen = 0;

        char _maskChar = '*';

        /// <summary>
        /// Initializes a new instance of the <see cref="MaskedTextBox"/> class.
        /// </summary>
        /// <param name="parent">Parent <see cref="Control"/> of this <see cref="Control"/>.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        /// <exception cref="NullReferenceException"><paramref name="parent"/> is null.</exception>
        public MaskedTextBox(Control parent, Vector2 position, Vector2 clientSize) : base(parent, position, clientSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaskedTextBox"/> class.
        /// </summary>
        /// <param name="guiManager">The GUI manager.</param>
        /// <param name="position">Position of the Control reletive to its parent.</param>
        /// <param name="clientSize">The size of the <see cref="Control"/>'s client area.</param>
        public MaskedTextBox(IGUIManager guiManager, Vector2 position, Vector2 clientSize)
            : base(guiManager, position, clientSize)
        {
        }

        /// <summary>
        /// Is always false for a <see cref="MaskedTextBox"/>.
        /// </summary>
        public override bool IsMultiLine
        {
            get { return base.IsMultiLine; }
            set { base.IsMultiLine = false; }
        }

        /// <summary>
        /// Gets or sets the <see cref="char"/> to use for the mask.
        /// </summary>
        public char MaskChar
        {
            get { return _maskChar; }
            set
            {
                if (_maskChar == value)
                    return;

                _maskChar = value;
                _charLen = 0;
            }
        }

        /// <summary>
        /// Draws the text to display for the <see cref="TextBox"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="offset">The offset to draw the text at.</param>
        protected override void DrawControlText(ISpriteBatch spriteBatch, Vector2 offset)
        {
            var numChars = NumCharsToDraw - LineCharBufferOffset;
            if (numChars > 0)
                spriteBatch.DrawString(Font, new string(MaskChar, numChars), offset, ForeColor);
        }

        /// <summary>
        /// Gets the offset for the text cursor for the given text. This is usually just the length of the text.
        /// </summary>
        /// <param name="text">The text to get the offset for.</param>
        /// <returns>The X offset for the cursor.</returns>
        protected override int GetTextCursorOffset(string text)
        {
            if (string.IsNullOrEmpty(text))
                return 0;

            if (_charLen == 0)
                _charLen = (byte)Font.MeasureString(MaskChar.ToString()).X;

            return (int)((float)_charLen * text.Length);
        }

        /// <summary>
        /// Handles when the <see cref="TextControl.Font"/> has changed.
        /// This is called immediately before <see cref="TextControl.FontChanged"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.FontChanged"/> when possible.
        /// </summary>
        protected override void OnFontChanged()
        {
            base.OnFontChanged();

            _charLen = 0;
        }

        /// <summary>
        /// Handles when the <see cref="TextControl.Text"/> has changed.
        /// This is called immediately before <see cref="TextControl.TextChanged"/>.
        /// Override this method instead of using an event hook on <see cref="TextControl.TextChanged"/> when possible.
        /// </summary>
        protected override void OnTextChanged()
        {
            base.OnTextChanged();

            if (string.IsNullOrEmpty(Text))
                CursorLinePosition = 0;
            else
                CursorLinePosition = Text.Length;
        }
    }
}