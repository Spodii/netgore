using System;
using System.Collections.Generic;
using System.Linq;

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
                    return new Stat<BaseStatValueType>(statType, initialValue);
                case StatCollectionType.Modified:
                    return new Stat<ModStatValueType>(statType, initialValue);
                case StatCollectionType.Requirement:
                    return new Stat<ReqStatValueType>(statType, initialValue);
                default:
                    throw new ArgumentOutOfRangeException("statCollectionType");
            }
        }
    }
}