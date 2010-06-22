using System.Diagnostics;
using System.Linq;
using NetGore.Graphics.ParticleEngine;

namespace NetGore.Graphics
{
    /// <summary>
    /// A <see cref="ITemporaryMapEffect"/> for a <see cref="ParticleEmitter"/>. Simply displays a <see cref="ParticleEmitter"/> at
    /// for a brief amount of time. Derived classes can override some methods to provide more advanced operations.
    /// </summary>
    public class MapParticleEffect : ITemporaryMapEffect
    {
        readonly ParticleEmitter _emitter;
        readonly bool _isForeground;

        bool _killed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapParticleEffect"/> class.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/>.</param>
        /// <param name="isForeground">If true, this will be drawn in the foreground layer. If false,
        /// it will be drawn in the background layer.</param>
        public MapParticleEffect(ParticleEmitter emitter, bool isForeground)
        {
            _isForeground = isForeground;
            _emitter = emitter;
        }

        /// <summary>
        /// Gets or sets if the effect will be killed automatically if the <see cref="Emitter"/> runs out of live particles.
        /// Default value is false.
        /// </summary>
        protected bool AutoKillWhenNoParticles { get; set; }

        /// <summary>
        /// Gets the <see cref="ParticleEmitter"/> used by this <see cref="MapParticleEffect"/>.
        /// </summary>
        protected ParticleEmitter Emitter
        {
            get { return _emitter; }
        }

        /// <summary>
        /// Kills this <see cref="MapParticleEffect"/>. This should happen once and only once for every <see cref="MapParticleEffect"/>.
        /// </summary>
        protected void Kill()
        {
            if (_killed)
            {
                const string errmsg = "Tried to kill dead MapParticleEffcet `{0}`. This should only be called once.";
                Debug.Fail(string.Format(errmsg, this));
                return;
            }

            _killed = true;

            _emitter.Kill();

            if (Died != null)
                Died(this);
        }

        /// <summary>
        /// When overridden in the derived class, performs the additional updating that this <see cref="MapParticleEffect"/>
        /// needs to do such as checking if it is time to kill the effect. This method will not be called after the effect has been killed.
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
        public event TemporaryMapEffectDiedHandler Died;

        /// <summary>
        /// Gets if this map effect is still alive. When false, it will be removed from the map. Once set to false, this
        /// value will remain false.
        /// </summary>
        public bool IsAlive
        {
            get { return !_killed || _emitter.ActiveParticles > 0; }
        }

        /// <summary>
        /// Gets if the <see cref="ITemporaryMapEffect"/> is in the foreground. If true, it will be drawn after the
        /// <see cref="MapRenderLayer.SpriteForeground"/> layer. If false, it will be drawn after the
        /// <see cref="MapRenderLayer.SpriteBackground"/> layer.
        /// </summary>
        public bool IsForeground
        {
            get { return _isForeground; }
        }

        /// <summary>
        /// Makes the object draw itself.
        /// </summary>
        /// <param name="sb"><see cref="ISpriteBatch"/> the object can use to draw itself with.</param>
        public void Draw(ISpriteBatch sb)
        {
            _emitter.Draw(sb);
        }

        /// <summary>
        /// Updates the map effect.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        public void Update(TickCount currentTime)
        {
            if (!IsAlive)
                return;

            _emitter.Update(currentTime);

            if (!_killed)
                UpdateEffect(currentTime);
        }

        #endregion
    }
}