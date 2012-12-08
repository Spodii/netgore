using System;
using System.Linq;
using NetGore.Stats;

namespace NetGore
{
    /// <summary>
    /// Provides helper methods for working with the modified Stats.
    /// </summary>
    public static class ModStatHelper
    {
        /// <summary>
        /// Calculates the modified stat value.
        /// </summary>
        /// <typeparam name="TStatType">The type of the stat.</typeparam>
        /// <param name="baseStats">The collection of unmodified stats to read the base stat value from.</param>
        /// <param name="statType">The <typeparamref name="TStatType"/> to calculate the value for.</param>
        /// <param name="modders">The <see cref="IModStatContainer{TStatType}"/>s to use for calculating the modified
        /// stat value.</param>
        /// <returns>The modified stat value.</returns>
        public static int Calculate<TStatType>(IStatCollection<TStatType> baseStats, TStatType statType, params IModStatContainer<TStatType>[] modders)
            where TStatType : struct, IComparable, IConvertible, IFormattable
        {
            int value = (int)baseStats[statType];

            if (modders == null || modders.Length == 0)
                return value;

            foreach (var modder in modders)
            {
                value += modder.GetStatModBonus(statType);
            }

            return value;
        }

        /// <summary>
        /// Calculates the modified stat value.
        /// </summary>
        /// <typeparam name="TStatType">The type of the stat.</typeparam>
        /// <param name="baseStats">The collection of unmodified Stats to read the base stat value from.</param>
        /// <param name="statType">The <typeparamref name="TStatType"/> to calculate the value for.</param>
        /// <param name="modder1">The first <see cref="IModStatContainer{TStatType}"/>s to use for calculating
        /// the modified stat value.</param>
        /// <returns>The modified Stat value.</returns>
        public static int Calculate<TStatType>(IStatCollection<TStatType> baseStats, TStatType statType, IModStatContainer<TStatType> modder1)
            where TStatType : struct, IComparable, IConvertible, IFormattable
        {
            int value = (int)baseStats[statType];

            value += modder1.GetStatModBonus(statType);

            return value;
        }

        /// <summary>
        /// Calculates the modified Stat value.
        /// </summary>
        /// <typeparam name="TStatType">The type of the stat.</typeparam>
        /// <param name="baseStats">The collection of unmodified Stats to read the base stat value from.</param>
        /// <param name="statType">The <typeparamref name="TStatType"/> to calculate the value for.</param>
        /// <param name="modder1">The first <see cref="IModStatContainer{TStatType}"/>s to use for calculating
        /// the modified Stat value.</param>
        /// <param name="modder2">The second <see cref="IModStatContainer{TStatType}"/>s to use for calculating
        /// the modified Stat value.</param>
        /// <returns>The modified Stat value.</returns>
        public static int Calculate<TStatType>(IStatCollection<TStatType> baseStats, TStatType statType,
                                               IModStatContainer<TStatType> modder1, IModStatContainer<TStatType> modder2)
            where TStatType : struct, IComparable, IConvertible, IFormattable
        {
            int value = (int)baseStats[statType];

            value += modder1.GetStatModBonus(statType);
            value += modder2.GetStatModBonus(statType);

            return value;
        }

        /// <summary>
        /// Calculates the modified Stat value.
        /// </summary>
        /// <typeparam name="TStatType">The type of the stat.</typeparam>
        /// <param name="baseStats">The collection of unmodified Stats to read the base stat value from.</param>
        /// <param name="statType">The <typeparamref name="TStatType"/> to calculate the value for.</param>
        /// <param name="modder1">The first <see cref="IModStatContainer{TStatType}"/>s to use for calculating
        /// the modified stat value.</param>
        /// <param name="modder2">The second <see cref="IModStatContainer{TStatType}"/>s to use for calculating
        /// the modified stat value.</param>
        /// <param name="modder3">The third <see cref="IModStatContainer{TStatType}"/>s to use for calculating
        /// the modified stat value.</param>
        /// <returns>The modified stat value.</returns>
        public static int Calculate<TStatType>(IStatCollection<TStatType> baseStats, TStatType statType,
                                               IModStatContainer<TStatType> modder1, IModStatContainer<TStatType> modder2, IModStatContainer<TStatType> modder3)
            where TStatType : struct, IComparable, IConvertible, IFormattable
        {
            int value = (int)baseStats[statType];

            value += modder1.GetStatModBonus(statType);
            value += modder2.GetStatModBonus(statType);
            value += modder3.GetStatModBonus(statType);

            return value;
        }

        /// <summary>
        /// Calculates the modified Stat value.
        /// </summary>
        /// <typeparam name="TStatType">The type of the stat.</typeparam>
        /// <param name="baseStats">The collection of unmodified Stats to read the base stat value from.</param>
        /// <param name="statType">The <typeparamref name="TStatType"/> to calculate the value for.</param>
        /// <param name="modder1">The first <see cref="IModStatContainer{TStatType}"/>s to use for calculating
        /// the modified Stat value.</param>
        /// <param name="modder2">The second <see cref="IModStatContainer{TStatType}"/>s to use for calculating
        /// the modified Stat value.</param>
        /// <param name="modder3">The third <see cref="IModStatContainer{TStatType}"/>s to use for calculating
        /// the modified Stat value.</param>
        /// <param name="modder4">The fourth <see cref="IModStatContainer{TStatType}"/>s to use for calculating
        /// the modified Stat value.</param>
        /// <returns>The modified Stat value.</returns>
        public static int Calculate<TStatType>(IStatCollection<TStatType> baseStats, TStatType statType,
                                               IModStatContainer<TStatType> modder1, IModStatContainer<TStatType> modder2,
                                               IModStatContainer<TStatType> modder3, IModStatContainer<TStatType> modder4)
            where TStatType : struct, IComparable, IConvertible, IFormattable
        {
            int value = (int)baseStats[statType];

            value += modder1.GetStatModBonus(statType);
            value += modder2.GetStatModBonus(statType);
            value += modder3.GetStatModBonus(statType);
            value += modder4.GetStatModBonus(statType);

            return value;
        }
    }
}