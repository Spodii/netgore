using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.Xna.Framework;
using NetGore;

namespace DemoGame.Client
{
    /// <summary>
    /// Assists in smoothing out Entity movement by interpolating the position drawn.
    /// </summary>
    public class EntityInterpolator
    {
        /// <summary>
        /// The absolute value of the greatest velocity of the Entity.
        /// </summary>
        Vector2 _greatestVelocity;
        Vector2 _drawPosition;

        /// <summary>
        /// Gets the position to use when drawing the Entity.
        /// </summary>
        public Vector2 DrawPosition
        {
            get { return _drawPosition.Round(); }
        }

        /// <summary>
        /// Forces the Interpolator to teleport the DrawPosition to the specified position.
        /// </summary>
        /// <param name="position">The new position.</param>
        public void Teleport(Vector2 position)
        {
            _drawPosition = position;
        }

        /// <summary>
        /// Updates the Entity's greatest velocity.
        /// </summary>
        /// <param name="currentVelocity">Current velocity.</param>
        void UpdateGreatestVelocity(Vector2 currentVelocity)
        {
            currentVelocity = currentVelocity.Abs();

            if (currentVelocity.X > _greatestVelocity.X)
                _greatestVelocity.X = currentVelocity.X;

            if (currentVelocity.Y > _greatestVelocity.Y)
                _greatestVelocity.Y = currentVelocity.Y;
        }

        /// <summary>
        /// Updates the drawing position interpolation.
        /// </summary>
        /// <param name="entity">Entity that this EntityInterpolator is for.</param>
        /// <param name="deltaTime">Time elapsed since the last update.</param>
        public void Update(Entity entity, int deltaTime)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            Vector2 position = entity.Position;
            UpdateGreatestVelocity(entity.Velocity);

            // Draw position and real position are already equal
            if (position == _drawPosition)
                return;

            // Get the velocity to use
            Vector2 velocity = entity.Velocity.Abs();

            // If the velocity we grabbed is zero, but we're not at the position we need to be at, we will have to use
            // the greatest velocity so we can "guess" on how to move (since not moving is not a good option)
            if (velocity.X == 0 && position.X != _drawPosition.X)
                velocity.X = _greatestVelocity.X;
            if (velocity.Y == 0 && position.Y != _drawPosition.Y)
                velocity.Y = _greatestVelocity.Y;

            // Make sure we point in the correct direction - that is, point the DrawPosition to the Position
            if (position.X < _drawPosition.X)
                velocity.X *= -1;
            if (position.Y < _drawPosition.Y)
                velocity.Y *= -1;

            // Get the new position
            Vector2 newPosition = _drawPosition + (velocity * deltaTime);

            // Don't allow the draw position to exceed the real position
            if (_drawPosition.X > position.X && newPosition.X < position.X)
                newPosition.X = position.X;
            else if (_drawPosition.X < position.X && newPosition.X > position.X)
                newPosition.X = position.X;

            if (_drawPosition.Y > position.Y && newPosition.Y < position.Y)
                newPosition.Y = position.Y;
            else if (_drawPosition.Y < position.Y && newPosition.Y > position.Y)
                newPosition.Y = position.Y;

            // If we are too far out of sync, just jump to the real position
            Vector2 diff = (newPosition - position).Abs();
            const float teleDiff = 50f;
            if (diff.X > teleDiff || diff.Y > teleDiff)
                newPosition = position;

            // Set the new drawing position
            _drawPosition = newPosition;
        }
    }
}