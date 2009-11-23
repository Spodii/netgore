using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// A <see cref="ConeParticleEmitter"/> that emits particles in the form of a cone.
    /// </summary>
    class ConeParticleEmitter : ParticleEmitter
    {
        const float _defaultConeAngle = MathHelper.PiOver2;
        const float _defaultDirection = 0;
        const string _emitterCategoryName = "Cone Emitter";

        /// <summary>
        /// Initializes a new instance of the <see cref="ConeParticleEmitter"/> class.
        /// </summary>
        public ConeParticleEmitter()
        {
            ConeAngle = _defaultConeAngle;
            Direction = _defaultDirection;
        }

        /// <summary>
        /// Gets or sets the angle (in radians) from edge to edge of the emitter beam.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("The angle (in radians) from edge to edge of the emitter beam.")]
        [DisplayName("Angle")]
        [DefaultValue(_defaultConeAngle)]
        public float ConeAngle { get; set; }

        /// <summary>
        /// Gets or sets the angle (in radians) that the emitter is facing.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("The angle (in radians) that the emitter is facing.")]
        [DisplayName("Direction")]
        [DefaultValue(_defaultDirection)]
        public float Direction { get; set; }

        /// <summary>
        /// Generates the offset vector to release the <see cref="Particle"/> at.
        /// </summary>
        /// <param name="offset">The offset vector.</param>
        protected override void GenerateParticleOffset(out Vector2 offset)
        {
            offset = Vector2.Zero;
        }

        /// <summary>
        /// Generates the normalized force vector for releasing the <see cref="Particle"/>.
        /// </summary>
        /// <param name="offset">The offset vector that was acquired for this <see cref="Particle"/>.</param>
        /// <param name="force">The normalized force vector.</param>
        protected override void GenerateParticleForce(ref Vector2 offset, out Vector2 force)
        {
            float f = ConeAngle * 0.5f;
            float radians = RandomHelper.NextFloat(Direction - f, Direction + f);

            GetForce(radians, out force);
        }
    }
}