using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SFML
{
    namespace Graphics
    {
        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Utility class for manipulating 32-bits RGBA colors
        /// </summary>
        ////////////////////////////////////////////////////////////
        [StructLayout(LayoutKind.Sequential)]
        public struct Color : IEquatable<Color>
        {
            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Construct the color from its red, green and blue components
            /// </summary>
            /// <param name="red">Red component</param>
            /// <param name="green">Green component</param>
            /// <param name="blue">Blue component</param>
            ////////////////////////////////////////////////////////////
            public Color(byte red, byte green, byte blue) : this(red, green, blue, 255)
            {
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Construct the color from its red, green, blue and alpha components
            /// </summary>
            /// <param name="red">Red component</param>
            /// <param name="green">Green component</param>
            /// <param name="blue">Blue component</param>
            /// <param name="alpha">Alpha (transparency) component</param>
            ////////////////////////////////////////////////////////////
            public Color(byte red, byte green, byte blue, byte alpha)
            {
                R = red;
                G = green;
                B = blue;
                A = alpha;
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Construct the color from another
            /// </summary>
            /// <param name="color">Color to copy</param>
            ////////////////////////////////////////////////////////////
            public Color(Color color) : this(color.R, color.G, color.B, color.A)
            {
            }

            /// <summary>Red component of the color</summary>
            public byte R;

            /// <summary>Green component of the color</summary>
            public byte G;

            /// <summary>Blue component of the color</summary>
            public byte B;

            /// <summary>Alpha (transparent) component of the color</summary>
            public byte A;

            /// <summary>Predefined black color</summary>
            public static readonly Color Black = new Color(0, 0, 0);

            /// <summary>Predefined white color</summary>
            public static readonly Color White = new Color(255, 255, 255);

            /// <summary>Predefined red color</summary>
            public static readonly Color Red = new Color(255, 0, 0);

            /// <summary>Predefined green color</summary>
            public static readonly Color Green = new Color(0, 255, 0);

            /// <summary>Predefined blue color</summary>
            public static readonly Color Blue = new Color(0, 0, 255);

            /// <summary>Predefined yellow color</summary>
            public static readonly Color Yellow = new Color(255, 255, 0);

            /// <summary>Predefined magenta color</summary>
            public static readonly Color Magenta = new Color(255, 0, 255);

            /// <summary>Predefined cyan color</summary>
            public static readonly Color Cyan = new Color(0, 255, 255);

            /// <summary>Gets a string representation of this object.</summary>
            public override string ToString()
            {
                return string.Format(CultureInfo.CurrentCulture, "{{R:{0} G:{1} B:{2} A:{3}}}", new object[] { R, G, B, A });
            }

            /// <summary>Returns a value that indicates whether the current instance is equal to a specified object.</summary>
            /// <param name="obj">The Object to compare with the current Color.</param>
            public override bool Equals(object obj)
            {
                return ((obj is Color) && Equals((Color)obj));
            }

            /// <summary>Returns a value that indicates whether the current instance is equal to a specified object.</summary>
            /// <param name="other">The Color to compare with the current Color.</param>
            public bool Equals(Color other)
            {
                return (R == other.R) && (G == other.G) && (B == other.B) && (A == other.A);
            }

            /// <summary>Compares two objects to determine whether they are the same.</summary>
            /// <param name="a">The object to the left of the equality operator.</param>
            /// <param name="b">The object to the right of the equality operator.</param>
            public static bool operator ==(Color a, Color b)
            {
                return a.Equals(b);
            }

            /// <summary>Compares two objects to determine whether they are different.</summary>
            /// <param name="a">The object to the left of the equality operator.</param>
            /// <param name="b">The object to the right of the equality operator.</param>
            public static bool operator !=(Color a, Color b)
            {
                return !a.Equals(b);
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <returns>
            /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
            /// </returns>
            public override int GetHashCode()
            {
                return ((R << 24) | (G << 16) | (B << 8) | A).GetHashCode();
            }
        }
    }
}