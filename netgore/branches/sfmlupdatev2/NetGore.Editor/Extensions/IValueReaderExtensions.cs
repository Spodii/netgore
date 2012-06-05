using System.Drawing;
using System.Linq;
using NetGore.IO;

namespace NetGore.Editor
{
    /// <summary>
    /// Extension methods for the <see cref="IValueReader"/>.
    /// </summary>
    public static class IValueReaderExtensions
    {
        /// <summary>
        /// Reads a Size.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static Size ReadSize(this IValueReader reader, string name)
        {
            if (reader.SupportsNameLookup)
            {
                var value = reader.ReadString(name);
                var split = value.Split(',');
                var x = Parser.Invariant.ParseInt(split[0]);
                var y = Parser.Invariant.ParseInt(split[1]);
                return new Size(x, y);
            }
            else
            {
                var x = reader.ReadInt(null);
                var y = reader.ReadInt(null);
                return new Size(x, y);
            }
        }

        /// <summary>
        /// Reads a Point.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static Point ReadSystemPoint(this IValueReader reader, string name)
        {
            if (reader.SupportsNameLookup)
            {
                var value = reader.ReadString(name);
                var split = value.Split(',');
                var x = Parser.Invariant.ParseInt(split[0]);
                var y = Parser.Invariant.ParseInt(split[1]);
                return new Point(x, y);
            }
            else
            {
                var x = reader.ReadInt(null);
                var y = reader.ReadInt(null);
                return new Point(x, y);
            }
        }
    }
}