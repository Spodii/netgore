using System.Linq;
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
            return NetGore.IValueReaderExtensions.ReadEnum<ItemType>(reader, name);
        }

        /// <summary>
        /// Reads a SkillType.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static SkillType ReadSkillType(this IValueReader reader, string name)
        {
            return NetGore.IValueReaderExtensions.ReadEnum<SkillType>(reader, name);
        }

        /// <summary>
        /// Reads a StatType.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static StatType ReadStatType(this IValueReader reader, string name)
        {
            return NetGore.IValueReaderExtensions.ReadEnum<StatType>(reader, name);
        }

        /// <summary>
        /// Reads a StatusEffectType.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static StatusEffectType ReadStatusEffectType(this IValueReader reader, string name)
        {
            return NetGore.IValueReaderExtensions.ReadEnum<StatusEffectType>(reader, name);
        }
    }
}