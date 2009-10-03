using System.Collections.Generic;
using System.Linq;
using DemoGame;
using NetGore;

namespace DemoGame.Server
{
    public class ItemStats : ItemStatsBase
    {
        public ItemStats(IEnumerable<KeyValuePair<StatType, int>> src, StatCollectionType statCollectionType)
            : base(src, statCollectionType)
        {
        }
    }
}