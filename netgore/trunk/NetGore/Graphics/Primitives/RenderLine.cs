using System.Diagnostics;
using System.Linq;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Draws a line
    /// </summary>
    public static class RenderLine
    {
        /// <summary>
        /// Draws a line.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> to draw to.</param>
        /// <param name="p1">First point of the line.</param>
        /// <param name="p2">Second point of the line.</param>
        /// <param name="color">Color of the line.</param>
        /// <param name="thickness">The thickness of the line in pixels. Default is 1.</param>
        public static void Draw(ISpriteBatch sb, Vector2 p1, Vector2 p2, Color color, float thickness = 1f)
        {
            if (sb == null)
            {
                Debug.Fail("sb is null.");
                return;
            }

            if (sb.IsDisposed)
            {
                Debug.Fail("sb is disposed.");
                return;
            }

            using (var s = new ConvexShape(4))
            {
                s.SetPoint(0, p1);
                s.SetPoint(1, p2);
                s.SetPoint(2, p2 + new Vector2(1, 0));
                s.SetPoint(3, p1 + new Vector2(1, 0));
                s.FillColor = color;
                s.OutlineColor = color;
                s.OutlineThickness = thickness;

                sb.Draw(s);
            }
        }
    }
}