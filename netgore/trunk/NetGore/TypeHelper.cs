using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;

namespace NetGore
{
    /// <summary>
    /// Provides helper functions for the Type class.
    /// </summary>
    public static class TypeHelper
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets all Types from every Assembly.
        /// </summary>
        /// <returns>All Types from every Assembly.</returns>
        public static IEnumerable<Type> AllTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Distinct();
        }

        /// <summary>
        /// Finds all Types that inherit the specified <paramref name="baseClass"/>.
        /// </summary>
        /// <param name="baseClass">Base class or interface to find the classes that inherit.</param>
        /// <param name="parameterlessConstructor">If true, a MissingMethodException will be thrown if a non-abstract Type
        /// is found that does not implement a parameterless constructor.</param>
        /// <returns>IEnumerable of all Types that inherit the specified <paramref name="baseClass"/>.</returns>
        public static IEnumerable<Type> FindTypesThatInherit(Type baseClass, bool parameterlessConstructor)
        {
            var types = AllTypes().Where(x => x.IsSubclassOf(baseClass));
            if (parameterlessConstructor)
            {
                foreach (Type type in types)
                {
                    if (!type.IsAbstract && type.GetConstructor(new Type[] { }) == null)
                    {
                        const string errmsg = "No parameterless constructor found for type `{0}`.";
                        string err = string.Format(errmsg, type);
                        Debug.Fail(err);
                        log.Fatal(err);
                        throw new MissingMethodException(err);
                    }
                }
            }

            return types;
        }
    }
}
