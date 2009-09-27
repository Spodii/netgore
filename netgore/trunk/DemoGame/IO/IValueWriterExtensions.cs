using System.Linq;
using NetGore;
using NetGore.IO;

namespace DemoGame
{
    /// <summary>
    /// Extension methods for the <see cref="IValueWriter"/>.
    /// </summary>
    public static class IValueWriterExtensions
    {
        /// <summary>
        /// Writes an ItemType.
        /// </summary>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public static void Write(this IValueWriter writer, string name, ItemType value)
        {
            writer.WriteEnum(name, value);
        }

        public static void Write(this IValueWriter writer, string name, EquipmentSlot value)
        {
            writer.WriteEnum(name, value);
        }

        /// <summary>
        /// Writes a StatType.
        /// </summary>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public static void Write(this IValueWriter writer, string name, StatType value)
        {
            writer.WriteEnum(name, value);
        }

        /// <summary>
        /// Writes a StatusEffectType.
        /// </summary>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public static void Write(this IValueWriter writer, string name, StatusEffectType value)
        {
            writer.WriteEnum(name, value);
        }

        /// <summary>
        /// Writes a SkillType.
        /// </summary>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public static void Write(this IValueWriter writer, string name, SkillType value)
        {
            writer.WriteEnum(name, value);
        }
    }
}