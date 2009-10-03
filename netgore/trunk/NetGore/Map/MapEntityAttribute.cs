using System;
using System.Collections.Generic;
using System.Linq;
using NetGore;

namespace NetGore
{
    /// <summary>
    /// Attribute that marks an Entity as being able to be stored in the map file. Entities with this attribute
    /// will appear in the map editor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MapFileEntityAttribute : Attribute
    {
        /// <summary>
        /// Cache of the Types found in GetTypes().
        /// </summary>
        static IEnumerable<Type> _types = null;

        /// <summary>
        /// Gets an IEnumerable of all Types with the MapFileEntityAttribute and inherit from Entity.
        /// </summary>
        /// <returns>An IEnumerable of all Types with the MapFileEntityAttribute and inherit from Entity.</returns>
        public static IEnumerable<Type> GetTypes()
        {
            if (_types == null)
            {
                var types = TypeHelper.FindTypesThatInherit(typeof(Entity), null, false);
                types = types.Where(x => x.GetCustomAttributes(typeof(MapFileEntityAttribute), true).Length > 0);
                _types = types.ToArray();
            }

            return _types;
        }
    }
}