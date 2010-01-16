using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Stats
{
    public static class IStatCollectionExtensions
    {
        public static IEnumerable<KeyValuePair<TStatType, int>> ToKeyValuePairs<TStatType>(
            this IEnumerable<IStat<TStatType>> stats) where TStatType : struct, IComparable, IConvertible, IFormattable
        {
            return stats.Select(x => new KeyValuePair<TStatType, int>(x.StatType, x.Value)).ToImmutable();
        }
    }
}