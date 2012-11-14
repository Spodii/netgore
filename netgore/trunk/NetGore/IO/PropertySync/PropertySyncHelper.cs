using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;

namespace NetGore.IO.PropertySync
{
    /// <summary>
    /// Provides helper methods for the <see cref="IPropertySync"/>.
    /// </summary>
    public static class PropertySyncHelper
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static readonly SyncValueAttributeInfoFactory _factory = SyncValueAttributeInfoFactory.Instance;

        /// <summary>
        /// Dictionary cache for PropertySyncHandlers where the key is the Type to be handled and the value is
        /// the Type of the PropertySyncHandler itself.
        /// </summary>
        static readonly Dictionary<Type, Type> _propertySyncTypes = new Dictionary<Type, Type>();

        /// <summary>
        /// Cache that handles creating the <see cref="IPropertySync"/>s that are not actually hooked
        /// to a property with a <see cref="SyncValueAttributeInfo"/>. These can be reused since they are only used
        /// for reading/writing the type, not for an actual particular attribute.
        /// </summary>
        static readonly ThreadSafeHashCache<Type, IPropertySync> _unhookedPropertySyncCache =
            new ThreadSafeHashCache<Type, IPropertySync>(
                x => (IPropertySync)TypeFactory.GetTypeInstance(_propertySyncTypes[x], (SyncValueAttributeInfo)null));

        /// <summary>
        /// Initializes the <see cref="PropertySyncHelper"/> class.
        /// </summary>
        /// <exception cref="TypeLoadException">Multiple <see cref="PropertySyncHandlerAttribute"/>s were found on a single type.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly",
            MessageId = "PropertySyncHandlerAttributes")]
        static PropertySyncHelper()
        {
            // Create the dictionary cache that maps a PropertySyncHandler's handled type
            // to the type of the PropertySyncHandler itself

            var typeFilterCreator = new TypeFilterCreator
            { IsClass = true, IsAbstract = false, Attributes = new Type[] { typeof(PropertySyncHandlerAttribute) } };

            var typeFilter = typeFilterCreator.GetFilter();

            foreach (var type in TypeHelper.AllTypes().Where(typeFilter))
            {
                // Look for classes that inherit the PropertySyncHandlerAttribute
                var attribs = type.GetCustomAttributes(typeof(PropertySyncHandlerAttribute), true);
                if (attribs.Length < 1)
                    continue;

                if (attribs.Length > 1)
                {
                    const string errmsg = "Multiple PropertySyncHandlerAttributes found on type `{0}`";
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, type);
                    Debug.Fail(string.Format(errmsg, type));
                    throw new TypeLoadException(string.Format(errmsg, type));
                }

                // Grab the attribute so we can find out what type this class handles
                var attrib = (PropertySyncHandlerAttribute)attribs[0];

                // Make sure the handler doesn't already exist
                if (_propertySyncTypes.ContainsKey(attrib.HandledType))
                {
                    const string errmsg =
                        "Duplicate PropertySync implementations for type `{0}`. Implementations: `{1}` and `{2}`.";
                    var existingPST = _propertySyncTypes[attrib.HandledType];
                    if (log.IsErrorEnabled)
                        log.ErrorFormat(errmsg, attrib.HandledType, existingPST, type);
                    Debug.Fail(string.Format(errmsg, attrib.HandledType, existingPST, type));
                    continue;
                }

                // Store the handled type
                _propertySyncTypes.Add(attrib.HandledType, type);

                // If the type can be made nullable, also store the nullable type
                if (attrib.HandledType.IsValueType && !attrib.HandledType.IsGenericType)
                {
                    try
                    {
                        var nullableType = typeof(Nullable<>).MakeGenericType(attrib.HandledType);

                        // Make sure the key doesn't already exist
                        if (!_propertySyncTypes.ContainsKey(nullableType))
                        {
                            var psType = typeof(PropertySyncNullable<>).MakeGenericType(attrib.HandledType);
                            _propertySyncTypes.Add(nullableType, psType);
                        }
                    }
                    catch (Exception ex)
                    {
                        const string errmsg = "Failed to create nullable type from `{0}`. Reason: {1}";
                        if (log.IsErrorEnabled)
                            log.ErrorFormat(errmsg, attrib.HandledType, ex);
                        Debug.Fail(string.Format(errmsg, attrib.HandledType, ex));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="IPropertySync"/> for a given <see cref="SyncValueAttributeInfo"/>.
        /// </summary>
        /// <param name="attribInfo">The <see cref="SyncValueAttributeInfo"/>.</param>
        /// <returns>The <see cref="IPropertySync"/> for a given <see cref="SyncValueAttributeInfo"/>.</returns>
        /// <exception cref="TypeLoadException">Could not create an <see cref="IPropertySync"/> from the 
        /// <paramref name="attribInfo"/>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="attribInfo"/> is for a type that does not have a 
        /// <see cref="PropertySyncHandlerAttribute"/> defined for it.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="attribInfo"/> is null.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "IPropertySync")]
        internal static IPropertySync GetPropertySync(SyncValueAttributeInfo attribInfo)
        {
            if (attribInfo == null)
                throw new ArgumentNullException("attribInfo");

            // Get the Type of the class used to handle the type of property
            Type propertySyncType;
            try
            {
                propertySyncType = _propertySyncTypes[attribInfo.PropertyType];
            }
            catch (KeyNotFoundException ex)
            {
                const string errmsg =
                    "No PropertySyncHandler exists for type `{0}`. You must define a class to handle this type (preferably derived from PropertySyncBase) and give it the PropertySyncHandlerAttribute.";
                throw new ArgumentException(string.Format(errmsg, attribInfo.PropertyType), ex);
            }

            // Get the instance of the class used to handle the attribute's type
            var instance = (IPropertySync)TypeFactory.GetTypeInstance(propertySyncType, attribInfo);

            if (instance == null)
            {
                const string errmsg = "Failed to create IPropertySync of type `{0}` for `{1}`.";
                var err = string.Format(errmsg, _propertySyncTypes[attribInfo.PropertyType], attribInfo);
                log.Fatal(err);
                throw new TypeLoadException(err);
            }

            return instance;
        }

        /// <summary>
        /// Gets the <see cref="IPropertySync"/> instances for the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to get the <see cref="IPropertySync"/>s for.</param>
        /// <returns>The <see cref="IPropertySync"/> instances for the given <paramref name="type"/>.</returns>
        public static IEnumerable<IPropertySync> GetPropertySyncs(Type type)
        {
            var infos = _factory[type];
            var ret = new IPropertySync[infos.Length];

            // Loop through each of the property sync infos
            for (var i = 0; i < ret.Length; i++)
            {
                var instance = GetPropertySync(infos[i]);
                ret[i] = instance;
            }

            return ret;
        }

        /// <summary>
        /// Gets the <see cref="IPropertySync"/> for a given <paramref name="type"/> that does not pass a
        /// valid <see cref="SyncValueAttributeInfo"/>. Attempting to perform state tracking operations on the created
        /// object will result in an exception being thrown. This method is intended only as a way to provide access
        /// to the code for reading and writing the values for the <see cref="PropertySyncNullable{T}"/>.
        /// </summary>
        /// <param name="type">The type for the <see cref="IPropertySync"/> to handle.</param>
        /// <returns>The <see cref="IPropertySync"/> for a given <paramref name="type"/> that does not pass a
        /// valid <see cref="SyncValueAttributeInfo"/>.</returns>
        internal static IPropertySync GetUnhookedPropertySync(Type type)
        {
            return _unhookedPropertySyncCache[type];
        }

        /// <summary>
        /// Compares two PropertyInfoDatas by using the PropertyInfo's Name.
        /// </summary>
        /// <param name="a">First PropertyInfoData.</param>
        /// <param name="b">Second PropertyInfoData.</param>
        /// <returns>Comparison of the two PropertyInfoDatas.</returns>
        static int PropertyInfoAndAttributeComparer(SyncValueAttributeInfo a, SyncValueAttributeInfo b)
        {
            return String.Compare(a.Name, b.Name, StringComparison.Ordinal);
        }

        /// <summary>
        /// Contains the <see cref="SyncValueAttributeInfo"/>s for a given <see cref="Type"/>.
        /// </summary>
        class SyncValueAttributeInfoFactory : ThreadSafeHashCache<Type, SyncValueAttributeInfo[]>
        {
            static readonly SyncValueAttributeInfoFactory _instance;

            /// <summary>
            /// Initializes the <see cref="SyncValueAttributeInfoFactory"/> class.
            /// </summary>
            static SyncValueAttributeInfoFactory()
            {
                _instance = new SyncValueAttributeInfoFactory();
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SyncValueAttributeInfoFactory"/> class.
            /// </summary>
            SyncValueAttributeInfoFactory() : base(CreateValue)
            {
            }

            /// <summary>
            /// Gets the <see cref="SyncValueAttributeInfoFactory"/> instance.
            /// </summary>
            public static SyncValueAttributeInfoFactory Instance
            {
                get { return _instance; }
            }

            /// <summary>
            /// Creates the <see cref="SyncValueAttributeInfo"/>s for the given <paramref name="key"/>.
            /// </summary>
            /// <param name="key">The key to create the value for.</param>
            /// <exception cref="TypeLoadException">Multiple <see cref="SyncValueAttribute"/> found on a single type.</exception>
            [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "SyncValueAttribute")]
            static SyncValueAttributeInfo[] CreateValue(Type key)
            {
                const BindingFlags flags =
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty |
                    BindingFlags.SetProperty;

                var tempPropInfos = new List<SyncValueAttributeInfo>();

                // Find all of the properties for this type
                foreach (var property in key.GetProperties(flags))
                {
                    // Get the SyncValueAttribute for the property, skipping if none found
                    var attribs = TypeHelper.GetAllCustomAttributes<SyncValueAttribute>(property, flags).ToArray();
                    if (attribs.IsEmpty())
                        continue;

                    if (attribs.Length > 1)
                    {
                        const string errmsg = "Property `{0}` contains more than one SyncValueAttribute!";
                        var err = string.Format(errmsg, property);
                        log.FatalFormat(err);
                        Debug.Fail(err);
                        throw new TypeLoadException(err);
                    }

                    // Get the attribute attached to this Property
                    var attribute = attribs.First();

                    // Create the SyncValueAttributeInfo
                    var targetSVAIType = typeof(SyncValueAttributeInfo<>).MakeGenericType(property.PropertyType);
                    var svai = (SyncValueAttributeInfo)TypeFactory.GetTypeInstance(targetSVAIType, property, attribute);

                    // Ensure we don't already have a property with this name
                    if (tempPropInfos.Any(x => x.Name == svai.Name))
                    {
                        const string errmsg = "Class `{0}` contains more than one SyncValueAttribute with the name `{1}`!";
                        var err = string.Format(errmsg, key, svai.Name);
                        log.FatalFormat(err);
                        Debug.Fail(err);
                        throw new TypeLoadException(err);
                    }

                    // Add the property the list
                    tempPropInfos.Add(svai);
                }

                // Sort the list so we can ensure that, every time this runs, the order will always be the same
                tempPropInfos.Sort(PropertyInfoAndAttributeComparer);

                // Convert to an array and return
                return tempPropInfos.ToArray();
            }
        }
    }
}