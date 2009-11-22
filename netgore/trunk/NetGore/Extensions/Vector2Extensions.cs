using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore
{
    /// <summary>
    /// Extensions for the Microsoft.Xna.Framework.Vector2.
    /// </summary>
    public static class Vector2Extensions
    {
        /// <summary>
        /// Gets the absolute value of the X and Y components of the Vector2.
        /// </summary>
        /// <param name="source">Vector2 to get the absolute value of.</param>
        /// <returns>A Vector2 containing the absolute value of the <paramref name="source"/> Vector2.</returns>
        public static Vector2 Abs(this Vector2 source)
        {
            return new Vector2(Math.Abs(source.X), Math.Abs(source.Y));
        }

        /// <summary>
        /// Checks if both of a Vector2's coordinates are greater than or equal to the
        /// corresponding coordinates in another Vector2.
        /// </summary>
        /// <param name="source">Vector2 to check.</param>
        /// <param name="other">Vector2 containing the coordinates to check if are greater than or equal to the 
        /// <paramref name="source"/>'s coordinates.</param>
        /// <returns>True if both of the coordinates in the <paramref name="source"/> are greater than or equal to the
        /// corresponding coordinates in the <paramref name="other"/> Vector2. False if either or both
        /// coordinates are less than their equivalent in the <paramref name="other"/> Vector2.</returns>
        public static bool IsGreaterOrEqual(this Vector2 source, Vector2 other)
        {
            return source.X >= other.X && source.Y >= other.Y;
        }

        /// <summary>
        /// Checks if both of a Vector2's coordinates are greater than the corresponding coordinates in another Vector2.
        /// </summary>
        /// <param name="source">Vector2 to check.</param>
        /// <param name="other">Vector2 containing the coordinates to check if are greater than the 
        /// <paramref name="source"/>'s coordinates.</param>
        /// <returns>True if both of the coordinates in the <paramref name="source"/> are greater than the
        /// corresponding coordinates in the <paramref name="other"/> Vector2. False if either or both
        /// coordinates are less than or equal to their equivalent in the <paramref name="other"/> Vector2.</returns>
        public static bool IsGreaterThan(this Vector2 source, Vector2 other)
        {
            return source.X > other.X && source.Y > other.Y;
        }

        /// <summary>
        /// Checks if both of a Vector2's coordinates are less than or equal to the
        /// corresponding coordinates in another Vector2.
        /// </summary>
        /// <param name="source">Vector2 to check.</param>
        /// <param name="other">Vector2 containing the coordinates to check if are less than or equal to the 
        /// <paramref name="source"/>'s coordinates.</param>
        /// <returns>True if both of the coordinates in the <paramref name="source"/> are less than or equal to the
        /// corresponding coordinates in the <paramref name="other"/> Vector2. False if either or both
        /// coordinates are greater than their equivalent in the <paramref name="other"/> Vector2.</returns>
        public static bool IsLessOrEqual(this Vector2 source, Vector2 other)
        {
            return source.X <= other.X && source.Y <= other.Y;
        }

        /// <summary>
        /// Checks if both of a Vector2's coordinates are less than the corresponding coordinates in another Vector2.
        /// </summary>
        /// <param name="source">Vector2 to check.</param>
        /// <param name="other">Vector2 containing the coordinates to check if are less than the 
        /// <paramref name="source"/>'s coordinates.</param>
        /// <returns>True if both of the coordinates in the <paramref name="source"/> are less than the
        /// corresponding coordinates in the <paramref name="other"/> Vector2. False if either or both
        /// coordinates are greater than or equal to their equivalent in the <paramref name="other"/> Vector2.</returns>
        public static bool IsLessThan(this Vector2 source, Vector2 other)
        {
            return source.X < other.X && source.Y < other.Y;
        }

        public static Vector2 Max(this Vector2 a, Vector2 b)
        {
            return new Vector2(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y));
        }

        public static Vector2 Min(this Vector2 a, Vector2 b)
        {
            return new Vector2(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y));
        }

        /// <summary>
        /// Gets the distance between this Vector2 and another Vector2 by getting the sum of the differences
        /// for both the x and y components of the Vector2s.
        /// </summary>
        /// <param name="source">Source Vector2.</param>
        /// <param name="target">Target Vector2.</param>
        /// <returns>The distance between this Vector2 and another Vector2 by getting the sum of the differences
        /// for both the x and y components of the Vector2s.</returns>
        public static float QuickDistance(this Vector2 source, Vector2 target)
        {
            Vector2 diff = source - target;
            return Abs(diff).Sum();
        }

        /// <summary>
        /// Gets a Vector2 with the X and Y components rounded.
        /// </summary>
        /// <param name="source">Vector2 to round.</param>
        /// <returns>A Vector2 containing the rounded value of the <paramref name="source"/> Vector2.</returns>
        public static Vector2 Round(this Vector2 source)
        {
            return new Vector2((float)Math.Round(source.X), (float)Math.Round(source.Y));
        }

        /// <summary>
        /// Gets the sum of the x and y components of the Vector2.
        /// </summary>
        /// <param name="source">Vector2 to get the sum of the components of.</param>
        /// <returns>Sum of the x and y components of the Vector2.</returns>
        public static float Sum(this Vector2 source)
        {
            return source.X + source.Y;
        }
    }
}