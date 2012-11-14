using System.Linq;
using SFML.Graphics;

namespace NetGore.World
{
    /// <summary>
    /// Interface for an object that occupies space in the world.
    /// </summary>
    public interface ISpatial : IPositionable
    {
        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has moved.
        /// </summary>
        event TypedEventHandler<ISpatial, EventArgs<Vector2>> Moved;

        /// <summary>
        /// Notifies listeners when this <see cref="ISpatial"/> has been resized.
        /// </summary>
        event TypedEventHandler<ISpatial, EventArgs<Vector2>> Resized;

        /// <summary>
        /// Gets the world coordinates of the bottom-right corner of this <see cref="ISpatial"/>.
        /// </summary>
        Vector2 Max { get; }

        /// <summary>
        /// Gets the center position of the <see cref="ISpatial"/>.
        /// </summary>
        Vector2 Center { get; }

        /// <summary>
        /// Gets if this <see cref="ISpatial"/> can ever be moved with <see cref="ISpatial.TryMove"/>.
        /// </summary>
        bool SupportsMove { get; }

        /// <summary>
        /// Gets if this <see cref="ISpatial"/> can ever be resized with <see cref="ISpatial.TryResize"/>.
        /// </summary>
        bool SupportsResize { get; }

        /// <summary>
        /// Gets a <see cref="Rectangle"/> that represents the world area that this <see cref="ISpatial"/> occupies.
        /// </summary>
        /// <returns>A <see cref="Rectangle"/> that represents the world area that this <see cref="ISpatial"/>
        /// occupies.</returns>
        Rectangle ToRectangle();

        /// <summary>
        /// Tries to move the <see cref="ISpatial"/>.
        /// </summary>
        /// <param name="newPos">The new position.</param>
        /// <returns>True if the <see cref="ISpatial"/> was moved to the <paramref name="newPos"/>; otherwise false.</returns>
        bool TryMove(Vector2 newPos);

        /// <summary>
        /// Tries to resize the <see cref="ISpatial"/>.
        /// </summary>
        /// <param name="newSize">The new size.</param>
        /// <returns>True if the <see cref="ISpatial"/> was resized to the <paramref name="newSize"/>; otherwise false.</returns>
        bool TryResize(Vector2 newSize);
    }
}