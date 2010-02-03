using System.Linq;
using NetGore.Stats;

namespace DemoGame.Server
{
    public abstract class CharacterStats : CharacterStatsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterStats"/> class.
        /// </summary>
        /// <param name="statCollectionType">Type of the stat collection.</param>
        protected CharacterStats(StatCollectionType statCollectionType) : base(statCollectionType)
        {
        }
    }
}