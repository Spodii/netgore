using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using NetGore.Collections;
using NetGore.Scripting;

namespace DemoGame.Server
{
    public static class AIFactory
    {
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        static readonly Type _handledType = typeof(AIBase);
        static readonly Type[] _constructorParams = new Type[] { typeof(Character) };
        static FactoryTypeCollection _typeCollection;

        static AIFactory()
        {
            if (log.IsInfoEnabled)
                log.Info("Initializing the AI factory.");

            var types = ScriptTypeHelper.GetTypes(_handledType, _constructorParams);
            _typeCollection = new FactoryTypeCollection(types);
        }

        public static AIBase Create(string aiName, Character character)
        {
            return (AIBase)_typeCollection.GetTypeInstance(aiName, new object[] { character });
        }
    }
}
