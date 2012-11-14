using SFML.Graphics;

namespace NetGore.World
{
    /// <summary>
    /// Interface for an object that defines a position and size.
    /// </summary>
    public interface IPositionable
    {
        /// <summary>
        /// Gets the world coordinates of the top-left corner of this <see cref="IPositionable"/>.
        /// </summary>
        Vector2 Position { get; }

        /// <summary>
        /// Gets the size of this <see cref="IPositionable"/>.
        /// </summary>
        Vector2 Size { get; }
    }
}