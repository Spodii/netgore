using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// Extensions for the IValueReader.
    /// </summary>
    public static class IValueReaderExtensions
    {
        /// <summary>
        /// Reads an Alignment.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static Alignment ReadAlignment(this IValueReader reader, string name)
        {
            return ReadEnum<Alignment>(reader, name);
        }

        /// <summary>
        /// Reads a CollisionType.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static CollisionType ReadCollisionType(this IValueReader reader, string name)
        {
            return ReadEnum<CollisionType>(reader, name);
        }

        /// <summary>
        /// Reads a Color.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static Color ReadColor(this IValueReader reader, string name)
        {
            byte r, g, b, a;

            if (reader.SupportsNameLookup)
            {
                string value = reader.ReadString(name);
                var split = value.Split(',');
                r = byte.Parse(split[0]);
                g = byte.Parse(split[1]);
                b = byte.Parse(split[2]);
                a = byte.Parse(split[3]);
            }
            else
            {
                r = reader.ReadByte(null);
                g = reader.ReadByte(null);
                b = reader.ReadByte(null);
                a = reader.ReadByte(null);
            }

            return new Color(r, g, b, a);
        }

        /// <summary>
        /// Reads an enum from an IValueReader.
        /// </summary>
        /// <typeparam name="T">Type of the enum to read.</typeparam>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static T ReadEnum<T>(IValueReader reader, string name)
        {
            Type type = typeof(T);
            string str = reader.ReadString(name);
            T value = (T)Enum.Parse(type, str);
            return value;
        }

        /// <summary>
        /// Reads an unsigned integer with the specified range from an IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the value from.</param>
        /// <param name="name">Name of the value to read.</param>
        /// <param name="minValue">Minimum (inclusive) value that the read value can be.</param>
        /// <param name="maxValue">Maximum (inclusive) value that the read value can be.</param>
        /// <returns>Value read from the IValueReader.</returns>
        public static uint ReadUInt(this IValueReader reader, string name, uint minValue, uint maxValue)
        {
            if (maxValue < minValue)
                throw new ArgumentOutOfRangeException("maxValue", "MaxValue must be greater than or equal to MinValue.");

            // Find the number of bits required for the range of desired values
            uint maxWriteValue = maxValue - minValue;
            int bitsRequired = BitOps.RequiredBits(maxWriteValue);

            // Read the value, which is the offset from the minimum possible value, then add it to the minimum
            // possible value
            uint offsetFromMin = reader.ReadUInt(name, bitsRequired);
            return minValue + offsetFromMin;
        }

        /// <summary>
        /// Reads a signed integer with the specified range from an IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the value from.</param>
        /// <param name="name">Name of the value to read.</param>
        /// <param name="minValue">Minimum (inclusive) value that the read value can be.</param>
        /// <param name="maxValue">Maximum (inclusive) value that the read value can be.</param>
        /// <returns>Value read from the IValueReader.</returns>
        public static int ReadUInt(this IValueReader reader, string name, int minValue, int maxValue)
        {
            if (maxValue < minValue)
                throw new ArgumentOutOfRangeException("maxValue", "MaxValue must be greater than or equal to MinValue.");

            // Find the number of bits required for the range of desired values
            uint maxWriteValue = (uint)(maxValue - minValue);
            int bitsRequired = BitOps.RequiredBits(maxWriteValue);

            // Read the value, which is the offset from the minimum possible value, then add it to the minimum
            // possible value
            uint offsetFromMin = reader.ReadUInt(name, bitsRequired);
            return minValue + (int)offsetFromMin;
        }

        /// <summary>
        /// Reads a Vector2.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static Vector2 ReadVector2(this IValueReader reader, string name)
        {
            if (reader.SupportsNameLookup)
            {
                string value = reader.ReadString(name);
                var split = value.Split(',');
                float x = float.Parse(split[0]);
                float y = float.Parse(split[1]);
                return new Vector2(x, y);
            }
            else
            {
                float x = reader.ReadFloat(null);
                float y = reader.ReadFloat(null);
                return new Vector2(x, y);
            }
        }
    }
}