using System.Linq;
using NetGore;
using NetGore.IO;

namespace DemoGame
{
    /// <summary>
    /// Extension methods for the <see cref="IValueReader"/>.
    /// </summary>
    public static class IValueReaderExtensions
    {
        /// <summary>
        /// Reads an ItemType.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static ItemType ReadItemType(this IValueReader reader, string name)
        {
            return reader.ReadEnum<ItemType>(name);
        }

        public static EquipmentSlot ReadEquipmentSlot(this IValueReader reader, string name)
        {
            return reader.ReadEnum<EquipmentSlot>(name);
        }

        /// <summary>
        /// Reads a SkillType.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static SkillType ReadSkillType(this IValueReader reader, string name)
        {
            return reader.ReadEnum<SkillType>(name);
        }

        /// <summary>
        /// Reads a StatType.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static StatType ReadStatType(this IValueReader reader, string name)
        {
            return reader.ReadEnum<StatType>(name);
        }

        /// <summary>
        /// Reads a StatusEffectType.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static StatusEffectType ReadStatusEffectType(this IValueReader reader, string name)
        {
            return reader.ReadEnum<StatusEffectType>(name);
        }
    }
}