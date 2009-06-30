using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore.Collections
{
    /// <summary>
    /// A read-only collection of Types that is specialized for an object factory.
    /// </summary>
    public class FactoryTypeCollection : IEnumerable<Type>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly Dictionary<string, Type> _nameToType;
        readonly Dictionary<Type, string> _typeToName;

        /// <summary>
        /// Gets a Type from its name.
        /// </summary>
        /// <param name="typeName">Name of the Type to get.</param>
        /// <returns>The Type with the specified name.</returns>
        public Type this[string typeName]
        {
            get { return _nameToType[typeName]; }
        }

        /// <summary>
        /// Gets the name of a Type.
        /// </summary>
        /// <param name="type">Type to get the name of.</param>
        /// <returns>The name of the Type.</returns>
        public string this[Type type]
        {
            get { return _typeToName[type]; }
        }

        /// <summary>
        /// Gets an IEnumerable of all of the Types in this FactoryTypeCollection.
        /// </summary>
        public IEnumerable<Type> Types
        {
            get { return _typeToName.Keys; }
        }

        /// <summary>
        /// FactoryTypeCollection constructor.
        /// </summary>
        /// <param name="types">Types to be used in this FactoryTypeCollection.</param>
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
        
        /// <summary>
        /// Creates an instance of a Type.
        /// </summary>
        /// <param name="type">Type to create an instance of.</param>
        /// <returns>An instance of the Type.</returns>
        public static object GetTypeInstance(Type type)
        {
            object instance = Activator.CreateInstance(type, true);
            return instance;
        }

        /// <summary>
        /// Gets an instance of a Type from its name.
        /// </summary>
        /// <param name="typeName">Name of the Type to create an instance of.</param>
        /// <returns>An instance of the Type.</returns>
        public object GetTypeInstance(string typeName)
        {
            Type type = this[typeName];
            return GetTypeInstance(type);
        }

        /// <summary>
        /// Gets the name of a Type.
        /// </summary>
        /// <param name="type">Type to get the name of.</param>
        /// <returns>The name of the Type.</returns>
        protected virtual string GetTypeName(Type type)
        {
            string name = type.ToString().Split('.').Last();
            return name;
        }

        #region IEnumerable<Type> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<Type> GetEnumerator()
        {
            foreach (Type key in _typeToName.Keys)
            {
                yield return key;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}