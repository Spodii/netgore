using System.Collections.Generic;
using System.Linq;
using NetGore.IO;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Base class for a class that modifies a <see cref="ParticleEmitter"/>.
    /// </summary>
    public abstract class EmitterModifier
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
        /// When overridden in the derived class, handles updating the <see cref="ParticleEmitter"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> to be modified.</param>
        /// <param name="elapsedTime">The amount of time that has elapsed since the last update.</param>
        protected abstract void HandleUpdate(ParticleEmitter emitter, int elapsedTime);

        /// <summary>
        /// Updates the <see cref="EmitterModifier"/>.
        /// </summary>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> to modifier.</param>
        /// <param name="elapsedTime">The amount of time that has elapsed since the last update.</param>
        public void Update(ParticleEmitter emitter, int elapsedTime)
        {
            HandleUpdate(emitter, elapsedTime);
        }

        /// <summary>
        /// Reads a <see cref="EmitterModifier"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read the values from.</param>
        /// <returns>The <see cref="EmitterModifier"/> instance created from the values read from the
        /// <paramref name="reader"/>.</returns>
        public static EmitterModifier Read(IValueReader reader)
        {
            // Get the type
            string typeName = reader.ReadString(_typeKeyName);

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
            modifier.ReadCustomValues(customValueReader);

            return modifier;
        }

        /// <summary>
        /// When overridden in the derived class, reads the <see cref="EmitterModifier"/>'s custom values
        /// from the <see cref="reader"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read the custom values from.</param>
        protected abstract void ReadCustomValues(IValueReader reader);

        /// <summary>
        /// Writes the <see cref="EmitterModifier"/> to the <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        public void Write(IValueWriter writer)
        {
            string typeName = _typeFactory.GetTypeName(GetType());

            writer.Write(_typeKeyName, typeName);

            writer.WriteStartNode(_customValuesNodeName);
            {
                WriteCustomValues(writer);
            }
            writer.WriteEndNode(_customValuesNodeName);
        }

        /// <summary>
        /// When overridden in the derived class, writes all custom state values to the <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the state values to.</param>
        protected abstract void WriteCustomValues(IValueWriter writer);
    }
}