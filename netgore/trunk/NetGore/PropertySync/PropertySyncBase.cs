using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NetGore.IO;

namespace NetGore
{
    public abstract class PropertySyncBase<T> : PropertySyncBase
    {
        GetHandler _getter;
        T _lastSentValue;
        SetHandler _setter;

        protected T Value
        {
            get { return _getter(); }
            set { _setter(value); }
        }

        protected PropertySyncBase(object bindObject, PropertyInfo p)
            : base(bindObject, p)
        {
        }

        protected override void GetDelegates(Delegate getter, Delegate setter)
        {
            _getter = (GetHandler)getter;
            _setter = (SetHandler)setter;
        }

        protected override Type GetGetDelegateType()
        {
            return typeof(GetHandler);
        }

        protected override Type GetSetDelegateType()
        {
            return typeof(SetHandler);
        }

        public override bool HasValueChanged()
        {
            return !_lastSentValue.Equals(Value);
        }

        protected abstract T Read(string name, IValueReader reader);

        public override void ReadValue(IValueReader reader)
        {
            Value = Read(Name, reader);
        }

        protected abstract void Write(string name, IValueWriter writer, T value);

        public override void WriteValue(IValueWriter writer)
        {
            T value = Value;
            Write(Name, writer, value);
            _lastSentValue = value;
        }

        delegate T GetHandler();

        delegate void SetHandler(T value);
    }

    public abstract class PropertySyncBase : IComparable<PropertySyncBase>
    {
        static readonly Dictionary<Type, Type> _propertySyncTypes = new Dictionary<Type, Type>();
        static readonly Dictionary<Type, List<PropertyInfoData>> _syncValueProperties = new Dictionary<Type, List<PropertyInfoData>>();

        string _name;

        public string Name { get { return _name; } private set { _name = value; } }

        static PropertySyncBase()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).Where(x => x.IsClass);
            foreach (Type type in types)
            {
                var attribs = type.GetCustomAttributes(typeof(PropertySyncHandlerAttribute), true);
                if (attribs.Length < 1)
                    continue;
                if (attribs.Length > 1)
                {
                    const string errmsg = "Multiple PropertySyncHandlerAttributes found on type `{0}`";
                    Debug.Fail(string.Format(errmsg, type));
                    throw new Exception(string.Format(errmsg, type));
                }

                PropertySyncHandlerAttribute attrib = (PropertySyncHandlerAttribute)attribs[0];
                _propertySyncTypes.Add(attrib.HandledType, type);
            }
        }

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
            Delegate g = Delegate.CreateDelegate(GetGetDelegateType(), bindObject, getMethod, true);
            Delegate s = Delegate.CreateDelegate(GetSetDelegateType(), bindObject, setMethod, true);

            GetDelegates(g, s);
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        protected abstract void GetDelegates(Delegate getter, Delegate setter);

        protected abstract Type GetGetDelegateType();

        public static PropertySyncBase GetPropertySync(PropertyInfo propertyInfo, object bindObject)
        {
            if (bindObject == null)
                throw new ArgumentNullException("bindObject");
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            if (!propertyInfo.CanRead || !propertyInfo.CanWrite)
                throw new ArgumentException("Only properties with both a getter and setter can be synchronized.", "propertyInfo");

            Type handledType = propertyInfo.PropertyType;

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

            return (PropertySyncBase)Activator.CreateInstance(syncType, bindObject, propertyInfo);
        }

        protected abstract Type GetSetDelegateType();

        struct PropertyInfoData
        {
            public readonly PropertyInfo PropertyInfo;
            public readonly string Name;

            public PropertyInfoData(PropertyInfo propertyInfo, string name)
            {
                PropertyInfo = propertyInfo;
                Name = name;
            }
        }

        public static IEnumerable<PropertySyncBase> GetPropertySyncs(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            Type type = obj.GetType();

            List<PropertyInfoData> properties;
            if (!_syncValueProperties.TryGetValue(type, out properties))
            {
                const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                       BindingFlags.GetProperty | BindingFlags.SetProperty;

                properties = new List<PropertyInfoData>();
                foreach (var property in type.GetProperties(flags))
                {
                    var attribs = property.GetCustomAttributes(typeof(SyncValueAttribute), true);
                    if (attribs.Length == 0)
                        continue;
                    if (attribs.Length > 1)
                        throw new Exception("Only one SyncValueAttribute allowed per class!");

                    var attribute = (SyncValueAttribute)attribs[0];
                    string name;
                    if (string.IsNullOrEmpty(attribute.CustomName))
                        name = property.Name;
                    else
                        name = attribute.CustomName;

                    properties.Add(new PropertyInfoData(property, name));
                }

                properties.TrimExcess();
                properties.Sort(PropertyInfoAndAttributeComparer);
                _syncValueProperties.Add(type, properties);
            }

            foreach (var p in properties)
            {
                var propertySync = GetPropertySync(p.PropertyInfo, obj);
                propertySync.Name = p.Name;
                yield return propertySync;
            }
        }

        static int PropertyInfoAndAttributeComparer(PropertyInfoData a, PropertyInfoData b)
        {
            return a.PropertyInfo.Name.CompareTo(b.PropertyInfo.Name);
        }

        public abstract bool HasValueChanged();

        public abstract void ReadValue(IValueReader reader);

        public abstract void WriteValue(IValueWriter writer);

        public int CompareTo(PropertySyncBase other)
        {
            return Name.CompareTo(other.Name);
        }
    }
}