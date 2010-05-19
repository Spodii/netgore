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
        /// <param name="borderThickness">The thickness of the border in pixels. Default is 1.</param>
        public static void Draw(ISpriteBatch sb, Rectangle dest, Color color, Color borderColor, float borderThickness = 1f)
        {
            var fDest = new FloatRect(dest.Left, dest.Top, dest.Width, dest.Height);

            using (
                var s = Shape.Rectangle(fDest, color, borderThickness, borderColor))
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
            var fDest = new FloatRect(dest.Left, dest.Top, dest.Width, dest.Height);

            using (var s = Shape.Rectangle(fDest, color))
            {
                sb.Draw(s);
            }
        }
    }
}