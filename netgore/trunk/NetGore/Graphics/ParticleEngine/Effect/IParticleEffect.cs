using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Interface for a particle effect, which is the combination of one or more <see cref="IParticleEmitter"/>s combined
    /// together.
    /// </summary>
    public interface IParticleEffect : IDisposable, IPersistable
    {
        /// <summary>
        /// Notifies listeners when this <see cref="IParticleEffect"/> has been disposed.
        /// </summary>
        event IParticleEffectEventHandler Disposed;

        /// <summary>
        /// Notifies listeners when an <see cref="IParticleEmitter"/> has been added to this <see cref="IParticleEffect"/>.
        /// </summary>
        event IParticleEffectEmitterEventHandler EmitterAdded;

        /// <summary>
        /// Gets the <see cref="IParticleEmitter"/>s in this <see cref="IParticleEffect"/>.
        /// </summary>
        IEnumerable<IParticleEmitter> Emitters { get; }

        /// <summary>
        /// Notifies listeners when an <see cref="IParticleEmitter"/> has been removed from this <see cref="IParticleEffect"/>.
        /// </summary>
        event IParticleEffectEmitterEventHandler EmitterRemoved;

        /// <summary>
        /// Gets if this <see cref="IParticleEffect"/> has been disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Gets if every <see cref="IParticleEmitter"/> in this <see cref="IParticleEffect"/> is expired.
        /// </summary>
        bool IsExpired { get; }

        /// <summary>
        /// Gets or sets the life of the effect in milliseconds. After this amount of time has elapsed, all of the
        /// <see cref="IParticleEmitter"/>s in this <see cref="IParticleEffect"/> will be killed. If less than 0, the
        /// effect will live indefinitely.
        /// </summary>
        int Life { get; set; }

        /// <summary>
        /// Gets or sets the name of this <see cref="IParticleEffect"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null or empty.</exception>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the position of the <see cref="IParticleEffect"/>.
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// Gets the amount of time remaining for the <see cref="IParticleEffect"/> before it is
        /// automatically terminated. If less than zero, it will never be terminated automatically.
        /// If zero, this <see cref="IParticleEffect"/> has expired.
        /// </summary>
        int RemainingLife { get; }

        /// <summary>
        /// Creates a deep copy of this <see cref="IParticleEffect"/>.
        /// </summary>
        /// <returns>A deep copy of this <see cref="IParticleEffect"/>.</returns>
        IParticleEffect DeepCopy();

        /// <summary>
        /// Draws the <see cref="IParticleEffect"/> and all <see cref="IParticleEmitter"/>s inside it.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        void Draw(ISpriteBatch spriteBatch);

        /// <summary>
        /// Gets a name for a <see cref="IParticleEmitter"/> in this <see cref="IParticleEffect"/> that is guaranteed to not already exist
        /// in this <see cref="IParticleEffect"/>.
        /// </summary>
        /// <param name="baseEmitterName">The base name to use.</param>
        /// <returns>The name to use for an <see cref="IParticleEmitter"/>, derived from the <paramref name="baseEmitterName"/>, that
        /// is not already in use in this <see cref="IParticleEffect"/>.</returns>
        string GenerateUniqueEmitterName(string baseEmitterName);

        /// <summary>
        /// Gets a <see cref="ParticleEmitter"/> in this <see cref="IParticleEffect"/> from the <see cref="IParticleEmitter"/>'s name.
        /// </summary>
        /// <param name="emitterName">The name of the <see cref="ParticleEmitter"/> to get.</param>
        /// <returns>The <see cref="ParticleEmitter"/> with the given <paramref name="emitterName"/>, or null if not found.</returns>
        IParticleEmitter GetEmitter(string emitterName);

        /// <summary>
        /// Kills this <see cref="IParticleEffect"/> and all the <see cref="IParticleEmitter"/>s in it.
        /// </summary>
        void Kill();

        /// <summary>
        /// Resets the <see cref="IParticleEffect"/> and all <see cref="IParticleEmitter"/>s in it, effectively starting the
        /// effect over again from the start.
        /// </summary>
        void Reset();

        /// <summary>
        /// Tries to rename an <see cref="IParticleEmitter"/> in this <see cref="IParticleEffect"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="IParticleEmitter"/> to rename.</param>
        /// <param name="newName">The new name to give the <paramref name="emitter"/>.</param>
        /// <returns>True if the <paramref name="emitter"/> was successfully renamed; false if the <paramref name="newName"/>
        /// was invalid or already in use, or if the <paramref name="emitter"/> was invalid.</returns>
        bool TryRenameEmitter(IParticleEmitter emitter, string newName);

        /// <summary>
        /// Updates the <see cref="IParticleEffect"/> and all <see cref="IParticleEmitter"/>s inside it.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        void Update(TickCount currentTime);
    }
}