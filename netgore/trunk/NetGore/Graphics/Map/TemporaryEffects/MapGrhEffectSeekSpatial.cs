using System;
using System.Diagnostics;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// A <see cref="MapGrhEffect"/> that will seek out a specific position, then die once it reaches it.
    /// </summary>
    public class MapGrhEffectSeekPosition : MapGrhEffect
    {
        readonly Vector2 _startPosition;
        readonly Vector2 _targetPosition;
        readonly Vector2 _velocity;
        readonly TickCount _startTime;
        readonly TickCount _endTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapGrhEffectSeekPosition"/> class.
        /// </summary>
        /// <param name="grh">Grh to draw.</param>
        /// <param name="position">Position to draw on the map.</param>
        /// <param name="isForeground">If true, this will be drawn in the foreground layer. If false,
        /// it will be drawn in the background layer.</param>
        /// <param name="target">The destination position.</param>
        /// <param name="speed">How fast this object moves towards the target in pixels per second.</param>
        public MapGrhEffectSeekPosition(Grh grh, Vector2 position, bool isForeground, Vector2 target, float speed)
            : base(grh, position, isForeground)
        {
            speed = speed / 1000f;

            _startTime = TickCount.Now;
            _startPosition = position;
            _targetPosition = target;

            // Calculate the angle between the position and target
            Vector2 diff = position - target;
            double angle = Math.Atan2(diff.Y, diff.X);

            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            // Multiply the normalized direction vector (the cos and sine values) by the speed to get the velocity
            // to use for each elapsed millisecond
            _velocity = -new Vector2((float)(cos * speed), (float)(sin * speed));

            // Precalculate the amount of time it will take to hit the target. This way, we can check if we have reached
            // the destination simply by checking the current time.
            var dist = Vector2.Distance(position, target);
            int reqTime = (int)Math.Ceiling(dist / speed);

            _endTime = (TickCount)(_startTime + reqTime);
        }

        /// <summary>
        /// When overridden in the derived class, performs the additional updating that this <see cref="MapGrhEffect"/>
        /// needs to do such as checking if it is time to kill the effect. This method should be overridden instead of
        /// <see cref="MapGrh.Update"/>. This method will not be called after the effect has been killed.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        protected override void UpdateEffect(TickCount currentTime)
        {
            // Check if enough time has elapsed for us to reach the target position
            if (currentTime >= _endTime)
            {
                Position = _targetPosition;
                Kill();
                return;
            }

            // Recalculate the position based on the elapsed time. Recalculate the complete position each time since the computational
            // cost is pretty much the same, and this way doesn't fall victim to accumulated rounding errors
            var elapsedTime = currentTime - _startTime;
            Position = _startPosition + (_velocity * elapsedTime);
        }
    }

    /// <summary>
    /// A <see cref="MapGrhEffect"/> that will seek out a <see cref="ISpatial"/>, then die once it reaches it.
    /// </summary>
    public class MapGrhEffectSeekSpatial : MapGrhEffect
    {
        readonly ISpatial _target;
        readonly float _speed;

        TickCount _lastUpdate;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapGrhEffectLoopOnce"/> class.
        /// </summary>
        /// <param name="grh">Grh to draw.</param>
        /// <param name="position">Position to draw on the map.</param>
        /// <param name="isForeground">If true, this will be drawn in the foreground layer. If false,
        /// it will be drawn in the background layer.</param>
        /// <param name="target">The <see cref="ISpatial"/> to seek out.</param>
        /// <param name="speed">How fast this object moves towards the target in pixels per second.</param>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is null.</exception>
        public MapGrhEffectSeekSpatial(Grh grh, Vector2 position, bool isForeground, ISpatial target, float speed)
            : base(grh, position, isForeground)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            _lastUpdate = TickCount.Now;

            _target = target;
            _speed = speed / 1000f;
        }

        /// <summary>
        /// When overridden in the derived class, performs the additional updating that this <see cref="MapGrhEffect"/>
        /// needs to do such as checking if it is time to kill the effect. This method should be overridden instead of
        /// <see cref="MapGrh.Update"/>. This method will not be called after the effect has been killed.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        protected override void UpdateEffect(TickCount currentTime)
        {
            var elapsed = currentTime - _lastUpdate;
            _lastUpdate = currentTime;

            // Get the amount to move, which is the product of the speed and elapsed time
            var modSpeed = elapsed * _speed;

            // Calculate and set the new position
            Position = Position.MoveTowards(_target.Center, modSpeed);

            // Check if we are close enough to the target
            if (Position.QuickDistance(_target.Center) < 8)
                Kill();
        }
    }
}