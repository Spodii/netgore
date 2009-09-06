using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// Describes a string of text and the style it uses.
    /// </summary>
    public class StyledText
    {
        readonly Color _color;
        readonly string _text;

        /// <summary>
        /// Gets the color of the text.
        /// </summary>
        public Color Color { get { return _color; } }

        /// <summary>
        /// Gets the text.
        /// </summary>
        public string Text { get { return _text; } }

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
        /// Draws the StyledText to the <paramref name="spriteBatch"/>.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw the StyledText to.</param>
        /// <param name="spriteFont">SpriteFont to use for drawing the characters.</param>
        /// <param name="position">Top-left corner of where to begin drawing the StyledText.</param>
        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont, Vector2 position)
        {
            spriteBatch.DrawString(spriteFont, Text, position, Color);
        }
    }
}