using System;
using SFML.Graphics;

namespace NetGore.Graphics
{
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

        bool _hasReachedTarget = false;

        /// <summary>
        /// Updates the <see cref="MapGrh"/>.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        public override void Update(TickCount currentTime)
        {
            base.Update(currentTime);

            var elapsed = currentTime - _lastUpdate;
            _lastUpdate = currentTime;

            var modSpeed = elapsed * _speed;

            // Move towards the target
            var diff = Center - _target.Center;
            var angle = Math.Atan2(diff.Y, diff.X);

            var offset = new Vector2((float)(Math.Cos(angle) * modSpeed), (float)(Math.Sin(angle) * modSpeed));
            var newCenter = Center + offset;

            // Make sure we do not overshoot
            if (offset.X > 0)
            {
                if (newCenter.X > _target.Center.X)
                    newCenter.X = _target.Center.X;
            }
            else
            {
                if (newCenter.X < _target.Center.X)
                    newCenter.X = _target.Center.X;
            }

            if (offset.Y > 0)
            {
                if (newCenter.Y > _target.Center.Y)
                    newCenter.Y = _target.Center.Y;
            }
            else
            {
                if (newCenter.Y < _target.Center.Y)
                    newCenter.Y = _target.Center.Y;
            }

            // Set the new position
            Position = newCenter - (Size / 2f);

            // Check if we are close enough to the target
            if (Center.QuickDistance(_target.Center) < 8)
                _hasReachedTarget = true;
        }

        /// <summary>
        /// Gets if this map effect is still alive. When false, it will be removed from the map.
        /// </summary>
        public override bool IsAlive
        {
            get { return _hasReachedTarget; }
        }
    }
}