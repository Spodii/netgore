using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore.Collections;

namespace NetGore.AI
{
    /// <summary>
    /// The base class for the AI factory.
    /// </summary>
    /// <typeparam name="T">The Type of DynamicEntity that uses the AI.</typeparam>
    public abstract class AIFactoryBase<T> where T : DynamicEntity
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        readonly Dictionary<AIID, Type> _aiByID = new Dictionary<AIID, Type>();
        readonly TypeFactory _typeFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AIFactoryBase&lt;T&gt;"/> class.
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

            _typeFactory = new TypeFactory(filter.GetFilter(), OnLoadTypeHandler, false);
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
            var type = _aiByID[id];
            var instance = (IAI)TypeFactory.GetTypeInstance(type, new object[] { entity });
            Debug.Assert(instance.ID == id);
            return instance;
        }

        /// <summary>
        /// Handles when a new type has been loaded into a <see cref="TypeFactory"/>.
        /// </summary>
        /// <param name="typeFactory"><see cref="TypeFactory"/> that the event occured on.</param>
        /// <param name="loadedType">Type that was loaded.</param>
        /// <param name="name">Name of the Type.</param>
        protected virtual void OnLoadTypeHandler(TypeFactory typeFactory, Type loadedType, string name)
        {
            var aiAttributes = loadedType.GetCustomAttributes(typeof(AIAttribute), false).Cast<AIAttribute>();

            if (aiAttributes.Count() == 0)
                throw new Exception(string.Format("Expected loaded AI Type {0} to have one or more AIAttributes.", loadedType));

            foreach (var aiAttribute in aiAttributes)
            {
                var id = aiAttribute.ID;

                // Ensure the ID is not already in use
                if (_aiByID.ContainsKey(id))
                {
                    const string errmsg = "Failed to load AI `{0}` - AIID `{1}` is already in use by Type `{2}`";
                    string err = string.Format(errmsg, loadedType, id, _aiByID[id]);
                    if (log.IsFatalEnabled)
                        log.Fatal(err);
                    Debug.Fail(err);
                    throw new Exception(err);
                }

                _aiByID.Add(id, loadedType);

                if (log.IsInfoEnabled)
                    log.InfoFormat("Loaded AI `{0}` from Type `{1}`.", name, loadedType);
            }
        }
    }
}