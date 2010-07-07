using System.Linq;

namespace NetGore.World
{
    /// <summary>
    /// Describes how something is aligned to the target.
    /// </summary>
    public enum Alignment : byte
    {
        /// <summary>
        /// Top-left corner of this item is at the top-left corner of the target.
        /// </summary>
        TopLeft,

        /// <summary>
        /// Top-right corner of this item is at the top-right corner of the target.
        /// </summary>
        TopRight,

        /// <summary>
        /// Bottom-left corner of this item is at the bottom-left corner of the target.
        /// </summary>
        BottomLeft,

        /// <summary>
        /// Bottom-right corner of this item is at the bottom-right corner of the target.
        /// </summary>
        BottomRight,

        /// <summary>
        /// Top side of this item is centered on the top side of the target.
        /// </summary>
        Top,

        /// <summary>
        /// Left side of this item is centered on the left side of the target.
        /// </summary>
        Left,

        /// <summary>
        /// Bottom side of this item is centered on the bottom side of the target.
        /// </summary>
        Bottom,

        /// <summary>
        /// Right side of this item is centered on the right side of the target.
        /// </summary>
        Right,

        /// <summary>
        /// Center of this item is centered on the center of the target.
        /// </summary>
        Center
    }
}