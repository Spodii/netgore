using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Scripting
{
    /// <summary>
    /// Static helper class for Type usage involving scripting.
    /// </summary>
    public static class ScriptTypeHelper
    {
        /// <summary>
        /// Filters an IEnumerable of Types to only the instanceable types.
        /// </summary>
        /// <param name="types">Types to filter.</param>
        /// <returns>The <paramref name="types"/> that are instanceable.</returns>
        static IEnumerable<Type> FilterInstanceable(IEnumerable<Type> types)
        {
            return types.Where(x => !x.IsAbstract && !x.IsInterface && x.IsClass);
        }

        /// <summary>
        /// Gets all Types from every Assembly.
        /// </summary>
        /// <returns>All Types from every Assembly.</returns>
        public static IEnumerable<Type> AllTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Distinct();
        }

        /// <summary>
        /// Finds every instanceable Type that is defined in the code or in scripts that meets the specified conditions.
        /// </summary>
        /// <param name="subclassType">The Type that each type must be a subclass of.</param>
        /// <param name="constructorParams">Parameters that the constructor must contain.</param>
        /// <returns>Every Type that satisfies the given conditions.</returns>
        public static IEnumerable<Type> GetTypes(Type subclassType, Type[] constructorParams)
        {
            var allTypes = FilterInstanceable(AllTypes());

            if (subclassType != null)
                allTypes = allTypes.Where(x => x.IsSubclassOf(subclassType));

            allTypes = allTypes.Where(x => x.GetConstructor(constructorParams) != null);

            return allTypes;
        }
    }
}