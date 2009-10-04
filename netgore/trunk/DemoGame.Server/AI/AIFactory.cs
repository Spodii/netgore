using System;
using System.Linq;
using System.Reflection;
using DemoGame;
using log4net;
using NetGore;
using NetGore.Collections;

namespace DemoGame.Server
{
    public abstract class AIFactoryBase
    {
        readonly TypeFactory _typeFactory;

        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="AIFactoryBase"/> class.
        /// </summary>
        protected AIFactoryBase()
        {
            if (log.IsInfoEnabled)
                log.Info("Creating AI factory.");

            var filter = new TypeFilterCreator
            {
                IsClass = true,
                IsAbstract = false,
                Subclass = typeof(AIBase),
                Attributes = new Type[] { typeof(AIAttribute) },
                ConstructorParameters = new Type[] { typeof(Character) },
                MatchAllAttributes = true,
                MatchAllInterfaces = true,
                RequireAttributes = true,
                RequireConstructor = true,
                RequireInterfaces = true
            };

            _typeFactory = new TypeFactory(filter.GetFilter(), OnLoadTypeHandler, false);
            _typeFactory.BeginLoading();
        }

        /// <summary>
        /// Creates an AIBase instance.
        /// </summary>
        /// <param name="aiName">Name of the AI.</param>
        /// <param name="character">Character to bind the AI to.</param>
        /// <returns>An AIBase instance.</returns>
        public AIBase Create(string aiName, Character character)
        {
            return (AIBase)_typeFactory.GetTypeInstance(aiName, new object[] { character });
        }

        /// <summary>
        /// Handles when a new type has been loaded into a FactoryTypeCollection.
        /// </summary>
        /// <param name="factoryTypeCollection">FactoryTypeCollection that the event occured on.</param>
        /// <param name="loadedType">Type that was loaded.</param>
        /// <param name="name">Name of the Type.</param>
        protected virtual void OnLoadTypeHandler(TypeFactory factoryTypeCollection, Type loadedType, string name)
        {
            if (log.IsInfoEnabled)
                log.InfoFormat("Loaded AI `{0}` from Type `{1}`.", name, loadedType);
        }
    }
    
    /// <summary>
    /// Factory for the AI.
    /// </summary>
    public class AIFactory : AIFactoryBase
    {
        static readonly AIFactory _instance;

        public static AIFactory Instance { get { return _instance; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="AIFactory"/> class.
        /// </summary>
        AIFactory()
        {
        }

        /// <summary>
        /// Initializes the <see cref="AIFactory"/> class.
        /// </summary>
        static AIFactory()
        {
            _instance = new AIFactory();
        }
    }
}