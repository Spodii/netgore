using System;
using System.Linq;
using NetGore.Graphics.ParticleEngine;

namespace NetGore.Graphics
{
    /// <summary>
    /// A <see cref="ITemporaryMapEffect"/> for a <see cref="IParticleEffect"/>. Simply displays a <see cref="IParticleEffect"/> at
    /// for a brief amount of time. Derived classes can override some methods to provide more advanced operations.
    /// </summary>
    public class TemporaryMapParticleEffect : ITemporaryMapEffect
    {
        readonly IParticleEffect _particleEffect;

        bool _isAlive = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemporaryMapParticleEffect"/> class.
        /// </summary>
        /// <param name="particleEffect">The <see cref="IParticleEffect"/>.</param>
        public TemporaryMapParticleEffect(IParticleEffect particleEffect)
        {
            _particleEffect = particleEffect;
        }

        /// <summary>
        /// Gets or sets if the effect will be killed automatically if the <see cref="ParticleEffect"/> runs out of live particles.
        /// Default value is false.
        /// </summary>
        protected bool AutoKillWhenNoParticles { get; set; }

        /// <summary>
        /// Gets the <see cref="IParticleEffect"/> used by this <see cref="TemporaryMapParticleEffect"/>.
        /// </summary>
        protected IParticleEffect ParticleEffect
        {
            get { return _particleEffect; }
        }

        /// <summary>
        /// When overridden in the derived class, performs the additional updating that this <see cref="TemporaryMapParticleEffect"/>
        /// needs to do. This method will not be called after the effect has been killed.
        /// </summary>
        /// <param name="currentTime">Current game time.</param>
        protected virtual void UpdateEffect(TickCount currentTime)
        {
        }

        #region ITemporaryMapEffect Members

        /// <summary>
        /// Notifies listeners when this <see cref="ITemporaryMapEffect"/> has died. This is only raised once per
        /// <see cref="ITemporaryMapEffect"/>, and is raised when <see cref="ITemporaryMapEffect.IsAlive"/> is set to false.
        /// </summary>
        public event TypedEventHandler<ITemporaryMapEffect> Died;

        /// <summary>
        /// Gets if this map effect is still alive. When false, it will be removed from the map. Once set to false, this
        /// value will remain false.
        /// </summary>
        public bool IsAlive
        {
            get { return _isAlive; }
        }

        public MapRenderLayer MapRenderLayer { get; set; }

        /// <summary>
        /// Makes the object draw itself.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> the object can use to draw itself with.</param>
        public void Draw(ISpriteBatch sb)
        {
            if (!IsAlive)
                return;

            _particleEffect.Draw(sb);
        }

        /// <summary>
        /// Forcibly kills the effect.
        /// </summary>
        /// <param name="immediate">If true, the particle effect will stop emitting and existing particles will be given time
        /// to expire.</param>
        public void Kill(bool immediate)
        {
            if (!IsAlive)
                return;

            _particleEffect.Kill();

            if (!immediate)
                return;

            _isAlive = false;

            if (Died != null)
                Died.Raise(this, EventArgs.Empty);
        }

        /// <summary>
        /// Updates the map effect.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        public void Update(TickCount currentTime)
        {
            if (!IsAlive)
                return;

            // Check if the effect died off
            if (_particleEffect.IsExpired)
            {
                Kill(true);
                return;
            }

            // Update the ParticleEffect
            _particleEffect.Update(currentTime);

            // Allow for the derived class to update its own logic
            UpdateEffect(currentTime);
        }

        /// <summary>
        /// Checks if in the object is in view of the specified <paramref name="camera"/>.
        /// </summary>
        /// <param name="camera">The <see cref="ICamera2D"/> to check if this object is in view of.</param>
        /// <returns>True if the object is in view of the camera, else False.</returns>
        public bool InView(ICamera2D camera)
        {
            var pe = ParticleEffect;
            if (pe == null)
                return false;

            return pe.InView(camera);
        }

        #endregion
    }
}