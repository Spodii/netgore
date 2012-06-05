using System.Linq;
using NetGore.IO;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Interface that describes a single light.
    /// </summary>
    public interface ILight : ISpatial, IPersistable
    {
        /// <summary>
        /// Gets or sets the color of the light. The alpha value has no affect and will always be set to 255.
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// Gets or sets if this light is enabled.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="ISpatial"/> that provides the position to use. If set, the
        /// <see cref="ISpatial.Position"/> value will automatically be acquired with the position of the
        /// <see cref="ISpatial"/> instead, and setting the position will have no affect.
        /// </summary>
        ISpatial PositionProvider { get; set; }

        /// <summary>
        /// Gets or sets the amount to rotate the <see cref="ILight"/> in radians.
        /// </summary>
        float Rotation { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Grh"/> used to draw the light. If null, the light will not be drawn.
        /// </summary>
        Grh Sprite { get; set; }

        /// <summary>
        /// Gets or sets an object that can be used to identify or store information about this <see cref="ILight"/>.
        /// This property is purely optional.
        /// </summary>
        object Tag { get; set; }

        /// <summary>
        /// Draws the <see cref="ILight"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw with.</param>
        void Draw(ISpriteBatch spriteBatch);

        /// <summary>
        /// Translates this <see cref="ILight"/> relative to the current position.
        /// </summary>
        /// <param name="offset">The amount to move from the current position.</param>
        void Move(Vector2 offset);

        /// <summary>
        /// Sets the size of this <see cref="ILight"/>.
        /// </summary>
        /// <param name="newSize">The new size.</param>
        void Resize(Vector2 newSize);

        /// <summary>
        /// Sets the center position of the <see cref="ILight"/>.
        /// </summary>
        /// <param name="value">The new center.</param>
        void SetCenter(Vector2 value);

        /// <summary>
        /// Moves this <see cref="ILight"/> to a new position.
        /// </summary>
        /// <param name="newPosition">The new position.</param>
        void Teleport(Vector2 newPosition);

        /// <summary>
        /// Updates the <see cref="ILight"/>.
        /// </summary>
        /// <param name="currentTime">The current game time in milliseconds.</param>
        void Update(TickCount currentTime);
    }
}