using System.Linq;
using NetGore.Stats;

namespace DemoGame.Server
{
    public class NPCStats : CharacterStats
    {
        public NPCStats(StatCollectionType statCollectionType) : base(statCollectionType)
        {
        }
    }
}