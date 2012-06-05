using System;
using System.Linq;
using NetGore.Collections;

namespace NetGore.Graphics.ParticleEngine
{
    /// <summary>
    /// Factory that caches instances of the <see cref="EmitterModifier"/>s.
    /// </summary>
    public sealed class EmitterModifierFactory : TypeFactory
    {
        /// <summary>
        /// The only instance of the <see cref="EmitterModifierFactory"/>.
        /// </summary>
        static readonly EmitterModifierFactory _instance;

        /// <summary>
        /// Initializes the <see cref="EmitterModifierFactory"/> class.
        /// </summary>
        static EmitterModifierFactory()
        {
            // Create the factory instance
            _instance = new EmitterModifierFactory();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmitterModifierFactory"/> class.
        /// </summary>
        EmitterModifierFactory() : base(GetTypeFilter())
        {
        }

        /// <summary>
        /// Gets the <see cref="EmitterModifierFactory"/> instance.
        /// </summary>
        public static EmitterModifierFactory Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Gets the type filter for the <see cref="EmitterModifierFactory"/>.
        /// </summary>
        /// <returns>The type filter for the <see cref="EmitterModifierFactory"/>.</returns>
        static Func<Type, bool> GetTypeFilter()
        {
            var filterCreator = new TypeFilterCreator
            {
                IsClass = true,
                Subclass = typeof(EmitterModifier),
                IsAbstract = false,
                RequireConstructor = true,
                ConstructorParameters = Type.EmptyTypes,
            };

            return filterCreator.GetFilter();
        }
    }
}