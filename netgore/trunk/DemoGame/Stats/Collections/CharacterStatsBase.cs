using System.Linq;
using NetGore.Stats;

namespace DemoGame
{
    /// <summary>
    /// Base class for a collection of stats for Characters.
    /// </summary>
    public abstract class CharacterStatsBase : StatCollection<StatType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterStatsBase"/> class.
        /// </summary>
        /// <param name="statCollectionType">Type of the stat collection.</param>
        protected CharacterStatsBase(StatCollectionType statCollectionType) : base(statCollectionType)
        {
        }
    }
}