using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Factory that caches instances of the <see cref="ParticleModifier"/>s.
    /// </summary>
    internal class ParticleModifierFactory : TypeFactory
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The only instance of the <see cref="ParticleModifierFactory"/>.
        /// </summary>
        static readonly ParticleModifierFactory _instance;

        /// <summary>
        /// Dictionary of the modifier Types and the instances for the Type.
        /// </summary>
        static readonly Dictionary<Type, ParticleModifier> _instances = new Dictionary<Type, ParticleModifier>();

        /// <summary>
        /// Dictionary of the modifier Types and the instances for the Type's name.
        /// </summary>
        static readonly Dictionary<string, ParticleModifier> _instancesByName = new Dictionary<string, ParticleModifier>();

        /// <summary>
        /// Sync for the <see cref="_instances"/>.
        /// </summary>
        static readonly object _instancesSync = new object();

        /// <summary>
        /// Initializes the <see cref="ParticleModifierFactory"/> class.
        /// </summary>
        static ParticleModifierFactory()
        {
            // Create the factory instance
            _instance = new ParticleModifierFactory();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleModifierFactory"/> class.
        /// </summary>
        ParticleModifierFactory() : base(GetTypeFilter(), HandleTypeLoaded, false)
        {
        }

        /// <summary>
        /// Gets an IEnumerable of the Types in this <see cref="TypeFactory"/>.
        /// </summary>
        public static IEnumerable<Type> ModifierTypes
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets the factory type name of a <see cref="ParticleModifier"/>.
        /// </summary>
        /// <param name="type">The Type of the <see cref="ParticleModifier"/>.</param>
        /// <returns>The factory type name of the <see cref="ParticleModifier"/>.</returns>
        public static string GetFactoryTypeName(Type type)
        {
            return _instance.GetTypeName(type);
        }

        /// <summary>
        /// Gets a <see cref="ParticleModifier"/> instance of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ParticleModifier"/>.</typeparam>
        /// <returns>A <see cref="ParticleModifier"/> instance of type <typeparamref name="T"/>.</returns>
        public static T GetInstance<T>() where T : ParticleModifier
        {
            return (T)GetInstance(typeof(T));
        }

        /// <summary>
        /// Gets a <see cref="ParticleModifier"/> instance .
        /// </summary>
        /// <param name="type">The Type of <see cref="ParticleModifier"/>.</param>
        /// <returns>A <see cref="ParticleModifier"/> instance.</returns>
        public static ParticleModifier GetInstance(Type type)
        {
            var ret = _instances[type];
            return ret;
        }

        /// <summary>
        /// Gets a <see cref="ParticleModifier"/> instance .
        /// </summary>
        /// <param name="typeName">The name of the <see cref="ParticleModifier"/> type as given by
        /// GetFactoryTypeName().</param>
        /// <returns>A <see cref="ParticleModifier"/> instance.</returns>
        public static ParticleModifier GetInstance(string typeName)
        {
            var ret = _instancesByName[typeName];
            return ret;
        }

        /// <summary>
        /// Gets the type filter for the <see cref="ParticleModifierFactory"/>.
        /// </summary>
        /// <returns>The type filter for the <see cref="ParticleModifierFactory"/>.</returns>
        static Func<Type, bool> GetTypeFilter()
        {
            var filterCreator = new TypeFilterCreator
            {
                IsClass = true,
                Subclass = typeof(ParticleModifier),
                IsAbstract = false,
                RequireConstructor = true,
                ConstructorParameters = Type.EmptyTypes,
            };

            return filterCreator.GetFilter();
        }

        static void HandleTypeLoaded(TypeFactory typefactory, Type loadedtype, string name)
        {
            if (_instances.ContainsKey(loadedtype))
            {
                const string errmsg =
                    "Why did we end up trying to load the same Type more than once?" +
                    " Did someone else instantiate this? Hmm...";
                Debug.Fail(errmsg);
                if (log.IsErrorEnabled)
                    log.Error(errmsg);
                return;
            }

            // Ensure all names are unique
            if (_instancesByName.ContainsKey(name))
            {
                const string errmsg =
                    "The ParticleModifierFactory has already loaded a modifier Type with the name `{0}`." +
                    " Make sure all modifiers have a unique name.";
                string err = string.Format(errmsg, name);
                Debug.Fail(err);
                if (log.IsFatalEnabled)
                    log.Fatal(err);
                throw new TypeException(err, loadedtype);
            }

            var instance = (ParticleModifier)GetTypeInstance(loadedtype);

            // Lock when adding instances just in case... who knows
            // Not like this lock will cost us more than a few microseconds
            lock (_instancesSync)
            {
                _instances.Add(loadedtype, instance);
                _instancesByName.Add(name, instance);
            }
        }
    }
}