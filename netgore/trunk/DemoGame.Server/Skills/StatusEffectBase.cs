using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
