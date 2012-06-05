using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetGore.IO;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Base class for a class that modifies a <see cref="ParticleEmitter"/>.
    /// </summary>
    public abstract class EmitterModifier : IPersistable
    {
        const string _customValuesNodeName = "CustomValues";
        const string _typeKeyName = "ModifierType";

        static readonly EmitterModifierFactory _typeFactory;

        /// <summary>
        /// Initializes the <see cref="EmitterModifier"/> class.
        /// </summary>
        static EmitterModifier()
        {
            _typeFactory = EmitterModifierFactory.Instance;
        }

        /// <summary>
        /// Creates a deep copy of this <see cref="EmitterModifier"/>.
        /// </summary>
        /// <returns>A deep copy of this <see cref="EmitterModifier"/>.</returns>
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public EmitterModifier DeepCopy()
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
        /// When overridden in the derived class, handles reverting changes made to the <see cref="ParticleEmitter"/>
        /// by this <see cref="EmitterModifier"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> to revert the changes to.</param>
        protected abstract void HandleRestore(ParticleEmitter emitter);

        /// <summary>
        /// When overridden in the derived class, handles updating the <see cref="ParticleEmitter"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> to be modified.</param>
        /// <param name="elapsedTime">The amount of time that has elapsed since the last update.</param>
        protected abstract void HandleUpdate(ParticleEmitter emitter, int elapsedTime);

        /// <summary>
        /// Reads a <see cref="EmitterModifier"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        /// <returns>The <see cref="EmitterModifier"/> instance created from the values read from the
        /// <paramref name="reader"/>.</returns>
        /// <exception cref="ParticleEmitterLoadEmitterModifierException">The <see cref="EmitterModifier"/> could not be
        /// loaded.</exception>
        public static EmitterModifier Read(IValueReader reader)
        {
            // Get the type
            var typeName = reader.ReadString(_typeKeyName);

            // Create the instance
            EmitterModifier modifier;
            try
            {
                modifier = (EmitterModifier)_typeFactory.GetTypeInstance(typeName);
            }
            catch (KeyNotFoundException ex)
            {
                throw new ParticleEmitterLoadEmitterModifierException(typeName, ex);
            }

            // Read the custom values
            var customValueReader = reader.ReadNode(_customValuesNodeName);
            modifier.ReadState(customValueReader);

            return modifier;
        }

        /// <summary>
        /// When overridden in the derived class, reads the <see cref="EmitterModifier"/>'s custom values
        /// from the <see cref="reader"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read the custom values from.</param>
        protected abstract void ReadCustomValues(IValueReader reader);

        /// <summary>
        /// Restores the <see cref="ParticleEmitter"/> from changes made by this <see cref="EmitterModifier"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> to revert changes on.</param>
        public void Restore(ParticleEmitter emitter)
        {
            HandleRestore(emitter);
        }

        /// <summary>
        /// Updates the <see cref="EmitterModifier"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> to modify.</param>
        /// <param name="elapsedTime">The amount of time that has elapsed since the last update.</param>
        public void Update(ParticleEmitter emitter, int elapsedTime)
        {
            HandleUpdate(emitter, elapsedTime);
        }

        /// <summary>
        /// Writes the <see cref="EmitterModifier"/> to the <paramref name="writer"/>.
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