using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// A single line for a <see cref="TextBox"/>-style <see cref="Control"/>.
    /// </summary>
    class TextBoxLine
    {
        readonly List<StyledText> _texts;
        string _lineText;

        /// <summary>
        /// Gets the last StyledText on this line.
        /// </summary>
        StyledText LastStyledText
        {
            get
            {
                if (_texts.Count == 0)
                    return null;

                return _texts[_texts.Count - 1];
            }
        }

        /// <summary>
        /// Gets the string of text that makes up this line.
        /// </summary>
        public string LineText
        {
            get { return _lineText; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxLine"/> class.
        /// </summary>
        public TextBoxLine()
        {
            _lineText = string.Empty;
            _texts = new List<StyledText>(4);
        }

        /// <summary>
        /// Appends styled text to the line.
        /// </summary>
        /// <param name="text">The text to append.</param>
        public void Append(StyledText text)
        {
            if (text == null)
                return;

            // Since this class is a single line, remove any line breaks
            text = text.ToSingleline();
            if (text.Text.Length == 0)
                return;

            // Check if we can combine with the current last StyledText
            var last = LastStyledText;
            if (last != null && last.HasSameStyle(text))
            {
                // Append to the existing StyledText
                _texts[_texts.Count - 1] = last + text.Text;
                _lineText += text.Text;
            }
            else
            {
                // Add the new StyledText
                _texts.Add(text);
                _lineText += text.Text;
            }
        }

        public void Draw(SpriteBatch sb, SpriteFont font, Vector2 screenPos, Color defaultColor)
        {
            for (int i = 0; i < _texts.Count; i++)
            {
                var curr = _texts[i];
                curr.Draw(sb, font, screenPos, defaultColor);
                if (i < _texts.Count - 1)
                    screenPos += new Vector2(curr.GetWidth(font), 0);
            }
        }

        /// <summary>
        /// Finds the <see cref="StyledText"/> and the index in that <see cref="StyledText"/> that contains
        /// the line's character at the given <paramref name="index"/>. That is, the character in
        /// <paramref name="text"/> at index <paramref name="textIndex"/> will be the line character with the
        /// given <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The index of the character to find in the line.</param>
        /// <param name="text">The <see cref="StyledText"/> containing the character at the given
        /// <paramref name="index"/>.</param>
        /// <param name="textIndex">The index within the <paramref name="text"/> of the character at the given
        /// <paramref name="index"/>.</param>
        /// <param name="listIndex">The index of the <paramref name="text"/> in the <see cref="_texts"/> list.</param>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="index"/> is out of range.</exception>
        void FindLineCharacter(int index, out StyledText text, out int textIndex, out int listIndex)
        {
            if (index < 0 || index >= _lineText.Length)
                throw new ArgumentOutOfRangeException("index");

            int sumLength = 0;
            for (listIndex = 0; listIndex < _texts.Count; listIndex++)
            {
                text = _texts[listIndex];
                int currTextLen = text.Text.Length;
                if (sumLength + currTextLen >= index)
                {
                    // The character we want is somewhere in this text
                    textIndex = index - sumLength;
                    return;
                }
                sumLength += currTextLen;
            }

            // Unlikely this will ever happen, so a shitty exception works
            throw new ArgumentOutOfRangeException("index", "This exception should never appear. Please re-examine the code.");
        }

        /// <summary>
        /// Inserts the <see cref="text"/> into the line at the specified position.
        /// </summary>
        /// <param name="position">The 0-based index to insert the <see cref="text"/>. The text
        /// will be inserted before the character in the line whos index is equal to the position. For example,
        /// inserting at position 3 will insert between the 3rd and 4th character of the string. As such,
        /// position 0 will insert at the start of the string, and the length of the string will insert
        /// at the end. If the position is invalid, this method will always return false.</param>
        /// <param name="text">The character to insert.</param>
        /// <returns>True if the <paramref name="text"/> was inserted successfully; otherwise false.</returns>
        public bool Insert(string text, int position)
        {
            return Insert(new StyledText(text), position);
        }

        /// <summary>
        /// Inserts the <see cref="text"/> into the line at the specified position.
        /// </summary>
        /// <param name="position">The 0-based index to insert the <see cref="text"/>. The text
        /// will be inserted before the character in the line whos index is equal to the position. For example,
        /// inserting at position 3 will insert between the 3rd and 4th character of the string. As such,
        /// position 0 will insert at the start of the string, and the length of the string will insert
        /// at the end. If the position is invalid, this method will always return false.</param>
        /// <param name="text">The character to insert.</param>
        /// <returns>True if the <paramref name="text"/> was inserted successfully; otherwise false.</returns>
        public bool Insert(StyledText text, int position)
        {
            if (position < 0 || position > _lineText.Length)
                return false;

            if (position == 0)
            {
                // Insert at the start
                var first = _texts.Count > 0 ? _texts[0] : null;
                if (first != null)
                {
                    // Try to combine the texts
                    if (text.HasSameStyle(first))
                        _texts[0] = text + first.Text;
                    else
                        _texts.Insert(0, text);
                }
                else
                    _texts.Add(text);

                _lineText = text.Text + _lineText;
            }
            else if (position == _lineText.Length)
            {
                // Insert at the end
                var last = _texts.Count > 0 ? _texts[_texts.Count - 1] : null;
                if (last != null)
                {
                    // Try to combine the texts
                    if (text.HasSameStyle(last))
                        _texts[_texts.Count - 1] = last + text.Text;
                    else
                        _texts.Add(text);
                }
                else
                    _texts.Add(text);

                _lineText += text.Text;
            }
            else
            {
                // Somewhere in the middle

                // Get the StyledText containing the character to insert before
                StyledText subText;
                int subTextIndex;
                int listIndex;
                try
                {
                    FindLineCharacter(position, out subText, out subTextIndex, out listIndex);
                }
                catch (ArgumentOutOfRangeException)
                {
                    return false;
                }

                // Try to combine the texts
                if (text.HasSameStyle(subText))
                {
                    string combined = string.Empty;
                    if (subTextIndex > 0)
                        combined += subText.Text.Substring(0, subTextIndex);
                    combined += text.Text;
                    combined += subText.Text.Substring(subTextIndex);
                    _texts[listIndex] = new StyledText(combined, text);
                }
                else
                {
                    if (subTextIndex == 0)
                    {
                        // Don't need to split up the existing StyledText (inserts whole thing before)
                        _texts.Insert(listIndex, text);
                    }
                    else
                    {
                        // Have to split apart the existing StyledText and insert the new one in between
                        // the split up parts
                        var firstPart = subText.Substring(0, subTextIndex);
                        var secondPart = subText.Substring(subTextIndex);
                        _texts[listIndex] = secondPart;
                        _texts.Insert(listIndex, text);
                        _texts.Insert(listIndex, firstPart);
                    }
                }

                _lineText = _lineText.Substring(0, position) + text.Text + _lineText.Substring(position);
            }

            return true;
        }
    }
}