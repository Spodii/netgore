using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetGore.Collections
{
    /// <summary>
    /// A read-only collection of Types that is specialized for an object factory.
    /// </summary>
    public class FactoryTypeCollection : IEnumerable<Type>
    {
        readonly Dictionary<string, Type> _nameToType;
        readonly Dictionary<Type, string> _typeToName;

        public Type this[string typeName]
        {
            get { return _nameToType[typeName]; }
        }

        public string this[Type type]
        {
            get { return _typeToName[type]; }
        }

        public IEnumerable<Type> Types
        {
            get { return _typeToName.Keys; }
        }

        public FactoryTypeCollection(IEnumerable<Type> types)
        {
            int count = types.Count();

            _nameToType = new Dictionary<string, Type>(count, StringComparer.OrdinalIgnoreCase);
            _typeToName = new Dictionary<Type, string>(count);

            foreach (Type type in types)
            {
                // ReSharper disable DoNotCallOverridableMethodsInConstructor
                string typeName = GetTypeName(type);
                // ReSharper restore DoNotCallOverridableMethodsInConstructor

                _nameToType.Add(typeName, type);
                _typeToName.Add(type, typeName);
            }
        }

        static IEnumerable<Type> AllTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes());
        }

        /// <summary>
        /// Finds all Types that inherit the specified <paramref name="baseClass"/>.
        /// </summary>
        /// <param name="baseClass">Base class or interface to find the classes that inherit.</param>
        /// <param name="parameterlessConstructor">If true, only types with a parameterless constructor will be returned.</param>
        /// <returns>IEnumerable of all Types that inherit the specified <paramref name="baseClass"/>.</returns>
        public static IEnumerable<Type> FindTypesThatInherit(Type baseClass, bool parameterlessConstructor)
        {
            var types = AllTypes().Where(x => x.IsSubclassOf(baseClass));
            if (parameterlessConstructor)
                types = types.Where(x => !x.IsAbstract && x.GetConstructor(new Type[] { }) != null);
            return types;
        }

        public object GetTypeInstance(Type type)
        {
            object instance = Activator.CreateInstance(type, true);
            return instance;
        }

        public object GetTypeInstance(string typeName)
        {
            Type type = this[typeName];
            return GetTypeInstance(type);
        }

        protected virtual string GetTypeName(Type type)
        {
            return type.ToString().Split('.').Last();
        }

        #region IEnumerable<Type> Members

        public IEnumerator<Type> GetEnumerator()
        {
            foreach (Type key in _typeToName.Keys)
            {
                yield return key;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}