using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.IO;

namespace NetGore.Graphics
{
    /// <summary>
    /// Extensions for the IValueWriter.
    /// </summary>
    public static class IValueWriterExtensions
    {
        /// <summary>
        /// Writes a BackgroundLayerLayout.
        /// </summary>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public static void Write(this IValueWriter writer, string name, BackgroundLayerLayout value)
        {
            NetGore.IValueWriterExtensions.WriteEnum(writer, name, value);
        }
    }
}
