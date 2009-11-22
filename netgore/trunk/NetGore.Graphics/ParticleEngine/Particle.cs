using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Particle
    {
        #region Shader members

        /// <summary>
        /// The current world position of the <see cref="Particle"/>.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The size of the <see cref="Particle"/> in pixels.
        /// </summary>
        public float Scale;

        /// <summary>
        /// The amount the <see cref="Particle"/> is rotated in radians. Rotation will not be used if
        /// using <see cref="PointSpriteRenderer"/>.
        /// </summary>
        public float Rotation;

        /// <summary>
        /// A <see cref="Vector4"/> describing the color of the <see cref="Particle"/> in the format of
        /// RGBA. Each value must be between 0.0 and 1.0.
        /// </summary>
        public Vector4 Color;

        #endregion

        #region Non-shader members

        /// <summary>
        /// The direction the <see cref="Particle"/> is moving.
        /// </summary>
        public Vector2 Momentum;

        /// <summary>
        /// The speed and direction the <see cref="Particle"/> is increasing <see cref="Momentum"/> at.
        /// </summary>
        public Vector2 Velocity;

        /// <summary>
        /// The time at which the <see cref="Particle"/> was created.
        /// </summary>
        public int LifeStart;

        /// <summary>
        /// The time at which the <see cref="Particle"/> will die.
        /// </summary>
        public int LifeEnd;

        #endregion

        /// <summary>
        /// The vertex element data for a <see cref="Particle"/>.
        /// </summary>
        public static readonly VertexElement[] VertexElements;

        /// <summary>
        /// Initializes the <see cref="Particle"/> struct.
        /// </summary>
        static Particle()
        {
            // Position vertex element
            var positionElement = new VertexElement
            { VertexElementFormat = VertexElementFormat.Vector2, VertexElementUsage = VertexElementUsage.Position };

            // Scale vertex element
            var scaleElement = new VertexElement
            { Offset = 8, VertexElementFormat = VertexElementFormat.Single, VertexElementUsage = VertexElementUsage.PointSize };

            // Rotation vertex element
            var rotationElement = new VertexElement
            {
                Offset = 12,
                VertexElementFormat = VertexElementFormat.Single,
                VertexElementUsage = VertexElementUsage.TextureCoordinate
            };

            // Color vertex element
            var colorElement = new VertexElement
            { Offset = 16, VertexElementFormat = VertexElementFormat.Vector4, VertexElementUsage = VertexElementUsage.Color };

            // Vertex element array
            VertexElements = new VertexElement[] { positionElement, scaleElement, rotationElement, colorElement };
        }

        /// <summary>
        /// Applies a force to the <see cref="Particle"/>.
        /// </summary>
        /// <param name="force">The <see cref="Vector2"/> describing the force.</param>
        public void ApplyForce(ref Vector2 force)
        {
            Vector2.Add(ref Velocity, ref force, out Velocity);
        }

        /// <summary>
        /// Updates the <see cref="Particle"/>.
        /// </summary>
        /// <param name="elapsedTime">Amount of time in milliseconds that has elapsed since the last update.</param>
        public void Update(int elapsedTime)
        {
            // Increase the momentum by the velocity
            Vector2.Add(ref Velocity, ref Momentum, out Momentum);

            // Set the velocity back to zero
            Velocity = Vector2.Zero;

            Vector2 deltaMomentum;

            // Calculate the momentum for the elapsed time
            Vector2.Multiply(ref Momentum, elapsedTime * 0.001f, out deltaMomentum);

            // Add the delta momentum to the position
            Vector2.Add(ref Position, ref deltaMomentum, out Position);
        }
    }
}