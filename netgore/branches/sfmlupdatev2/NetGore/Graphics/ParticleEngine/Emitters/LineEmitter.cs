using System;
using System.ComponentModel;
using System.Linq;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// A <see cref="ParticleEmitter"/> that emits particles from a line.
    /// </summary>
    public class LineEmitter : ParticleEmitter
    {
        const string _angleKeyName = "Angle";
        const int _defaultAngle = 0;
        const bool _defaultEmitBothWays = true;
        const int _defaultLength = 100;
        const bool _defaultRectilinear = false;
        const string _emitBothWaysKeyName = "EmitBothWays";
        const string _emitterCategoryName = "Line Emitter";
        const string _lengthKeyName = "Length";
        const string _rectilinearKeyName = "Rectilinear";

        bool _emitBothWays = _defaultEmitBothWays;

        /// <summary>
        /// Used when <see cref="EmitBothWays"/> is set to alter which direction a particle is emitted.
        /// Value is flipped for each <see cref="Particle"/>.
        /// </summary>
        bool _flip;

        int _length = _defaultLength;
        bool _rectilinear = _defaultRectilinear;
        Matrix _rotationMatrix = Matrix.CreateRotationZ(_defaultAngle);

        /// <summary>
        /// Initializes a new instance of the <see cref="LineEmitter"/> class.
        /// </summary>
        /// <param name="owner">The <see cref="IParticleEffect"/> that owns this <see cref="IParticleEmitter"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="owner"/> is null.</exception>
        public LineEmitter(IParticleEffect owner) : base(owner)
        {
        }

        /// <summary>
        /// Gets or sets the rotation in radians of the line around its center point.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("The rotation in radians of the line around its center point.")]
        [DisplayName("Angle")]
        [DefaultValue(_defaultAngle)]
        public float Angle
        {
            get { return (float)Math.Atan2(_rotationMatrix.M12, _rotationMatrix.M11); }
            set { _rotationMatrix = Matrix.CreateRotationZ(value); }
        }

        /// <summary>
        /// Gets or sets if <see cref="Particle"/>s are emitted in both directions from the line. Only valid if
        /// <see cref="Rectilinear"/> is set.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("If particles are emitted in both directions from the line. Only valid if rectilinear is set.")]
        [DisplayName("Rectilinear")]
        [DefaultValue(_defaultEmitBothWays)]
        public bool EmitBothWays
        {
            get { return _emitBothWays; }
            set { _emitBothWays = value; }
        }

        /// <summary>
        /// Gets or sets the length of the line.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><c>value</c> is out of range.</exception>
        [Category(_emitterCategoryName)]
        [Description("The length of the line.")]
        [DisplayName("Length")]
        [DefaultValue(_defaultLength)]
        public int Length
        {
            get { return _length; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");

                _length = value;
            }
        }

        /// <summary>
        /// Gets or sets if <see cref="Particle"/>s are emitted only in the direction of the line's sides.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("If particles are emitted only in the direction of the line's sides.")]
        [DisplayName("Rectilinear")]
        [DefaultValue(_defaultRectilinear)]
        public bool Rectilinear
        {
            get { return _rectilinear; }
            set { _rectilinear = value; }
        }

        /// <summary>
        /// Creates a deep copy of this <see cref="ParticleEmitter"/> instance.
        /// </summary>
        /// <returns>A deep copy of this <see cref="ParticleEmitter"/>.</returns>
        public override ParticleEmitter DeepCopy(IParticleEffect newOwner)
        {
            var ret = new LineEmitter(newOwner);
            CopyValuesTo(ret);
            ret.Angle = Angle;
            ret.EmitBothWays = EmitBothWays;
            ret.Length = Length;
            ret.Rectilinear = Rectilinear;
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
            var halfLength = Length * 0.5f;

            offset = new Vector2(RandomHelper.NextFloat(-halfLength, halfLength), 0);

            Vector2.Transform(ref offset, ref _rotationMatrix, out offset);

            if (Rectilinear)
            {
                force = new Vector2(_rotationMatrix.Up.X, _rotationMatrix.Up.Y);

                if (EmitBothWays)
                {
                    if (_flip)
                        Vector2.Multiply(ref force, -1.0f, out force);

                    _flip = !_flip;
                }
            }
            else
                GetForce(RandomHelper.NextFloat(MathHelper.TwoPi), out force);
        }

        /// <summary>
        /// When overridden in the derived class, resets the variables for the <see cref="ParticleEmitter"/> in the derived
        /// class to make it like this instance is starting over from the start. This only resets state variables such as
        /// the time the effect was created and how long it has to live, not properties such as position and emitting style.
        /// </summary>
        protected override void HandleReset()
        {
            _flip = false;

            base.HandleReset();
        }

        /// <summary>
        /// When overridden in the derived class, reads all custom state values from the <paramref name="reader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the state values from.</param>
        protected override void ReadCustomValues(IValueReader reader)
        {
            Angle = reader.ReadFloat(_angleKeyName);
            EmitBothWays = reader.ReadBool(_emitBothWaysKeyName);
            Length = reader.ReadInt(_lengthKeyName);
            Rectilinear = reader.ReadBool(_rectilinearKeyName);
        }

        /// <summary>
        /// When overridden in the derived class, writes all custom state values to the <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the state values to.</param>
        protected override void WriteCustomValues(IValueWriter writer)
        {
            writer.Write(_angleKeyName, Angle);
            writer.Write(_emitBothWaysKeyName, EmitBothWays);
            writer.Write(_lengthKeyName, Length);
            writer.Write(_rectilinearKeyName, Rectilinear);
        }
    }
}