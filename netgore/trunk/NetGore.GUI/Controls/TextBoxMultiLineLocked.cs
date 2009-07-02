using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// A multi-line box of text that does not support text editing, but does support scrolling
    /// and manual text alteration.
    /// </summary>
    public class TextBoxMultiLineLocked : TextControl
    {
        /// <summary>
        /// Extra offset of the drawn text on the TextBox.
        /// </summary>
        const float _borderOffsetBonus = 1;

        /// <summary>
        /// Initial size of the buffer.
        /// </summary>
        const int _initialBufferSize = 128;

        /// <summary>
        /// Buffered lines of text
        /// </summary>
        readonly List<TextBoxLine> _lines = new List<TextBoxLine>(128);

        int _bufferOffset = 0;

        int _bufferSize = _initialBufferSize;

        /// <summary>
        /// Gets or sets how this TextBox handles scrolling when new lines of text is appended or removed.
        /// </summary>
        public TextBoxAutoScrollMode AutoScrollMode { get; set; }

        /// <summary>
        /// Gets or sets the current buffer offset. This value indicates the the index of the first line that
        /// is displayed. 0 indicates the very first line is displayed while BufferSize - 1 indicates only
        /// the last line is displayed.
        /// </summary>
        public int BufferOffset
        {
            get { return _bufferOffset; }
            set
            {
                // Keep the new value in a valid range
                if (value > _lines.Count - 1)
                {
                    if (_lines.Count > 0)
                        _bufferOffset = _lines.Count - 1;
                    else
                        _bufferOffset = 0;
                }
                else if (value < 0)
                    _bufferOffset = 0;
                else
                    _bufferOffset = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of lines that can be buffered.
        /// </summary>
        public int BufferSize
        {
            get { return _bufferSize; }
            set { _bufferSize = value; }
        }

        /// <summary>
        /// Gets the offset from the top-left corner of the TextBox to draw the text.
        /// </summary>
        Vector2 DrawOffset
        {
            get
            {
                ControlBorder border = Border;
                var v = new Vector2(_borderOffsetBonus);
                if (border != null)
                    v += new Vector2(border.LeftWidth, border.TopHeight);
                return v;
            }
        }

        /// <summary>
        /// Gets the number of lines in this TextBox.
        /// </summary>
        public int Lines
        {
            get { return _lines.Count; }
        }

        /// <summary>
        /// Gets the number of lines to draw.
        /// </summary>
        int LinesToDraw
        {
            get
            {
                ControlBorder border = Border;
                float borderHeight = border != null ? border.Height : 0;
                return (int)((Size.Y - borderHeight - (_borderOffsetBonus * 2)) / Font.LineSpacing);
            }
        }

        /// <summary>
        /// Gets the maximum length of a single line.
        /// </summary>
        float MaxLineLength
        {
            get
            {
                ControlBorder border = Border;
                if (border != null)
                    return Size.X - border.Width - (_borderOffsetBonus * 2);
                return Size.X - (_borderOffsetBonus * 2);
            }
        }

        /// <summary>
        /// Gets or sets the text of the TextControl.
        /// </summary>
        public override string Text
        {
            get
            {
                var sb = new StringBuilder(2048);
                foreach (TextBoxLine line in _lines)
                {
                    foreach (StyledText t in line.Text)
                    {
                        sb.Append(t.Text);
                    }
                    sb.AppendLine();
                }
                return sb.ToString();
            }
            set
            {
                Clear();
                Append(value);
            }
        }

        public TextBoxMultiLineLocked(GUIManagerBase gui, TextControlSettings settings, string text, SpriteFont font,
                                      Vector2 position, Vector2 size, Control parent)
            : base(gui, settings, text, font, position, size, parent)
        {
            // Set the properties to match that of a TextBox
            CanDrag = false;
            CanFocus = true;
            AutoScrollMode = TextBoxAutoScrollMode.ScrollIfCurrent;

            // Append all of the text since it doesn't get set properly through the overload
            Clear();
            if (!string.IsNullOrEmpty(text))
                Append(text);
        }

        public TextBoxMultiLineLocked(string text, Vector2 position, Vector2 size, Control parent)
            : this(parent.GUIManager, parent.GUIManager.TextBoxSettings, text, parent.GUIManager.Font, position, size, parent)
        {
        }

        /// <summary>
        /// Appends a string of text to the TextBox.
        /// </summary>
        /// <param name="text">Text to append to the TextBox.</param>
        /// <param name="color">Color of the text to append.</param>
        public void Append(string text, Color color)
        {
            Append(new List<StyledText> { new StyledText(text, color) });
        }

        /// <summary>
        /// Appends a set of styled text to the TextBox.
        /// </summary>
        /// <param name="text">Text to append to the TextBox.</param>
        public void Append(List<StyledText> text)
        {
            int startBufferOffset = BufferOffset;

            // Break up the text into the new lines
            IEnumerable<TextBoxLine> newLines = BreakString(text);
            int count = newLines.Count();

            // Make room in the buffer if needed
            int removeAmount = (_lines.Count + count) - BufferSize;
            if (removeAmount > 0)
                _lines.RemoveRange(BufferSize - removeAmount - 1, removeAmount);

            // Add the new lines
            _lines.InsertRange(0, newLines);

            // AutoScroll
            switch (AutoScrollMode)
            {
                case TextBoxAutoScrollMode.None:
                    // Do nothing
                    break;

                case TextBoxAutoScrollMode.AlwaysScroll:
                    // Always scroll
                    BufferOffset = 0;
                    break;

                case TextBoxAutoScrollMode.ScrollIfCurrent:
                    // Scroll only if we started at 0
                    if (startBufferOffset == 0)
                        BufferOffset = 0;
                    break;
            }
        }

        /// <summary>
        /// Appends a string of text to the TextBox.
        /// </summary>
        /// <param name="text">Text to append to the TextBox.</param>
        public void Append(string text)
        {
            Append(text, ForeColor);
        }

        /// <summary>
        /// Brings a single string of styled text into multiple lines, if needed.
        /// </summary>
        /// <param name="text">Text to process.</param>
        /// <returns>IEnumerable of all the resulting lines</returns>
        IEnumerable<TextBoxLine> BreakString(List<StyledText> text)
        {
            var ret = new List<TextBoxLine>(2);
            var totalSB = new StringBuilder(256); // StringBuilder used for building the complete string

            // Loop until our list is empty
            while (text.Count > 0)
            {
                // Join all the text together to find the complete string
                totalSB.Length = 0;
                foreach (StyledText t in text)
                {
                    totalSB.Append(t.Text);
                }
                string totalText = totalSB.ToString();

                // Get the total length to see if the text even needs to be split
                float totalLength = Font.MeasureString(totalText).X;
                if (totalLength > MaxLineLength)
                {
                    // The text needs to be split, so find exactly where the text needs to be split
                    int lastFit = FindLastFittingChar(totalText);

                    // Find where to split the first line at
                    int splitAt = FindIndexToSplitAt(totalText, lastFit);

                    // Perform the split and rebuild the string as StyledText
                    List<StyledText> line = SplitStyledText(text, splitAt);

                    // Return the new line
                    ret.Add(new TextBoxLine(line));
                }
                else
                {
                    // Finally, the whole line fits - we can now leave this stupid method
                    ret.Add(new TextBoxLine(text));
                    break;
                }
            }

            return ret;
        }

        public void Clear()
        {
            _lines.Clear();
            BufferOffset = 0;
        }

        protected override void DrawControl(SpriteBatch spriteBatch)
        {
            base.DrawControl(spriteBatch);

            // Check for text to draw
            if (_lines.Count < 1)
                return;

            // Find the lines of the buffer to draw, and where to draw it
            int totalLines = LinesToDraw;
            int high = Math.Min(_lines.Count - 1, BufferOffset + totalLines - 1);
            int low = BufferOffset;

            Vector2 offset = ScreenPosition + DrawOffset;
            offset.Y += (totalLines - 1) * Font.LineSpacing;

            for (int i = low; i <= high; i++)
            {
                DrawText(spriteBatch, Font, _lines[i], offset);
                offset.Y -= Font.LineSpacing;
            }
        }

        /// <summary>
        /// Draws a TextBoxLine to the screen.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw with.</param>
        /// <param name="font">Font to use.</param>
        /// <param name="text">TextBoxLine to draw.</param>
        /// <param name="pos">Screen position to draw the text at.</param>
        void DrawText(SpriteBatch spriteBatch, SpriteFont font, TextBoxLine text, Vector2 pos)
        {
            StyledText last = text.Text.Last();

            // No last element implies an empty set
            if (last == null)
                return;

            foreach (StyledText t in text.Text)
            {
                // Draw the text
                t.Draw(spriteBatch, Font, pos);

                // If this isn't the last element, increase the offset
                if (t != last)
                    pos.X += font.MeasureString(t.Text).X;
            }
        }

        /// <summary>
        /// Finds the index of a string to split it at.
        /// </summary>
        /// <param name="text">Text to split.</param>
        /// <param name="max">Maximum number of chars allowed in the primary string.</param>
        /// <returns>The index of a string to split it at.</returns>
        static int FindIndexToSplitAt(string text, int max)
        {
            // Split at either the first acceptable split character
            for (int i = max; i > Math.Max(0, max - 8); i--)
            {
                switch (text[i])
                {
                    case ' ':
                    case ',':
                    case ';':
                    case '.':
                    case '-':
                    case '+':
                    case '_':
                        // Valid split char found
                        return i;
                }
            }

            // No valid split characters were found, so just split at the max length
            return max;
        }

        /// <summary>
        /// Finds the 0-based index of the last character that will fit into a single line.
        /// </summary>
        /// <param name="text">Text to check.</param>
        /// <returns>The 0-based index of the last character that will fit into a single line.</returns>
        int FindLastFittingChar(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                float length = Font.MeasureString(text.Substring(0, i)).X;
                if (length > MaxLineLength)
                    return i - 1;
            }

            // All text fits
            return text.Length - 1;
        }

        /// <summary>
        /// Splits a string of text into two parts and rebuilds the StyledText from which it came from.
        /// </summary>
        /// <param name="text">StyledText to split up. After this method runs, this will only contain the
        /// remaining (right-hand side of the split) text.</param>
        /// <param name="splitIndex">Index to create the split at.</param>
        /// <returns>Left-hand side (char index 0 to splitIndex) of the split text.</returns>
        static List<StyledText> SplitStyledText(List<StyledText> text, int splitIndex)
        {
            // Find which StyledText index the splitIndex is in, and the charOffset of the splitIndex from the
            // start of that StyledText
            int lengthSum = 0;
            int charOffset = -1;
            int textIndex;
            for (textIndex = 0; textIndex < text.Count; textIndex++)
            {
                int length = text[textIndex].Text.Length;

                if (length + lengthSum >= splitIndex)
                {
                    charOffset = splitIndex - lengthSum;
                    break;
                }

                lengthSum += length;
            }

            // If the charOffset is still -1, then the text can't be split
            if (charOffset < 0)
                throw new ArgumentException("splitIndex is greater than the sum of the length of the specified texts.");

            // Create the left hand side, which will always contain every StyledText before the index
            // we will split at
            var left = new List<StyledText>();
            for (int i = 0; i < textIndex; i++)
            {
                left.Add(text[i]);
            }

            // Check if we can avoid splitting
            if (charOffset == 0)
            {
                // The splitIndex is zero, we got lucky and the split happened between the StyledTexts, so no
                // reconstruction is needed

                // Remove parts that are now in the left hand side
                text.RemoveRange(0, textIndex);
            }
            else
            {
                // The character to split at resides in the middle of a StyledText, so we will have to split
                // up one of the StyledTexts into two
                StyledText a;
                StyledText b;
                SplitStyledText(text[textIndex], charOffset, out a, out b);

                // Remove parts that are now in the left hand side plus the line that got broken up
                text.RemoveRange(0, textIndex + 1);

                // Join the resulting left side of the split into the end of the left side list, and the resulting
                // right side of the split at the start of the right side list
                if (a != null)
                    left.Add(a);
                if (b != null)
                    text.Insert(0, b);
            }

            return left;
        }

        /// <summary>
        /// Splits a single StyledText into two parts.
        /// </summary>
        /// <param name="text">StyledText to split.</param>
        /// <param name="offset">Text offset to perform the split at.</param>
        /// <param name="left">Left-hand side of the resulting split.</param>
        /// <param name="right">Right-hand side of the resulting split.</param>
        static void SplitStyledText(StyledText text, int offset, out StyledText left, out StyledText right)
        {
            // Check for an unneeded split
            if (offset == 0)
            {
                left = text;
                right = null;
                return;
            }
            if (offset == text.Text.Length)
            {
                left = null;
                right = text;
                return;
            }

            // Check for an invalid split
            if (offset < 0 || offset > text.Text.Length)
                throw new ArgumentOutOfRangeException("offset");

            // Perform the split
            left = new StyledText(text.Text.Substring(0, offset).TrimEnd(), text.Color);
            right = new StyledText(text.Text.Substring(offset).TrimStart(), text.Color);
        }

        /// <summary>
        /// Describes a single line of the TextBox.
        /// </summary>
        class TextBoxLine
        {
            /// <summary>
            /// IEnumerable of all the text in this line.
            /// </summary>
            public readonly IEnumerable<StyledText> Text;

            public TextBoxLine(IEnumerable<StyledText> text)
            {
                Text = text;
            }
        }
    }
}