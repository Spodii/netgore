using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// <summary>
        /// Color of the text.
        /// </summary>
        public readonly Color Color;

        /// <summary>
        /// String of text.
        /// </summary>
        public readonly string Text;

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

        public StyledText(string text, Color color)
        {
            Text = text;
            Color = color;
        }
    }
}