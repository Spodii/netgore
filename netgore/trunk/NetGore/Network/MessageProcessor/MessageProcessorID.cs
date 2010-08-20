using System;

namespace NetGore.Network
{
    public struct MessageProcessorID
    {
        readonly ushort _value;

        public MessageProcessorID(ushort v)
        {
            _value = v;
        }

        public MessageProcessorID(uint v)
        {
            if (v > ushort.MaxValue || v < ushort.MinValue)
                throw new ArgumentOutOfRangeException("v", "Value be between ushort.MinValue and ushort.MaxValue.");

            _value = (ushort)v;
        }

        public static implicit operator ushort(MessageProcessorID v)
        {
            return v._value;
        }

        public static implicit operator uint(MessageProcessorID v)
        {
            return v._value;
        }

        public static implicit operator MessageProcessorID(ushort v)
        {
            return new MessageProcessorID(v);
        }
    }
}