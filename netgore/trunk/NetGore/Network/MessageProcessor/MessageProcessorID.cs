using System;
using System.Linq;

namespace NetGore.Network
{
    /// <summary>
    /// A value type that represents the unique ID of a message sent from the server to the client, or from the client to the
    /// server.
    /// </summary>
    public struct MessageProcessorID
    {
        readonly ushort _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessorID"/> struct.
        /// </summary>
        /// <param name="v">The value.</param>
        public MessageProcessorID(ushort v)
        {
            _value = v;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessorID"/> struct.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="v"/> is less than <see cref="ushort.MinValue"/>
        /// or greater than <see cref="ushort.MaxValue"/>.</exception>
        public MessageProcessorID(uint v)
        {
            if (v > ushort.MaxValue || v < ushort.MinValue)
                throw new ArgumentOutOfRangeException("v", "Value be between ushort.MinValue and ushort.MaxValue.");

            _value = (ushort)v;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="NetGore.Network.MessageProcessorID"/> to <see cref="System.UInt16"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ushort(MessageProcessorID v)
        {
            return v._value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="NetGore.Network.MessageProcessorID"/> to <see cref="System.UInt32"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator uint(MessageProcessorID v)
        {
            return v._value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.UInt16"/> to <see cref="NetGore.Network.MessageProcessorID"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator MessageProcessorID(ushort v)
        {
            return new MessageProcessorID(v);
        }
    }
}