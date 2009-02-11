using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;

namespace DemoGame.Client
{
    public class CharacterStats : CharacterStatsBase
    {
        public CharacterStats()
        {
            CreateStats();
        }

        protected override IStat CreateBaseStat<T>(StatType statType)
        {
            return new Stat<T>(statType);
        }

        protected override IStat CreateModStat<T>(StatType statType)
        {
            return new Stat<T>(statType);
        }

        /// <summary>
        /// Wrapper for creating a new Stat
        /// </summary>
        /// <typeparam name="T">Value type of the stat</typeparam>
        /// <param name="statType">Type of stat</param>
        void NewStat<T>(StatType statType) where T : IStatValueType, new()
        {
            var stat = new Stat<T>(statType);
            Add(stat);
        }
    }
}