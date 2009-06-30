using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace DemoGame.Server
{
    public class ItemStats : ItemStatsBase
    {
        static IEnumerable<StatType> _databaseStats;

        public static IEnumerable<StatType> DatabaseStats
        {
            get
            {
                if (_databaseStats == null)
                    _databaseStats = new ItemStats().Where(x => x.CanWrite).Select(x => x.StatType);

                return _databaseStats;
            }
        }

        public ItemStats()
        {
        }

        public ItemStats(IEnumerable<IStat> copyStatValuesFrom) : base(copyStatValuesFrom)
        {
        }
    }
}