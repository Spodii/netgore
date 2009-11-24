using System;
using System.Collections.Generic;
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

        public static ParticleEmitter LoadEmitter(ContentPaths contentPath, string emitterName)
        {
            // TODO: !! ...
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves a <see cref="ParticleEmitter"/> to file.
        /// </summary>
        /// <param name="contentPath">The <see cref="ContentPaths"/> to save to.</param>
        /// <param name="emitter">The <see cref="ParticleEmitter"/> to save.</param>
        /// <param name="emitterName">The unique name to give the <paramref name="emitter"/>.</param>
        public static void SaveEmitter(ContentPaths contentPath, ParticleEmitter emitter, string emitterName)
        {
            var filePath = contentPath.ParticleEffects.Join(emitterName + "." + EmitterFileSuffix);
            using (var writer = new XmlValueWriter(filePath, "ParticleEffect"))
            {
                Write(writer, emitter);
            }
        }

        const string _emitterTypeKeyName = "EmitterType";
        const string _emitterNodeName = "Emitter";

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
