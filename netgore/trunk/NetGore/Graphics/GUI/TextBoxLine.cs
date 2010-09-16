using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// A single line for a <see cref="TextBox"/>-style <see cref="Control"/>.
    /// </summary>
    public class TextBoxLine
    {
        readonly List<StyledText> _texts;
        readonly bool _wasAutoBroken;

        string _lineText;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxLine"/> class.
        /// </summary>
        /// <param name="owner">The <see cref="TextBoxLines"/> this line is part of.</param>
        /// <param name="wasAutoBroken">if this <see cref="TextBoxLine"/> was created as the result of automatic line
        /// breaking due to word wrapping.</param>
        public TextBoxLine(TextBoxLines owner, bool wasAutoBroken = false)
        {
            TextBoxLines = owner;
            _wasAutoBroken = wasAutoBroken;
            _lineText = string.Empty;
            _texts = new List<StyledText>(4);
        }

        /// <summary>
        /// Gets an IEnumerable of the <see cref="StyledText"/>s contained in this line.
        /// </summary>
        public IEnumerable<StyledText> GetLineTexts
        {
            get { return _texts; }
        }

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
        /// Gets or sets the <see cref="TextBoxLines"/> this line is part of.
        /// </summary>
        internal TextBoxLines TextBoxLines { get; set; }

        /// <summary>
        /// Gets if this <see cref="TextBoxLine"/> was created as the result of automatic line breaking due
        /// to word wrapping.
        /// </summary>
        public bool WasAutoBroken
        {
            get { return _wasAutoBroken; }
        }

        /// <summary>
        /// Appends styled text to the line.
        /// </summary>
        /// <param name="texts">The texts to append.</param>
        public void Append(IEnumerable<StyledText> texts)
        {
            foreach (var text in texts)
            {
                InternalAppend(text);
            }

            if (TextBoxLines != null)
                TextBoxLines.NotifyTextAdded(this);
        }

        /// <summary>
        /// Appends another <see cref="TextBoxLine"/> to this <see cref="TextBoxLine"/>.
        /// </summary>
        /// <param name="textBoxLine">The other <see cref="TextBoxLine"/> to append to this.</param>
        public void Append(TextBoxLine textBoxLine)
        {
            for (var i = 0; i < textBoxLine._texts.Count; i++)
            {
                InternalAppend(textBoxLine._texts[i]);
            }

            if (TextBoxLines != null)
                TextBoxLines.NotifyTextAdded(this);
        }

        /// <summary>
        /// Appends styled text to the line.
        /// </summary>
        /// <param name="text">The text to append.</param>
        public void Append(StyledText text)
        {
            InternalAppend(text);

            if (TextBoxLines != null)
                TextBoxLines.NotifyTextAdded(this);
        }

        /// <summary>
        /// Completely clears the line of all text.
        /// </summary>
        public void Clear()
        {
            _texts.Clear();
            _lineText = string.Empty;
            EnsureCacheMatchesActualText();
        }

        public int CountFittingCharactersLeft(Font font, int startChar, int maxLineLength)
        {
            string subStr;
            if (startChar == 0)
                subStr = _lineText;
            else if (startChar >= _lineText.Length)
                return 1;
            else
                subStr = _lineText.Substring(startChar);

            return StyledText.FindLastFittingChar(subStr, font, maxLineLength);
        }

        public int CountFittingCharactersRight(Font font, int maxLineLength)
        {
            return StyledText.FindFirstFittingChar(_lineText, font, maxLineLength);
        }

        /// <summary>
        /// Draws the line of text.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="font">The <see cref="Font"/> to draw the text with.</param>
        /// <param name="screenPos">The screen position to start drawing the text at.</param>
        /// <param name="defaultColor">The default color of the text.</param>
        public void Draw(ISpriteBatch sb, Font font, Vector2 screenPos, Color defaultColor)
        {
            for (var i = 0; i < _texts.Count; i++)
            {
                var curr = _texts[i];
                curr.Draw(sb, font, screenPos, defaultColor);
                if (i < _texts.Count - 1)
                    screenPos += new Vector2(curr.GetWidth(font), 0);
            }
        }

        /// <summary>
        /// Draws the line of text.
        /// </summary>
        /// <param name="sb">The <see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="font">The <see cref="Font"/> to draw the text with.</param>
        /// <param name="screenPos">The screen position to start drawing the text at.</param>
        /// <param name="defaultColor">The default color of the text.</param>
        /// <param name="firstChar">The index of the first character to draw.</param>
        /// <param name="numChars">The maximum number of characters to draw.</param>
        public void Draw(ISpriteBatch sb, Font font, Vector2 screenPos, Color defaultColor, int firstChar, int numChars)
        {
            if (firstChar + numChars > _lineText.Length)
                numChars = _lineText.Length - firstChar;

            var currCharCount = 0;
            var lastChar = firstChar + numChars - 1;

            for (var i = 0; i < _texts.Count; i++)
            {
                var curr = _texts[i];
                var currLen = curr.Text.Length;

                var start = firstChar - currCharCount;
                var end = lastChar - currCharCount;

                currCharCount += currLen;

                if (start > end || start > currLen)
                    continue;

                // Check if we have passed the substring we wanted to draw
                if (end < 0)
                    return;

                start = Math.Max(start, 0);
                end = Math.Min(end, currLen - 1);

                var substr = curr.Substring(start, end - start + 1);
                substr.Draw(sb, font, screenPos, defaultColor);

                if (i < _texts.Count - 1)
                    screenPos += new Vector2(substr.GetWidth(font), 0);
            }
        }

        /// <summary>
        /// Ensures that the raw cached line text (<see cref="_lineText"/>) matches the list of <see cref="StyledText"/>s
        /// used to display the text (<see cref="_texts"/>);
        /// </summary>
        [Conditional("DEBUG")]
        void EnsureCacheMatchesActualText()
        {
            var combinedTexts = StyledText.ToString(_texts);
            if (_lineText != combinedTexts)
            {
                const string errmsg = "Line cache does not match the styled text list. Cache: `{0}` Text: `{1}`";
                Debug.Fail(string.Format(errmsg, _lineText, combinedTexts));
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

            var sumLength = 0;
            for (listIndex = 0; listIndex < _texts.Count; listIndex++)
            {
                text = _texts[listIndex];
                var currTextLen = text.Text.Length;
                if (sumLength + currTextLen > index)
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
        /// Gets the width of the line in pixels.
        /// </summary>
        /// <param name="font">The font being measured.</param>
        /// <returns>The width of the line in pixels.</returns>
        public int GetWidth(Font font)
        {
            return (int)_texts.Sum(x => x.GetWidth(font));
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
                    var combined = string.Empty;
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

            EnsureCacheMatchesActualText();

            if (TextBoxLines != null)
                TextBoxLines.NotifyTextAdded(this);

            return true;
        }

        /// <summary>
        /// Appends styled text to the line but does NOT invoke the NotifyTextAdded method. This way, multiple
        /// appends can be made, and NotifyTextAdded is only called once.
        /// </summary>
        /// <param name="text">The text to append.</param>
        void InternalAppend(StyledText text)
        {
            if (text == null || text.Text.Length == 0)
                return;

            // Since this class is a single line, remove any line breaks
            text = text.ToSingleline(" ");
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

            EnsureCacheMatchesActualText();
        }

        /// <summary>
        /// Deletes the character at the given index.
        /// </summary>
        /// <param name="charIndex">The 0-based index of the character in the line to remove.</param>
        public void Remove(int charIndex)
        {
            if (charIndex < 0 || charIndex >= LineText.Length)
            {
                const string errmsg = "Invalid character index `{0}`. Line is `{1}` character(s) long [{2}].";
                Debug.Fail(string.Format(errmsg, charIndex, LineText.Length, LineText));
                return;
            }

            StyledText text;
            int textIndex;
            int listIndex;
            FindLineCharacter(charIndex, out text, out textIndex, out listIndex);

            if (text.Text.Length == 1)
            {
                // Remove the text completely since there is only this one character in it
                _texts.RemoveAt(listIndex);
            }
            else
            {
                // Remove the one character
                var newText = new StyledText(text.Text.Remove(textIndex, 1), text);
                _texts[listIndex] = newText;
            }

            // Update the line cache
            _lineText = _lineText.Remove(charIndex, 1);

            EnsureCacheMatchesActualText();
        }

        /// <summary>
        /// Splits the text in this <see cref="TextBoxLine"/> at the given character index in the line.
        /// </summary>
        /// <param name="lineCharacterIndex">The index of the character to split the line at. This character
        /// at this index will be the first character on the new line.</param>
        /// <param name="currentLine">A <see cref="TextBoxLine"/> representing the remainder of the current
        /// line (the one the split initially was made on).</param>
        /// <param name="nextLine">A <see cref="TextBoxLine"/> representing the additional line created
        /// from the split.</param>
        public void SplitAt(int lineCharacterIndex, out TextBoxLine currentLine, out TextBoxLine nextLine)
        {
            if (lineCharacterIndex == 0)
            {
                // Split at the start of the line
                // First line is empty, second line is the full line
                currentLine = new TextBoxLine(TextBoxLines);
                nextLine = this;
                return;
            }
            else if (lineCharacterIndex == LineText.Length)
            {
                // Split at the end of the line
                // First line is the full line, second line is empty
                currentLine = this;
                nextLine = new TextBoxLine(TextBoxLines);
                return;
            }
            else
            {
                // Split up somewhere in the middle

                // Find the StyledText for the character
                StyledText text;
                int textIndex;
                int listIndex;
                FindLineCharacter(lineCharacterIndex, out text, out textIndex, out listIndex);

                // Split the StyledText
                StyledText left;
                StyledText right;
                text.SplitAt(textIndex, out left, out right);

                // The first line will be all the StyledTexts before the one split, plus the left side of the split
                // The second line will be the right side of the split, plus all the StyledTexts after the one split
                currentLine = new TextBoxLine(TextBoxLines);
                for (var i = 0; i < listIndex; i++)
                {
                    currentLine.Append(_texts[i]);
                }
                currentLine.Append(left);

                nextLine = new TextBoxLine(TextBoxLines);
                nextLine.Append(right);
                for (var i = listIndex + 1; i < _texts.Count; i++)
                {
                    nextLine.Append(_texts[i]);
                }
            }
        }
    }
}