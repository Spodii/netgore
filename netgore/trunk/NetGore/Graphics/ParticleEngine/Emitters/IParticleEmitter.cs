using System;
using System.Linq;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Interface for an object that emits <see cref="Particle"/>s.
    /// </summary>
    /// <remarks>
    /// This interface is primarily intended to expose a <see cref="ParticleEmitter"/> to higher levels of the application where
    /// the <see cref="ParticleEmitter"/> should not be modified at all, and instead just provide access to reading its values.
    /// It is not provided so you can create your own implementation of an <see cref="IParticleEmitter"/> to use instead of
    /// deriving from <see cref="ParticleEmitter"/> since doing so should be needed.
    /// </remarks>
    public interface IParticleEmitter : IDisposable, IPersistable
    {
        /// <summary>
        /// Notifies listeners when this <see cref="IParticleEmitter"/> has bene disposed.
        /// </summary>
        event TypedEventHandler<IParticleEmitter> Disposed;

        /// <summary>
        /// Gets the number of living <see cref="Particle"/>s.
        /// </summary>
        int ActiveParticles { get; }

        /// <summary>
        /// Gets the <see cref="BlendMode"/> to use when rendering the <see cref="Particle"/>s
        /// emitted by this <see cref="ParticleEmitter"/>.
        /// </summary>
        BlendMode BlendMode { get; }

        /// <summary>
        /// Gets the maximum number of live particles this <see cref="ParticleEmitter"/> may create at
        /// any given time.
        /// </summary>
        int Budget { get; }

        /// <summary>
        /// Gets how long, in milliseconds, the emitter lives after being created. If less than 0, it lives
        /// indefinitely.
        /// </summary>
        int EmitterLife { get; }

        /// <summary>
        /// Gets the collection of modifiers to use on the <see cref="ParticleEmitter"/>.
        /// </summary>
        EmitterModifierCollection EmitterModifiers { get; }

        /// <summary>
        /// Gets if this <see cref="ParticleEmitter"/> has an infinite life span. If true,
        /// it will never expire automatically. If false, the amount of time remaining can be found
        /// from <see cref="RemainingLife"/>.
        /// </summary>
        bool HasInfiniteLife { get; }

        /// <summary>
        /// Gets if this <see cref="ParticleEmitter"/> has been disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Gets if the <see cref="ParticleEmitter"/> is expired and all <see cref="Particle"/>s it has spawned
        /// have expired.
        /// </summary>
        bool IsExpired { get; }

        /// <summary>
        /// Gets the name of the <see cref="IParticleEmitter"/> that is unique to the <see cref="IParticleEmitter.Owner"/>.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the origin of this <see cref="ParticleEmitter"/>.
        /// </summary>
        Vector2 Origin { get; }

        /// <summary>
        /// Gets the <see cref="IParticleEffect"/> that owns this <see cref="IParticleEmitter"/>.
        /// </summary>
        IParticleEffect Owner { get; }

        /// <summary>
        /// Gets the life of each <see cref="Particle"/> emitted.
        /// </summary>
        VariableInt ParticleLife { get; }

        /// <summary>
        /// Gets the collection of modifiers to use on the <see cref="Particle"/>s from this
        /// <see cref="ParticleEmitter"/>.
        /// </summary>
        ParticleModifierCollection ParticleModifiers { get; }

        /// <summary>
        /// Gets the number of <see cref="Particle"/>s that are emitted at each release.
        /// </summary>
        VariableUShort ReleaseAmount { get; }

        /// <summary>
        /// Gets the initial color of the <see cref="Particle"/> when emitted.
        /// </summary>
        VariableColor ReleaseColor { get; }

        /// <summary>
        /// Gets the rate in milliseconds that <see cref="Particle"/>s are emitted.
        /// </summary>
        VariableUShort ReleaseRate { get; }

        /// <summary>
        /// Gets the initial rotation in radians of the <see cref="Particle"/> when emitted.
        /// </summary>
        VariableFloat ReleaseRotation { get; }

        /// <summary>
        /// Gets the initial scale of the <see cref="Particle"/> when emitted.
        /// </summary>
        VariableFloat ReleaseScale { get; }

        /// <summary>
        /// Gets the speed of <see cref="Particle"/>s when released.
        /// </summary>
        VariableFloat ReleaseSpeed { get; }

        /// <summary>
        /// Gets the amount of time remaining for the <see cref="ParticleEmitter"/> before it is
        /// automatically terminated. If less than zero, it will never be terminated automatically.
        /// If zero, this <see cref="IParticleEmitter"/> is no longer emitting particles.
        /// </summary>
        int RemainingLife { get; }

        /// <summary>
        /// Gets the <see cref="ISprite"/> to draw the <see cref="Particle"/>s.
        /// </summary>
        Grh Sprite { get; }

        /// <summary>
        /// Checks if in the object is in view of the specified <paramref name="camera"/>.
        /// </summary>
        /// <param name="camera">The <see cref="ICamera2D"/> to check if this object is in view of.</param>
        /// <returns>True if the object is in view of the camera, else False.</returns>
        bool InView(ICamera2D camera);

        /// <summary>
        /// Forces the <see cref="IParticleEmitter"/> to be reset from the start. This only resets state variables such as
        /// the time the effect was created and how long it has to live, not properties such as position and emitting style.
        /// Has no effect when disposed.
        /// </summary>
        void Reset();
    }
}