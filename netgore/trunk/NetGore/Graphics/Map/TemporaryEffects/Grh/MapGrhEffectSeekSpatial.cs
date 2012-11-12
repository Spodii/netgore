using System;
using System.Linq;
using NetGore.World;
using SFML.Graphics;

namespace NetGore.Graphics
{
    /// <summary>
    /// A <see cref="MapGrhEffect"/> that will seek out a <see cref="ISpatial"/>, then die once it reaches it.
    /// </summary>
    public class MapGrhEffectSeekSpatial : MapGrhEffect
    {
        readonly float _speed;
        readonly ISpatial _target;

        TickCount _lastUpdate;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapGrhEffectLoopOnce"/> class.
        /// </summary>
        /// <param name="grh">Grh to draw.</param>
        /// <param name="position">Position to draw on the map.</param>
        /// <param name="target">The <see cref="ISpatial"/> to seek out.</param>
        /// <param name="speed">How fast this object moves towards the target in pixels per second.</param>
        /// <exception cref="ArgumentNullException"><paramref name="target"/> is null.</exception>
        public MapGrhEffectSeekSpatial(Grh grh, Vector2 position, ISpatial target, float speed)
            : base(grh, position)
        {
            if (target == null)
                throw new ArgumentNullException("target");

            _lastUpdate = TickCount.Now;

            _target = target;
            _speed = speed / 1000f;
        }

        /// <summary>
        /// Forcibly kills the effect.
        /// </summary>
        /// <param name="immediate">If false, the effect will continue to run until the target is reached.</param>
        public override void Kill(bool immediate)
        {
            if (!immediate)
                return;

            base.Kill(immediate);
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
                Kill(true);
        }
    }
}