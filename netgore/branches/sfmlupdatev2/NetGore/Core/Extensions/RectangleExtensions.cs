using System.Linq;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="Rectangle"/> struct.
    /// </summary>
    public static class RectangleExtensions
    {
        /// <summary>
        /// Determines whether this <see cref="Rectangle"/> contains a specified point.
        /// </summary>
        /// <param name="r">The <see cref="Rectangle"/>.</param>
        /// <param name="point">The point.</param>
        /// <returns>True if the <see cref="Rectangle"/> contains the <paramref name="point"/>; otherwise false.</returns>
        public static bool Contains(this Rectangle r, Vector2 point)
        {
            return r.Contains((int)point.X, (int)point.Y);
        }

        /// <summary>
        /// Gets the distance between two <see cref="Rectangle"/>s.
        /// </summary>
        /// <param name="a">The first <see cref="Rectangle"/>.</param>
        /// <param name="b">The second <see cref="Rectangle"/>.</param>
        /// <returns>The distance between <see cref="Rectangle"/> <paramref name="a"/> and <paramref name="b"/> as an
        /// absolute value greater than or equal to zero. If <paramref name="a"/> intersects <paramref name="b"/>, the
        /// value will be 0. Otherwise, the value will be the length of the shortest path between the two
        /// <see cref="Rectangle"/>s.</returns>
        public static int GetDistance(this Rectangle a, Rectangle b)
        {
            Alignment alignment;

            if (a.Right < b.X)
            {
                // a is to the left of b
                if (a.Bottom < b.Y)
                    alignment = Alignment.TopLeft;
                else if (a.Y > b.Y + b.Height)
                    alignment = Alignment.BottomLeft;
                else
                    alignment = Alignment.Left;
            }
            else if (a.X > b.Right)
            {
                // a is to the right of b
                if (a.Bottom < b.Y)
                    alignment = Alignment.TopRight;
                else if (a.Y > b.Y + b.Height)
                    alignment = Alignment.BottomRight;
                else
                    alignment = Alignment.Right;
            }
            else if (a.Bottom < b.Y)
                alignment = Alignment.Top;
            else if (a.Y > b.Y + b.Height)
                alignment = Alignment.Bottom;
            else
                alignment = Alignment.Center;

            int cx;
            int cy;

            switch (alignment)
            {
                case Alignment.TopLeft:
                    cx = a.Right - b.X;
                    cy = a.Bottom - b.Y;
                    return -(cx + cy);

                case Alignment.Top:
                    return -(a.Bottom - b.Y);

                case Alignment.TopRight:
                    cx = b.Right - a.X;
                    cy = a.Bottom - b.Y;
                    return -(cx + cy);

                case Alignment.Right:
                    return -(b.Bottom - a.X);

                case Alignment.BottomRight:
                    cx = b.Right - a.X;
                    cy = b.Bottom - a.Y;
                    return -(cx + cy);

                case Alignment.Bottom:
                    return -(b.Bottom - a.Y);

                case Alignment.BottomLeft:
                    cx = a.Right - b.X;
                    cy = b.Bottom - a.Y;
                    return -(cx + cy);

                case Alignment.Left:
                    return -(a.Right - b.X);

                default:
                    return 0;
            }
        }
    }
}