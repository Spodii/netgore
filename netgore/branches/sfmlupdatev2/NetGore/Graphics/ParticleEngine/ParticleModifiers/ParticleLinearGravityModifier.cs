using System.ComponentModel;
using System.Linq;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Applies a constant linear force to <see cref="Particle"/>s.
    /// </summary>
    public class ParticleLinearGravityModifier : ParticleModifier
    {
        const string _categoryName = "Linear Gravity Modifier";
        const string _gravityKeyName = "Gravity";

        Vector2 _gravity = new Vector2(0, 100);

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleLinearGravityModifier"/> class.
        /// </summary>
        public ParticleLinearGravityModifier() : base(false, true)
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

        /// <summary>
        /// Reads the <see cref="ParticleModifier"/>'s custom values from the <see cref="reader"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read the custom values from.</param>
        protected override void ReadCustomValues(IValueReader reader)
        {
            Gravity = reader.ReadVector2(_gravityKeyName);
        }

        /// <summary>
        /// When overridden in the derived class, writes all custom state values to the <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the state values to.</param>
        protected override void WriteCustomValues(IValueWriter writer)
        {
            writer.Write(_gravityKeyName, Gravity);
        }
    }
}