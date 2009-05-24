using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NetGore.IO;

namespace NetGore
{
    /// <summary>
    /// Extensions for the IValueReader.
    /// </summary>
    public static class IValueReaderExtensions
    {
        /// <summary>
        /// Reads a Vector2.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static Vector2 ReadVector2(this IValueReader reader, string name)
        {
            if (reader.SupportsNameLookup)
            {
                string value = reader.ReadString(name);
                string[] split = value.Split(',');
                var x = float.Parse(split[0]);
                var y = float.Parse(split[1]);
                return new Vector2(x, y);
            }
            else
            {
                var x = reader.ReadFloat(null);
                var y = reader.ReadFloat(null);
                return new Vector2(x, y);
            }
        }

        /// <summary>
        /// Reads a CollisionType.
        /// </summary>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        public static CollisionType ReadCollisionType(this IValueReader reader, string name)
        {
            return ReadEnum<CollisionType>(reader, name);
        }

        /// <summary>
        /// Reads an enum from an IValueReader.
        /// </summary>
        /// <typeparam name="T">Type of the enum to read.</typeparam>
        /// <param name="reader">IValueReader to read from.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>Value read from the reader.</returns>
        static T ReadEnum<T>(IValueReader reader, string name)
        {
            Type type = typeof(T);
            var str = reader.ReadString(name);
            var value = (T)Enum.Parse(type, str);
            return value;
        }
    }
}
