using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Scripting
{
    /// <summary>
    /// Static helper class for Type usage involving scripting.
    /// </summary>
    public static class ScriptTypeHelper
    {
        static readonly Dictionary<ScriptTypeCollection, IEnumerable<Type>> _cachedScriptTypes = new Dictionary<ScriptTypeCollection, IEnumerable<Type>>();
        static readonly IEnumerable<Type> _assemblyTypes;

        static ScriptTypeHelper()
        {
            _assemblyTypes = FilterInstanceable(TypeHelper.AllTypes());
        }

        /// <summary>
        /// Gets all of the instanceable Types for a ScriptTypeCollection.
        /// </summary>
        /// <param name="scriptTypes">ScriptTypeCollection to get the instanceable Types for.</param>
        /// <returns>All of the instanceable Types for a ScriptTypeCollection.</returns>
        static IEnumerable<Type> GetTypesForScript(ScriptTypeCollection scriptTypes)
        {
            IEnumerable<Type> types;
            if (!_cachedScriptTypes.TryGetValue(scriptTypes, out types))
            {
                types = FilterInstanceable(scriptTypes);
                _cachedScriptTypes.Add(scriptTypes, types);
            }

            return types;
        }

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
        /// Finds every instanceable Type that is defined in the code or in scripts that meets the specified conditions.
        /// </summary>
        /// <param name="scriptTypes">ScriptTypeCollection to get the scripted Types from.</param>
        /// <param name="subclassType">The Type that each type must be a subclass of.</param>
        /// <param name="constructorParams">Parameters that the constructor must contain.</param>
        /// <returns>Every Type that satisfies the given conditions.</returns>
        public static IEnumerable<Type> GetTypes(ScriptTypeCollection scriptTypes, Type subclassType, Type[] constructorParams)
        {
            var allTypes = _assemblyTypes.Concat(GetTypesForScript(scriptTypes));

            if (subclassType != null)
                allTypes = allTypes.Where(x => x.IsSubclassOf(subclassType));

            allTypes = allTypes.Where(x => x.GetConstructor(constructorParams) != null);

            return allTypes;
        }
    }
}
