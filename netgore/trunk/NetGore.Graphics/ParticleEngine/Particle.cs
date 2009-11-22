using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Describes a single particle in a particle system.
    /// </summary>
    public class Particle
    {
        /// <summary>
        /// The current world position of the <see cref="Particle"/>.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The size of the <see cref="Particle"/> in pixels.
        /// </summary>
        public float Scale;

        /// <summary>
        /// The amount the <see cref="Particle"/> is rotated in radians.
        /// </summary>
        public float Rotation;

        /// <summary>
        /// The color of the <see cref="Particle"/>.
        /// </summary>
        public Color Color;

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