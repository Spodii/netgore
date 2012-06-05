using System;
using System.ComponentModel;
using System.Linq;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// A <see cref="ConeEmitter"/> that emits particles in the form of a cone.
    /// </summary>
    public class ConeEmitter : ParticleEmitter
    {
        const string _coneAngleKeyName = "ConeAngle";
        const float _defaultConeAngle = MathHelper.PiOver2;
        const float _defaultDirection = 0;
        const string _directionKeyName = "Direction";
        const string _emitterCategoryName = "Cone Emitter";

        float _coneAngle = _defaultConeAngle;
        float _direction = _defaultDirection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConeEmitter"/> class.
        /// </summary>
        /// <param name="owner">The <see cref="IParticleEffect"/> that owns this <see cref="IParticleEmitter"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="owner"/> is null.</exception>
        public ConeEmitter(IParticleEffect owner) : base(owner)
        {
        }

        /// <summary>
        /// Gets or sets the angle (in radians) from edge to edge of the emitter beam.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("The angle (in radians) from edge to edge of the emitter beam.")]
        [DisplayName("Angle")]
        [DefaultValue(_defaultConeAngle)]
        public float ConeAngle
        {
            get { return _coneAngle; }
            set { _coneAngle = value; }
        }

        /// <summary>
        /// Gets or sets the angle (in radians) that the emitter is facing.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("The angle (in radians) that the emitter is facing.")]
        [DisplayName("Direction")]
        [DefaultValue(_defaultDirection)]
        public float Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        /// <summary>
        /// Creates a deep copy of this <see cref="ParticleEmitter"/> instance.
        /// </summary>
        /// <returns>A deep copy of this <see cref="ParticleEmitter"/>.</returns>
        public override ParticleEmitter DeepCopy(IParticleEffect newOwner)
        {
            var ret = new ConeEmitter(newOwner);
            CopyValuesTo(ret);
            ret.ConeAngle = ConeAngle;
            ret.Direction = Direction;
            return ret;
        }

        /// <summary>
        /// When overridden in the derived class, generates the offset and normalized force vectors to
        /// release the <see cref="Particle"/> at.
        /// </summary>
        /// <param name="particle">The <see cref="Particle"/> that the values are being generated for.</param>
        /// <param name="offset">The offset vector.</param>
        /// <param name="force">The normalized force vector.</param>
        protected override void GenerateParticleOffsetAndForce(Particle particle, out Vector2 offset, out Vector2 force)
        {
            offset = Vector2.Zero;

            var f = ConeAngle * 0.5f;
            var radians = RandomHelper.NextFloat(Direction - f, Direction + f);

            GetForce(radians, out force);
        }

        /// <summary>
        /// When overridden in the derived class, reads all custom state values from the <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the state values from.</param>
        protected override void ReadCustomValues(IValueReader reader)
        {
            ConeAngle = reader.ReadFloat(_coneAngleKeyName);
            Direction = reader.ReadFloat(_directionKeyName);
        }

        /// <summary>
        /// When overridden in the derived class, writes all custom state values to the <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the state values to.</param>
        protected override void WriteCustomValues(IValueWriter writer)
        {
            writer.Write(_coneAngleKeyName, ConeAngle);
            writer.Write(_directionKeyName, Direction);
        }
    }
}