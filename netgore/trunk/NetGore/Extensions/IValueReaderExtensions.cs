using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NetGore;
using NetGore.Globalization;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// Extensions for the IValueReader.
    /// </summary>
    public static class IValueReaderExtensions
    {
        public static SpriteCategory ReadSpriteCategory(this IValueReader reader, string name)
        {
            // TODO: ?? Try to remove as many usages of this as possible
            string s = reader.ReadString(name);
            return new SpriteCategory(s);
        }

        public static SpriteTitle ReadSpriteTitle(this IValueReader reader, string name)
        {
            // TODO: ?? Try to remove as many usages of this as possible
            string s = reader.ReadString(name);
            return new SpriteTitle(s);
        }

        public static SpriteCategorization ReadSpriteCategorization(this IValueReader reader, string name)
        {
            string s = reader.ReadString(name);
            return new SpriteCategorization(s);
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
                r = Parser.Invariant.ParseByte(split[0]);
                g = Parser.Invariant.ParseByte(split[1]);
                b = Parser.Invariant.ParseByte(split[2]);
                a = Parser.Invariant.ParseByte(split[3]);
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
        /// Reads a <see cref="Rectangle"/> from an IValueReader.
        /// </summary>
        /// <param name="reader">The IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the <paramref name="reader"/>.</returns>
        public static Rectangle ReadRectangle(this IValueReader reader, string name)
        {
            int x, y, w, h;

            if (reader.SupportsNameLookup)
            {
                string value = reader.ReadString(name);
                var split = value.Split(',');
                x = Parser.Invariant.ParseInt(split[0]);
                y = Parser.Invariant.ParseInt(split[1]);
                w = Parser.Invariant.ParseInt(split[2]);
                h = Parser.Invariant.ParseInt(split[3]);
            }
            else
            {
                x = reader.ReadInt(null);
                y = reader.ReadInt(null);
                w = reader.ReadInt(null);
                h = reader.ReadInt(null);
            }

            return new Rectangle(x, y, w, h);
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
                float x = Parser.Invariant.ParseFloat(split[0]);
                float y = Parser.Invariant.ParseFloat(split[1]);
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