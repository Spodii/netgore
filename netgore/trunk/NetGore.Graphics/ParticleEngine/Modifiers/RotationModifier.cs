using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NetGore.Graphics.ParticleEngine
{
    public class RotationModifier : ParticleModifier
    {
        const string _categoryName = "Rotation Modifier";
        
        /// <summary>
        /// Default rotation rate. Using a literal instead of MathHelper.Pi since the latter doesn't work well
        /// with the DefaultValueAttribute.
        /// </summary>
        const float _defaultRate = 3.14159f;

        float _rate;

        /// <summary>
        /// Gets or sets the rate and direction of the rotation in radians per second.
        /// </summary>
        [Category(_categoryName)]
        [Description("The rate and direction of the rotation in radians per second.")]
        [DisplayName("Rate")]
        [DefaultValue(_defaultRate)]
        public float Rate { get { return _rate * 1000f; } set { _rate = value * 0.001f; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleModifier"/> class.
        /// </summary>
        public RotationModifier() : base(false, true)
        {
            Rate = _defaultRate;
        }

        /// <summary>
        /// When overridden in the derived class, handles processing the <paramref name="particle"/> when
        /// it is released. Only valid if <see cref="ParticleModifier.ProcessOnRelease"/> is set.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> that the <paramref name="particle"/>
        /// came from.</param>
        /// <param name="particle">The <see cref="Particle"/> to process.</param>
        protected override void HandleProcessReleased(ParticleEmitter emitter, Particle particle)
        {
        }

        /// <summary>
        /// When overridden in the derived class, handles processing the <paramref name="particle"/> when
        /// it is updated. Only valid if <see cref="ParticleModifier.ProcessOnUpdate"/> is set.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> that the <paramref name="particle"/>
        /// came from.</param>
        /// <param name="particle">The <see cref="Particle"/> to process.</param>
        /// <param name="elapsedTime">The amount of time that has elapsed since the <paramref name="emitter"/>
        /// was last updated.</param>
        protected override void HandleProcessUpdated(ParticleEmitter emitter, Particle particle, int elapsedTime)
        {
            particle.Rotation += _rate * elapsedTime;
        }
    }
}
