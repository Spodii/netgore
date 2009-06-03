using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore.IO;

namespace DemoGame
{
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
        /// Reads a StatType.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static StatType ReadStatType(this IValueReader reader, string name)
        {
            return NetGore.IValueReaderExtensions.ReadEnum<StatType>(reader, name);
        }
    }
}