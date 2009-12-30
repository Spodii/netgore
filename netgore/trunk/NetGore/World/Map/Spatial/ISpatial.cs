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

    /// <summary>
    /// Handles when an <see cref="ISpatial"/> has been resized.
    /// </summary>
    /// <param name="sender">The <see cref="ISpatial"/> that was resized.</param>
    /// <param name="oldSize">The size of the <see cref="ISpatial"/> before it was resized.</param>
    public delegate void SpatialResizeEventHandler(ISpatial sender, Vector2 oldSize);

    /// <summary>
    /// Interface for an object that occupies space in the world.
    /// </summary>
    public interface ISpatial
    {
        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has moved.
        /// </summary>
        event SpatialMoveEventHandler OnMove;

        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has been resized.
        /// </summary>
        event SpatialResizeEventHandler OnResize;

        /// <summary>
        /// Gets the world coordinates of the bottom-right corner of this <see cref="ISpatial"/>.
        /// </summary>
        Vector2 Max { get; }

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