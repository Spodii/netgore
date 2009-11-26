using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace NetGore.Collections
{
    /// <summary>
    /// A read-only collection of Types that is specialized for an object factory.
    /// </summary>
    public class TypeFactory : IEnumerable<Type>
    {
        /// <summary>
        /// Contains a List of Assemblies that have already been loaded (used to prevent loading an Assembly more than once).
        /// </summary>
        readonly List<Assembly> _loadedAssemblies = new List<Assembly>();

        readonly Dictionary<string, Type> _nameToType = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Func used to filter what Types we want to handle.
        /// </summary>
        readonly Func<Type, bool> _typeFilter;

        readonly Dictionary<Type, string> _typeToName = new Dictionary<Type, string>();
        readonly bool _useGAC;

        /// <summary>
        /// Notifies listeners when a Type has been loaded into this <see cref="TypeFactory"/>.
        /// </summary>
        public event TypeFactoryLoadedHandler OnLoadType;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeFactory"/> class.
        /// </summary>
        /// <param name="typeFilter">Filter that determines the Types to go into this FactoryTypeCollection.</param>
        public TypeFactory(Func<Type, bool> typeFilter) : this(typeFilter, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeFactory"/> class.
        /// </summary>
        /// <param name="typeFilter">Filter that determines the Types to go into this FactoryTypeCollection.</param>
        /// <param name="loadTypeHandler">Initial handler for the OnLoadType event.</param>
        public TypeFactory(Func<Type, bool> typeFilter, TypeFactoryLoadedHandler loadTypeHandler)
            : this(typeFilter, loadTypeHandler, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeFactory"/> class.
        /// </summary>
        /// <param name="typeFilter">Filter that determines the Types to go into this FactoryTypeCollection.</param>
        /// <param name="loadTypeHandler">Initial handler for the OnLoadType event.</param>
        /// <param name="useGAC">If true, Assemblies from the Global Assembly Cache will be included. If false,
        /// the Assemblies in the Global Assembly Cache will be ignored and no Types from these Assemblies will
        /// be found by this FactoryTypeCollection.</param>
        public TypeFactory(Func<Type, bool> typeFilter, TypeFactoryLoadedHandler loadTypeHandler, bool useGAC)
        {
            _typeFilter = typeFilter;
            _useGAC = useGAC;

            if (loadTypeHandler != null)
                OnLoadType += loadTypeHandler;

            // Listen for when new assemblies are loaded
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;

            // Load the already loaded assemblies
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                LoadAssemblyTypes(assembly);
            }
        }

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
        /// Gets if Assemblies from the Global Assembly Cache will be included. If false,
        /// the Assemblies in the Global Assembly Cache will be ignored and no Types from these Assemblies will
        /// be found by this FactoryTypeCollection.
        /// </summary>
        public bool UseGAC
        {
            get { return _useGAC; }
        }

        /// <summary>
        /// Handles when a new Assembly is loaded.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Args.</param>
        void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            LoadAssemblyTypes(args.LoadedAssembly);
        }

        /// <summary>
        /// Creates an instance of a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">Type to create an instance of.</param>
        /// <returns>An instance of the Type.</returns>
        public static object GetTypeInstance(Type type)
        {
            object instance = Activator.CreateInstance(type, true);
            return instance;
        }

        /// <summary>
        /// Creates an instance of a Type.
        /// </summary>
        /// <param name="type">Type to create an instance of.</param>
        /// <param name="arguments">Arguments to use when invoking the constructor.</param>
        /// <returns>An instance of the Type.</returns>
        public static object GetTypeInstance(Type type, params object[] arguments)
        {
            object instance = Activator.CreateInstance(type, arguments);
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
        /// Gets an instance of a Type from its name.
        /// </summary>
        /// <param name="typeName">Name of the Type to create an instance of.</param>
        /// <param name="arguments">Arguments to use when invoking the constructor.</param>
        /// <returns>An instance of the Type.</returns>
        public object GetTypeInstance(string typeName, params object[] arguments)
        {
            Type type = this[typeName];
            return GetTypeInstance(type, arguments);
        }

        /// <summary>
        /// Gets the name of a <see cref="Type"/> for this <see cref="TypeFactory"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to get the name of.</param>
        /// <returns>The name of the <see cref="Type"/>.</returns>
        public virtual string GetTypeName(Type type)
        {
            return type.Name;
        }

        /// <summary>
        /// Loads the <see cref="Type"/>s of an <see cref="Assembly"/>.
        /// </summary>
        /// <param name="assembly"><see cref="Assembly"/> to load the Types from.</param>
        void LoadAssemblyTypes(Assembly assembly)
        {
            // Check if this is from the Global Assembly Cache
            if (!UseGAC && assembly.GlobalAssemblyCache)
                return;

            // Ensure we haven't already loaded this assembly
            lock (_loadedAssemblies)
            {
                if (_loadedAssemblies.Contains(assembly))
                    return;
                _loadedAssemblies.Add(assembly);
            }

            // Load all the Types from the Assembly
            var newTypes = assembly.GetTypes().Where(_typeFilter);
            foreach (Type type in newTypes)
            {
                string typeName = GetTypeName(type);
                _nameToType.Add(typeName, type);
                _typeToName.Add(type, typeName);

                if (OnLoadType != null)
                    OnLoadType(this, type, typeName);
            }
        }

        #region IEnumerable<Type> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
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
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}