using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace SFML
{
    namespace Window
    {
        ////////////////////////////////////////////////////////////
        /// <summary>
        /// VideoMode defines a video mode (width, height, bpp, frequency)
        /// and provides static functions for getting modes supported
        /// by the display device
        /// </summary>
        ////////////////////////////////////////////////////////////
        [StructLayout(LayoutKind.Sequential)]
        public struct VideoMode : IEquatable<VideoMode>
        {
            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Construct the video mode with its width and height
            /// </summary>
            /// <param name="width">Video mode width</param>
            /// <param name="height">Video mode height</param>
            ////////////////////////////////////////////////////////////
            public VideoMode(uint width, uint height) : this(width, height, 32)
            {
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Construct the video mode with its width, height and depth
            /// </summary>
            /// <param name="width">Video mode width</param>
            /// <param name="height">Video mode height</param>
            /// <param name="bpp">Video mode depth (bits per pixel)</param>
            ////////////////////////////////////////////////////////////
            public VideoMode(uint width, uint height, uint bpp)
            {
                Width = width;
                Height = height;
                BitsPerPixel = bpp;
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Tell whether or not the video mode is supported
            /// </summary>
            /// <returns>True if the video mode is valid, false otherwise</returns>
            ////////////////////////////////////////////////////////////
            public bool IsValid()
            {
                return sfVideoMode_IsValid(this);
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Get the list of all the supported fullscreen video modes
            /// </summary>
            ////////////////////////////////////////////////////////////
            public static VideoMode[] FullscreenModes
            {
                get
                {
                    unsafe
                    {
                        uint Count;
                        var ModesPtr = sfVideoMode_GetFullscreenModes(out Count);
                        var Modes = new VideoMode[Count];
                        for (uint i = 0; i < Count; ++i)
                        {
                            Modes[i] = ModesPtr[i];
                        }

                        return Modes;
                    }
                }
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Get the current desktop video mode
            /// </summary>
            ////////////////////////////////////////////////////////////
            public static VideoMode DesktopMode
            {
                get { return sfVideoMode_GetDesktopMode(); }
            }

            ////////////////////////////////////////////////////////////
            /// <summary>
            /// Provide a string describing the object
            /// </summary>
            /// <returns>String description of the object</returns>
            ////////////////////////////////////////////////////////////
            public override string ToString()
            {
                return "[VideoMode]" + " Width(" + Width + ")" + " Height(" + Height + ")" + " BitsPerPixel(" + BitsPerPixel + ")";
            }

            /// <summary>Video mode width, in pixels</summary>
            public uint Width;

            /// <summary>Video mode height, in pixels</summary>
            public uint Height;

            /// <summary>Video mode depth, in bits per pixel</summary>
            public uint BitsPerPixel;

            #region Imports

            [DllImport("csfml2-window", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern VideoMode sfVideoMode_GetDesktopMode();

            [DllImport("csfml2-window", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern unsafe VideoMode* sfVideoMode_GetFullscreenModes(out uint Count);

            [DllImport("csfml2-window", CallingConvention = CallingConvention.Cdecl)]
            [SuppressUnmanagedCodeSecurity]
            static extern bool sfVideoMode_IsValid(VideoMode Mode);

            #endregion

            /// <summary>
            /// Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>
            /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
            /// </returns>
            public bool Equals(VideoMode other)
            {
                return other.Width == Width && other.Height == Height && other.BitsPerPixel == BitsPerPixel;
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
                return obj is VideoMode && this == (VideoMode)obj;
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
                    var result = Width.GetHashCode();
                    result = (result * 397) ^ Height.GetHashCode();
                    result = (result * 397) ^ BitsPerPixel.GetHashCode();
                    return result;
                }
            }

            /// <summary>
            /// Implements the operator ==.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator ==(VideoMode left, VideoMode right)
            {
                return left.Equals(right);
            }

            /// <summary>
            /// Implements the operator !=.
            /// </summary>
            /// <param name="left">The left argument.</param>
            /// <param name="right">The right argument.</param>
            /// <returns>The result of the operator.</returns>
            public static bool operator !=(VideoMode left, VideoMode right)
            {
                return !left.Equals(right);
            }
        }
    }
}