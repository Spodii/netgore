using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Platyform.Extensions;

// FUTURE: Could optimize the OnStatChange by only hooking to the Stat.OnChange when OnStatChange has listeners

namespace DemoGame
{
    public abstract class ItemStatsBase : StatCollectionBase
    {
        /// <summary>
        /// Notifies the listener that any of the stats have raised their OnChange event
        /// </summary>
        public event StatChangeHandler OnStatChange;

        protected ItemStatsBase(IEnumerable<IStat> copyStatValuesFrom) : this()
        {
            CopyStatValuesFrom(copyStatValuesFrom, true);
        }

        protected ItemStatsBase()
        {
            Add<StatValueUShort>(StatType.Agi);
            Add<StatValueUShort>(StatType.Armor);
            Add<StatValueUShort>(StatType.Bra);
            Add<StatValueUShort>(StatType.Defence);
            Add<StatValueUShort>(StatType.Dex);
            Add<StatValueUShort>(StatType.Evade);
            Add<StatValueShort>(StatType.HP);
            Add<StatValueUShort>(StatType.Imm);
            Add<StatValueUShort>(StatType.Int);
            Add<StatValueUShort>(StatType.MaxHit);
            Add<StatValueShort>(StatType.MaxMP);
            Add<StatValueShort>(StatType.MaxHP);
            Add<StatValueUShort>(StatType.MinHit);
            Add<StatValueShort>(StatType.MP);
            Add<StatValueUShort>(StatType.Perc);
            Add<StatValueByte>(StatType.ReqAcc);
            Add<StatValueByte>(StatType.ReqAgi);
            Add<StatValueByte>(StatType.ReqArmor);
            Add<StatValueByte>(StatType.ReqBra);
            Add<StatValueByte>(StatType.ReqDex);
            Add<StatValueByte>(StatType.ReqEvade);
            Add<StatValueByte>(StatType.ReqImm);
            Add<StatValueByte>(StatType.ReqInt);
        }

        void Add<T>(StatType statType) where T : IStatValueType, new()
        {
            Add(NewStat<T>(statType));
        }

        /// <summary>
        /// Checks if the stats in this ItemStatsBase contain the same values as another ItemStatsBase
        /// </summary>
        /// <param name="other">ItemStatsBase to compare against</param>
        /// <returns>True if each stat in this ItemStatsBase has the same value as the stats in
        /// <paramref name="other"/>, else false</returns>
        public bool HasEqualValues(ItemStatsBase other)
        {
            // Iterate through each stat
            foreach (IStat stat in this)
            {
                // Check if the values are equal, and return false on the first non-equal value found
                if (other[stat.StatType] != stat.Value)
                    return false;
            }

            // Everything matched, so return true
            return true;
        }

        /// <summary>
        /// Wrapper for creating a new Stat
        /// </summary>
        /// <typeparam name="T">Internal stat value type</typeparam>
        /// <param name="statType">Type of the Stat</param>
        /// <returns>The new Stat</returns>
        Stat<T> NewStat<T>(StatType statType) where T : IStatValueType, new()
        {
            var stat = new Stat<T>(statType);
            stat.OnChange += StatChanged;
            return stat;
        }

        /// <summary>
        /// Handler for listening to all of the stats and forwarding to the OnStatChange
        /// </summary>
        /// <param name="stat">Stat that changed</param>
        void StatChanged(IStat stat)
        {
            if (OnStatChange != null)
                OnStatChange(stat);
        }
    }
}