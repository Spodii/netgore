using System;
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
        /// Rectangle used for drawing our lines.
        /// </summary>
        static readonly RectangleShape _drawLineRect = new RectangleShape();

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

            // Common properties set no matter the approach we use below
            _drawLineRect.Position = p1;
            _drawLineRect.FillColor = color;

            // If we have a perfectly vertical or horizontal line, we can avoid using rotation to speed up the calculation, so check for that first.
            // This is purely an optimization - the rotation approach works for all points still.
            if (p1.X == p2.X)
            {
                // Perfectly vertical
                _drawLineRect.Size = new Vector2(thickness, p2.Y - p1.Y);
                _drawLineRect.Rotation = 0;
            }
            else if (p1.Y == p2.Y)
            {
                // Perfectly horizontal
                _drawLineRect.Size = new Vector2(p2.X - p1.X, thickness);
                _drawLineRect.Rotation = 0;
            }
            else
            {
                // Treat as horizontal by setting the X as the distance and Y as the thickness, then rotate
                _drawLineRect.Size = new Vector2(p2.Distance(p1), thickness);
                _drawLineRect.Rotation = MathHelper.ToDegrees((float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X));
            }

            sb.Draw(_drawLineRect);
        }
    }
}