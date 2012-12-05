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
        event TypedEventHandler<IParticleEffect> Disposed;

        /// <summary>
        /// Notifies listeners when an <see cref="IParticleEmitter"/> has been added to this <see cref="IParticleEffect"/>.
        /// </summary>
        event TypedEventHandler<IParticleEffect, EventArgs<IParticleEmitter>> EmitterAdded;

        /// <summary>
        /// Notifies listeners when an <see cref="IParticleEmitter"/> has been removed from this <see cref="IParticleEffect"/>.
        /// </summary>
        event TypedEventHandler<IParticleEffect, EventArgs<IParticleEmitter>> EmitterRemoved;

        /// <summary>
        /// Gets the <see cref="IParticleEmitter"/>s in this <see cref="IParticleEffect"/>.
        /// </summary>
        IEnumerable<IParticleEmitter> Emitters { get; }

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
        /// Changes the order of an <see cref="IParticleEmitter"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="IParticleEmitter"/> to change the order of.</param>
        /// <param name="newIndex">The new index to give the <paramref name="emitter"/>. Other <see cref="IParticleEmitter"/>s will be
        /// shifted accordingly. If this value is less than or equal to 0, the <paramref name="emitter"/> will be placed at the head.
        /// If greater than or equal to the number of <see cref="IParticleEmitter"/>s in this collection, it will be placed at the
        /// tail.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="emitter"/> is null.</exception>
        /// <exception cref="ArgumentException">The <paramref name="emitter"/> is not in this <see cref="IParticleEffect"/>.</exception>
        void ChangeEmitterOrder(IParticleEmitter emitter, int newIndex);

        /// <summary>
        /// Gets if an <see cref="IParticleEmitter"/> with the given name exists in this <see cref="IParticleEffect"/>.
        /// </summary>
        /// <param name="emitterName">The name of the <see cref="IParticleEmitter"/> to look for.</param>
        /// <returns>True if a <see cref="IParticleEmitter"/> exists in this collection with the given name; otherwise false.</returns>
        bool Contains(string emitterName);

        /// <summary>
        /// Gets if an <see cref="IParticleEmitter"/> exists in this <see cref="IParticleEffect"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="IParticleEmitter"/> to look for.</param>
        /// <returns>True if the <see cref="IParticleEmitter"/> exists in this collection; otherwise false.</returns>
        bool Contains(IParticleEmitter emitter);

        /// <summary>
        /// Decrements the order of an <see cref="IParticleEmitter"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="IParticleEmitter"/> to decrement the order of.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="emitter"/> is null.</exception>
        /// <exception cref="ArgumentException">The <paramref name="emitter"/> is not in this <see cref="IParticleEffect"/>.</exception>
        void DecrementEmitterOrder(IParticleEmitter emitter);

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
        /// Gets an <see cref="IParticleEmitter"/> by its emitter index.
        /// </summary>
        /// <param name="emitterIndex">The emitter index of the <see cref="IParticleEmitter"/> to get.</param>
        /// <returns>The <see cref="IParticleEmitter"/> at the given <paramref name="emitterIndex"/>, or null if the
        /// <paramref name="emitterIndex"/> is out of range or otherwise invalid.</returns>
        IParticleEmitter GetEmitter(int emitterIndex);

        /// <summary>
        /// Gets a <see cref="ParticleEmitter"/> in this <see cref="IParticleEffect"/> from the <see cref="IParticleEmitter"/>'s name.
        /// </summary>
        /// <param name="emitterName">The name of the <see cref="IParticleEmitter"/> to get.</param>
        /// <returns>The <see cref="ParticleEmitter"/> with the given <paramref name="emitterName"/>, or null if not found.</returns>
        IParticleEmitter GetEmitter(string emitterName);

        /// <summary>
        /// Gets the 0-based order index of a <see cref="IParticleEmitter"/> in this <see cref="IParticleEffect"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="IParticleEmitter"/> to get the order index of.</param>
        /// <returns>The 0-based order index of a <see cref="IParticleEmitter"/> in this <see cref="IParticleEffect"/>, or
        /// -1 if the <paramref name="emitter"/> is not in this <see cref="IParticleEffect"/> or invalid.</returns>
        int GetEmitterOrder(IParticleEmitter emitter);

        /// <summary>
        /// Increments the order of an <see cref="IParticleEmitter"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="IParticleEmitter"/> to decrement the order of.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="emitter"/> is null.</exception>
        /// <exception cref="ArgumentException">The <paramref name="emitter"/> is not in this <see cref="IParticleEffect"/>.</exception>
        void IncrementEmitterOrder(IParticleEmitter emitter);

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

        /// <summary>
        /// Checks if in the object is in view of the specified <paramref name="camera"/>.
        /// </summary>
        /// <param name="camera">The <see cref="ICamera2D"/> to check if this object is in view of.</param>
        /// <returns>True if the object is in view of the camera, else False.</returns>
        bool InView(ICamera2D camera);
    }
}