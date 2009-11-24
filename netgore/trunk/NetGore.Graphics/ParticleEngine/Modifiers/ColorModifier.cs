using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    public class ColorModifier : ParticleModifier
    {
        public Color ReleaseColor { get; set; }

        public Color UltimateColor { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorModifier"/> class.
        /// </summary>
        public ColorModifier()
            : base(true, true)
        {
            ReleaseColor = Color.White;
            ReleaseColor = Color.White;
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
            particle.Color = ReleaseColor;
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
            var agePercent = particle.GetAgePercent(CurrentTime);
            particle.Color = ReleaseColor.Lerp(UltimateColor, agePercent);
        }
    }
}
