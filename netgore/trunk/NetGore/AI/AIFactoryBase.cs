using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;
using NetGore.World;

namespace NetGore.AI
{
    /// <summary>
    /// The base class for the AI factory.
    /// </summary>
    /// <typeparam name="T">The Type of DynamicEntity that uses the AI.</typeparam>
    public abstract class AIFactoryBase<T> : IAIFactory<T> where T : DynamicEntity
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly Dictionary<AIID, Type> _aiByID = new Dictionary<AIID, Type>();
        readonly TypeFactory _typeFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AIFactoryBase{T}"/> class.
        /// </summary>
        /// <param name="subclass">The Type of the subclass that all AI classes must be derived from. If null, the
        /// subclass will not be enforced.</param>
        protected AIFactoryBase(Type subclass)
        {
            if (log.IsInfoEnabled)
                log.Info("Creating AI factory.");

            var filter = new TypeFilterCreator
            {
                IsClass = true,
                IsAbstract = false,
                Subclass = subclass,
                Interfaces = new Type[] { typeof(IAI) },
                Attributes = new Type[] { typeof(AIAttribute) },
                ConstructorParameters = new Type[] { typeof(T) },
                MatchAllAttributes = true,
                MatchAllInterfaces = true,
                RequireAttributes = true,
                RequireConstructor = true,
                RequireInterfaces = (subclass != null ? true : false)
            };

            _typeFactory = new TypeFactory(filter.GetFilter(), OnLoadTypeHandler);
        }

        /// <summary>
        /// Handles when a new type has been loaded into a <see cref="TypeFactory"/>.
        /// </summary>
        /// <param name="typeFactory">The type factory.</param>
        /// <param name="e">The <see cref="NetGore.Collections.TypeFactoryLoadedEventArgs"/> instance containing the event data.</param>
        /// <exception cref="ArgumentException"><see cref="TypeFactoryLoadedEventArgs.LoadedType"/> does not contain
        /// the <see cref="AIAttribute"/>.</exception>
        /// <exception cref="DuplicateKeyException">The loaded type in <paramref name="e"/> was already loaded.</exception>
        protected virtual void OnLoadTypeHandler(TypeFactory typeFactory, TypeFactoryLoadedEventArgs e)
        {
            var aiAttributes = e.LoadedType.GetCustomAttributes(typeof(AIAttribute), false).Cast<AIAttribute>();

            if (aiAttributes.IsEmpty())
                throw new ArgumentException(
                    string.Format("Expected loaded AI Type {0} to have one or more AIAttributes.", e.LoadedType), "e");

            foreach (var aiAttribute in aiAttributes)
            {
                var id = aiAttribute.ID;

                // Ensure the ID is not already in use
                if (_aiByID.ContainsKey(id))
                {
                    const string errmsg = "Failed to load AI `{0}` - AIID `{1}` is already in use by Type `{2}`";
                    var err = string.Format(errmsg, e.LoadedType, id, _aiByID[id]);
                    if (log.IsFatalEnabled)
                        log.Fatal(err);
                    Debug.Fail(err);
                    throw new DuplicateKeyException(err);
                }

                _aiByID.Add(id, e.LoadedType);

                if (log.IsInfoEnabled)
                    log.InfoFormat("Loaded AI `{0}` from Type `{1}`.", e.Name, e.LoadedType);
            }
        }

        #region IAIFactory<T> Members

        /// <summary>
        /// Gets all of the <see cref="AIID"/>s and the corresponding <see cref="Type"/> used to handle it.
        /// </summary>
        public IEnumerable<KeyValuePair<AIID, Type>> AIs
        {
            get { return _aiByID; }
        }

        /// <summary>
        /// Creates an <see cref="IAI"/> instance.
        /// </summary>
        /// <param name="aiName">Name of the AI.</param>
        /// <param name="entity"><see cref="DynamicEntity"/> to bind the AI to.</param>
        /// <returns>An <see cref="IAI"/> instance.</returns>
        public virtual IAI Create(string aiName, T entity)
        {
            return (IAI)_typeFactory.GetTypeInstance(aiName, new object[] { entity });
        }

        /// <summary>
        /// Creates an <see cref="IAI"/> instance.
        /// </summary>
        /// <param name="id">ID of the AI.</param>
        /// <param name="entity"><see cref="DynamicEntity"/> to bind the AI to.</param>
        /// <returns>An <see cref="IAI"/> instance.</returns>
        public IAI Create(AIID id, T entity)
        {
            var type = GetAIType(id);
            var instance = (IAI)TypeFactory.GetTypeInstance(type, new object[] { entity });
            Debug.Assert(instance.ID == id);
            return instance;
        }

        /// <summary>
        /// Gets the name of the AI for the given <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to get the AI name for.</param>
        /// <returns>
        /// The name of the AI for the given <paramref name="type"/>, or null if the
        /// <paramref name="type"/> is invalid or does not correspond to an AI.
        /// </returns>
        public string GetAIName(Type type)
        {
            if (type == null)
                return null;

            return _typeFactory[type];
        }

        /// <summary>
        /// Gets the name of the AI for the given <see cref="AIID"/>.
        /// </summary>
        /// <param name="aiID">The <see cref="AIID"/> to get the AI name for.</param>
        /// <returns>
        /// The name of the AI for the given <paramref name="aiID"/>, or null if the
        /// <paramref name="aiID"/> is invalid or does not correspond to an AI.
        /// </returns>
        public string GetAIName(AIID aiID)
        {
            var type = GetAIType(aiID);
            return GetAIName(type);
        }

        /// <summary>
        /// Gets the <see cref="Type"/> used to handle the specified <see cref="AIID"/>.
        /// </summary>
        /// <param name="aiID">The <see cref="AIID"/> to get the <see cref="Type"/> for.</param>
        /// <returns>The <see cref="Type"/> of the class for handling the <paramref name="aiID"/>, or null
        /// if invalid or no value was found.</returns>
        public Type GetAIType(AIID aiID)
        {
            Type ret;
            if (!_aiByID.TryGetValue(aiID, out ret))
                return null;

            return ret;
        }

        /// <summary>
        /// Gets the <see cref="Type"/> used to handle the AI with the given name.
        /// </summary>
        /// <param name="aiName">The name of the AI to get the <see cref="Type"/> for.</param>
        /// <returns>The <see cref="Type"/> of the class for handling the <paramref name="aiName"/>, or null
        /// if invalid or no value was found.</returns>
        public Type GetAIType(string aiName)
        {
            return _typeFactory[aiName];
        }

        #endregion
    }
}