using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore.Collections;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Factory that caches instances of the <see cref="ParticleModifier"/>s.
    /// </summary>
    internal class ParticleModifierFactory : TypeFactory
    {
        /// <summary>
        /// Dictionary of the modifier Types and the instances for the Type.
        /// </summary>
        static readonly Dictionary<Type, ParticleModifier> _instances = new Dictionary<Type, ParticleModifier>();

        /// <summary>
        /// Sync for the <see cref="_instances"/>.
        /// </summary>
        static readonly object _instancesSync = new object();

        /// <summary>
        /// The only instance of the <see cref="ParticleModifierFactory"/>.
        /// </summary>
        static readonly ParticleModifierFactory _instance;

        /// <summary>
        /// Initializes the <see cref="ParticleModifierFactory"/> class.
        /// </summary>
        static ParticleModifierFactory()
        {
            // Create the factory instance
            _instance = new ParticleModifierFactory();
        }

        /// <summary>
        /// Gets an IEnumerable of the Types in this <see cref="TypeFactory"/>.
        /// </summary>
        public static IEnumerable<Type> ModifierTypes { get { return _instance; } }

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

        /// <summary>
        /// Initializes a new instance of the <see cref="ParticleModifierFactory"/> class.
        /// </summary>
        ParticleModifierFactory()
            : base(GetTypeFilter(), HandleTypeLoaded, false)
        {
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

        static void HandleTypeLoaded(TypeFactory typefactory, Type loadedtype, string name)
        {
            if (_instances.ContainsKey(loadedtype))
            {
                Debug.Fail("Why did we end up trying to load the same Type more than once? Did someone else instantiate this? Hmm...");
                return;
            }

            var instance = (ParticleModifier)GetTypeInstance(loadedtype);

            // Lock when adding instances just in case... who knows
            // Not like this lock will cost us more than a few microseconds
            lock (_instancesSync)
            {
                _instances.Add(loadedtype, instance);
            }
        }
    }
}