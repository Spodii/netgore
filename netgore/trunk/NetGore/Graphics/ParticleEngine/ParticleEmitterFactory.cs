using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// A factory for creating <see cref="ParticleEmitter"/>s for particle effects.
    /// </summary>
    public class ParticleEmitterFactory : TypeFactory
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The suffix for <see cref="ParticleEmitter"/> files, not including the prefixed period.
        /// </summary>
        public const string EmitterFileSuffix = "xml";

        const string _emitterNodeName = "Emitter";
        const string _emitterTypeKeyName = "EmitterType";
        const string _rootNodeName = "ParticleEffect";

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
                ConstructorParameters = Type.EmptyTypes,
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
        /// Gets the name of a particle effect from the file path.
        /// </summary>
        /// <param name="filePath">The file path for the particle effect.</param>
        /// <returns>The name of the particle effect, or "Unnamed" if the <paramref name="filePath"/> is invalid.</returns>
        public static string GetEffectNameFromPath(string filePath)
        {
            try
            {
                return Path.GetFileNameWithoutExtension(filePath);
            }
            catch (ArgumentException)
            {
                return filePath ?? "Unnamed";
            }
        }

        /// <summary>
        /// Gets the name of all the <see cref="ParticleEmitter"/> files in a given <see cref="ContentPaths"/>.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to get the effect files from.</param>
        /// <returns>The name of all the <see cref="ParticleEmitter"/> files in the <paramref name="contentPath"/>.</returns>
        public static IEnumerable<string> GetEffectsInPath(ContentPaths contentPath)
        {
            var files = Directory.GetFiles(contentPath.ParticleEffects, "*." + EmitterFileSuffix, SearchOption.TopDirectoryOnly);
            var names = files.Select(GetEffectNameFromPath);
            return names.ToImmutable();
        }

        /// <summary>
        /// Gets the file path for a <see cref="ParticleEmitter"/> by name.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/>.</param>
        /// <param name="emitterName">The name of the <see cref="ParticleEmitter"/>.</param>
        /// <returns>The path for the <see cref="ParticleEmitter"/>.</returns>
        static string GetFilePath(ContentPaths contentPath, string emitterName)
        {
            return contentPath.ParticleEffects.Join(emitterName + "." + EmitterFileSuffix);
        }

        /// <summary>
        /// Checks if a <see cref="ParticleEmitter"/> exists.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/>.</param>
        /// <param name="emitterName">The name of the <see cref="ParticleEmitter"/>.</param>
        /// <returns>True if the <see cref="ParticleEmitter"/> exists; false is it does not exist or any
        /// of the parameters are invalid.</returns>
        public static bool EmitterExists(ContentPaths contentPath, string emitterName)
        {
            if (contentPath == null || string.IsNullOrEmpty(emitterName))
                return false;

            var filePath = GetFilePath(contentPath, emitterName);
            return File.Exists(filePath);
        }

        /// <summary>
        /// Loads a <see cref="ParticleEmitter"/> from file.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to load from.</param>
        /// <param name="emitterName">The unique name of the <see cref="ParticleEmitter"/>.</param>
        /// <returns>The loaded <see cref="ParticleEmitter"/>, or null if the emitter does not exist or was not valid.</returns>
        public static ParticleEmitter LoadEmitter(ContentPaths contentPath, string emitterName)
        {
            var filePath = GetFilePath(contentPath, emitterName);

            // Ensure the file exists
            if (!File.Exists(filePath))
                return null;

            if (log.IsInfoEnabled)
                log.InfoFormat("Loading ParticleEmitter `{0}` from `{1}`.", emitterName, filePath);

            // Get the reader and read the emitter
            var reader = new XmlValueReader(filePath, _rootNodeName);
            var emitter = Read(reader);

            Debug.Assert(emitter.Name.Equals(emitterName, StringComparison.OrdinalIgnoreCase));

            return emitter;
        }

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
            ParticleEmitter emitter;
            try
            {
                emitter = (ParticleEmitter)Instance.GetTypeInstance(emitterTypeString);
            }
            catch (KeyNotFoundException ex)
            {
                throw new ParticleEmitterLoadEmitterException(emitterTypeString, ex);
            }

            // Grab the reader for the emitter node, then read the values into the emitter
            var emitterReader = reader.ReadNode(_emitterNodeName);
            emitter.Read(emitterReader);

            return emitter;
        }

        /// <summary>
        /// Saves a <see cref="ParticleEmitter"/> to file.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to save to.</param>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> to save.</param>
        public static void SaveEmitter(ContentPaths contentPath, ParticleEmitter emitter)
        {
            var filePath = GetFilePath(contentPath, emitter.Name);

            if (log.IsInfoEnabled)
                log.InfoFormat("Saving ParticleEmitter `{0}` to `{1}`.", emitter, filePath);

            // Create the writer and begin writing
            using (var writer = new XmlValueWriter(filePath, _rootNodeName))
            {
                Write(writer, emitter);
            }
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
    }
}