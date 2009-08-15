using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;

namespace DemoGame.Server
{
    /// <summary>
    /// The base class for describing a single status effect that can be placed on a Character.
    /// </summary>
    public abstract class StatusEffectBase
    {
        static readonly StatType[] _allStatTypes = Enum.GetValues(typeof(StatType)).Cast<StatType>().ToArray();
        readonly StatType[] _modifiedStats;
        readonly StatusEffectType _statusEffectType;
        
        static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public StatusEffectType StatusEffectType { get { return _statusEffectType; } }

        public bool TryGetStatModifier(StatType statType, ushort power, out int value)
        {
            int? v = InternalTryGetStatModifier(statType, power);

            if (!v.HasValue)
            {
                value = 0;
                return false;
            }
            else
            {
                value = v.Value;
                return true;
            }
        }

        protected abstract int? InternalTryGetStatModifier(StatType statType, ushort power);

        protected StatusEffectBase(StatusEffectType statusEffectType)
        {
            _statusEffectType = statusEffectType;
            _modifiedStats = GetUsedStatTypes();

            AssertReturnValuesAreConsistent();
        }

        [Conditional("DEBUG")]
        void AssertReturnValuesAreConsistent()
        {
            Random r = new Random();

            // Perform 10 test iterations
            for (int i = 0; i < 10; i++)
            {
                // Grab a different, random power for each iteration
                int power = r.Next(0, 100);

                // Make the first iteration the lowest power, and the second iteration the highest power
                if (i == 0)
                    power = 0;
                else if (i == 1)
                    power = GameData.MaxStatusEffectPower;

                // Test each StatType that this StatusEffect actually modifies (in opposed to testing every single one)
                foreach (var statType in _modifiedStats)
                {
                    int a = GetStatModifier(statType, (ushort)power);
                    int b = GetStatModifier(statType, (ushort)power);

                    if (a != b)
                    {
                        const string errmsg = "StatusEffect `{0}`, handling StatusEffectType `{1}`, failed to return" +
                            " the same value for StatType `{2}` when using power `{3}`. The values were `{4}` and `{5}`." +
                            " It is vital that a StatusEffect always returns the same value for a given StatType and Power.";

                        string err = string.Format(errmsg, GetType(), StatusEffectType, statType, power, a, b);
                        if (log.IsFatalEnabled)
                            log.Fatal(err);
                        Debug.Fail(err);
                        throw new Exception(err);
                    }
                }
            }
        }

        StatType[] GetUsedStatTypes()
        {
            List<StatType> usedStatTypes = new List<StatType>();

            foreach (var statType in _allStatTypes)
            {
                int value;
                if (TryGetStatModifier(statType, 1, out value))
                    usedStatTypes.Add(statType);
            }

            return usedStatTypes.ToArray();
        }

        public int GetStatModifier(StatType statType, ushort power)
        {
            int value;
            if (!TryGetStatModifier(statType, power, out value))
                return 0;

            return value;
        }
    }
}
