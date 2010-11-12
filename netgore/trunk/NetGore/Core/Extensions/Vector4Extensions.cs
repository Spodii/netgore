using System;
using System.Linq;
using SFML.Graphics;

namespace NetGore
{
    /// <summary>
    /// Extension methods for the <see cref="Vector4"/>.
    /// </summary>
    public static class Vector4Extensions
    {
        /// <summary>
        /// Gets the absolute value of the components of the <see cref="Vector4"/>.
        /// </summary>
        /// <param name="source"><see cref="Vector4"/> to get the absolute value of.</param>
        /// <returns>A <see cref="Vector4"/> containing the absolute value of the <paramref name="source"/>
        /// <see cref="Vector4"/>.</returns>
        public static Vector4 Abs(this Vector4 source)
        {
            return new Vector4(Math.Abs(source.X), Math.Abs(source.Y), Math.Abs(source.Z), Math.Abs(source.W));
        }

        /// <summary>
        /// Gets a <see cref="Vector4"/> with the X and Y components rounded up.
        /// </summary>
        /// <param name="source"><see cref="Vector4"/> to round.</param>
        /// <returns>A <see cref="Vector4"/> containing the ceiling-down value of the <paramref name="source"/>
        /// <see cref="Vector4"/>.</returns>
        public static Vector4 Ceiling(this Vector4 source)
        {
            return new Vector4((float)Math.Ceiling(source.X), (float)Math.Ceiling(source.Y), (float)Math.Ceiling(source.Z),
                (float)Math.Ceiling(source.W));
        }

        /// <summary>
        /// Gets a <see cref="Vector4"/> with the X and Y components rounded down.
        /// </summary>
        /// <param name="source"><see cref="Vector4"/> to round.</param>
        /// <returns>A <see cref="Vector4"/> containing the rounded-down value of the <paramref name="source"/>
        /// <see cref="Vector4"/>.</returns>
        public static Vector4 Floor(this Vector4 source)
        {
            return new Vector4((float)Math.Floor(source.X), (float)Math.Floor(source.Y), (float)Math.Floor(source.Z),
                (float)Math.Floor(source.W));
        }

        /// <summary>
        /// Checks if both of a <see cref="Vector4"/>'s coordinates are greater than or equal to the
        /// corresponding coordinates in another <see cref="Vector4"/>.
        /// </summary>
        /// <param name="source"><see cref="Vector4"/> to check.</param>
        /// <param name="other"><see cref="Vector4"/> containing the coordinates to check if are greater than or equal to the 
        /// <paramref name="source"/>'s coordinates.</param>
        /// <returns>True if both of the coordinates in the <paramref name="source"/> are greater than or equal to the
        /// corresponding coordinates in the <paramref name="other"/> <see cref="Vector4"/>. False if either or both
        /// coordinates are less than their equivalent in the <paramref name="other"/> <see cref="Vector4"/>.</returns>
        public static bool IsGreaterOrEqual(this Vector4 source, Vector4 other)
        {
            return source.X >= other.X && source.Y >= other.Y && source.Z >= other.Z && source.W >= other.W;
        }

        /// <summary>
        /// Checks if both of a <see cref="Vector4"/>'s coordinates are greater than the corresponding coordinates in another
        /// <see cref="Vector4"/>.
        /// </summary>
        /// <param name="source"><see cref="Vector4"/> to check.</param>
        /// <param name="other"><see cref="Vector4"/> containing the coordinates to check if are greater than the 
        /// <paramref name="source"/>'s coordinates.</param>
        /// <returns>True if both of the coordinates in the <paramref name="source"/> are greater than the
        /// corresponding coordinates in the <paramref name="other"/> <see cref="Vector4"/>. False if either or both
        /// coordinates are less than or equal to their equivalent in the <paramref name="other"/>
        /// <see cref="Vector4"/>.</returns>
        public static bool IsGreaterThan(this Vector4 source, Vector4 other)
        {
            return source.X > other.X && source.Y > other.Y && source.Z > other.Z && source.W > other.W;
        }

        /// <summary>
        /// Checks if both of a <see cref="Vector4"/>'s coordinates are less than or equal to the
        /// corresponding coordinates in another Vector4.
        /// </summary>
        /// <param name="source"><see cref="Vector4"/> to check.</param>
        /// <param name="other"><see cref="Vector4"/> containing the coordinates to check if are less than or equal to the 
        /// <paramref name="source"/>'s coordinates.</param>
        /// <returns>True if both of the coordinates in the <paramref name="source"/> are less than or equal to the
        /// corresponding coordinates in the <paramref name="other"/> <see cref="Vector4"/>. False if either or both
        /// coordinates are greater than their equivalent in the <paramref name="other"/> <see cref="Vector4"/>.</returns>
        public static bool IsLessOrEqual(this Vector4 source, Vector4 other)
        {
            return source.X <= other.X && source.Y <= other.Y && source.Z <= other.Z && source.W <= other.W;
        }

        /// <summary>
        /// Checks if both of a <see cref="Vector4"/>'s coordinates are less than the corresponding coordinates
        /// in another <see cref="Vector4"/>.
        /// </summary>
        /// <param name="source"><see cref="Vector4"/> to check.</param>
        /// <param name="other"><see cref="Vector4"/> containing the coordinates to check if are less than the 
        /// <paramref name="source"/>'s coordinates.</param>
        /// <returns>True if both of the coordinates in the <paramref name="source"/> are less than the
        /// corresponding coordinates in the <paramref name="other"/> <see cref="Vector4"/>. False if either or both
        /// coordinates are greater than or equal to their equivalent in the <paramref name="other"/>
        /// <see cref="Vector4"/>.</returns>
        public static bool IsLessThan(this Vector4 source, Vector4 other)
        {
            return source.X < other.X && source.Y < other.Y && source.Z < other.Z && source.W < other.W;
        }

        /// <summary>
        /// Gets a <see cref="Vector4"/> composed of the max value of the given components for the respective property.
        /// </summary>
        /// <param name="a">The first argument.</param>
        /// <param name="b">The second argument.</param>
        /// <returns>A <see cref="Vector4"/> composed of the max value of the given components for the respective
        /// property.</returns>
        public static Vector4 Max(this Vector4 a, Vector4 b)
        {
            return new Vector4(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y), Math.Max(a.Z, b.Z), Math.Max(a.W, b.W));
        }

        /// <summary>
        /// Gets a <see cref="Vector4"/> composed of the min value of the given components for the respective property.
        /// </summary>
        /// <param name="a">The first argument.</param>
        /// <param name="b">The second argument.</param>
        /// <returns>A <see cref="Vector4"/> composed of the max value of the given components for the respective
        /// property.</returns>
        public static Vector4 Min(this Vector4 a, Vector4 b)
        {
            return new Vector4(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Min(a.Z, b.Z), Math.Min(a.W, b.W));
        }

        /// <summary>
        /// Gets the distance between this <see cref="Vector4"/> and another <see cref="Vector4"/> by getting the
        /// sum of the differences for the components of the <see cref="Vector4"/>s.
        /// </summary>
        /// <param name="source">Source <see cref="Vector4"/>.</param>
        /// <param name="target">Target <see cref="Vector4"/>.</param>
        /// <returns>The distance between this <see cref="Vector4"/> and another <see cref="Vector4"/> by getting the
        /// sum of the differences for the components of the <see cref="Vector4"/>s.</returns>
        public static float QuickDistance(this Vector4 source, Vector4 target)
        {
            var diff = source - target;
            return Abs(diff).Sum();
        }

        /// <summary>
        /// Gets a <see cref="Vector4"/> with the X and Y components rounded.
        /// </summary>
        /// <param name="source"><see cref="Vector4"/> to round.</param>
        /// <returns>A <see cref="Vector4"/> containing the rounded value of the <paramref name="source"/>
        /// <see cref="Vector4"/>.</returns>
        public static Vector4 Round(this Vector4 source)
        {
            return new Vector4((float)Math.Round(source.X), (float)Math.Round(source.Y), (float)Math.Round(source.Z),
                (float)Math.Round(source.W));
        }

        /// <summary>
        /// Gets the sum of the x and y components of the <see cref="Vector4"/>.
        /// </summary>
        /// <param name="source"><see cref="Vector4"/> to get the sum of the components of.</param>
        /// <returns>Sum of the x and y components of the <see cref="Vector4"/>.</returns>
        public static float Sum(this Vector4 source)
        {
            return source.X + source.Y + source.Z + source.W;
        }
    }
}