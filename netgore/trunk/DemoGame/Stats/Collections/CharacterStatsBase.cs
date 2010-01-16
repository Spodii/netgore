using System.Linq;
using NetGore.Stats;

namespace DemoGame
{
    /// <summary>
    /// Base class for a collection of stats for Characters.
    /// </summary>
    public abstract class CharacterStatsBase : FullStatCollection<StatType>
    {
        protected CharacterStatsBase(StatCollectionType statCollectionType) : base(statCollectionType)
        {
        }
    }
}