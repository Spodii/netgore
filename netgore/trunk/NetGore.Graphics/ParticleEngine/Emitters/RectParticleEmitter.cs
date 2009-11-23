using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// A <see cref="ParticleEmitter"/> that emits particles from a rectangle.
    /// </summary>
    public class RectParticleEmitter : ParticleEmitter
    {
        const int _defaultHeight = 100;
        const bool _defaultPerimeter = false;
        const int _defaultWidth = 100;
        const string _emitterCategoryName = "Rect Emitter";

        int _height = _defaultHeight;
        bool _perimeter = _defaultPerimeter;
        int _width = _defaultWidth;

        /// <summary>
        /// Gets or sets the height of the rectangle.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than or
        /// equal to zero.</exception>
        [Category(_emitterCategoryName)]
        [Description("The height of the rectangle.")]
        [DisplayName("Width")]
        [DefaultValue(_defaultHeight)]
        public int Height
        {
            get { return _height; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value");

                _height = value;
            }
        }

        /// <summary>
        /// Gets or sets if <see cref="Particle"/>s are only released on the perimeter of the rectangle.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("If Particles are only released on the perimeter of the rectangle.")]
        [DisplayName("Perimeter")]
        [DefaultValue(_defaultPerimeter)]
        public bool Perimeter
        {
            get { return _perimeter; }
            set { _perimeter = value; }
        }

        /// <summary>
        /// Gets or sets the width of the rectangle.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is less than or
        /// equal to zero.</exception>
        [Category(_emitterCategoryName)]
        [Description("The width of the rectangle.")]
        [DisplayName("Width")]
        [DefaultValue(_defaultWidth)]
        public int Width
        {
            get { return _width; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("value");

                _width = value;
            }
        }

        /// <summary>
        /// Generates the offset and normalized force vectors to release the <see cref="Particle"/> at.
        /// </summary>
        /// <param name="particle">The <see cref="Particle"/> that the values are being generated for.</param>
        /// <param name="offset">The offset vector.</param>
        /// <param name="force">The normalized force vector.</param>
        protected override void GenerateParticleOffsetAndForce(Particle particle, out Vector2 offset, out Vector2 force)
        {
            float hw = Width * 0.5f;
            float hh = Height * 0.5f;

            // Get the offset
            if (Perimeter)
            {
                if (RandomHelper.NextBool())
                    offset = new Vector2(RandomHelper.Choose(-hw, hw), RandomHelper.NextFloat(-hh, hh));
                else
                    offset = new Vector2(RandomHelper.NextFloat(-hw, hw), RandomHelper.Choose(-hh, hh));
            }
            else
                offset = new Vector2(RandomHelper.NextFloat(-hw, hw), RandomHelper.NextFloat(-hh, hh));

            // Get the force
            float radians = RandomHelper.NextFloat(MathHelper.TwoPi);
            force = new Vector2((float)Math.Sin(radians), (float)Math.Cos(radians));
        }
    }
}