using System.Linq;
using NetGore.Extensions;
using SFML.Graphics;

namespace NetGore.World
{
    /// <summary>
    /// Extension methods for the <see cref="ISpatial"/> interface.
    /// </summary>
    public static class ISpatialExtensions
    {
        /// <summary>
        /// Gets if an <see cref="ISpatial"/> is fully contained inside of another <see cref="ISpatial"/>. That is, if
        /// the <see cref="contained"/> resides completely inside of the <see cref="container"/>.
        /// </summary>
        /// <param name="container">The <see cref="ISpatial"/> to check if it contains the <see cref="contained"/>.</param>
        /// <param name="contained">The <see cref="ISpatial"/> to check if is contained.</param>
        /// <returns>If the <paramref name="contained"/> is fully contained inside of the
        /// <paramref name="container"/>.</returns>
        public static bool Contains(this ISpatial container, ISpatial contained)
        {
            return container.ToRectangle().Contains(contained.ToRectangle());
        }

        /// <summary>
        /// Gets if a <see cref="Rectangle"/> is fully contained inside of a <see cref="ISpatial"/>. That is, if
        /// the <see cref="contained"/> resides completely inside of the <see cref="container"/>.
        /// </summary>
        /// <param name="container">The <see cref="ISpatial"/> to check if it contains the <see cref="contained"/>.</param>
        /// <param name="contained">The <see cref="Rectangle"/> to check if is contained.</param>
        /// <returns>If the <paramref name="contained"/> is fully contained inside of the
        /// <paramref name="container"/>.</returns>
        public static bool Contains(this ISpatial container, Rectangle contained)
        {
            return container.ToRectangle().Contains(contained);
        }

        /// <summary>
        /// Checks if the <see cref="ISpatial"/> contains a point.
        /// </summary>
        /// <param name="spatial">The spatial.</param>
        /// <param name="p">Point to check if the <paramref name="spatial"/> contains.</param>
        /// <returns>True if the <paramref name="spatial"/> contains <paramref name="p"/>; otherwise false.</returns>
        public static bool Contains(this ISpatial spatial, Vector2 p)
        {
            var min = spatial.Position;
            var max = spatial.Max;

            return (min.X <= p.X && max.X >= p.X && min.Y <= p.Y && max.Y >= p.Y);
        }

        /// <summary>
        /// Gets the distance between this <see cref="ISpatial"/> and another.
        /// </summary>
        /// <param name="spatial">The first <see cref="ISpatial"/>.</param>
        /// <param name="other">The other <see cref="ISpatial"/>.</param>
        /// <returns>The distance between this <see cref="ISpatial"/> and another <see cref="ISpatial"/> as an
        /// absolute value greater than or equal to zero. If this intersects <paramref name="other"/>, the
        /// value will be 0. Otherwise, the value will be the length of the shortest path between the two
        /// <see cref="ISpatial"/>s.</returns>
        public static int GetDistance(this ISpatial spatial, ISpatial other)
        {
            return spatial.ToRectangle().GetDistance(other.ToRectangle());
        }

        /// <summary>
        /// Gets the distance between this <see cref="ISpatial"/> and a <see cref="Rectangle"/>.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/>.</param>
        /// <param name="other">The rectangle.</param>
        /// <returns>The distance between this <see cref="ISpatial"/> and the <see cref="Rectangle"/> as an
        /// absolute value greater than or equal to zero. If this intersects <paramref name="other"/>, the
        /// value will be 0. Otherwise, the value will be the length of the shortest path between the two
        /// <see cref="ISpatial"/>s.</returns>
        public static int GetDistance(this ISpatial spatial, Rectangle other)
        {
            return spatial.ToRectangle().GetDistance(other);
        }

        /// <summary>
        /// Gets a <see cref="Rectangle"/> that represents the area that represents where this <see cref="ISpatial"/>
        /// is standing.
        /// </summary>
        /// <param name="spatial">The <see cref="ISpatial"/>.</param>
        /// <returns>A <see cref="Rectangle"/> that represents the area that represents where this <see cref="ISpatial"/>
        /// is standing.</returns>
        public static Rectangle GetStandingAreaRect(this ISpatial spatial)
        {
            Vector2 min = spatial.Position;
            Vector2 size = spatial.Size;
            Rectangle r = new Rectangle(min.X + 1, min.Y + size.Y - 1, size.X - 2, 2);
            return r;
        }

        /// <summary>
        /// Gets if an <see cref="ISpatial"/> and <see cref="Rectangle"/> occupy any common space.
        /// </summary>
        /// <param name="a">The <see cref="ISpatial"/>.</param>
        /// <param name="b">The <see cref="Rectangle"/>.</param>
        /// <returns>True if the two occupy any common space; otherwise false.</returns>
        public static bool Intersects(this ISpatial a, Rectangle b)
        {
            bool ret;
            a.ToRectangle().Intersects(ref b, out ret);
            return ret;
        }

        /// <summary>
        /// Gets if an <see cref="ISpatial"/> and another <see cref="ISpatial"/> occupy any common space.
        /// </summary>
        /// <param name="a">The first <see cref="ISpatial"/>.</param>
        /// <param name="b">The other <see cref="ISpatial"/>.</param>
        /// <returns>True if the two occupy any common space; otherwise false.</returns>
        public static bool Intersects(this ISpatial a, ISpatial b)
        {
            var bRect = b.ToRectangle();
            bool ret;
            a.ToRectangle().Intersects(ref bRect, out ret);
            return ret;
        }
    }
}