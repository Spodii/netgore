using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using log4net;

namespace NetGore.Collections
{
    /// <summary>
    /// A read-only collection of Types that is specialized for an object factory.
    /// </summary>
    public class TypeFactory : IEnumerable<Type>
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
        /// Initializes a new instance of the <see cref="TypeFactory"/> class.
        /// </summary>
        /// <param name="typeFilter">Filter that determines the Types to go into this <see cref="TypeFactory"/>.</param>
        /// <param name="loadTypeHandler">Initial handler for the TypeLoaded event.</param>
        /// <param name="useGAC">If true, Assemblies from the Global Assembly Cache will be included. If false,
        /// the Assemblies in the Global Assembly Cache will be ignored and no Types from these Assemblies will
        /// be found by this <see cref="TypeFactory"/>.</param>
        public TypeFactory(Func<Type, bool> typeFilter, TypedEventHandler<TypeFactory, TypeFactoryLoadedEventArgs> loadTypeHandler = null, bool useGAC = false)
        {
            _typeFilter = typeFilter;
            _useGAC = useGAC;

            if (loadTypeHandler != null)
            {
                TypeLoaded -= loadTypeHandler;
                TypeLoaded += loadTypeHandler;
            }

            var currentDomain = AppDomain.CurrentDomain;

            // Listen for when new assemblies are loaded
            currentDomain.AssemblyLoad -= CurrentDomain_AssemblyLoad;
            currentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;

            // Load the already loaded assemblies
            foreach (var assembly in currentDomain.GetAssemblies())
            {
                LoadAssemblyTypes(assembly);
            }
        }

        /// <summary>
        /// Notifies listeners when a <see cref="Type"/> has been loaded into this <see cref="TypeFactory"/>.
        /// </summary>
        public event TypedEventHandler<TypeFactory, TypeFactoryLoadedEventArgs> TypeLoaded;

        /// <summary>
        /// Gets a Type from its name.
        /// </summary>
        /// <param name="typeName">Name of the Type to get.</param>
        /// <returns>The Type with the specified name, or null if the <paramref name="typeName"/> is invalid
        /// or does not contain a corresponding <see cref="Type"/>.</returns>
        public Type this[string typeName]
        {
            get
            {
                Type ret;
                if (!_nameToType.TryGetValue(typeName, out ret))
                    return null;

                return ret;
            }
        }

        /// <summary>
        /// Gets the name of a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">Type to get the name of.</param>
        /// <returns>The name of the <paramref name="type"/>, or null if the <paramref name="type"/> is invalid
        /// or is not part of this collection.</returns>
        public string this[Type type]
        {
            get
            {
                string ret;
                if (!_typeToName.TryGetValue(type, out ret))
                    return null;

                return ret;
            }
        }

        /// <summary>
        /// Gets an IEnumerable of all of the <see cref="Type"/>s in this <see cref="TypeFactory"/>.
        /// </summary>
        public IEnumerable<Type> Types
        {
            get { return _typeToName.Keys; }
        }

        /// <summary>
        /// Gets if Assemblies from the Global Assembly Cache will be included. If false,
        /// the Assemblies in the Global Assembly Cache will be ignored and no Types from these Assemblies will
        /// be found by this <see cref="TypeFactory"/>.
        /// </summary>
        public bool UseGAC
        {
            get { return _useGAC; }
        }

        /// <summary>
        /// Handles when a new <see cref="Assembly"/> is loaded.
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
            var instance = Activator.CreateInstance(type, true);
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
            const BindingFlags bf =
                BindingFlags.CreateInstance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            Binder binder = null;
            CultureInfo ci = null;

            var instance = Activator.CreateInstance(type, bf, binder, arguments, ci);
            return instance;
        }

        /// <summary>
        /// Gets an instance of a Type from its name.
        /// </summary>
        /// <param name="typeName">Name of the Type to create an instance of.</param>
        /// <returns>An instance of the Type.</returns>
        /// <exception cref="KeyNotFoundException">No Type with the given <paramref name="typeName"/> exists
        /// in this <see cref="TypeFactory"/>.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "TypeFactory")]
        public object GetTypeInstance(string typeName)
        {
            var type = this[typeName];
            if (type == null)
                throw new KeyNotFoundException(string.Format("No Type with the name `{0}` exists in this TypeFactory.", typeName));

            return GetTypeInstance(type);
        }

        /// <summary>
        /// Gets an instance of a Type from its name.
        /// </summary>
        /// <param name="typeName">Name of the Type to create an instance of.</param>
        /// <param name="arguments">Arguments to use when invoking the constructor.</param>
        /// <returns>An instance of the Type.</returns>
        /// <exception cref="KeyNotFoundException">No Type with the given <paramref name="typeName"/> exists
        /// in this <see cref="TypeFactory"/>.</exception>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "TypeFactory")]
        public object GetTypeInstance(string typeName, params object[] arguments)
        {
            var type = this[typeName];
            if (type == null)
                throw new KeyNotFoundException(string.Format("No Type with the name `{0}` exists in this TypeFactory.", typeName));

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
        /// <exception cref="TypeException">Duplicate <see cref="Type"/> names were found.</exception>
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
            foreach (var type in newTypes)
            {
                var typeName = GetTypeName(type);

                // Ensure the name isn't already in use
                if (_nameToType.ContainsKey(typeName))
                {
                    const string errmsg = "Type name `{0}` is already used by Type `{1}` - cannot add new Type `{2}`.";
                    var err = string.Format(errmsg, typeName, _nameToType[typeName], type);
                    log.Fatal(err);
                    throw new TypeException(err);
                }

                // Add the type/key to the dictionaries
                _nameToType.Add(typeName, type);
                _typeToName.Add(type, typeName);

                // Notify listeners
                OnTypeLoaded(type, typeName);

                if (TypeLoaded != null)
                    TypeLoaded.Raise(this, new TypeFactoryLoadedEventArgs(type, typeName));
            }
        }

        /// <summary>
        /// When overridden in the derived class, allows for additional handling the corresponding event without
        /// the overhead of using event hooks. Therefore, it is recommended that this overload is used instead of
        /// the corresponding event when possible.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="typeName">Name of the type.</param>
        protected virtual void OnTypeLoaded(Type type, string typeName)
        {
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
            foreach (var key in _typeToName.Keys)
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