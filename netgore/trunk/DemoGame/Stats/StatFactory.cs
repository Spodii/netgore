using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Provides helper methods for creating and managing IStats and StatTypes.
    /// </summary>
    public static class StatFactory
    {
        /// <summary>
        /// All of the StatTypes.
        /// </summary>
        static readonly StatType[] _allStatTypes = Enum.GetValues(typeof(StatType)).Cast<StatType>().ToArray();

        /// <summary>
        /// Gets an IEnumerable of all of the StatTypes.
        /// </summary>
        public static IEnumerable<StatType> AllStatTypes
        {
            get { return _allStatTypes; }
        }

        /// <summary>
        /// Gets an IEnumerable of all of the StatTypes who's base value can be raised by a Character.
        /// </summary>
        public static IEnumerable<StatType> RaisableStats
        {
            get { return AllStatTypes; }
        }

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
                    return new Stat<StatValueUShort>(statType, initialValue);
                case StatCollectionType.Modified:
                    return new Stat<StatValueUShort>(statType, initialValue);
                case StatCollectionType.Requirement:
                    return new Stat<StatValueUShort>(statType, initialValue);

                default:
                    throw new ArgumentOutOfRangeException("statCollectionType");
            }
        }
    }
}