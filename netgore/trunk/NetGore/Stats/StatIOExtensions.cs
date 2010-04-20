using System;
using System.Linq;
using NetGore.IO;

namespace NetGore.Stats
{
    /// <summary>
    /// IO extension methods for the <see cref="Stat{TStatType}"/> struct.
    /// </summary>
    public static class StatIOExtensions
    {
        /// <summary>
        /// Reads an <see cref="Stat{StatType}"/> from a <see cref="BitStream"/>.
        /// </summary>
        /// <typeparam name="TStatType">The type of the stat.</typeparam>
        /// <param name="bitStream"><see cref="BitStream"/> to read from.</param>
        /// <returns>The <see cref="Stat{TStatType}"/> read from the <paramref name="bitStream"/>.</returns>
        public static Stat<TStatType> ReadStat<TStatType>(this BitStream bitStream)
            where TStatType : struct, IComparable, IConvertible, IFormattable
        {
            var statType = bitStream.ReadEnum<TStatType>();
            var value = bitStream.ReadStatValueType();
            return new Stat<TStatType>(statType, value);
        }

        /// <summary>
        /// Reads an <see cref="Stat{StatType}"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <typeparam name="TStatType">The type of the stat.</typeparam>
        /// <param name="reader"><see cref="IValueReader"/> to read from.</param>
        /// <param name="name">The unique name of the value to read.</param>
        /// <returns>The <see cref="Stat{TStatType}"/> read from the <paramref name="reader"/>.</returns>
        public static Stat<TStatType> ReadStat<TStatType>(this IValueReader reader, string name)
            where TStatType : struct, IComparable, IConvertible, IFormattable
        {
            if (reader.SupportsNodes)
            {
                reader = reader.ReadNode(name);
                var statType = reader.ReadEnum<TStatType>("StatType");
                var value = reader.ReadStatValueType("Value");
                return new Stat<TStatType>(statType, value);
            }
            else
            {
                var statType = reader.ReadEnum<TStatType>(name + "_StatType");
                var value = reader.ReadStatValueType(name + "_Value");
                return new Stat<TStatType>(statType, value);
            }
        }

        /// <summary>
        /// Writes an <see cref="Stat{StatType}"/> to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <typeparam name="TStatType">The type of the stat.</typeparam>
        /// <param name="writer"><see cref="IValueWriter"/> to write to.</param>
        /// <param name="name">The unique name of the value to write.</param>
        /// <param name="stat">The <see cref="Stat{TStatType}"/> to write.</param>
        public static void Write<TStatType>(this IValueWriter writer, string name, Stat<TStatType> stat)
            where TStatType : struct, IComparable, IConvertible, IFormattable
        {
            if (writer.SupportsNodes)
            {
                writer.WriteStartNode(name);
                writer.WriteEnum("StatType", stat.StatType);
                writer.Write("Value", stat.Value);
                writer.WriteEndNode(name);
            }
            else
            {
                writer.WriteEnum(name + "_StatType", stat.StatType);
                writer.Write(name + "_Value", stat.Value);
            }
        }

        /// <summary>
        /// Writes an <see cref="Stat{StatType}"/> to a <see cref="BitStream"/>.
        /// </summary>
        /// <typeparam name="TStatType">The type of the stat.</typeparam>
        /// <param name="bitStream"><see cref="BitStream"/> to write to.</param>
        /// <param name="stat">The <see cref="Stat{TStatType}"/> to write.</param>
        public static void Write<TStatType>(this BitStream bitStream, Stat<TStatType> stat)
            where TStatType : struct, IComparable, IConvertible, IFormattable
        {
            bitStream.WriteEnum(stat.StatType);
            bitStream.Write(stat.Value);
        }
    }
}