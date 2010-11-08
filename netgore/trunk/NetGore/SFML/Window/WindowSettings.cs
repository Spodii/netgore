using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace SFML
{
    namespace Window
    {
        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Structure defining the creation settings of windows
        /// </summary>
        ////////////////////////////////////////////////////////////
        [StructLayout(LayoutKind.Sequential)]
        public struct WindowSettings : IEquatable<WindowSettings>
        {
            ////////////////////////////////////////////////////////////

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Construct the settings from depth / stencil bits and antialiasing level
            /// </summary>
            /// <param name="depthBits">Depth buffer bits</param>
            /// <param name="stencilBits">Stencil buffer bits</param>
            /// <param name="antialiasingLevel">Antialiasing level</param>
            ////////////////////////////////////////////////////////////
            public WindowSettings(uint depthBits, uint stencilBits, uint antialiasingLevel = 0u)
            {
                DepthBits = depthBits;
                StencilBits = stencilBits;
                AntialiasingLevel = antialiasingLevel;
            }

            /// <summary>Depth buffer bits (0 is disabled)</summary>
            public uint DepthBits;

            /// <summary>Stencil buffer bits (0 is disabled)</summary>
            public uint StencilBits;

            /// <summary>Antialiasing level (0 is disabled)</summary>
            public uint AntialiasingLevel;

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
            /// </returns>
            public bool Equals(WindowSettings other)
            {
                return other.DepthBits == DepthBits && other.StencilBits == StencilBits && other.AntialiasingLevel == AntialiasingLevel;
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
                return obj is WindowSettings && this == (WindowSettings)obj;
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
                    int result = DepthBits.GetHashCode();
                    result = (result * 397) ^ StencilBits.GetHashCode();
                    result = (result * 397) ^ AntialiasingLevel.GetHashCode();
                    return result;
                }
            }

            /// <summary>
            /// Implements the operator ==.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator ==(WindowSettings left, WindowSettings right)
            {
                return left.Equals(right);
            }

            /// <summary>
            /// Implements the operator !=.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator !=(WindowSettings left, WindowSettings right)
            {
                return !left.Equals(right);
            }
        }
    }
}