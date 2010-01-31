using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore
{
    /// <summary>
    /// Handles when an <see cref="ISpatial"/> has moved.
    /// </summary>
    /// <param name="sender">The <see cref="ISpatial"/> that moved.</param>
    /// <param name="e">The event argument.</param>
    public delegate void SpatialEventHandler<T>(ISpatial sender, T e);
}