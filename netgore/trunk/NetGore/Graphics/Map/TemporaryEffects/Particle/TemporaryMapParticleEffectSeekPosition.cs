using System;
using System.Linq;
using NetGore.Graphics.ParticleEngine;
using SFML.Graphics;

namespace NetGore.Graphics
{
    public class TemporaryMapParticleEffectSeekPosition : TemporaryMapParticleEffect
    {
        readonly TickCount _endTime;
        readonly Vector2 _startPosition;
        readonly TickCount _startTime;
        readonly Vector2 _targetPosition;
        readonly Vector2 _velocity;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemporaryMapParticleEffect"/> class.
        /// </summary>
        /// <param name="pe">The <see cref="IParticleEffect"/>.</param>
        /// <param name="target">The destination position.</param>
        /// <param name="speed">How fast this object moves towards the target in pixels per second.</param>
        public TemporaryMapParticleEffectSeekPosition(IParticleEffect pe, Vector2 target, float speed)
            : base(pe)
        {
            var position = pe.Position;

            speed = speed / 1000f;

            _startTime = TickCount.Now;
            _startPosition = position;
            _targetPosition = target;

            // Calculate the angle between the position and target
            var diff = position - target;
            var angle = Math.Atan2(diff.Y, diff.X);

            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);

            // Multiply the normalized direction vector (the cos and sine values) by the speed to get the velocity
            // to use for each elapsed millisecond
            _velocity = -new Vector2((float)(cos * speed), (float)(sin * speed));

            // Precalculate the amount of time it will take to hit the target. This way, we can check if we have reached
            // the destination simply by checking the current time.
            var dist = Vector2.Distance(position, target);
            var reqTime = (int)Math.Ceiling(dist / speed);

            _endTime = (TickCount)(_startTime + reqTime);
        }

        /// <summary>
        /// When overridden in the derived class, performs the additional updating that this <see cref="TemporaryMapParticleEffect"/>
        /// needs to do such as checking if it is time to kill the effect. This method will not be called after the effect has been killed.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        protected override void UpdateEffect(TickCount currentTime)
        {
            // Check if enough time has elapsed for us to reach the target position
            if (currentTime >= _endTime)
            {
                ParticleEffect.Position = _targetPosition;
                Kill(true);
                return;
            }

            // Recalculate the position based on the elapsed time. Recalculate the complete position each time since the computational
            // cost is pretty much the same, and this way doesn't fall victim to accumulated rounding errors
            var elapsedTime = currentTime - _startTime;
            ParticleEffect.Position = _startPosition + (_velocity * elapsedTime);
        }
    }
}