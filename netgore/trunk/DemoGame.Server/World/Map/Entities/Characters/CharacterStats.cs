using System.Linq;

namespace DemoGame.Server
{
    public abstract class CharacterStats : CharacterStatsBase
    {
        protected CharacterStats(StatCollectionType statCollectionType) : base(statCollectionType)
        {
        }
    }
}