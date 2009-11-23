using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore.Graphics.ParticleEngine
{
    public class CircleParticleEmitter : ParticleEmitter
    {
        const string _emitterCategoryName = "Circle Emitter";

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
        public VariableFloat Radius
        {
        get;set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CircleParticleEmitter"/> class.
        /// </summary>
        public CircleParticleEmitter()
        {
            Radius = 50;
            IsRing = true;
            Radiate = false;
        }

        /// <summary>
        /// Initializes a <see cref="Particle"/>.
        /// </summary>
        /// <param name="particleIndex">Index of the particle to initialize.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="offset">The position offset to release the particle from the origin.</param>
        /// <param name="releaseVelocity">The velocity vector to apply to the <see cref="Particle"/>.</param>
        protected override void InitializeParticle(int particleIndex, float speed, out Vector2 offset,
                                                   out Vector2 releaseVelocity)
        {
            var radius = Radius.GetNext();
            float radians = RandomHelper.NextFloat(MathHelper.TwoPi);

            offset = new Vector2((float)Math.Cos(radians) * radius, (float)Math.Sin(radians) * radius);

            if (!IsRing)
                Vector2.Multiply(ref offset, RandomHelper.NextFloat(), out offset);

            if (Radiate)
            {
                Vector2.Normalize(ref offset, out releaseVelocity);
                Vector2.Multiply(ref releaseVelocity, speed, out releaseVelocity);
            }
            else
            {
                GetVelocity(RandomHelper.NextFloat(MathHelper.TwoPi), speed, out releaseVelocity);
            }
        }
    }
}