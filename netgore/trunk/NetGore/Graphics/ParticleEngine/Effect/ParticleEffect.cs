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
        readonly List<ParticleEmitter> _emitters = new List<ParticleEmitter>();

        bool _isDisposed;
        bool _isExpired = false;
        TickCount _timeCreated = TickCount.Now;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEffect"/> class.
        /// </summary>
        public ParticleEffect()
        {
            Life = -1;

            var name = ParticleEffectManager.Instance.GenerateUniqueEffectName(DefaultEffectName);
            _effectConfig = new ParticleEffectConfig(name);
            ParticleEffectManager.Instance.AddEffect(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEffect"/> class.
        /// </summary>
        /// <param name="r">The <see cref="IValueReader"/> to read from.</param>
        internal ParticleEffect(IValueReader r) : this()
        {
            Life = -1;

            ReadState(r);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEffect"/> class.
        /// </summary>
        /// <param name="effectConfig">The <see cref="ParticleEffectConfig"/> to use.</param>
        ParticleEffect(ParticleEffectConfig effectConfig)
        {
            Life = -1;

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

            Debug.Assert(!_emitters.Contains(emitter));
            Debug.Assert(emitter.Owner == this);

            // Make sure the emitter's name is unique
            var newName = GenerateUniqueEmitterName(emitter.Name ?? ParticleEmitter.DefaultName);
            emitter.ChangeName(newName);

            // Add
            _emitters.Add(emitter);

            emitter.Disposed -= emitter_Disposed;
            emitter.Disposed += emitter_Disposed;

            if (EmitterAdded != null)
                EmitterAdded.Raise(this, EventArgsHelper.Create((IParticleEmitter)emitter));
        }

        /// <summary>
        /// Handles when a <see cref="IParticleEmitter"/> in this <see cref="IParticleEffect"/> is disposed.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        void emitter_Disposed(IParticleEmitter sender, EventArgs e)
        {
            sender.Disposed -= emitter_Disposed;

            Debug.Assert(_emitters.Contains(sender));
            Debug.Assert(sender.Owner == this);
            Debug.Assert(sender is ParticleEmitter);

            _emitters.Remove((ParticleEmitter)sender);

            if (EmitterRemoved != null)
                EmitterRemoved.Raise(this, EventArgsHelper.Create(sender));
        }

        #region IParticleEffect Members

        /// <summary>
        /// Notifies listeners when this <see cref="IParticleEffect"/> has bene disposed.
        /// </summary>
        public event TypedEventHandler<IParticleEffect> Disposed;

        /// <summary>
        /// Notifies listeners when an <see cref="IParticleEmitter"/> has been added to this <see cref="IParticleEffect"/>.
        /// </summary>
        public event TypedEventHandler<IParticleEffect, EventArgs<IParticleEmitter>> EmitterAdded;

        /// <summary>
        /// Notifies listeners when an <see cref="IParticleEmitter"/> has been removed from this <see cref="IParticleEffect"/>.
        /// </summary>
        public event TypedEventHandler<IParticleEffect, EventArgs<IParticleEmitter>> EmitterRemoved;

        /// <summary>
        /// Gets the <see cref="IParticleEmitter"/>s in this <see cref="IParticleEffect"/>.
        /// </summary>
        [Browsable(false)]
        public IEnumerable<IParticleEmitter> Emitters
        {
            get { return _emitters; }
        }

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
        /// The default value is -1.
        /// </summary>
        [Browsable(true)]
        [Category(_effectCategory)]
        [Description("The life of the particle effect, in milliseconds. If less than 0, it will live indefinitely.")]
        [DefaultValue(-1)]
        [SyncValue]
        public int Life { get; set; }

        /// <summary>
        /// Gets or sets the name of this <see cref="IParticleEffect"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null or empty.</exception>
        [Browsable(true)]
        [Category(_effectCategory)]
        [Description("The unique name of this particle effect.")]
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
        [DefaultValue(typeof(Vector2), "{0, 0}")]
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
        /// Changes the order of an <see cref="IParticleEmitter"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="IParticleEmitter"/> to change the order of.</param>
        /// <param name="newIndex">The new index to give the <paramref name="emitter"/>. Other <see cref="IParticleEmitter"/>s will be
        /// shifted accordingly. If this value is less than or equal to 0, the <paramref name="emitter"/> will be placed at the head.
        /// If greater than or equal to the number of <see cref="IParticleEmitter"/>s in this collection, it will be placed at the
        /// tail.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="emitter"/> is null.</exception>
        /// <exception cref="ArgumentException">The <paramref name="emitter"/> is not in this <see cref="IParticleEffect"/>.</exception>
        public void ChangeEmitterOrder(IParticleEmitter emitter, int newIndex)
        {
            if (emitter == null)
                throw new ArgumentNullException("emitter");

            var currIndex = GetEmitterOrder(emitter);
            if (currIndex == -1)
            {
                const string errmsg = "IParticleEmitter `{0}` not found in this collection.";
                throw new ArgumentException(string.Format(errmsg, emitter), "emitter");
            }

            // Clamp
            newIndex = newIndex.Clamp(0, _emitters.Count - 1);

            if (currIndex == newIndex)
                return;

            // Remove then re-add at the given index
            Debug.Assert(_emitters[currIndex] == emitter);
            _emitters.RemoveAt(currIndex);

            _emitters.Insert(newIndex, (ParticleEmitter)emitter);
            Debug.Assert(_emitters[newIndex] == emitter);
        }

        /// <summary>
        /// Gets if an <see cref="IParticleEmitter"/> with the given name exists in this <see cref="IParticleEffect"/>.
        /// </summary>
        /// <param name="emitterName">The name of the <see cref="IParticleEmitter"/> to look for.</param>
        /// <returns>True if a <see cref="IParticleEmitter"/> exists in this collection with the given name; otherwise false.</returns>
        public bool Contains(string emitterName)
        {
            return _emitters.Any(x => ParticleEmitter.EmitterNameComparer.Equals(emitterName, x.Name));
        }

        /// <summary>
        /// Gets if an <see cref="IParticleEmitter"/> exists in this <see cref="IParticleEffect"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="IParticleEmitter"/> to look for.</param>
        /// <returns>True if the <see cref="IParticleEmitter"/> exists in this collection; otherwise false.</returns>
        public bool Contains(IParticleEmitter emitter)
        {
            return _emitters.Contains(emitter);
        }

        /// <summary>
        /// Decrements the order of an <see cref="IParticleEmitter"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="IParticleEmitter"/> to decrement the order of.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="emitter"/> is null.</exception>
        /// <exception cref="ArgumentException">The <paramref name="emitter"/> is not in this <see cref="IParticleEffect"/>.</exception>
        public void DecrementEmitterOrder(IParticleEmitter emitter)
        {
            var index = GetEmitterOrder(emitter);
            if (index < 0)
                return;

            ChangeEmitterOrder(emitter, index - 1);
        }

        /// <summary>
        /// Creates a deep copy of this <see cref="IParticleEffect"/>.
        /// </summary>
        /// <returns>A deep copy of this <see cref="IParticleEffect"/>.</returns>
        public IParticleEffect DeepCopy()
        {
            var ret = new ParticleEffect(_effectConfig) { Life = Life, Position = Position };

            foreach (var e in _emitters)
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
            foreach (var e in _emitters)
            {
                e.Dispose();
            }

            if (Disposed != null)
                Disposed.Raise(this, EventArgs.Empty);
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
            for (var i = 0; i < _emitters.Count; i++)
            {
                var emitter = _emitters[i];
                emitter.Draw(spriteBatch);
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
            if (!Contains(baseEmitterName))
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
            while (Contains(newName));

            Debug.Assert(!_emitters.Any(x => ParticleEmitter.EmitterNameComparer.Equals(x.Name, newName)));

            return newName;
        }

        /// <summary>
        /// Gets an <see cref="IParticleEmitter"/> by its emitter index.
        /// </summary>
        /// <param name="emitterIndex">The emitter index of the <see cref="IParticleEmitter"/> to get.</param>
        /// <returns>The <see cref="IParticleEmitter"/> at the given <paramref name="emitterIndex"/>, or null if the
        /// <paramref name="emitterIndex"/> is out of range or otherwise invalid.</returns>
        public IParticleEmitter GetEmitter(int emitterIndex)
        {
            if (emitterIndex < 0 || emitterIndex >= _emitters.Count)
                return null;

            return _emitters[emitterIndex];
        }

        /// <summary>
        /// Gets a <see cref="ParticleEmitter"/> in this <see cref="IParticleEffect"/> from the <see cref="IParticleEmitter"/>'s name.
        /// </summary>
        /// <param name="emitterName">The name of the <see cref="ParticleEmitter"/> to get.</param>
        /// <returns>The <see cref="ParticleEmitter"/> with the given <paramref name="emitterName"/>, or null if not found.</returns>
        public IParticleEmitter GetEmitter(string emitterName)
        {
            return _emitters.FirstOrDefault(x => ParticleEmitter.EmitterNameComparer.Equals(x.Name, emitterName));
        }

        /// <summary>
        /// Gets the 0-based order index of a <see cref="IParticleEmitter"/> in this <see cref="IParticleEffect"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="IParticleEmitter"/> to get the order index of.</param>
        /// <returns>The 0-based order index of a <see cref="IParticleEmitter"/> in this <see cref="IParticleEffect"/>.</returns>
        public int GetEmitterOrder(IParticleEmitter emitter)
        {
            var e = emitter as ParticleEmitter;
            if (e == null)
                return -1;

            return _emitters.IndexOf(e);
        }

        /// <summary>
        /// Increments the order of an <see cref="IParticleEmitter"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="IParticleEmitter"/> to decrement the order of.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="emitter"/> is null.</exception>
        /// <exception cref="ArgumentException">The <paramref name="emitter"/> is not in this <see cref="IParticleEffect"/>.</exception>
        public void IncrementEmitterOrder(IParticleEmitter emitter)
        {
            var index = GetEmitterOrder(emitter);
            if (index < 0)
                return;

            ChangeEmitterOrder(emitter, index + 1);
        }

        /// <summary>
        /// Kills this <see cref="IParticleEffect"/> and all the <see cref="IParticleEmitter"/>s in it.
        /// </summary>
        public void Kill()
        {
            foreach (var e in _emitters)
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
            foreach (var e in _emitters.ToImmutable())
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
            _isExpired = false;

            // Reset all emitters
            foreach (var e in _emitters)
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
            if (Contains(newName))
                return false;

            // Add the emitter if not already in the collection
            if (!_emitters.Contains(e))
                _emitters.Add(e);

            // Set the new name
            e.ChangeName(newName);

            Debug.Assert(ParticleEmitter.EmitterNameComparer.Equals(e.Name, newName));
            Debug.Assert(_emitters.Contains(e));

            return true;
        }

        /// <summary>
        /// Checks if in the object is in view of the specified <paramref name="camera"/>.
        /// </summary>
        /// <param name="camera">The <see cref="ICamera2D"/> to check if this object is in view of.</param>
        /// <returns>True if the object is in view of the camera, else False.</returns>
        public bool InView(ICamera2D camera)
        {
            return _emitters.Any(x => x.InView(camera));
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
                foreach (var e in _emitters)
                {
                    e.Kill();
                }
            }

            // Update the emitters
            var emittersHaveExpired = true;
            foreach (var e in _emitters)
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
                writer.WriteManyNodes(_emittersNodeName, _emitters, ParticleEmitterFactory.Write);
            }
            writer.WriteEndNode(_particleEffectNodeName);
        }

        #endregion
    }
}