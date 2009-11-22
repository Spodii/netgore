using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Describes a single particle in a particle system.
    /// </summary>
    public class Particle : IDisposable
    {
        static readonly Stack<Particle> _freeParticles = new Stack<Particle>();

        /// <summary>
        /// The color of the <see cref="Particle"/>.
        /// </summary>
        public Color Color;

        /// <summary>
        /// The time at which the <see cref="Particle"/> will die.
        /// </summary>
        public int LifeEnd;

        /// <summary>
        /// The time at which the <see cref="Particle"/> was created.
        /// </summary>
        public int LifeStart;

        /// <summary>
        /// The direction the <see cref="Particle"/> is moving.
        /// </summary>
        public Vector2 Momentum;

        /// <summary>
        /// The current world position of the <see cref="Particle"/>.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// The amount the <see cref="Particle"/> is rotated in radians.
        /// </summary>
        public float Rotation;

        /// <summary>
        /// The size of the <see cref="Particle"/> in pixels.
        /// </summary>
        public float Scale;

        /// <summary>
        /// The speed and direction the <see cref="Particle"/> is increasing <see cref="Momentum"/> at.
        /// </summary>
        public Vector2 Velocity;

        bool _isDisposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="Particle"/> class.
        /// </summary>
        Particle()
        {
            // Force the Create() method to be used.
        }

        /// <summary>
        /// Gets if this <see cref="Particle"/> has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return _isDisposed; }
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
        /// Creates a <see cref="Particle"/>.
        /// </summary>
        /// <returns>The <see cref="Particle"/> that was created.</returns>
        public static Particle Create()
        {
            // Return any free particles that we have already created
            if (_freeParticles.Count > 0)
            {
                var free = _freeParticles.Pop();
                Debug.Assert(free._isDisposed);
                free._isDisposed = false;
                return free;
            }

            return new Particle();
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

        #region IDisposable Members

        /// <summary>
        /// Disposes of the <see cref="Particle"/>.
        /// </summary>
        /// <exception cref="MethodAccessException">This particle is already disposed.</exception>
        public void Dispose()
        {
            if (_isDisposed)
                throw new MethodAccessException("Particle is already disposed.");

            _isDisposed = true;

            _freeParticles.Push(this);
        }

        #endregion
    }
}