using System;
using System.Linq;
using NetGore.Content;
using NetGore.IO;
using SFML.Graphics;

namespace NetGore
{
    /// <summary>
    /// Extensions for the <see cref="IValueReader"/>.
    /// </summary>
    public static class IValueReaderExtensions
    {
        public static BlendMode ReadBlendMode(IValueReader reader, string name)
        {
            return reader.ReadEnum<BlendMode>(name);
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
                var value = reader.ReadString(name);
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

        public static ContentAssetName ReadContentAssetName(this IValueReader reader, string name)
        {
            var s = reader.ReadString(name);
            return new ContentAssetName(s);
        }

        /// <summary>
        /// Reads a Point.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static Point ReadPoint(this IValueReader reader, string name)
        {
            if (reader.SupportsNameLookup)
            {
                var value = reader.ReadString(name);
                var split = value.Split(',');
                var x = Parser.Invariant.ParseInt(split[0]);
                var y = Parser.Invariant.ParseInt(split[1]);
                return new Point(x, y);
            }
            else
            {
                var x = reader.ReadInt(null);
                var y = reader.ReadInt(null);
                return new Point(x, y);
            }
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
                var value = reader.ReadString(name);
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

        public static SpriteCategorization ReadSpriteCategorization(this IValueReader reader, string name)
        {
            var s = reader.ReadString(name);
            return new SpriteCategorization(s);
        }

        public static SpriteCategory ReadSpriteCategory(this IValueReader reader, string name)
        {
            var s = reader.ReadString(name);
            return new SpriteCategory(s);
        }

        public static SpriteTitle ReadSpriteTitle(this IValueReader reader, string name)
        {
            var s = reader.ReadString(name);
            return new SpriteTitle(s);
        }

        public static TextureAssetName ReadTextureAssetName(this IValueReader reader, string name)
        {
            var s = reader.ReadString(name);
            return new TextureAssetName(s);
        }

        /// <summary>
        /// Reads an unsigned integer with the specified range from an IValueReader.
        /// </summary>
        /// <param name="reader">IValueReader to read the value from.</param>
        /// <param name="name">Name of the value to read.</param>
        /// <param name="minValue">Minimum (inclusive) value that the read value can be.</param>
        /// <param name="maxValue">Maximum (inclusive) value that the read value can be.</param>
        /// <returns>Value read from the IValueReader.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><c>maxValue</c> is out of range.</exception>
        public static uint ReadUInt(this IValueReader reader, string name, uint minValue, uint maxValue)
        {
            if (maxValue < minValue)
                throw new ArgumentOutOfRangeException("maxValue", "MaxValue must be greater than or equal to MinValue.");

            // Find the number of bits required for the range of desired values
            var maxWriteValue = maxValue - minValue;
            var bitsRequired = BitOps.RequiredBits(maxWriteValue);

            // Read the value, which is the offset from the minimum possible value, then add it to the minimum
            // possible value
            var offsetFromMin = reader.ReadUInt(name, bitsRequired);
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
        /// <exception cref="ArgumentOutOfRangeException"><c>maxValue</c> is out of range.</exception>
        public static int ReadUInt(this IValueReader reader, string name, int minValue, int maxValue)
        {
            if (maxValue < minValue)
                throw new ArgumentOutOfRangeException("maxValue", "MaxValue must be greater than or equal to MinValue.");

            // Find the number of bits required for the range of desired values
            var maxWriteValue = (uint)(maxValue - minValue);
            var bitsRequired = BitOps.RequiredBits(maxWriteValue);

            // Read the value, which is the offset from the minimum possible value, then add it to the minimum
            // possible value
            var offsetFromMin = reader.ReadUInt(name, bitsRequired);
            return minValue + (int)offsetFromMin;
        }

        /// <summary>
        /// Reads a <see cref="VariableByte"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static VariableByte ReadVariableByte(this IValueReader reader, string name)
        {
            return
                (VariableByte)ReadVariableValue(reader, name, (r, n) => r.ReadByte(n), (min, max) => new VariableByte(min, max));
        }

        /// <summary>
        /// Reads a <see cref="VariableColor"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static VariableColor ReadVariableColor(this IValueReader reader, string name)
        {
            return
                (VariableColor)
                ReadVariableValue(reader, name, (r, n) => r.ReadColor(n), (min, max) => new VariableColor(min, max));
        }

        /// <summary>
        /// Reads a <see cref="VariableFloat"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static VariableFloat ReadVariableFloat(this IValueReader reader, string name)
        {
            return
                (VariableFloat)
                ReadVariableValue(reader, name, (r, n) => r.ReadFloat(n), (min, max) => new VariableFloat(min, max));
        }

        /// <summary>
        /// Reads a <see cref="VariableInt"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static VariableInt ReadVariableInt(this IValueReader reader, string name)
        {
            return (VariableInt)ReadVariableValue(reader, name, (r, n) => r.ReadInt(n), (min, max) => new VariableInt(min, max));
        }

        /// <summary>
        /// Reads a <see cref="VariableSByte"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static VariableSByte ReadVariableSByte(this IValueReader reader, string name)
        {
            return
                (VariableSByte)
                ReadVariableValue(reader, name, (r, n) => r.ReadSByte(n), (min, max) => new VariableSByte(min, max));
        }

        /// <summary>
        /// Reads a <see cref="VariableShort"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static VariableShort ReadVariableShort(this IValueReader reader, string name)
        {
            return
                (VariableShort)
                ReadVariableValue(reader, name, (r, n) => r.ReadShort(n), (min, max) => new VariableShort(min, max));
        }

        /// <summary>
        /// Reads a <see cref="VariableUShort"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static VariableUShort ReadVariableUShort(this IValueReader reader, string name)
        {
            return
                (VariableUShort)
                ReadVariableValue(reader, name, (r, n) => r.ReadUShort(n), (min, max) => new VariableUShort(min, max));
        }

        /// <summary>
        /// Helps read a variable value.
        /// </summary>
        /// <typeparam name="T">The internal type of the variable value.</typeparam>
        /// <param name="reader">The IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <param name="readValue">Delegate used to read a value of type <typeparamref name="T"/>.</param>
        /// <param name="createValue">Delegate used to create the needed variable value.</param>
        /// <returns>Value read from the <paramref name="reader"/>.</returns>
        static IVariableValue<T> ReadVariableValue<T>(this IValueReader reader, string name,
                                                      Func<IValueReader, string, T> readValue,
                                                      Func<T, T, IVariableValue<T>> createValue)
        {
            T min;
            T max;

            if (reader.SupportsNameLookup && reader.SupportsNodes)
            {
                var node = reader.ReadNode(name);
                min = readValue(node, "Min");
                max = readValue(node, "Max");
            }
            else
            {
                min = readValue(reader, "Min");
                max = readValue(reader, "Max");
            }

            return createValue(min, max);
        }

        /// <summary>
        /// Reads a <see cref="Vector2"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the <paramref name="reader"/>.</returns>
        public static Vector2 ReadVector2(this IValueReader reader, string name)
        {
            if (reader.SupportsNameLookup)
            {
                var value = reader.ReadString(name);
                var split = value.Split(',');
                var x = Parser.Invariant.ParseFloat(split[0]);
                var y = Parser.Invariant.ParseFloat(split[1]);
                return new Vector2(x, y);
            }
            else
            {
                var x = reader.ReadFloat(null);
                var y = reader.ReadFloat(null);
                return new Vector2(x, y);
            }
        }

        /// <summary>
        /// Reads a <see cref="Vector3"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the <paramref name="reader"/>.</returns>
        public static Vector3 ReadVector3(this IValueReader reader, string name)
        {
            if (reader.SupportsNameLookup)
            {
                var value = reader.ReadString(name);
                var split = value.Split(',');
                var x = Parser.Invariant.ParseFloat(split[0]);
                var y = Parser.Invariant.ParseFloat(split[1]);
                var z = Parser.Invariant.ParseFloat(split[2]);
                return new Vector3(x, y, z);
            }
            else
            {
                var x = reader.ReadFloat(null);
                var y = reader.ReadFloat(null);
                var z = reader.ReadFloat(null);
                return new Vector3(x, y, z);
            }
        }

        /// <summary>
        /// Reads a <see cref="Vector4"/>.
        /// </summary>
        /// <param name="reader"><see cref="IValueReader"/> to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the <paramref name="reader"/>.</returns>
        public static Vector4 ReadVector4(this IValueReader reader, string name)
        {
            if (reader.SupportsNameLookup)
            {
                var value = reader.ReadString(name);
                var split = value.Split(',');
                var x = Parser.Invariant.ParseFloat(split[0]);
                var y = Parser.Invariant.ParseFloat(split[1]);
                var z = Parser.Invariant.ParseFloat(split[2]);
                var w = Parser.Invariant.ParseFloat(split[3]);
                return new Vector4(x, y, z, w);
            }
            else
            {
                var x = reader.ReadFloat(null);
                var y = reader.ReadFloat(null);
                var z = reader.ReadFloat(null);
                var w = reader.ReadFloat(null);
                return new Vector4(x, y, z, w);
            }
        }
    }
}