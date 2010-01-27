using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetGore.Features.Guilds
{
    public struct GuildRank
    {
        readonly byte _value;

        public GuildRank(byte value)
        {
            _value = value;
        }

        public override bool Equals(object obj)
        {
            return _value.Equals(obj);
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public static implicit operator byte(GuildRank value)
        {
            return value._value;
        }

        public static implicit operator GuildRank(byte value)
        {
            return new GuildRank(value);
        }

        public bool Equals(GuildRank other)
        {
            return other._value == _value;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}
