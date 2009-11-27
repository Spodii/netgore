using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NetGore
{
    public static class MethodInfoHelper
    {
        public static IEnumerable<MethodInfo> FindMethodsWithAttribute<T>(IEnumerable<Type> types) where T : Attribute
        {
            return FindMethodsWithAttribute<T>(types, x => true);
        }

        public static IEnumerable<MethodInfo> FindMethodsWithAttribute<T>(IEnumerable<Type> types,
                                                                          Func<MethodInfo, bool> conditions) where T : Attribute
        {
            var internalConditions = new Func<MethodInfo, bool>(x => x.GetCustomAttributes(typeof(T), true).Length > 0);
            return GetMethods(types).Where(x => internalConditions(x) && conditions(x));
        }

        public static IEnumerable<MethodInfo> FindStaticMethodsWithAttribute<T>(IEnumerable<Type> types) where T : Attribute
        {
            return FindStaticMethodsWithAttribute<T>(types, x => true);
        }

        public static IEnumerable<MethodInfo> FindStaticMethodsWithAttribute<T>(IEnumerable<Type> types,
                                                                                Func<MethodInfo, bool> conditions)
            where T : Attribute
        {
            var internalConditions = new Func<MethodInfo, bool>(x => x.GetCustomAttributes(typeof(T), true).Length > 0);
            return GetStaticMethods(types).Where(x => internalConditions(x) && conditions(x));
        }

        public static IEnumerable<MethodInfo> GetMethods(IEnumerable<Type> types)
        {
            const BindingFlags flags =
                BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic |
                BindingFlags.Public;
            return types.SelectMany(x => x.GetMethods(flags));
        }

        public static IEnumerable<MethodInfo> GetStaticMethods(IEnumerable<Type> types)
        {
            const BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
            return types.SelectMany(x => x.GetMethods(flags));
        }
    }
}