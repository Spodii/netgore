using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Applies a constant linear force to <see cref="Particle"/>s.
    /// </summary>
    public class LinearGravityModifier : ParticleModifier
    {
        const string _categoryName = "Linear Gravity Modifier";

        Vector2 _gravity = new Vector2(0, 100);

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleModifier"/> class.
        /// </summary>
        public LinearGravityModifier() : base(false, true)
        {
        }

        /// <summary>
        /// Gets or sets the gravity vector in units of force per second.
        /// </summary>
        [Category(_categoryName)]
        [Description("The gravitational force vector in units of force per second.")]
        [DisplayName("Gravity")]
        [DefaultValue(typeof(Vector2), "0, 100")]
        public Vector2 Gravity
        {
            get { return _gravity; }
            set { _gravity = value; }
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
            Vector2 deltaGrav;

            Vector2.Multiply(ref _gravity, elapsedTime * 0.001f, out deltaGrav);

            particle.ApplyForce(ref deltaGrav);
        }
    }
}