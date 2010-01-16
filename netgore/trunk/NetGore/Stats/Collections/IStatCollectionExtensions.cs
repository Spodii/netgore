using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGore.Stats
{
    /// <summary>
    /// Extension methods for the <see cref="IStatCollection{StatType}"/>.
    /// </summary>
    public static class IStatCollectionExtensions
    {
        /// <summary>
        /// Gets the <typeparamref name="TStatType"/> and stat value as a <see cref="KeyValuePair{T,U}"/>.
        /// </summary>
        /// <typeparam name="TStatType">The type of stat.</typeparam>
        /// <param name="stats">The <see cref="IStat{StatType}"/>s to create the <see cref="KeyValuePair{T,U}"/>
        /// from.</param>
        /// <returns>The <see cref="KeyValuePair{T,U}"/>s for the <paramref name="stats"/>.</returns>
        public static IEnumerable<KeyValuePair<TStatType, int>> ToKeyValuePairs<TStatType>(
            this IEnumerable<IStat<TStatType>> stats) where TStatType : struct, IComparable, IConvertible, IFormattable
        {
            return stats.Select(x => new KeyValuePair<TStatType, int>(x.StatType, x.Value)).ToImmutable();
        }
    }
}