using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using log4net;
using SFML.Graphics;

// Thanks goes out to the open source particle engine ProjectMercury, which a lot of the particle engine concepts
// and design is derived from.
// http://www.codeplex.com/mpe

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Describes a single particle in a particle system.
    /// </summary>
    public sealed class Particle : IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static readonly Stack<Particle> _freeParticles = new Stack<Particle>();

        /// <summary>
        /// The color of the <see cref="Particle"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields",
            Justification = "Allows us to pass this field by reference, which can increase performance due to less copying.")]
        public Color Color;

        /// <summary>
        /// The time at which the <see cref="Particle"/> will die.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields",
            Justification = "Allows us to pass this field by reference, which can increase performance due to less copying.")]
        public TickCount LifeEnd;

        /// <summary>
        /// The time at which the <see cref="Particle"/> was created.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields",
            Justification = "Allows us to pass this field by reference, which can increase performance due to less copying.")]
        public TickCount LifeStart;

        /// <summary>
        /// The direction the <see cref="Particle"/> is moving.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields",
            Justification = "Allows us to pass this field by reference, which can increase performance due to less copying.")]
        public Vector2 Momentum;

        /// <summary>
        /// The current world position of the <see cref="Particle"/>.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields",
            Justification = "Allows us to pass this field by reference, which can increase performance due to less copying.")]
        public Vector2 Position;

        /// <summary>
        /// The amount the <see cref="Particle"/> is rotated in radians.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields",
            Justification = "Allows us to pass this field by reference, which can increase performance due to less copying.")]
        public float Rotation;

        /// <summary>
        /// The magnification scale to draw the <see cref="Particle"/>s at, where 1.0 is normal size.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields",
            Justification = "Allows us to pass this field by reference, which can increase performance due to less copying.")]
        public float Scale;

        /// <summary>
        /// The speed and direction the <see cref="Particle"/> is increasing <see cref="Momentum"/> at.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields",
            Justification = "Allows us to pass this field by reference, which can increase performance due to less copying.")]
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
        /// Gets the total number of milliseconds that this <see cref="Particle"/> will live, from start to finish.
        /// </summary>
        public TickCount LifeSpan
        {
            get { return LifeEnd - LifeStart; }
        }

        /// <summary>
        /// Applies a force to the <see cref="Particle"/>.
        /// </summary>
        /// <param name="force">The <see cref="Vector2"/> describing the force.</param>
        public void ApplyForce(Vector2 force)
        {
            Vector2.Add(ref Velocity, ref force, out Velocity);
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
                Particle free;
                try
                {
                    free = _freeParticles.Pop();
                }
                catch (Exception ex)
                {
                    const string errmsg =
                        "Failed to pop particle from _freeParticles even though Count was > 0. Threading issue maybe? Exception: {0}";
                    if (log.IsWarnEnabled)
                        log.WarnFormat(errmsg, ex);
                    Debug.Fail(string.Format(errmsg, ex));
                    return new Particle();
                }

                Debug.Assert(free._isDisposed, "Uh-oh, we had a undisposed Particle in the free stack! How did this happen!?");
                free._isDisposed = false;
                return free;
            }

            return new Particle();
        }

        /// <summary>
        /// Gets the age of the particle in milliseconds.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <returns>The age of the particle in milliseconds.</returns>
        public TickCount GetAge(TickCount currentTime)
        {
            return currentTime - LifeStart;
        }

        /// <summary>
        /// Gets the age of the particle on a scale of 0.0 to 1.0, where 0.0 means the particle
        /// was just created and 1.0 means the particle is at the end of its life.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <returns>The age of the particle as a percent.</returns>
        public float GetAgePercent(TickCount currentTime)
        {
            return (float)GetAge(currentTime) / LifeSpan;
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

            // Calculate the momentum for the elapsed time
            Vector2 deltaMomentum;
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
                return;

            _isDisposed = true;

            _freeParticles.Push(this);
        }

        #endregion
    }
}