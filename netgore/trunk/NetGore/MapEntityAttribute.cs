using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MapFileEntityAttribute : Attribute
    {
        static IEnumerable<Type> _types = null;

        public static IEnumerable<Type> GetTypes()
        {
            if (_types == null)
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                var types = assemblies.SelectMany(x => x.GetTypes());
                types = types.Where(x => x.IsClass && !x.IsAbstract);
                types = types.Where(x => x.GetCustomAttributes(typeof(MapFileEntityAttribute), true).Length > 0);

                _types = types;
            }

            return _types;
        }
    }
}