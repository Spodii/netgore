using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;
using NetGore.IO;

namespace NetGore.Graphics
{
    /// <summary>
    /// A factory for creating persistable <see cref="IRefractionEffect"/>s.
    /// A valid persistable <see cref="IRefractionEffect"/>s must inherit <see cref="IPersistable"/> and contain a default
    /// constructor.
    /// </summary>
    public class RefractionEffectFactory : TypeFactory
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        const string _refractionEffectNodeName = "RefractionEffect";
        const string _refractionEffectTypeKeyName = "RefractionEffectType";

        /// <summary>
        /// The type filter used for the <see cref="IRefractionEffect"/> types.
        /// </summary>
        static readonly Func<Type, bool> _filter;

        /// <summary>
        /// The only instance of the <see cref="RefractionEffectFactory"/>.
        /// </summary>
        static readonly RefractionEffectFactory _instance;

        /// <summary>
        /// Initializes the <see cref="RefractionEffectFactory"/> class.
        /// </summary>
        static RefractionEffectFactory()
        {
            // Create the filter
            var fc = new TypeFilterCreator
            {
                IsAbstract = false,
                IsClass = true,
                Interfaces = new Type[] { typeof(IPersistable), typeof(IRefractionEffect) },
                ConstructorParameters = Type.EmptyTypes
            };

            _filter = fc.GetFilter();

            // Create the factory instance
            _instance = new RefractionEffectFactory();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RefractionEffectFactory"/> class.
        /// </summary>
        RefractionEffectFactory() : base(_filter)
        {
        }

        /// <summary>
        /// Gets the <see cref="RefractionEffectFactory"/> instance.
        /// </summary>
        static RefractionEffectFactory Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets the <see cref="Type"/>s of the valid persistable <see cref="IRefractionEffect"/>s.
        /// </summary>
        public static IEnumerable<Type> ValidTypes
        {
            get { return Instance.Types; }
        }

        /// <summary>
        /// Gets if a given <see cref="Type"/> is a valid type for this factory.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to check.</param>
        /// <returns>True if the <paramref name="type"/> is valid for this factory; otherwise false.</returns>
        public static bool IsValidType(Type type)
        {
            return Instance[type] != null;
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling the corresponding event without
        /// the overhead of using event hooks. Therefore, it is recommended that this overload is used instead of
        /// the corresponding event when possible.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="typeName">Name of the type.</param>
        protected override void OnTypeLoaded(Type type, string typeName)
        {
            base.OnTypeLoaded(type, typeName);

            if (log.IsDebugEnabled)
                log.DebugFormat("Found persistable refraction effect type: `{0}`", type);
        }

        /// <summary>
        /// Reads a <see cref="IRefractionEffect"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="IValueReader"/> to read from.</param>
        /// <returns>The <see cref="IRefractionEffect"/> instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public static IRefractionEffect Read(IValueReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            var typeName = reader.ReadString(_refractionEffectTypeKeyName);
            var nodeReader = reader.ReadNode(_refractionEffectNodeName);

            var instance = (IPersistable)Instance.GetTypeInstance(typeName);
            instance.ReadState(nodeReader);

            return (IRefractionEffect)instance;
        }

        /// <summary>
        /// Writes a <see cref="IRefractionEffect"/> to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="IValueWriter"/> to write to.</param>
        /// <param name="re">The <see cref="IRefractionEffect"/> to write.</param>
        /// <exception cref="ArgumentException"><paramref name="re"/> is not in the <see cref="ValidTypes"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="re"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public static void Write(IValueWriter writer, IRefractionEffect re)
        {
            if (writer == null)
                throw new ArgumentNullException("writer");

            if (re == null)
                throw new ArgumentNullException("re");

            var asPersistable = re as IPersistable;

            string typeName = null;
            if (asPersistable != null)
                typeName = Instance[re.GetType()];

            if (typeName == null)
            {
                const string errmsg = "Type `{0}` is not a valid persistable IRefractionEffect type.";
                if (log.IsErrorEnabled)
                    log.ErrorFormat(errmsg, re.GetType());
                throw new ArgumentException(string.Format(errmsg, re.GetType()), "re");
            }

            writer.Write(_refractionEffectTypeKeyName, typeName);
            writer.WriteStartNode(_refractionEffectNodeName);
            {
                asPersistable.WriteState(writer);
            }
            writer.WriteEndNode(_refractionEffectNodeName);
        }
    }
}