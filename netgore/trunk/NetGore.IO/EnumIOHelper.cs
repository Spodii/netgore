using System;
using System.Linq;
using NetGore;

namespace NetGore.IO
{
    /// <summary>
    /// Provides helper methods for reading and writing Enums.
    /// </summary>
    public static class EnumIOHelper
    {
        /// <summary>
        /// Gets the Enum of the given type from its name. 
        /// </summary>
        /// <typeparam name="T">The Type of <see cref="Enum"/>.</typeparam>
        /// <param name="value">The name of the <see cref="Enum"/> value.</param>
        /// <returns>The parsed enum.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><typeparamref name="T"/> is not an <see cref="Enum"/> -or-
        /// <paramref name="value"/> is an valid enum name -or-
        /// <paramref name="value"/> is a name, but not one of the named constants defined for the enumeration.</exception>
        public static T FromName<T>(string value) where T : struct, IComparable, IConvertible, IFormattable
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        /// <summary>
        /// Reads an <see cref="Enum"/> from an <see cref="IValueReader"/>.
        /// </summary>
        /// <typeparam name="T">The Type of <see cref="Enum"/>.</typeparam>
        /// <param name="reader">The <see cref="IValueReader"/> to read the value from.</param>
        /// <param name="enumReader">The enum value reader.</param>
        /// <param name="name">Unique name of the value to read.</param>
        /// <returns>The read and parsed enum.</returns>
        public static T ReadEnum<T>(IValueReader reader, IEnumValueReader<T> enumReader, string name)
            where T : struct, IComparable, IConvertible, IFormattable
        {
            if (reader.UseEnumNames)
                return reader.ReadEnumName<T>(name);
            else
                return reader.ReadEnumValue(enumReader, name);
        }

        /// <summary>
        /// Gets the name of the <see cref="Enum"/>.
        /// </summary>
        /// <typeparam name="T">The Type of <see cref="Enum"/>.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>The string name of the <paramref name="value"/>.</returns>
        public static string ToName<T>(T value) where T : struct, IComparable, IConvertible, IFormattable
        {
            return value.ToString();
        }

        /// <summary>
        /// Writes an <see cref="Enum"/> to an <see cref="IValueWriter"/>.
        /// </summary>
        /// <typeparam name="T">The Type of <see cref="Enum"/>.</typeparam>
        /// <param name="writer">The <see cref="IValueWriter"/> to write the value to.</param>
        /// <param name="enumWriter">The enum value writer.</param>
        /// <param name="name">Unique name of the <paramref name="value"/> that will be used to distinguish it
        /// from other values when reading.</param>
        /// <param name="value">The enum to write.</param>
        public static void WriteEnum<T>(IValueWriter writer, IEnumValueWriter<T> enumWriter, string name, T value)
            where T : struct, IComparable, IConvertible, IFormattable
        {
            if (writer.UseEnumNames)
                writer.WriteEnumName(name, value);
            else
                writer.WriteEnumValue(enumWriter, name, value);
        }
    }
}