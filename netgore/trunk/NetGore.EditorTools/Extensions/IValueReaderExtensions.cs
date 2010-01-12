using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using NetGore.IO;

namespace NetGore.EditorTools
{
    public static class IValueReaderExtensions
    {
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
                string value = reader.ReadString(name);
                var split = value.Split(',');
                int x = Parser.Invariant.ParseInt(split[0]);
                int y = Parser.Invariant.ParseInt(split[1]);
                return new Point(x, y);
            }
            else
            {
                int x = reader.ReadInt(null);
                int y = reader.ReadInt(null);
                return new Point(x, y);
            }
        }

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
                string value = reader.ReadString(name);
                var split = value.Split(',');
                int x = Parser.Invariant.ParseInt(split[0]);
                int y = Parser.Invariant.ParseInt(split[1]);
                return new Size(x, y);
            }
            else
            {
                int x = reader.ReadInt(null);
                int y = reader.ReadInt(null);
                return new Size(x, y);
            }
        }
    }
}
