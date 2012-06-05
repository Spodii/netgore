using System;
using System.Linq;

namespace NetGore.Editor
{
    /// <summary>
    /// States how a transformation box can modify the target
    /// </summary>
    [Flags]
    enum TransBoxType : byte
    {
        /// <summary>
        /// Move box (no transformation)
        /// </summary>
        Move = 0,

        /// <summary>
        /// Top side transform
        /// </summary>
        Top = 1,

        /// <summary>
        /// Left side transform
        /// </summary>
        Left = 2,

        /// <summary>
        /// Bottom side transform
        /// </summary>
        Bottom = 4,

        /// <summary>
        /// Right side transform
        /// </summary>
        Right = 8,

        /// <summary>
        /// Top-left side transform
        /// </summary>
        TopLeft = Top | Left,

        /// <summary>
        /// Top-right side transform
        /// </summary>
        TopRight = Top | Right,

        /// <summary>
        /// Bottom-left side transform
        /// </summary>
        BottomLeft = Bottom | Left,

        /// <summary>
        /// Bottom-right side transform
        /// </summary>
        BottomRight = Bottom | Right
    }
}