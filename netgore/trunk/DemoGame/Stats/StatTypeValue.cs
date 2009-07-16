using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame
{
    public struct StatTypeValue
    {
        public readonly StatType StatType;
        public readonly int Value;

        public StatTypeValue(StatType statType, int value)
        {
            StatType = statType;
            Value = value;
        }

        public static implicit operator KeyValuePair<StatType, int>(StatTypeValue v)
        {
            return new KeyValuePair<StatType, int>(v.StatType, v.Value);
        }

        public static implicit operator StatTypeValue(KeyValuePair<StatType, int> v)
        {
            return new StatTypeValue(v.Key, v.Value);
        }
    }
}