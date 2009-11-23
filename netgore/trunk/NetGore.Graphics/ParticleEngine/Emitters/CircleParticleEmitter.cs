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
        const bool _defaultPerimeter = false;
        const bool _defaultRadiate = false;
        const string _emitterCategoryName = "Circle Emitter";

        bool _perimeter = _defaultPerimeter;
        bool _radiate = _defaultRadiate;
        VariableFloat _radius = 50;

        /// <summary>
        /// Gets or sets if <see cref="Particle"/>s are emitted only from the perimeter. If true,
        /// <see cref="Particle"/>s will only be emitted on the perimeter of the circle.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("If Particles are emitter along the perimeter of the circle to form a ring.")]
        [DisplayName("Perimeter")]
        [DefaultValue(_defaultPerimeter)]
        public bool Perimeter
        {
            get { return _perimeter; }
            set { _perimeter = value; }
        }

        /// <summary>
        /// Gets or sets if the <see cref="Particle"/>s will radiate away from the center of the circle. If false,
        /// they will just radiate out in a random direction.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("If true, Particles will radiate out from the center of the circle.")]
        [DisplayName("Radiate")]
        [DefaultValue(_defaultRadiate)]
        public bool Radiate
        {
            get { return _radiate; }
            set { _radiate = value; }
        }

        /// <summary>
        /// Gets or sets the radius of the circle.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("The size of the circle's radius.")]
        [DisplayName("Radius")]
        [DefaultValue(typeof(VariableFloat), "50")]
        public VariableFloat Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        /// <summary>
        /// Generates the offset and normalized force vectors to release the <see cref="Particle"/> at.
        /// </summary>
        /// <param name="particle">The <see cref="Particle"/> that the values are being generated for.</param>
        /// <param name="offset">The offset vector.</param>
        /// <param name="force">The normalized force vector.</param>
        protected override void GenerateParticleOffsetAndForce(Particle particle, out Vector2 offset, out Vector2 force)
        {
            float rads = RandomHelper.NextFloat(MathHelper.TwoPi);
            var radius = Radius.GetNext();

            float cosRads = (float)Math.Cos(rads);
            float sinRads = (float)Math.Sin(rads);
            offset = new Vector2(cosRads * radius, sinRads * radius);

            if (!Perimeter)
                Vector2.Multiply(ref offset, RandomHelper.NextFloat(), out offset);

            if (Radiate)
                force = new Vector2(cosRads, sinRads);
            else
                GetForce(RandomHelper.NextFloat(MathHelper.TwoPi), out force);
        }
    }
}