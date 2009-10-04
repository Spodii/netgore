using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Describes a string of text and the style it uses.
    /// </summary>
    public class StyledText
    {
        /// <summary>
        /// A <see cref="StyledText"/> with an empty string.
        /// </summary>
        public static readonly StyledText Empty = new StyledText(string.Empty);

        /// <summary>
        /// An empty array of <see cref="StyledText"/>s.
        /// </summary>
        public static readonly StyledText[] EmptyArray = new StyledText[0];

        /// <summary>
        /// A <see cref="StyledText"/> that contains just a line break.
        /// </summary>
        public static readonly StyledText LineBreak = new StyledText(Environment.NewLine);

        static readonly Color _colorForDefault = new Color(0, 0, 0, 0);
        static readonly char[] _splitChars = new char[] { '-', '_', ' ', '\n', ',', ';', '.', '!', '?' };

        readonly Color _color;
        readonly string _text;

        SpriteFont _fontUsedToMeasure;
        float _width;

        /// <summary>
        /// Gets the <see cref="Color"/> to use to denote that the <see cref="StyledText"/> should use the
        /// specified default color when drawing.
        /// </summary>
        public static Color ColorForDefault
        {
            get { return _colorForDefault; }
        }

        /// <summary>
        /// Gets the preferred chars that are used to split lines of text apart at.
        /// </summary>
        public static IEnumerable<char> SplitChars
        {
            get { return _splitChars; }
        }

        /// <summary>
        /// Gets the color of the text.
        /// </summary>
        public Color Color
        {
            get { return _color; }
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        public string Text
        {
            get { return _text; }
        }

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
            _text = text;
            _color = sourceStyle.Color;
        }

        /// <summary>
        /// Concatenates the <see cref="StyledText"/>s that have the same style together while retaining order.
        /// </summary>
        /// <param name="input">The <see cref="StyledText"/>s to concatenate.</param>
        /// <returns>The concatenated <see cref="StyledText"/>s that have the same style.</returns>
        public static IEnumerable<StyledText> Concat(StyledText[] input)
        {
            var ret = new List<StyledText>(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                StyledText current = input[i];
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
        public static IEnumerable<StyledText> Concat(List<StyledText> input)
        {
            var ret = new List<StyledText>(input.Count);

            for (int i = 0; i < input.Count; i++)
            {
                StyledText current = input[i];
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
        /// Draws the StyledText to the <paramref name="spriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw the StyledText to.</param>
        /// <param name="spriteFont">SpriteFont to use for drawing the characters.</param>
        /// <param name="position">Top-left corner of where to begin drawing the StyledText.</param>
        /// <param name="defaultColor">The default color to use for drawing the text. Only used if the
        /// <see cref="Color"/> is equal to <see cref="ColorForDefault"/>.</param>
        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, Vector2 position, Color defaultColor)
        {
            if (Color == ColorForDefault)
                spriteBatch.DrawString(spriteFont, Text, position, defaultColor);
            else
                spriteBatch.DrawString(spriteFont, Text, position, Color);
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
            for (int i = maxChars; i > Math.Max(0, maxChars - 8); i--)
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
        /// <param name="font">The SpriteFont used to measure the length.</param>
        /// <param name="maxLineLength">The maximum allowed line length in pixels.</param>
        /// <returns>The 0-based index of the last character that will fit into a single line.</returns>
        public static int FindLastFittingChar(string text, SpriteFont font, int maxLineLength)
        {
            StringBuilder sb = new StringBuilder(text);

            for (int i = 0; i < text.Length; i++)
            {
                float length = font.MeasureString(sb).X;
                if (length <= maxLineLength)
                    return sb.Length;
                sb.Length--;
            }

            Debug.Fail("No text fits into the given maxLineLength.");
            return 0;
        }

        /// <summary>
        /// Gets the width of this StyledText for the given <paramref name="font"/>.
        /// </summary>
        /// <param name="font">The SpriteFont to use to measure.</param>
        /// <returns>The width of this StyledText for the given <paramref name="font"/>.</returns>
        public float GetWidth(SpriteFont font)
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
        /// Gets if this StyledText has the same style as another StyledText.
        /// </summary>
        /// <param name="other">The other StyledText.</param>
        /// <returns>True if this StyledText has same style as the other StyledText; otherwise false.</returns>
        public bool HasSameStyle(StyledText other)
        {
            return Color == other.Color;
        }

        /// <summary>
        /// Splits the StyledText into multiple pieces using the given <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator">An array of Unicode characters that delimit the substrings in this instance,
        /// an empty array that contains no delimiters, or null.</param>
        /// <returns>An array containing the pieces of the StyledText split using the given
        /// <paramref name="separator"/>.</returns>
        public StyledText[] Split(params char[] separator)
        {
            var strings = Text.Split(separator);

            var ret = new StyledText[strings.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                if (strings[i] == Text)
                    ret[i] = this;
                else
                    ret[i] = new StyledText(strings[i], Color);
            }

            return ret;
        }

        /// <summary>
        /// Splits the StyledText into multiple pieces using the given <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator">An array of Unicode characters that delimit the substrings in this instance,
        /// an empty array that contains no delimiters, or null.</param>
        /// <param name="count">The maximum number of substrings to return.</param>
        /// <returns>An array containing the pieces of the StyledText split using the given
        /// <paramref name="separator"/>.</returns>
        public StyledText[] Split(char[] separator, int count)
        {
            var strings = Text.Split(separator, count);

            var ret = new StyledText[strings.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                if (strings[i] == Text)
                    ret[i] = this;
                else
                    ret[i] = new StyledText(strings[i], Color);
            }

            return ret;
        }

        /// <summary>
        /// Splits the StyledText into multiple pieces using the given <paramref name="separator"/>.
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
            for (int i = 0; i < ret.Length; i++)
            {
                if (strings[i] == Text)
                    ret[i] = this;
                else
                    ret[i] = new StyledText(strings[i], Color);
            }

            return ret;
        }

        /// <summary>
        /// Splits the StyledText into multiple pieces using the given <paramref name="separator"/>.
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
            for (int i = 0; i < ret.Length; i++)
            {
                if (strings[i] == Text)
                    ret[i] = this;
                else
                    ret[i] = new StyledText(strings[i], Color);
            }

            return ret;
        }

        /// <summary>
        /// Splits the StyledText into multiple pieces using the given <paramref name="separator"/>.
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
            for (int i = 0; i < ret.Length; i++)
            {
                if (strings[i] == Text)
                    ret[i] = this;
                else
                    ret[i] = new StyledText(strings[i], Color);
            }

            return ret;
        }

        /// <summary>
        /// Splits the StyledText into multiple pieces using the given <paramref name="separator"/>.
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
            for (int i = 0; i < ret.Length; i++)
            {
                if (strings[i] == Text)
                    ret[i] = this;
                else
                    ret[i] = new StyledText(strings[i], Color);
            }

            return ret;
        }

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a specified character position.
        /// </summary>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
        /// <returns>A StyledText equivalent to the substring that begins at startIndex in this instance,
        /// or StyledText with an empty string if startIndex is equal to the length of this instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or
        /// greater than the length of this instance.</exception>
        public StyledText Substring(int startIndex)
        {
            string s = Text.Substring(startIndex);
            if (string.IsNullOrEmpty(s))
                return this;

            return new StyledText(s, Color);
        }

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a specified character position.
        /// </summary>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <returns>A StyledText equivalent to the substring that begins at startIndex in this instance,
        /// or StyledText with an empty string if startIndex is equal to the length of this instance.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startIndex"/> is less than zero or
        /// greater than the length of this instance -or- startIndex or length is less than zero.</exception>
        public StyledText Substring(int startIndex, int length)
        {
            string s = Text.Substring(startIndex, length);
            if (string.IsNullOrEmpty(s))
                return this;

            return new StyledText(s, Color);
        }

        public static List<List<StyledText>> ToMultiline(IEnumerable<StyledText> texts, bool putInputOnNewLines, SpriteFont font,
                                                         int maxLineLength)
        {
            var lines = ToMultiline(texts, putInputOnNewLines);
            var ret = new List<List<StyledText>>();

            StringBuilder currentLineText = new StringBuilder();
            var linePartsStack = new Stack<StyledText>();

            foreach (var line in lines)
            {
                currentLineText.Length = 0;
                var retLine = new List<StyledText>();

                for (int i = line.Count - 1; i >= 0; i--)
                {
                    linePartsStack.Push(line[i]);
                }

                while (linePartsStack.Count > 0)
                {
                    StyledText current = linePartsStack.Pop();
                    currentLineText.Append(current.Text);

                    // Check if the line has become too long
                    if (font.MeasureString(currentLineText).X <= maxLineLength)
                        retLine.Add(current);
                    else
                    {
                        string s = currentLineText.ToString();

                        int lastFittingChar = FindLastFittingChar(currentLineText.ToString(), font, maxLineLength);
                        int splitAt = FindIndexToSplitAt(s, lastFittingChar);

                        StyledText left = current.Substring(0, splitAt + 1);
                        StyledText right = current.Substring(splitAt + 1);

                        retLine.Add(left);
                        ret.Add(retLine);
                        retLine = new List<StyledText>();

                        currentLineText.Length = 0;
                        linePartsStack.Push(right);
                    }
                }

                ret.Add(retLine);
            }

            return ret;
        }

        public static List<StyledText> ToMultiline(StyledText text)
        {
            var ret = new List<StyledText>();

            var split = text.Split('\n');
            foreach (StyledText s in split)
            {
                StyledText newS = new StyledText(s.Text.Replace('\r'.ToString(), string.Empty), s);
                ret.Add(newS);
            }

            return ret;
        }

        public static List<List<StyledText>> ToMultiline(IEnumerable<StyledText> texts, bool putInputOnNewLines)
        {
            var ret = new List<List<StyledText>>();

            if (putInputOnNewLines)
            {
                foreach (StyledText t in texts)
                {
                    var split = t.Split('\n');
                    foreach (StyledText s in split)
                    {
                        StyledText newS = new StyledText(s.Text.Replace('\r'.ToString(), string.Empty), s);
                        ret.Add(new List<StyledText> { newS });
                    }
                }
            }
            else
            {
                var line = new List<StyledText>();
                foreach (StyledText t in texts)
                {
                    var split = t.Split('\n');
                    line.Add(split[0]);

                    if (split.Length > 1)
                    {
                        for (int i = 1; i < split.Length; i++)
                        {
                            ret.Add(line);
                            line = new List<StyledText> { split[i] };
                        }
                    }
                }

                ret.Add(line);
            }

            return ret;
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