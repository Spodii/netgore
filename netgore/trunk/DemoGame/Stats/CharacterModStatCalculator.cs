namespace DemoGame
{
    public static class CharacterModStatCalculator
    {
        public static int Calculate(IStatCollection baseStats, StatType statType, params IModStatContainer[] modders)
        {
            // FUTURE: Can optimize by adding a few overloads for 1, 2, 3, and 4 IModStatContainers, THEN a params for > 4
            int value = 0;

            value += baseStats[statType];

            foreach (IModStatContainer modder in modders)
            {
                value += modder.GetStatModBonus(statType);
            }

            return value;
        }

        /*
        static int GetAccMod()
        {
            return GetSecondaryStatMod(StatType.Acc, StatType.ModDex, StatType.ModAgi);
        }

        static int GetAgiMod()
        {
            return GetPrimaryStatMod(StatType.Agi);
        }

        static int GetArmorMod()
        {
            return GetSecondaryStatMod(StatType.Armor, StatType.ModStr, StatType.ModAgi);
        }

        static int GetBraMod()
        {
            return GetPrimaryStatMod(StatType.Bra);
        }

        static int GetDefenceMod()
        {
            return (this[StatType.ModArmor] / 3) + GetEquipmentBonus(StatType.Defence);
        }

        static int GetDexMod()
        {
            return GetPrimaryStatMod(StatType.Dex);
        }

        static int GetEquipmentBonus(StatType statType)
        {
            return 0;
        }

        static int GetEvadeMod()
        {
            return GetSecondaryStatMod(StatType.Evade, StatType.ModAgi, StatType.ModDex);
        }

        static int GetImmMod()
        {
            return GetSecondaryStatMod(StatType.Imm, StatType.ModInt, StatType.ModStr);
        }

        static int GetIntMod()
        {
            return GetPrimaryStatMod(StatType.Int);
        }

        static int GetMaxHit()
        {
            return (this[StatType.ModStr] / 3) + GetEquipmentBonus(StatType.MaxHit);
        }

        static int GetMinHit()
        {
            return (this[StatType.ModStr] / 3) + GetEquipmentBonus(StatType.MinHit);
        }

        static int GetPercMod()
        {
            return GetSecondaryStatMod(StatType.Perc, StatType.ModInt, StatType.ModDex);
        }

        static int GetPrimaryStatMod(StatType baseStat)
        {
            int r = this[baseStat];
            r += GetEquipmentBonus(baseStat);
            return r;
        }

        static int GetRecovMod()
        {
            return GetSecondaryStatMod(StatType.Recov, StatType.ModStr, StatType.ModAgi);
        }

        static int GetRegenMod()
        {
            return GetSecondaryStatMod(StatType.Regen, StatType.ModInt);
        }

        static int GetSecondaryStatMod(StatType baseStat, StatType stat12, StatType stat3)
        {
            int r = this[baseStat];
            r += GetStatBonus(this[stat12], this[stat3]);
            r += GetEquipmentBonus(baseStat);
            return r;
        }

        static int GetSecondaryStatMod(StatType baseStat, StatType stat123)
        {
            int r = this[baseStat];
            r += GetStatBonus(this[stat123]);
            r += GetEquipmentBonus(baseStat);
            return r;
        }

        static int GetStatBonus(int stat12, int stat3)
        {
            return (stat12 * 2 + stat3 + this[StatType.ModBra]) / 6;
        }

        static int GetStatBonus(int stat123)
        {
            return (stat123 * 3 + this[StatType.ModBra]) / 6;
        }

        static int GetStrMod()
        {
            return GetPrimaryStatMod(StatType.Str);
        }

        static int GetTactMod()
        {
            return GetSecondaryStatMod(StatType.Tact, StatType.ModAgi, StatType.ModDex);
        }

        static int GetWSMod()
        {
            return GetSecondaryStatMod(StatType.WS, StatType.ModStr, StatType.ModAgi);
        }
        */
    }
}