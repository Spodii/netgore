using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// Extensions for the IValueWriter.
    /// </summary>
    public static class IValueWriterExtensions
    {
        /// <summary>
        /// Writes an unsigned integer with the specified range to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="value">Value to write.</param>
        /// <param name="name">Name of the value to write.</param>
        /// <param name="minValue">Minimum (inclusive) value that the written value can be.</param>
        /// <param name="maxValue">Maximum (inclusive) value that the written value can be.</param>
        public static void Write(this IValueWriter writer, string name, uint value, uint minValue, uint maxValue)
        {
            if (value < minValue || value > maxValue)
                throw new ArgumentOutOfRangeException("value", "Value parameter must be between minValue and maxValue.");
            if (maxValue < minValue)
                throw new ArgumentOutOfRangeException("maxValue", "MaxValue must be greater than or equal to MinValue.");

            // Find the number of bits required for the range of desired values
            uint maxWriteValue = maxValue - minValue;
            int bitsRequired = BitOps.RequiredBits(maxWriteValue);

            // Subtract the minimum value from the value since we want to write how high above the minimum value
            // the value is, not the actual value
            uint offsetFromMin = value - minValue;
            writer.Write(name, offsetFromMin, bitsRequired);

            Debug.Assert((value - minValue) <= maxWriteValue);
            Debug.Assert((1 << bitsRequired) >= maxWriteValue);
        }

        /// <summary>
        /// Writes a signed integer with the specified range to an IValueWriter.
        /// </summary>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="value">Value to write.</param>
        /// <param name="name">Name of the value to write.</param>
        /// <param name="minValue">Minimum (inclusive) value that the written value can be.</param>
        /// <param name="maxValue">Maximum (inclusive) value that the written value can be.</param>
        public static void Write(this IValueWriter writer, string name, int value, int minValue, int maxValue)
        {
            if (value < minValue || value > maxValue)
                throw new ArgumentOutOfRangeException("value", "Value parameter must be between minValue and maxValue.");
            if (maxValue < minValue)
                throw new ArgumentOutOfRangeException("maxValue", "MaxValue must be greater than or equal to MinValue.");

            // Find the number of bits required for the range of desired values
            uint maxWriteValue = (uint)(maxValue - minValue);
            int bitsRequired = BitOps.RequiredBits(maxWriteValue);

            // Subtract the minimum value from the value since we want to write how high above the minimum value
            // the value is, not the actual value
            uint offsetFromMin = (uint)(value - minValue);
            writer.Write(name, offsetFromMin, bitsRequired);

            Debug.Assert((value - minValue) <= maxWriteValue);
            Debug.Assert((1 << bitsRequired) >= maxWriteValue);
        }

        /// <summary>
        /// Writes a Vector2.
        /// </summary>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public static void Write(this IValueWriter writer, string name, Vector2 value)
        {
            if (writer.SupportsNameLookup)
            {
                // We are using name lookup, so we have to combine the values so we use only one name
                writer.Write(name, value.X + "," + value.Y);
            }
            else
            {
                // Not using name lookup, so just write them out
                writer.Write(null, value.X);
                writer.Write(null, value.Y);
            }
        }

        /// <summary>
        /// Writes a CollisionType.
        /// </summary>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public static void Write(this IValueWriter writer, string name, CollisionType value)
        {
            WriteEnum(writer, name, value);
        }

        /// <summary>
        /// Write an enum to the IValueWriter.
        /// </summary>
        /// <typeparam name="T">Type of the enum to write.</typeparam>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        static void WriteEnum<T>(IValueWriter writer, string name, T value)
        {
            writer.Write(name, value.ToString());
        }
    }
}