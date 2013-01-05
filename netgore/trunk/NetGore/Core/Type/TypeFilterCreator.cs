using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;

namespace NetGore
{
    public class TypeFilterCreator
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets or sets an array of the type of attributes the Type must implement. If null or empty, no attributes
        /// will be required.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public Type[] Attributes { get; set; }

        /// <summary>
        /// Gets or sets an array of the parameters needed by the constructor. If null, then no specific constructor
        /// will be searched for. If this is an empty Type array, then an empty constructor will be required.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public Type[] ConstructorParameters { get; set; }

        /// <summary>
        /// Gets or sets a custom filter used to match a Type. This filter is used before any requirement checks are
        /// performed. As a result, if this customer filter returns false for any Type, no exceptions will be thrown
        /// due to the Type missing a requirement. If null, no custom filter will be used.
        /// </summary>
        public Func<Type, bool> CustomFilter { get; set; }

        /// <summary>
        /// Gets or sets an array of the Type of interfaces that the Type must implement. If null or empty, no interfaces
        /// will be required.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public Type[] Interfaces { get; set; }

        /// <summary>
        /// Gets or sets if the Type must be an abstract class. If null, this value will not be used.
        /// </summary>
        public bool? IsAbstract { get; set; }

        /// <summary>
        /// Gets or sets if the Type must be a class. If null, this value will not be used.
        /// </summary>
        public bool? IsClass { get; set; }

        /// <summary>
        /// Gets or sets if the Type must be an enum. If null, this value will not be used.
        /// </summary>
        public bool? IsEnum { get; set; }

        /// <summary>
        /// Gets or sets if the Type must be an interface. If null, this value will not be used.
        /// </summary>
        public bool? IsInterface { get; set; }

        /// <summary>
        /// Gets or sets if all of the <see cref="Attributes"/> are required for the Type to be a match. If false, the
        /// Type will be a valid match if at least one of the attribute Types are implemented. Only valid if
        /// <see cref="Attributes"/> is not null or empty.
        /// </summary>
        public bool MatchAllAttributes { get; set; }

        /// <summary>
        /// Gets or sets if all of the <see cref="Interfaces"/> are required for the Type to be a match. If false, the
        /// Type will be a valid match if at least one of the interfaces Types are implemented. Only valid if
        /// <see cref="Interfaces"/> is not null or empty.
        /// </summary>
        public bool MatchAllInterfaces { get; set; }

        /// <summary>
        /// Gets or sets if the attributes on the Type must match the required <see cref="Attributes"/>. If true,
        /// any Type that matches the other conditions but fails to implement these attributes will result in a
        /// <see cref="TypeFilterException"/> being thrown.
        /// </summary>
        public bool RequireAttributes { get; set; }

        /// <summary>
        /// Gets or sets if a constructor that matches the <see cref="ConstructorParameters"/> is required. If true,
        /// any Type that matches the other conditions but fails to implement this constructor will result in a
        /// <see cref="TypeFilterException"/> being thrown.
        /// </summary>
        public bool RequireConstructor { get; set; }

        /// <summary>
        /// Gets or sets if the interfaces on the Type must match the required <see cref="Interfaces"/>. If true,
        /// any Type that matches the other conditions but fails to implement these interfaces will result in an
        /// <see cref="TypeFilterException"/> being thrown.
        /// </summary>
        public bool RequireInterfaces { get; set; }

        /// <summary>
        /// Gets or sets the class that the Type must inherit from. The inheritance can take place anywhere on the
        /// inheritance hierarchy - that is, it doesn't have to immediately inherit this Type. If null, no subclass
        /// will be required.
        /// </summary>
        public Type Subclass { get; set; }

        /// <summary>
        /// Runs the filtering process on a single <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to filter.</param>
        /// <returns>True if the <paramref name="type"/> is to be used; otherwise false.</returns>
        /// <exception cref="TypeFilterException">The <paramref name="type"/> does not contain expected meta-data (e.g. attribute,
        /// signature, return type, etc).</exception>
        bool Filter(Type type)
        {
            // Check the type of Type
            if (IsClass != null && type.IsClass != IsClass)
                return false;

            if (IsAbstract != null && type.IsAbstract != IsAbstract)
                return false;

            if (IsInterface != null && type.IsInterface != IsInterface)
                return false;

            if (IsEnum != null && type.IsEnum != IsEnum)
                return false;

            // Check the subclass
            if (Subclass != null && !type.IsSubclassOf(Subclass))
                return false;

            // Check the custom filter
            if (CustomFilter != null && !CustomFilter(type))
                return false;

            // Check the other stuff
            // We have to make sure we don't just abort on the first failure since we will still need to throw an
            // exception if needed
            // Basically, the process is this:
            // 1. Do steps 2-4, first for non-required filters, then for required filters
            // 2. Check the filter
            // 3. Once "failed" has reached true, don't let it go back to false
            // 4. If we have failed, only check the filter if there is a chance it can throw an exception
            var failed = false;

            // Non-required filters (that way we avoid throwing an exception for as long as possible)
            failed |= !FilterConstructorParameters(type, false);
            failed |= !FilterInterfaces(type, false);
            failed |= !FilterAttributes(type, false);

            if (failed)
                return false;

            // Required filters
            failed |= !FilterConstructorParameters(type, true);
            failed |= !FilterInterfaces(type, true);
            failed |= !FilterAttributes(type, true);

            return !failed;
        }

        /// <summary>
        /// Filters <see cref="Type"/>s based on their attributes.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to filter.</param>
        /// <param name="mustBeRequired">This check will only be performed with this argument matches the value of the
        /// <see cref="RequireAttributes"/> property.</param>
        /// <returns>True if the <paramref name="type"/> should be included; false if it was filtered out.</returns>
        /// <exception cref="TypeFilterException"><paramref name="mustBeRequired"/> is true, <see cref="RequireAttributes"/>
        /// is true, and the <paramref name="type"/> does not contain the required <see cref="Attributes"/>.</exception>
        bool FilterAttributes(ICustomAttributeProvider type, bool mustBeRequired)
        {
            if (mustBeRequired != RequireAttributes)
                return true;

            if (Attributes == null || Attributes.IsEmpty())
                return true;

            IEnumerable<Type> a = type.GetCustomAttributes(true).Select(x => x.GetType());
            bool isValid = (MatchAllAttributes ? Attributes.All(x => a.Contains(x)) : Attributes.Any(x => a.Contains(x)));
            if (!isValid)
            {
                if (RequireAttributes)
                {
                    const string errmsg = "Type `{0}` does not have the required attributes: `{1}`.";
                    string err = string.Format(errmsg, type, GetTypeString(Attributes));
                    if (log.IsFatalEnabled)
                        log.Fatal(err);
                    throw new TypeFilterException(err);
                }
                else
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Filters <see cref="Type"/>s based on their constructors.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to filter.</param>
        /// <param name="mustBeRequired">This check will only be performed with this argument matches the value of the
        /// <see cref="RequireConstructor"/> property.</param>
        /// <returns>True if the <paramref name="type"/> should be included; false if it was filtered out.</returns>
        /// <exception cref="TypeFilterException"><paramref name="mustBeRequired"/> is true, <see cref="RequireConstructor"/>
        /// is true, and the <paramref name="type"/> does not contain the required <see cref="ConstructorParameters"/>.</exception>
        bool FilterConstructorParameters(Type type, bool mustBeRequired)
        {
            if (mustBeRequired != RequireConstructor)
                return true;

            if (ConstructorParameters == null)
                return true;

            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            if (type.GetConstructor(flags, null, ConstructorParameters, null) == null)
            {
                if (RequireConstructor)
                {
                    const string errmsg = "Type `{0}` does not have the required constructor containing the parameters: `{1}`.";
                    var err = string.Format(errmsg, type, GetTypeString(ConstructorParameters));
                    if (log.IsFatalEnabled)
                        log.Fatal(err);
                    throw new TypeFilterException(err);
                }
                else
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Filters <see cref="Type"/>s based on the interfaces they implement.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to filter.</param>
        /// <param name="mustBeRequired">This check will only be performed with this argument matches the value of the
        /// <see cref="RequireInterfaces"/> property.</param>
        /// <returns>True if the <paramref name="type"/> should be included; false if it was filtered out.</returns>
        /// <exception cref="TypeFilterException"><paramref name="mustBeRequired"/> is true, <see cref="RequireInterfaces"/>
        /// is true, and the <paramref name="type"/> does not implement the required <see cref="Interfaces"/>.</exception>
        bool FilterInterfaces(Type type, bool mustBeRequired)
        {
            if (mustBeRequired != RequireInterfaces)
                return true;

            if (Interfaces == null || Interfaces.IsEmpty())
                return true;

            Type[] i = type.GetInterfaces();
            bool isValid = (MatchAllInterfaces ? Interfaces.All(x => i.Contains(x)) : Interfaces.Any(x => i.Contains(x)));
            if (!isValid)
            {
                if (RequireInterfaces)
                {
                    const string errmsg = "Type `{0}` does not have the required interfaces: `{1}`.";
                    var err = string.Format(errmsg, type, GetTypeString(Interfaces));
                    if (log.IsFatalEnabled)
                        log.Fatal(err);
                    throw new TypeFilterException(err);
                }
                else
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the func for filtering the types.
        /// </summary>
        /// <returns>The func for filtering the types.</returns>
        public Func<Type, bool> GetFilter()
        {
            return Filter;
        }

        /// <summary>
        /// Gets a string containing the name of the <paramref name="types"/>.
        /// </summary>
        /// <param name="types">The Types.</param>
        /// <returns>A string containing the name of the <paramref name="types"/>.</returns>
        static string GetTypeString(IList<Type> types)
        {
            if (types == null || types.Count == 0)
                return string.Empty;

            if (types.Count == 1)
                return types[0].Name;

            var sb = new StringBuilder();
            for (var i = 0; i < types.Count; i++)
            {
                sb.Append(types[i].Name);
                sb.Append(", ");
            }
            sb.Length -= 2;

            return sb.ToString();
        }
    }
}