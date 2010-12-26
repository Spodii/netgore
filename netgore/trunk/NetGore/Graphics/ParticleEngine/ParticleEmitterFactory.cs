using System;
using System.Collections.Generic;
using System.Linq;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// A factory for creating <see cref="ParticleEmitter"/>s for particle effects.
    /// </summary>
    public sealed class ParticleEmitterFactory : TypeFactory
    {
        const string _emitterNodeName = "Emitter";
        const string _emitterTypeKeyName = "EmitterType";

        /// <summary>
        /// The only instance of the <see cref="ParticleEmitterFactory"/>.
        /// </summary>
        static readonly ParticleEmitterFactory _instance;

        /// <summary>
        /// The type filter used for the <see cref="ParticleEmitter"/> types.
        /// </summary>
        static readonly Func<Type, bool> _particleEmitterFilter;

        /// <summary>
        /// Initializes the <see cref="ParticleEmitterFactory"/> class.
        /// </summary>
        static ParticleEmitterFactory()
        {
            // Create the filter
            var filterCreator = new TypeFilterCreator
            {
                IsClass = true,
                IsAbstract = false,
                RequireConstructor = true,
                ConstructorParameters = new Type[] { typeof(IParticleEffect) },
                Subclass = typeof(ParticleEmitter)
            };

            _particleEmitterFilter = filterCreator.GetFilter();

            // Create the factory instance
            _instance = new ParticleEmitterFactory();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterFactory"/> class.
        /// </summary>
        ParticleEmitterFactory() : base(_particleEmitterFilter)
        {
        }

        /// <summary>
        /// Gets the <see cref="ParticleEmitterFactory"/> instance.
        /// </summary>
        public static ParticleEmitterFactory Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Reads a <see cref="ParticleEmitter"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read from.</param>
        /// <param name="owner">The <see cref="IParticleEffect"/> to add the read <see cref="ParticleEmitter"/> to.</param>
        /// <returns>
        /// The <see cref="ParticleEmitter"/> read from the <paramref name="reader"/>.
        /// </returns>
        /// <exception cref="ParticleEmitterLoadEmitterException">The <see cref="ParticleEmitter"/> could not be loaded.</exception>
        public static ParticleEmitter Read(IValueReader reader, IParticleEffect owner)
        {
            // Get the type name
            var emitterTypeString = reader.ReadString(_emitterTypeKeyName);

            // Create the instance using the type name
            ParticleEmitter emitter;
            try
            {
                emitter = (ParticleEmitter)Instance.GetTypeInstance(emitterTypeString, owner);
            }
            catch (KeyNotFoundException ex)
            {
                throw new ParticleEmitterLoadEmitterException(emitterTypeString, ex);
            }

            // Grab the reader for the emitter node, then read the values into the emitter
            var emitterReader = reader.ReadNode(_emitterNodeName);
            emitter.ReadState(emitterReader);

            return emitter;
        }

        /// <summary>
        /// Writes a <see cref="ParticleEmitter"/> to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> to write.</param>
        public static void Write(IValueWriter writer, ParticleEmitter emitter)
        {
            writer.Write(_emitterTypeKeyName, Instance.GetTypeName(emitter.GetType()));
            writer.WriteStartNode(_emitterNodeName);
            {
                emitter.WriteState(writer);
            }
            writer.WriteEndNode(_emitterNodeName);
        }
    }
}