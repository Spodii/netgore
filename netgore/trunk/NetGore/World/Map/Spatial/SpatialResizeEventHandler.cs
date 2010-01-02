using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore
{
    /// <summary>
    /// Handles when an <see cref="ISpatial"/> has been resized.
    /// </summary>
    /// <param name="sender">The <see cref="ISpatial"/> that was resized.</param>
    /// <param name="oldSize">The size of the <see cref="ISpatial"/> before it was resized.</param>
    public delegate void SpatialResizeEventHandler(ISpatial sender, Vector2 oldSize);
}