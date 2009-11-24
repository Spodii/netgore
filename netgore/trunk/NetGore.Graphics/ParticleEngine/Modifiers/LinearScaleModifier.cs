using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Scales the <see cref="Particle"/>s linearly from one value to another over the life of the
    /// <see cref="Particle"/>.
    /// </summary>
    public class LinearScaleModifier : ParticleModifier
    {
        const string _categoryName = "Linear Scale Modifier";
        const float _defaultInitialScale = 1.0f;
        const float _defaultUltimateScale = 2.0f;

        /// <summary>
        /// Gets or sets the initial scale value.
        /// </summary>
        [Category(_categoryName)]
        [Description("The initial scale value.")]
        [DisplayName("Initial Scale")]
        [DefaultValue(_defaultInitialScale)]
        public float InitialScale { get; set; }

        /// <summary>
        /// Gets or sets the ending scale value.
        /// </summary>
        [Category(_categoryName)]
        [Description("The final scale value.")]
        [DisplayName("Ultimate Scale")]
        [DefaultValue(_defaultUltimateScale)]
        public float UltimateScale { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleModifier"/> class.
        /// </summary>
        public LinearScaleModifier() : base(false, true)
        {
            InitialScale = _defaultInitialScale;
            UltimateScale = _defaultUltimateScale;
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
            particle.Scale = MathHelper.Lerp(InitialScale, UltimateScale, particle.GetAgePercent(CurrentTime));
        }
    }
}
