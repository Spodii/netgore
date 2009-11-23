using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// A <see cref="ParticleEmitter"/> that emits particles from a line.
    /// </summary>
    public class LineParticleEmitter : ParticleEmitter
    {
        const int _defaultAngle = 0;
        const bool _defaultEmitBothWays = true;
        const int _defaultLength = 100;
        const bool _defaultRectilinear = false;
        const string _emitterCategoryName = "Line Emitter";

        /// <summary>
        /// Used when <see cref="EmitBothWays"/> is set to alter which direction a particle is emitted.
        /// Value is flipped for each <see cref="Particle"/>.
        /// </summary>
        bool _flip;

        bool _emitBothWays = _defaultEmitBothWays;
        int _length = _defaultLength;
        bool _rectilinear = _defaultRectilinear;
        Matrix _rotationMatrix = Matrix.CreateRotationZ(_defaultAngle);

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
        public bool EmitBothWays { get { return _emitBothWays; } set { _emitBothWays = value; } }

        /// <summary>
        /// Gets or sets the length of the line.
        /// </summary>
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
        public bool Rectilinear { get { return _rectilinear; } set { _rectilinear = value; } }

        /// <summary>
        /// Generates the offset and normalized force vectors to release the <see cref="Particle"/> at.
        /// </summary>
        /// <param name="particle">The <see cref="Particle"/> that the values are being generated for.</param>
        /// <param name="offset">The offset vector.</param>
        /// <param name="force">The normalized force vector.</param>
        protected override void GenerateParticleOffsetAndForce(Particle particle, out Vector2 offset, out Vector2 force)
        {
            float halfLength = Length * 0.5f;

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
    }
}