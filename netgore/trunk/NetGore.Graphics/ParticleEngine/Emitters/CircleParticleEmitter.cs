using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// A <see cref="ParticleEmitter"/> that emits particles from a circle.
    /// </summary>
    public class CircleParticleEmitter : ParticleEmitter
    {
        const string _emitterCategoryName = "Circle Emitter";

        /// <summary>
        /// Initializes a new instance of the <see cref="CircleParticleEmitter"/> class.
        /// </summary>
        public CircleParticleEmitter()
        {
            Radius = 50;
            IsRing = false;
            Radiate = false;
        }

        /// <summary>
        /// Gets or sets if <see cref="Particle"/>s are emitted as a ring. If true, <see cref="Particle"/>s will only
        /// be emitted on the perimeter of the circle.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("If Particles are emitter along the perimeter of the circle to form a ring.")]
        [DisplayName("IsRing")]
        [DefaultValue(false)]
        public bool IsRing { get; set; }

        /// <summary>
        /// Gets or sets if the <see cref="Particle"/>s will radiate away from the center of the circle. If false,
        /// they will just radiate out in a random direction.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("If true, Particles will radiate out from the center of the circle.")]
        [DisplayName("Radiate")]
        [DefaultValue(false)]
        public bool Radiate { get; set; }

        /// <summary>
        /// Gets or sets the radius of the circle.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("The size of the circle's radius.")]
        [DisplayName("Radius")]
        [DefaultValue(typeof(VariableFloat), "50")]
        public VariableFloat Radius { get; set; }

        /// <summary>
        /// Generates the offset vector to release the <see cref="Particle"/> at.
        /// </summary>
        /// <param name="offset">The offset vector.</param>
        protected override void GenerateParticleOffset(out Vector2 offset)
        {
            float rads = RandomHelper.NextFloat(MathHelper.TwoPi);

            var radius = Radius.GetNext();
            offset = new Vector2((float)Math.Cos(rads) * radius, (float)Math.Sin(rads) * radius);

            if (!IsRing)
                Vector2.Multiply(ref offset, RandomHelper.NextFloat(), out offset);
        }

        /// <summary>
        /// Generates the normalized force vector for releasing the <see cref="Particle"/>.
        /// </summary>
        /// <param name="offset">The offset vector that was acquired for this <see cref="Particle"/>.</param>
        /// <param name="force">The normalized force vector.</param>
        protected override void GenerateParticleForce(ref Vector2 offset, out Vector2 force)
        {
            if (Radiate)
                Vector2.Normalize(ref offset, out force);
            else
                base.GenerateParticleForce(ref offset, out force);
        }
    }
}