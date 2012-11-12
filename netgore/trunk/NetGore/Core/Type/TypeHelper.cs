using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore
{
    /// <summary>
    /// Provides helper functions for the Type class.
    /// </summary>
    public static class TypeHelper
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        const bool _gacDefault = false;

        /// <summary>
        /// Gets all Types from every Assembly.
        /// </summary>
        /// <param name="includeGAC">If true, Types from Assemblies from the Global Assembly Cache will be included.
        /// Default is false.</param>
        /// <returns>All Types from every Assembly.</returns>
        public static IEnumerable<Type> AllTypes(bool includeGAC = _gacDefault)
        {
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();
            if (!includeGAC)
                assemblies = assemblies.Where(x => !x.GlobalAssemblyCache);
            var types = assemblies.SelectMany(x => x.GetTypes());
            return types;
        }

        /// <summary>
        /// Finds all Types that match the given conditions.
        /// </summary>
        /// <param name="conditions">The conditions that each Type must meet.</param>
        /// <param name="constructorParams">An array of Types that define the parameters required for
        /// the constructor. If no constructor with the specified parameter Types are found, a
        /// MissingMethodException will be thrown. Set to null to not require any constructor, and set to
        /// an empty array to require an empty constructor. Use this parameter only if you want to require
        /// a specific constructor signature, not if you just want to find Types that have a specific
        /// constructor signature. If this value is not null, only instanceable classes will be returned.</param>
        /// <param name="includeGAC">If true, Types from Assemblies from the Global Assembly Cache will be included.
        /// Default is false.</param>
        /// <returns>All Types that match the given conditions.</returns>
        /// <exception cref="MissingMethodException">A <see cref="Type"/> was found that does not contain a constructor
        /// with the required set of parameters defined by <paramref name="constructorParams"/>.</exception>
        public static IEnumerable<Type> FindTypes(Func<Type, bool> conditions, Type[] constructorParams,
                                                  bool includeGAC = _gacDefault)
        {
            // Grab all types
            var types = AllTypes(includeGAC);

            // Match against the custom conditions
            if (conditions != null)
                types = types.Where(conditions);

            // Check for the required constructor
            if (constructorParams != null)
            {
                // If we are using the required constructor, only select instanceable classes
                types = types.Where(x => x.IsClass && !x.IsAbstract);

                // Make sure all the remaining Types have the required constructor
                foreach (var type in types)
                {
                    if (!type.IsAbstract && !HasConstructorWithParameters(type, constructorParams))
                    {
                        const string errmsg = "Type `{0}` does not contain a constructor with the specified parameters.";
                        var err = string.Format(errmsg, type);
                        Debug.Fail(err);
                        if (log.IsFatalEnabled)
                            log.Fatal(err);
                        throw new MissingMethodException(err);
                    }
                }
            }

            return types;
        }

        /// <summary>
        /// Finds all Types that inherit the specified <paramref name="baseType"/>.
        /// </summary>
        /// <param name="baseType">Type of the base class or interface to find the Types that inherit.</param>
        /// <param name="constructorParams">An array of Types that define the parameters required for
        /// the constructor. If no constructor with the specified parameter Types are found, a
        /// MissingMethodException will be thrown. Set to null to not require any constructor, and set to
        /// an empty array to require an empty constructor. Use this parameter only if you want to require
        /// a specific constructor signature, not if you just want to find Types that have a specific
        /// constructor signature. If this value is not null, only instanceable classes will be returned.</param>
        /// <param name="includeGAC">If true, Types from Assemblies from the Global Assembly Cache will be included.
        /// Default is false.</param>
        /// <returns>IEnumerable of all Types that inherit the specified <paramref name="baseType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="baseType"/> is null.</exception>
        public static IEnumerable<Type> FindTypesThatInherit(Type baseType, Type[] constructorParams,
                                                             bool includeGAC = _gacDefault)
        {
            if (baseType == null)
                throw new ArgumentNullException("baseType");

            return FindTypes(x => x.IsSubclassOf(baseType), constructorParams, includeGAC);
        }

        /// <summary>
        /// Finds all Types that contain an attribute of Type <paramref name="attributeType"/>.
        /// </summary>
        /// <param name="attributeType">Type of the attribute to find on the Types.</param>
        /// <param name="constructorParams">An array of Types that define the parameters required for
        /// the constructor. If no constructor with the specified parameter Types are found, a
        /// MissingMethodException will be thrown. Set to null to not require any constructor, and set to
        /// an empty array to require an empty constructor. Use this parameter only if you want to require
        /// a specific constructor signature, not if you just want to find Types that have a specific
        /// constructor signature. If this value is not null, only instanceable classes will be returned.</param>
        /// <param name="includeGAC">If true, Types from Assemblies from the Global Assembly Cache will be included.
        /// Default is false.</param>
        /// <returns>IEnumerable of all Types that have the specified <paramref name="attributeType"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="attributeType"/> is null.</exception>
        public static IEnumerable<Type> FindTypesWithAttribute(Type attributeType, Type[] constructorParams,
                                                               bool includeGAC = _gacDefault)
        {
            if (attributeType == null)
                throw new ArgumentNullException("attributeType");

            Func<Type, bool> func = (x => !x.GetCustomAttributes(attributeType, true).IsEmpty());
            return FindTypes(func, constructorParams, includeGAC);
        }

        /// <summary>
        /// Gets all of the custom attributes for a PropertyInfo, including those attached to
        /// abstract Properties on base classes.
        /// </summary>
        /// <typeparam name="T">The Type of attribute to find.</typeparam>
        /// <param name="propInfo">PropertyInfo to get the custom attributes for.</param>
        /// <param name="flags">BindingFlags to use for finding the PropertyInfos.</param>
        /// <returns>
        /// An IEnumerable of all of the custom attributes for a PropertyInfo.
        /// </returns>
        public static IEnumerable<T> GetAllCustomAttributes<T>(PropertyInfo propInfo, BindingFlags flags) where T : Attribute
        {
            return InternalGetAllCustomAttributes<T>(propInfo, propInfo.DeclaringType, flags).Distinct();
        }

        static bool HasConstructorWithParameters(Type type, IList<Type> expected)
        {
            const BindingFlags bf = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

            foreach (var constructor in type.GetConstructors(bf))
            {
                var actual = constructor.GetParameters().Select(x => x.ParameterType).ToArray();

                if (expected.Count != actual.Length)
                    continue;

                for (var i = 0; i < expected.Count; i++)
                {
                    if (expected[i] != actual[i])
                        continue;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets all of the custom attributes for a PropertyInfo, including those attached to interfaces and abstract
        /// Properties on base classes.
        /// </summary>
        /// <typeparam name="T">The Type of attribute to find.</typeparam>
        /// <param name="propInfo">PropertyInfo to get the custom attributes for.</param>
        /// <param name="type">Type of the class to search for the PropertyInfos in.</param>
        /// <param name="flags">BindingFlags to use for finding the PropertyInfos.</param>
        /// <returns>An IEnumerable of all of the custom attributes for a PropertyInfo.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="propInfo"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
        static IEnumerable<T> InternalGetAllCustomAttributes<T>(PropertyInfo propInfo, Type type, BindingFlags flags)
            where T : Attribute
        {
            if (propInfo == null)
                throw new ArgumentNullException("propInfo");
            if (type == null)
                throw new ArgumentNullException("type");

            // Get the attributes for this type
            var customAttributes = propInfo.GetCustomAttributes(typeof(T), false).OfType<T>();
            foreach (var attrib in customAttributes)
            {
                yield return attrib;
            }

            // Get the base type
            var baseType = type.BaseType;

            // Get the property for the base type
            var baseProp = baseType.GetProperty(propInfo.Name, flags);

            // If the property for the base type exists, find the attributes for it
            if (baseProp != null)
            {
                var baseAttributes = InternalGetAllCustomAttributes<T>(baseProp, baseType, flags);
                foreach (var attrib in baseAttributes)
                {
                    yield return attrib;
                }
            }
        }

        /// <summary>
        /// Gets if the <paramref name="type"/> is public through the whole declaration tree. For example, a nested
        /// class will only return true if it is public, and so is the class it is nested in. Any non-public
        /// type in the hierarchy will result in a false value.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// True if the <paramref name="type"/> is a class and public through the whole declaration tree.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
        public static bool IsClassTypeTreePublic(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (!type.IsClass)
                return false;

            if (type.IsNested)
            {
                if (!type.IsNestedPublic)
                    return false;

                return IsClassTypeTreePublic(type.DeclaringType);
            }
            else
                return type.IsPublic;
        }

        /// <summary>
        /// Gets the nullable type of a non-nullable type.
        /// </summary>
        /// <param name="type">The non-nullable type. If already a nullable type, the same type will be returned.</param>
        /// <returns>The nullable type.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="type"/> is not a value type.</exception>
        public static Type NonNullableToNullable(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (!type.IsValueType)
            {
                const string errmsg = "Type `{0}` is not a value type.";
                throw new ArgumentException(string.Format(errmsg, type), "type");
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return type;

            return typeof(Nullable<>).MakeGenericType(type);
        }

        /// <summary>
        /// Gets the underlying non-nullable type of a nullable type.
        /// </summary>
        /// <param name="nullableType">The nullable type.</param>
        /// <returns>The underlying non-nullable type.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="nullableType"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="nullableType"/> is not a nullable type.</exception>
        public static Type NullableToNonNullable(Type nullableType)
        {
            if (nullableType == null)
                throw new ArgumentNullException("nullableType");

            if (!nullableType.IsValueType || !nullableType.IsGenericType)
            {
                const string errmsg = "Type `{0}` is either not a value type or not a generic type.";
                throw new ArgumentException(string.Format(errmsg, nullableType), "nullableType");
            }

            var ret = nullableType.GetGenericArguments().FirstOrDefault();
            if (ret == null)
            {
                const string errmsg = "Could not find generic arguments for type `{0}`.";
                throw new ArgumentException(string.Format(errmsg, nullableType), "nullableType");
            }

            return ret;
        }
    }
}