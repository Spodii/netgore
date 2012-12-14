using System;
using System.Linq;
using SFML.Graphics;

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
        /// Calculates the <see cref="Vector2"/> position to use to make the <paramref name="source"/> travel towards
        /// the <paramref name="target"/>.
        /// </summary>
        /// <param name="source">The <see cref="Vector2"/> that will be moving.</param>
        /// <param name="target">The <see cref="Vector2"/> containing the position to move towards.</param>
        /// <param name="distance">The total number of units to move. When moving objects based on elapsed time, this will typically
        /// contain the number of pixels to move per millisecond (speed * elapsedTime).</param>
        /// <returns>The new <see cref="Vector2"/> position to give the <paramref name="source"/>.</returns>
        public static Vector2 MoveTowards(this Vector2 source, Vector2 target, float distance)
        {
            // Get the difference between the source and target
            var diff = source - target;

            // Find the angle to travel
            var angle = Math.Atan2(diff.Y, diff.X);

            // Get the offset vector
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);
            var offset = new Vector2((float)(cos * distance), (float)(sin * distance));

            return source - offset;
        }

        /// <summary>
        /// Calculates the <see cref="Vector2"/> position to use to make the <paramref name="source"/> travel towards
        /// the <paramref name="target"/>.
        /// </summary>
        /// <param name="source">The <see cref="Vector2"/> that will be moving.</param>
        /// <param name="target">The <see cref="Vector2"/> containing the position to move towards.</param>
        /// <param name="distance">The total number of units to move. When moving objects based on elapsed time, this will typically
        /// contain the number of pixels to move per millisecond (speed * elapsedTime).</param>
        /// <param name="doNotPass">If true, the <paramref name="source"/> will stop at the <paramref name="target"/> when it has
        /// been reached instead of going past it.</param>
        /// <returns>
        /// The new <see cref="Vector2"/> position to give the <paramref name="source"/>.
        /// </returns>
        public static Vector2 MoveTowards(this Vector2 source, Vector2 target, float distance, bool doNotPass = true)
        {
            // Get the difference between the source and target
            var diff = source - target;

            // Find the angle to travel
            var angle = Math.Atan2(diff.Y, diff.X);

            // Get the offset vector
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);
            var offset = new Vector2((float)(cos * distance), (float)(sin * distance));

            // Apply the offset to get the new position
            var newPos = source - offset;

            // Make sure we do not overshoot
            if (doNotPass)
            {
                if (offset.X > float.Epsilon)
                {
                    if (newPos.X > target.X)
                        newPos.X = target.X;
                }
                else if (offset.X < -float.Epsilon)
                {
                    if (newPos.X < target.X)
                        newPos.X = target.X;
                }

                if (offset.Y > float.Epsilon)
                {
                    if (newPos.Y > target.Y)
                        newPos.Y = target.Y;
                }
                else if (offset.Y < -float.Epsilon)
                {
                    if (newPos.Y < target.Y)
                        newPos.Y = target.Y;
                }
            }

            return newPos;
        }

        /// <summary>
        /// Gets the distance between this <see cref="Vector2"/> and another <see cref="Vector2"/> by getting the
        /// sum of the differences for the components of the <see cref="Vector2"/>s. This is a much quicker but less accurate
        /// version of Distance().
        /// </summary>
        /// <param name="source">Source <see cref="Vector2"/>.</param>
        /// <param name="target">Target <see cref="Vector2"/>.</param>
        /// <returns>The distance between this <see cref="Vector2"/> and another <see cref="Vector2"/> by getting the
        /// sum of the differences for the components of the <see cref="Vector2"/>s.</returns>
        public static float QuickDistance(this Vector2 source, Vector2 target)
        {
            var diff = source - target;
            return Abs(diff).Sum();
        }

        /// <summary>
        /// Gets the proper distance between two vectors. Slower than QuickDistance, but more accurate.
        /// </summary>
        /// <param name="source">Source <see cref="Vector2"/>.</param>
        /// <param name="target">Target <see cref="Vector2"/>.</param>
        /// <returns>The distance between the two vectors.</returns>
        public static float Distance(this Vector2 source, Vector2 target)
        {
            return Vector2.Distance(source, target);
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
        /// Gets the sum of the x and y components of the <see cref="Vector2"/>.
        /// </summary>
        /// <param name="source"><see cref="Vector2"/> to get the sum of the components of.</param>
        /// <returns>Sum of the x and y components of the <see cref="Vector2"/>.</returns>
        public static float Sum(this Vector2 source)
        {
            return source.X + source.Y;
        }

        /// <summary>
        /// Gets a <see cref="Rectangle"/> that describes the area around a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Vector2"/>.</param>
        /// <param name="size">The size of the <see cref="Rectangle"/>.</param>
        /// <param name="center">If true, the <paramref name="source"/> will be at the center of the returned
        /// <see cref="Rectangle"/>. If false, the <paramref name="source"/> will be the top-left corner (min)
        /// of the <see cref="Rectangle"/>.</param>
        /// <returns>The <see cref="Rectangle"/>.</returns>
        public static Rectangle ToRectangle(this Vector2 source, Vector2 size, bool center)
        {
            return ToRectangle(source, size.X, size.Y, center);
        }

        /// <summary>
        /// Gets a <see cref="Rectangle"/> that describes the area around a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="source">The source <see cref="Vector2"/>.</param>
        /// <param name="width">The width of the <see cref="Rectangle"/>.</param>
        /// <param name="height">The height of the <see cref="Rectangle"/>.</param>
        /// <param name="center">If true, the <paramref name="source"/> will be at the center of the returned
        /// <see cref="Rectangle"/>. If false, the <paramref name="source"/> will be the top-left corner (min)
        /// of the <see cref="Rectangle"/>.</param>
        /// <returns>The <see cref="Rectangle"/>.</returns>
        public static Rectangle ToRectangle(this Vector2 source, float width, float height, bool center)
        {
            Vector2 min;

            if (center)
                min = source - new Vector2(width / 2f, height / 2f);
            else
                min = source;

            return new Rectangle(Math.Round(min.X), Math.Round(min.Y), Math.Round(width), Math.Round(height));
        }
    }
}