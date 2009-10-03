using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NetGore;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// A box of a single line of text that supports on-screen editing and interaction
    /// </summary>
    public class TextBoxSingleLine : TextControl
    {
        /// <summary>
        /// How fast the cursor flashes in ticks.
        /// </summary>
        const int _cursorFlashRate = 65;

        /// <summary>
        /// The number of ticks that the cursor remains solid after moving
        /// </summary>
        const int _cursorMoveSolidTime = _repeatRate * 4;

        /// <summary>
        /// How many ticks needed after a key is pressed down for it to start repeating.
        /// </summary>
        const int _holdDelay = 20;

        /// <summary>
        /// How many ticks it takes for a held key to be repeated (higher = slower repeating).
        /// </summary>
        const int _repeatRate = 6;

        /// <summary>
        /// Contains the length of the character and all characters before it, assuming the first character's left
        /// side starts at 0.
        /// </summary>
        float[] _charRights = new float[8];

        int _cursorFlashTimer = 0;

        int _cursorPos = -1;

        /// <summary>
        /// Counter that keeps track of how many more ticks the cursor needs to remain solid. If this value is
        /// greater than 0, the the cursor should be solid.
        /// </summary>
        int _cursorSolidCount = 0;

        Keys _lastKey;

        int _lastKeyTicks;

        /// <summary>
        /// Gets or sets the character position of the cursor. This value indicates which character
        /// the cursor is in front of, with -1 being behind the first character, so the valid range
        /// of values is -1 to Text.Length - 1.
        /// </summary>
        int CursorPos
        {
            get { return _cursorPos; }
            set
            {
                if (_cursorPos == value)
                    return;

                // Move the cursor
                _cursorPos = value;

                // Set the cursor to be solid for a bit
                _cursorSolidCount = _cursorMoveSolidTime;
            }
        }

        /// <summary>
        /// Gets or sets the last key that was pressed down.
        /// </summary>
        Keys LastKey
        {
            get { return _lastKey; }
            set
            {
                if (_lastKey == value)
                    return;

                _lastKey = value;
                _lastKeyTicks = 0;
            }
        }

        Vector2 TextAreaOffset
        {
            get
            {
                ControlBorder border = Border;
                if (border == null)
                    return Vector2.Zero;
                return new Vector2(border.LeftWidth, border.TopHeight);
            }
        }

        float TextAreaWidth
        {
            get { return Size.X - (Border != null ? Border.Width : 0); }
        }

        public TextBoxSingleLine(GUIManagerBase gui, TextControlSettings settings, string text, SpriteFont font, Vector2 position,
                                 Vector2 size, Control parent) : base(gui, settings, text, font, position, size, parent)
        {
            // Set the properties to match that of a TextBox
            CanDrag = false;
            CanFocus = true;

            OnChangeText += TextBox_OnChangeText;

            // Set the initial text
            TextBox_OnChangeText(this);
        }

        public TextBoxSingleLine(string text, Vector2 position, Vector2 size, Control parent)
            : this(parent.GUIManager, parent.GUIManager.TextBoxSettings, text, parent.GUIManager.Font, position, size, parent)
        {
        }

        protected override void DrawControl(SpriteBatch spriteBatch)
        {
            base.DrawControl(spriteBatch);

            int charOffset = GetCharOffset();

            // Draw the text
            if (!string.IsNullOrEmpty(Text))
            {
                int first = charOffset;
                int last = FindLastVisibleChar(charOffset);
                string str = Text.Substring(first, last - first + 1);
                spriteBatch.DrawString(Font, str, ScreenPosition + TextAreaOffset, ForeColor);
            }

            // Draw the cursor
            if (HasFocus && IsEnabled)
            {
                _cursorFlashTimer = (_cursorFlashTimer + 1) % _cursorFlashRate;
                if (_cursorSolidCount > 0 || _cursorFlashTimer < _cursorFlashRate / 2)
                {
                    float off = CursorPos >= 0 ? _charRights[CursorPos] - 1 : 0;
                    if (charOffset - 1 >= 0)
                        off -= _charRights[charOffset - 1];

                    Vector2 pos = ScreenPosition + TextAreaOffset + new Vector2(off, 0);
                    spriteBatch.DrawString(Font, "|", pos, ForeColor);
                }
            }
        }

        /// <summary>
        /// Finds the index of the first visible char from the given max.
        /// </summary>
        /// <param name="max">Index of the right-most visible character.</param>
        /// <returns>The index of the first visible char from the given max.</returns>
        int FindFirstVisibleChar(int max)
        {
            if (max <= 0)
                return 0;

            float target = _charRights[max] - TextAreaWidth;
            if (target <= 0)
                return 0;

            int i = 0;
            while (i < max)
            {
                if (_charRights[i] > target)
                    return i + 1;
                i++;
            }
            return max - 1;
        }

        /// <summary>
        /// Finds the index of the last visible char from the given offset.
        /// </summary>
        /// <param name="offset">Offset to the first visible character.</param>
        /// <returns>The index of the last visible char from the given offset.</returns>
        int FindLastVisibleChar(int offset)
        {
            float minX = offset >= 0 ? _charRights[offset] : 0;
            float maxX = minX + TextAreaWidth;

            for (int i = offset + 1; i < Text.Length; i++)
            {
                if (_charRights[i] > maxX)
                    return i - 1;
            }

            return Text.Length - 1;
        }

        int GetCharOffset()
        {
            if (CursorPos <= 0)
                return 0;

            return FindFirstVisibleChar(CursorPos);
        }

        void TextBox_OnChangeText(Control sender)
        {
            // Ensure the cursor position is valid
            if (CursorPos > Text.Length - 1)
                CursorPos = Text.Length - 1;

            // Resize the CharRights array if needed
            if (Text.Length > _charRights.Length)
            {
                int newSize = Text.Length * 2;
                while (newSize < Text.Length)
                {
                    newSize *= 2;
                }
                Array.Resize(ref _charRights, newSize);
            }

            // Update the character right sides
            for (int i = 0; i < Text.Length; i++)
            {
                _charRights[i] = Font.MeasureString(Text.Substring(0, i + 1)).X;
            }
        }

        protected override void UpdateControl(int currentTime)
        {
            // Decrease the solid cursor time
            if (_cursorSolidCount > 0)
                _cursorSolidCount--;

            base.UpdateControl(currentTime);
        }

        protected override void UpdateKeyboard()
        {
            base.UpdateKeyboard();

            // Ensure we have focus
            if (!HasFocus)
            {
                LastKey = Keys.None;
                return;
            }

            var keysDown = GUIManager.NewKeysDown;
            if (keysDown == null || keysDown.Count() <= 0)
            {
                // If no keys were recently pressed, check if the LastKey was held
                var keysHeld = GUIManager.KeysPressed;
                if (LastKey == Keys.None || keysHeld == null || !keysHeld.Contains(LastKey))
                {
                    LastKey = Keys.None;
                    return;
                }

                // Check if held down long enough
                if (++_lastKeyTicks < _holdDelay)
                    return;

                // Check the repeat delay
                if (_lastKeyTicks % _repeatRate != 0)
                    return;

                // Last pressed key was held down
                keysDown = new Keys[] { LastKey };
            }

            // Update the text with newly pressed keys
            string newText = Text;
            foreach (Keys key in keysDown)
            {
                // Do nothing for these keys
                if (key == Keys.Enter)
                    continue;

                // Handle valid keys
                switch (key)
                {
                    case Keys.Delete:
                    case Keys.Back:
                        // Delete one character
                        if (CursorPos >= 0 && CursorPos < Text.Length)
                        {
                            newText = newText.Remove(CursorPos, 1);
                            CursorPos--;
                        }
                        break;

                    case Keys.Left:
                        // Move the cursor left
                        if (CursorPos > -1)
                            CursorPos--;
                        break;

                    case Keys.Right:
                        // Move the cursor right
                        if (CursorPos < Text.Length - 1)
                            CursorPos++;
                        break;

                    default:
                        // Append key's character
                        string addChar = GetKeyString(key, GUIManager.KeyboardState);
                        if (!string.IsNullOrEmpty(addChar))
                            newText = newText.Insert(++CursorPos, addChar);
                        break;
                }

                // Set the LastKey
                LastKey = key;
            }

            // Set the new text
            Text = newText;
        }
    }
}