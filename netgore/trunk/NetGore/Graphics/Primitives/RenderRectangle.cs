using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Assists in drawing a rectangle.
    /// </summary>
    public static class RenderRectangle
    {
        static readonly RectangleShape _drawRectNoBorder = new RectangleShape();
        static readonly RectangleShape _drawRectBorder = new RectangleShape();

        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="dest">Destination rectangle.</param>
        /// <param name="color">Color of the box.</param>
        /// <param name="borderColor">Color of the border to draw around the rectangle.</param>
        /// <param name="borderThickness">The thickness of the border in pixels. Default is 1.</param>
        public static void Draw(ISpriteBatch sb, Rectangle dest, Color color, Color borderColor, float borderThickness = 1f)
        {
            Draw(sb, new Vector2(dest.X, dest.Y), new Vector2(dest.Width, dest.Height), color, borderColor, borderThickness);
        }

        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="position">Top-left corner position.</param>
        /// <param name="size">The size of the rectangle.</param>
        /// <param name="color">Color of the box.</param>
        /// <param name="borderColor">Color of the border to draw around the rectangle.</param>
        /// <param name="borderThickness">The thickness of the border in pixels. Default is 1.</param>
        public static void Draw(ISpriteBatch sb, Vector2 position, Vector2 size, Color color, Color borderColor, float borderThickness = 1f)
        {
            var drawRect = _drawRectNoBorder;

            drawRect.Size = size;
            drawRect.Position = position;
            drawRect.FillColor = color;
            drawRect.OutlineColor = borderColor;
            drawRect.OutlineThickness = borderThickness;

            sb.Draw(drawRect);
        }

        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="dest">Destination rectangle.</param>
        /// <param name="color">Color of the rectangle.</param>
        public static void Draw(ISpriteBatch sb, Rectangle dest, Color color)
        {
            Draw(sb, new Vector2(dest.X, dest.Y), new Vector2(dest.Width, dest.Height), color);
        }

        /// <summary>
        /// Draws a rectangle.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="position">Top-left corner position.</param>
        /// <param name="size">The size of the rectangle.</param>
        /// <param name="color">Color of the rectangle.</param>
        public static void Draw(ISpriteBatch sb, Vector2 position, Vector2 size, Color color)
        {
            var drawRect = _drawRectBorder;

            drawRect.Size = size;
            drawRect.Position = position;
            drawRect.FillColor = color;

            sb.Draw(drawRect);
        }
    }
}