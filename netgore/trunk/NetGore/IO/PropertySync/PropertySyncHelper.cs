using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// Initializes the <see cref="PropertySyncHelper"/> class.
        /// </summary>
        static PropertySyncHelper()
        {
            // Create the dictionary cache that maps a PropertySyncHandler's handled type
            // to the type of the PropertySyncHandler itself

            var typeFilterCreator = new TypeFilterCreator
            { IsClass = true, IsAbstract = false, Attributes = new Type[] { typeof(PropertySyncHandlerAttribute) } };

            var typeFilter = typeFilterCreator.GetFilter();

            foreach (Type type in TypeHelper.AllTypes(false).Where(typeFilter))
            {
                // Look for classes that inherit the PropertySyncHandlerAttribute
                var attribs = type.GetCustomAttributes(typeof(PropertySyncHandlerAttribute), true);
                if (attribs.Length < 1)
                    continue;

                if (attribs.Length > 1)
                {
                    const string errmsg = "Multiple PropertySyncHandlerAttributes found on type `{0}`";
                    Debug.Fail(string.Format(errmsg, type));
                    throw new Exception(string.Format(errmsg, type));
                }

                // Grab the attribute so we can find out what type this class handles
                PropertySyncHandlerAttribute attrib = (PropertySyncHandlerAttribute)attribs[0];

                // Store the handled type
                _propertySyncTypes.Add(attrib.HandledType, type);
            }
        }

        /// <summary>
        /// Gets the <see cref="IPropertySync"/> instances for the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to get the <see cref="IPropertySync"/>s for.</param>
        /// <returns>The <see cref="IPropertySync"/> instances for the given <paramref name="type"/>.</returns>
        public static IPropertySync[] GetPropertySyncs(Type type)
        {
            var infos = _factory[type];
            IPropertySync[] ret = new IPropertySync[infos.Length];

            // Loop through each of the property sync infos
            for (int i = 0; i < ret.Length; i++)
            {
                var propertySyncType = _propertySyncTypes[infos[i].PropertyType];
                var instance = (IPropertySync)TypeFactory.GetTypeInstance(propertySyncType, infos[i]);
                if (instance == null)
                {
                    const string errmsg = "Failed to create IPropertySync of type `{0}` for `{1}`.";
                    string err = string.Format(errmsg, propertySyncType, infos[i]);
                    log.Fatal(err);
                    throw new TypeLoadException(err);
                }

                ret[i] = instance;
            }

            return ret;
        }

        /// <summary>
        /// Contains the <see cref="SyncValueAttributeInfo"/>s for a given <see cref="Type"/>.
        /// </summary>
        class SyncValueAttributeInfoFactory : ThreadSafeHashFactory<Type, SyncValueAttributeInfo[]>
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
            static SyncValueAttributeInfo[] CreateValue(Type key)
            {
                const BindingFlags flags =
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty |
                    BindingFlags.SetProperty;

                var tempPropInfos = new List<SyncValueAttributeInfo>();

                // Find all of the properties for this type
                foreach (PropertyInfo property in key.GetProperties(flags))
                {
                    // Get the SyncValueAttribute for the property, skipping if none found
                    var attribs = TypeHelper.GetAllCustomAttributes<SyncValueAttribute>(property, flags).ToArray();
                    if (attribs.Count() == 0)
                        continue;

                    if (attribs.Length > 1)
                    {
                        const string errmsg = "Property `{0}` contains more than one SyncValueAttribute!";
                        string err = string.Format(errmsg, property);
                        log.FatalFormat(err);
                        Debug.Fail(err);
                        throw new Exception(err);
                    }

                    // Get the attribute attached to this Property
                    SyncValueAttribute attribute = attribs.First();

                    // Create the SyncValueAttributeInfo
                    var targetSVAIType = typeof(SyncValueAttributeInfo<>).MakeGenericType(property.PropertyType);
                    var svai = (SyncValueAttributeInfo)TypeFactory.GetTypeInstance(targetSVAIType, property, attribute);

                    // Ensure we don't already have a property with this name
                    if (tempPropInfos.Any(x => x.Name == svai.Name))
                    {
                        const string errmsg = "Class `{0}` contains more than one SyncValueAttribute with the name `{1}`!";
                        string err = string.Format(errmsg, key, svai.Name);
                        log.FatalFormat(err);
                        Debug.Fail(err);
                        throw new Exception(err);
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

        /// <summary>
        /// Compares two PropertyInfoDatas by using the PropertyInfo's Name.
        /// </summary>
        /// <param name="a">First PropertyInfoData.</param>
        /// <param name="b">Second PropertyInfoData.</param>
        /// <returns>Comparison of the two PropertyInfoDatas.</returns>
        static int PropertyInfoAndAttributeComparer(SyncValueAttributeInfo a, SyncValueAttributeInfo b)
        {
            return a.Name.CompareTo(b.Name);
        }
    }
}