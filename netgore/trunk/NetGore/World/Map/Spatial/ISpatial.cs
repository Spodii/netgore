using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore
{
    /// <summary>
    /// Interface for an object that occupies space in the world.
    /// </summary>
    public interface ISpatial
    {
        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has moved.
        /// </summary>
        event SpatialEventHandler<Vector2> Moved;

        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has been resized.
        /// </summary>
        event SpatialEventHandler<Vector2> Resized;

        /// <summary>
        /// Gets the world coordinates of the bottom-right corner of this <see cref="ISpatial"/>.
        /// </summary>
        Vector2 Max { get; }

        /// <summary>
        /// Gets the center position of the <see cref="ISpatial"/>.
        /// </summary>
        Vector2 Center { get; }

        /// <summary>
        /// Gets the world coordinates of the top-left corner of this <see cref="ISpatial"/>.
        /// </summary>
        Vector2 Position { get; }

        /// <summary>
        /// Gets the size of this <see cref="ISpatial"/>.
        /// </summary>
        Vector2 Size { get; }

        /// <summary>
        /// Gets a <see cref="Rectangle"/> that represents the world area that this <see cref="ISpatial"/> occupies.
        /// </summary>
        /// <returns>A <see cref="Rectangle"/> that represents the world area that this <see cref="ISpatial"/>
        /// occupies.</returns>
        Rectangle ToRectangle();
    }
}