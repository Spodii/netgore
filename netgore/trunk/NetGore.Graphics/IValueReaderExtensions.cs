using System.Linq;
using NetGore.IO;

namespace NetGore.Graphics
{
    /// <summary>
    /// Extensions for the IValueReader.
    /// </summary>
    public static class IValueReaderExtensions
    {
        /// <summary>
        /// Reads a BackgroundLayerLayout.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static BackgroundLayerLayout ReadBackgroundLayerLayout(this IValueReader reader, string name)
        {
            return NetGore.IValueReaderExtensions.ReadEnum<BackgroundLayerLayout>(reader, name);
        }
    }
}