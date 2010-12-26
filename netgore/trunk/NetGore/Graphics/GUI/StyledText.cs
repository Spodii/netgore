using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using SFML.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Describes a string of text and the style it uses.
    /// </summary>
    public class StyledText
    {
        static readonly Color _colorForDefault = new Color(0, 0, 0, 0);
        static readonly StyledText _empty = new StyledText(string.Empty);
        static readonly StyledText[] _emptyArray = new StyledText[0];
        static readonly StyledText _lineBreak = new StyledText(Environment.NewLine);
        static readonly char[] _splitChars = new char[] { '-', '_', ' ', '\n', ',', ';', '.', '!', '?' };

        readonly Color _color;
        readonly string _text;

        Font _fontUsedToMeasure;
        float _width;

        /// <summary>
        /// Initializes a new instance of the <see cref="StyledText"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public StyledText(string text) : this(text, ColorForDefault)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StyledText"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="color">The color.</param>
        public StyledText(string text, Color color)
        {
            text = text.Replace("\r\n", "\n");
            text = text.Replace("\r", "\n");

            _text = text;
            _color = color;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StyledText"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="sourceStyle">The StyledText to copy the style from.</param>
        public StyledText(string text, StyledText sourceStyle)
        {
            text = text.Replace("\r\n", "\n");
            text = text.Replace("\r", "\n");

            _text = text;
            _color = sourceStyle.Color;
        }

        /// <summary>
        /// Gets the color of the text.
        /// </summary>
        public Color Color
        {
            get { return _color; }
        }

        /// <summary>
        /// Gets the <see cref="Color"/> to use to denote that the <see cref="StyledText"/> should use the
        /// specified default color when drawing.
        /// </summary>
        public static Color ColorForDefault
        {
            get { return _colorForDefault; }
        }

        /// <summary>
        /// Gets a <see cref="StyledText"/> with an empty string.
        /// </summary>
        public static StyledText Empty
        {
            get { return _empty; }
        }

        /// <summary>
        /// Gets an empty array of <see cref="StyledText"/>s.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public static StyledText[] EmptyArray
        {
            get { return _emptyArray; }
        }

        /// <summary>
        /// Gets a <see cref="StyledText"/> that contains just a line break.
        /// </summary>
        public static StyledText LineBreak
        {
            get { return _lineBreak; }
        }

        /// <summary>
        /// Gets the preferred chars that are used to split lines of text apart at.
        /// </summary>
        public static IEnumerable<char> SplitChars
        {
            get { return _splitChars; }
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        public string Text
        {
            get { return _text; }
        }

        /// <summary>
        /// Concatenates the <see cref="StyledText"/>s that have the same style together while retaining order.
        /// </summary>
        /// <param name="input">The <see cref="StyledText"/>s to concatenate.</param>
        /// <returns>The concatenated <see cref="StyledText"/>s that have the same style.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public static List<StyledText> Concat(StyledText[] input)
        {
            var ret = new List<StyledText>(input.Length);

            for (var i = 0; i < input.Length; i++)
            {
                var current = input[i];
                while (i + 1 < input.Length && current.HasSameStyle(input[i + 1]))
                {
                    current += input[i + 1].Text;
                    i++;
                }
                ret.Add(current);
            }

            return ret;
        }

        /// <summary>
        /// Concatenates the <see cref="StyledText"/>s that have the same style together while retaining order.
        /// </summary>
        /// <param name="input">The <see cref="StyledText"/>s to concatenate.</param>
        /// <returns>The concatenated <see cref="StyledText"/>s that have the same style.</returns>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public static IEnumerable<StyledText> Concat(List<StyledText> input)
        {
            var ret = new List<StyledText>(input.Count);

            for (var i = 0; i < input.Count; i++)
            {
                var current = input[i];
                while (i + 1 < input.Count && current.HasSameStyle(input[i + 1]))
                {
                    current += input[i + 1].Text;
                    i++;
                }
                ret.Add(current);
            }

            return ret;
        }

        /// <summary>
        /// Draws the <see cref="StyledText"/> to the <paramref name="spriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch"><see cref="ISpriteBatch"/> to draw the <see cref="StyledText"/> to.</param>
        /// <param name="font"><see cref="Font"/> to use for drawing the characters.</param>
        /// <param name="position">Top-left corner of where to begin drawing the <see cref="StyledText"/>.</param>
        /// <param name="defaultColor">The default color to use for drawing the text. Only used if the
        /// <see cref="Color"/> is equal to <see cref="ColorForDefault"/>.</param>
        public void Draw(ISpriteBatch spriteBatch, Font font, Vector2 position, Color defaultColor)
        {
            var colorToUse = (Color == ColorForDefault ? defaultColor : Color);

            spriteBatch.DrawString(font, Text, position, colorToUse);
        }

        /// <summary>
        /// Finds the 0-based index of the first character that will fit into a single line when starting
        /// from the end of the line and working backwards.
        /// </summary>
        /// <param name="text">The text to check.</param>
        /// <param name="font">The <see cref="Font"/> used to measure the length.</param>
        /// <param name="maxLineLength">The maximum allowed line length in pixels.</param>
        /// <returns>The 0-based index of the first character that will fit into a single line.</returns>
        public static int FindFirstFittingChar(string text, Font font, int maxLineLength)
        {
            var sb = new StringBuilder(text);

            for (var i = 0; i < text.Length; i++)
            {
                var length = font.MeasureString(sb).X;
                if (length <= maxLineLength)
                    return text.Length - sb.Length;
                sb.Remove(0, 1);
            }

            return 0;
        }

        /// <summary>
        /// Finds the index of a string to split it at.
        /// </summary>
        /// <param name="text">Text to split.</param>
        /// <param name="maxChars">Maximum number of chars allowed in the primary string.</param>
        /// <returns>The index of a string to split it at.</returns>
        public static int FindIndexToSplitAt(string text, int maxChars)
        {
            // Split at either the first acceptable split character
            for (var i = maxChars; i > Math.Max(0, maxChars - 8); i--)
            {
                if (SplitChars.Contains(text[i]))
                    return i;
            }

            // No valid split characters were found, so just split at the max length
            return maxChars;
        }

        /// <summary>
        /// Finds the 0-based index of the last character that will fit into a single line.
        /// </summary>
        /// <param name="text">The text to check.</param>
        /// <param name="font">The <see cref="Font"/> used to measure the length.</param>
        /// <param name="maxLineLength">The maximum allowed line length in pixels.</param>
        /// <returns>The 0-based index of the last character that will fit into a single line.</returns>
        public static int FindLastFittingChar(string text, Font font, int maxLineLength)
        {
            var sb = new StringBuilder(text);

            for (var i = 0; i < text.Length; i++)
            {
                var length = font.MeasureString(sb).X;
                if (length <= maxLineLength)
                    return sb.Length;

                sb.Length--;
            }

            return 0;
        }

        /// <summary>
        /// Gets the width of this <see cref="StyledText"/> for the given <paramref name="font"/>.
        /// </summary>
        /// <param name="font">The <see cref="Font"/> to use to measure.</param>
        /// <returns>The width of this <see cref="StyledText"/> for the given <paramref name="font"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="font" /> is <c>null</c>.</exception>
        public float GetWidth(Font font)
        {
            if (font == null)
                throw new ArgumentNullException("font");

            // Re-measure (or measure for the first time
            if (_fontUsedToMeasure == null || _fontUsedToMeasure != font)
            {
                _width = font.MeasureString(Text).X;
                _fontUsedToMeasure = font;
            }

            return _width;
        }

        /// <summary>
        /// Gets if this <see cref="StyledText"/> has the same style as another <see cref="StyledText"/>.
        /// </summary>
        /// <param name="other">The other <see cref="StyledText"/>.</param>
        /// <returns>True if this StyledText has same style as the other <see cref="StyledText"/>; otherwise false.</returns>
        public bool HasSameStyle(StyledText other)
        {
            return Color == other.Color;
        }

        /// <summary>
        /// Splits the <see cref="StyledText"/> into multiple pieces using the given <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator">An array of unicode characters that delimit the substrings in this instance,
        /// an empty array that contains no delimiters, or null.</param>
        /// <returns>An array containing the pieces of the <see cref="StyledText"/> split using the given
        /// <paramref name="separator"/>.</returns>
        public StyledText[] Split(params char[] separator)
        {
            var strings = Text.Split(separator);

            var ret = new StyledText[strings.Length];
            for (var i = 0; i < ret.Length; i++)
            {
                if (strings[i] == Text)
                    ret[i] = this;
                else
                    ret[i] = new StyledText(strings[i], Color);
            }

            return ret;
        }

        /// <summary>
        /// Splits the <see cref="StyledText"/> into multiple pieces using the given <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator">An array of Unicode characters that delimit the substrings in this instance,
        /// an empty array that contains no delimiters, or null.</param>
        /// <param name="count">The maximum number of substrings to return.</param>
        /// <returns>An array containing the pieces of the <see cref="StyledText"/> split using the given
        /// <paramref name="separator"/>.</returns>
        public StyledText[] Split(char[] separator, int count)
        {
            var strings = Text.Split(separator, count);

            var ret = new StyledText[strings.Length];
            for (var i = 0; i < ret.Length; i++)
            {
                if (strings[i] == Text)
                    ret[i] = this;
                else
                    ret[i] = new StyledText(strings[i], Color);
            }

            return ret;
        }

        /// <summary>
        /// Splits the <see cref="StyledText"/> into multiple pieces using the given <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator">An array of Unicode characters that delimit the substrings in this instance,
        /// an empty array that contains no delimiters, or null.</param>
        /// <param name="count">The maximum number of substrings to return.</param>
        /// <param name="options">Specify <see cref="System.StringSplitOptions.RemoveEmptyEntries"/> to omit empty array
        /// elements from the array returned, or <see cref="System.StringSplitOptions.None"/> to include
        /// empty array elements in the array returned.</param>
        /// <returns>An array containing the pieces of the StyledText split using the given
        /// <paramref name="separator"/>.</returns>
        public StyledText[] Split(char[] separator, int count, StringSplitOptions options)
        {
            var strings = Text.Split(separator, count, options);

            var ret = new StyledText[strings.Length];
            for (var i = 0; i < ret.Length; i++)
            {
                if (strings[i] == Text)
                    ret[i] = this;
                else
                    ret[i] = new StyledText(strings[i], Color);
            }

            return ret;
        }

        /// <summary>
        /// Splits the <see cref="StyledText"/> into multiple pieces using the given <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator">An array of strings that delimit the substrings in this string,
        /// an empty array that contains no delimiters, or null.</param>
        /// <param name="count">The maximum number of substrings to return.</param>
        /// <param name="options">Specify <see cref="System.StringSplitOptions.RemoveEmptyEntries"/> to omit empty array
        /// elements from the array returned, or <see cref="System.StringSplitOptions.None"/> to include
        /// empty array elements in the array returned.</param>
        /// <returns>An array containing the pieces of the StyledText split using the given
        /// <paramref name="separator"/>.</returns>
        public StyledText[] Split(string[] separator, int count, StringSplitOptions options)
        {
            var strings = Text.Split(separator, count, options);

            var ret = new StyledText[strings.Length];
            for (var i = 0; i < ret.Length; i++)
            {
                if (strings[i] == Text)
                    ret[i] = this;
                else
                    ret[i] = new StyledText(strings[i], Color);
            }

            return ret;
        }

        /// <summary>
        /// Splits the <see cref="StyledText"/> into multiple pieces using the given <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator">An array of Unicode characters that delimit the substrings in this instance,
        /// an empty array that contains no delimiters, or null.</param>
        /// <param name="options">Specify <see cref="System.StringSplitOptions.RemoveEmptyEntries"/> to omit empty array
        /// elements from the array returned, or <see cref="System.StringSplitOptions.None"/> to include
        /// empty array elements in the array returned.</param>
        /// <returns>An array containing the pieces of the StyledText split using the given
        /// <paramref name="separator"/>.</returns>
        public StyledText[] Split(char[] separator, StringSplitOptions options)
        {
            var strings = Text.Split(separator, options);

            var ret = new StyledText[strings.Length];
            for (var i = 0; i < ret.Length; i++)
            {
                if (strings[i] == Text)
                    ret[i] = this;
                else
                    ret[i] = new StyledText(strings[i], Color);
            }

            return ret;
        }

        /// <summary>
        /// Splits the <see cref="StyledText"/> into multiple pieces using the given <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator">An array of strings that delimit the substrings in this string,
        /// an empty array that contains no delimiters, or null.</param>
        /// <param name="options">Specify <see cref="System.StringSplitOptions.RemoveEmptyEntries"/> to omit empty array
        /// elements from the array returned, or <see cref="System.StringSplitOptions.None"/> to include
        /// empty array elements in the array returned.</param>
        /// <returns>An array containing the pieces of the StyledText split using the given
        /// <paramref name="separator"/>.</returns>
        public StyledText[] Split(string[] separator, StringSplitOptions options)
        {
            var strings = Text.Split(separator, options);

            var ret = new StyledText[strings.Length];
            for (var i = 0; i < ret.Length; i++)
            {
                if (strings[i] == Text)
                    ret[i] = this;
                else
                    ret[i] = new StyledText(strings[i], Color);
            }

            return ret;
        }

        /// <summary>
        /// Splits the <see cref="StyledText"/> at the given index into two parts. The character at the given
        /// <paramref name="charIndex"/> will end up in the <paramref name="right"/> side.
        /// </summary>
        /// <param name="charIndex">The 0-based character index to split at.</param>
        /// <param name="left">The left side of the split.</param>
        /// <param name="right">The right side of the split.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="charIndex"/> is less than 0 or greater
        /// than the length of the <see cref="Text"/>.</exception>
        public void SplitAt(int charIndex, out StyledText left, out StyledText right)
        {
            if (charIndex < 0 || charIndex > Text.Length)
                throw new ArgumentOutOfRangeException("charIndex");

            if (charIndex == 0)
            {
                left = new StyledText(string.Empty, this);
                right = this;
            }
            else if (charIndex == Text.Length)
            {
                left = this;
                right = new StyledText(string.Empty, this);
            }
            else
            {
                left = Substring(0, charIndex);
                right = Substring(charIndex);
            }
        }

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a specified character position.
        /// </summary>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
        /// <returns>A <see cref="StyledText"/> equivalent to the substring that begins at startIndex in this instance,
        /// or <see cref="StyledText"/> with an empty string if startIndex is equal to the length of this instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or
        /// greater than the length of this instance.</exception>
        public StyledText Substring(int startIndex)
        {
            var s = Text.Substring(startIndex);
            if (string.IsNullOrEmpty(s))
                return Empty;

            return new StyledText(s, Color);
        }

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a specified character position.
        /// </summary>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <returns>A <see cref="StyledText"/> equivalent to the substring that begins at startIndex in this instance,
        /// or <see cref="StyledText"/> with an empty string if startIndex is equal to the length of this instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or
        /// greater than the length of this instance -or- startIndex or length is less than zero.</exception>
        public StyledText Substring(int startIndex, int length)
        {
            var s = Text.Substring(startIndex, length);
            if (string.IsNullOrEmpty(s))
                return Empty;

            return new StyledText(s, Color);
        }

        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public static List<List<StyledText>> ToMultiline(IEnumerable<StyledText> texts, bool putInputOnNewLines, Font font,
                                                         int maxLineLength)
        {
            var lines = ToMultiline(texts, putInputOnNewLines);
            var ret = new List<List<StyledText>>();

            var currentLineText = new StringBuilder();
            var linePartsStack = new Stack<StyledText>();

            foreach (var line in lines)
            {
                currentLineText.Length = 0;
                var retLine = new List<StyledText>();

                // Add all the pieces in the line to the line parts stack
                for (var i = line.Count - 1; i >= 0; i--)
                {
                    linePartsStack.Push(line[i]);
                }

                while (linePartsStack.Count > 0)
                {
                    var current = linePartsStack.Pop();
                    currentLineText.Append(current.Text);

                    // Check if the line has become too long
                    if (font.MeasureString(currentLineText).X <= maxLineLength)
                        retLine.Add(current);
                    else
                    {
                        var s = currentLineText.ToString();

                        var lastFittingChar = FindLastFittingChar(s, font, maxLineLength);
                        var splitAt = FindIndexToSplitAt(s, lastFittingChar - 1);

                        splitAt -= (s.Length - current.Text.Length);

                        // Don't try to split farther back than the current styled text block unless the character is larger
                        // than the maximum line length. In which case, we just throw the character into the line (not much
                        // else we can do other than just show nothing, and that is no better...)
                        if (retLine.Count == 0 && splitAt < 0)
                            splitAt = 0;

                        if (splitAt + 1 == current.Text.Length)
                            retLine.Add(current);
                        else
                        {
                            var left = current.Substring(0, splitAt + 1);
                            var right = current.Substring(splitAt + 1);

                            // Split like normal
                            if (left.Text.Length > 0)
                                retLine.Add(left);

                            linePartsStack.Push(right);
                        }

                        // Add the line to the return list, create the new line buffer, and reset the line string
                        if (retLine.Count > 0)
                        {
                            ret.Add(retLine);
                            retLine = new List<StyledText>();
                        }

                        currentLineText.Length = 0;
                    }
                }

                if (retLine.Count > 0)
                {
                    Debug.Assert(ret.Count == 0 || ret.Last() != retLine, "Uh-oh, we added the same line twice...");
                    ret.Add(retLine);
                }
            }

            return ret;
        }

        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public static List<StyledText> ToMultiline(StyledText text)
        {
            var ret = new List<StyledText>();

            var split = text.Split('\n');
            foreach (var s in split)
            {
                var newS = new StyledText(s.Text.Replace('\r'.ToString(), string.Empty), s);
                ret.Add(newS);
            }

            return ret;
        }

        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public static List<List<StyledText>> ToMultiline(IEnumerable<StyledText> texts, bool putInputOnNewLines)
        {
            var ret = new List<List<StyledText>>();

            if (putInputOnNewLines)
            {
                foreach (var t in texts)
                {
                    var split = t.Split('\n');
                    foreach (var s in split)
                    {
                        ret.Add(new List<StyledText> { s });
                    }
                }
            }
            else
            {
                var line = new List<StyledText>();
                foreach (var t in texts)
                {
                    var split = t.Split('\n');

                    if (split[0].Text.Length > 0)
                        line.Add(split[0]);

                    for (var i = 1; i < split.Length; i++)
                    {
                        if (split[i].Text.Length > 0)
                        {
                            ret.Add(line);
                            line = new List<StyledText> { split[i] };
                        }
                    }
                }

                if (line.Count > 0)
                    ret.Add(line);
            }

            return ret;
        }

        /// <summary>
        /// Removes all of the line breaks from the <see cref="StyledText"/>.
        /// </summary>
        public StyledText ToSingleline()
        {
            return ToSingleline(string.Empty);
        }

        /// <summary>
        /// Removes all of the line breaks from the <see cref="StyledText"/>.
        /// </summary>
        /// <param name="replacementString">The string to replace the line breaks with.</param>
        /// <returns>A <see cref="StyledText"/> with all the line breaks replaced with the given
        /// <see cref="replacementString"/>.</returns>
        public StyledText ToSingleline(string replacementString)
        {
            if (string.IsNullOrEmpty(Text))
                return new StyledText(string.Empty, Color);

            var newText = Text.Replace("\r\n", replacementString);
            if (newText.Length > 0)
                newText = newText.Replace("\n", replacementString);
            if (newText.Length > 0)
                newText = newText.Replace("\r", replacementString);

            return new StyledText(newText, Color);
        }

        /// <summary>
        /// Gets the concatenated string for a collection of <see cref="StyledText"/>s.
        /// </summary>
        /// <param name="texts">The <see cref="StyledText"/>s.</param>
        /// <returns>The concatenated string for the <paramref name="texts"/>.</returns>
        public static string ToString(IEnumerable<StyledText> texts)
        {
            if (texts == null)
                return string.Empty;

            var sb = new StringBuilder();
            foreach (var text in texts)
            {
                sb.Append(text);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the concatenated string for a collection of <see cref="StyledText"/>s.
        /// </summary>
        /// <param name="texts">The <see cref="StyledText"/>s.</param>
        /// <param name="delimiter">The delimiter used between each of the <paramref name="texts"/>.</param>
        /// <returns>The concatenated string for the <paramref name="texts"/>.</returns>
        public static string ToString(IEnumerable<StyledText> texts, string delimiter)
        {
            if (texts == null)
                return string.Empty;

            var sb = new StringBuilder();
            foreach (var text in texts)
            {
                sb.Append(text);
                sb.Append(delimiter);
            }

            // Remove the last delimiter
            if (sb.Length > 0)
                sb.Length -= delimiter.Length;

            return sb.ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Text;
        }

        /// <summary>
        /// Implements operator + for a StyledText and string.
        /// </summary>
        /// <param name="l">The StyledText.</param>
        /// <param name="r">The string.</param>
        /// <returns>A new StyledText with the concatenated strings of the two arguments.</returns>
        public static StyledText operator +(StyledText l, string r)
        {
            if (string.IsNullOrEmpty(r))
                return l;

            return new StyledText(l.Text + r, l.Color);
        }
    }
}