using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Describes a string of text and the style it uses. This class is immutable.
    /// </summary>
    public class StyledText
    {
        /// <summary>
        /// A StyledText with an empty string.
        /// </summary>
        public static readonly StyledText Empty = new StyledText(string.Empty, Color.White);

        readonly Color _color;
        readonly string _text;

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
        /// Draws the StyledText to the <paramref name="spriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw the StyledText to.</param>
        /// <param name="spriteFont">SpriteFont to use for drawing the characters.</param>
        /// <param name="position">Top-left corner of where to begin drawing the StyledText.</param>
        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, Vector2 position)
        {
            spriteBatch.DrawString(spriteFont, Text, position, Color);
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
            return new StyledText(s, Color);
        }

        /// <summary>
        /// Implements operator + for a StyledText and string.
        /// </summary>
        /// <param name="l">The StyledText.</param>
        /// <param name="r">The string.</param>
        /// <returns>A new StyledText with the concatenated strings of the two arguments.</returns>
        public static StyledText operator +(StyledText l, string r)
        {
            return new StyledText(l.Text + r, l.Color);
        }
    }
}