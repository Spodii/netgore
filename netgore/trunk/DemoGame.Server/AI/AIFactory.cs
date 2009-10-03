using System;
using System.Linq;
using System.Reflection;
using DemoGame;
using log4net;
using NetGore;
using NetGore.Collections;

namespace DemoGame.Server
{
    /*
    public class AIFactoryBase : FactoryTypeCollection
    {
        static Func<Type, bool> GetFilter()
        {
            var filter = CreateFilter(typeof(AIBase), true, typeof(Character));

        }

        /// <summary>
        /// </summary>
        /// <param name="typeFilter">Filter that determines the Types to go into this FactoryTypeCollection.</param>
        /// <param name="loadTypeHandler">Initial handler for the OnLoadType event.</param>
        /// <param name="useGAC">If true, Assemblies from the Global Assembly Cache will be included. If false,
        /// the Assemblies in the Global Assembly Cache will be ignored and no Types from these Assemblies will
        /// be found by this FactoryTypeCollection.</param>
        public AIFactoryBase(Func<Type, bool> typeFilter, FactoryTypeLoadedHandler loadTypeHandler, bool useGAC) : base(, loadTypeHandler, useGAC)
        {
        }
    }
    */
    /// <summary>
    /// Factory for the AIBase.
    /// </summary>
    public static class AIFactory
    {
        static readonly FactoryTypeCollection _typeCollection;
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes the <see cref="AIFactory"/> class.
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