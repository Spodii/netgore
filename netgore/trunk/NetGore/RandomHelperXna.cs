using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore
{
    /// <summary>
    /// Provides helper methods for acquiring random values from types defined by Xna.
    /// </summary>
    public static class RandomHelperXna
    {
        /// <summary>
        /// Returns a random <see cref="Vector2"/> with both coordinates between 0.0 and 1.0.
        /// </summary>
        /// <returns>A random <see cref="Vector2"/> with both coordinates between 0.0 and 1.0.</returns>
        public static Vector2 NextVector2()
        {
            var x = RandomHelper.NextFloat();
            var y = RandomHelper.NextFloat();
            return new Vector2(x, y);
        }

        /// <summary>
        /// Returns a random Vector2 with both coordinates betwen 0.0 and the specified <paramref name="maximum"/>.
        /// </summary>
        /// <param name="maximum">The inclusive upper bound of the random number returned.</param>
        /// <returns>A random <see cref="Vector2"/> with both coordinates betwen 0.0 and the specified
        /// <paramref name="maximum"/>.</returns>
        public static Vector2 NextVector2(float maximum)
        {
            var x = RandomHelper.NextFloat(maximum);
            var y = RandomHelper.NextFloat(maximum);
            return new Vector2(x, y);
        }

        /// <summary>
        /// Returns a random <see cref="Vector2"/> with both coordinates within the specified range.
        /// </summary>
        /// <param name="min">The inclusive lower bound of the random number returned.</param>
        /// <param name="max">The inclusive upper bound of the random number returned.</param>
        /// <returns>A random <see cref="Vector2"/> with both coordinates within the specified range.</returns>
        public static Vector2 NextVector2(float min, float max)
        {
            var x = RandomHelper.NextFloat(min, max);
            var y = RandomHelper.NextFloat(min, max);
            return new Vector2(x, y);
        }

        /// <summary>
        /// Returns a random <see cref="Vector2"/> with both coordinates within the specified range.
        /// </summary>
        /// <param name="min">The inclusive lower bound of the random number returned.</param>
        /// <param name="max">The inclusive upper bound of the random number returned.</param>
        /// <returns>A random <see cref="Vector2"/> with both coordinates within the specified range.</returns>
        public static Vector2 NextVector2(Vector2 min, Vector2 max)
        {
            return NextVector2(min.X, max.X, min.Y, max.Y);
        }

        /// <summary>
        /// Returns a random <see cref="Vector2"/> with both coordinates within the specified range.
        /// </summary>
        /// <param name="minX">The inclusive lower bound of the x coordinate.</param>
        /// <param name="maxX">The inclusive upper bound of the x coordinate.</param>
        /// <param name="minY">The inclusive lower bound of the y coordinate.</param>
        /// <param name="maxY">The inclusive upper bound of the y coordinate.</param>
        /// <returns>A random <see cref="Vector2"/> with both coordinates within the specified range.</returns>
        public static Vector2 NextVector2(float minX, float maxX, float minY, float maxY)
        {
            var x = RandomHelper.NextFloat(minX, maxX);
            var y = RandomHelper.NextFloat(minY, maxY);
            return new Vector2(x, y);
        }
    }
}