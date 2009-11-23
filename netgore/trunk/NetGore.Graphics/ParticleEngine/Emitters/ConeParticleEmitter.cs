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
        /// Initializes a <see cref="Particle"/>.
        /// </summary>
        /// <param name="particleIndex">Index of the particle to initialize.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="offset">The position offset to release the particle from the origin.</param>
        /// <param name="releaseVelocity">The velocity vector to apply to the <see cref="Particle"/>.</param>
        protected override void InitializeParticle(int particleIndex, float speed, out Vector2 offset, out Vector2 releaseVelocity)
        {
            float f = ConeAngle * 0.5f;
            float radians = RandomHelper.NextFloat(Direction - f, Direction + f);

            offset = Vector2.Zero;

            GetVelocity(radians, speed, out releaseVelocity);
        }
    }
}