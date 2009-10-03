using System;
using System.Linq;
using System.Reflection;
using DemoGame;
using log4net;
using NetGore;
using NetGore.Collections;

namespace DemoGame.Server
{
    /// <summary>
    /// Factory for the AIBase.
    /// </summary>
    public static class AIFactory
    {
        static readonly FactoryTypeCollection _typeCollection;
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Static AIFactory constructor.
        /// </summary>
        static AIFactory()
        {
            if (log.IsInfoEnabled)
                log.Info("Initializing the AI factory.");

            var filter = FactoryTypeCollection.CreateFilter(typeof(AIBase), true, typeof(Character));
            _typeCollection = new FactoryTypeCollection(filter, OnLoadTypeHandler, false);
            _typeCollection.BeginLoading();
        }

        /// <summary>
        /// Creates an AIBase instance.
        /// </summary>
        /// <param name="aiName">Name of the AI.</param>
        /// <param name="character">Character to bind the AI to.</param>
        /// <returns>An AIBase instance.</returns>
        public static AIBase Create(string aiName, Character character)
        {
            return (AIBase)_typeCollection.GetTypeInstance(aiName, new object[] { character });
        }

        /// <summary>
        /// Handles when a new type has been loaded into a FactoryTypeCollection.
        /// </summary>
        /// <param name="factoryTypeCollection">FactoryTypeCollection that the event occured on.</param>
        /// <param name="loadedType">Type that was loaded.</param>
        /// <param name="name">Name of the Type.</param>
        static void OnLoadTypeHandler(FactoryTypeCollection factoryTypeCollection, Type loadedType, string name)
        {
            if (log.IsInfoEnabled)
                log.InfoFormat("Loaded AI `{0}` from Type `{1}`.", name, loadedType);
        }
    }
}