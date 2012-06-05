using System;
using System.Linq;
using NetGore.Collections;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Factory that caches instances of the <see cref="ParticleModifier"/>s.
    /// </summary>
    public sealed class ParticleModifierFactory : TypeFactory
    {
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
        /// Initializes a new instance of the <see cref="ParticleModifierFactory"/> class.
        /// </summary>
        ParticleModifierFactory() : base(GetTypeFilter())
        {
        }

        /// <summary>
        /// Gets the <see cref="ParticleModifierFactory"/> instance.
        /// </summary>
        public static ParticleModifierFactory Instance
        {
            get { return _instance; }
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
    }
}