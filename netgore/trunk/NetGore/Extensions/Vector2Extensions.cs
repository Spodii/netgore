using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore
{
    /// <summary>
    /// Extension methods for the <see cref="Vector2"/>.
    /// </summary>
    public static class Vector2Extensions
    {
        /// <summary>
        /// Gets the absolute value of the components of the <see cref="Vector2"/>.
        /// </summary>
        /// <param name="source"><see cref="Vector2"/> to get the absolute value of.</param>
        /// <returns>A <see cref="Vector2"/> containing the absolute value of the <paramref name="source"/>
        /// <see cref="Vector2"/>.</returns>
        public static Vector2 Abs(this Vector2 source)
        {
            return new Vector2(Math.Abs(source.X), Math.Abs(source.Y));
        }

        /// <summary>
        /// Checks if both of a <see cref="Vector2"/>'s coordinates are greater than or equal to the
        /// corresponding coordinates in another <see cref="Vector2"/>.
        /// </summary>
        /// <param name="source"><see cref="Vector2"/> to check.</param>
        /// <param name="other"><see cref="Vector2"/> containing the coordinates to check if are greater than or equal to the 
        /// <paramref name="source"/>'s coordinates.</param>
        /// <returns>True if both of the coordinates in the <paramref name="source"/> are greater than or equal to the
        /// corresponding coordinates in the <paramref name="other"/> <see cref="Vector2"/>. False if either or both
        /// coordinates are less than their equivalent in the <paramref name="other"/> <see cref="Vector2"/>.</returns>
        public static bool IsGreaterOrEqual(this Vector2 source, Vector2 other)
        {
            return source.X >= other.X && source.Y >= other.Y;
        }

        /// <summary>
        /// Checks if both of a <see cref="Vector2"/>'s coordinates are greater than the corresponding coordinates in another
        /// <see cref="Vector2"/>.
        /// </summary>
        /// <param name="source"><see cref="Vector2"/> to check.</param>
        /// <param name="other"><see cref="Vector2"/> containing the coordinates to check if are greater than the 
        /// <paramref name="source"/>'s coordinates.</param>
        /// <returns>True if both of the coordinates in the <paramref name="source"/> are greater than the
        /// corresponding coordinates in the <paramref name="other"/> <see cref="Vector2"/>. False if either or both
        /// coordinates are less than or equal to their equivalent in the <paramref name="other"/>
        /// <see cref="Vector2"/>.</returns>
        public static bool IsGreaterThan(this Vector2 source, Vector2 other)
        {
            return source.X > other.X && source.Y > other.Y;
        }

        /// <summary>
        /// Checks if both of a <see cref="Vector2"/>'s coordinates are less than or equal to the
        /// corresponding coordinates in another Vector2.
        /// </summary>
        /// <param name="source"><see cref="Vector2"/> to check.</param>
        /// <param name="other"><see cref="Vector2"/> containing the coordinates to check if are less than or equal to the 
        /// <paramref name="source"/>'s coordinates.</param>
        /// <returns>True if both of the coordinates in the <paramref name="source"/> are less than or equal to the
        /// corresponding coordinates in the <paramref name="other"/> <see cref="Vector2"/>. False if either or both
        /// coordinates are greater than their equivalent in the <paramref name="other"/> <see cref="Vector2"/>.</returns>
        public static bool IsLessOrEqual(this Vector2 source, Vector2 other)
        {
            return source.X <= other.X && source.Y <= other.Y;
        }

        /// <summary>
        /// Checks if both of a <see cref="Vector2"/>'s coordinates are less than the corresponding coordinates
        /// in another <see cref="Vector2"/>.
        /// </summary>
        /// <param name="source"><see cref="Vector2"/> to check.</param>
        /// <param name="other"><see cref="Vector2"/> containing the coordinates to check if are less than the 
        /// <paramref name="source"/>'s coordinates.</param>
        /// <returns>True if both of the coordinates in the <paramref name="source"/> are less than the
        /// corresponding coordinates in the <paramref name="other"/> <see cref="Vector2"/>. False if either or both
        /// coordinates are greater than or equal to their equivalent in the <paramref name="other"/>
        /// <see cref="Vector2"/>.</returns>
        public static bool IsLessThan(this Vector2 source, Vector2 other)
        {
            return source.X < other.X && source.Y < other.Y;
        }

        /// <summary>
        /// Gets a <see cref="Vector2"/> composed of the max value of the given components for the respective property.
        /// </summary>
        /// <param name="a">The first argument.</param>
        /// <param name="b">The second argument.</param>
        /// <returns>A <see cref="Vector2"/> composed of the max value of the given components for the respective
        /// property.</returns>
        public static Vector2 Max(this Vector2 a, Vector2 b)
        {
            return new Vector2(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));
        }

        /// <summary>
        /// Gets a <see cref="Vector2"/> composed of the min value of the given components for the respective property.
        /// </summary>
        /// <param name="a">The first argument.</param>
        /// <param name="b">The second argument.</param>
        /// <returns>A <see cref="Vector2"/> composed of the max value of the given components for the respective
        /// property.</returns>
        public static Vector2 Min(this Vector2 a, Vector2 b)
        {
            return new Vector2(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y));
        }

        /// <summary>
        /// Gets the distance between this <see cref="Vector2"/> and another <see cref="Vector2"/> by getting the
        /// sum of the differences for the components of the <see cref="Vector2"/>s.
        /// </summary>
        /// <param name="source">Source <see cref="Vector2"/>.</param>
        /// <param name="target">Target <see cref="Vector2"/>.</param>
        /// <returns>The distance between this <see cref="Vector2"/> and another <see cref="Vector2"/> by getting the
        /// sum of the differences for the components of the <see cref="Vector2"/>s.</returns>
        public static float QuickDistance(this Vector2 source, Vector2 target)
        {
            Vector2 diff = source - target;
            return Abs(diff).Sum();
        }

        /// <summary>
        /// Gets a <see cref="Vector2"/> with the X and Y components rounded.
        /// </summary>
        /// <param name="source"><see cref="Vector2"/> to round.</param>
        /// <returns>A <see cref="Vector2"/> containing the rounded value of the <paramref name="source"/>
        /// <see cref="Vector2"/>.</returns>
        public static Vector2 Round(this Vector2 source)
        {
            return new Vector2((float)Math.Round(source.X), (float)Math.Round(source.Y));
        }

        /// <summary>
        /// Gets a <see cref="Vector2"/> with the X and Y components rounded down.
        /// </summary>
        /// <param name="source"><see cref="Vector2"/> to round.</param>
        /// <returns>A <see cref="Vector2"/> containing the rounded-down value of the <paramref name="source"/>
        /// <see cref="Vector2"/>.</returns>
        public static Vector2 Floor(this Vector2 source)
        {
            return new Vector2((float)Math.Floor(source.X), (float)Math.Floor(source.Y));
        }

        /// <summary>
        /// Gets a <see cref="Vector2"/> with the X and Y components rounded up.
        /// </summary>
        /// <param name="source"><see cref="Vector2"/> to round.</param>
        /// <returns>A <see cref="Vector2"/> containing the ceiling-down value of the <paramref name="source"/>
        /// <see cref="Vector2"/>.</returns>
        public static Vector2 Ceiling(this Vector2 source)
        {
            return new Vector2((float)Math.Ceiling(source.X), (float)Math.Ceiling(source.Y));
        }

        /// <summary>
        /// Gets the sum of the x and y components of the <see cref="Vector2"/>.
        /// </summary>
        /// <param name="source"><see cref="Vector2"/> to get the sum of the components of.</param>
        /// <returns>Sum of the x and y components of the <see cref="Vector2"/>.</returns>
        public static float Sum(this Vector2 source)
        {
            return source.X + source.Y;
        }
    }
}