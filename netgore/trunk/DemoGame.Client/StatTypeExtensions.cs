using System;
using System.Collections.Generic;
using System.Linq;
using NetGore;

namespace DemoGame.Client
{
    public static class StatTypeExtensions
    {
        static readonly Dictionary<StatType, StatType> _baseToMod = new Dictionary<StatType, StatType>();

        /// <summary>
        /// Gets the mod <see cref="StatType"/> for the given base <see cref="StatType"/>.
        /// </summary>
        /// <param name="baseStat">Base <see cref="StatType"/> to find the mod <see cref="StatType"/> for.</param>
        /// <returns>Mod <see cref="StatType"/> for the given base <see cref="StatType"/>.</returns>
        /// <exception cref="ArgumentException">No mod <see cref="StatType"/> for the given base
        /// <see cref="StatType"/>.</exception>
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
            var value = EnumHelper<StatType>.Parse("Mod" + baseStat, true);
            if (!EnumHelper<StatType>.IsDefined(value))
                throw new ArgumentException("No mod StatType for the given base StatType", "baseStat");

            return value;
        }
    }
}