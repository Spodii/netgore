using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace SFML
{
    namespace Graphics
    {
        ////////////////////////////////////////////////////////////
        /// <summary>
        /// IntRect is an utility class for manipulating 2D rectangles
        /// with integer coordinates
        /// </summary>
        ////////////////////////////////////////////////////////////
        [StructLayout(LayoutKind.Sequential)]
        public struct IntRect : IEquatable<IntRect>
        {
            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Construct the rectangle from its coordinates
            /// </summary>
            /// <param name="left">Left coordinate of the rectangle</param>
            /// <param name="top">Top coordinate of the rectangle</param>
            /// <param name="width">Width of the rectangle</param>
            /// <param name="height">Height of the rectangle</param>
            ////////////////////////////////////////////////////////////
            public IntRect(int left, int top, int width, int height)
            {
                Left = left;
                Top = top;
                Width = width;
                Height = height;
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Check if a point is inside the rectangle's area
            /// </summary>
            /// <param name="x">X coordinate of the point to test</param>
            /// <param name="y">Y coordinate of the point to test</param>
            /// <returns>True if the point is inside</returns>
            ////////////////////////////////////////////////////////////
            public bool Contains(int x, int y)
            {
                return (x >= Left) && (x < Left + Width) && (y >= Top) && (y < Top + Height);
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Check intersection between two rectangles
            /// </summary>
            /// <param name="rect"> Rectangle to test</param>
            /// <returns>True if rectangles overlap</returns>
            ////////////////////////////////////////////////////////////
            public bool Intersects(IntRect rect)
            {
                // Compute the intersection boundaries
                var left = Math.Max(Left, rect.Left);
                var top = Math.Max(Top, rect.Top);
                var right = Math.Min(Left + Width, rect.Left + rect.Width);
                var bottom = Math.Min(Top + Height, rect.Top + rect.Height);

                return (left < right) && (top < bottom);
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Check intersection between two rectangles
            /// </summary>
            /// <param name="rect"> Rectangle to test</param>
            /// <param name="overlap">Rectangle to be filled with overlapping rect</param>
            /// <returns>True if rectangles overlap</returns>
            ////////////////////////////////////////////////////////////
            public bool Intersects(IntRect rect, out IntRect overlap)
            {
                // Compute the intersection boundaries
                var left = Math.Max(Left, rect.Left);
                var top = Math.Max(Top, rect.Top);
                var right = Math.Min(Left + Width, rect.Left + rect.Width);
                var bottom = Math.Min(Top + Height, rect.Top + rect.Height);

                // If the intersection is valid (positive non zero area), then there is an intersection
                if ((left < right) && (top < bottom))
                {
                    overlap.Left = left;
                    overlap.Top = top;
                    overlap.Width = right - left;
                    overlap.Height = bottom - top;
                    return true;
                }
                else
                {
                    overlap.Left = 0;
                    overlap.Top = 0;
                    overlap.Width = 0;
                    overlap.Height = 0;
                    return false;
                }
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Provide a string describing the object
            /// </summary>
            /// <returns>String description of the object</returns>
            ////////////////////////////////////////////////////////////
            public override string ToString()
            {
                return "[IntRect]" + " Left(" + Left + ")" + " Top(" + Top + ")" + " Width(" + Width + ")" + " Height(" + Height +
                       ")";
            }

            /// <summary>Left coordinate of the rectangle</summary>
            public int Left;

            /// <summary>Top coordinate of the rectangle</summary>
            public int Top;

            /// <summary>Width of the rectangle</summary>
            public int Width;

            /// <summary>Height of the rectangle</summary>
            public int Height;

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
            /// </returns>
            public bool Equals(IntRect other)
            {
                return other.Left == Left && other.Top == Top && other.Width == Width && other.Height == Height;
            }

            /// <summary>
            /// Indicates whether this instance and a specified object are equal.
            /// </summary>
            /// <param name="obj">Another object to compare to.</param>
            /// <returns>
            /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
            /// </returns>
            public override bool Equals(object obj)
            {
                return obj is IntRect && this == (IntRect)obj;
            }

            /// <summary>
            /// Returns the hash code for this instance.
            /// </summary>
            /// <returns>
            /// A 32-bit signed integer that is the hash code for this instance.
            /// </returns>
            public override int GetHashCode()
            {
                unchecked
                {
                    int result = Left;
                    result = (result * 397) ^ Top;
                    result = (result * 397) ^ Width;
                    result = (result * 397) ^ Height;
                    return result;
                }
            }

            /// <summary>
            /// Implements the operator ==.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator ==(IntRect left, IntRect right)
            {
                return left.Equals(right);
            }

            /// <summary>
            /// Implements the operator !=.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator !=(IntRect left, IntRect right)
            {
                return !left.Equals(right);
            }
        }

        ////////////////////////////////////////////////////////////
    }
}