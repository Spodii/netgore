using System.Linq;
using NetGore.Stats;

namespace DemoGame
{
    /// <summary>
    /// Provides helper methods for working with the modified Stats.
    /// </summary>
    public static class ModStatHelper
    {
        /// <summary>
        /// Calculates the modified stat value.
        /// </summary>
        /// <param name="baseStats">The collection of unmodified stats to read the base stat value from.</param>
        /// <param name="statType">The <see cref="StatType"/> to calculate the value for.</param>
        /// <param name="modders">The <see cref="IModStatContainer"/>s to use for calculating the modified Stat value.</param>
        /// <returns>The modified Stat value.</returns>
        public static int Calculate(IStatCollection<StatType> baseStats, StatType statType, params IModStatContainer[] modders)
        {
            int value = baseStats[statType];

            if (modders == null || modders.Length == 0)
                return value;

            foreach (IModStatContainer modder in modders)
            {
                value += modder.GetStatModBonus(statType);
            }

            return value;
        }

        /// <summary>
        /// Calculates the modified stat value.
        /// </summary>
        /// <param name="baseStats">The collection of unmodified Stats to read the base stat value from.</param>
        /// <param name="statType">The <see cref="StatType"/> to calculate the value for.</param>
        /// <param name="modder1">The first <see cref="IModStatContainer"/>s to use for calculating the modified
        /// stat value.</param>
        /// <returns>The modified Stat value.</returns>
        public static int Calculate(IStatCollection<StatType> baseStats, StatType statType, IModStatContainer modder1)
        {
            int value = baseStats[statType];

            value += modder1.GetStatModBonus(statType);

            return value;
        }

        /// <summary>
        /// Calculates the modified Stat value.
        /// </summary>
        /// <param name="baseStats">The collection of unmodified Stats to read the base stat value from.</param>
        /// <param name="statType">The <see cref="StatType"/> to calculate the value for.</param>
        /// <param name="modder1">The first <see cref="IModStatContainer"/>s to use for calculating the modified Stat
        /// value.</param>
        /// <param name="modder2">The second <see cref="IModStatContainer"/>s to use for calculating the modified Stat
        /// value.</param>
        /// <returns>The modified Stat value.</returns>
        public static int Calculate(IStatCollection<StatType> baseStats, StatType statType, IModStatContainer modder1,
                                    IModStatContainer modder2)
        {
            int value = baseStats[statType];

            value += modder1.GetStatModBonus(statType);
            value += modder2.GetStatModBonus(statType);

            return value;
        }

        /// <summary>
        /// Calculates the modified Stat value.
        /// </summary>
        /// <param name="baseStats">The collection of unmodified Stats to read the base stat value from.</param>
        /// <param name="statType">The <see cref="StatType"/> to calculate the value for.</param>
        /// <param name="modder1">The first <see cref="IModStatContainer"/>s to use for calculating the modified
        /// Stat value.</param>
        /// <param name="modder2">The second <see cref="IModStatContainer"/>s to use for calculating the modified
        /// Stat value.</param>
        /// <param name="modder3">The third <see cref="IModStatContainer"/>s to use for calculating the modified
        /// Stat value.</param>
        /// <returns>The modified Stat value.</returns>
        public static int Calculate(IStatCollection<StatType> baseStats, StatType statType, IModStatContainer modder1,
                                    IModStatContainer modder2, IModStatContainer modder3)
        {
            int value = baseStats[statType];

            value += modder1.GetStatModBonus(statType);
            value += modder2.GetStatModBonus(statType);
            value += modder3.GetStatModBonus(statType);

            return value;
        }

        /// <summary>
        /// Calculates the modified Stat value.
        /// </summary>
        /// <param name="baseStats">The collection of unmodified Stats to read the base stat value from.</param>
        /// <param name="statType">The <see cref="StatType"/> to calculate the value for.</param>
        /// <param name="modder1">The first <see cref="IModStatContainer"/>s to use for calculating the modified Stat
        /// value.</param>
        /// <param name="modder2">The second <see cref="IModStatContainer"/>s to use for calculating the modified Stat
        /// value.</param>
        /// <param name="modder3">The third <see cref="IModStatContainer"/>s to use for calculating the modified Stat
        /// value.</param>
        /// <param name="modder4">The fourth <see cref="IModStatContainer"/>s to use for calculating the modified Stat
        /// value.</param>
        /// <returns>The modified Stat value.</returns>
        public static int Calculate(IStatCollection<StatType> baseStats, StatType statType, IModStatContainer modder1,
                                    IModStatContainer modder2, IModStatContainer modder3, IModStatContainer modder4)
        {
            int value = baseStats[statType];

            value += modder1.GetStatModBonus(statType);
            value += modder2.GetStatModBonus(statType);
            value += modder3.GetStatModBonus(statType);
            value += modder4.GetStatModBonus(statType);

            return value;
        }
    }
}