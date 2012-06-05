using System;
using System.Linq;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// Assists in smoothing out <see cref="Entity"/> movement by interpolating the position drawn.
    /// </summary>
    public class EntityInterpolator
    {
        /// <summary>
        /// The lowest acceptable velocity to use for when no velocity is given. This is to ensure that an entity
        /// does that currently has no velocity does not end up getting stuck moving REAALLLYY slowly since their
        /// _greatestVelocity is greater than zero, but just barely.
        /// </summary>
        const float _lowestAcceptableVelocity = 0.05f;

        /// <summary>
        /// The maximum distance allowed between the Entity's Position and the DrawPosition. If the distance between
        /// these two points exceeds this value, the DrawPosition should be set to the Position instead of interpolating
        /// to it.
        /// </summary>
        const float _maxAllowedDistance = 50.0f;

        Vector2 _drawPosition;

        /// <summary>
        /// The absolute value of the greatest velocity of the <see cref="Entity"/>.
        /// </summary>
        Vector2 _greatestVelocity;

        /// <summary>
        /// Gets the position to use when drawing the <see cref="Entity"/>.
        /// </summary>
        public Vector2 DrawPosition
        {
            get { return _drawPosition.Round(); }
        }

        /// <summary>
        /// Forces the <see cref="DrawPosition"/> to the specified position without any interpolation.
        /// </summary>
        /// <param name="position">The new position.</param>
        public void Teleport(Vector2 position)
        {
            _drawPosition = position;
        }

        /// <summary>
        /// Updates the drawing position interpolation.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/> that this <see cref="EntityInterpolator"/> is for.</param>
        /// <param name="deltaTime">Time elapsed since the last update.</param>
        /// <exception cref="ArgumentNullException"><paramref name="entity" /> is <c>null</c>.</exception>
        public void Update(Entity entity, int deltaTime)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var position = entity.Position;
            UpdateGreatestVelocity(entity.Velocity);

            // Draw position and real position are already equal
            if (position == _drawPosition)
                return;

            // Get the velocity to use
            var velocity = entity.Velocity.Abs();

            // If the velocity we grabbed is zero, but we're not at the position we need to be at, we will have to use
            // the greatest velocity so we can "guess" on how to move (since not moving is not a good option). If we also
            // don't have a greatestVelocity (the entity has never moved), then we just warp the entity's draw position
            // straight to the real position.
            if (velocity.X <= float.Epsilon && position.X != _drawPosition.X)
            {
                if (_greatestVelocity.X <= _lowestAcceptableVelocity)
                    _drawPosition.X = position.X;
                else
                    velocity.X = _greatestVelocity.X;
            }
            if (velocity.Y <= float.Epsilon && position.Y != _drawPosition.Y)
            {
                if (_greatestVelocity.Y <= _lowestAcceptableVelocity)
                    _drawPosition.Y = position.Y;
                else
                    velocity.Y = _greatestVelocity.Y;
            }

            // Make sure we point in the correct direction - that is, point the DrawPosition to the Position
            if (position.X < _drawPosition.X)
                velocity.X *= -1;
            if (position.Y < _drawPosition.Y)
                velocity.Y *= -1;

            // Get the new position
            var newPosition = _drawPosition + (velocity * deltaTime);

            // Don't allow the draw position to exceed the real position
            if (velocity.X > 0)
            {
                // Moving right
                if (newPosition.X > position.X)
                    newPosition.X = position.X;
            }
            else if (velocity.X < 0)
            {
                // Moving left
                if (newPosition.X < position.X)
                    newPosition.X = position.X;
            }

            if (velocity.Y > 0)
            {
                // Moving down
                if (newPosition.Y > position.Y)
                    newPosition.Y = position.Y;
            }
            else if (velocity.Y < 0)
            {
                // Moving up
                if (newPosition.Y < position.Y)
                    newPosition.Y = position.Y;
            }

            // If we are too far out of sync, just jump to the real position
            var diff = newPosition.QuickDistance(position);
            if (diff > _maxAllowedDistance)
                newPosition = position;

            // Set the new drawing position
            _drawPosition = newPosition;
        }

        /// <summary>
        /// Updates the <see cref="Entity"/>'s greatest velocity.
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
    }
}