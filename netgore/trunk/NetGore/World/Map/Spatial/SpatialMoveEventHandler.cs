using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore
{
    /// <summary>
    /// Handles when an <see cref="ISpatial"/> has moved.
    /// </summary>
    /// <param name="sender">The <see cref="ISpatial"/> that moved.</param>
    /// <param name="oldPosition">The position of the <see cref="ISpatial"/> before it moved.</param>
    public delegate void SpatialMoveEventHandler(ISpatial sender, Vector2 oldPosition);
}