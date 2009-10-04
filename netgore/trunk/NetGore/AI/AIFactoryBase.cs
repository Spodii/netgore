using System;
using System.Linq;
using System.Reflection;
using log4net;
using NetGore;
using NetGore.Collections;

namespace NetGore.AI
{
    /// <summary>
    /// The base class for the AI factory.
    /// </summary>
    /// <typeparam name="T">The Type of DynamicEntity that uses the AI.</typeparam>
    public abstract class AIFactoryBase<T> where T : DynamicEntity
    {
        readonly TypeFactory _typeFactory;

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
            _typeFactory.BeginLoading();
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
        /// Handles when a new type has been loaded into a <see cref="TypeFactory"/>.
        /// </summary>
        /// <param name="factoryTypeCollection"><see cref="TypeFactory"/> that the event occured on.</param>
        /// <param name="loadedType">Type that was loaded.</param>
        /// <param name="name">Name of the Type.</param>
        protected virtual void OnLoadTypeHandler(TypeFactory factoryTypeCollection, Type loadedType, string name)
        {
            if (log.IsInfoEnabled)
                log.InfoFormat("Loaded AI `{0}` from Type `{1}`.", name, loadedType);
        }
    }
}