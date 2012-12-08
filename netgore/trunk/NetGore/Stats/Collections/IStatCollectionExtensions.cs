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
        /// Copies a collection of values into a <see cref="IStatCollection{TStatType}"/>.
        /// </summary>
        /// <typeparam name="TStatType">The type of stat.</typeparam>
        /// <param name="statColl">The collection to copy the values into.</param>
        /// <param name="values">The values to copy into the <paramref name="statColl"/>.</param>
        public static void CopyValuesFrom<TStatType>(this IStatCollection<TStatType> statColl, IEnumerable<KeyValuePair<TStatType, StatValueType>> values)
            where TStatType : struct, IComparable, IConvertible, IFormattable
        {
            if (values != null)
            {
                foreach (var value in values)
                {
                    statColl[value.Key] = value.Value;
                }
            }
        }

        /// <summary>
        /// Copies a collection of values into a <see cref="IStatCollection{TStatType}"/>.
        /// </summary>
        /// <typeparam name="TStatType">The type of stat.</typeparam>
        /// <param name="statColl">The collection to copy the values into.</param>
        /// <param name="values">The values to copy into the <paramref name="statColl"/>.</param>
        public static void CopyValuesFrom<TStatType>(this IStatCollection<TStatType> statColl, IEnumerable<KeyValuePair<TStatType, int>> values)
            where TStatType : struct, IComparable, IConvertible, IFormattable
        {
            if (values != null)
            {
                foreach (var value in values)
                {
                    statColl[value.Key] = (StatValueType)value.Value;
                }
            }
        }

        /// <summary>
        /// Copies a collection of values into a <see cref="IStatCollection{TStatType}"/>.
        /// </summary>
        /// <typeparam name="TStatType">The type of stat.</typeparam>
        /// <param name="statColl">The collection to copy the values into.</param>
        /// <param name="values">The values to copy into the <paramref name="statColl"/>.</param>
        public static void CopyValuesFrom<TStatType>(this IStatCollection<TStatType> statColl, IEnumerable<Stat<TStatType>> values)
            where TStatType : struct, IComparable, IConvertible, IFormattable
        {
            if (values != null)
            {
                foreach (var value in values)
                {
                    statColl[value.StatType] = value.Value;
                }
            }
        }
    }
}