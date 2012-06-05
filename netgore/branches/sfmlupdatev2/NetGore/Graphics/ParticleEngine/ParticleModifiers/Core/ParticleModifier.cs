using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore.IO;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Base class for a class that modifies the behavior of each <see cref="Particle"/> that is emitted from a
    /// <see cref="ParticleEmitter"/>.
    /// </summary>
    public abstract class ParticleModifier : IPersistable
    {
        const string _customValuesNodeName = "CustomValues";
        const string _typeKeyName = "ModifierType";
        static readonly ParticleModifierFactory _typeFactory;

        readonly bool _processOnRelease;
        readonly bool _processOnUpdate;

        TickCount _currentTime;

        /// <summary>
        /// Initializes the <see cref="ParticleModifier"/> class.
        /// </summary>
        static ParticleModifier()
        {
            _typeFactory = ParticleModifierFactory.Instance;
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
        protected TickCount CurrentTime
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
        /// Creates a deep copy of this <see cref="ParticleModifier"/>.
        /// </summary>
        /// <returns>A deep copy of this <see cref="ParticleModifier"/>.</returns>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public ParticleModifier DeepCopy()
        {
            // Create the deep copy by serializing to/from an IValueWriter
            using (var bs = new BitStream())
            {
                // Write
                using (var writer = BinaryValueWriter.Create(bs, false))
                {
                    Write(writer);
                }

                bs.Position = 0;

                // Read
                var reader = BinaryValueReader.Create(bs, false);
                return Read(reader);
            }
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

        /// <summary>
        /// Reads a <see cref="ParticleModifier"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        /// <returns>The <see cref="ParticleModifier"/> instance created from the values read from the
        /// <paramref name="reader"/>.</returns>
        /// <exception cref="ParticleEmitterLoadParticleModifierException">The <see cref="ParticleModifier"/> could not be loaded.</exception>
        public static ParticleModifier Read(IValueReader reader)
        {
            // Get the type
            var typeName = reader.ReadString(_typeKeyName).Trim();

            // Create the instance
            ParticleModifier modifier;
            try
            {
                modifier = (ParticleModifier)_typeFactory.GetTypeInstance(typeName);
            }
            catch (KeyNotFoundException ex)
            {
                throw new ParticleEmitterLoadParticleModifierException(typeName, ex);
            }

            // Read the custom values
            var customValueReader = reader.ReadNode(_customValuesNodeName);
            modifier.ReadState(customValueReader);

            return modifier;
        }

        /// <summary>
        /// When overridden in the derived class, reads the <see cref="ParticleModifier"/>'s custom values
        /// from the <see cref="reader"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read the custom values from.</param>
        protected abstract void ReadCustomValues(IValueReader reader);

        /// <summary>
        /// Updates the current time.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        internal void UpdateCurrentTime(TickCount currentTime)
        {
            _currentTime = currentTime;
        }

        /// <summary>
        /// Writes the <see cref="ParticleModifier"/> to the <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        public void Write(IValueWriter writer)
        {
            var typeName = _typeFactory.GetTypeName(GetType());

            writer.Write(_typeKeyName, typeName);

            writer.WriteStartNode(_customValuesNodeName);
            {
                WriteState(writer);
            }
            writer.WriteEndNode(_customValuesNodeName);
        }

        /// <summary>
        /// When overridden in the derived class, writes all custom state values to the <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the state values to.</param>
        protected abstract void WriteCustomValues(IValueWriter writer);

        #region IPersistable Members

        /// <summary>
        /// Reads the state of the object from an <see cref="IValueReader"/>. Values should be read in the exact
        /// same order as they were written.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        public void ReadState(IValueReader reader)
        {
            ReadCustomValues(reader);
        }

        /// <summary>
        /// Writes the state of the object to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the values to.</param>
        public void WriteState(IValueWriter writer)
        {
            WriteCustomValues(writer);
        }

        #endregion
    }
}