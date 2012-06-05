using System.Linq;

namespace NetGore.Graphics.GUI
{
    /// <summary>
    /// The different sprite types for a <see cref="ControlBorder"/>.
    /// </summary>
    public enum ControlBorderSpriteType : byte
    {
        /// <summary>
        /// The background.
        /// </summary>
        Background,

        /// <summary>
        /// The top border.
        /// </summary>
        Top,

        /// <summary>
        /// The top-right corner.
        /// </summary>
        TopRight,

        /// <summary>
        /// The right border.
        /// </summary>
        Right,

        /// <summary>
        /// The bottom-right corner.
        /// </summary>
        BottomRight,

        /// <summary>
        /// The bottom border.
        /// </summary>
        Bottom,

        /// <summary>
        /// The bottom-left corner.
        /// </summary>
        BottomLeft,

        /// <summary>
        /// The left border.
        /// </summary>
        Left,

        /// <summary>
        /// The top-left corner.
        /// </summary>
        TopLeft
    }
}