using System;
using System.Linq;

namespace NetGore.Stats
{
    /// <summary>
    /// Provides helper methods for creating and managing <see cref="IStat{StatType}"/>s and
    /// <typeparamref name="TStatType"/>s.
    /// </summary>
    /// <typeparam name="TStatType">The type of stat.</typeparam>
    public static class StatFactory<TStatType> where TStatType : struct, IComparable, IConvertible, IFormattable
    {
        // TODO: Have to make it so people can easily define HOW their stats are created instead of always using StatValueShort

        /// <summary>
        /// Creates an <see cref="IStat{StatType}"/>.
        /// </summary>
        /// <param name="statType">The <typeparamref name="TStatType"/> of the <see cref="IStat{StatType}"/> to create.</param>
        /// <param name="statCollectionType">The <see cref="StatCollectionType"/> to create the
        /// <see cref="IStat{StatType}"/> for.</param>
        /// <returns>The <see cref="IStat{StatType}"/> created using the given <paramref name="statType"/> and
        /// <paramref name="statCollectionType"/>.</returns>
        public static IStat<TStatType> CreateStat(TStatType statType, StatCollectionType statCollectionType)
        {
            return CreateStat(statType, statCollectionType, 0);
        }

        /// <summary>
        /// Creates an <see cref="IStat{StatType}"/>.
        /// </summary>
        /// <param name="statType">The <typeparamref name="TStatType"/> of the <see cref="IStat{StatType}"/> to create.</param>
        /// <param name="statCollectionType">The <see cref="StatCollectionType"/> to create the
        /// <see cref="IStat{StatType}"/> for.</param>
        /// <param name="initialValue">The initial value to give the <see cref="IStat{StatType}"/>.</param>
        /// <returns>The <see cref="IStat{StatType}"/> created using the given <paramref name="statType"/> and
        /// <paramref name="statCollectionType"/> with an initial value of <paramref name="initialValue"/>.</returns>
        public static IStat<TStatType> CreateStat(TStatType statType, StatCollectionType statCollectionType, int initialValue)
        {
            switch (statCollectionType)
            {
                case StatCollectionType.Base:
                    return new Stat<TStatType, StatValueShort>(statType, initialValue);
                case StatCollectionType.Modified:
                    return new Stat<TStatType, StatValueShort>(statType, initialValue);
                case StatCollectionType.Requirement:
                    return new Stat<TStatType, StatValueShort>(statType, initialValue);

                default:
                    throw new ArgumentOutOfRangeException("statCollectionType");
            }
        }
    }
}