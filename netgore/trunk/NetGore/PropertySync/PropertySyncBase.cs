using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// Helper class for creating a PropertySyncBase. This class handles almost all of the implementation
    /// aspects of a PropertySyncBase. It is highly recommended you never override any of the methods you
    /// do not have to, and that each derived class is sealed.
    /// </summary>
    public abstract class PropertySyncBase<T> : PropertySyncBase
    {
        GetHandler _getter;
        T _lastSentValue;
        SetHandler _setter;

        /// <summary>
        /// Gets or sets the Property's value. This is not cached in any way - the value comes from or goes to
        /// the Property directly. Local caching should be used where possible.
        /// </summary>
        protected T Value
        {
            get { return _getter(); }
            set { _setter(value); }
        }

        /// <summary>
        /// PropertySyncBase constructor.
        /// </summary>
        /// <param name="bindObject">Object that this property is to be bound to.</param>
        /// <param name="p">PropertyInfo for the property to bind to.</param>
        protected PropertySyncBase(object bindObject, PropertyInfo p)
            : base(bindObject, p)
        {
        }

        /// <summary>
        /// When overridden in the derived class, contains the Delegate for the property's getter and setter so they
        /// can be stored by the derived class, casted to their appropriate type.
        /// </summary>
        /// <param name="getter">Getter delegate to be stored. Guarenteed to be able to cast to the
        /// same Type that was returned by GetGetDelegateType().</param>
        /// <param name="setter">Setter delegate to be stored. Guarenteed to be able to cast to the
        /// same Type that was returned by GetSetDelegateType().</param>
        protected override void StoreDelegates(Delegate getter, Delegate setter)
        {
            _getter = (GetHandler)getter;
            _setter = (SetHandler)setter;
        }

        /// <summary>
        /// When overridden in the derived class, gets the Type of the Delegate for the property's accessor.
        /// </summary>
        /// <returns>The Type of the Delegate for the property's accessor.</returns>
        protected override Type GetGetDelegateType()
        {
            return typeof(GetHandler);
        }

        /// <summary>
        /// When overridden in the derived class, gets the Type of the Delegate for the property's mutator.
        /// </summary>
        /// <returns>The Type of the Delegate for the property's mutator.</returns>
        protected override Type GetSetDelegateType()
        {
            return typeof(SetHandler);
        }

        /// <summary>
        /// When overridden in the derived class, gets if the Property's value has changed and needs to be re-synchronized.
        /// </summary>
        /// <returns>True if the Property needs to be re-synchronized, else False.</returns>
        public override bool HasValueChanged()
        {
            return !_lastSentValue.Equals(Value);
        }

        /// <summary>
        /// When overridden in the derived class, reads a value with the specified name from an IValueReader.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="reader">IValueReader to read from.</param>
        /// <returns>Value read from the IValueReader.</returns>
        protected abstract T Read(string name, IValueReader reader);

        /// <summary>
        /// When overridden in the derived class, reads the Property's value from an IValueReader and updates the
        /// Property's value with the value read.
        /// </summary>
        /// <param name="reader">IValueReader to read the Property's new value from.</param>
        public override void ReadValue(IValueReader reader)
        {
            Value = Read(Name, reader);
        }

        /// <summary>
        /// When overridden in the derived class, writes a value to an IValueWriter with the specified name.
        /// </summary>
        /// <param name="name">Name of the value.</param>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="value">Value to write.</param>
        protected abstract void Write(string name, IValueWriter writer, T value);

        /// <summary>
        /// When overridden in the derived class, writes the Property's value to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write the Property's value to.</param>
        public override void WriteValue(IValueWriter writer)
        {
            T value = Value;
            Write(Name, writer, value);
            _lastSentValue = value;
        }

        /// <summary>
        /// Delegate for the Property's accessor.
        /// </summary>
        /// <returns>The value of the Property.</returns>
        delegate T GetHandler();

        /// <summary>
        /// Delegate for the Property's mutator.
        /// </summary>
        /// <param name="value">Value to assign to the Property.</param>
        delegate void SetHandler(T value);
    }

    /// <summary>
    /// Base class for an object that can serialize and deserialize the property of an object.
    /// </summary>
    public abstract class PropertySyncBase : IComparable<PropertySyncBase>
    {
        /// <summary>
        /// Dictionary cache for PropertySyncHandlers where the key is the Type to be handled and the value is
        /// the Type of the PropertySyncHandler itself.
        /// </summary>
        static readonly Dictionary<Type, Type> _propertySyncTypes = new Dictionary<Type, Type>();

        /// <summary>
        /// Dictionary cache for PropertyInfoDatas where the key is the Type of the object to find the PropertyInfo for
        /// and the value is a sorted list of the PropertyInfoDatas for that Type.
        /// </summary>
        static readonly Dictionary<Type, PropertyInfoData[]> _syncValueProperties = new Dictionary<Type, PropertyInfoData[]>();

        string _name;

        /// <summary>
        /// Gets the name of the synchronized value. This is what populates the Name parameter of the IValueReader
        /// and IValueWriter functions.
        /// </summary>
        public string Name { get { return _name; } private set { _name = value; } }

        /// <summary>
        /// PropertySyncBase static constructor.
        /// </summary>
        static PropertySyncBase()
        {
            // Create the dictionary cache that maps a PropertySyncHandler's handled type
            // to the type of the PropertySyncHandler itself

            // Get all of the class Types
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => x.IsClass);

            foreach (Type type in types)
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
        /// PropertySyncBase constructor.
        /// </summary>
        /// <param name="bindObject">Object that this property is to be bound to.</param>
        /// <param name="p">PropertyInfo for the property to bind to.</param>
        protected PropertySyncBase(object bindObject, PropertyInfo p)
        {
            _name = p.Name;

            if (!p.CanWrite)
                throw new ArgumentException("Properties with the SyncValue attribute must contain a setter.");
            if (!p.CanRead)
                throw new ArgumentException("Properties with the SyncValue attribute must contain a getter.");

            MethodInfo getMethod = p.GetGetMethod(true);
            MethodInfo setMethod = p.GetSetMethod(true);

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            Delegate getter = Delegate.CreateDelegate(GetGetDelegateType(), bindObject, getMethod, true);
            Delegate setter = Delegate.CreateDelegate(GetSetDelegateType(), bindObject, setMethod, true);
         
            StoreDelegates(getter, setter);
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        /// <summary>
        /// When overridden in the derived class, contains the Delegate for the property's getter and setter so they
        /// can be stored by the derived class, casted to their appropriate type.
        /// </summary>
        /// <param name="getter">Getter delegate to be stored. Guarenteed to be able to cast to the
        /// same Type that was returned by GetGetDelegateType().</param>
        /// <param name="setter">Setter delegate to be stored. Guarenteed to be able to cast to the
        /// same Type that was returned by GetSetDelegateType().</param>
        protected abstract void StoreDelegates(Delegate getter, Delegate setter);

        /// <summary>
        /// When overridden in the derived class, gets the Type of the Delegate for the property's accessor.
        /// </summary>
        /// <returns>The Type of the Delegate for the property's accessor.</returns>
        protected abstract Type GetGetDelegateType();

        /// <summary>
        /// Gets the PropertySyncBase for the specified PropertyInfo and bound to the specified Object.
        /// </summary>
        /// <param name="propertyInfo">PropertyInfo to synchronize to.</param>
        /// <param name="bindObject">Object to bind to.</param>
        /// <returns>The PropertySyncBase for the specified PropertyInfo and bound to the specified Object.</returns>
        static PropertySyncBase GetPropertySync(PropertyInfo propertyInfo, object bindObject)
        {
            if (bindObject == null)
                throw new ArgumentNullException("bindObject");
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            if (!propertyInfo.CanRead || !propertyInfo.CanWrite)
                throw new ArgumentException("Only properties with both a getter and setter can be synchronized.", "propertyInfo");

            // Get the property's value Type
            Type handledType = propertyInfo.PropertyType;

            // Get the type of the PropertySyncBase that will be used to synchronize the Property's value
            Type syncType;
            try
            {
                syncType = _propertySyncTypes[handledType];
            }
            catch (KeyNotFoundException)
            {
                const string errmsg = "Failed to find PropertySync for type `{0}`.";
                Debug.Fail(string.Format(errmsg, handledType));
                throw new Exception(string.Format(errmsg, handledType));
            }
            
            // Return the instance of the PropertySyncBase
            return (PropertySyncBase)Activator.CreateInstance(syncType, bindObject, propertyInfo);
        }

        /// <summary>
        /// When overridden in the derived class, gets the Type of the Delegate for the property's mutator.
        /// </summary>
        /// <returns>The Type of the Delegate for the property's mutator.</returns>
        protected abstract Type GetSetDelegateType();

        /// <summary>
        /// Struct containing the cached PropertyInfo data.
        /// </summary>
        struct PropertyInfoData
        {
            /// <summary>
            /// The PropertyInfo.
            /// </summary>
            public readonly PropertyInfo PropertyInfo;

            /// <summary>
            /// The name to give the PropertySyncBase.
            /// </summary>
            public readonly string Name;

            /// <summary>
            /// PropertyInfoData constructor.
            /// </summary>
            /// <param name="propertyInfo">The PropertyInfo.</param>
            /// <param name="name">The name to give the PropertySyncBase.</param>
            public PropertyInfoData(PropertyInfo propertyInfo, string name)
            {
                PropertyInfo = propertyInfo;
                Name = name;
            }
        }

        /// <summary>
        /// Creates an array of PropertyInfoDatas for the given Type.
        /// </summary>
        /// <param name="type">Type to get the PropertyInfoDatas for.</param>
        /// <returns>An array of PropertyInfoDatas for the given Type.</returns>
        static PropertyInfoData[] CreatePropertyInfoDatas(Type type)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                       BindingFlags.GetProperty | BindingFlags.SetProperty;

            var tempPropInfos = new List<PropertyInfoData>();

            // Find all of the properties for this type
            foreach (var property in type.GetProperties(flags))
            {
                // Get the SyncValueAttribute for the property, skipping if none found
                var attribs = property.GetCustomAttributes(typeof(SyncValueAttribute), true);
                if (attribs.Length == 0)
                    continue;
                if (attribs.Length > 1)
                {
                    const string errmsg = "Property `{0}` contains more than one SyncValueAttribute!";
                    string err = string.Format(errmsg, property);
                    log.FatalFormat(err);
                    Debug.Fail(err);
                    throw new Exception(err);
                }

                // Find the name to use (CustomName if one supplied, otherwise use the Property's name)
                var attribute = (SyncValueAttribute)attribs[0];
                string name;
                if (string.IsNullOrEmpty(attribute.CustomName))
                    name = property.Name;
                else
                    name = attribute.CustomName;

                // Ensure we don't already have a property with this name
                if (tempPropInfos.Count(x => x.Name == name) > 0)
                {
                    const string errmsg = "Class `{0}` contains more than one SyncValueAttribute with the name `{1}`!";
                    string err = string.Format(errmsg, type, name);
                    log.FatalFormat(err);
                    Debug.Fail(err);
                    throw new Exception(err);
                }

                // Add the property the list
                tempPropInfos.Add(new PropertyInfoData(property, name));
            }

            // Sort the list so we can ensure that, every time this runs, the order will always be the same
            tempPropInfos.Sort(PropertyInfoAndAttributeComparer);

            // Convert to an array and return
            return tempPropInfos.ToArray();
        }

        /// <summary>
        /// Gets an IEnumerable of PropertySyncBases for all of the SyncValueAttributes found for the Object.
        /// </summary>
        /// <param name="obj">Object instance to find the PropertySyncBases for.</param>
        /// <returns>An IEnumerable of PropertySyncBases for all of the SyncValueAttributes found for the Object.</returns>
        public static IEnumerable<PropertySyncBase> GetPropertySyncs(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            // Get the object's type
            Type type = obj.GetType();

            // Get the PropertyInfos for this type
            PropertyInfoData[] propInfos;

            // Try to grab the list from our dictionary cache, or create the list if not found in the cache
            if (!_syncValueProperties.TryGetValue(type, out propInfos))
            {
                propInfos = CreatePropertyInfoDatas(type);
                _syncValueProperties.Add(type, propInfos);
            }

            // Loop through each of the PropertyInfoDatas and create the PropertySyncBase for it
            foreach (var p in propInfos)
            {
                var propertySync = GetPropertySync(p.PropertyInfo, obj);
                propertySync.Name = p.Name;
                yield return propertySync;
            }
        }

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Compares two PropertyInfoDatas by using the PropertyInfo's Name.
        /// </summary>
        /// <param name="a">First PropertyInfoData.</param>
        /// <param name="b">Second PropertyInfoData.</param>
        /// <returns>Comparison of the two PropertyInfoDatas.</returns>
        static int PropertyInfoAndAttributeComparer(PropertyInfoData a, PropertyInfoData b)
        {
            return a.PropertyInfo.Name.CompareTo(b.PropertyInfo.Name);
        }

        /// <summary>
        /// When overridden in the derived class, gets if the Property's value has changed and needs to be re-synchronized.
        /// </summary>
        /// <returns>True if the Property needs to be re-synchronized, else False.</returns>
        public abstract bool HasValueChanged();

        /// <summary>
        /// When overridden in the derived class, reads the Property's value from an IValueReader and updates the
        /// Property's value with the value read.
        /// </summary>
        /// <param name="reader">IValueReader to read the Property's new value from.</param>
        public abstract void ReadValue(IValueReader reader);

        /// <summary>
        /// When overridden in the derived class, writes the Property's value to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write the Property's value to.</param>
        public abstract void WriteValue(IValueWriter writer);

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: 
        ///                     Value 
        ///                     Meaning 
        ///                     Less than zero 
        ///                     This object is less than the <paramref name="other"/> parameter.
        ///                     Zero 
        ///                     This object is equal to <paramref name="other"/>. 
        ///                     Greater than zero 
        ///                     This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(PropertySyncBase other)
        {
            return Name.CompareTo(other.Name);
        }
    }
}