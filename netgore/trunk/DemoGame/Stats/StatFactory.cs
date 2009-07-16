using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DemoGame
{
    public static class StatFactory
    {
        public static IEnumerable<StatType> AllStats
        {
            get { return Enum.GetValues(typeof(StatType)).Cast<StatType>(); }
        }

        public static IEnumerable<StatType> RaisableStats
        {
            get { return AllStats; }
        }

        public static IStat CreateStat(StatType statType, StatCollectionType statCollectionType)
        {
            return CreateStat(statType, statCollectionType, 0);
        }

        public static IStat CreateStat(StatType statType, StatCollectionType statCollectionType, int initialValue)
        {
            switch (statCollectionType)
            {
                case StatCollectionType.Base:
                    return new BaseStat<BaseStatValueType>(statType, initialValue);
                case StatCollectionType.Modified:
                    return new BaseStat<ModStatValueType>(statType, initialValue);
                case StatCollectionType.Requirement:
                    return new BaseStat<ReqStatValueType>(statType, initialValue);
                default:
                    throw new ArgumentOutOfRangeException("statCollectionType");
            }
        }
    }
}