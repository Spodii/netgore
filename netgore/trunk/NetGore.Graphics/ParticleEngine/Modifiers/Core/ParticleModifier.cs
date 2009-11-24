using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Base class for a modifier for a <see cref="ParticleEmitter"/>.
    /// </summary>
    public abstract class ParticleModifier
    {
        readonly bool _processOnRelease;
        readonly bool _processOnUpdate;
        int _currentTime;

        /// <summary>
        /// Gets an IEnumerable of the <see cref="Type"/>s of the particle modifiers.
        /// </summary>
        public static IEnumerable<Type> ModifierTypes
        {
            get { return ParticleModifierFactory.ModifierTypes; }
        }

        /// <summary>
        /// Creates an instance of a <see cref="ParticleModifier"/> of type <typeparamref name="T"/>. This is the
        /// preferred method of creating modifiers.
        /// </summary>
        /// <typeparam name="T">The type of modifier.</typeparam>
        /// <returns>An instance of a <see cref="ParticleModifier"/> of type <typeparamref name="T"/>.</returns>
        public static T CreateModifier<T>() where T : ParticleModifier
        {
            return ParticleModifierFactory.GetInstance<T>();
        }

        /// <summary>
        /// Creates an instance of a <see cref="ParticleModifier"/>. This is the preferred method of
        /// creating modifiers.
        /// </summary>
        /// <param name="type">The type of modifier.</param>
        /// <returns>An instance of a <see cref="ParticleModifier"/>.</returns>
        public static ParticleModifier CreateModifier(Type type)
        {
            return ParticleModifierFactory.GetInstance(type);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleModifier"/> class.
        /// </summary>
        /// <param name="processOnRelease">If <see cref="Particle"/>s will be processed after being released.</param>
        /// <param name="processOnUpdate">If <see cref="Particle"/>s will be processed after being updated.</param>
        /// <exception cref="ArgumentException">Both parameters are false.</exception>
        protected ParticleModifier(bool processOnRelease, bool processOnUpdate)
        {
            if (!processOnRelease && !processOnUpdate)
                throw new ArgumentException("Either one of or both parameters must be true.");

            _processOnRelease = processOnRelease;
            _processOnUpdate = processOnUpdate;
        }

        /// <summary>
        /// Gets the current game time.
        /// </summary>
        [Browsable(false)]
        protected int CurrentTime
        {
            get { return _currentTime; }
        }

        /// <summary>
        /// Gets if <see cref="Particle"/>s will be processed after being released.
        /// </summary>
        [Browsable(false)]
        public bool ProcessOnRelease
        {
            get { return _processOnRelease; }
        }

        /// <summary>
        /// Gets if <see cref="Particle"/>s will be processed after being updated.
        /// </summary>
        [Browsable(false)]
        public bool ProcessOnUpdate
        {
            get { return _processOnUpdate; }
        }

        /// <summary>
        /// When overridden in the derived class, handles processing the <paramref name="particle"/> when
        /// it is released. Only valid if <see cref="ParticleModifier.ProcessOnRelease"/> is set.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> that the <paramref name="particle"/>
        /// came from.</param>
        /// <param name="particle">The <see cref="Particle"/> to process.</param>
        protected abstract void HandleProcessReleased(ParticleEmitter emitter, Particle particle);

        /// <summary>
        /// When overridden in the derived class, handles processing the <paramref name="particle"/> when
        /// it is updated. Only valid if <see cref="ParticleModifier.ProcessOnUpdate"/> is set.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> that the <paramref name="particle"/>
        /// came from.</param>
        /// <param name="particle">The <see cref="Particle"/> to process.</param>
        /// <param name="elapsedTime">The amount of time that has elapsed since the <paramref name="emitter"/>
        /// was last updated.</param>
        protected abstract void HandleProcessUpdated(ParticleEmitter emitter, Particle particle, int elapsedTime);

        /// <summary>
        /// Process a <see cref="Particle"/> that was released.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> the <paramref name="particle"/> came from.</param>
        /// <param name="particle">The <see cref="Particle"/> to process.</param>
        internal void ProcessReleased(ParticleEmitter emitter, Particle particle)
        {
            Debug.Assert(ProcessOnRelease, "This method should NOT be called when ProcessOnRelease is not set!");
            HandleProcessReleased(emitter, particle);
        }

        /// <summary>
        /// Updates the current time.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        internal void UpdateCurrentTime(int currentTime)
        {
            _currentTime = currentTime;
        }

        /// <summary>
        /// Process a <see cref="Particle"/> that was updated.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> the <paramref name="particle"/> came from.</param>
        /// <param name="particle">The <see cref="Particle"/> to process.</param>
        /// <param name="elapsedTime">The amount of time that has elapsed since the <paramref name="emitter"/>
        /// was last updated.</param>
        internal void ProcessUpdated(ParticleEmitter emitter, Particle particle, int elapsedTime)
        {
            Debug.Assert(ProcessOnUpdate, "This method should NOT be called when ProcessOnUpdate is not set!");
            HandleProcessUpdated(emitter, particle, elapsedTime);
        }
    }
}