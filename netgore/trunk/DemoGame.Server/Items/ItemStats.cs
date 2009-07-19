using System.Collections.Generic;

namespace DemoGame.Server
{
    public class ItemStats : ItemStatsBase
    {
        public ItemStats(StatCollectionType statCollectionType) : base(statCollectionType)
        {
        }

        public ItemStats(IEnumerable<StatTypeValue> src, StatCollectionType statCollectionType) : base(src, statCollectionType)
        {
        }
    }
}