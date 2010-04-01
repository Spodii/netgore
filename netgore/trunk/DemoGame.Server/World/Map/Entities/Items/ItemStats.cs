using System.Collections.Generic;
using System.Linq;
using NetGore.Stats;

namespace DemoGame.Server
{
    public class ItemStats : ItemStatsBase
    {
        public ItemStats(IEnumerable<Stat<StatType>> src, StatCollectionType statCollectionType)
            : base(src, statCollectionType)
        {
        }
    }
}