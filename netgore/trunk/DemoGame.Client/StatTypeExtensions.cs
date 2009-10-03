using System;
using System.Collections.Generic;
using System.Linq;
using DemoGame;
using NetGore;

namespace DemoGame.Client
{
    public static class StatTypeExtensions
    {
        static readonly Dictionary<StatType, StatType> _baseToMod = new Dictionary<StatType, StatType>();

        /// <summary>
        /// Gets the mod StatType for the given base StatType.
        /// </summary>
        /// <param name="baseStat">Base StatType to find the mod StatType for.</param>
        /// <returns>Mod StatType for the given base StatType.</returns>
        /// <exception cref="ArgumentException">No mod StatType for the given base StatType.</exception>
        public static StatType GetMod(this StatType baseStat)
        {
            StatType modStat;
            if (_baseToMod.TryGetValue(baseStat, out modStat))
                return modStat;

            modStat = GetModFromBase(baseStat);
            _baseToMod.Add(baseStat, modStat);

            return modStat;
        }

        static StatType GetModFromBase(StatType baseStat)
        {
            StatType value = EnumHelper<StatType>.Parse("Mod" + baseStat, true);
            if (!EnumHelper<StatType>.IsDefined(value))
                throw new ArgumentException("No mod StatType for the given base StatType", "baseStat");

            return value;
        }
    }
}