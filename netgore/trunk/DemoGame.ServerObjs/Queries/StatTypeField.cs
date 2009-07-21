using System.Collections.Generic;

namespace DemoGame.Server.Queries
{
    public struct StatTypeField
    {
        public readonly string Field;
        public readonly StatType StatType;

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