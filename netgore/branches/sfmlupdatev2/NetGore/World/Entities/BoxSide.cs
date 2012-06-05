using System.Linq;

namespace NetGore.World
{
    /// <summary>
    /// Represents the side of a box.
    /// </summary>
    public enum BoxSide : byte
    {
        /// <summary>
        /// The left side.
        /// </summary>
        Left,

        /// <summary>
        /// The right side.
        /// </summary>
        Right,

        /// <summary>
        /// The top side.
        /// </summary>
        Top,

        /// <summary>
        /// The bottom side.
        /// </summary>
        Bottom
    }
}