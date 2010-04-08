using System;
using System.Linq;

namespace NetGore.Graphics
{
    /// <summary>
    /// Enum of flags containing the effects that can be applied to a sprite when rendering it.
    /// </summary>
    [Flags]
    public enum SpriteEffects : byte
    {
        /// <summary>
        /// No effects specified.
        /// </summary>
        None = 0,

        /// <summary>
        /// Rotate 180 degrees about the Y axis before rendering.
        /// </summary>
        FlipHorizontally = 1 << 0,

        /// <summary>
        /// Rotate 180 degrees about the X axis before rendering.
        /// </summary>
        FlipVertically = 1 << 1
    }
}