using System;
using System.Linq;
using SFML.Graphics;

namespace NetGore
{
    /// <summary>
    /// Extension methods for the <see cref="Vector3"/>.
    /// </summary>
    public static class Vector3Extensions
    {
        /// <summary>
        /// Gets the absolute value of the components of the <see cref="Vector3"/>.
        /// </summary>
        /// <param name="source"><see cref="Vector3"/> to get the absolute value of.</param>
        /// <returns>A <see cref="Vector3"/> containing the absolute value of the <paramref name="source"/>
        /// <see cref="Vector3"/>.</returns>
        public static Vector3 Abs(this Vector3 source)
        {
            return new Vector3(Math.Abs(source.X), Math.Abs(source.Y), Math.Abs(source.Z));
        }

        /// <summary>
        /// Gets a <see cref="Vector3"/> with the X and Y components rounded up.
        /// </summary>
        /// <param name="source"><see cref="Vector3"/> to round.</param>
        /// <returns>A <see cref="Vector3"/> containing the ceiling-down value of the <paramref name="source"/>
        /// <see cref="Vector3"/>.</returns>
        public static Vector3 Ceiling(this Vector3 source)
        {
            return new Vector3((float)Math.Ceiling(source.X), (float)Math.Ceiling(source.Y), (float)Math.Ceiling(source.Z));
        }

        /// <summary>
        /// Gets a <see cref="Vector3"/> with the X and Y components rounded down.
        /// </summary>
        /// <param name="source"><see cref="Vector3"/> to round.</param>
        /// <returns>A <see cref="Vector3"/> containing the rounded-down value of the <paramref name="source"/>
        /// <see cref="Vector3"/>.</returns>
        public static Vector3 Floor(this Vector3 source)
        {
            return new Vector3((float)Math.Floor(source.X), (float)Math.Floor(source.Y), (float)Math.Floor(source.Z));
        }

        /// <summary>
        /// Checks if both of a <see cref="Vector3"/>'s coordinates are greater than or equal to the
        /// corresponding coordinates in another <see cref="Vector3"/>.
        /// </summary>
        /// <param name="source"><see cref="Vector3"/> to check.</param>
        /// <param name="other"><see cref="Vector3"/> containing the coordinates to check if are greater than or equal to the 
        /// <paramref name="source"/>'s coordinates.</param>
        /// <returns>True if both of the coordinates in the <paramref name="source"/> are greater than or equal to the
        /// corresponding coordinates in the <paramref name="other"/> <see cref="Vector3"/>. False if either or both
        /// coordinates are less than their equivalent in the <paramref name="other"/> <see cref="Vector3"/>.</returns>
        public static bool IsGreaterOrEqual(this Vector3 source, Vector3 other)
        {
            return source.X >= other.X && source.Y >= other.Y && source.Z >= other.Z;
        }

        /// <summary>
        /// Checks if both of a <see cref="Vector3"/>'s coordinates are greater than the corresponding coordinates in another
        /// <see cref="Vector3"/>.
        /// </summary>
        /// <param name="source"><see cref="Vector3"/> to check.</param>
        /// <param name="other"><see cref="Vector3"/> containing the coordinates to check if are greater than the 
        /// <paramref name="source"/>'s coordinates.</param>
        /// <returns>True if both of the coordinates in the <paramref name="source"/> are greater than the
        /// corresponding coordinates in the <paramref name="other"/> <see cref="Vector3"/>. False if either or both
        /// coordinates are less than or equal to their equivalent in the <paramref name="other"/>
        /// <see cref="Vector3"/>.</returns>
        public static bool IsGreaterThan(this Vector3 source, Vector3 other)
        {
            return source.X > other.X && source.Y > other.Y && source.Z > other.Z;
        }

        /// <summary>
        /// Checks if both of a <see cref="Vector3"/>'s coordinates are less than or equal to the
        /// corresponding coordinates in another Vector3.
        /// </summary>
        /// <param name="source"><see cref="Vector3"/> to check.</param>
        /// <param name="other"><see cref="Vector3"/> containing the coordinates to check if are less than or equal to the 
        /// <paramref name="source"/>'s coordinates.</param>
        /// <returns>True if both of the coordinates in the <paramref name="source"/> are less than or equal to the
        /// corresponding coordinates in the <paramref name="other"/> <see cref="Vector3"/>. False if either or both
        /// coordinates are greater than their equivalent in the <paramref name="other"/> <see cref="Vector3"/>.</returns>
        public static bool IsLessOrEqual(this Vector3 source, Vector3 other)
        {
            return source.X <= other.X && source.Y <= other.Y && source.Z <= other.Z;
        }

        /// <summary>
        /// Checks if both of a <see cref="Vector3"/>'s coordinates are less than the corresponding coordinates
        /// in another <see cref="Vector3"/>.
        /// </summary>
        /// <param name="source"><see cref="Vector3"/> to check.</param>
        /// <param name="other"><see cref="Vector3"/> containing the coordinates to check if are less than the 
        /// <paramref name="source"/>'s coordinates.</param>
        /// <returns>True if both of the coordinates in the <paramref name="source"/> are less than the
        /// corresponding coordinates in the <paramref name="other"/> <see cref="Vector3"/>. False if either or both
        /// coordinates are greater than or equal to their equivalent in the <paramref name="other"/>
        /// <see cref="Vector3"/>.</returns>
        public static bool IsLessThan(this Vector3 source, Vector3 other)
        {
            return source.X < other.X && source.Y < other.Y && source.Z < other.Z;
        }

        /// <summary>
        /// Gets a <see cref="Vector3"/> composed of the max value of the given components for the respective property.
        /// </summary>
        /// <param name="a">The first argument.</param>
        /// <param name="b">The second argument.</param>
        /// <returns>A <see cref="Vector3"/> composed of the max value of the given components for the respective
        /// property.</returns>
        public static Vector3 Max(this Vector3 a, Vector3 b)
        {
            return new Vector3(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y), Math.Max(a.Z, b.Z));
        }

        /// <summary>
        /// Gets a <see cref="Vector3"/> composed of the min value of the given components for the respective property.
        /// </summary>
        /// <param name="a">The first argument.</param>
        /// <param name="b">The second argument.</param>
        /// <returns>A <see cref="Vector3"/> composed of the max value of the given components for the respective
        /// property.</returns>
        public static Vector3 Min(this Vector3 a, Vector3 b)
        {
            return new Vector3(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Min(a.Z, b.Z));
        }

        /// <summary>
        /// Gets the distance between this <see cref="Vector3"/> and another <see cref="Vector3"/> by getting the
        /// sum of the differences for the components of the <see cref="Vector3"/>s.
        /// </summary>
        /// <param name="source">Source <see cref="Vector3"/>.</param>
        /// <param name="target">Target <see cref="Vector3"/>.</param>
        /// <returns>The distance between this <see cref="Vector3"/> and another <see cref="Vector3"/> by getting the
        /// sum of the differences for the components of the <see cref="Vector3"/>s.</returns>
        public static float QuickDistance(this Vector3 source, Vector3 target)
        {
            var diff = source - target;
            return Abs(diff).Sum();
        }

        /// <summary>
        /// Gets a <see cref="Vector3"/> with the X and Y components rounded.
        /// </summary>
        /// <param name="source"><see cref="Vector3"/> to round.</param>
        /// <returns>A <see cref="Vector3"/> containing the rounded value of the <paramref name="source"/>
        /// <see cref="Vector3"/>.</returns>
        public static Vector3 Round(this Vector3 source)
        {
            return new Vector3((float)Math.Round(source.X), (float)Math.Round(source.Y), (float)Math.Round(source.Z));
        }

        /// <summary>
        /// Gets the sum of the x and y components of the <see cref="Vector3"/>.
        /// </summary>
        /// <param name="source"><see cref="Vector3"/> to get the sum of the components of.</param>
        /// <returns>Sum of the x and y components of the <see cref="Vector3"/>.</returns>
        public static float Sum(this Vector3 source)
        {
            return source.X + source.Y + source.Z;
        }
    }
}