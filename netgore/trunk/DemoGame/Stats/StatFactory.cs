using System;
using System.Linq;
using DemoGame;
using NetGore;

namespace DemoGame
{
    /// <summary>
    /// Provides helper methods for creating and managing IStats and StatTypes.
    /// </summary>
    public static class StatFactory
    {
        /// <summary>
        /// Creates an IStat.
        /// </summary>
        /// <param name="statType">The StatType of the IStat to create.</param>
        /// <param name="statCollectionType">The StatCollectionType to create the IStat for.</param>
        /// <returns>The IStat created using the given <paramref name="statType"/> and
        /// <paramref name="statCollectionType"/>.</returns>
        public static IStat CreateStat(StatType statType, StatCollectionType statCollectionType)
        {
            return CreateStat(statType, statCollectionType, 0);
        }

        /// <summary>
        /// Creates an IStat.
        /// </summary>
        /// <param name="statType">The StatType of the IStat to create.</param>
        /// <param name="statCollectionType">The StatCollectionType to create the IStat for.</param>
        /// <param name="initialValue">The initial value to give the IStat.</param>
        /// <returns>The IStat created using the given <paramref name="statType"/> and
        /// <paramref name="statCollectionType"/> with an initial value of <paramref name="initialValue"/>.</returns>
        public static IStat CreateStat(StatType statType, StatCollectionType statCollectionType, int initialValue)
        {
            switch (statCollectionType)
            {
                case StatCollectionType.Base:
                    return new Stat<StatValueShort>(statType, initialValue);
                case StatCollectionType.Modified:
                    return new Stat<StatValueShort>(statType, initialValue);
                case StatCollectionType.Requirement:
                    return new Stat<StatValueShort>(statType, initialValue);

                default:
                    throw new ArgumentOutOfRangeException("statCollectionType");
            }
        }
    }
}