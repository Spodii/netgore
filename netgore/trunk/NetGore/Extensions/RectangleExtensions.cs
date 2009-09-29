using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="Rectangle"/> struct.
    /// </summary>
    public static class RectangleExtensions
    {
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

            if (a.X + a.Width < b.X)
            {
                // a is to the left of b
                if (a.Y + a.Height < b.Y)
                    alignment = Alignment.TopLeft;
                else if (a.Y > b.Y + b.Height)
                    alignment = Alignment.BottomLeft;
                else
                    alignment = Alignment.Left;
            }
            else if (a.X > b.X + b.Width)
            {
                // a is to the right of b
                if (a.Y + a.Height < b.Y)
                    alignment = Alignment.TopRight;
                else if (a.Y > b.Y + b.Height)
                    alignment = Alignment.BottomRight;
                else
                    alignment = Alignment.Right;
            }
            else if (a.Y + a.Height < b.Y)
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
                    cx = (a.X + a.Width) - b.X;
                    cy = (a.Y + a.Height) - b.Y;
                    return -(cx + cy);

                case Alignment.Top:
                    return -((a.Y + a.Height) - b.Y);

                case Alignment.TopRight:
                    cx = (b.X + b.Width) - a.X;
                    cy = (a.Y + a.Height) - b.Y;
                    return -(cx + cy);

                case Alignment.Right:
                    return -((b.X + b.Width) - a.X);

                case Alignment.BottomRight:
                    cx = (b.X + b.Width) - a.X;
                    cy = (b.Y + b.Height) - a.Y;
                    return -(cx + cy);

                case Alignment.Bottom:
                    return -((b.Y + b.Height) - a.Y);

                case Alignment.BottomLeft:
                    cx = (a.X + a.Width) - b.X;
                    cy = (b.Y + b.Height) - a.Y;
                    return -(cx + cy);

                case Alignment.Left:
                    return -((a.X + a.Width) - b.X);

                default:
                    return 0;
            }
        }
    }
}