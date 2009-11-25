using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.Graphics.ParticleEngine
{
    public class ParticleEmitterFactory : TypeFactory
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the <see cref="ParticleEmitterFactory"/> instance.
        /// </summary>
        public static ParticleEmitterFactory Instance { get { return _instance; } }

        /// <summary>
        /// The only instance of the <see cref="ParticleEmitterFactory"/>.
        /// </summary>
        static readonly ParticleEmitterFactory _instance;

        /// <summary>
        /// Initializes the <see cref="ParticleEmitterFactory"/> class.
        /// </summary>
        static ParticleEmitterFactory()
        {
            // Create the factory instance
            _instance = new ParticleEmitterFactory();
        }

        /// <summary>
        /// The suffix for <see cref="ParticleEmitter"/> files, not including the prefixed period.
        /// </summary>
        public const string EmitterFileSuffix = "xml";

        /// <summary>
        /// Loads a <see cref="ParticleEmitter"/> from file.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to load from.</param>
        /// <param name="emitterName">The unique name of the <see cref="ParticleEmitter"/>.</param>
        /// <returns>The loaded <see cref="ParticleEmitter"/>.</returns>
        /// <exception cref="ParticleEmitterNotFoundException">No emitter found with the given
        /// <paramref name="emitterName"/>.</exception>
        public static ParticleEmitter LoadEmitter(ContentPaths contentPath, string emitterName)
        {
            var filePath = GetFilePath(contentPath, emitterName);

            if (log.IsInfoEnabled)
                log.InfoFormat("Loading ParticleEmitter `{0}` from `{1}`.", emitterName, filePath);

            // Ensure the file exists
            if (!File.Exists(filePath))
                throw new ParticleEmitterNotFoundException(emitterName);

            // Get the reader and read the emitter
            XmlValueReader reader = new XmlValueReader(filePath, _rootNodeName);
            var emitter = Read(reader);

            return emitter;
        }

        static string GetFilePath(ContentPaths contentPath, string emitterName)
        {
            return contentPath.ParticleEffects.Join(emitterName + "." + EmitterFileSuffix);
        }

        /// <summary>
        /// Saves a <see cref="ParticleEmitter"/> to file.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to save to.</param>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> to save.</param>
        /// <param name="emitterName">The unique name of the <paramref name="emitter"/>.</param>
        public static void SaveEmitter(ContentPaths contentPath, ParticleEmitter emitter, string emitterName)
        {
            var filePath = GetFilePath(contentPath, emitterName);

            if (log.IsInfoEnabled)
                log.InfoFormat("Saving ParticleEmitter `{0}` to `{1}`.", emitter, filePath);

            // Create the writer and begin writing
            using (var writer = new XmlValueWriter(filePath, _rootNodeName))
            {
                Write(writer, emitter);
            }
        }

        const string _rootNodeName = "ParticleEffect";
        const string _emitterTypeKeyName = "EmitterType";
        const string _emitterNodeName = "Emitter";

        /// <summary>
        /// Reads a <see cref="ParticleEmitter"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read from.</param>
        /// <returns>The <see cref="ParticleEmitter"/> read from the <paramref name="reader"/>.</returns>
        public static ParticleEmitter Read(IValueReader reader)
        {
            // Get the type name
            var emitterTypeString = reader.ReadString(_emitterTypeKeyName);

            // Create the instance using the type name
            var emitter = (ParticleEmitter)Instance.GetTypeInstance(emitterTypeString);

            // Grab the reader for the emitter node, then read the values into the emitter
            var emitterReader = reader.ReadNode(_emitterNodeName);
            emitter.Read(emitterReader);

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
                emitter.Write(writer);
            }
            writer.WriteEndNode(_emitterNodeName);
        }

        static Func<Type, bool> GetTypeFilter()
        {
            var filterCreator = new TypeFilterCreator
            {
                IsClass = true,
                IsAbstract = false,
                RequireConstructor = true,
                ConstructorParameters = Type.EmptyTypes,
                Subclass = typeof(ParticleEmitter)
            };

            return filterCreator.GetFilter();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleEmitterFactory"/> class.
        /// </summary>
        ParticleEmitterFactory()
            : base(GetTypeFilter(), null, false)
        {
        }
    }
}
