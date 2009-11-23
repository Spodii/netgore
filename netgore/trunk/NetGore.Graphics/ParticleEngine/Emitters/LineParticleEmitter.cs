using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// A <see cref="ParticleEmitter"/> that emits particles from a line.
    /// </summary>
    public class LineParticleEmitter : ParticleEmitter
    {
        const string _emitterCategoryName = "Line Emitter";

        const int _defaultLength = 100;
        const int _defaultAngle = 0;

        private Matrix RotationMatrix = Matrix.CreateRotationZ(0f);

        int _length;

        /// <summary>
        /// Gets or sets the rotation in radians of the line around its center point.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("The rotation in radians of the line around its center point.")]
        [DisplayName("Angle")]
        [DefaultValue(_defaultAngle)]
        public float Angle
        {
            get { return (float)Math.Atan2(RotationMatrix.M12, RotationMatrix.M11); }
            set { RotationMatrix = Matrix.CreateRotationZ(value); }
        }

        public bool Rectilinear;

        public bool EmitBothWays;

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

        private bool flip;

        /// <summary>
        /// Initializes a new instance of the <see cref="LineParticleEmitter"/> class.
        /// </summary>
        public LineParticleEmitter()
        {
            Length = _defaultLength;
        }
    }
}
