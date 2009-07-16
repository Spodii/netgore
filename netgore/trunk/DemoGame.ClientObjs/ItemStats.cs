using System.Collections.Generic;

namespace DemoGame.Client
{
    public class ItemStats : ItemStatsBase
    {
        public ItemStats(IEnumerable<IStat> copyStatValuesFrom, StatCollectionType statCollectionType) : base(copyStatValuesFrom, statCollectionType)
        {
        }

        public ItemStats(StatCollectionType statCollectionType) : base(statCollectionType)
        {
        }
    }
}