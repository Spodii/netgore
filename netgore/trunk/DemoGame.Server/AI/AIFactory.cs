using System.Linq;
using NetGore;
using NetGore.AI;

namespace DemoGame.Server
{
    /// <summary>
    /// Factory for the AI.
    /// </summary>
    public class AIFactory : AIFactoryBase<Character>
    {
        static readonly AIFactory _instance;

        public static AIFactory Instance
        {
            get { return _instance; }
        }

        /// <summary>
        /// Initializes the <see cref="AIFactory"/> class.
        /// </summary>
        static AIFactory()
        {
            _instance = new AIFactory();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AIFactory"/> class.
        /// </summary>
        AIFactory() : base(typeof(AIBase))
        {
        }
    }
}