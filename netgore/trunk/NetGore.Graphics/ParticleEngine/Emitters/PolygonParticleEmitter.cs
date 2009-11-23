using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// A <see cref="ParticleEmitter"/> that emits particles from a custom-defined polygon.
    /// </summary>
    public class PolygonParticleEmitter : ParticleEmitter
    {
        internal const PolygonOrigin DefaultPolygonOrigin = PolygonOrigin.Center;
        const bool _defaultClosed = true;
        const float _defaultRotation = 0;
        const float _defaultScale = 1;
        const string _emitterCategoryName = "Polygon Emitter";

        bool _closed = _defaultClosed;
        PolygonPointCollection _points = new PolygonPointCollection();
        Matrix _rotationMatrix = Matrix.CreateRotationZ(_defaultRotation);
        Matrix _scaleMatrix = Matrix.CreateScale(_defaultScale);

        /// <summary>
        /// Gets or sets if the polygon is closed. If true, the last point will be connected to the first.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("If the polygon is closed. If true, the last point will be connected to the first.")]
        [DisplayName("Closed")]
        [DefaultValue(_defaultClosed)]
        public bool Closed
        {
            get { return _closed; }
            set { _closed = value; }
        }

        /// <summary>
        /// Gets or sets the collection of points that define the polygon.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("The collection of points that define the polygon.")]
        [DisplayName("Points")]
        public PolygonPointCollection Points
        {
            get { return _points; }
            set { _points = value; }
        }

        /// <summary>
        /// Gets or sets the origin mode of the polygon for the given points.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("The origin mode of the polygon for the given points.")]
        [DisplayName("Polygon Origin")]
        [DefaultValue(DefaultPolygonOrigin)]
        public PolygonOrigin PolygonOrigin
        {
            get { return Points.Origin; }
            set { Points.Origin = value; }
        }

        /// <summary>
        /// Gets or sets the rotation factor for the polygon.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("The origin mode of the polygon for the given points.")]
        [DisplayName("Polygon Origin")]
        [DefaultValue(_defaultRotation)]
        public float Rotation
        {
            get { return (float)Math.Atan2(_rotationMatrix.M12, _rotationMatrix.M11); }
            set { _rotationMatrix = Matrix.CreateRotationZ(value); }
        }

        /// <summary>
        /// Gets or sets the scale of the polygon.
        /// </summary>
        [Category(_emitterCategoryName)]
        [Description("The scale of the polygon.")]
        [DisplayName("Scale")]
        [DefaultValue(_defaultScale)]
        public float Scale
        {
            get { return _scaleMatrix.M11; }
            set { _scaleMatrix = Matrix.CreateScale(value); }
        }

        /// <summary>
        /// Generates the offset and normalized force vectors to release the <see cref="Particle"/> at.
        /// </summary>
        /// <param name="particle">The <see cref="Particle"/> that the values are being generated for.</param>
        /// <param name="offset">The offset vector.</param>
        /// <param name="force">The normalized force vector.</param>
        protected override void GenerateParticleOffsetAndForce(Particle particle, out Vector2 offset, out Vector2 force)
        {
            if (Points.Count == 0)
                offset = Vector2.Zero;
            else if (Points.Count == 1)
                offset = Points[0];
            else if (Points.Count == 2)
                offset = Vector2.Lerp(Points[0], Points[1], RandomHelper.NextFloat());
            else
            {
                int i = Closed ? RandomHelper.NextInt(0, Points.Count) : RandomHelper.NextInt(0, Points.Count - 1);

                offset = Vector2.Lerp(Points[i], Points[(i + 1) % Points.Count], RandomHelper.NextFloat());
            }

            // Apply any necessary transformation to the resultant offset vector...
            Matrix transform;

            Matrix.Multiply(ref Points.TranslationMatrix, ref _rotationMatrix, out transform);

            Matrix.Multiply(ref _scaleMatrix, ref transform, out transform);

            Vector2.Transform(ref offset, ref transform, out offset);

            // Get the force
            GetForce(RandomHelper.NextFloat(MathHelper.TwoPi), out force);
        }
    }
}