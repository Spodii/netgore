using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using Platyform.Extensions;

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
                    _databaseStats = from stat in new ItemStats() where stat.CanWrite select stat.StatType;

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