using System.Linq;
using NetGore.AI;

namespace DemoGame.Server
{
    /// <summary>
    /// Factory for the AI.
    /// </summary>
    public sealed class AIFactory : AIFactoryBase<Character>
    {
        static readonly AIFactory _instance;

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

        /// <summary>
        /// Gets the <see cref="AIFactory"/> instance.
        /// </summary>
        public static IAIFactory<Character> Instance
        {
            get { return _instance; }
        }
    }
}