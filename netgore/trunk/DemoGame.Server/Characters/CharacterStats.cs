using System.Linq;
using DemoGame;
using NetGore;

namespace DemoGame.Server
{
    public abstract class CharacterStats : CharacterStatsBase
    {
        protected CharacterStats(StatCollectionType statCollectionType) : base(statCollectionType)
        {
        }
    }
}