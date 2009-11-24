using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NetGore.Graphics.ParticleEngine
{
    public class LinearGravityModifier : ParticleModifier
    {
        Vector2 _gravity;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleModifier"/> class.
        /// </summary>
        public LinearGravityModifier() : base(false, true)
        {
        }

        protected override void HandleProcessReleased(ParticleEmitter emitter, Particle particle)
        {
        }

        /// <summary>
        /// Gets or sets the gravity vector.
        /// </summary>
        public Vector2 Gravity { get { return _gravity; } set { _gravity = value; } }

        protected override void HandleProcessUpdated(ParticleEmitter emitter, Particle particle, int elapsedTime)
        {
            Vector2 deltaGrav;

            Vector2.Multiply(ref _gravity, elapsedTime * 0.001f, out deltaGrav);

            particle.ApplyForce(ref deltaGrav);
        }
    }
}
