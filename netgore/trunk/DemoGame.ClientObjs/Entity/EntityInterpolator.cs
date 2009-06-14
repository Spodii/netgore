using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NetGore;
using NetGore.Extensions;

namespace DemoGame.Client
{
    public class EntityInterpolator
    {
        Vector2 _drawPosition;

        /// <summary>
        /// Gets the position to use when drawing the Entity.
        /// </summary>
        public Vector2 DrawPosition { get { return _drawPosition.Round(); } }

        int _lastTime;

        public void Update(Entity entity, int currentTime)
        {
            Vector2 position = entity.Position;

            if (position == _drawPosition)
                return;

            int elapsedTime = currentTime - _lastTime;
            _lastTime = currentTime;

            Vector2 velocity = entity.Velocity;
            Vector2 newPosition = _drawPosition + (velocity * elapsedTime);

            if (_drawPosition.X > position.X && newPosition.X < position.X)
                newPosition.X = position.X;
            else if (_drawPosition.X < position.X && newPosition.X > position.X)
                newPosition.X = position.X;

            if (_drawPosition.Y > position.Y && newPosition.Y < position.Y)
                newPosition.Y = position.Y;
            else if (_drawPosition.Y < position.Y && newPosition.Y > position.Y)
                newPosition.Y = position.Y;

            Vector2 diff = newPosition - position;
            const float teleDiff = 25f;
            if (diff.X > teleDiff || diff.X < teleDiff || diff.Y > teleDiff || diff.Y < teleDiff)
                newPosition = position;

            _drawPosition = newPosition;
        }
    }
}
