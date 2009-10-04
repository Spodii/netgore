using System;
using System.Linq;
using System.Reflection;
using DemoGame;
using log4net;
using NetGore;
using NetGore.AI;
using NetGore.Collections;

namespace DemoGame.Server
{
    /// <summary>
    /// Factory for the AI.
    /// </summary>
    public class AIFactory : AIFactoryBase<Character>
    {
        static readonly AIFactory _instance;

        public static AIFactory Instance { get { return _instance; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="AIFactory"/> class.
        /// </summary>
        AIFactory() : base(typeof(AIBase))
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