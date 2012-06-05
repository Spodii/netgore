using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace NetGore
{
    /// <summary>
    /// Helper class for finding the <see cref="MethodInfo"/> for methods with various conditions.
    /// </summary>
    public static class MethodInfoHelper
    {
        /// <summary>
        /// Finds all instance methods in the specified <paramref name="types"/> that contain the given <see cref="Attribute"/>
        /// specified by <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The Type of <see cref="Attribute"/>.</typeparam>
        /// <param name="types">The class <see cref="Type"/>s to check the methods of.</param>
        /// <returns>The <see cref="MethodInfo"/>s of the instance methods from the given <paramref name="types"/>
        /// that contain the <see cref="Attribute"/> defined by <typeparamref name="T"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IEnumerable<MethodInfo> FindInstanceMethodsWithAttribute<T>(IEnumerable<Type> types) where T : Attribute
        {
            return FindInstanceMethodsWithAttribute<T>(types, x => true);
        }

        /// <summary>
        /// Finds all instance methods in the specified <paramref name="types"/> that contain the given <see cref="Attribute"/>
        /// specified by <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The Type of <see cref="Attribute"/>.</typeparam>
        /// <param name="types">The class <see cref="Type"/>s to check the methods of.</param>
        /// <param name="conditions">The additional conditions each method must pass in order to be included
        /// in the returned values.</param>
        /// <returns>The <see cref="MethodInfo"/>s of the instance methods from the given <paramref name="types"/>
        /// that contain the <see cref="Attribute"/> defined by <typeparamref name="T"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IEnumerable<MethodInfo> FindInstanceMethodsWithAttribute<T>(IEnumerable<Type> types,
                                                                                  Func<MethodInfo, bool> conditions)
            where T : Attribute
        {
            var internalConditions = new Func<MethodInfo, bool>(x => x.GetCustomAttributes(typeof(T), true).Length > 0);
            return GetInstanceMethods(types).Where(x => internalConditions(x) && conditions(x));
        }

        /// <summary>
        /// Finds all methods in the specified <paramref name="types"/> that contain the given <see cref="Attribute"/>
        /// specified by <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The Type of <see cref="Attribute"/>.</typeparam>
        /// <param name="types">The class <see cref="Type"/>s to check the methods of.</param>
        /// <returns>The <see cref="MethodInfo"/>s of the methods from the given <paramref name="types"/>
        /// that contain the <see cref="Attribute"/> defined by <typeparamref name="T"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IEnumerable<MethodInfo> FindMethodsWithAttribute<T>(IEnumerable<Type> types) where T : Attribute
        {
            return FindMethodsWithAttribute<T>(types, x => true);
        }

        /// <summary>
        /// Finds all methods in the specified <paramref name="types"/> that contain the given <see cref="Attribute"/>
        /// specified by <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The Type of <see cref="Attribute"/>.</typeparam>
        /// <param name="types">The class <see cref="Type"/>s to check the methods of.</param>
        /// <param name="conditions">The additional conditions each method must pass in order to be included
        /// in the returned values.</param>
        /// <returns>The <see cref="MethodInfo"/>s of the methods from the given <paramref name="types"/>
        /// that contain the <see cref="Attribute"/> defined by <typeparamref name="T"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IEnumerable<MethodInfo> FindMethodsWithAttribute<T>(IEnumerable<Type> types,
                                                                          Func<MethodInfo, bool> conditions) where T : Attribute
        {
            var internalConditions = new Func<MethodInfo, bool>(x => x.GetCustomAttributes(typeof(T), true).Length > 0);
            return GetMethods(types).Where(x => internalConditions(x) && conditions(x));
        }

        /// <summary>
        /// Finds all static methods in the specified <paramref name="types"/> that contain the given <see cref="Attribute"/>
        /// specified by <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The Type of <see cref="Attribute"/>.</typeparam>
        /// <param name="types">The class <see cref="Type"/>s to check the methods of.</param>
        /// <returns>The <see cref="MethodInfo"/>s of the static methods from the given <paramref name="types"/>
        /// that contain the <see cref="Attribute"/> defined by <typeparamref name="T"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IEnumerable<MethodInfo> FindStaticMethodsWithAttribute<T>(IEnumerable<Type> types) where T : Attribute
        {
            return FindStaticMethodsWithAttribute<T>(types, x => true);
        }

        /// <summary>
        /// Finds all static methods in the specified <paramref name="types"/> that contain the given <see cref="Attribute"/>
        /// specified by <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The Type of <see cref="Attribute"/>.</typeparam>
        /// <param name="types">The class <see cref="Type"/>s to check the methods of.</param>
        /// <param name="conditions">The additional conditions each method must pass in order to be included
        /// in the returned values.</param>
        /// <returns>The <see cref="MethodInfo"/>s of the static methods from the given <paramref name="types"/>
        /// that contain the <see cref="Attribute"/> defined by <typeparamref name="T"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static IEnumerable<MethodInfo> FindStaticMethodsWithAttribute<T>(IEnumerable<Type> types,
                                                                                Func<MethodInfo, bool> conditions)
            where T : Attribute
        {
            var internalConditions = new Func<MethodInfo, bool>(x => x.GetCustomAttributes(typeof(T), true).Length > 0);
            return GetStaticMethods(types).Where(x => internalConditions(x) && conditions(x));
        }

        /// <summary>
        /// Finds all methods for a collection of <see cref="Type"/>s.
        /// </summary>
        /// <param name="types">The <see cref="Type"/>s to find the methods for.</param>
        /// <returns>The <see cref="MethodInfo"/>s for the methods in the given <paramref name="types"/>.</returns>
        public static IEnumerable<MethodInfo> GetInstanceMethods(IEnumerable<Type> types)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            return types.SelectMany(x => x.GetMethods(flags));
        }

        /// <summary>
        /// Finds all methods for a collection of <see cref="Type"/>s.
        /// </summary>
        /// <param name="types">The <see cref="Type"/>s to find the methods for.</param>
        /// <returns>The <see cref="MethodInfo"/>s for the methods in the given <paramref name="types"/>.</returns>
        public static IEnumerable<MethodInfo> GetMethods(IEnumerable<Type> types)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
            return types.SelectMany(x => x.GetMethods(flags));
        }

        /// <summary>
        /// Finds all static methods for a collection of <see cref="Type"/>s.
        /// </summary>
        /// <param name="types">The <see cref="Type"/>s to find the static methods for.</param>
        /// <returns>The <see cref="MethodInfo"/>s for the static methods in the given <paramref name="types"/>.</returns>
        public static IEnumerable<MethodInfo> GetStaticMethods(IEnumerable<Type> types)
        {
            const BindingFlags flags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
            return types.SelectMany(x => x.GetMethods(flags));
        }
    }
}