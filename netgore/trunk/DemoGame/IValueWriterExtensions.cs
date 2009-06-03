using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NetGore.IO;

namespace DemoGame
{
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
            NetGore.IValueWriterExtensions.WriteEnum(writer, name, value);
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
            NetGore.IValueWriterExtensions.WriteEnum(writer, name, value);
        }
    }
}