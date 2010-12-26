using System;
using System.ComponentModel;
using System.Linq;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore.Graphics.ParticleEngine.Modifiers
{
    /// <summary>
    /// A modifier that pulls <see cref="Particle"/>s in range towards it like a gravity well.
    /// </summary>
    public class ParticleRadialGravityModifier : ParticleModifier
    {
        const string _categoryName = "Radial Gravity Modifier";
        const float _defaultRadius = 250f;
        const float _defaultStrength = 250f;
        const string _positionKeyName = "Position";
        const string _radiusKeyName = "Radius";
        const string _strengthKeyName = "Strength";
        Vector2 _position = Vector2.Zero;

        float _radius;
        float _radiusSquared;
        float _strength;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleRadialGravityModifier"/> class.
        /// </summary>
        public ParticleRadialGravityModifier() : base(false, true)
        {
            Strength = _defaultStrength;
            Radius = _defaultRadius;
        }

        /// <summary>
        /// Gets or sets the position of the center of the gravity well relative to the emitter's origin.
        /// </summary>
        [Category(_categoryName)]
        [Description("The position of the center of the gravity well relative to the emitter's origin.")]
        [DisplayName("Position")]
        [DefaultValue(typeof(Vector2), "0, 0")]
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Gets or sets the radius of the gravity well.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><c>value</c> is out of range.</exception>
        [Category(_categoryName)]
        [Description("The radius of the gravity well.")]
        [DisplayName("Radius")]
        [DefaultValue(_defaultRadius)]
        public float Radius
        {
            get { return _radius; }
            set
            {
                if (value <= float.Epsilon)
                    throw new ArgumentOutOfRangeException("value");

                _radius = value;
                _radiusSquared = _radius * _radius;
            }
        }

        /// <summary>
        /// Gets or sets the gravitational strength of the gravity well.
        /// </summary>
        [Category(_categoryName)]
        [Description("The gravitational strength of the gravity well.")]
        [DisplayName("Strength")]
        [DefaultValue(_defaultStrength)]
        public float Strength
        {
            get { return _strength * 1000f; }
            set { _strength = value * 0.001f; }
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
            // Get the absolute position
            Vector2 absolutePosition;
            emitter.GetAbsoultePosition(ref _position, out absolutePosition);

            // Get the distance from the particle
            Vector2 distance;
            Vector2.Subtract(ref absolutePosition, ref particle.Position, out distance);

            // Check if the particle is in range
            if (distance.LengthSquared() >= _radiusSquared)
                return;

            // Normalize the distance vector to get the force
            Vector2 force;
            Vector2.Normalize(ref distance, out force);

            // Adjust the force by the strength and elapsed time
            Vector2.Multiply(ref force, _strength * elapsedTime, out force);

            // Apply the force to the particle
            particle.ApplyForce(ref force);
        }

        /// <summary>
        /// Reads the <see cref="ParticleModifier"/>'s custom values from the <see cref="reader"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read the custom values from.</param>
        protected override void ReadCustomValues(IValueReader reader)
        {
            Position = reader.ReadVector2(_positionKeyName);
            Radius = reader.ReadFloat(_radiusKeyName);
            Strength = reader.ReadFloat(_strengthKeyName);
        }

        /// <summary>
        /// When overridden in the derived class, writes all custom state values to the <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the state values to.</param>
        protected override void WriteCustomValues(IValueWriter writer)
        {
            writer.Write(_positionKeyName, Position);
            writer.Write(_radiusKeyName, Radius);
            writer.Write(_strengthKeyName, Strength);
        }
    }
}