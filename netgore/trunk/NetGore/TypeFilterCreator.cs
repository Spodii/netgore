using System;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using NetGore;

namespace NetGore.Collections
{
    public class TypeFilterCreator
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets or sets an array of the type of attributes the Type must implement. If null or empty, no attributes
        /// will be required.
        /// </summary>
        public Type[] Attributes { get; set; }

        /// <summary>
        /// Gets or sets an array of the parameters needed by the constructor. If null, then no specific constructor
        /// will be searched for. If this is an empty Type array, then an empty constructor will be required.
        /// </summary>
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

            // Check the constructor
            if (ConstructorParameters != null)
            {
                const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
                if (type.GetConstructor(flags, null, ConstructorParameters, null) == null)
                {
                    if (RequireConstructor)
                    {
                        const string errmsg =
                            "Type `{0}` does not have the required constructor containing the parameters: `{1}`.";
                        string err = string.Format(errmsg, type, GetTypeString(ConstructorParameters));
                        if (log.IsFatalEnabled)
                            log.Fatal(err);
                        throw new TypeFilterException(err);
                    }
                    else
                        return false;
                }
            }

            // Check the interfaces
            if (Interfaces != null && Interfaces.Count() > 0)
            {
                var i = type.GetInterfaces();
                bool isValid = (MatchAllInterfaces ? Interfaces.All(x => i.Contains(x)) : Interfaces.Any(x => i.Contains(x)));
                if (!isValid)
                {
                    if (RequireInterfaces)
                    {
                        const string errmsg = "Type `{0}` does not have the required interfaces: `{1}`.";
                        string err = string.Format(errmsg, type, GetTypeString(Interfaces));
                        if (log.IsFatalEnabled)
                            log.Fatal(err);
                        throw new TypeFilterException(err);
                    }
                    else
                        return false;
                }
            }

            // Check the attributes
            if (Attributes != null && Attributes.Count() > 0)
            {
                var a = type.GetCustomAttributes(true).Select(x => x.GetType());
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
            }

            return true;
        }

        public Func<Type, bool> GetFilter()
        {
            return Filter;
        }

        /// <summary>
        /// Gets a string containing the name of the <paramref name="types"/>.
        /// </summary>
        /// <param name="types">The Types.</param>
        /// <returns>A string containing the name of the <paramref name="types"/>.</returns>
        static string GetTypeString(Type[] types)
        {
            if (types == null || types.Length == 0)
                return string.Empty;

            if (types.Length == 1)
                return types[0].Name;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < types.Length; i++)
            {
                sb.Append(types[i].Name);
                sb.Append(", ");
            }
            sb.Length -= 2;

            return sb.ToString();
        }
    }
}