using System.Drawing;
using System.Linq;
using NetGore.IO;

namespace NetGore.Editor
{
    /// <summary>
    /// Extension methods for the <see cref="IValueWriter"/>.
    /// </summary>
    public static class IValueWriterExtensions
    {
        /// <summary>
        /// Writes a Point.
        /// </summary>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public static void Write(this IValueWriter writer, string name, Point value)
        {
            if (writer.SupportsNameLookup)
            {
                // We are using name lookup, so we have to combine the values so we use only one name
                var x = Parser.Invariant.ToString(value.X);
                var y = Parser.Invariant.ToString(value.Y);
                writer.Write(name, x + "," + y);
            }
            else
            {
                // Not using name lookup, so just write them out
                writer.Write(null, value.X);
                writer.Write(null, value.Y);
            }
        }

        /// <summary>
        /// Writes a Size.
        /// </summary>
        /// <param name="writer">IValueWriter to write to.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">Value to write.</param>
        public static void Write(this IValueWriter writer, string name, Size value)
        {
            if (writer.SupportsNameLookup)
            {
                // We are using name lookup, so we have to combine the values so we use only one name
                var x = Parser.Invariant.ToString(value.Width);
                var y = Parser.Invariant.ToString(value.Height);
                writer.Write(name, x + "," + y);
            }
            else
            {
                // Not using name lookup, so just write them out
                writer.Write(null, value.Width);
                writer.Write(null, value.Height);
            }
        }
    }
}