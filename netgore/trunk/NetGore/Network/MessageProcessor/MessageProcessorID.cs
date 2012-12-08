using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NetGore.IO;

namespace NetGore.Network
{
    /// <summary>
    /// A value type that represents the unique ID of a message sent from the server to the client, or from the client to the
    /// server.
    /// </summary>
    public struct MessageProcessorID : IEquatable<MessageProcessorID>
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
        public static explicit operator ushort(MessageProcessorID v)
        {
            return v._value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="NetGore.Network.MessageProcessorID"/> to <see cref="System.UInt32"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator uint(MessageProcessorID v)
        {
            return v._value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.UInt16"/> to <see cref="NetGore.Network.MessageProcessorID"/>.
        /// </summary>
        /// <param name="v">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator MessageProcessorID(ushort v)
        {
            return new MessageProcessorID(v);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(MessageProcessorID other)
        {
            return other._value == _value;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is MessageProcessorID && this == (MessageProcessorID)obj;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(MessageProcessorID left, MessageProcessorID right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(MessageProcessorID left, MessageProcessorID right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Gets the raw internal value of this MessageProcessorID.
        /// </summary>
        /// <returns>The raw internal value.</returns>
        public ushort GetRawValue()
        {
            return _value;
        }

        /// <summary>
        /// Writes to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="name">Unique name that will be used to distinguish it from other values when reading.</param>
        public void Write(IValueWriter writer, string name)
        {
            writer.Write(name, _value);
        }

        /// <summary>
        /// Writes to an IValueWriter.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        public void Write(BitStream bitStream)
        {
            bitStream.Write(_value);
        }

        /// <summary>
        /// Reads from an IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>The value read from the IValueReader.</returns>
        public static MessageProcessorID Read(IValueReader reader, string name)
        {
            var value = reader.ReadUShort(name);
            return new MessageProcessorID(value);
        }

        /// <summary>
        /// Reads from an <see cref="IDataRecord"/>.
        /// </summary>
        /// <param name="reader"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The value read from the <see cref="IDataRecord"/>.</returns>
        public static MessageProcessorID Read(IDataRecord reader, int i)
        {
            var value = reader.GetValue(i);
            if (value is ushort)
                return new MessageProcessorID((ushort)value);

            var convertedValue = Convert.ToUInt16(value);
            return new MessageProcessorID(convertedValue);
        }

        /// <summary>
        /// Reads from an <see cref="IDataRecord"/>.
        /// </summary>
        /// <param name="reader"><see cref="IDataRecord"/> to get the value from.</param>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The value read from the <see cref="IDataRecord"/>.</returns>
        public static MessageProcessorID Read(IDataRecord reader, string name)
        {
            return Read(reader, reader.GetOrdinal(name));
        }

        /// <summary>
        /// Reads from an IValueReader.
        /// </summary>
        /// <param name="bitStream">BitStream to read from.</param>
        /// <returns>The value read from the BitStream.</returns>
        public static MessageProcessorID Read(BitStream bitStream)
        {
            var value = bitStream.ReadUShort();
            return new MessageProcessorID(value);
        }
    }

    /// <summary>
    /// Adds extensions to some data I/O objects for performing Read and Write operations for the MessageProcessorID.
    /// All of the operations are implemented in the MessageProcessorID struct. These extensions are provided
    /// purely for the convenience of accessing all the I/O operations from the same place.
    /// </summary>
    public static class MessageProcessorIDReadWriteExtensions
    {
        /// <summary>
        /// Gets the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type MessageProcessorID.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as a MessageProcessorID.</returns>
        public static MessageProcessorID AsMessageProcessorID<T>(this IDictionary<T, string> dict, T key)
        {
            return Parser.Invariant.ParseMessageProcessorID(dict[key]);
        }

        /// <summary>
        /// Tries to get the value in the <paramref name="dict"/> entry at the given <paramref name="key"/> as type MessageProcessorID.
        /// </summary>
        /// <typeparam name="T">The key Type.</typeparam>
        /// <param name="dict">The IDictionary.</param>
        /// <param name="key">The key for the value to get.</param>
        /// <param name="defaultValue">The value to use if the value at the <paramref name="key"/> could not be parsed.</param>
        /// <returns>The value at the given <paramref name="key"/> parsed as an int, or the
        /// <paramref name="defaultValue"/> if the <paramref name="key"/> did not exist in the <paramref name="dict"/>
        /// or the value at the given <paramref name="key"/> could not be parsed.</returns>
        public static MessageProcessorID AsMessageProcessorID<T>(this IDictionary<T, string> dict, T key, MessageProcessorID defaultValue)
        {
            string value;
            if (!dict.TryGetValue(key, out value))
                return defaultValue;

            MessageProcessorID parsed;
            if (!Parser.Invariant.TryParse(value, out parsed))
                return defaultValue;

            return parsed;
        }

        /// <summary>
        /// Reads the MessageProcessorID from an <see cref="IDataRecord"/>.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to read the MessageProcessorID from.</param>
        /// <param name="i">The field index to read.</param>
        /// <returns>The MessageProcessorID read from the <see cref="IDataRecord"/>.</returns>
        public static MessageProcessorID GetMessageProcessorID(this IDataRecord r, int i)
        {
            return MessageProcessorID.Read(r, i);
        }

        /// <summary>
        /// Reads the MessageProcessorID from an <see cref="IDataRecord"/>.
        /// </summary>
        /// <param name="r"><see cref="IDataRecord"/> to read the MessageProcessorID from.</param>
        /// <param name="name">The name of the field to read the value from.</param>
        /// <returns>The MessageProcessorID read from the <see cref="IDataRecord"/>.</returns>
        public static MessageProcessorID GetMessageProcessorID(this IDataRecord r, string name)
        {
            return MessageProcessorID.Read(r, name);
        }

        /// <summary>
        /// Parses the MessageProcessorID from a string.
        /// </summary>
        /// <param name="parser">The Parser to use.</param>
        /// <param name="value">The string to parse.</param>
        /// <returns>The MessageProcessorID parsed from the string.</returns>
        public static MessageProcessorID ParseMessageProcessorID(this Parser parser, string value)
        {
            return new MessageProcessorID(parser.ParseUShort(value));
        }

        /// <summary>
        /// Reads the MessageProcessorID from a BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to read the MessageProcessorID from.</param>
        /// <returns>The MessageProcessorID read from the BitStream.</returns>
        public static MessageProcessorID ReadMessageProcessorID(this BitStream bitStream)
        {
            return MessageProcessorID.Read(bitStream);
        }

        /// <summary>
        /// Reads the MessageProcessorID from an IValueReader.
        /// </summary>
        /// <param name="valueReader">IValueReader to read the MessageProcessorID from.</param>
        /// <param name="name">The unique name of the value to read.</param>
        /// <returns>The MessageProcessorID read from the IValueReader.</returns>
        public static MessageProcessorID ReadMessageProcessorID(this IValueReader valueReader, string name)
        {
            return MessageProcessorID.Read(valueReader, name);
        }

        /// <summary>
        /// Tries to parse the MessageProcessorID from a string.
        /// </summary>
        /// <param name="parser">The Parser to use.</param>
        /// <param name="value">The string to parse.</param>
        /// <param name="outValue">If this method returns true, contains the parsed MessageProcessorID.</param>
        /// <returns>True if the parsing was successfully; otherwise false.</returns>
        public static bool TryParse(this Parser parser, string value, out MessageProcessorID outValue)
        {
            ushort tmp;
            var ret = parser.TryParse(value, out tmp);
            outValue = new MessageProcessorID(tmp);
            return ret;
        }

        /// <summary>
        /// Writes a MessageProcessorID to a BitStream.
        /// </summary>
        /// <param name="bitStream">BitStream to write to.</param>
        /// <param name="value">MessageProcessorID to write.</param>
        public static void Write(this BitStream bitStream, MessageProcessorID value)
        {
            value.Write(bitStream);
        }

        /// <summary>
        /// Writes a MessageProcessorID to a IValueWriter.
        /// </summary>
        /// <param name="valueWriter">IValueWriter to write to.</param>
        /// <param name="name">Unique name of the MessageProcessorID that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">MessageProcessorID to write.</param>
        public static void Write(this IValueWriter valueWriter, string name, MessageProcessorID value)
        {
            value.Write(valueWriter, name);
        }
    }
}