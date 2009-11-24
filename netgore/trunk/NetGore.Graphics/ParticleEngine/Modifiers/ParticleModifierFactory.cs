using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore.Collections;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Factory that caches instances of the <see cref="ParticleModifierBase"/>s.
    /// </summary>
    internal class ParticleModifierFactory : TypeFactory
    {
        /// <summary>
        /// Dictionary of the modifier Types and the instances for the Type.
        /// </summary>
        static readonly Dictionary<Type, ParticleModifierBase> _instances = new Dictionary<Type, ParticleModifierBase>();

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
                Subclass = typeof(ParticleModifierBase),
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
        /// Gets a <see cref="ParticleModifierBase"/> instance of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="ParticleModifierBase"/>.</typeparam>
        /// <returns>A <see cref="ParticleModifierBase"/> instance of type <typeparamref name="T"/>.</returns>
        public static T GetInstance<T>() where T : ParticleModifierBase
        {
            var type = typeof(T);
            var ret = _instances[type];
            return (T)ret;
        }

        static void HandleTypeLoaded(TypeFactory typefactory, Type loadedtype, string name)
        {
            if (_instances.ContainsKey(loadedtype))
            {
                Debug.Fail("Why did we end up trying to load the same Type more than once? Did someone else instantiate this? Hmm...");
                return;
            }

            var instance = (ParticleModifierBase)GetTypeInstance(loadedtype);

            // Lock when adding instances just in case... who knows
            // Not like this lock will cost us more than a few microseconds
            lock (_instancesSync)
            {
                _instances.Add(loadedtype, instance);
            }
        }
    }
}