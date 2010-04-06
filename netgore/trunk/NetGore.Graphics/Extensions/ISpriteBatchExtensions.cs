using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Extensions for the <see cref="ISpriteBatch"/> interface.
    /// </summary>
    public static class ISpriteBatchExtensions
    {
        /// <summary>
        /// Draws a string with shading.
        /// </summary>
        /// <param name="spriteBatch">ISpriteBatch to draw to.</param>
        /// <param name="spriteFont">SpriteFont to draw the string with.</param>
        /// <param name="text">The string to draw.</param>
        /// <param name="position">The position of the top-left corner of the string to draw.</param>
        /// <param name="fontColor">The font color.</param>
        /// <param name="borderColor">The shading color.</param>
        public static void DrawStringShaded(this ISpriteBatch spriteBatch, Font spriteFont, string text, Vector2 position,
                                            Color fontColor, Color borderColor)
        {
            position = position.Round();

            spriteBatch.DrawString(spriteFont, text, position - new Vector2(0, 1), borderColor);
            spriteBatch.DrawString(spriteFont, text, position - new Vector2(1, 0), borderColor);
            spriteBatch.DrawString(spriteFont, text, position + new Vector2(0, 1), borderColor);
            spriteBatch.DrawString(spriteFont, text, position + new Vector2(1, 0), borderColor);

            spriteBatch.DrawString(spriteFont, text, position, fontColor);
        }
    }
}