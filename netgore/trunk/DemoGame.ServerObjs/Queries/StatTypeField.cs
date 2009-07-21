using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoGame.Server.Queries
{
    public struct StatTypeField 
    {
        public readonly StatType StatType;
        public readonly string Field;

        public StatTypeField(StatType statType, string field)
        {
            StatType = statType;
            Field = field;
        }

        public static implicit operator KeyValuePair<StatType, string>(StatTypeField v)
        {
            return new KeyValuePair<StatType, string>(v.StatType, v.Field);
        }

        public static implicit operator StatTypeField(KeyValuePair<StatType, string> v)
        {
            return new StatTypeField(v.Key, v.Value);
        }
    }
}
