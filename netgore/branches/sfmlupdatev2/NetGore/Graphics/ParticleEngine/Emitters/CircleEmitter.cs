using System;
using System.ComponentModel;
using System.Linq;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// A <see cref="ParticleEmitter"/> that emits particles from a circle.
    /// </summary>
    public class CircleEmitter : ParticleEmitter
    {
        const bool _defaultPerimeter = false;
        const bool _defaultRadiate = false;
        const string _emitterCategoryName = "Circle Emitter";
        const string _perimeterKeyName = "Perimeter";
        const string _radiateKeyName = "Radiate";
        const string _radiusKeyName = "Radius";

        bool _perimeter = _defaultPerimeter;
        bool _radiate = _defaultRadiate;
        VariableFloat _radius = 50;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircleEmitter"/> class.
        /// </summary>
        /// <param name="owner">The <see cref="IParticleEffect"/> that owns this <see cref="IParticleEmitter"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="owner"/> is null.</exception>
        public CircleEmitter(IParticleEffect owner) : base(owner)
        {
        }

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
        /// Creates a deep copy of this <see cref="ParticleEmitter"/> instance.
        /// </summary>
        /// <returns>A deep copy of this <see cref="ParticleEmitter"/>.</returns>
        public override ParticleEmitter DeepCopy(IParticleEffect newOwner)
        {
            var ret = new CircleEmitter(newOwner);
            CopyValuesTo(ret);
            ret.Perimeter = Perimeter;
            ret.Radiate = Radiate;
            ret.Radius = Radius;
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
            var rads = RandomHelper.NextFloat(MathHelper.TwoPi);
            var radius = Radius.GetNext();

            var cosRads = (float)Math.Cos(rads);
            var sinRads = (float)Math.Sin(rads);
            offset = new Vector2(cosRads * radius, sinRads * radius);

            if (!Perimeter)
                Vector2.Multiply(ref offset, RandomHelper.NextFloat(), out offset);

            if (Radiate)
                force = new Vector2(cosRads, sinRads);
            else
                GetForce(RandomHelper.NextFloat(MathHelper.TwoPi), out force);
        }

        /// <summary>
        /// When overridden in the derived class, reads all custom state values from the <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the state values from.</param>
        protected override void ReadCustomValues(IValueReader reader)
        {
            Perimeter = reader.ReadBool(_perimeterKeyName);
            Radiate = reader.ReadBool(_radiateKeyName);
            Radius = reader.ReadVariableFloat(_radiusKeyName);
        }

        /// <summary>
        /// When overridden in the derived class, writes all custom state values to the <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the state values to.</param>
        protected override void WriteCustomValues(IValueWriter writer)
        {
            writer.Write(_perimeterKeyName, Perimeter);
            writer.Write(_radiateKeyName, Radiate);
            writer.Write(_radiusKeyName, Radius);
        }
    }
}