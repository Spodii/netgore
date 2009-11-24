using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    public class ParticleColorModifier : ParticleModifierBase
    {
        public Color ReleaseColor { get; set; }

        public Color UltimateColor { get; set; }

        public ParticleColorModifier()
            : base(true, true)
        {
            ReleaseColor = Color.White;
            ReleaseColor = Color.White;
        }

        /// <summary>
        /// When overridden in the derived class, handles processing the <paramref name="particle"/> when
        /// it is released. Only valid if <see cref="ParticleModifierBase.ProcessOnRelease"/> is set.
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
        /// it is updated. Only valid if <see cref="ParticleModifierBase.ProcessOnUpdate"/> is set.
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
