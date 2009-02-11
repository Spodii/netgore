using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DemoGame
{
    /// <summary>
    /// Base class for a collection of stats for Characters.
    /// </summary>
    public abstract class CharacterStatsBase : StatCollectionBase
    {
        static readonly StatType[] _modStats = new StatType[]
                                               {
                                                   StatType.ModAcc, StatType.ModAgi, StatType.ModArmor, StatType.ModBra,
                                                   StatType.ModDex, StatType.ModEvade, StatType.ModImm, StatType.ModInt,
                                                   StatType.ModPerc, StatType.ModRecov, StatType.ModRegen, StatType.ModStr,
                                                   StatType.ModTact, StatType.ModWS, StatType.MinHit, StatType.MaxHit,
                                                   StatType.Defence
                                               };

        static readonly StatType[] _nonModStats = new StatType[]
                                                  {
                                                      StatType.Acc, StatType.Agi, StatType.Armor, StatType.Bra, StatType.Cash,
                                                      StatType.Dex, StatType.Evade, StatType.Exp, StatType.ExpSpent, StatType.HP,
                                                      StatType.Imm, StatType.Int, StatType.Level, StatType.MaxHP, StatType.MaxMP,
                                                      StatType.MP, StatType.Perc, StatType.Recov, StatType.Regen, StatType.Str,
                                                      StatType.Tact, StatType.WS
                                                  };

        static readonly StatType[] _raiseableStats = new StatType[]
                                                     {
                                                         StatType.Acc, StatType.Agi, StatType.Armor, StatType.Bra, StatType.Dex,
                                                         StatType.Evade, StatType.Imm, StatType.Int, StatType.Perc, StatType.Recov,
                                                         StatType.Regen, StatType.Str, StatType.Tact, StatType.WS
                                                     };

        /// <summary>
        /// Gets an IEnumerable of all the StatTypes used by Characters that are actually a mod of other stats.
        /// </summary>
        public static IEnumerable<StatType> ModStats
        {
            get { return _modStats; }
        }

        /// <summary>
        /// Gets an IEnumerable of all the STatTypes used by Characters that are not a mod stat.
        /// </summary>
        public static IEnumerable<StatType> NonModStats
        {
            get { return _nonModStats; }
        }

        /// <summary>
        /// Gets an IEnumerable of all the StatTypes used by Characters that can be raised by spending points.
        /// </summary>
        public static IEnumerable<StatType> RaiseableStats
        {
            get { return _raiseableStats; }
        }

        /// <summary>
        /// Gets the amount of points the Character has to spend on stats by finding
        /// the difference between the total exp and amount of exp spent.
        /// </summary>
        public int Points
        {
            get { return this[StatType.Exp] - this[StatType.ExpSpent]; }
        }

        /// <summary>
        /// Private static CharacterStatsBase constructor
        /// </summary>
        static CharacterStatsBase()
        {
#if DEBUG
            // Ensure a stat is either ModStat or NonModStat, not both
            var intersections = ModStats.Intersect(NonModStats);
            Debug.Assert(intersections.Count() == 0, "Intersection(s) found between ModStats and NonModStats.");

            // Ensure RaiseableStats are part of either ModStat or NonModStat
            var statsUnion = RaiseableStats.Union(NonModStats);
            var invalidRaiseables = RaiseableStats.Where(stat => !statsUnion.Contains(stat));
            Debug.Assert(invalidRaiseables.Count() == 0, "Found RaiseableStat that is not in either ModStats or NonModStats.");
#endif
        }

        protected abstract IStat CreateBaseStat<T>(StatType statType) where T : IStatValueType, new();

        protected abstract IStat CreateModStat<T>(StatType statType) where T : IStatValueType, new();

        protected void CreateStats()
        {
            Add(CreateBaseStat<StatValueByte>(StatType.Str));
            Add(CreateBaseStat<StatValueByte>(StatType.Agi));
            Add(CreateBaseStat<StatValueByte>(StatType.Dex));
            Add(CreateBaseStat<StatValueByte>(StatType.Int));
            Add(CreateBaseStat<StatValueByte>(StatType.Bra));

            Add(CreateModStat<StatValueUShort>(StatType.ModStr));
            Add(CreateModStat<StatValueUShort>(StatType.ModAgi));
            Add(CreateModStat<StatValueUShort>(StatType.ModDex));
            Add(CreateModStat<StatValueUShort>(StatType.ModInt));
            Add(CreateModStat<StatValueUShort>(StatType.ModBra));

            Add(CreateBaseStat<StatValueShort>(StatType.HP));
            Add(CreateBaseStat<StatValueShort>(StatType.MP));
            Add(CreateBaseStat<StatValueShort>(StatType.MaxHP));
            Add(CreateBaseStat<StatValueShort>(StatType.MaxMP));

            Add(CreateBaseStat<StatValueInt>(StatType.Cash));
            Add(CreateBaseStat<StatValueByte>(StatType.Level));
            Add(CreateBaseStat<StatValueInt>(StatType.ExpSpent));
            Add(CreateBaseStat<StatValueInt>(StatType.Exp));

            Add(CreateModStat<StatValueUShort>(StatType.Defence));
            Add(CreateModStat<StatValueUShort>(StatType.MinHit));
            Add(CreateModStat<StatValueUShort>(StatType.MaxHit));

            Add(CreateBaseStat<StatValueByte>(StatType.Acc));
            Add(CreateBaseStat<StatValueByte>(StatType.Armor));
            Add(CreateBaseStat<StatValueByte>(StatType.Evade));
            Add(CreateBaseStat<StatValueByte>(StatType.Imm));
            Add(CreateBaseStat<StatValueByte>(StatType.Perc));
            Add(CreateBaseStat<StatValueByte>(StatType.Recov));
            Add(CreateBaseStat<StatValueByte>(StatType.Regen));
            Add(CreateBaseStat<StatValueByte>(StatType.Tact));
            Add(CreateBaseStat<StatValueByte>(StatType.WS));

            Add(CreateModStat<StatValueUShort>(StatType.ModAcc));
            Add(CreateModStat<StatValueUShort>(StatType.ModArmor));
            Add(CreateModStat<StatValueUShort>(StatType.ModEvade));
            Add(CreateModStat<StatValueUShort>(StatType.ModImm));
            Add(CreateModStat<StatValueUShort>(StatType.ModPerc));
            Add(CreateModStat<StatValueUShort>(StatType.ModRecov));
            Add(CreateModStat<StatValueUShort>(StatType.ModRegen));
            Add(CreateModStat<StatValueUShort>(StatType.ModTact));
            Add(CreateModStat<StatValueUShort>(StatType.ModWS));
        }

        protected virtual int GetAccMod()
        {
            return GetSecondaryStatMod(StatType.Acc, StatType.ModDex, StatType.ModAgi);
        }

        protected virtual int GetAgiMod()
        {
            return GetPrimaryStatMod(StatType.Agi);
        }

        protected virtual int GetArmorMod()
        {
            return GetSecondaryStatMod(StatType.Armor, StatType.ModStr, StatType.ModAgi);
        }

        protected virtual int GetBraMod()
        {
            return GetPrimaryStatMod(StatType.Bra);
        }

        protected virtual int GetDefenceMod()
        {
            return (this[StatType.ModArmor] / 3) + GetEquipmentBonus(StatType.Defence);
        }

        protected virtual int GetDexMod()
        {
            return GetPrimaryStatMod(StatType.Dex);
        }

        protected virtual int GetEquipmentBonus(StatType statType)
        {
            return 0;
        }

        protected virtual int GetEvadeMod()
        {
            return GetSecondaryStatMod(StatType.Evade, StatType.ModAgi, StatType.ModDex);
        }

        protected virtual int GetImmMod()
        {
            return GetSecondaryStatMod(StatType.Imm, StatType.ModInt, StatType.ModStr);
        }

        protected virtual int GetIntMod()
        {
            return GetPrimaryStatMod(StatType.Int);
        }

        protected virtual int GetMaxHit()
        {
            return (this[StatType.ModStr] / 3) + GetEquipmentBonus(StatType.MaxHit);
        }

        protected virtual int GetMinHit()
        {
            return (this[StatType.ModStr] / 3) + GetEquipmentBonus(StatType.MinHit);
        }

        protected ModStatHandler GetModStatHandler(StatType statType)
        {
            switch (statType)
            {
                case StatType.ModAcc:
                    return GetAccMod;
                case StatType.ModArmor:
                    return GetArmorMod;
                case StatType.Defence:
                    return GetDefenceMod;
                case StatType.ModEvade:
                    return GetEvadeMod;
                case StatType.ModImm:
                    return GetImmMod;
                case StatType.MaxHit:
                    return GetMaxHit;
                case StatType.MinHit:
                    return GetMinHit;
                case StatType.ModPerc:
                    return GetPercMod;
                case StatType.ModRecov:
                    return GetRecovMod;
                case StatType.ModStr:
                    return GetStrMod;
                case StatType.ModAgi:
                    return GetAgiMod;
                case StatType.ModBra:
                    return GetBraMod;
                case StatType.ModDex:
                    return GetDexMod;
                case StatType.ModInt:
                    return GetIntMod;
                case StatType.ModRegen:
                    return GetRegenMod;
                case StatType.ModTact:
                    return GetTactMod;
                case StatType.ModWS:
                    return GetWSMod;
                default:
                    throw new ArgumentException(string.Format("No ModStatHandler found for StatType `{0}`.", statType));
            }
        }

        protected virtual int GetPercMod()
        {
            return GetSecondaryStatMod(StatType.Perc, StatType.ModInt, StatType.ModDex);
        }

        int GetPrimaryStatMod(StatType baseStat)
        {
            int r = this[baseStat];
            r += GetEquipmentBonus(baseStat);
            return r;
        }

        protected virtual int GetRecovMod()
        {
            return GetSecondaryStatMod(StatType.Recov, StatType.ModStr, StatType.ModAgi);
        }

        protected virtual int GetRegenMod()
        {
            return GetSecondaryStatMod(StatType.Regen, StatType.ModInt);
        }

        int GetSecondaryStatMod(StatType baseStat, StatType stat12, StatType stat3)
        {
            int r = this[baseStat];
            r += GetStatBonus(this[stat12], this[stat3]);
            r += GetEquipmentBonus(baseStat);
            return r;
        }

        int GetSecondaryStatMod(StatType baseStat, StatType stat123)
        {
            int r = this[baseStat];
            r += GetStatBonus(this[stat123]);
            r += GetEquipmentBonus(baseStat);
            return r;
        }

        int GetStatBonus(int stat12, int stat3)
        {
            return (stat12 * 2 + stat3 + this[StatType.ModBra]) / 6;
        }

        int GetStatBonus(int stat123)
        {
            return (stat123 * 3 + this[StatType.ModBra]) / 6;
        }

        protected virtual int GetStrMod()
        {
            return GetPrimaryStatMod(StatType.Str);
        }

        protected virtual int GetTactMod()
        {
            return GetSecondaryStatMod(StatType.Tact, StatType.ModAgi, StatType.ModDex);
        }

        protected virtual int GetWSMod()
        {
            return GetSecondaryStatMod(StatType.WS, StatType.ModStr, StatType.ModAgi);
        }
    }
}