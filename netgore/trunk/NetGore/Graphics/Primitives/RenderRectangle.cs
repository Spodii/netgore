using System;
using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Assists in drawing a rectangle.
    /// </summary>
    public static class RenderRectangle
    {
        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="dest">Destination rectangle.</param>
        /// <param name="color">Color of the box.</param>
        /// <param name="borderColor">Color of the border to draw around the rectangle.</param>
        public static void Draw(ISpriteBatch sb, Rectangle dest, Color color, Color borderColor)
        {
            Draw(sb, dest, color, borderColor, 1f);
        }

        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="dest">Destination rectangle.</param>
        /// <param name="color">Color of the box.</param>
        /// <param name="borderColor">Color of the border to draw around the rectangle.</param>
        /// <param name="borderThickness">The thickness of the border in pixels. Default is 1.</param>
        public static void Draw(ISpriteBatch sb, Rectangle dest, Color color, Color borderColor, float borderThickness)
        {
            using (var s = Shape.Rectangle(new Vector2(dest.X, dest.Y), new Vector2(dest.Right, dest.Bottom), color, borderThickness, borderColor))
            {
                sb.Draw(s);
            }
        }

        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="dest">Destination rectangle.</param>
        /// <param name="color">Color of the rectangle.</param>
        public static void Draw(ISpriteBatch sb, Rectangle dest, Color color)
        {
            using (var s = Shape.Rectangle(new Vector2(dest.X, dest.Y), new Vector2(dest.Right, dest.Bottom), color))
            {
                sb.Draw(s);
            }
        }
    }
}