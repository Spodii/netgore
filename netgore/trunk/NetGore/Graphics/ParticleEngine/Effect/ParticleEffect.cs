using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// An effect made from one or more <see cref="ParticleEmitter"/>s combined together.
    /// </summary>
    public class ParticleEffect : IParticleEffect
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The default name for a <see cref="ParticleEffect"/>.
        /// </summary>
        public const string DefaultEffectName = "Unnamed";

        const string _effectCategory = "Particle Effect";
        const string _emittersNodeName = "Emitters";
        const string _particleEffectNodeName = "ParticleEffect";

        readonly ParticleEffectConfig _effectConfig;

        readonly Dictionary<string, ParticleEmitter> _emitters =
            new Dictionary<string, ParticleEmitter>(ParticleEmitter.EmitterNameComparer);

        bool _isDisposed;
        bool _isExpired = false;
        TickCount _timeCreated = TickCount.Now;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEffect"/> class.
        /// </summary>
        public ParticleEffect()
        {
            _effectConfig = new ParticleEffectConfig(ParticleEffectManager.Instance.GenerateUniqueEffectName(DefaultEffectName));
            ParticleEffectManager.Instance.AddEffect(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEffect"/> class.
        /// </summary>
        /// <param name="r">The <see cref="IValueReader"/> to read from.</param>
        internal ParticleEffect(IValueReader r) : this()
        {
            ReadState(r);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEffect"/> class.
        /// </summary>
        /// <param name="effectConfig">The <see cref="ParticleEffectConfig"/> to use.</param>
        ParticleEffect(ParticleEffectConfig effectConfig)
        {
            _effectConfig = effectConfig;
        }

        /// <summary>
        /// Adds a <see cref="IParticleEmitter"/> to this <see cref="IParticleEffect"/>. Should only be called from the
        /// <see cref="IParticleEmitter"/>'s constructor.
        /// </summary>
        /// <param name="emitter">The <see cref="IParticleEmitter"/> to add.</param>
        internal void AddParticleEmitter(ParticleEmitter emitter)
        {
            if (IsDisposed)
            {
                const string errmsg =
                    "Tried to add ParticleEmitter `{0}` to ParticleEffect `{1}`, but the ParticleEffect was disposed.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, emitter, this);
                Debug.Fail(string.Format(errmsg, emitter, this));
                emitter.Dispose();
                return;
            }

            Debug.Assert(!_emitters.ContainsValue(emitter));
            Debug.Assert(emitter.Owner == this);

            // Make sure the emitter's name is unique
            var newName = GenerateUniqueEmitterName(emitter.Name);
            emitter.ChangeName(newName);

            // Add
            _emitters.Add(newName, emitter);

            emitter.Disposed += emitter_Disposed;
        }

        /// <summary>
        /// Handles when a <see cref="IParticleEmitter"/> in this <see cref="IParticleEffect"/> is disposed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        void emitter_Disposed(IParticleEmitter sender)
        {
            sender.Disposed -= emitter_Disposed;

            Debug.Assert(_emitters.ContainsKey(sender.Name));
            Debug.Assert(_emitters[sender.Name] == sender);
            Debug.Assert(sender.Owner == this);

            _emitters.Remove(sender.Name);
        }

        #region IParticleEffect Members

        /// <summary>
        /// Notifies listeners when this <see cref="IParticleEffect"/> has bene disposed.
        /// </summary>
        public event IParticleEffectEventHandler Disposed;

        /// <summary>
        /// Gets if this <see cref="IParticleEffect"/> has been disposed.
        /// </summary>
        [Browsable(false)]
        public bool IsDisposed
        {
            get { return _isDisposed; }
        }

        /// <summary>
        /// Gets if every <see cref="IParticleEmitter"/> in this <see cref="IParticleEffect"/> is expired.
        /// </summary>
        [Browsable(false)]
        public bool IsExpired
        {
            get { return _isExpired; }
        }

        /// <summary>
        /// Gets or sets the life of the effect in milliseconds. After this amount of time has elapsed, all of the
        /// <see cref="IParticleEmitter"/>s in this <see cref="IParticleEffect"/> will be killed. If less than 0, the
        /// effect will live indefinitely.
        /// </summary>
        [Browsable(true)]
        [Category(_effectCategory)]
        [Description("The life of the particle effect, in milliseconds. If less than 0, it will live indefinitely.")]
        [SyncValue]
        public int Life { get; set; }

        /// <summary>
        /// Gets or sets the name of this <see cref="IParticleEffect"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null or empty.</exception>
        [Browsable(true)]
        [Category(_effectCategory)]
        [Description(
            "The name of this particle effect. Should be unique for each particle effect to avoid overwriting existing effects.")]
        [SyncValue]
        public string Name
        {
            get { return _effectConfig.Name; }
            set { _effectConfig.Name = value; }
        }

        /// <summary>
        /// Gets or sets the position of the <see cref="IParticleEffect"/>.
        /// </summary>
        [Browsable(true)]
        [Category(_effectCategory)]
        [Description("The position of the particle effect.")]
        [SyncValue]
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets the amount of time remaining for the <see cref="ParticleEmitter"/> before it is
        /// automatically terminated. If less than zero, it will never be terminated automatically.
        /// If zero, this <see cref="IParticleEmitter"/> is no longer emitting particles.
        /// </summary>
        [Browsable(false)]
        public int RemainingLife
        {
            get
            {
                if (IsDisposed || IsExpired)
                    return 0;

                if (Life < 0)
                    return -1;

                var ret = Math.Max(0, Life - ((int)TickCount.Now - (int)_timeCreated));
                return ret;
            }
        }

        /// <summary>
        /// Creates a deep copy of this <see cref="IParticleEffect"/>.
        /// </summary>
        /// <returns>A deep copy of this <see cref="IParticleEffect"/>.</returns>
        public IParticleEffect DeepCopy()
        {
            var ret = new ParticleEffect(_effectConfig) { Life = Life, Position = Position };

            foreach (var e in _emitters.Values)
            {
                e.DeepCopy(ret);
            }

            return ret;
        }

        /// <summary>
        /// Immediately terminates the <see cref="IParticleEffect"/> and all <see cref="IParticleEffect"/>s in it.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            // Dispose all emitters
            foreach (var e in _emitters.Values)
            {
                e.Dispose();
            }

            if (Disposed != null)
                Disposed(this);
        }

        /// <summary>
        /// Draws the <see cref="IParticleEffect"/> and all <see cref="IParticleEmitter"/>s inside it.
        /// </summary>
        /// <param name="spriteBatch">The <see cref="ISpriteBatch"/> to use to draw.</param>
        public void Draw(ISpriteBatch spriteBatch)
        {
            if (IsDisposed || IsExpired)
                return;

            // Draw the emitters
            foreach (var e in _emitters.Values)
            {
                e.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Gets a name for a <see cref="IParticleEmitter"/> in this <see cref="IParticleEffect"/> that is guaranteed to not already exist
        /// in this <see cref="IParticleEffect"/>.
        /// </summary>
        /// <param name="baseEmitterName">The base name to use.</param>
        /// <returns>The name to use for an <see cref="IParticleEmitter"/>, derived from the <paramref name="baseEmitterName"/>, that
        /// is not already in use in this <see cref="IParticleEffect"/>.</returns>
        public string GenerateUniqueEmitterName(string baseEmitterName)
        {
            // Initial check - see if the base name is available
            if (!_emitters.ContainsKey(baseEmitterName))
                return baseEmitterName;

            // Base name not available, so start appending an incrementing value. So if baseEmitterName = MyEmitter, it will look like:
            //  MyEmitter (1)
            //  MyEmitter (2)
            //  MyEmitter (3)
            //  ... and so on
            var i = 1;
            string newName;
            do
            {
                newName = baseEmitterName + " (" + i + ")";
                i++;
            }
            while (_emitters.ContainsKey(baseEmitterName));

            Debug.Assert(!_emitters.ContainsKey(newName));

            return newName;
        }

        /// <summary>
        /// Gets a <see cref="ParticleEmitter"/> in this <see cref="IParticleEffect"/> from the <see cref="IParticleEmitter"/>'s name.
        /// </summary>
        /// <param name="emitterName">The name of the <see cref="ParticleEmitter"/> to get.</param>
        /// <returns>The <see cref="ParticleEmitter"/> with the given <paramref name="emitterName"/>, or null if not found.</returns>
        public IParticleEmitter GetEmitter(string emitterName)
        {
            ParticleEmitter ret;
            if (_emitters.TryGetValue(emitterName, out ret))
                return ret;

            return null;
        }

        /// <summary>
        /// Kills this <see cref="IParticleEffect"/> and all the <see cref="IParticleEmitter"/>s in it.
        /// </summary>
        public void Kill()
        {
            foreach (var e in _emitters.Values)
            {
                e.Kill();
            }
        }

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public void ReadState(IValueReader reader)
        {
            reader = reader.ReadNode(_particleEffectNodeName);

            // Clear the emitters
            foreach (var e in _emitters.Values.ToImmutable())
            {
                e.Dispose();
            }

            Debug.Assert(_emitters.IsEmpty());
            _emitters.Clear();

            // Read the effect properties
            PersistableHelper.Read(this, reader);

            // Read the emitters
            var readEmitters = reader.ReadManyNodes(_emittersNodeName, r => ParticleEmitterFactory.Read(r, this));

            Debug.Assert(readEmitters.All(x => x.Owner == this));
        }

        /// <summary>
        /// Resets the <see cref="IParticleEffect"/> and all <see cref="IParticleEmitter"/>s in it, effectively starting the
        /// effect over again from the start.
        /// </summary>
        public void Reset()
        {
            if (IsDisposed)
                return;

            // Reset the effect's life
            _timeCreated = TickCount.Now;

            // Reset all emitters
            foreach (var e in _emitters.Values)
            {
                e.Reset();
            }
        }

        /// <summary>
        /// Tries to rename an <see cref="IParticleEmitter"/> in this <see cref="IParticleEffect"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="IParticleEmitter"/> to rename.</param>
        /// <param name="newName">The new name to give the <paramref name="emitter"/>.</param>
        /// <returns>True if the <paramref name="emitter"/> was successfully renamed; false if the <paramref name="newName"/>
        /// was invalid or already in use, or if the <paramref name="emitter"/> was invalid.</returns>
        public bool TryRenameEmitter(IParticleEmitter emitter, string newName)
        {
            var e = emitter as ParticleEmitter;

            // Ensure the emitter is valid
            if (e == null || e.Owner != this)
            {
                Debug.Fail("Invalid emitter.");
                return false;
            }

            if (string.IsNullOrEmpty(newName))
            {
                Debug.Fail("Invalid name.");
                return false;
            }

            // Check if the emitter already has that name
            if (ParticleEmitter.EmitterNameComparer.Equals(e.Name, newName))
                return true;

            // Check if the name is free
            if (_emitters.ContainsKey(newName))
                return false;

            // Remove from the emitters collection
            // The name will be null if the emitter was just constructed
            if (!string.IsNullOrEmpty(e.Name))
            {
                Debug.Assert(_emitters[e.Name] == e);
                _emitters.Remove(e.Name);
            }

            // Re-add with the new name
            _emitters.Add(newName, e);

            // Set the new name
            e.ChangeName(newName);

            Debug.Assert(ParticleEmitter.EmitterNameComparer.Equals(e.Name, newName));
            Debug.Assert(_emitters[newName] == e);

            return true;
        }

        /// <summary>
        /// Updates the <see cref="IParticleEffect"/> and all <see cref="IParticleEmitter"/>s inside it.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        public void Update(TickCount currentTime)
        {
            if (IsDisposed)
                return;

            // Check if we have expired due to the elapsed time
            if (RemainingLife == 0)
            {
                // Kill all the emitters
                foreach (var e in _emitters.Values)
                {
                    e.Kill();
                }
            }

            // Update the emitters
            var emittersHaveExpired = true;
            foreach (var e in _emitters.Values)
            {
                e.Update(currentTime);
                if (!e.IsExpired)
                    emittersHaveExpired = false;
            }

            // Check if we have expired because all the children have expired
            if (emittersHaveExpired)
                _isExpired = true;
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public void WriteState(IValueWriter writer)
        {
            writer.WriteStartNode(_particleEffectNodeName);
            {
                // Write the effect properties
                PersistableHelper.Write(this, writer);

                // Write the emitters
                writer.WriteManyNodes(_emittersNodeName, _emitters.Values, ParticleEmitterFactory.Write);
            }
            writer.WriteEndNode(_particleEffectNodeName);
        }

        #endregion
    }
}