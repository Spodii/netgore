using System.Linq;
using NetGore.Stats;

namespace DemoGame.Server
{
    public abstract class CharacterStats : CharacterStatsBase
    {
        protected CharacterStats(StatCollectionType statCollectionType) : base(statCollectionType)
        {
        }
    }
}