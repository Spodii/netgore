using System;
using System.Linq;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Interface for an effect that is used on the refraction mapping to distort the screen.
    /// </summary>
    public interface IRefractionEffect : ISpatial, IDisposable
    {
        /// <summary>
        /// Gets the drawing priority of this <see cref="IRefractionEffect"/>. The value is relative to other
        /// <see cref="IRefractionEffect"/>s. <see cref="IRefractionEffect"/>s with lower values are drawn first.
        /// </summary>
        int DrawPriority { get; }

        /// <summary>
        /// Gets or sets if this refraction effect is enabled.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Gets if this effect has expired. Not all effects have to expire. This is not the same as <see cref="IRefractionEffect.IsEnabled"/>
        /// since expired effects are completely destroyed and removed, while disabled effects can still be enabled again. Once this
        /// value is true, it should remain true and the effect should cease being used.
        /// Disposing an <see cref="IRefractionEffect"/> will force it to be expired.
        /// </summary>
        bool IsExpired { get; }

        /// <summary>
        /// Gets or sets an <see cref="ISpatial"/> that provides the position to use. If set, the
        /// <see cref="ISpatial.Position"/> value will automatically be acquired with the position of the
        /// <see cref="ISpatial"/> instead, and setting the position will have no affect.
        /// </summary>
        ISpatial PositionProvider { get; set; }

        /// <summary>
        /// Gets or sets an object that can be used to identify or store information about this <see cref="IRefractionEffect"/>.
        /// This property is purely optional.
        /// </summary>
        object Tag { get; set; }

        /// <summary>
        /// Draws the <see cref="IRefractionEffect"/>.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to draw with.</param>
        void Draw(ISpriteBatch spriteBatch);

        /// <summary>
        /// Translates this <see cref="IRefractionEffect"/> relative to the current position.
        /// </summary>
        /// <param name="offset">The amount to move from the current position.</param>
        void Move(Vector2 offset);

        /// <summary>
        /// Moves this <see cref="IRefractionEffect"/> to a new position.
        /// </summary>
        /// <param name="newPosition">The new position.</param>
        void Teleport(Vector2 newPosition);

        /// <summary>
        /// Updates the <see cref="IRefractionEffect"/>.
        /// </summary>
        /// <param name="currentTime">The current game time in milliseconds.</param>
        void Update(TickCount currentTime);
    }
}